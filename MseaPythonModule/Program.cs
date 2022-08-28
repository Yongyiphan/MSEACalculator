using IronPython.Compiler.Ast;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Composition.Interactions;

namespace MseaPythonModule
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Program Execute successfully.");
            try
            {
                //python ./scrapysea/CompleteRun.py
                string projectDirectory = FindSolutionBaseDirectory(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
                string pythonSourceFolder = Path.Combine(projectDirectory + @"\MseaPythonModule\ScrapyMSEA");
                string localFolderPath = ApplicationData.Current.LocalFolder.Path;


                
                string activateVenv = @".\env\Scripts\activate";
                if (!Directory.Exists(Path.Combine(pythonSourceFolder + @"\env")))
                {
                    using(Process InitVenv = new Process())
                    {
                        InitVenv.StartInfo.FileName = "cmd.exe";
                        InitVenv.StartInfo.WorkingDirectory = pythonSourceFolder;
                        InitVenv.StartInfo.RedirectStandardInput = true;
                        InitVenv.StartInfo.RedirectStandardInput = true;
                        InitVenv.StartInfo.RedirectStandardError = true;
                        InitVenv.StartInfo.UseShellExecute = false;
                        InitVenv.StartInfo.CreateNoWindow = true;

                        InitVenv.Start();
                       

                        using(StreamWriter sw = InitVenv.StandardInput)
                        {
                            sw.WriteLine("py -m venv env");
                            sw.WriteLine(activateVenv);
                            sw.WriteLine("python -m pip install --upgrade --force-reinstall pip");
                            sw.WriteLine("pip install -r requirements.txt");
                            //sw.WriteLine("deactivate");
                        }
                        InitVenv.WaitForExit();
                    }
                }

                using (Process RunPython = new Process())
                {
                    RunPython.StartInfo.FileName = "cmd.exe";
                    RunPython.StartInfo.WorkingDirectory = pythonSourceFolder;
                    RunPython.StartInfo.RedirectStandardInput = true;
                    RunPython.StartInfo.RedirectStandardInput = true;
                    RunPython.StartInfo.RedirectStandardError = true;
                    RunPython.StartInfo.UseShellExecute = false;
                    RunPython.StartInfo.CreateNoWindow = false;
                    
                    
                    RunPython.Start();

                    using(StreamWriter sw = RunPython.StandardInput)
                    {
                        sw.WriteLine(activateVenv);

                        sw.WriteLine(@"python ./scrapysea/CompleteRun.py " + localFolderPath + " True");

                        sw.WriteLine("deactivate");
                    }
                    RunPython.WaitForExit();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        public static DirectoryInfo FindSolutionBaseDirectory(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.appxmanifest").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
