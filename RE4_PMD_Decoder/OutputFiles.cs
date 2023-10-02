using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PMD_API;
using System.Globalization;

namespace RE4_PMD_Decoder
{
    public static class OutputFiles
    {
        private const string PMD_MATERIAL_NULL = "PMD_MATERIAL_NULL.tga";
        private const string PMD_MATERIAL_ = "PMD_MATERIAL_";

        public static void DebugInformation(PMD pmd, string File)
        {

            StreamWriter arq = new StreamWriter(File + ".info.txt2", false);

            arq.WriteLine("## RE4_PMD_Decoder ##");
            arq.WriteLine($"## Version {Program.VERSION} ##");

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
                arq.Write(pmd.Materials[i].TextureUnknown.ToString("D").PadLeft(10) + " ");
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


        public static void SmdCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            StreamWriter SMD = new StreamWriter(baseDiretory + baseFileName + ".smd", false);

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
                     pmd.SkeletonBoneData[i][7].ToString("F9", ci) + " " +
                   (pmd.SkeletonBoneData[i][9] * -1).ToString("F9", ci) + " " +
                     pmd.SkeletonBoneData[i][8].ToString("F9", ci) + "  0.000000000 0.000000000 0.000000000");

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

                                string TextureName = pmd.Materials[pmd.Nodes[g].Meshs[im].TextureIndex].TextureName;
                                if (TextureName == null || TextureName.Length == 0)
                                {
                                    TextureName = PMD_MATERIAL_NULL;
                                }
                                SMD.WriteLine(TextureName);
                            }
                            else 
                            {
                                SMD.WriteLine(PMD_MATERIAL_ + pmd.Nodes[g].Meshs[im].TextureIndex.ToString("D3"));
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


                        string Fixed_x = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].x).ToString("F9", ci);
                        string Fixed_y = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].y).ToString("F9", ci);
                        string Fixed_z = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].z * -1).ToString("F9", ci);

                        string Fixed_nx = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].nx.ToString("F9", ci);
                        string Fixed_ny = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].ny.ToString("F9", ci);
                        string Fixed_nz = (pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].nz * -1).ToString("F9", ci);

                        string Fixed_tu = pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].tu.ToString("F9", ci);
                        string Fixed_tv = ((pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].tv - 1) * -1).ToString("F9", ci);

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

                        res += pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].w0.ToString("F9", ci);

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

                            res += " " + pmd.Nodes[g].Meshs[im].Vertexs[pmd.Nodes[g].Meshs[im].Orders[iv]].w1.ToString("F9", ci);
                        }

                        SMD.WriteLine(res);

                    }
                }
            }

            SMD.WriteLine("end");
            SMD.WriteLine("//##RE4_PMD_Decoder##");
            SMD.WriteLine($"//##Version {Program.VERSION}##");
            SMD.Close();

        }


        public static void ObjCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {
            StreamWriter obj = new StreamWriter(baseDiretory + baseFileName + ".obj", false);
            obj.WriteLine("##RE4_PMD_Decoder##");
            obj.WriteLine($"##Version {Program.VERSION}##");
            obj.WriteLine("");

            obj.WriteLine("mtllib " + baseFileName + ".mtl");


            uint indexCount = 0;

            for (int g = 0; g < pmd.Nodes.Length; g++)
            {
              
                for (int im = 0; im < pmd.Nodes[g].Meshs.Length; im++)
                {
                    if (pmd.Nodes[g].Meshs[im].Orders.Length != 0)
                    {
                        obj.WriteLine("g " + pmd.NodeGroupNames[g] + "#" + PMD_MATERIAL_ + pmd.Nodes[g].Meshs[im].TextureIndex.ToString("D3"));

                        if (configFile.ReplaceMaterialNameByTextureName)
                        {
                            string TextureName = pmd.Materials[pmd.Nodes[g].Meshs[im].TextureIndex].TextureName;
                            if (TextureName == null || TextureName.Length == 0)
                            {
                                TextureName = PMD_MATERIAL_NULL;
                            }
                            obj.WriteLine("usemtl " + TextureName);
                        }
                        else
                        {
                            obj.WriteLine("usemtl " + PMD_MATERIAL_ + pmd.Nodes[g].Meshs[im].TextureIndex.ToString("D3"));
                        }
                        
                        for (int iv = 0; iv < pmd.Nodes[g].Meshs[im].Vertexs.Length; iv++)
                        {


                            string v = "v " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].x).ToString("f9", CultureInfo.InvariantCulture)
                                      + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].y).ToString("f9", CultureInfo.InvariantCulture)
                                      + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].z).ToString("f9", CultureInfo.InvariantCulture);

                            if (configFile.UseColorsInObjFile)
                            {
                                v += " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].r).ToString("f9", CultureInfo.InvariantCulture)
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].g).ToString("f9", CultureInfo.InvariantCulture)
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].b).ToString("f9", CultureInfo.InvariantCulture)
                                   + " " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].a).ToString("f9", CultureInfo.InvariantCulture);
                            }
                            obj.WriteLine(v);

                            obj.WriteLine("vn " + pmd.Nodes[g].Meshs[im].Vertexs[iv].nx.ToString("f9", CultureInfo.InvariantCulture)
                               + " " + pmd.Nodes[g].Meshs[im].Vertexs[iv].ny.ToString("f9", CultureInfo.InvariantCulture)
                               + " " + pmd.Nodes[g].Meshs[im].Vertexs[iv].nz.ToString("f9", CultureInfo.InvariantCulture)
                               );

                            obj.WriteLine("vt " + (pmd.Nodes[g].Meshs[im].Vertexs[iv].tu).ToString("f9", CultureInfo.InvariantCulture)
                              + " " + ((pmd.Nodes[g].Meshs[im].Vertexs[iv].tv - 1) * -1).ToString("f9", CultureInfo.InvariantCulture)
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

        public static void MtlCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {

            StreamWriter MTLtext = new FileInfo(baseDiretory + baseFileName + ".mtl").CreateText();
            MTLtext.WriteLine("##RE4_PMD_Decoder##");
            MTLtext.WriteLine($"##Version {Program.VERSION}##");
            MTLtext.WriteLine("");

            HashSet<string> materialNames = new HashSet<string>(pmd.Materials.Length);

            for (int i = 0; i < pmd.Materials.Length; i++)
            {
                string TextureName = pmd.Materials[i].TextureName;
                if (TextureName == null || TextureName.Length == 0)
                {
                    TextureName = PMD_MATERIAL_NULL;
                }

                string MaterialName = PMD_MATERIAL_ + i.ToString("D3");

                if (configFile.ReplaceMaterialNameByTextureName)
                {
                    MaterialName = TextureName;
                }

                if (!materialNames.Contains(MaterialName))
                {

                    MTLtext.WriteLine("");
                    MTLtext.WriteLine("newmtl " + MaterialName);
                    MTLtext.WriteLine("Ka 1.000 1.000 1.000");
                    MTLtext.WriteLine("Kd 1.000 1.000 1.000");
                    MTLtext.WriteLine("Ks 0.000 0.000 0.000");
                    MTLtext.WriteLine("Ns 0");
                    MTLtext.WriteLine("d 1");
                    MTLtext.WriteLine("Tr 1");
                    MTLtext.WriteLine("map_Kd " + TextureName);
                    MTLtext.WriteLine("");

                    materialNames.Add(MaterialName);
                }
            }
            MTLtext.Close();
        }

        public static void IdxPmdCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile) 
        {
            StreamWriter idx = new StreamWriter(baseDiretory + baseFileName + ".idxpmd", false);
            idx.WriteLine(":##RE4_PMD_Decoder##");
            idx.WriteLine($":##Version {Program.VERSION}##");
            idx.WriteLine("");


            idx.WriteLine("CompressVertices:True");
            idx.WriteLine("IsScenarioPmd:" + (pmd.ObjRefBones.Count == 0));


            idx.WriteLine();
            idx.WriteLine(": ## Bones ##");
            idx.WriteLine("ObjFileUseBone:0");
            idx.WriteLine(": BonesCount in decimal value");
            idx.WriteLine("BonesCount:" + pmd.SkeletonBoneParents.Length.ToString());
            idx.WriteLine(": BoneLines");
            for (int i = 0; i < pmd.SkeletonBoneParents.Length; i++)
            {
                idx.WriteLine("BoneLine_" + i + "_Nome:" + pmd.SkeletonBoneNames[i]);
                idx.WriteLine("BoneLine_" + i + "_Parent:" + pmd.SkeletonBoneParents[i]);
                idx.Write("BoneLine_" + i + "_Data:");
                for (int ii = 0; ii < pmd.SkeletonBoneData[i].Length; ii++)
                {
                    idx.Write(" " + pmd.SkeletonBoneData[i][ii].ToString("F9", CultureInfo.InvariantCulture));
                }
                idx.Write("\r\n");
            }


            idx.WriteLine();
            idx.WriteLine(": ## Groups ##");
            idx.WriteLine(": GroupsCount in decimal value");
            idx.WriteLine("GroupsCount:" + pmd.Nodes.Length.ToString());
            for (int i = 0; i < pmd.Nodes.Length; i++)
            {
                idx.Write("Group_" + i + ":" + pmd.NodeGroupNames[i] +"?" + pmd.Nodes[i].SkeletonIndex + "?");

                for (int im = 0; im < pmd.Nodes[i].Meshs.Length; im++)
                {
                    idx.Write(PMD_MATERIAL_ + pmd.Nodes[i].Meshs[im].TextureIndex.ToString("D3"));
                    if (im < pmd.Nodes[i].Meshs.Length -1)
                    {
                        idx.Write("?");
                    }
                }
                idx.Write("\r\n");
            }


            idx.WriteLine();
            idx.WriteLine("LoadColorsFromObjFile: " + configFile.UseColorsInObjFile);
            idx.WriteLine("UseMtlFile: " + !configFile.ReplaceMaterialNameByTextureName);
            idx.WriteLine("UseMaterialLines: " + false.ToString());

            idx.WriteLine();
            idx.WriteLine(": ## Materials ##");
            for (int i = 0; i < pmd.Materials.Length; i++)
            {
                string TextureName = pmd.Materials[i].TextureName;
                if (TextureName == null || TextureName.Length == 0)
                {
                    TextureName = PMD_MATERIAL_NULL;
                }

                idx.Write("MaterialLine?" + PMD_MATERIAL_ + i.ToString("D3") + "?" + TextureName + ": ");
                for (int f = 0; f < pmd.Materials[i].TextureData.Length; f++)
                {
                    idx.Write(pmd.Materials[i].TextureData[f].ToString("F9", CultureInfo.InvariantCulture) + " ");
                }
                idx.Write(pmd.Materials[i].TextureUnknown.ToString() + "\r\n");
            }

            idx.Close();
        }

    }
}
