using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PMD_API;

namespace RE4_2007_PMD_EXTRACT
{
    public static class Program
    {
        public const string VERSION = "B.1.1.0 (2025-02-03)";

        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            Console.WriteLine("# youtube.com/@JADERLINK");
            Console.WriteLine("# RE4_2007_PMD_TOOL (EXTRACT) By JADERLINK");
            Console.WriteLine($"# Version {VERSION}");
            Console.WriteLine("# Created from Re4 PMD to SMD model exporter by magnum29 (perl)");

            if (args.Length == 0)
            {
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE4-2007-PMD-TOOL");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else if (args.Length >= 1 && File.Exists(args[0]))
            {
                FileInfo fileInfo = new FileInfo(args[0]);

                if (fileInfo.Extension.ToUpper() == ".PMD")
                {
                    string baseDiretory = fileInfo.DirectoryName + "\\";

                    ConfigFile configFile = new ConfigFile();

                    string configFilePath = null;
                    string configFileName = "RE4_2007_PMD_EXTRACT.txt";

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
                    Console.WriteLine("EnableUseIdxPmdMaterial: " + configFile.EnableUseIdxPmdMaterial);
                    Console.WriteLine("UseColorsInObjFile: " + configFile.UseColorsInObjFile);

                    Console.WriteLine(fileInfo.Name);
                    try
                    {

                        string baseName = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length, fileInfo.Extension.Length);

                        PMD pmd = PmdDecoder.GetPMD(args[0]);

                        OutputIdxPmd.IdxPmdCreate(pmd, baseDiretory, baseName, configFile);
                        OutputIdxPmdBone.IdxPmdBoneCreate(pmd, baseDiretory, baseName);
                        OutputFiles.SmdCreate(pmd, baseDiretory, baseName, configFile);
                        OutputFiles.ObjCreate(pmd, baseDiretory, baseName, configFile);
                        var mat = MaterialParser.Parser(pmd, configFile);
                        OutputMtl.MtlCreate(mat, baseDiretory, baseName);
                        OutputIdxPmdMaterial.IdxPmdMaterialCreate(mat, baseDiretory, baseName);

                        if (configFile.EnableDebugFiles)
                        {
                            OutputDebug.DebugInformation(pmd, fileInfo.FullName);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex);
                    }
                }
                else
                {
                    Console.WriteLine("Wrong file!");
                }
            }
            else
            {
                Console.WriteLine("The file does not exist!");
            }

            Console.WriteLine("Finished!!!");
        }
    }
}
