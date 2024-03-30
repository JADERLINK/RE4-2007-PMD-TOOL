using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PMD_API;
using System.Globalization;

namespace RE4_2007_PMD_EXTRACT
{
    public static class OutputFiles
    {
        public static void SmdCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {
            StreamWriter SMD = new StreamWriter(Path.Combine(baseDiretory, baseFileName + ".smd"), false);

            SMD.WriteLine("version 1");
            SMD.WriteLine("nodes");
      
            for (int i = 0; i < pmd.SkeletonBoneParents.Length; i++)
            {
                SMD.WriteLine(i.ToString() + " \"" + pmd.SkeletonBoneNames[i].Replace(" ", "_") + "\" " + pmd.SkeletonBoneParents[i].ToString());
            }

            SMD.WriteLine("end");
            SMD.WriteLine("skeleton");
            SMD.WriteLine("time 0");


            for (int i = 0; i < pmd.SkeletonBoneData.Length; i++)
            {
                SMD.WriteLine(i.ToString() + "  " +
                     pmd.SkeletonBoneData[i][7].ToFloatString() + " " +
                   (pmd.SkeletonBoneData[i][9] * -1).ToFloatString() + " " +
                     pmd.SkeletonBoneData[i][8].ToFloatString() + "  0.0 0.0 0.0");

            }

            SMD.WriteLine("end");
            SMD.WriteLine("triangles");


            for (int g = 0; g < pmd.Nodes.Length; g++)
            {
                int New = 0;

                for (int im = 0; im < pmd.Nodes[g].Meshs.Length; im++)
                {
                    for (int iv = 0; iv < pmd.Nodes[g].Meshs[im].Orders.Length; iv++)
                    {

                        int links;

                        if (New == 0)
                        {
                            if (configFile.ReplaceMaterialNameByTextureName)
                            {

                                string TextureName = pmd.Materials[pmd.Nodes[g].Meshs[im].TextureIndex].TextureName.ToLowerInvariant();
                                if (TextureName == null || TextureName.Length == 0)
                                {
                                    TextureName = CONSTS.PMD_MATERIAL_NULL;
                                }
                                SMD.WriteLine(TextureName);
                            }
                            else 
                            {
                                SMD.WriteLine(CONSTS.PMD_MATERIAL_ + pmd.Nodes[g].Meshs[im].TextureIndex.ToString("D3"));
                            }   
                            
                        }
                        New = (New + 1) % 3;


                        if (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].w0 == 1)
                        {
                            links = 1;
                        }
                        else
                        {
                            links = 2;
                        }


                        string Fixed_x = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].x).ToFloatString();
                        string Fixed_y = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].y).ToFloatString();
                        string Fixed_z = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].z * -1).ToFloatString();

                        string Fixed_nx = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].nx.ToFloatString();
                        string Fixed_ny = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].ny.ToFloatString();
                        string Fixed_nz = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].nz * -1).ToFloatString();

                        string Fixed_tu = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].tu.ToFloatString();
                        string Fixed_tv = ((pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].tv - 1) * -1).ToFloatString();

                        string res = pmd.Nodes[g].SkeletonIndex.ToString() + " " +
                            Fixed_x + " " +
                            Fixed_z + " " +
                            Fixed_y + " " +
                            Fixed_nx + " " +
                            Fixed_nz + " " +
                            Fixed_ny + " " +
                            Fixed_tu + " " +
                            Fixed_tv + " " +
                            links.ToString() + " ";

                        string keyId1 = pmd.Nodes[g].ObjNodeId + "_" + (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].i0);
                        if (pmd.ObjRefBones.ContainsKey(keyId1))
                        {
                            res += pmd.ObjRefBones[keyId1].ToString() + " ";
                        }
                        else
                        {
                            res += 0.ToString() + " ";
                        }

                        res += pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].w0.ToFloatString();

                        if (links == 2)
                        {
                            string keyId2 = pmd.Nodes[g].ObjNodeId + "_" + (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].i1);
                            if (pmd.ObjRefBones.ContainsKey(keyId2))
                            {
                                res += " " + pmd.ObjRefBones[keyId2].ToString();
                            }
                            else
                            {
                                res += " " + 0.ToString();
                            }

                            res += " " + pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].w1.ToFloatString();
                        }

                        SMD.WriteLine(res);

                    }
                }
            }

            SMD.WriteLine("end");
            SMD.WriteLine("// github.com/JADERLINK/RE4-2007-PMD-TOOL");
            SMD.WriteLine("// RE4_2007_PMD_TOOL By JADERLINK");
            SMD.Write($"// Version {Program.VERSION}");
            SMD.Close();

        }

        public static void ObjCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {
            StreamWriter obj = new StreamWriter(Path.Combine(baseDiretory, baseFileName + ".obj"), false);
            obj.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            obj.WriteLine("# RE4_2007_PMD_TOOL By JADERLINK");
            obj.WriteLine($"# Version {Program.VERSION}");
            obj.WriteLine("");

            obj.WriteLine("mtllib " + baseFileName + ".mtl");


            uint indexCount = 0;

            for (int g = 0; g < pmd.Nodes.Length; g++)
            {
                obj.WriteLine("g " + pmd.NodeGroupNames[g].Replace(" ", "_"));

                for (int im = 0; im < pmd.Nodes[g].Meshs.Length; im++)
                {
                    if (pmd.Nodes[g].Meshs[im].Orders.Length != 0)
                    {
                        if (configFile.ReplaceMaterialNameByTextureName)
                        {
                            string TextureName = pmd.Materials[pmd.Nodes[g].Meshs[im].TextureIndex].TextureName.ToLowerInvariant();
                            if (TextureName == null || TextureName.Length == 0)
                            {
                                TextureName = CONSTS.PMD_MATERIAL_NULL;
                            }
                            obj.WriteLine("usemtl " + TextureName);
                        }
                        else
                        {
                            obj.WriteLine("usemtl " + CONSTS.PMD_MATERIAL_ + pmd.Nodes[g].Meshs[im].TextureIndex.ToString("D3"));
                        }
                        
                        for (int iv = 0; iv < pmd.Nodes[g].Meshs[im].Vertexs.Length; iv++)
                        {

                            string v = "v " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].x).ToFloatString()
                                      + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].y).ToFloatString()
                                      + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].z).ToFloatString();

                            if (configFile.UseColorsInObjFile)
                            {
                                v += " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].r).ToFloatString()
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].g).ToFloatString()
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].b).ToFloatString()
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].a).ToFloatString();
                            }
                            obj.WriteLine(v);

                            obj.WriteLine("vn " + pmd.Nodes[g].Meshs[im].Vertexs[iv].nx.ToFloatString()
                               + " " + pmd.Nodes[g].Meshs[im].Vertexs[iv].ny.ToFloatString()
                               + " " + pmd.Nodes[g].Meshs[im].Vertexs[iv].nz.ToFloatString()
                               );

                            obj.WriteLine("vt " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].tu).ToFloatString()
                              + " " + ((pmd.Nodes[g].Meshs[im].Vertexs[iv].tv - 1) * -1).ToFloatString()
                              );
                        }

                        for (int io = 0; io < pmd.Nodes[g].Meshs[im].Orders.Length; io += 3)
                        {
                            string a = (pmd.Nodes[g].Meshs[im].Orders[io] + indexCount + 1).ToString();
                            string b = (pmd.Nodes[g].Meshs[im].Orders[io + 1] + indexCount + 1).ToString();
                            string c = (pmd.Nodes[g].Meshs[im].Orders[io + 2] + indexCount + 1).ToString();

                            obj.WriteLine("f " + a + "/" + a + "/" + a + " "
                                + b + "/" + b + "/" + b + " "
                                + c + "/" + c + "/" + c);

                        }

                        indexCount += (uint)pmd.Nodes[g].Meshs[im].Vertexs.Length;
                    }
                }
            }

            obj.Close();
        }
  
    }
}
