using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PMD_API;

namespace RE4_PMD_Decoder
{
    public static class Program
    {
        public const string VERSION = "B.1.0.0.0";

        static void Main(string[] args)
        {
            Console.WriteLine("## RE4_PMD_Decoder ##");
            Console.WriteLine($"## Version {VERSION} ##");
            Console.WriteLine("## By JADERLINK ##");
            Console.WriteLine("## Created from Re4 PMD to SMD model exporter by magnum29 (perl) ##");

            if (args.Length >= 1 && File.Exists(args[0]) && new FileInfo(args[0]).Extension.ToUpper() == ".PMD")
            {
                FileInfo fileInfo = new FileInfo(args[0]);
                string baseDiretory = fileInfo.DirectoryName + "\\";

                ConfigFile configFile = new ConfigFile();

                string configFilePath = null;
                string configFileName = "RE4_PMD_Decoder.txt";

                if (File.Exists(baseDiretory + configFileName))
                {
                    configFilePath = baseDiretory + configFileName;
                }
                else if (File.Exists(AppContext.BaseDirectory + configFileName))
                {
                    configFilePath = AppContext.BaseDirectory + configFileName;
                }

                if (configFilePath != null)
                {
                    try
                    {
                        configFile = ConfigFile.LoadConfigFile(configFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading file \"{configFileName}\": " + ex);
                    }
                }

                Console.WriteLine("EnableDebugFiles: " + configFile.EnableDebugFiles);
                Console.WriteLine("ReplaceMaterialNameByTextureName: " + configFile.ReplaceMaterialNameByTextureName);
                Console.WriteLine("UseColorsInObjFile: " + configFile.UseColorsInObjFile);

                Console.WriteLine(args[0]);
                try
                {
                  
                    string baseName = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length, fileInfo.Extension.Length);
                   
                    PMD pmd = PmdDecoder.GetPMD(args[0]);

                    OutputFiles.IdxPmdCreate(pmd, baseDiretory, baseName, configFile);
                    OutputFiles.SmdCreate(pmd, baseDiretory, baseName, configFile);
                    OutputFiles.ObjCreate(pmd, baseDiretory, baseName, configFile);
                    OutputFiles.MtlCreate(pmd, baseDiretory, baseName, configFile);

                    if (configFile.EnableDebugFiles)
                    {
                        OutputFiles.DebugInformation(pmd, fileInfo.FullName);
                    }
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }

            }
            else
            {
                Console.WriteLine("No arguments or invalid file");
            }

            Console.WriteLine("End");

        }
    }
}
