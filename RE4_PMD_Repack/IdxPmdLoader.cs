using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_2007_PMD_REPACK
{
    public static class IdxPmdLoader
    {
        public static IdxPmd Loader(StreamReader idxFile)
        {
            IdxPmd idx = new IdxPmd();

            List<PmdNodeGroup> NodeGroups = new List<PmdNodeGroup>();  
            Dictionary<string, string> pair = new Dictionary<string, string>();

            int GroupsCount = 0;
            int ObjFileUseBone = 0;

            string line = "";
            while (line != null)
            {
                line = idxFile.ReadLine();
                if (line != null && line.Length != 0)
                {
                    var split = line.Trim().Split(new char[] { ':' });

                    if (line.TrimStart().StartsWith(":") || line.TrimStart().StartsWith("#") || line.TrimStart().StartsWith("/") || line.TrimStart().StartsWith("\\"))
                    {
                        continue;
                    }
                    else if (split.Length >= 2)
                    {
                        string key = split[0].ToUpper().Trim();
                        if (!pair.ContainsKey(key))
                        {
                            pair.Add(key, split[1]);
                        }


                    }

                }
            }
            idxFile.Close();

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

            if (pair.ContainsKey("USECUSTOMGROUPS"))
            {
                try
                {
                    idx.UseCustomGroups = bool.Parse(pair["USECUSTOMGROUPS"].Trim());
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

            if (pair.ContainsKey("USEIDXPMDMATERIAL"))
            {
                try
                {
                    idx.UseIdxPmdMaterial = bool.Parse(pair["USEIDXPMDMATERIAL"].Trim());
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

            if (idx.UseCustomGroups)
            {

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

            }

            idx.ObjFileUseBone = ObjFileUseBone;
            idx.NodeGroups = NodeGroups.ToArray();

            return idx;
        }

    }

    public class IdxPmd 
    {
        public bool CompressVertices { get; set; }
        public bool IsScenarioPmd { get; set; }
        public int ObjFileUseBone { get; set; }
        public bool LoadColorsFromObjFile { get; set; }
        public bool UseMtlFile { get; set; }
        public bool UseIdxPmdMaterial { get; set; }
        public bool UseCustomGroups { get; set; }
        public PmdNodeGroup[] NodeGroups { get; set; }
    }

    public class PmdNodeGroup 
    {
        public string GroupName { get; set; }
        public int SkeletonIndex { get; set; }
        public string[] MaterialList { get; set; }
    }


}
