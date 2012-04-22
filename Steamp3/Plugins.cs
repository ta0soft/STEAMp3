#region Using
using Microsoft.CSharp;
using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
#endregion

namespace Steamp3.Plugins
{
    #region Compiler
    public class Compiler
    {
        #region Objects
        private string p_URL;
        private List<string> p_Errors;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public List<string> Errors
        {
            get { return p_Errors; }
        }
        #endregion
        
        #region Constructor/Destructor
        public Compiler(string url)
        {
            p_URL = url;
            p_Errors = new List<string>();

            if (Directory.Exists(p_URL)) CompileFolder();
            else if (File.Exists(p_URL)) CompileFile();
            else p_Errors.Add("Invalid plugin directory: " + p_URL);
        }

        public void Dispose()
        {
            p_Errors.Clear();
            p_URL = string.Empty;
        }
        #endregion

        #region Private Methods
        private bool CompileFile()
        {
            p_Errors.Clear();

            string dllFile = Application.StartupPath + "\\Plugins\\" + Path.GetFileNameWithoutExtension(p_URL) + ".dll";

            CompilerParameters cp = new CompilerParameters();
            cp.CompilerOptions = "/optimize";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            cp.ReferencedAssemblies.AddRange(new string[] { "System.dll", "System.Drawing.dll", "System.Web.dll", "System.Windows.Forms.dll", "System.Xml.dll" });
            cp.OutputAssembly = dllFile;

            CSharpCodeProvider provider = new CSharpCodeProvider();

            return !GetCompilerErrors(provider.CompileAssemblyFromSource(cp, Global.LoadString(p_URL)));
        }

        private bool CompileFolder()
        {
            p_Errors.Clear();

            string[] csFiles = Directory.GetFiles(p_URL, "*.cs", SearchOption.AllDirectories);

            if (csFiles.GetUpperBound(0) == 0)
            {
                p_Errors.Add("No .CS files found in: " + p_URL);
                return false;
            }

            string dllFile = Application.StartupPath + "\\Plugins\\" + Path.GetFileNameWithoutExtension(p_URL) + ".dll";

            CompilerParameters cp = new CompilerParameters();
            cp.CompilerOptions = "/optimize";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            cp.ReferencedAssemblies.AddRange(new string[] { "System.dll", "System.Drawing.dll", "System.Web.dll", "System.Windows.Forms.dll", "System.Xml.dll" });
            cp.OutputAssembly = dllFile;

            CSharpCodeProvider provider = new CSharpCodeProvider();

            List<string> result = new List<string>();

            foreach (string csFile in csFiles)
            {
                result.Add(Global.LoadString(csFile));
            }

            return !GetCompilerErrors(provider.CompileAssemblyFromSource(cp, result.ToArray()));
        }

        private bool GetCompilerErrors(CompilerResults results)
        {
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    p_Errors.Add("Line " + error.Line + " Column " + error.Column + " " + error.ErrorText);
                }

                return true;
            }

            return false;
        }
        #endregion
    }
    #endregion

    #region IPlugin
    public interface IPlugin
    {
    }
    #endregion

    #region Plugin
    public class Plugin : IPlugin
    {
        #region Constructor/Destructor
        public Plugin()
        {
        }

        public virtual void Dispose()
        {
        }
        #endregion
    }
    #endregion

    #region UIPlugin
    public class UIPlugin : UI.Dialog, IPlugin
    {
        #region Constructor/Destructor
        public UIPlugin()
        {
            this.ShowInTaskbar = true; //?
        }

        public new virtual void Dispose()
        {
        }
        #endregion

        #region Overrides
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Dispose();

            base.OnFormClosing(e);
        }
        #endregion
    }
    #endregion
}