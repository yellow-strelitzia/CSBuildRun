//--------------------------------------------------
// Date         Name                Description
// 2019.08.01   y-strelitzia        Initial
//--------------------------------------------------

using System;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace YellowStrelitzia.CSTools
{
    //--------------------------------------------------
    /// <summary>
    /// Bootstrap classs, run C# code from file
    /// </summary>
    /// <remarks>
    /// Bootstrap classs, run C# code from file
    /// </remarks>
    //--------------------------------------------------
    public class Bootstrap
    {
        //--------------------------------------------------
        /// <summary>
        /// Entry point of Bootstrap application
        /// </summary>
        /// <param name="args">
        /// commandline paramteters
        /// </param>
        /// <remarks>
        /// Entry point of Bootstrap application
        /// </remarks>
        //--------------------------------------------------            
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // 
                Process process = Process.GetCurrentProcess();
                string curProcessFullPath = process.MainModule.FileName;

                // information of execution of C# code file
                string[] inputSourceCode = {"Main.cs"};
                string inputStartupClass = "MainClass";
                string inputStartupMethod = "startup";
                string[] assemblyRefs = {"mscorlib.dll","System.dll","System.Drawing.dll","System.Windows.Forms.dll",
                                    "System.Data.dll","System.Xml.dll","Microsoft.CSharp.dll",curProcessFullPath};
                
                // re-write configuration from config file
                Dictionary<string, string> configs = new Dictionary<string, string>();
                ConfigurationManager.OpenExeConfiguration(@Process.GetCurrentProcess().MainModule.FileName);
                foreach (string key in ConfigurationManager.AppSettings.AllKeys)
                {
                    configs[key] = ConfigurationManager.AppSettings[key];
                }
                foreach (string key in configs.Keys)
                {
                    if ( key == "InputSourceCode" )
                        inputSourceCode = new string[] {configs[key]};
                    else if ( key == "InputStartupClass" )
                        inputStartupClass = configs[key];
                    else if ( key == "InputStartupMethod" )
                        inputStartupMethod = configs[key];
                    else if ( key == "AssemblyRefs" )
                    {
                        assemblyRefs = configs[key].Split(',');
                        List<string> list = new List<string>(assemblyRefs);
                        list.Add(curProcessFullPath);
                        assemblyRefs = list.ToArray();
                    }
                    else if ( key == "AssemblyRefs20" )
                    {
                        #if (Net20)
                        assemblyRefs = configs[key].Split(',');
                        List<string> list = new List<string>(assemblyRefs);
                        list.Add(curProcessFullPath);
                        assemblyRefs = list.ToArray();
                        #endif
                    }
                }

                CompileAndRunCSFile(inputSourceCode, inputStartupClass, inputStartupMethod, assemblyRefs, args);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during Compile and Invoke Method.");
                Console.WriteLine(e);
            }
        }

        //--------------------------------------------------
        /// <summary>
        /// Compile and execute method of compiled assembly dynamically
        /// </summary>
        /// <param name="inputSourceCode">
        /// array of compile source files
        /// </param>
        /// <param name="inputStartupClass">
        /// start up class name
        /// </param>
        /// <param name="inputStartupMethod">
        /// start up method name
        /// </param>
        /// <param name="assemblyRefs">
        /// link target assembly reference array
        /// </param>
        /// <param name="args">
        /// commandline paramteters
        /// </param>
        /// <remarks>
        /// Compile and execute method of compiled assembly dynamically
        /// </remarks>
        //--------------------------------------------------      
        public static void CompileAndRunCSFile(string[] inputSourceCode, string inputStartupClass, 
                                         string inputStartupMethod, string[] assemblyRefs, string[] args)
        {
            // try to compile c# file
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            parameters.CompilerOptions = "/optimize";
            
            foreach ( string asmref in assemblyRefs )
            {
                parameters.ReferencedAssemblies.Add(asmref);
            }
            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters,inputSourceCode);

            // compile staet check
            if (results.Errors.Count > 0)
            {
                string errDetails = "";
                foreach(CompilerError CompErr in results.Errors)
                {
                    errDetails += "Line number " + CompErr.Line + 
                              ", Error Number: " + CompErr.ErrorNumber + 
                              ", '" + CompErr.ErrorText + ";" + 
                              Environment.NewLine;
                }
                System.Console.WriteLine( errDetails );
            }
            else
            {
                //Successful Compile
                Assembly asmbl = results.CompiledAssembly;
                object instance = asmbl.CreateInstance(inputStartupClass);
                MethodInfo method = null;
                if ( instance != null )
                {
                    method = instance.GetType().GetMethod(inputStartupMethod);
                }
                if ( method != null )
                {
                    method.Invoke(instance, new object[] {args});
                } 
                
                if ( instance == null )
                {
                    throw new Exception(string.Format("Instance source class {0} is not found.", inputStartupClass));
                }
                else if ( method == null )
                {
                    throw new Exception(string.Format("Startup method {0} is not found.", inputStartupMethod));
                }
            }
        }
    }

    //--------------------------------------------------
    /// <summary>
    /// Template classs, to be build and run C# code from file
    /// </summary>
    /// <remarks>
    /// Template classs, to be build and run C# code from file
    /// </remarks>
    //--------------------------------------------------
    public class BuildAndRunTemplate
    {
        //
        protected string[] _args = null;
        //
        protected bool _isGUI = false;

        //--------------------------------------------------
        /// <summary>
        /// Entry point of c# code on demand
        /// (but not called from BuildAndRun bootstrap)
        /// </summary>
        /// <param name="args">
        /// commandline paramteters
        /// </param>
        /// <remarks>
        /// Entry point of c# code on demand
        /// (but not called from BuildAndRun bootstrap)
        /// </remarks>
        //--------------------------------------------------
        public static void MainSupport( string[] args )
        {
            // try to detect declaring class of this Main method and try to create instance
            Type thisType = System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType;
            BuildAndRunTemplate instance = Activator.CreateInstance(thisType) as BuildAndRunTemplate;
            Process p = Process.GetCurrentProcess();
            
            instance.startup(args);
        }

        //--------------------------------------------------
        /// <summary>
        /// Entry point of c# code on demand
        /// </summary>
        /// <value>
        /// <param name="args">
        /// commandline paramteters
        /// </param>
        /// <remarks>
        /// Entry point of c# code on demand
        /// </remarks>
        //--------------------------------------------------
        public void startup( string[] args )
        {
            #if (GUI)   
                // template code to launch GUI
                _isGUI = true;
            #else  
                // template code to start CUI
                _isGUI = false;
            #endif 

            startupImpl( args, _isGUI );
        }

        //--------------------------------------------------
        /// <summary>
        /// Entry point of c# code on demand
        /// </summary>
        /// <value>
        /// <param name="args">
        /// commandline paramteters
        /// </param>
        /// <param name="isGUI">
        /// gui flag
        /// </param>
        /// <remarks>
        /// Entry point of c# code on demand
        /// </remarks>
        //--------------------------------------------------
        public void startupImpl( string[] args, bool isGUI )
        {
            _args = args;

            if ( isGUI ) 
            {
                // template code to launch GUI
                Thread staThread = new Thread(new ThreadStart(this.startGUI));
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
            } else {
                // template code to launch CUI
                Thread staThread = new Thread(new ThreadStart(this.startCUI));
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
            }
        }

        //--------------------------------------------------
        /// <summary>
        /// CUI entry point function
        /// </summary>
        /// <value>
        /// <remarks>
        /// CUI entry point function
        /// </remarks>
        //--------------------------------------------------
        public virtual void startCUI()
        {
            System.Console.WriteLine("startCUI called.");
        }

        //--------------------------------------------------
        /// <summary>
        /// GUI entry point function
        /// </summary>
        /// <value>
        /// <remarks>
        /// GUI entry point function
        /// </remarks>
        //--------------------------------------------------
        public virtual void startGUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form());
        }
    }

    //--------------------------------------------------
    /// <summary>
    /// Utility classs, for build and run C# code 
    /// </summary>
    /// <remarks>
    /// Utility classs, for build and run C# code 
    /// </remarks>
    //--------------------------------------------------
    public class BuildAndRunUtil
    {
        #if (!Net20)
        //--------------------------------------------------
        /// <summary>
        /// Utility method for process files recursively
        /// </summary>
        /// <value>
        /// <param name="rootPath">
        /// root path
        /// </param>
        /// <param name="filter">
        /// file name filter
        /// </param>
        /// <param name="handler">
        /// handler Funcs
        /// </param>
        /// <remarks>
        /// Utility method for process files recursively
        /// </remarks>
        //--------------------------------------------------
        public static List<string> ProcessRecurcively( string rootPath, 
            string filter, Func<string, bool> handler )
        {
            var pickedList = new List<string>();

            try
            {
                IEnumerable<string> subDirs = Directory.EnumerateDirectories(rootPath);
                foreach (string dir in subDirs)
                {
                    pickedList.AddRange(ProcessRecurcively(dir, filter, handler)); // Add files in subdirectories recursively to the list
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (PathTooLongException) { }

            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(rootPath, filter);
                foreach (string f in files)
                {
                    if ( handler(f) ) pickedList.Add(f);
                }
            }
            catch (UnauthorizedAccessException) { }
            return pickedList;
        }        

        //--------------------------------------------------
        /// <summary>
        /// Utility method for process excel cell walker
        /// </summary>
        /// <value>
        /// <param name="excelPath">
        /// root path
        /// </param>
        /// <param name="handler">
        /// handler Func
        /// </param>
        /// <remarks>
        /// Utility method for process files recursivel
        /// </remarks>
        //--------------------------------------------------
        public static List<string> ProcessExcelCells( string excelPath, 
            Func<string, string> handler )
        {
            return null;
        }
        #endif
    }
}