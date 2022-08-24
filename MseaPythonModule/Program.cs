using IronPython.Compiler.Ast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MseaPythonModule
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Program Execute successfully.");
            try
            {

                //C: \Users\edgar\Documents\Personal Progamming\MseaPythonModule\
                // This will get the current PROJECT directory
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                //string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                //string path = Path.Combine(projectDirectory.ToString(), @"\\ScrapyMSEA\scrapysea\CompleteRun.py");
                Console.WriteLine(dir);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
