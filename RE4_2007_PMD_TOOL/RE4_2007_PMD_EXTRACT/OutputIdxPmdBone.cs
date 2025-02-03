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
    public class OutputIdxPmdBone
    {
        public static void IdxPmdBoneCreate(PMD pmd, string baseDiretory, string baseFileName)
        {
            StreamWriter idx = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".idxpmdbone")).CreateText();
            idx.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("# RE4_2007_PMD_TOOL By JADERLINK");
            idx.WriteLine($"# Version {Program.VERSION}");
            idx.WriteLine("");
            idx.WriteLine("");

            for (int i = 0; i < pmd.SkeletonBoneParents.Length; i++)
            {
                idx.WriteLine("UseBoneID:" + i.ToString());
                idx.WriteLine("BoneName:" + pmd.SkeletonBoneNames[i].Replace(" ", "_").Replace(":", "_"));
                idx.WriteLine("BoneParent:" + pmd.SkeletonBoneParents[i].ToString());

                FloatPrint(ref idx, "B00_scaleX", pmd.SkeletonBoneData[i][00]);
                FloatPrint(ref idx, "B01_scaleY", pmd.SkeletonBoneData[i][01]);
                FloatPrint(ref idx, "B02_scaleZ", pmd.SkeletonBoneData[i][02]);
                FloatPrint(ref idx, "B03_unknown", pmd.SkeletonBoneData[i][03]);
                FloatPrint(ref idx, "B04_unknown", pmd.SkeletonBoneData[i][04]);
                FloatPrint(ref idx, "B05_unknown", pmd.SkeletonBoneData[i][05]);
                FloatPrint(ref idx, "B06_unknown", pmd.SkeletonBoneData[i][06]);
                FloatPrint(ref idx, "B07_positionX", pmd.SkeletonBoneData[i][07]);
                FloatPrint(ref idx, "B08_positionY", pmd.SkeletonBoneData[i][08]);
                FloatPrint(ref idx, "B09_positionZ", pmd.SkeletonBoneData[i][09]);
                FloatPrint(ref idx, "B10_unknown", pmd.SkeletonBoneData[i][10]);
                FloatPrint(ref idx, "B11_unknown", pmd.SkeletonBoneData[i][11]);
                FloatPrint(ref idx, "B12_unknown", pmd.SkeletonBoneData[i][12]);
                FloatPrint(ref idx, "B13_unknown", pmd.SkeletonBoneData[i][13]);
                FloatPrint(ref idx, "B14_unknown", pmd.SkeletonBoneData[i][14]);
                FloatPrint(ref idx, "B15_unknown", pmd.SkeletonBoneData[i][15]);
                FloatPrint(ref idx, "B16_unknown", pmd.SkeletonBoneData[i][16]);
                FloatPrint(ref idx, "B17_unknown", pmd.SkeletonBoneData[i][17]);
                FloatPrint(ref idx, "B18_unknown", pmd.SkeletonBoneData[i][18]);
                FloatPrint(ref idx, "B19_unknown", pmd.SkeletonBoneData[i][19]);
                FloatPrint(ref idx, "B20_unknown", pmd.SkeletonBoneData[i][20]);
                FloatPrint(ref idx, "B21_unknown", pmd.SkeletonBoneData[i][21]);
                FloatPrint(ref idx, "B22_positionX", pmd.SkeletonBoneData[i][22]);
                FloatPrint(ref idx, "B23_positionY", pmd.SkeletonBoneData[i][23]);
                FloatPrint(ref idx, "B24_positionZ", pmd.SkeletonBoneData[i][24]);
                FloatPrint(ref idx, "B25_unknown", pmd.SkeletonBoneData[i][25]);

                idx.WriteLine("");
                idx.WriteLine("");
            }

            idx.Close();
        }

        private static void FloatPrint(ref StreamWriter text, string key, float value)
        {
            text.WriteLine(key + ":" + value.ToString("F9", CultureInfo.InvariantCulture));
        }
    }
}
