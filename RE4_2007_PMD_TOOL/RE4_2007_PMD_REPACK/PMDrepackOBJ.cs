using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RE4_2007_PMD_REPACK
{
    public static class PMDrepackOBJ
    {
        public static void RepackOBJ(IdxPmd idxPmd, string objPath,
            out IntermediaryStructure intermediaryStructure,
            out string[] ModelMaterialsArr)
        {
            // load .obj file
            var objLoaderFactory = new ObjLoader.Loader.Loaders.ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            StreamReader streamReader = null;
            ObjLoader.Loader.Loaders.LoadResult arqObj = null;

            try
            {
                streamReader = new StreamReader(objPath, Encoding.ASCII);
                arqObj = objLoader.Load(streamReader);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }

            //lista de materiais usados no modelo
            HashSet<string> ModelMaterials = new HashSet<string>();

            //--- crio a primeira estrutura:

            StartStructure startStructure = new StartStructure();

            Vector4 color = new Vector4(1, 1, 1, 1);
            StartWeightMap weightMap = new StartWeightMap(1, idxPmd.ObjFileUseBone, 1, 0, 0);

            for (int iG = 0; iG < arqObj.Groups.Count; iG++)
            {
                List<List<StartVertex>> facesList = new List<List<StartVertex>>();

                for (int iF = 0; iF < arqObj.Groups[iG].Faces.Count; iF++)
                {
                    List<StartVertex> verticeListInObjFace = new List<StartVertex>();

                    for (int iI = 0; iI < arqObj.Groups[iG].Faces[iF].Count; iI++)
                    {
                        StartVertex vertice = new StartVertex();

                        if (arqObj.Groups[iG].Faces[iF][iI].VertexIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1 >= arqObj.Vertices.Count)
                        {
                            throw new ArgumentException("Vertex Position Index is invalid! Value: " + arqObj.Groups[iG].Faces[iF][iI].VertexIndex);
                        }

                        Vector3 position = new Vector3(
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].X,
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].Y,
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].Z
                            );

                        vertice.Position = position;


                        if (arqObj.Groups[iG].Faces[iF][iI].TextureIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1 >= arqObj.Textures.Count)
                        {
                            vertice.Texture = new Vector2(0, 0);
                        }
                        else
                        {
                            Vector2 texture = new Vector2(
                            arqObj.Textures[arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1].U,
                            ((arqObj.Textures[arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1].V -1) * -1)
                            );

                            vertice.Texture = texture;
                        }


                        if (arqObj.Groups[iG].Faces[iF][iI].NormalIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1 >= arqObj.Normals.Count)
                        {
                            vertice.Normal = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            Vector3 normal = new Vector3(
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].X,
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Y,
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Z
                            );

                            vertice.Normal = normal;
                        }

                        vertice.Color = color;
                        vertice.WeightMap = weightMap;

                        if (idxPmd.LoadColorsFromObjFile)
                        {
                            Vector4 vColor = new Vector4(
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].R,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].G,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].B,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].A
                           );
                            vertice.Color = vColor;
                        }

                        verticeListInObjFace.Add(vertice);

                    }

                    if (verticeListInObjFace.Count >= 3)
                    {
                        for (int i = 2; i < verticeListInObjFace.Count; i++)
                        {
                            List<StartVertex> face = new List<StartVertex>();
                            face.Add(verticeListInObjFace[0]);
                            face.Add(verticeListInObjFace[i-1]);
                            face.Add(verticeListInObjFace[i]);
                            facesList.Add(face);
                        }        
                    }

                }

                string materialNameInvariant = arqObj.Groups[iG].MaterialName.ToUpperInvariant().Trim();
                string materialName = arqObj.Groups[iG].MaterialName.Trim();

                if (startStructure.FacesByMaterial.ContainsKey(materialNameInvariant))
                {
                    startStructure.FacesByMaterial[materialNameInvariant].Faces.AddRange(facesList);
                }
                else
                {
                    ModelMaterials.Add(materialName);
                    startStructure.FacesByMaterial.Add(materialNameInvariant, new StartFacesGroup(facesList));
                }

            }

            // faz a compressão das vertives
            if (idxPmd.CompressVertices == true)
            {
                startStructure.CompressAllFaces();
            }

            ModelMaterialsArr = ModelMaterials.ToArray();

            // estrutura intermediaria
            intermediaryStructure = PMDrepackIntermediary.MakeIntermediaryStructure(startStructure);
        }

        public static FinalBoneLine[] GetBoneLines(PmdBoneLine[] boneLines) 
        {
            FinalBoneLine[] finalBoneLine = new FinalBoneLine[boneLines.Length];

            for (int i = 0; i < boneLines.Length; i++)
            {
                FinalBoneLine boneLine = new FinalBoneLine(boneLines[i].ID, boneLines[i].Parent, boneLines[i].Name);
                boneLines[i].Values.CopyTo(boneLine.Values, 0);
                finalBoneLine[i] = boneLine;
            }

            return finalBoneLine;
        } 

    }
}
