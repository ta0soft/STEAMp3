#region Imports
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Xml;
using Steamp3;
using Steamp3.UI;
using Steamp3.Plugins;
using Steamp3.SteamAPI;
#endregion

#region Assembly Info
[assembly: AssemblyProduct("Chat Log")]
[assembly: AssemblyCompany("Ta0soft")]
[assembly: AssemblyDescription("Chat Log plug-in (SDK Sample 04)")]
[assembly: AssemblyCopyright("Copyright © ta0soft.com 2012")]
[assembly: AssemblyVersion("1.0")]
#endregion

#region ChatLog
public class ChatLog : UIPlugin
{
    #region Objects
    private static string g_SettingsURL = System.Windows.Forms.Application.StartupPath + "\\Plugins\\ChatLog.Settings.xml";

    private MessageList p_MessageList;
    private CheckBox p_CheckBox;
    private Button p_ClearButton, p_LoadButton, p_SaveButton;
    #endregion

    #region Constructor/Destructor
    public ChatLog() : base()
    {
        this.MinimizeBox = true;
        this.MinimumSize = new Size(400, 240);
        this.Size = new Size(400, 240);
        this.Text = "Chat Log plug-in (SDK Sample 04)";

        p_MessageList = new MessageList(this, null);

        p_CheckBox = new CheckBox(this, null, "Log Private Messages");
        p_CheckBox.Checked = true;

        p_ClearButton = new Button(this, null, "Clear Log");
        p_ClearButton.MouseClick += new System.Windows.Forms.MouseEventHandler(p_ClearButton_MouseClick);

        p_LoadButton = new Button(this, null, "Load XML file...");
        p_LoadButton.MouseClick += new System.Windows.Forms.MouseEventHandler(p_LoadButton_MouseClick);

        p_SaveButton = new Button(this, null, "Save XML file...");
        p_SaveButton.MouseClick += new System.Windows.Forms.MouseEventHandler(p_SaveButton_MouseClick);

        LoadSettings();

        Global.Steam.Chat.MessageReceived += new ChatRoom.MessageReceivedEventHandler(ChatMessageReceived);
        Global.Steam.PM.MessageReceived += new PrivateMessage.MessageReceivedEventHandler(PrivateMessageReceived);
    }

    public override void Dispose()
    {
        SaveSettings();

        base.Dispose();
    }
    #endregion

    #region Overrides
    protected override void OnResize(EventArgs e)
    {
        if (!Visible) return;

        int x = (this.Width / 2) - 12;

        p_MessageList.SetBounds(12, 30, this.Width - 24, this.Height - 100);
        p_CheckBox.SetBounds(12, this.Height - 58, x - 6, 20);
        p_ClearButton.SetBounds(12 + x, this.Height - 58, x, 20);
        p_LoadButton.SetBounds(12, this.Height - 32, x - 6, 20);
        p_SaveButton.SetBounds(12 + x, this.Height - 32, x, 20);

        base.OnResize(e);
    }
    #endregion

