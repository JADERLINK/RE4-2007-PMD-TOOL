using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PMD_API;

namespace RE4_2007_PMD_EXTRACT
{
    public static class OutputDebug
    {
        public static void DebugInformation(PMD pmd, string File)
        {
            StreamWriter arq = new StreamWriter(File + ".info.txt2", false);

            arq.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            arq.WriteLine("# youtube.com/@JADERLINK");
            arq.WriteLine("# RE4_2007_PMD_TOOL By JADERLINK");
            arq.WriteLine($"# Version {Program.VERSION}");
            arq.WriteLine("");

            arq.WriteLine(File);

            arq.WriteLine();
            arq.WriteLine("NodeGroupNames:");
            for (int i = 0; i < pmd.NodeGroupNames.Length; i++)
            {
                arq.WriteLine(i + ": " + pmd.NodeGroupNames[i]);
            }

            arq.WriteLine();
            arq.WriteLine("TextureNames:");
            for (int i = 0; i < pmd.Materials.Length; i++)
            {
                arq.WriteLine(i + ": " + pmd.Materials[i].TextureName);
            }

            arq.WriteLine();
            arq.WriteLine("TextureData:");
            for (int i = 0; i < pmd.Materials.Length; i++)
            {
                arq.Write((i + ":").PadLeft(6));
                for (int it = 0; it < pmd.Materials[i].TextureData.Length; it++)
                {
                    arq.Write(pmd.Materials[i].TextureData[it].ToString("F6").PadLeft(10) + " ");
                }
                arq.Write(pmd.Materials[i].TextureEnable.ToString("D").PadLeft(10) + " ");
                arq.Write("\r\n");
            }

            arq.WriteLine();
            arq.WriteLine("SkeletonBoneNames:");

            for (int i = 0; i < pmd.SkeletonBoneNames.Length; i++)
            {
                arq.WriteLine(i + ": " + pmd.SkeletonBoneNames[i]);
            }

            arq.WriteLine();
            arq.WriteLine("SkeletonBoneParents:");
            for (int i = 0; i < pmd.SkeletonBoneParents.Length; i++)
            {
                arq.Write(i + ": " + pmd.SkeletonBoneParents[i] + "\r\n");
            }


            arq.Write("\r\nSkeletonBoneData:\r\n");
            arq.Write("id".PadLeft(12));
            for (int i = 0; i < 26; i++)
            {
                arq.Write((i).ToString().PadLeft(12));
                if (i == 13)
                {
                    arq.Write("\r\n");
                }
            }
            arq.Write("\r\n");

            for (int i = 0; i < pmd.SkeletonBoneData.Length; i++)
            {
                arq.Write((i).ToString().PadLeft(12));
                for (int iS = 0; iS < pmd.SkeletonBoneData[i].Length; iS++)
                {
                    arq.Write(pmd.SkeletonBoneData[i][iS].ToString("F6").PadLeft(12));
                    if (iS == 13)
                    {
                        arq.Write("\r\n");
                    }
                }
                arq.Write("\r\n");
            }

            if (pmd.ObjRefBones.Count != 0)
            {
                var keys = pmd.ObjRefBones.Keys.ToArray();
                arq.WriteLine();
                arq.WriteLine("ObjBones:");
                for (int i = 0; i < keys.Length; i++)
                {
                    arq.WriteLine(keys[i] + ": " + pmd.ObjRefBones[keys[i]]);
                }
            }

            arq.WriteLine();
            arq.WriteLine("Nodes:");
            for (int i = 0; i < pmd.Nodes.Length; i++)
            {
                arq.WriteLine();
                arq.WriteLine("Node[" + i + "]:");
                arq.WriteLine("ObjId:" + pmd.Nodes[i].ObjNodeId);
                arq.WriteLine("SkeletonIndex:" + pmd.Nodes[i].SkeletonIndex);


                if (pmd.Nodes[i].Bones.Length != 0)
                {
                    arq.WriteLine();
                    arq.WriteLine("Bones:");
                    for (int ib = 0; ib < pmd.Nodes[i].Bones.Length; ib++)
                    {
                        arq.Write((i + "_" + (ib * 3) + ": ").PadLeft(10) + " ");

                        arq.Write(("[" + pmd.Nodes[i].Bones[ib].boneId + "]").PadLeft(5) + " ");

                        for (int iu = 0; iu < pmd.Nodes[i].Bones[ib].unknown.Length; iu++)
                        {
                            arq.Write(pmd.Nodes[i].Bones[ib].unknown[iu].ToString("F6").PadLeft(10) + " ");
                        }
                        arq.Write(
                             (pmd.Nodes[i].Bones[ib].x.ToString("F6").PadLeft(10)) + " "
                           + (pmd.Nodes[i].Bones[ib].y.ToString("F6").PadLeft(10)) + " "
                           + (pmd.Nodes[i].Bones[ib].z.ToString("F6").PadLeft(10)) + " "
                           + (pmd.Nodes[i].Bones[ib].w.ToString("F6").PadLeft(10))
                           + "\r\n");
                    }
                }
                arq.WriteLine();

                for (int im = 0; im < pmd.Nodes[i].Meshs.Length; im++)
                {
                    arq.WriteLine("Meshs[" + im + "]:");
                    arq.WriteLine("TextureIndex:" + pmd.Nodes[i].Meshs[im].TextureIndex);
                    arq.Write("Orders [ ");
                    for (int io = 0; io < pmd.Nodes[i].Meshs[im].Orders.Length; io++)
                    {
                        arq.Write(pmd.Nodes[i].Meshs[im].Orders[io]);
                        if (io != pmd.Nodes[i].Meshs[im].Orders.Length - 1)
                        {
                            arq.Write(", ");
                        }
                    }
                    arq.Write(" ]\r\n");
                    arq.Write("Vertexs: \r\n" +
                        "Id".PadLeft(10) + " " +
                        "x".PadLeft(10) + " " +
                        "y".PadLeft(10) + " " +
                        "z".PadLeft(10) + " " +
                        "w0".PadLeft(10) + " " +
                        "w1".PadLeft(10) + " " +
                        "i0".PadLeft(10) + " " +
                        "i1".PadLeft(10) + " " +
                        "nx".PadLeft(10) + " " +
                        "ny".PadLeft(10) + " " +
                        "nz".PadLeft(10) + " " +
                        "tu".PadLeft(10) + " " +
                        "tv".PadLeft(10) + " " +
                        "r".PadLeft(10) + " " +
                        "g".PadLeft(10) + " " +
                        "b".PadLeft(10) + " " +
                        "a".PadLeft(10) + "\r\n");
                    for (int iv = 0; iv < pmd.Nodes[i].Meshs[im].Vertexs.Length; iv++)
                    {
                        arq.Write((iv + "").PadLeft(10) +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].x.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].y.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].z.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].w0.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].w1.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].i0.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].i1.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].nx.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].ny.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].nz.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].tu.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].tv.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].r.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].g.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].b.ToString("F6").PadLeft(10)) + " " +
                            (pmd.Nodes[i].Meshs[im].Vertexs[iv].a.ToString("F6").PadLeft(10)) + "\r\n");
                    }

                }

                arq.WriteLine();
            }

            arq.Close();

        }

    }
}
