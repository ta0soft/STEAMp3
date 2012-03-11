// Imported namespaces
using System;
using System.Reflection;
using System.Windows.Forms;
using Steamp3.Plugins;
using Steamp3.UI;

// Assembly information
[assembly: AssemblyProduct("Dialogs")]
[assembly: AssemblyCompany("Ta0soft")]
[assembly: AssemblyDescription("Dialogs plug-in (SDK Sample 02)")]
[assembly: AssemblyCopyright("Copyright © 2011 Ta0 Software")]
[assembly: AssemblyVersion("1.0")]

public class Dialogs : Plugin
{
    public Dialogs()
    {
        // Loop until the user enters the correct information.
        retry:

        // Create a Steamp3.UI.InputDialog and display it to the user.
        InputDialog input = new InputDialog("Please enter your name:", "John Doe");
        if (input.ShowDialog() == DialogResult.OK)
        {
            // Create a Steamp3.UI.InfoDialog and allow the user to verify.
            InfoDialog info = new InfoDialog("Are you sure this name is correct?" + Environment.NewLine + input.TextBox.Text, InfoDialog.InfoButtons.YesNo);
            if (info.ShowDialog() == DialogResult.No) goto retry;

            // Create another Steamp3.UI.InfoDialog and display it to the user.
            InfoDialog info2 = new InfoDialog("Hello " + input.TextBox.Text + "!");
            info2.ShowDialog();
        }
    }
}