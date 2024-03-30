using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_2007_PMD_REPACK
{
    class Program
    {
        public const string VERSION = "B.1.0.13 (2024-03-30)";

        static void Main(string[] args)
        {
            Console.WriteLine("# github.com/JADERLINK/RE4-2007-PMD-TOOL");
            Console.WriteLine("# youtube.com/@JADERLINK");
            Console.WriteLine("# RE4_2007_PMD_TOOL (REPACK) By JADERLINK");
            Console.WriteLine($"# Version {Program.VERSION}");
            Console.WriteLine("# Created from Re4 SMD to PMD converter by magnum29 (perl)");

            if (args.Length == 0)
            {
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE4-2007-PMD-TOOL");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else if (args.Length >= 1 && File.Exists(args[0]))
            {
                var fileinfo = new FileInfo(args[0]);
                string fileExtension = fileinfo.Extension.ToUpperInvariant();

                if (fileExtension == ".OBJ" || fileExtension == ".SMD")
                {
                    Console.WriteLine(fileinfo.Name);
               
                    Action(fileinfo, fileExtension);
                }
                else
                {
                    Console.WriteLine("Wrong file");
                }
            }
            else
            {
                Console.WriteLine("The file does not exist");
            }

            Console.WriteLine("End");

        }

        private static void Action(FileInfo fileinfo, string fileExtension) 
        {
            string basePath = fileinfo.FullName.Substring(0, fileinfo.FullName.Length - fileinfo.Extension.Length);
            string idxPmdPath = basePath + ".idxpmd";

            if (File.Exists(idxPmdPath))
            {
                string pmdPath = basePath + ".pmd";
                string mtlPath = basePath + ".mtl";
                string bonePath = basePath + ".idxpmdbone";
                string materialPath = basePath + ".idxpmdmaterial";

                try
                {
                    // objetos
                    string[] ModelMaterialsArr = null;
                    IntermediaryStructure intermediaryStructure = null;
                    FinalBoneLine[] boneLines = null;

                    Dictionary<string, PmdMaterialLine> idxMaterial = new Dictionary<string, PmdMaterialLine>();
                    ObjLoader.Loader.Data.Material[] MtlMaterials = new ObjLoader.Loader.Data.Material[0];

                    // carrega o arquivo .idxPmd
                    StreamReader idxFile = new StreamReader(new FileInfo(idxPmdPath).OpenRead(), Encoding.ASCII);
                    IdxPmd idxPmd = IdxPmdLoader.Loader(idxFile);

                    if (idxPmd.UseIdxPmdMaterial)
                    {
                        if (!File.Exists(materialPath))
                        {
                            Console.WriteLine(new FileInfo(materialPath).Name + " does not exist");
                            return;
                        }
                        else 
                        {
                            Stream matFile = new FileInfo(materialPath).OpenRead();
                            idxMaterial = IdxPmdMaterialLoader.Load(matFile);
                        }
                    }

                    if (idxPmd.UseMtlFile)
                    {
                        if (!File.Exists(mtlPath))
                        {
                            Console.WriteLine(new FileInfo(mtlPath).Name + " does not exist");
                            return;
                        }
                        else 
                        {
                            MtlMaterials = PMDrepackMat.LoadMtlMaterials(mtlPath, idxPmd.UseMtlFile);
                        }
                     
                    }

                    if (fileExtension.Contains("OBJ"))
                    {
                        if (!File.Exists(bonePath))
                        {
                            Console.WriteLine(new FileInfo(bonePath).Name + " does not exist");
                            return;
                        }
                        else 
                        {
                            Stream boneFile = new FileInfo(bonePath).OpenRead();
                            var lines = IdxPmdBoneLoader.Load(boneFile);
                            boneLines = PMDrepackOBJ.GetBoneLines(lines);
                        }

                        PMDrepackOBJ.RepackOBJ(idxPmd, fileinfo.FullName, out intermediaryStructure, out ModelMaterialsArr);

                    }
                    else if (fileExtension.Contains("SMD"))
                    {
                        PMDrepackSMD.RepackSMD(idxPmd, fileinfo.FullName, out intermediaryStructure, out ModelMaterialsArr, out boneLines);
                    }

                    //---------
                    //arruma o Material
                    var UseMaterial = PMDrepackMat.GetMaterials(ModelMaterialsArr, idxMaterial, MtlMaterials, idxPmd.UseMtlFile, idxPmd.UseIdxPmdMaterial);
                    
                    PMDrepackMat.PrintTextureNamesInConsole(UseMaterial.Values.ToArray());

                    // estrutura final
                    FinalStructure finalStructure = PMDrepackFinal.MakeFinalStructure(intermediaryStructure, PMDrepackMat.GetIntermediaryNodeGroups(idxPmd.NodeGroups), idxPmd.IsScenarioPmd);

                    //finaliza e cria o arquivo pmd
                    PMDrepackFinal.MakeFinalPmdFile(pmdPath, finalStructure, boneLines, UseMaterial);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }

            }
            else
            {
                Console.WriteLine(new FileInfo(idxPmdPath).Name + " does not exist");
            }


        }


    }
}
