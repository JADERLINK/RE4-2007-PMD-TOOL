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
    public static class OutputIdxPmd
    {
        public static void IdxPmdCreate(PMD pmd, string baseDiretory, string baseFileName, ConfigFile configFile)
        {
            StreamWriter idx = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".idxpmd")).CreateText();
            idx.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("# RE4_2007_PMD_TOOL By JADERLINK");
            idx.WriteLine($"# Version {Program.VERSION}");
            idx.WriteLine("");


            idx.WriteLine("CompressVertices:" + true.ToString());
            idx.WriteLine("IsScenarioPmd:" + (pmd.ObjRefBones.Count == 0));
            idx.WriteLine("ObjFileUseBone:0");
            idx.WriteLine("LoadColorsFromObjFile:" + configFile.UseColorsInObjFile);
            idx.WriteLine("UseMtlFile:" + !configFile.ReplaceMaterialNameByTextureName);
            idx.WriteLine("UseIdxPmdMaterial:" + configFile.EnableUseIdxPmdMaterial);


            idx.WriteLine();
            idx.WriteLine();
            idx.WriteLine("UseCustomGroups:" + false.ToString());
            idx.WriteLine("GroupsCount:" + pmd.Nodes.Length.ToString());
            for (int i = 0; i < pmd.Nodes.Length; i++)
            {
                idx.Write("Group_" + i + ":" + pmd.NodeGroupNames[i] + "?" + pmd.Nodes[i].SkeletonIndex + "?");

                for (int im = 0; im < pmd.Nodes[i].Meshs.Length; im++)
                {
                    idx.Write(CONSTS.PMD_MATERIAL_ + pmd.Nodes[i].Meshs[im].TextureIndex.ToString("D3"));
                    if (im < pmd.Nodes[i].Meshs.Length - 1)
                    {
                        idx.Write("?");
                    }
                }
                idx.Write("\r\n");
            }


            idx.Close();
        }




    }
}
