using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PMD_Decoder
{
    public class ConfigFile
    {
        public bool EnableDebugFiles = false;
        public bool ReplaceMaterialNameByTextureName = false;
        public bool UseColorsInObjFile = false;
        public static ConfigFile LoadConfigFile(string filepath) 
        {
            ConfigFile config = new ConfigFile();

            var arq = new FileInfo(filepath).OpenText();
            
            while (!arq.EndOfStream)
            {
                string line = arq.ReadLine().Trim();

                var split = line.Split(':');

                if (line.Length == 0 || line.StartsWith(":") || line.StartsWith("#") || line.StartsWith("/"))
                {
                    continue;
                }
                else if (split.Length >= 2)
                {
                    string key = split[0].ToUpperInvariant();

                    if (key == "ENABLEDEBUGFILES")
                    {
                        bool.TryParse(split[1], out config.EnableDebugFiles);
                    }
                    else if (key == "REPLACEMATERIALNAMEBYTEXTURENAME") 
                    {
                        bool.TryParse(split[1], out config.ReplaceMaterialNameByTextureName);
                    }

                    else if (key == "USECOLORSINOBJFILE")
                    {
                        bool.TryParse(split[1], out config.UseColorsInObjFile);
                    }
                }

            }

            return config;
        }

    }
}
