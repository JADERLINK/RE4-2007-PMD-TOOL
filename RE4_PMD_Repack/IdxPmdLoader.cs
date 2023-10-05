using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PMD_Repack
{
    public static class IdxPmdLoader
    {
        public static IdxPmd Loader(StreamReader idxFile)
        {
            IdxPmd idx = new IdxPmd();

            List<PmdNodeGroup> NodeGroups = new List<PmdNodeGroup>();
            List<PmdBoneLine> BoneLines = new List<PmdBoneLine>();
            Dictionary<string, PmdMaterialLine> MaterialLines = new Dictionary<string, PmdMaterialLine>();

            Dictionary<string, string> pair = new Dictionary<string, string>();

            int BonesCount = 0;
            int GroupsCount = 0;
            int ObjFileUseBone = 0;

            string line = "";
            while (line != null)
            {
                line = idxFile.ReadLine();
                if (line != null && line.Length != 0)
                {
                    var split = line.Trim().Split(new char[] { ':' });

                    if (line.TrimStart().StartsWith(":") || line.TrimStart().StartsWith("#") || line.TrimStart().StartsWith("/"))
                    {
                        continue;
                    }
                    else if (split.Length >= 2)
                    {

                        string key = split[0].ToUpper().Trim();

                        if (key.StartsWith("MATERIALLINE"))
                        {
                            var subSplitKey = key.Split(new char[] { '?' });
                            if (subSplitKey.Length >= 3)
                            {
                                string materialKey = subSplitKey[1].ToUpperInvariant().Trim();
                                string textureName = subSplitKey[2].Trim();
                                float[] textureData = new float[17];
                                int textureUnknown = 1;


                                var splitValue = split[1].Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                                for (int i = 0; i < splitValue.Length || i < textureData.Length; i++)
                                {
                                    try
                                    {
                                        textureData[i] = float.Parse(Utils.ReturnValidFloatValue(splitValue[i]), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }

                                if (splitValue.Length >= 18)
                                {
                                    try
                                    {
                                        string value = Utils.ReturnValidDecWithNegativeValue(pair[splitValue[17]]);
                                        textureUnknown = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                var material = new PmdMaterialLine();
                                material.TextureName = textureName;
                                material.TextureUnknown = textureUnknown;
                                material.TextureData = textureData;

                                MaterialLines.Add(materialKey, material);
                            }

                        }
                        else if (!pair.ContainsKey(key))
                        {
                            pair.Add(key, split[1]);
                        }


                    }

                }
            }

            //----

            if (pair.ContainsKey("ISSCENARIOPMD"))
            {
                try
                {
                    idx.IsScenarioPmd = bool.Parse(pair["ISSCENARIOPMD"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("COMPRESSVERTICES"))
            {
                try
                {
                    idx.CompressVertices = bool.Parse(pair["COMPRESSVERTICES"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("BONESCOUNT"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["BONESCOUNT"]);
                    BonesCount = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("GROUPSCOUNT"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["GROUPSCOUNT"]);
                    GroupsCount = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("LOADCOLORSFROMOBJFILE"))
            {
                try
                {
                    idx.LoadColorsFromObjFile = bool.Parse(pair["LOADCOLORSFROMOBJFILE"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("USEMTLFILE"))
            {
                try
                {
                    idx.UseMtlFile = bool.Parse(pair["USEMTLFILE"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("USEMATERIALLINES"))
            {
                try
                {
                    idx.UseMaterialLines = bool.Parse(pair["USEMATERIALLINES"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("OBJFILEUSEBONE"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["OBJFILEUSEBONE"]);
                    ObjFileUseBone = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }


            //---

            for (int i = 0; i < BonesCount; i++)
            {
                string name = "Null";
                int parent = -1;
                float[] data = new float[26];

                string keyName = "BONELINE_" + i.ToString() + "_NOME";
                string keyParent = "BONELINE_" + i.ToString() + "_PARENT";
                string keyData = "BONELINE_" + i.ToString() + "_DATA";

                if (pair.ContainsKey(keyName))
                {
                    name = pair[keyName].Trim();
                }

                if (pair.ContainsKey(keyParent))
                {
                    try
                    {
                        string value = Utils.ReturnValidDecWithNegativeValue(pair[keyParent]);
                        parent = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (pair.ContainsKey(keyData))
                {
                   var split = pair[keyData].Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    for (int il = 0; il < split.Length || il < data.Length; il++)
                    {
                        try
                        {
                            data[il] = float.Parse(Utils.ReturnValidFloatValue(split[il]), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                        }
                       
                    }
                }

                var bone = new PmdBoneLine(i, parent, name);
                data.CopyTo(bone.Values, 0);
                BoneLines.Add(bone);
            }

            // ----

            for (int i = 0; i < GroupsCount; i++)
            {
                string groupName = "Null";
                int skeletonIndex = 0;
                List<string> materialList = new List<string>();

                string key = "GROUP_" + i.ToString();

                if (pair.ContainsKey(key))
                {
                    var split = pair[key].Trim().Split('?');

                    if (split.Length >= 1)
                    {
                        groupName = split[0].Trim();
                    }

                    if (split.Length >= 2)
                    {
                        try
                        {
                            string value = Utils.ReturnValidDecWithNegativeValue(split[1]);
                            skeletonIndex = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (split.Length >= 3)
                    {
                        for (int im = 2; im < split.Length; im++)
                        {
                            materialList.Add(split[im].Trim().ToUpperInvariant());
                        }

                    }
                }

                var group = new PmdNodeGroup();
                group.GroupName = groupName;
                group.SkeletonIndex = skeletonIndex;
                group.MaterialList = materialList.ToArray();

                NodeGroups.Add(group);
            }


            //---
            idxFile.Close();

            idx.ObjFileUseBone = ObjFileUseBone;
            idx.NodeGroups = NodeGroups.ToArray();
            idx.BoneLines = BoneLines.ToArray();
            idx.MaterialLines = MaterialLines;

            return idx;
        }

    }

    public class IdxPmd 
    {
        public bool CompressVertices { get; set; }
    
        public bool IsScenarioPmd { get; set; }

        public int ObjFileUseBone { get; set; }

        public PmdBoneLine[] BoneLines { get; set; }

        public PmdNodeGroup[] NodeGroups { get; set; }

        public Dictionary<string, PmdMaterialLine> MaterialLines { get; set; }

        public bool LoadColorsFromObjFile { get; set; }
        public bool UseMtlFile { get; set; }
        public bool UseMaterialLines { get; set; }

    }

  
    public class PmdBoneLine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public float[] Values { get; private set; }

        public PmdBoneLine(int id, int parent, string name) 
        {
            ID = id;
            Name = name;
            Parent = parent;
            Values = new float[26];
        }
    }

    public class PmdNodeGroup 
    {
        public string GroupName { get; set; }
        public int SkeletonIndex { get; set; }
        public string[] MaterialList { get; set; }
    }


    public class PmdMaterialLine 
    {
        public string TextureName { get; set; }
        public float[] TextureData { get; set; }
        public int TextureUnknown { get; set; }
    }



}
