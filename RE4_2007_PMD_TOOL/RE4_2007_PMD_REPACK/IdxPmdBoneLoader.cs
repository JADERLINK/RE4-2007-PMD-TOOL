using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace RE4_2007_PMD_REPACK
{
    public class PmdBoneLine
    {
        public int ID;
        public string Name;
        public int Parent;
        public float[] Values; //new float[26];

        public PmdBoneLine()
        {
            ID = -1;
            Name = "null";
            Parent = -1;
            Values = new float[26];
        }
    }

    public static class IdxPmdBoneLoader
    {
        public static PmdBoneLine[] Load(Stream stream)
        {
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);

            Dictionary<int, PmdBoneLine> BoneDic = new Dictionary<int, PmdBoneLine>();

            PmdBoneLine temp = new PmdBoneLine();

            int boneCount = 0;

            while (!reader.EndOfStream)
            {
                string nline = reader.ReadLine().Trim();
                string line = nline.ToUpperInvariant();

                if (line == null || line.Length == 0 || line.StartsWith("\\") || line.StartsWith("/") || line.StartsWith("#") || line.StartsWith(":"))
                {
                    continue;
                }
                else if (line.StartsWith("USEBONEID"))
                {
                    temp = new PmdBoneLine();

                    var split = line.Split(':');
                    if (split.Length >= 2)
                    {
                        int ID = -1;

                        try
                        {
                            ID = byte.Parse(Utils.ReturnValidDecValue(split[1]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                        }

                        if (ID > -1 && !BoneDic.ContainsKey(ID))
                        {
                            temp.ID = ID;
                            BoneDic.Add(ID, temp);

                            if (ID >= boneCount)
                            {
                                boneCount = ID + 1;
                            }
                        }
                    }
                }
                else if (line.StartsWith("BONENAME"))
                {
                    var split = nline.Split(':');
                    if (split.Length >= 2)
                    {
                        temp.Name = split[1].Trim().Replace(" ", "_").Replace(":", "_");
                    }
                }
                else if (line.StartsWith("BONEPARENT"))
                {
                    var split = line.Split(':');
                    if (split.Length >= 2)
                    {
                        try
                        {
                            temp.Parent = int.Parse(Utils.ReturnValidDecWithNegativeValue(split[1]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    _ = SetFloatDec(ref line, "B00_", ref temp.Values[00])
                     || SetFloatDec(ref line, "B01_", ref temp.Values[01])
                     || SetFloatDec(ref line, "B02_", ref temp.Values[02])
                     || SetFloatDec(ref line, "B03_", ref temp.Values[03])
                     || SetFloatDec(ref line, "B04_", ref temp.Values[04])
                     || SetFloatDec(ref line, "B05_", ref temp.Values[05])
                     || SetFloatDec(ref line, "B06_", ref temp.Values[06])
                     || SetFloatDec(ref line, "B07_", ref temp.Values[07])
                     || SetFloatDec(ref line, "B08_", ref temp.Values[08])
                     || SetFloatDec(ref line, "B09_", ref temp.Values[09])
                     || SetFloatDec(ref line, "B10_", ref temp.Values[10])
                     || SetFloatDec(ref line, "B11_", ref temp.Values[11])
                     || SetFloatDec(ref line, "B12_", ref temp.Values[12])
                     || SetFloatDec(ref line, "B13_", ref temp.Values[13])
                     || SetFloatDec(ref line, "B14_", ref temp.Values[14])
                     || SetFloatDec(ref line, "B15_", ref temp.Values[15])
                     || SetFloatDec(ref line, "B16_", ref temp.Values[16])
                     || SetFloatDec(ref line, "B17_", ref temp.Values[17])
                     || SetFloatDec(ref line, "B18_", ref temp.Values[18])
                     || SetFloatDec(ref line, "B19_", ref temp.Values[19])
                     || SetFloatDec(ref line, "B20_", ref temp.Values[20])
                     || SetFloatDec(ref line, "B21_", ref temp.Values[21])
                     || SetFloatDec(ref line, "B22_", ref temp.Values[22])
                     || SetFloatDec(ref line, "B23_", ref temp.Values[23])
                     || SetFloatDec(ref line, "B24_", ref temp.Values[24])
                     || SetFloatDec(ref line, "B25_", ref temp.Values[25])
                      ;

                }

            }

            stream.Close();

            PmdBoneLine[] res = new PmdBoneLine[boneCount];

            for (int i = 0; i < boneCount; i++)
            {
                if (!BoneDic.ContainsKey(i))
                {
                    PmdBoneLine ntemp = new PmdBoneLine();
                    ntemp.ID = i;
                    ntemp.Parent = -1;
                    ntemp.Name = "null";
                    res[i] = ntemp;
                }
                else 
                {
                    res[i] = BoneDic[i];
                }
            }

            return res;
        }

        private static bool SetFloatDec(ref string line, string key, ref float varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = float.Parse(Utils.ReturnValidFloatValue(split[1]), NumberStyles.Float, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

    }
}
