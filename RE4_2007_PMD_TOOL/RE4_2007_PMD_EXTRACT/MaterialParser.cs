using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMD_API;

namespace RE4_2007_PMD_EXTRACT
{
    public static class MaterialParser
    {
        public static Dictionary<string, PmdMaterialPart> Parser(PMD pmd, ConfigFile configFile)
        {
            Dictionary<string, PmdMaterialPart> MaterialDic = new Dictionary<string, PmdMaterialPart>();

            for (int i = 0; i < pmd.Materials.Length; i++)
            {
                string MaterialName = CONSTS.PMD_MATERIAL_ + i.ToString("D3");

                string TextureName = pmd.Materials[i].TextureName.ToLowerInvariant();
                if (TextureName == null || TextureName.Length == 0)
                {
                    TextureName = CONSTS.PMD_MATERIAL_NULL;
                }

                if (configFile.ReplaceMaterialNameByTextureName)
                {
                    MaterialName = TextureName;
                }

                PmdMaterialPart part = new PmdMaterialPart();
                part.MaterialName = MaterialName;
                part.TextureName = TextureName;
                part.TextureEnable = pmd.Materials[i].TextureEnable;
                part.TextureData = pmd.Materials[i].TextureData;

                if (!MaterialDic.ContainsKey(MaterialName))
                {
                    MaterialDic.Add(MaterialName, part);
                }
            }
            return MaterialDic;
        }

    }
}
