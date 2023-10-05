using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PMD_Repack
{
    class Program
    {
        public const string VERSION = "B.1.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("## RE4_PMD_Repack ##");
            Console.WriteLine($"## Version {VERSION} ##");
            Console.WriteLine("## By JADERLINK ##");
            Console.WriteLine("## Created from Re4 SMD to PMD converter by magnum29 (perl) ##");

            if (args.Length >= 1 && File.Exists(args[0])
                && (new FileInfo(args[0]).Extension.ToUpper() == ".OBJ"
                || new FileInfo(args[0]).Extension.ToUpper() == ".SMD"))

            {
                var fileinfo = new FileInfo(args[0]);
                var idxpmdPath = fileinfo.FullName.Substring(0, fileinfo.FullName.Length - fileinfo.Extension.Length) + ".IDXPMD";

                Console.WriteLine(args[0]);
                if (File.Exists(idxpmdPath))
                {
                    try
                    {
                        string pmdPath = fileinfo.FullName.Substring(0, fileinfo.FullName.Length - fileinfo.Extension.Length) + ".pmd";
                        string mtlPath = fileinfo.FullName.Substring(0, fileinfo.FullName.Length - fileinfo.Extension.Length) + ".mtl";
                        if (fileinfo.Extension.ToUpper().Contains("OBJ"))
                        {
                            PMDrepack.RepackOBJ(idxpmdPath, fileinfo.FullName, mtlPath, pmdPath);
                        }
                        else if (fileinfo.Extension.ToUpper().Contains("SMD"))
                        {
                            PMDrepack.RepackSMD(idxpmdPath, fileinfo.FullName, mtlPath, pmdPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex);
                    }

                }
                else
                {
                    Console.WriteLine(idxpmdPath + " does not exist");
                }


            }
            else
            {
                Console.WriteLine("no arguments or invalid file");
            }

            Console.WriteLine("End");


        }
    }
}
