// Imported namespaces
using System.Reflection;
using Steamp3.Plugins;
using Steamp3.UI;

// Assembly information
[assembly: AssemblyProduct("HelloWorld")]
[assembly: AssemblyCompany("Ta0soft")]
[assembly: AssemblyDescription("Hello world plug-in (SDK Sample 01)")]
[assembly: AssemblyCopyright("Copyright © 2011 Ta0 Software")]
[assembly: AssemblyVersion("1.0")]

// Main class must inherit the Steamp3.Plugins.IPlugin interface.
// The Plugin class provides access to STEAMp3 without the use of UI elements.
// This class will be automatically disposed when the constructor exits the scope.
public class HelloWorld : Plugin
{
    // Constructor AKA entry point of the plug-in (Required).
    public HelloWorld()
    {
        // Create a Steamp3.UI.InfoDialog and display it to the user.
        InfoDialog info = new InfoDialog("Hello world!");
        info.ShowDialog();
    }
}