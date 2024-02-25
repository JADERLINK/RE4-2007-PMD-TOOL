using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_2007_PMD_REPACK
{
    public static class PMDrepackSMD
    {
        public static void RepackSMD(IdxPmd idxPmd, string smdPath, 
            out IntermediaryStructure intermediaryStructure, 
            out string[] ModelMaterialsArr,
            out FinalBoneLine[] bones)
        {
            //carrega o arquivo smd;
            StreamReader stream = new StreamReader(new FileInfo(smdPath).OpenRead(), Encoding.ASCII);
            SMD_READER_API.SMD smd = SMD_READER_API.SmdReader.Reader(stream);

            //lista de materiais usados no modelo
            HashSet<string> ModelMaterials = new HashSet<string>();

            //--- crio a primeira estrutura:

            StartStructure startStructure = new StartStructure();

            Vector4 color = new Vector4(1, 1, 1, 1);

            for (int i = 0; i < smd.Triangles.Count; i++)
            {
                string materialNameInvariant = smd.Triangles[i].Material.ToUpperInvariant().Trim();
                string materialName = smd.Triangles[i].Material.Trim();

                List<StartVertex> verticeList = new List<StartVertex>();

                for (int t = 0; t < smd.Triangles[i].Vertexs.Count; t++)
                {
                    // cria o objeto com os indices.
                    StartVertex vertice = new StartVertex();
                    vertice.Color = color;


                    Vector3 position = new Vector3(
                            smd.Triangles[i].Vertexs[t].PosX,
                            smd.Triangles[i].Vertexs[t].PosZ,
                            smd.Triangles[i].Vertexs[t].PosY * -1
                            );

                    vertice.Position = position;

                    Vector3 normal = new Vector3(
                            smd.Triangles[i].Vertexs[t].NormX,
                            smd.Triangles[i].Vertexs[t].NormZ,
                            smd.Triangles[i].Vertexs[t].NormY * -1
                            );

                    vertice.Normal = normal;

                    Vector2 texture = new Vector2(
                    smd.Triangles[i].Vertexs[t].U,
                    ((smd.Triangles[i].Vertexs[t].V-1)*-1)
                    );

                    vertice.Texture = texture;

                    //cria o objetos com os weight
                    // e corrige a soma total para dar 1

                    if (smd.Triangles[i].Vertexs[t].Links.Count == 0)
                    {
                        StartWeightMap weightMap = new StartWeightMap();
                        weightMap.Links = 1;
                        weightMap.BoneID1 = smd.Triangles[i].Vertexs[t].ParentBone;
                        weightMap.Weight1 = 1f;

                        vertice.WeightMap = weightMap;
                    }
                    else
                    {
                        StartWeightMap weightMap = new StartWeightMap();

                        var links = (from link in smd.Triangles[i].Vertexs[t].Links
                                     orderby link.Weight
                                     select link).ToArray();

                        if (links.Length >= 1)
                        {
                            weightMap.Links = 1;
                            weightMap.BoneID1 = links[0].BoneID;
                            weightMap.Weight1 = links[0].Weight;
                        }
                        if (links.Length >= 2)
                        {
                            weightMap.Links = 2;
                            weightMap.BoneID2 = links[1].BoneID;
                            weightMap.Weight2 = links[1].Weight;
                        }

                        // verificação para soma total dar 1

                        float sum = weightMap.Weight1 + weightMap.Weight2;

                        if (sum > 1  // se por algum motivo aleatorio ficar maior que 1
                            || sum < 1) // ou se caso for menor que 1
                        {
                            float difference = sum - 1; // se for maior diferença é positiva, e se for menor é positiva
                            float average = difference / weightMap.Links; // aqui mantem o sinal da operação

                            if (weightMap.Links >= 1)
                            {
                                weightMap.Weight1 -= average; // se for positivo tem que dimiuir,
                                                              // porem se for negativo tem que aumentar,
                                                              // porem menos com menos da mais, então esta certo.
                            }
                            if (weightMap.Links >= 2)
                            {
                                weightMap.Weight2 -= average;
                            }
  
                            //re verifica se ainda tem diferença
                            float newSum = weightMap.Weight1 + weightMap.Weight2;
                            float newDifference = newSum - 1;

                            if (newDifference != 1)
                            {
                                weightMap.Weight1 -= newDifference;
                            }
                        }

                        vertice.WeightMap = weightMap;
                    }


                    verticeList.Add(vertice);
  
                }

                if (startStructure.FacesByMaterial.ContainsKey(materialNameInvariant))
                {
                    startStructure.FacesByMaterial[materialNameInvariant].Faces.Add(verticeList);
                }
                else // cria novo
                {
                    ModelMaterials.Add(materialName);

                    StartFacesGroup facesGroup = new StartFacesGroup();
                    facesGroup.Faces.Add(verticeList);
                    startStructure.FacesByMaterial.Add(materialNameInvariant, facesGroup);
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

            //FinalBoneLine é usado os bones do arquivo smd
            bones = GetBoneLines(smd);
        }

        private static FinalBoneLine[] GetBoneLines(SMD_READER_API.SMD smd)
        {
            int maxBone = 0;

            Dictionary<int, FinalBoneLine> bones = new Dictionary<int, FinalBoneLine>();

            SMD_READER_API.Time time = (from tt in smd.Times
                                        where tt.ID == 0
                                        select tt).FirstOrDefault();

            for (int i = 0; i < smd.Nodes.Count; i++)
            {
                int boneId = smd.Nodes[i].ID;

                if (maxBone < boneId)
                {
                    maxBone = boneId;
                }

                float[] values = new float[26];

                (float X, float Y, float Z) bonePos = (0, 0, 0);

                if (time != null)
                {
                    SMD_READER_API.Skeleton skeleton = (from ss in time.Skeletons
                                                        where ss.BoneID == boneId
                                                        select ss).FirstOrDefault();
                    if (skeleton != null)
                    {
                        bonePos.X = skeleton.PosX;
                        bonePos.Y = skeleton.PosZ;
                        bonePos.Z = skeleton.PosY * -1;
                    }
                }

                values[0] = 1f;
                values[1] = 1f;
                values[2] = 1f; 
                values[6] = 1f;
                values[7] = bonePos.X;
                values[8] = bonePos.Y;
                values[9] = bonePos.Z;
                values[10] = 1f;
                values[15] = 1f;
                values[20] = 1f;
                values[22] = bonePos.X;
                values[23] = bonePos.Y;
                values[24] = bonePos.Z;
                values[25] = 1f;
                FinalBoneLine boneLine = new FinalBoneLine(boneId, smd.Nodes[i].ParentID, smd.Nodes[i].BoneName);
                values.CopyTo(boneLine.Values, 0);

                if (!bones.ContainsKey(boneId))
                {
                    bones.Add(boneId, boneLine);
                }
               
            }

            if (bones.Count -1 != maxBone)
            {
                for (int i = 0; i < maxBone; i++)
                {
                    if (!bones.ContainsKey(i))
                    {
                        float[] values = new float[26];
                        values[0] = 1f;
                        values[1] = 1f;
                        values[2] = 1f;
                        values[6] = 1f;
                        values[10] = 1f;
                        values[15] = 1f;
                        values[20] = 1f;
                        values[25] = 1f;
                        FinalBoneLine boneLine = new FinalBoneLine(i, -1, "NullBone");
                        values.CopyTo(boneLine.Values, 0);
                        bones.Add(i, boneLine);
                    }


                }
            }

            var bonesArr = (from bb in bones
                     orderby bb.Key
                     select bb.Value).ToArray();

            return bonesArr;
        }
       
    }
}
