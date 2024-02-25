using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RE4_2007_PMD_REPACK
{
    public static class PMDrepackIntermediary
    {
        public static IntermediaryStructure MakeIntermediaryStructure(StartStructure startStructure)
        {
            IntermediaryStructure intermediary = new IntermediaryStructure();

            foreach (var item in startStructure.FacesByMaterial)
            {
                IntermediaryMesh mesh = new IntermediaryMesh();

                for (int i = 0; i < item.Value.Faces.Count; i++)
                {
                    IntermediaryFace face = new IntermediaryFace();

                    for (int iv = 0; iv < item.Value.Faces[i].Count; iv++)
                    {
                        IntermediaryVertex vertex = new IntermediaryVertex();

                        vertex.PosX = item.Value.Faces[i][iv].Position.X;
                        vertex.PosY = item.Value.Faces[i][iv].Position.Y;
                        vertex.PosZ = item.Value.Faces[i][iv].Position.Z;

                        vertex.NormalX = item.Value.Faces[i][iv].Normal.X;
                        vertex.NormalY = item.Value.Faces[i][iv].Normal.Y;
                        vertex.NormalZ = item.Value.Faces[i][iv].Normal.Z;

                        vertex.TextureU = item.Value.Faces[i][iv].Texture.U;
                        vertex.TextureV = item.Value.Faces[i][iv].Texture.V;

                        vertex.ColorR = item.Value.Faces[i][iv].Color.R;
                        vertex.ColorG = item.Value.Faces[i][iv].Color.G;
                        vertex.ColorB = item.Value.Faces[i][iv].Color.B;
                        vertex.ColorA = item.Value.Faces[i][iv].Color.A;

                        vertex.BoneID0 = item.Value.Faces[i][iv].WeightMap.BoneID1;
                        vertex.BoneID1 = item.Value.Faces[i][iv].WeightMap.BoneID2;

                        vertex.Weight0 = item.Value.Faces[i][iv].WeightMap.Weight1;
                        vertex.Weight1 = item.Value.Faces[i][iv].WeightMap.Weight2;

                        face.Vertexs.Add(vertex);
                    }

                    mesh.Faces.Add(face);
                }

                mesh.MaterialName = item.Key.ToUpperInvariant();
                intermediary.Meshs.Add(mesh.MaterialName, mesh);
            }

            return intermediary;
        }

    }
}