    #region Child Events
    private void p_ClearButton_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        p_MessageList.Clear();
    }

    private void p_LoadButton_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
        ofd.CheckFileExists = true;
        ofd.Filter = "Extensible Markup Language (*.xml)|*.xml";
        ofd.Multiselect = false;
        ofd.Title = "Load XML File";

        if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
            p_MessageList.Clear();

            XmlDocument xml = new XmlDocument();
            xml.Load(ofd.FileName);

            XmlNodeList nodeList = xml.SelectNodes("ChatLog/Message");

            foreach (XmlNode node in nodeList)
            {
                p_MessageList.Add(new Message(Global.StringToUInt64(Global.GetXmlValue(node, "ID", "0")), Global.GetXmlValue(node, "Message", string.Empty)));
            }
        }

        ofd.Dispose();
    }

    private void p_SaveButton_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
        sfd.CheckPathExists = true;
        sfd.Filter = "Extensible Markup Language (*.xml)|*.xml";
        sfd.OverwritePrompt = true;
        sfd.Title = "Save XML File";

        if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
            xml.AppendChild(declaration);

            XmlElement root = xml.CreateElement("ChatLog");
            xml.AppendChild(root);

            foreach (Message item in p_MessageList.Items)
            {
                XmlElement element = xml.CreateElement("Message");
                element.SetAttribute("ID", item.SteamID64.ToString());
                element.SetAttribute("PersonaName", item.PersonaName);
                element.SetAttribute("Message", item.Text);
                root.AppendChild(element);
            }

            Global.SaveXml(xml, sfd.FileName, Encoding.UTF8);
        }

        sfd.Dispose();
    }

    private void ChatMessageReceived(SteamID chatID, SteamID sender, string message)
    {
        p_MessageList.Add(new Message(sender.ToUInt64(), message));
    }

    private void PrivateMessageReceived(SteamID sender, SteamID receiver, string message)
    {
        if (!p_CheckBox.Checked) return;

        p_MessageList.Add(new Message(sender.ToUInt64(), message));
    }
    #endregion

    #region Private Methods
    private void LoadSettings()
    {
        if (File.Exists(g_SettingsURL))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(g_SettingsURL);

            p_CheckBox.Checked = Global.StringToBool(Global.GetXmlValue(xml.SelectSingleNode("ChatLog.Settings/LogPrivate"), string.Empty, "1"), "1");
        }
    }

    private void SaveSettings()
    {
        XmlDocument xml = new XmlDocument();
        XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
        xml.AppendChild(declaration);

        XmlElement root = xml.CreateElement("ChatLog.Settings");
        xml.AppendChild(root);

        XmlElement element = xml.CreateElement("LogPrivate");
        element.InnerText = Global.BoolToString(p_CheckBox.Checked, "0", "1");
        root.AppendChild(element);

        Global.SaveXml(xml, g_SettingsURL, Encoding.UTF8);
    }
    #endregion
}
#endregion

#region Message
public class Message : GenericListItem
{
    #region Objects
    private ulong p_SteamID64;
    #endregion

    #region Properties
    public ulong SteamID64
    {
        get { return p_SteamID64; }
    }

    public string PersonaName
    {
        get
        {
            if (p_SteamID64 == 0) return "Dealer";
            return Global.Steam.Client.GetFriendPersonaName(new SteamID(p_SteamID64));
        }
    }
    #endregion

    #region Constructor/Destructor
    public Message(ulong steamID64, string text) : base()
    {
        p_SteamID64 = steamID64;
        Text = text;
    }

    public override void Dispose()
    {
        p_SteamID64 = 0;

        base.Dispose();
    }
    #endregion
}
#endregion

#region MessageList
public class MessageList : GenericList<Message>
{
    #region Constructor/Destructor
    public MessageList(Window owner, Control parent) : base(owner, parent)
    {
        Mask = "No messages found.";
    }

    public override void Dispose()
    {
        base.Dispose();
    }
    #endregion

    #region Overrides
    protected override void DrawItem(DrawItemEventArgs e)
    {
        Message item = e.Item as Message;
        if (item == null) return;

        Color textColor = Global.Skin.List.TextColor;

        if (item.Equals(SelectedItem))
        {
            textColor = Global.Skin.List.Down_TextColor;

            LinearGradientBrush b = new LinearGradientBrush(e.Bounds, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
            b.WrapMode = WrapMode.TileFlipX;

            e.Graphics.FillRectangle(b, e.Bounds);
        }

        if (item.Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

        e.Graphics.DrawString(item.PersonaName, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(e.Bounds.X, e.Bounds.Y, 100, e.Bounds.Height), e.StringFormat);
        e.Graphics.DrawString(item.Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(e.Bounds.X + 100, e.Bounds.Y, e.Bounds.Width - 100, e.Bounds.Height), e.StringFormat);
    }
    #endregion
}
#endregion