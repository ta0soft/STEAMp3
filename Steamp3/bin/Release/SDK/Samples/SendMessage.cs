#region Imports
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Steamp3;
using Steamp3.SteamAPI;
#endregion

#region AssemblyInfo
[assembly: AssemblyProduct("SendMessage")]
[assembly: AssemblyCompany("Ta0soft")]
[assembly: AssemblyDescription("SendMessage plug-in (SDK Sample 03)")]
[assembly: AssemblyCopyright("Copyright © 2011 Ta0 Software")]
[assembly: AssemblyVersion("1.0")]
#endregion

// The UIPlugin class provides a Popup window that allows you to add/remove UI elements.
// This class will be automatically disposed when the user closes the window.
public class SendMessage : Steamp3.Plugins.UIPlugin
{
    #region Objects
    private Steamp3.UI.List p_FriendsList;
    private Steamp3.UI.TextBox p_TextBox;
    private Steamp3.UI.Button p_ButtonChat, p_ButtonFriend;
    #endregion

    #region Constructor
    public SendMessage()
    {
        // Base window properties
        this.Sizable = false;
        this.Size = new Size(272, 200);
        this.Text = "SendMessage plug-in (SDK Sample 03)";

        // Create a Steamp3.UI.List control
        p_FriendsList = new Steamp3.UI.List(this, null);
        p_FriendsList.SelectedItemChanged += new EventHandler(p_FriendsList_SelectedItemChanged);
        p_FriendsList.SetBounds(12, 30, 248, 100);

        // Create a Steamp3.UI.TextBox control
        p_TextBox = new Steamp3.UI.TextBox(this, null, "Hello {name}!");
        p_TextBox.SetBounds(12, 136, 248, 20);

        // Create a Steamp3.UI.Button control
        p_ButtonChat = new Steamp3.UI.Button(this, null, "Send text to chat");
        p_ButtonChat.Enabled = true;
        p_ButtonChat.MouseClick += new MouseEventHandler(p_ButtonChat_MouseClick);
        p_ButtonChat.SetBounds(12, 168, 120, 20);

        // Create another Button control
        p_ButtonFriend = new Steamp3.UI.Button(this, null, "Send text to friend");
        p_ButtonFriend.Enabled = false;
        p_ButtonFriend.MouseClick += new MouseEventHandler(p_ButtonFriend_MouseClick);
        p_ButtonFriend.SetBounds(140, 168, 120, 20);

        // Add friends to p_FriendsList using the Global SteamClient.GetFriends method.
        foreach (SteamID id in Global.Steam.Client.GetFriends())
        {
            // Create a Steamp3.UI.ListItem and add it to the list.
            Steamp3.UI.ListItem item = new Steamp3.UI.ListItem(Global.Steam.Client.GetFriendPersonaName(id));
            item.Tag = id;

            p_FriendsList.Add(item);
        }
    }
    #endregion

    #region Events
    private void p_FriendsList_SelectedItemChanged(object sender, EventArgs e)
    {
        // Disable "Send text to friend" Button if no item is selected.
        p_ButtonFriend.Enabled = p_FriendsList.SelectedItem != null;
    }

    private void p_ButtonChat_MouseClick(object sender, MouseEventArgs e)
    {
        // Send message to current chat using the Global ChatRoom.SendMessage method.
        Global.Steam.Chat.SendMessage(FormatText(p_TextBox.Text, true));
    }

    private void p_ButtonFriend_MouseClick(object sender, MouseEventArgs e)
    {
        // Send message to selected friend using the Global PrivateMessage.SendMessage method.
        Global.Steam.PM.SendMessage((SteamID)p_FriendsList.SelectedItem.Tag, FormatText(p_TextBox.Text, false));
    }
    #endregion

    #region Private Methods
    private string FormatText(string text, bool chat)
    {
        string result = string.Empty;

        if (chat)
        {
            // Replace {name} with current chat room name.
            result = Global.ReplaceString(text, "{name}", Global.Steam.Chat.GetChatName());
        }
        else
        {
            // Replace {name} with selected friend's name.
            result = Global.ReplaceString(text, "{name}", p_FriendsList.SelectedItem.Text);
        }

        return result;
    }
    #endregion
}