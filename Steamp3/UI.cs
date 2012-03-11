#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using HundredMilesSoftware.UltraID3Lib;
#endregion

namespace Steamp3.UI
{
    #region AboutDialog
    public class AboutDialog : Dialog
    {
        #region Objects
        private Label p_Label;
        private Button p_ButtonHelp, p_ButtonCredits, p_ButtonClose;
        #endregion

        #region Constructors/Destuctor
        public AboutDialog() : base()
        {
            Popup = false;
            Sizable = false;
            Size = new Size(280, 206);
            Text = string.Empty;

            int x = (Width - 36) / 3;

            p_Label = new Label(this, null, "Version: " + Application.ProductVersion.Substring(0, 3) + " Build: " + Application.ProductVersion.Substring(4) + Environment.NewLine + Environment.NewLine + "For Windows XP/Vista/7/8" + Environment.NewLine + "Copyright © 2012 Ta0 Software" + Environment.NewLine + "Developed in Microsoft Visual C# 2010" + Environment.NewLine + Environment.NewLine + "For more information visit:" + Environment.NewLine + "http://steamp3.ta0soft.com");
            p_Label.SetBounds(12, 46, Width - 24, 120);

            p_ButtonHelp = new Button(this, null, string.Empty, "Help...", string.Empty);
            p_ButtonHelp.MouseClick += new MouseEventHandler(p_ButtonHelp_MouseClick);
            p_ButtonHelp.SetBounds(12, 172, x, 20); 

            p_ButtonCredits = new Button(this, null, string.Empty, "Credits...", string.Empty);
            p_ButtonCredits.MouseClick += new MouseEventHandler(p_ButtonCredits_MouseClick);
            p_ButtonCredits.SetBounds(x + 18, 172, x, 20);

            p_ButtonClose = new Button(this, null, string.Empty, "Close", string.Empty);
            p_ButtonClose.MouseClick += new MouseEventHandler(p_ButtonClose_MouseClick);
            p_ButtonClose.SetBounds((x * 2) + 24, 172, x, 20);
        }

        ~AboutDialog()
        {
        }
        #endregion

        #region Overrides
        protected override void OnDraw(Graphics g)
        {
            g.DrawString("¯", new Font(Skin.IconFont.Name, 180.0f, FontStyle.Regular), new SolidBrush(Color.FromArgb(30, Global.Skin.Window.TextColor)), 36, -24);
            g.DrawString("STEAMp3", new Font(Skin.WindowFont.Name, 30.0f, FontStyle.Regular), new SolidBrush(Global.Skin.Popup.TextColor), 4, 4);

            base.OnDraw(g);
        }
        #endregion

        #region Child Events
        private void p_ButtonHelp_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://steamp3.ta0soft.com/help/");
        }

        private void p_ButtonCredits_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://steamp3.ta0soft.com/credits/");
        }

        private void p_ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        #endregion
    }
    #endregion

    #region Achievement
    public class Achievement : GenericListItem
    {
        #region Enums
        public enum AchievementIDs : int
        {
            None = 0,
            ChattyCathy = 1,
            JukeboxHero = 2,
            LeetHaxor = 3,
            MP3Playah = 4,
            Repetition = 5,
        }

        public enum AchievementTypes : int
        {
            Media = 0,
            Steam = 1,
            Web = 2,
            Misc = 3,
        }
        #endregion

        #region Objects
        private AchievementIDs p_ID;
        private string p_Name, p_Description;
        private AchievementTypes p_AchievementType;
        private int p_Maximum;
        #endregion

        #region Properties
        public AchievementIDs ID
        {
            get { return p_ID; }
        }

        public string Name
        {
            get { return p_Name; }
        }

        public string Description
        {
            get { return p_Description; }
        }

        public AchievementTypes AchievementType
        {
            get { return p_AchievementType; }
        }

        public int Maximum
        {
            get { return p_Maximum; }
        }

        public int Value
        {
            get
            {
                switch (p_ID)
                {
                    case AchievementIDs.ChattyCathy:
                        return Global.Stats.ChattyCathy;
                    case AchievementIDs.JukeboxHero:
                        return Global.Stats.JukeboxHero;
                    case AchievementIDs.LeetHaxor:
                        return Global.Stats.LeetHaxor;
                    case AchievementIDs.MP3Playah:
                        return Global.Stats.MP3Playah;
                    case AchievementIDs.Repetition:
                        return Global.Stats.Repetition;
                    default:
                        return 0;
                }
            }
            set
            {
                if (value < 0) return;
                if (value > Maximum) return;

                switch (p_ID)
                {
                    case AchievementIDs.ChattyCathy:
                        Global.Stats.ChattyCathy = value; //?
                        break;
                    case AchievementIDs.JukeboxHero:
                        Global.Stats.JukeboxHero = value; //?
                        break;
                    case AchievementIDs.LeetHaxor:
                        Global.Stats.LeetHaxor = value; //?
                        break;
                    case AchievementIDs.MP3Playah:
                        Global.Stats.MP3Playah = value; //?
                        break;
                    case AchievementIDs.Repetition:
                        Global.Stats.Repetition = value; //?
                        break;
                }

                if (value == Maximum)
                {
                    Global.Steam.SendStatus("Achievement earned", p_Name, true);
                }
            }
        }
        #endregion

        #region Constructor/Destructor
        public Achievement(XmlNode node) : base()
        {
            if (node != null)
            {
                p_ID = (AchievementIDs)Global.StringToInt(Global.GetXmlValue(node, "ID", "0"));
                p_Name = Global.GetXmlValue(node, "Name", string.Empty);
                p_Description = Global.GetXmlValue(node, "Desc", string.Empty);
                p_AchievementType = (AchievementTypes)Global.StringToInt(Global.GetXmlValue(node, "Type", "0"));
                p_Maximum = Global.StringToInt(Global.GetXmlValue(node, "Max", "0"));
            }
            else
            {
                p_ID = AchievementIDs.None;
                p_Name = string.Empty;
                p_Description = string.Empty;
                p_AchievementType = AchievementTypes.Misc;
                p_Maximum = 0;
            }
        }

        public override void Dispose()
        {
            p_Maximum = 0;
            p_AchievementType = AchievementTypes.Media;
            p_Description = string.Empty;
            p_Name = string.Empty;
            p_ID = AchievementIDs.None;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_Name;
        }
        #endregion

        #region Save
        public string Save()
        {
            string result = string.Empty;
            bool fullOnly = false;

            switch (p_ID)
            {
                case AchievementIDs.ChattyCathy:
                    result = "&chattyCathy=";
                    fullOnly = true;
                    break;
                case AchievementIDs.JukeboxHero:
                    result = "&jukeboxHero=";
                    fullOnly = true;
                    break;
                case AchievementIDs.LeetHaxor:
                    result = "&leetHaxor=";
                    fullOnly = true;
                    break;
                case AchievementIDs.MP3Playah:
                    result = "&mp3Playah=";
                    break;
                case AchievementIDs.Repetition:
                    result = "&repetition=";
                    fullOnly = true;
                    break;
            }

            if (fullOnly)
            {
                if (Value == Maximum) result += Value.ToString();
                else result += "0";
            }
            else result += Value.ToString();

            return result;
        }
        #endregion
    }
    #endregion

    #region AchievementList
    public class AchievementList : GenericList<Achievement>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllAchievements,
            MediaAchievements,
            SteamAchievements,
            WebAchievements,
            MiscAchievements,
            Achieved,
            NotAchieved,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        private WebClient p_WebClient;

        private SteamAPI.SteamID p_ChattyCathyChatID;
        private string p_RepetitionURL;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public AchievementList(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Achievements", new string[] { "All Achievements (0)", "Media Achievements (0)", "Steam Achievements (0)", "Web Achievements (0)", "Misc. Achievements (0)", "Achieved (0)", "Not Achieved (0)" }, "Filter Achievements");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("q", "Refresh Achievements") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            p_WebClient = new WebClient();
            p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);

            p_ChattyCathyChatID = null;
            p_RepetitionURL = string.Empty;
        }

        public override void Dispose()
        {
            p_RepetitionURL = string.Empty;
            if (p_ChattyCathyChatID != null) p_ChattyCathyChatID.Dispose();

            p_WebClient.Dispose();
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int x2 = x / 3;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing achievements, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No achievements found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;
                    Color bgColor = Global.Skin.List.Stop_BGColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;
                    if (FilteredItems[i].Value == FilteredItems[i].Maximum) bgColor = Global.Skin.List.Play_BGColor;

                    g.DrawString(FilteredItems[i].Name, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x2, ItemHeight), sf);
                    g.DrawString(FilteredItems[i].Description, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + 3) + x2, (bounds.Y + y) + 3, (x2 * 2) - 100, ItemHeight), sf);

                    if (FilteredItems[i].Value > 0) Global.FillRoundedRectangle(g, new Rectangle((bounds.X + x) - 100, (bounds.Y + y) + 4, Global.GetPercent(FilteredItems[i].Value, FilteredItems[i].Maximum, 100), ItemHeight - 3), new SolidBrush(bgColor));
                    Global.DrawRoundedRectangle(g, new Rectangle((bounds.X + x) - 100, (bounds.Y + y) + 4, 100, ItemHeight - 3), new Pen(Global.Skin.List.BorderColor));

                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<Achievement> items = new List<Achievement>();

            foreach (Achievement item in Items)
            {
                if (item.Name.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllAchievements:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.MediaAchievements:
                    foreach (Achievement item in items)
                    {
                        if (item.AchievementType == Achievement.AchievementTypes.Media) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.SteamAchievements:
                    foreach (Achievement item in items)
                    {
                        if (item.AchievementType == Achievement.AchievementTypes.Steam) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.WebAchievements:
                    foreach (Achievement item in items)
                    {
                        if (item.AchievementType == Achievement.AchievementTypes.Web) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.MiscAchievements:
                    foreach (Achievement item in items)
                    {
                        if (item.AchievementType == Achievement.AchievementTypes.Misc) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Achieved:
                    foreach (Achievement item in items)
                    {
                        if (item.Value == item.Maximum) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.NotAchieved:
                    foreach (Achievement item in items)
                    {
                        if (item.Value < item.Maximum) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new GenericListSorter<Achievement>(GenericListSorter<Achievement>.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Achievements (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Media Achievements (" + Global.FormatNumber(GetMediaAchievementsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "Steam Achievements (" + Global.FormatNumber(GetSteamAchievementsCount()) + ")";
            p_FilterBar.DropDown.Items[3] = "Web Achievements (" + Global.FormatNumber(GetWebAchievementsCount()) + ")";
            p_FilterBar.DropDown.Items[4] = "Misc. Achievements (" + Global.FormatNumber(GetMiscAchievementsCount()) + ")";
            p_FilterBar.DropDown.Items[5] = "Achieved (" + Global.FormatNumber(GetAchievedCount()) + ")";
            p_FilterBar.DropDown.Items[6] = "Not Achieved (" + Global.FormatNumber(GetNotAchievedCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Refresh Achievements":
                    Refresh();
                    break;
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    List<Achievement> list = new List<Achievement>();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(e.Result);
                    XmlNodeList nodeList = xml.SelectNodes("Steamp3.Achievements/Achievement");

                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(new UI.Achievement(node));
                    }

                    Items.Clear();
                    AddRange(list);

                    list.Clear();
                }
                catch
                {
                }

                Refreshing = false;
            }
        }
        #endregion

        #region Public Methods
        public int GetMediaAchievementsCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.AchievementType == Achievement.AchievementTypes.Media) result++;
            }

            return result;
        }

        public int GetSteamAchievementsCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.AchievementType == Achievement.AchievementTypes.Steam) result++;
            }

            return result;
        }

        public int GetWebAchievementsCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.AchievementType == Achievement.AchievementTypes.Web) result++;
            }

            return result;
        }

        public int GetMiscAchievementsCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.AchievementType == Achievement.AchievementTypes.Misc) result++;
            }

            return result;
        }


        public int GetAchievedCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.Value == item.Maximum) result++;
            }

            return result;
        }

        public int GetNotAchievedCount()
        {
            int result = 0;

            foreach (Achievement item in Items)
            {
                if (item.Value < item.Maximum) result++;
            }

            return result;
        }

        public void Refresh()
        {
            if (!Global.MediaPlayer.IsOnline) return;

            Refreshing = true;

            p_WebClient.DownloadStringAsync(new Uri("http://steamp3.ta0soft.com/achievements/achievements.xml"));
        }

        public Achievement GetItemByID(Achievement.AchievementIDs id)
        {
            foreach (Achievement item in Items)
            {
                if (item.ID == id) return item;
            }

            return null;
        }

        public void IncreaseChattyCathy(SteamAPI.SteamID chatID)
        {
            Achievement achievement = GetItemByID(Achievement.AchievementIDs.ChattyCathy);

            if (achievement != null && achievement.Value < achievement.Maximum)
            {
                if (chatID == p_ChattyCathyChatID)
                {
                    achievement.Value++;
                    ReDraw();
                }
                else
                {
                    p_ChattyCathyChatID = chatID;

                    achievement.Value = 1;
                    ReDraw();
                }
            }
        }

        public void IncreaseJukeboxHero(PlaylistItem item)
        {
            Achievement achievement = GetItemByID(Achievement.AchievementIDs.JukeboxHero);

            if (achievement != null && achievement.Value < achievement.Maximum)
            {
                achievement.Value++;
                ReDraw();
            }
        }

        public void IncreaseLeetHaxor(UI.PlaylistItem item)
        {
            Achievement achievement = GetItemByID(Achievement.AchievementIDs.LeetHaxor);

            if (achievement != null && Global.ConvertTime(item.Duration, false) == "13:37")
            {
                achievement.Value++;
                ReDraw();
            }
        }

        public void IncreaseMP3Playah(UI.PlaylistItem item)
        {
            Achievement achievement = GetItemByID(Achievement.AchievementIDs.MP3Playah);

            if (achievement != null && Path.GetExtension(item.URL).ToLower() == ".mp3")
            {
                achievement.Value++;
                ReDraw();
            }
        }

        public void IncreaseRepetition(UI.PlaylistItem item, string status)
        {
            Achievement achievement = GetItemByID(Achievement.AchievementIDs.Repetition);

            if (achievement != null && achievement.Value < achievement.Maximum)
            {
                if (item.URL == p_RepetitionURL && status == "Repeat")
                {
                    achievement.Value++;
                    ReDraw();
                }
                else
                {
                    p_RepetitionURL = item.URL;

                    if (status == "Repeat") achievement.Value = 1;
                    else achievement.Value = 0;
                    ReDraw();
                }
            }
        }
        #endregion
    }
    #endregion

    #region Button
    public class Button : Control
    {
        #region Objects
        private DialogResult p_Result;
        #endregion

        #region Properties
        public DialogResult Result
        {
            get { return p_Result; }
            set { p_Result = value; }
        }
        #endregion

        #region Constructors/Destructor
        public Button(Window owner, Control parent, string text) : base(owner, parent)
        {
            Text = text;

            p_Result = DialogResult.OK;
        }

        public Button(Window owner, Control parent, string text, string toolTipText) : base(owner, parent)
        {
            Text = text;
            ToolTipText = toolTipText;

            p_Result = DialogResult.OK;
        }

        public Button(Window owner, Control parent, string icon, string text, string toolTipText) : base(owner, parent)
        {
            Icon = icon;
            Text = text;
            ToolTipText = toolTipText;

            p_Result = DialogResult.OK;
        }

        public override void Dispose()
        {
            p_Result = DialogResult.OK;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            RectangleF rf = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Color bgColor = Global.Skin.Button.BGColor;
            Color bgColor2 = Global.Skin.Button.BGColor2;
            Color borderColor = Global.Skin.Button.BorderColor;
            Color textColor = Global.Skin.Button.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    bgColor = Global.Skin.Button.Down_BGColor;
                    bgColor2 = Global.Skin.Button.Down_BGColor2;
                    borderColor = Global.Skin.Button.Down_BorderColor;
                    textColor = Global.Skin.Button.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    bgColor = Global.Skin.Button.Over_BGColor;
                    bgColor2 = Global.Skin.Button.Over_BGColor2;
                    borderColor = Global.Skin.Button.Over_BorderColor;
                    textColor = Global.Skin.Button.Over_TextColor;
                }

                if (DrawBackground)
                {
                    Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, bgColor, bgColor2, LinearGradientMode.Vertical));
                    Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
                }
            }
            else
            {
                borderColor = Global.Skin.Button.BorderColor;
                textColor = Color.FromArgb(75, Global.Skin.Button.TextColor);

                if (DrawBackground) Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
            }

            g.DrawString(Icon, Skin.IconFont, new SolidBrush(textColor), bounds.X + 1, bounds.Y + 1);
            g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), rf, sf);

            base.OnDraw(g, bounds);
        }
        #endregion
    }
    #endregion

    #region CheckBox
    public class CheckBox : Control
    {
        #region Events
        public event EventHandler CheckedChanged;
        #endregion

        #region Objects
        private bool p_Checked;
        #endregion

        #region Properties
        public bool Checked
        {
            get { return p_Checked; }
            set
            {
                p_Checked = value;
                OnCheckedChanged(new EventArgs());
                ReDraw();
            }
        }
        #endregion

        #region Constructors/Destructor
        public CheckBox(Window owner, Control parent, string text) : base(owner, parent)
        {
            DrawFocusRect = false;
            Text = text;

            p_Checked = false;
        }

        public CheckBox(Window owner, Control parent, string text, string toolTipText) : base(owner, parent)
        {
            DrawFocusRect = false;
            Text = text;
            ToolTipText = toolTipText;

            p_Checked = false;
        }

        public override void Dispose()
        {
            p_Checked = false;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            int y = bounds.Y + (bounds.Height / 2);
            Rectangle r = new Rectangle(bounds.X, y - 7, 11, 11);
            Font f = new Font(Skin.IconFont.Name, 14.0f, FontStyle.Regular);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Color borderColor = Global.Skin.CheckBox.BorderColor;
            Color textColor = Global.Skin.CheckBox.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    borderColor = Global.Skin.CheckBox.Down_BorderColor;
                    textColor = Global.Skin.CheckBox.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    borderColor = Global.Skin.CheckBox.Over_BorderColor;
                    textColor = Global.Skin.CheckBox.Over_TextColor;
                }
            }
            else textColor = Color.FromArgb(75, Global.Skin.CheckBox.TextColor);

            if (p_Checked) g.DrawString("a", f, new SolidBrush(textColor), bounds.X - 6, y - 14);
            Global.DrawRoundedRectangle(g, r, new Pen(borderColor));

            g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 16, bounds.Y, bounds.Width - 20, bounds.Height - 1), sf);

            base.OnDraw(g, bounds);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) p_Checked = !p_Checked;

            base.OnMouseClick(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (CheckedChanged != null) CheckedChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ColorGroup
    public class ColorGroup
    {
        #region Objects
        private Color p_BGColor;
        private Color p_BGColor2;
        private Color p_BorderColor;
        private Color p_BorderColor2;
        private Color p_LineColor;
        private Color p_TextColor;
        private Color p_Down_BGColor;
        private Color p_Down_BGColor2;
        private Color p_Down_BorderColor;
        private Color p_Down_TextColor;
        private Color p_Over_BGColor;
        private Color p_Over_BGColor2;
        private Color p_Over_BorderColor;
        private Color p_Over_TextColor;
        private Color p_Play_BGColor;
        private Color p_Play_TextColor;
        private Color p_Stop_BGColor;
        private Color p_Stop_TextColor;
        #endregion

        #region Properties
        public Color BGColor
        {
            get { return p_BGColor; }
            set { p_BGColor = value; }
        }

        public Color BGColor2
        {
            get { return p_BGColor2; }
            set { p_BGColor2 = value; }
        }

        public Color BorderColor
        {
            get { return p_BorderColor; }
            set { p_BorderColor = value; }
        }

        public Color BorderColor2
        {
            get { return p_BorderColor2; }
            set { p_BorderColor2 = value; }
        }

        public Color LineColor
        {
            get { return p_LineColor; }
            set { p_LineColor = value; }
        }

        public Color TextColor
        {
            get { return p_TextColor; }
            set { p_TextColor = value; }
        }

        public Color Down_BGColor
        {
            get { return p_Down_BGColor; }
            set { p_Down_BGColor = value; }
        }

        public Color Down_BGColor2
        {
            get { return p_Down_BGColor2; }
            set { p_Down_BGColor2 = value; }
        }

        public Color Down_BorderColor
        {
            get { return p_Down_BorderColor; }
            set { p_Down_BorderColor = value; }
        }

        public Color Down_TextColor
        {
            get { return p_Down_TextColor; }
            set { p_Down_TextColor = value; }
        }

        public Color Over_BGColor
        {
            get { return p_Over_BGColor; }
            set { p_Over_BGColor = value; }
        }

        public Color Over_BGColor2
        {
            get { return p_Over_BGColor2; }
            set { p_Over_BGColor2 = value; }
        }

        public Color Over_BorderColor
        {
            get { return p_Over_BorderColor; }
            set { p_Over_BorderColor = value; }
        }

        public Color Over_TextColor
        {
            get { return p_Over_TextColor; }
            set { p_Over_TextColor = value; }
        }

        public Color Play_BGColor
        {
            get { return p_Play_BGColor; }
            set { p_Play_BGColor = value; }
        }

        public Color Play_TextColor
        {
            get { return p_Play_TextColor; }
            set { p_Play_TextColor = value; }
        }

        public Color Stop_BGColor
        {
            get { return p_Stop_BGColor; }
            set { p_Stop_BGColor = value; }
        }

        public Color Stop_TextColor
        {
            get { return p_Stop_TextColor; }
            set { p_Stop_TextColor = value; }
        }
        #endregion

        #region Constructors/Destructor
        public ColorGroup()
        {
            p_BGColor = Color.White;
            p_BGColor2 = Color.White;
            p_BorderColor = Color.Black;
            p_BorderColor2 = Color.Black;
            p_LineColor = Color.White;
            p_TextColor = Color.Black;
            p_Down_BGColor = Color.White;
            p_Down_BGColor2 = Color.White;
            p_Down_BorderColor = Color.Black;
            p_Down_TextColor = Color.Black;
            p_Over_BGColor = Color.White;
            p_Over_BGColor2 = Color.White;
            p_Over_BorderColor = Color.Black;
            p_Over_TextColor = Color.Black;
            p_Play_BGColor = Color.White;
            p_Play_TextColor = Color.Black;
            p_Stop_BGColor = Color.White;
            p_Stop_TextColor = Color.Black;
        }

        public ColorGroup(XmlNode node)
        {
            p_BGColor = Color.Transparent;
            p_BGColor2 = Color.Transparent;
            p_BorderColor = Color.Transparent;
            p_BorderColor2 = Color.Transparent;
            p_LineColor = Color.Transparent;
            p_TextColor = Color.Transparent;
            p_Down_BGColor = Color.Transparent;
            p_Down_BGColor2 = Color.Transparent;
            p_Down_BorderColor = Color.Transparent;
            p_Down_TextColor = Color.Transparent;
            p_Over_BGColor = Color.Transparent;
            p_Over_BGColor2 = Color.Transparent;
            p_Over_BorderColor = Color.Transparent;
            p_Over_TextColor = Color.Transparent;
            p_Play_BGColor = Color.Transparent;
            p_Play_TextColor = Color.Transparent;
            p_Stop_BGColor = Color.Transparent;
            p_Stop_TextColor = Color.Transparent;

            if (node != null)
            {
                p_BGColor = GetColor(node.Attributes["BGColor"]);
                p_BGColor2 = GetColor(node.Attributes["BGColor2"]);
                p_BorderColor = GetColor(node.Attributes["BorderColor"]);
                p_BorderColor2 = GetColor(node.Attributes["BorderColor2"]);
                p_LineColor = GetColor(node.Attributes["LineColor"]);
                p_TextColor = GetColor(node.Attributes["TextColor"]);

                XmlNode node2 = node.SelectSingleNode("Down");
                if (node2 != null)
                {
                    p_Down_BGColor = GetColor(node2.Attributes["BGColor"]);
                    p_Down_BGColor2 = GetColor(node2.Attributes["BGColor2"]);
                    p_Down_BorderColor = GetColor(node2.Attributes["BorderColor"]);
                    p_Down_TextColor = GetColor(node2.Attributes["TextColor"]);
                }

                node2 = node.SelectSingleNode("Over");
                if (node2 != null)
                {
                    p_Over_BGColor = GetColor(node2.Attributes["BGColor"]);
                    p_Over_BGColor2 = GetColor(node2.Attributes["BGColor2"]);
                    p_Over_BorderColor = GetColor(node2.Attributes["BorderColor"]);
                    p_Over_TextColor = GetColor(node2.Attributes["TextColor"]);
                }

                node2 = node.SelectSingleNode("Play");
                if (node2 != null)
                {
                    p_Play_BGColor = GetColor(node2.Attributes["BGColor"]);
                    p_Play_TextColor = GetColor(node2.Attributes["TextColor"]);
                }

                node2 = node.SelectSingleNode("Stop");
                if (node2 != null)
                {
                    p_Stop_BGColor = GetColor(node2.Attributes["BGColor"]);
                    p_Stop_TextColor = GetColor(node2.Attributes["TextColor"]);
                }
            }
        }

        public void Dispose()
        {
            p_Stop_TextColor = Color.Black;
            p_Stop_BGColor = Color.Black;
            p_Play_TextColor = Color.Black;
            p_Play_BGColor = Color.Black;
            p_Over_TextColor = Color.Black;
            p_Over_BorderColor = Color.Black;
            p_Over_BGColor2 = Color.Black;
            p_Over_BGColor = Color.Black;
            p_Down_TextColor = Color.Black;
            p_Down_BorderColor = Color.Black;
            p_Down_BGColor2 = Color.Black;
            p_Down_BGColor = Color.Black;
            p_TextColor = Color.Black;
            p_LineColor = Color.Black;
            p_BorderColor2 = Color.Black;
            p_BorderColor = Color.Black;
            p_BGColor2 = Color.Black;
            p_BGColor = Color.Black;
        }
        #endregion

        #region Public Methods
        public string ToUrl(string prefix)
        {
            string result = ToUrl(p_BGColor, prefix + "_bg");
            result += ToUrl(p_BGColor2, prefix + "_bg2");
            result += ToUrl(p_BorderColor, prefix + "_border");
            result += ToUrl(p_BorderColor2, prefix + "_border2");
            result += ToUrl(p_LineColor, prefix + "_line");
            result += ToUrl(p_TextColor, prefix + "_text");
            result += ToUrl(p_Down_BGColor, prefix + "_down_bg");
            result += ToUrl(p_Down_BGColor2, prefix + "_down_bg2");
            result += ToUrl(p_Down_BorderColor, prefix + "_down_border");
            result += ToUrl(p_Down_TextColor, prefix + "_down_text");
            result += ToUrl(p_Over_BGColor, prefix + "_over_bg");
            result += ToUrl(p_Over_BGColor2, prefix + "_over_bg2");
            result += ToUrl(p_Over_BorderColor, prefix + "_over_border");
            result += ToUrl(p_Over_TextColor, prefix + "_over_text");
            result += ToUrl(p_Play_BGColor, prefix + "_play_bg");
            result += ToUrl(p_Play_TextColor, prefix + "_play_text");
            result += ToUrl(p_Stop_BGColor, prefix + "_stop_bg");
            result += ToUrl(p_Stop_TextColor, prefix + "_stop_text");

            return result;
        }
        #endregion

        #region Private Methods
        private Color GetColor(XmlAttribute attribute)
        {
            if (attribute == null) return Color.Transparent;

            return Global.GetColor(attribute.InnerText);
        }

        private string ToUrl(Color c, string attribute)
        {
            if (c != Color.Transparent) return "&" + attribute + "=" + Global.ColorToHex(c);
            return string.Empty;
        }
        #endregion
    }
    #endregion

    #region ColorDropDown
    public class ColorDropDown : Control
    {
        #region Events
        public event EventHandler ColorChanged;
        #endregion

        #region Objects
        private Color p_Color;
        private ColorPopup p_ColorPopup;
        private Menu p_ContextMenu;
        #endregion

        #region Properties
        public Color Color
        {
            get { return p_Color; }
            set
            {
                p_Color = value;
                OnColorChanged(new EventArgs());
                ReDraw();
            }
        }

        public ColorPopup ColorPopup
        {
            get { return p_ColorPopup; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }
        #endregion

        #region Constructors/Destructor
        public ColorDropDown(Window owner, Control parent, Color color) : base(owner, parent)
        {
            p_Color = color;

            p_ColorPopup = new ColorPopup(this);
            p_ColorPopup.ColorChanged += new EventHandler(p_ColorPopup_ColorChanged);

            p_ContextMenu = new Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(string.Empty, "Copy"), new MenuItem(string.Empty, "Paste") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public override void Dispose()
        {
            p_ContextMenu.Dispose();
            p_ColorPopup.Dispose();
            p_Color = Color.Black;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            RectangleF rf = new RectangleF(bounds.X + 2, bounds.Y, bounds.Width - 20, bounds.Height);

            StringFormat sf = new StringFormat();
            if (RightToLeft) sf.Alignment = StringAlignment.Far;
            else sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Color borderColor = Global.Skin.Button.BorderColor;
            Color textColor = Global.GetContrastColor(p_Color);

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    borderColor = Global.Skin.Button.Down_BorderColor;
                    //textColor = Global.Skin.Button.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    borderColor = Global.Skin.Button.Over_BorderColor;
                    //textColor = Global.Skin.Button.Over_TextColor;
                }
            }
            else
            {
                borderColor = Global.Skin.Button.BorderColor;
                textColor = Color.FromArgb(75, Global.Skin.Button.TextColor);
            }

            if (DrawBackground)
            {
                Global.FillRoundedRectangle(g, r, new SolidBrush(p_Color));
                Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
            }

            g.DrawString(Global.ColorToHex(p_Color), Skin.WindowFont, new SolidBrush(textColor), rf, sf);
            g.DrawString("6", Skin.IconFont, new SolidBrush(textColor), (bounds.X + bounds.Width) - 18, bounds.Y);

            base.OnDraw(g, bounds);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) p_ColorPopup.Show();

            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_ColorPopup_ColorChanged(object sender, EventArgs e)
        {
            Color = p_ColorPopup.Slider.Color;
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Copy":
                    Clipboard.SetText(Global.ColorToHex(p_Color));
                    break;
                case "Paste":
                    string s = Clipboard.GetText();
                    if (s.StartsWith("#")) s = s.Substring(1);
                    if (!string.IsNullOrEmpty(s) && Global.IsColor("#" + s))
                    {
                        Color = ColorTranslator.FromHtml("#" + s);
                        p_ColorPopup.SetColor(p_Color);
                    }
                    break;
            }
        }
        #endregion

        #region Public Methods
        public void SetColor(Color color)
        {
            p_Color = color;
            p_ColorPopup.SetColor(color);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnColorChanged(EventArgs e)
        {
            if (ColorChanged != null) ColorChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ColorDropDownGroup
    public class ColorDropDownGroup : ControlGroup
    {
        #region Events
        public event EventHandler ColorChanged;
        #endregion

        #region Objects
        private Label p_Label;
        private ColorDropDown p_DropDown;
        #endregion

        #region Properties
        public Label Label
        {
            get { return p_Label; }
        }

        public ColorDropDown DropDown
        {
            get { return p_DropDown; }
        }

        public override Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;

                p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                p_DropDown.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 146, base.Bounds.Height);

                OnBoundsChanged(new EventArgs());
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_Label.Enabled = base.Enabled;
                p_DropDown.Enabled = base.Enabled;

                OnEnabledChanged(new EventArgs());
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_Label.Visible = base.Visible;
                p_DropDown.Visible = base.Visible;

                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public ColorDropDownGroup(Window owner, Control parent, string text, Color color) : base()
        {
            p_Label = new Label(owner, parent, text);
            p_Label.RightToLeft = true;

            p_DropDown = new ColorDropDown(owner, parent, color);
            p_DropDown.ColorChanged += new EventHandler(p_DropDown_ColorChanged);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Child Events
        private void p_DropDown_ColorChanged(object sender, EventArgs e)
        {
            OnColorChanged(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnColorChanged(EventArgs e)
        {
            if (ColorChanged != null) ColorChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ColorPicker
    public class ColorPicker : Control
    {
        #region Events
        public event EventHandler ColorChanged;
        public event EventHandler PointChanged;
        #endregion

        #region Objects
        private Bitmap p_Colors;
        private Color p_Color;
        private Point p_Point;
        #endregion

        #region Properties
        public Bitmap Colors
        {
            get { return p_Colors; }
        }

        public Color Color
        {
            get { return p_Color; }
            set
            {
                p_Color = value;
                OnColorChanged(new EventArgs());
                ReDraw();
            }
        }

        public Point Point
        {
            get { return p_Point; }
            set
            {
                if (value.X < 0) value.X = 0;
                if (value.Y < 0) value.Y = 0;
                
                if (value.X > Properties.Resources.Colors.Width - 1) value.X = Properties.Resources.Colors.Width - 1;
                if (value.Y > Properties.Resources.Colors.Height - 1) value.Y = Properties.Resources.Colors.Height - 1;

                p_Point = value;
                OnPointChanged(new EventArgs());
                ReDraw();
            }
        }
        #endregion

        #region Constructor/Destructor
        public ColorPicker(Window owner, Control parent) : base(owner, parent)
        {
            Cursor = Cursors.Cross;

            p_Colors = Properties.Resources.Colors;
            p_Color = Color.Red;
            p_Point = new Point(0, 0);
        }

        public ColorPicker(Window owner, Control parent, Color color) : base(owner, parent)
        {
            Cursor = Cursors.Cross;

            p_Colors = Properties.Resources.Colors;
            p_Color = color;
            p_Point = new Point(0, 0);
        }

        public override void Dispose()
        {
            p_Point = new Point(0, 0);
            p_Color = Color.Black;
            p_Colors.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            g.DrawImage(p_Colors, bounds.X, bounds.Y, p_Colors.Width, p_Colors.Height);
            g.DrawRectangle(new Pen(Global.Skin.List.BorderColor), bounds.X - 1, bounds.Y - 1, bounds.Width, bounds.Height);

            g.DrawLine(Pens.Black, (bounds.X + p_Point.X) - 5, bounds.Y + p_Point.Y, (bounds.X + p_Point.X) + 5, bounds.Y + p_Point.Y);
            g.DrawLine(Pens.Black, bounds.X + p_Point.X, (bounds.Y + p_Point.Y) - 5, bounds.X + p_Point.X, (bounds.Y + p_Point.Y) + 5);

            base.OnDraw(g, bounds);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Point = new Point(e.X - Bounds.X, e.Y - Bounds.Y);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Point = new Point(e.X - Bounds.X, e.Y - Bounds.Y);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Color = Properties.Resources.Colors.GetPixel(p_Point.X, p_Point.Y);

            base.OnMouseUp(e);
        }
        #endregion

        #region Public Methods
        public void SetColor(Color color)
        {
            double highHue, highSat, highVal;
            double colorsHue, colorsSat, colorsVal;

            Global.ColorToHSV(color, out highHue, out highSat, out highVal);
            highVal = 1.0d;

            for (int y = 0; y < p_Colors.Height; y++)
            {
                for (int x = 0; x < p_Colors.Width; x++)
                {
                    Color c = p_Colors.GetPixel(x, y);
                    Global.ColorToHSV(c, out colorsHue, out colorsSat, out colorsVal);
                    colorsVal = 1.0d;

                    if (highHue >= colorsHue - 0.01d && highHue <= colorsHue + 0.01d)
                    {
                        if (highSat >= colorsSat - 0.01d && highSat <= colorsSat + 0.01d)
                        {
                            p_Point = new Point(x, y);
                            return;
                        }
                    }
                }
            }

            for (int y = 0; y < p_Colors.Height; y++)
            {
                for (int x = 0; x < p_Colors.Width; x++)
                {
                    Color c = p_Colors.GetPixel(x, y);
                    Global.ColorToHSV(c, out colorsHue, out colorsSat, out colorsVal);
                    colorsVal = 1.0d;

                    if (highHue >= colorsHue - 0.1d && highHue <= colorsHue + 0.1d)
                    {
                        if (highSat >= colorsSat - 0.1d && highSat <= colorsSat + 0.1d)
                        {
                            p_Point = new Point(x, y);
                            return;
                        }
                    }
                }
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnColorChanged(EventArgs e)
        {
            if (ColorChanged != null) ColorChanged.Invoke(this, e);
        }

        protected virtual void OnPointChanged(EventArgs e)
        {
            if (PointChanged != null) PointChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ColorPopup
    public class ColorPopup : Popup
    {
        #region Events
        public event EventHandler ColorChanged;
        #endregion

        #region Objects
        private Control p_Parent;
        private ColorPicker p_Picker;
        private ColorSlider p_Slider;
        #endregion

        #region Properties
        public ColorPicker Picker
        {
            get { return p_Picker; }
        }

        public ColorSlider Slider
        {
            get { return p_Slider; }
        }
        #endregion

        #region Constructor/Destructor
        public ColorPopup(Control parent) : base()
        {
            Sizable = false;
            Size = new Size(246, 126);

            p_Parent = parent;

            p_Picker = new ColorPicker(this, null);
            p_Picker.ColorChanged += new EventHandler(p_Picker_ColorChanged);
            p_Picker.DrawFocusRect = false;
            p_Picker.SetBounds(12, 12, 182, 102);

            p_Slider = new ColorSlider(this, null);
            p_Slider.ColorChanged += new EventHandler(p_Slider_ColorChanged);
            p_Slider.DrawFocusRect = false;
            p_Slider.SetBounds(210, 12, 20, 102);
        }

        ~ColorPopup()
        {

        }
        #endregion

        #region Child Events
        private void p_Picker_ColorChanged(object sender, EventArgs e)
        {
            p_Slider.TopColor = p_Picker.Color;
        }

        private void p_Slider_ColorChanged(object sender, EventArgs e)
        {
            OnColorChanged(new EventArgs());
        }
        #endregion

        #region Public Methods
        public void SetColor(Color color)
        {
            p_Picker.SetColor(color);
            p_Slider.SetColor(color);
        }

        public new void Show()
        {
            if (p_Parent != null)
            {
                Location = p_Parent.Owner.PointToScreen(new Point(p_Parent.Bounds.X, p_Parent.Bounds.Y + p_Parent.Bounds.Height));
                //if (p_Parent.Bounds.Width == 20) Size = new Size(80, (p_Items.Count * p_ItemHeight) + 6);
                //else Size = new Size(p_Parent.Bounds.Width, (p_Items.Count * p_ItemHeight) + 6);

                if (Left > Screen.PrimaryScreen.Bounds.Width - Width) Left = Cursor.Position.X - Width;
                if (Top > Screen.PrimaryScreen.Bounds.Height - Height) Top = Cursor.Position.Y - Height;

                if (Handle.Equals(IntPtr.Zero)) base.CreateControl();
                Global.SetParent(Handle, IntPtr.Zero);
                Global.ShowWindow(Handle, 1);
                Global.SetForegroundWindow(Handle);
            }
            else
            {
                Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                //Size = new Size(140, (p_Items.Count * p_ItemHeight) + 6);

                if (Left > Screen.PrimaryScreen.Bounds.Width - Width) Left = Cursor.Position.X - Width;
                if (Top > Screen.PrimaryScreen.Bounds.Height - Height) Top = Cursor.Position.Y - Height;

                if (Handle.Equals(IntPtr.Zero)) base.CreateControl();
                Global.SetParent(Handle, IntPtr.Zero);
                Global.ShowWindow(Handle, 1);
                Global.SetForegroundWindow(Handle);
            }
        }

        public new void Hide()
        {
            if (!Handle.Equals(IntPtr.Zero)) Global.ShowWindow(Handle, 0);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnColorChanged(EventArgs e)
        {
            if (ColorChanged != null) ColorChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ColorSlider
    public class ColorSlider : Control
    {
        #region Events
        public event EventHandler ColorChanged;
        public event EventHandler TopColorChanged;
        public event EventHandler ValueChanged;
        #endregion

        #region Objects
        private Color p_Color, p_TopColor;
        private int p_Value;
        private Bitmap p_Bitmap;
        #endregion

        #region Properties
        public Color Color
        {
            get { return p_Color; }
            set
            {
                p_Color = value;
                OnColorChanged(new EventArgs());
                ReDraw();
            }
        }
        
        public Color TopColor
        {
            get { return p_TopColor; }
            set
            {
                p_TopColor = value;
                UpdateBitmap();
                UpdateColor();
                OnTopColorChanged(new EventArgs());
                ReDraw();
            }
        }

        public int Value
        {
            get { return p_Value; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                p_Value = value;
                //UpdateColor();
                OnValueChanged(new EventArgs());
                ReDraw();
            }
        }
        #endregion

        #region Constructor/Destructor
        public ColorSlider(Window owner, Control parent) : base(owner, parent)
        {
            p_Color = Color.Red;
            p_TopColor = Color.Red;
            p_Value = 0;
            p_Bitmap = null;
        }

        public override void Dispose()
        {
            p_Bitmap.Dispose();
            p_Value = 0;
            p_TopColor = Color.Black;
            p_Color = Color.Black;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            g.DrawImage(p_Bitmap, bounds.X, bounds.Y, p_Bitmap.Width, p_Bitmap.Height);
            Global.DrawRoundedRectangle(g, new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), new Pen(Global.Skin.List.BorderColor));

            g.DrawString("4", Skin.IconFont, new SolidBrush(Global.Skin.Window.TextColor), bounds.X - 12, (bounds.Y + p_Value) - 8);
            g.DrawString("3", Skin.IconFont, new SolidBrush(Global.Skin.Window.TextColor), (bounds.X + bounds.Width) - 4, (bounds.Y + p_Value) - 8);

            base.OnDraw(g, bounds);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            UpdateBitmap();

            base.OnBoundsChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Value = e.Y - Bounds.Y;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Value = e.Y - Bounds.Y;

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            UpdateColor();

            base.OnMouseUp(e);
        }
        #endregion

        #region Public Methods
        public void SetColor(Color color)
        {
            double hue, sat, val;
            double colorsHue, colorsSat, colorsVal;

            Global.ColorToHSV(color, out hue, out sat, out val);

            p_TopColor = Global.HSVToColor(hue, sat, 1.0d);
            UpdateBitmap();

            if (val == 1.0d) p_Value = 0;
            else if (val == 0.0d) p_Value = 100;
            else
            {
                for (int y = 0; y < p_Bitmap.Height; y++)
                {
                    Color c = p_Bitmap.GetPixel(2, y);
                    Global.ColorToHSV(c, out colorsHue, out colorsSat, out colorsVal);

                    if (val >= colorsVal - 0.01d && val <= colorsVal + 0.01d)
                    {
                        p_Value = y;
                        return;
                    }
                }

                for (int y = 0; y < p_Bitmap.Height; y++)
                {
                    Color c = p_Bitmap.GetPixel(2, y);
                    Global.ColorToHSV(c, out colorsHue, out colorsSat, out colorsVal);

                    if (val >= colorsVal - 0.1d && val <= colorsVal + 0.1d)
                    {
                        p_Value = y;
                        return;
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void UpdateBitmap()
        {
            p_Bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            Graphics g = Graphics.FromImage(p_Bitmap);
            Rectangle r = new Rectangle(0, 0, Bounds.Width - 1, Bounds.Height - 1);

            Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, p_TopColor, Color.Black, LinearGradientMode.Vertical));
        }

        private void UpdateColor()
        {
            if (p_Value <= 2) Color = p_TopColor;
            else if (p_Value >= 98) Color = Color.Black;
            else Color = p_Bitmap.GetPixel(0, p_Value);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnColorChanged(EventArgs e)
        {
            if (ColorChanged != null) ColorChanged.Invoke(this, e);
        }
        
        protected virtual void OnTopColorChanged(EventArgs e)
        {
            if (TopColorChanged != null) TopColorChanged.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null) ValueChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Command
    public class Command : GenericListItem
    {
        #region Enums
        public enum CommandTypes : int
        {
            Global = 0,
            Media = 1,
            Steam = 2,
            Web = 3,
            Misc = 4,
        }
        #endregion

        #region Objects
        private string p_ID, p_Param, p_Description;
        private CommandTypes p_CommandType;
        private bool p_New;
        #endregion

        #region Properties
        public string ID
        {
            get { return p_ID; }
        }

        public string Param
        {
            get { return p_Param; }
        }

        public string Description
        {
            get { return p_Description; }
        }

        public CommandTypes CommandType
        {
            get { return p_CommandType; }
        }

        public bool New
        {
            get { return p_New; }
        }
        #endregion

        #region Constructor/Destructor
        public Command(XmlNode node) : base()
        {
            if (node != null)
            {
                p_ID = Global.GetXmlValue(node, "ID", string.Empty);
                p_Param = Global.GetXmlValue(node, "Param", string.Empty);
                p_Description = Global.GetXmlValue(node, "Desc", string.Empty);
                p_CommandType = (CommandTypes)Global.StringToInt(Global.GetXmlValue(node, "Type", string.Empty));
                p_New = Global.StringToBool(Global.GetXmlValue(node, "New", "0"), "1");
            }
            else
            {
                p_ID = string.Empty;
                p_Param = string.Empty;
                p_Description = string.Empty;
                p_CommandType = CommandTypes.Misc;
                p_New = false;
            }
        }

        public override void Dispose()
        {
            p_New = false;
            p_CommandType = CommandTypes.Global;
            p_Description = string.Empty;
            p_Param = string.Empty;
            p_ID = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_ID + " " + p_Param;
        }
        #endregion
    }
    #endregion

    #region CommandList
    public class CommandList : GenericList<Command>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllCommands,
            GlobalCommands,
            MediaCommands,
            SteamCommands,
            WebCommands,
            MiscCommands,
            NewCommands,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public CommandList(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Commands", new string[] { "All Commands (0)", "Global Commands (0)", "Media Commands (0)", "Steam Commands (0)", "Web Commands (0)", "Misc. Commands (0)", "New Commands (0)" }, "Filter Commands");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("q", "Refresh Commands") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            p_WebClient = new WebClient();
            p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);
        }

        public override void Dispose()
        {
            p_WebClient.Dispose();
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int x2 = x / 3;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing commands, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No commands found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    if (string.IsNullOrEmpty(FilteredItems[i].Param)) g.DrawString(Global.Settings.Trigger + FilteredItems[i].ID, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x2, ItemHeight), sf);
                    else g.DrawString(Global.Settings.Trigger + FilteredItems[i].ID + " {" + FilteredItems[i].Param + "}", Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x2, ItemHeight), sf);

                    g.DrawString(FormatCommand(FilteredItems[i]), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + 3) + x2, (bounds.Y + y) + 3, (x2 * 2) - 30, ItemHeight), sf);
                    if (FilteredItems[i].New) g.DrawString("New!", Skin.WindowFont, new SolidBrush(Global.Skin.List.Play_TextColor), new RectangleF((bounds.X + x) - 30, (bounds.Y + y) + 3, 30, ItemHeight), sf);

                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<Command> items = new List<Command>();

            foreach (Command item in Items)
            {
                if (item.ID.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllCommands:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.GlobalCommands:
                    foreach (Command item in items)
                    {
                        if (item.CommandType == Command.CommandTypes.Global) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.MediaCommands:
                    foreach (Command item in items)
                    {
                        if (item.CommandType == Command.CommandTypes.Media) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.SteamCommands:
                    foreach (Command item in items)
                    {
                        if (item.CommandType == Command.CommandTypes.Steam) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.WebCommands:
                    foreach (Command item in items)
                    {
                        if (item.CommandType == Command.CommandTypes.Web) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.MiscCommands:
                    foreach (Command item in items)
                    {
                        if (item.CommandType == Command.CommandTypes.Misc) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.NewCommands:
                    foreach (Command item in items)
                    {
                        if (item.New) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new GenericListSorter<Command>(GenericListSorter<Command>.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Commands (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Global Commands (" + Global.FormatNumber(GetGlobalCommandsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "Media Commands (" + Global.FormatNumber(GetMediaCommandsCount()) + ")";
            p_FilterBar.DropDown.Items[3] = "Steam Commands (" + Global.FormatNumber(GetSteamCommandsCount()) + ")";
            p_FilterBar.DropDown.Items[4] = "Web Commands (" + Global.FormatNumber(GetWebCommandsCount()) + ")";
            p_FilterBar.DropDown.Items[5] = "Misc. Commands (" + Global.FormatNumber(GetMiscCommandsCount()) + ")";
            p_FilterBar.DropDown.Items[6] = "New Commands (" + Global.FormatNumber(GetNewCommandsCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Refresh Commands":
                    Refresh();
                    break;
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    List<Command> list = new List<Command>();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(e.Result);
                    XmlNodeList nodeList = xml.SelectNodes("Steamp3.Commands/Command");

                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(new UI.Command(node));
                    }

                    Items.Clear();
                    AddRange(list);

                    list.Clear();
                }
                catch
                {
                }

                Refreshing = false;
            }
        }
        #endregion

        #region Public Methods
        public int GetGlobalCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.CommandType == Command.CommandTypes.Global) result++;
            }

            return result;
        }

        public int GetMediaCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.CommandType == Command.CommandTypes.Media) result++;
            }

            return result;
        }

        public int GetSteamCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.CommandType == Command.CommandTypes.Steam) result++;
            }

            return result;
        }

        public int GetWebCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.CommandType == Command.CommandTypes.Web) result++;
            }

            return result;
        }

        public int GetMiscCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.CommandType == Command.CommandTypes.Misc) result++;
            }

            return result;
        }

        public int GetNewCommandsCount()
        {
            int result = 0;

            foreach (Command item in Items)
            {
                if (item.New) result++;
            }

            return result;
        }

        public void Refresh()
        {
            if (!Global.MediaPlayer.IsOnline) return;

            Refreshing = true;

            p_WebClient.DownloadStringAsync(new Uri("http://steamp3.ta0soft.com/commands/commands.xml"));
        }
        #endregion

        #region Private Methods
        private string FormatCommand(Command command)
        {
            string result = command.Description.Replace("{id}", command.ID);
            result = result.Replace("{param}", "{" + command.Param + "}");

            return result;
        }
        #endregion
    }
    #endregion

    #region Control
    public class Control
    {
        #region Delegates
        public delegate void DrawEventHandler(Graphics g, Rectangle bounds);
        #endregion

        #region Callbacks
        public delegate void ReDrawCallback();
        #endregion

        #region Events
        public event DrawEventHandler Draw;
        public event EventHandler BoundsChanged;
        public event EventHandler CursorChanged;
        public event EventHandler IconChanged;
        public event EventHandler EnabledChanged;
        public event EventHandler FocusedChanged;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler EnterPressed;
        public event KeyPressEventHandler KeyPress;
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;
        public event EventHandler TabIndexChanged;
        public event EventHandler TagChanged;
        public event EventHandler TextChanged;
        public event EventHandler ToolTipTextChanged;
        public event EventHandler DrawBackgroundChanged;
        public event EventHandler DrawDisabledChanged;
        public event EventHandler DrawFocusRectChanged;
        public event EventHandler RightToLeftChanged;
        public event EventHandler VisibleChanged;
        #endregion

        #region Objects
        private Window p_Owner;
        private Control p_Parent;
        private List<Control> p_Controls;
        private Rectangle p_Bounds;
        private int p_Clicks, p_StartTime;
        private Cursor p_Cursor;
        private string p_Icon;
        private bool p_Enabled, p_Focused, p_IsMouseDown, p_IsMouseMoving;
        private int p_TabIndex;
        private object p_Tag;
        private string p_Text, p_ToolTipText;
        private bool p_DrawBackground, p_DrawDisabled, p_DrawFocusRect, p_RightToLeft, p_Visible;
        #endregion

        #region Properties
        public Window Owner
        {
            get { return p_Owner; }
            set { p_Owner = value; }
        }

        public Control Parent
        {
            get { return p_Parent; }
            set { p_Parent = value; }
        }

        public List<Control> Controls
        {
            get { return p_Controls; }
        }

        public Rectangle Bounds
        {
            get { return p_Bounds; }
            set
            {
                p_Bounds = value;
                OnBoundsChanged(new EventArgs());
            }
        }

        public virtual int Clicks
        {
            get { return p_Clicks; }
        }

        public Cursor Cursor
        {
            get { return p_Cursor; }
            set
            {
                p_Cursor = value;
                OnCursorChanged(new EventArgs());
            }
        }

        public string Icon
        {
            get { return p_Icon; }
            set
            {
                p_Icon = value;
                OnIconChanged(new EventArgs());
                ReDraw();
            }
        }

        public virtual bool Enabled
        {
            get { return p_Enabled; }
            set
            {
                p_Enabled = value;
                if (!p_Enabled && p_Focused) p_Focused = false;
                OnEnabledChanged(new EventArgs());
                ReDraw();
            }
        }

        public bool Focused
        {
            get { return p_Focused; }
            set
            {
                p_Focused = value;
                OnFocusedChanged(new EventArgs());
                ReDraw();
            }
        }

        public bool IsMouseDown
        {
            get { return p_IsMouseDown; }
        }

        public bool IsMouseMoving
        {
            get { return p_IsMouseMoving; }
        }

        public int TabIndex
        {
            get { return p_TabIndex; }
            set
            {
                p_TabIndex = value;
                OnTabIndexChanged(new EventArgs());
            }
        }

        public object Tag
        {
            get { return p_Tag; }
            set
            {
                p_Tag = value;
                OnTagChanged(new EventArgs());
            }
        }

        public string Text
        {
            get { return p_Text; }
            set
            {
                p_Text = value;
                OnTextChanged(new EventArgs());
                ReDraw();
            }
        }

        public string ToolTipText
        {
            get { return p_ToolTipText; }
            set
            {
                p_ToolTipText = value;
                OnToolTipTextChanged(new EventArgs());
            }
        }

        public virtual bool DrawBackground
        {
            get { return p_DrawBackground; }
            set
            {
                p_DrawBackground = value;
                OnDrawBackgroundChanged(new EventArgs());
            }
        }

        public virtual bool DrawDisabled
        {
            get { return p_DrawDisabled; }
            set
            {
                p_DrawDisabled = value;
                OnDrawDisabledChanged(new EventArgs());
            }
        }

        public virtual bool DrawFocusRect
        {
            get { return p_DrawFocusRect; }
            set
            {
                p_DrawFocusRect = value;
                OnDrawFocusRectChanged(new EventArgs());
            }
        }

        public virtual bool RightToLeft
        {
            get { return p_RightToLeft; }
            set
            {
                p_RightToLeft = value;
                OnRightToLeftChanged(new EventArgs());
            }
        }

        public virtual bool Visible
        {
            get { return p_Visible; }
            set
            {
                p_Visible = value;
                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public Control(Window owner, Control parent)
        {
            if (owner != null) owner.Controls.Add(this);
            Owner = owner;
            if (parent != null) parent.Controls.Add(this);
            Parent = parent;

            p_Controls = new List<Control>();
            p_Bounds = new Rectangle(0, 0, 0, 0);
            p_Clicks = 0;
            p_StartTime = 0;
            p_Cursor = Cursors.Default;
            p_Icon = string.Empty;
            p_Enabled = true;
            p_Focused = false;
            p_IsMouseDown = false;
            p_IsMouseMoving = false;
            p_TabIndex = 0;
            p_Tag = null;
            p_Text = string.Empty;
            p_ToolTipText = string.Empty;
            p_DrawBackground = true;
            p_DrawDisabled = true;
            p_DrawFocusRect = true;
            p_RightToLeft = false;
            p_Visible = true;

            if (owner != null) p_TabIndex = owner.Controls.Count - 1;
        }

        public virtual void Dispose()
        {
            p_Visible = false;
            p_RightToLeft = false;
            p_DrawFocusRect = false;
            p_DrawDisabled = false;
            p_DrawBackground = false;
            p_ToolTipText = string.Empty;
            p_Text = string.Empty;
            p_Tag = null;
            p_TabIndex = 0;
            p_IsMouseMoving = false;
            p_IsMouseDown = false;
            p_Focused = false;
            p_Enabled = false;
            p_Icon = string.Empty;
            p_Cursor = Cursors.Default;
            p_StartTime = 0;
            p_Clicks = 0;
            p_Bounds = new Rectangle(0, 0, 0, 0);
            p_Controls.Clear(); //?
            p_Parent = null; //?
            p_Owner = null; //?
        }
        #endregion

        #region Public Methods
        public bool KeyDownProc(KeyEventArgs e)
        {
            if (!p_Enabled) return false;
            if (!p_Focused) return false;
            if (!p_Visible) return false;

            OnKeyDown(e);
            //ReDraw();
            return true;
        }

        public bool KeyPressProc(KeyPressEventArgs e)
        {
            if (!p_Focused) return false;
            if (!p_Visible) return false;

            if (e.KeyChar == (char)Keys.Tab)
            {
                Focused = false;

                if (p_Owner != null)
                {
                    int tabIndex = p_TabIndex + 1;
                    if (tabIndex > p_Owner.Controls.Count - 1) tabIndex = 0;

                    foreach (Control control in p_Owner.Controls)
                    {
                        if (control.TabIndex.Equals(tabIndex))
                        {
                            control.Focused = true;
                            return false;
                        }
                    }
                }
            }

            if (!p_Enabled) return false;

            if (e.KeyChar == (char)Keys.Enter)
            {
                OnEnterPressed(e);
                return true;
            }

            OnKeyPress(e);
            //ReDraw();
            return true;
        }

        public bool MouseDownProc(MouseEventArgs e)
        {
            if (HitTest(e))
            {
                if (!p_IsMouseDown && p_IsMouseMoving)
                {
                    p_IsMouseDown = true;

                    OnMouseDown(e);
                    ReDraw();
                    return true;
                }
            }

            return false;
        }

        public bool MouseMoveProc(MouseEventArgs e)
        {
            if (!Owner.Enabled) return false;
            if (Owner.IsMouseDown) return false;

            if (HitTest(e))
            {
                if (e.Button == MouseButtons.None && !p_IsMouseMoving)
                {
                    if (!Owner.Cursor.Equals(p_Cursor)) Owner.Cursor = p_Cursor;
                    p_IsMouseMoving = true;

                    OnMouseEnter(e);
                    ReDraw();
                }
                else if (p_IsMouseDown || p_IsMouseMoving)
                {
                    OnMouseMove(e);
                    //ReDraw();
                }

                return true;
            }
            else
            {
                if (e.Button != MouseButtons.None && p_IsMouseDown)
                {
                    p_IsMouseMoving = false;

                    OnMouseMove(e);
                    //ReDraw();

                    return true;
                }
                else if (p_IsMouseDown)
                {
                    p_IsMouseDown = false;
                    p_IsMouseMoving = false;

                    OnMouseUp(e);
                    ReDraw();

                    return true;
                }
                else if (p_IsMouseMoving)
                {
                    //p_Clicks = 0;
                    //p_StartTime = 0;

                    if (Owner.Cursor.Equals(p_Cursor)) Owner.Cursor = Cursors.Default;
                    p_IsMouseDown = false;
                    p_IsMouseMoving = false;

                    OnMouseLeave(e);
                    ReDraw();

                    return true;
                }
            }

            return false;
        }

        public bool MouseUpProc(MouseEventArgs e)
        {
            if (HitTest(e))
            {
                if (p_IsMouseDown) //&& p_IsMouseMoving) && e.Clicks > 0 && 
                {
                    if (!p_Focused)
                    {
                        foreach (Control control in Owner.Controls)
                        {
                            if (control.Focused) control.Focused = false;
                        }
                        Focused = true;
                    }

                    p_IsMouseDown = false;

                    if (e.Button == MouseButtons.Left)
                    {
                        if (p_Clicks == 2)
                        {
                            p_Clicks = 0;
                            p_StartTime = 0;
                        }

                        if (p_Clicks == 0)
                        {
                            p_Clicks++;
                            p_StartTime = Environment.TickCount;
                        }
                        else
                        {
                            if (Environment.TickCount - p_StartTime < SystemInformation.DoubleClickTime)
                            {
                                p_Clicks++;
                            }
                            else
                            {
                                p_Clicks = 1;
                                p_StartTime = Environment.TickCount;
                            }
                        }

                        OnMouseClick(e); //?
                    }
                    OnMouseUp(e);
                    ReDraw();
                    return true;
                }
            }
            else
            {
                //?
            }

            return false;
        }

        public bool MouseWheelProc(MouseEventArgs e)
        {
            if (p_Focused && e.Delta != 0)
            {
                OnMouseWheel(e);
                //ReDraw();
                return true;
            }

            return false;
        }

        public void ReDraw()
        {
            try
            {
                if (p_Owner == null) return;

                if (p_Owner.InvokeRequired) p_Owner.Invoke(new ReDrawCallback(ReDraw));
                else p_Owner.ReDrawControl(this);
            }
            catch { }
        }
        #endregion

        #region Virtual Methods
        public virtual void OnPaint(Graphics g, Rectangle bounds)
        {
            if (!p_Visible) return;
            if (!p_Enabled && !p_DrawDisabled) return;
            if (bounds.Width < 6 || bounds.Height < 6) return;

            OnDraw(g, bounds);

            if (p_Focused && p_DrawFocusRect) Global.DrawRoundedRectangle(g, new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), new Pen(Global.Skin.Window.TextColor));
        }

        public virtual void OnDraw(Graphics g, Rectangle bounds)
        {
            if (Draw != null) Draw.Invoke(g, bounds);
        }

        public virtual bool HitTest(MouseEventArgs e)
        {
            if (!p_Enabled) return false;
            if (!p_Visible) return false;

            if (p_Bounds.Contains(e.X, e.Y)) return true;
            return false;
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
        }

        protected virtual void OnBoundsChanged(EventArgs e)
        {
            if (BoundsChanged != null) BoundsChanged.Invoke(this, e);
        }

        protected virtual void OnCursorChanged(EventArgs e)
        {
            if (CursorChanged != null) CursorChanged.Invoke(this, e);
        }

        protected virtual void OnIconChanged(EventArgs e)
        {
            if (IconChanged != null) IconChanged.Invoke(this, e);
        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {
            if (EnabledChanged != null) EnabledChanged.Invoke(this, e);
        }

        protected virtual void OnFocusedChanged(EventArgs e)
        {
            if (FocusedChanged != null) FocusedChanged.Invoke(this, e);
        }

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (KeyDown != null) KeyDown.Invoke(this, e);
        }

        protected virtual void OnEnterPressed(KeyPressEventArgs e)
        {
            if (EnterPressed != null) EnterPressed.Invoke(this, e);
        }

        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            if (KeyPress != null) KeyPress.Invoke(this, e);
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (MouseClick != null) MouseClick.Invoke(this, e);
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (Global.ToolTip != null && !string.IsNullOrEmpty(p_ToolTipText)) Global.ToolTip.Hide();

            if (MouseDown != null) MouseDown.Invoke(this, e);
        }

        protected virtual void OnMouseEnter(MouseEventArgs e)
        {
            if (Global.ToolTip != null && !string.IsNullOrEmpty(p_ToolTipText)) Global.ToolTip.Show(p_ToolTipText, 1000);

            if (MouseEnter != null) MouseEnter.Invoke(this, e);
        }

        protected virtual void OnMouseLeave(MouseEventArgs e)
        {
            if (Global.ToolTip != null && !string.IsNullOrEmpty(p_ToolTipText)) Global.ToolTip.Hide();

            if (MouseLeave != null) MouseLeave.Invoke(this, e);
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null) MouseMove.Invoke(this, e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null) MouseUp.Invoke(this, e);
        }

        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheel != null) MouseWheel.Invoke(this, e);
        }

        protected virtual void OnTabIndexChanged(EventArgs e)
        {
            if (TabIndexChanged != null) TabIndexChanged.Invoke(this, e);
        }

        protected virtual void OnTagChanged(EventArgs e)
        {
            if (TagChanged != null) TagChanged.Invoke(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null) TextChanged.Invoke(this, e);
        }

        protected virtual void OnToolTipTextChanged(EventArgs e)
        {
            if (ToolTipTextChanged != null) ToolTipTextChanged.Invoke(this, e);
        }

        protected virtual void OnDrawBackgroundChanged(EventArgs e)
        {
            if (DrawBackgroundChanged != null) DrawBackgroundChanged.Invoke(this, e);
        }

        protected virtual void OnDrawDisabledChanged(EventArgs e)
        {
            if (DrawDisabledChanged != null) DrawDisabledChanged.Invoke(this, e);
        }

        protected virtual void OnDrawFocusRectChanged(EventArgs e)
        {
            if (DrawFocusRectChanged != null) DrawFocusRectChanged.Invoke(this, e);
        }

        protected virtual void OnRightToLeftChanged(EventArgs e)
        {
            if (RightToLeftChanged != null) RightToLeftChanged.Invoke(this, e);
        }

        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null) VisibleChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ControlGroup
    public class ControlGroup
    {
        #region Events
        public event EventHandler BoundsChanged;
        public event EventHandler EnabledChanged;
        public event EventHandler VisibleChanged;
        #endregion

        #region Objects
        private Rectangle p_Bounds;
        private bool p_Enabled, p_Visible;
        #endregion

        #region Properties
        public virtual Rectangle Bounds
        {
            get { return p_Bounds; }
            set
            {
                p_Bounds = value;
                OnBoundsChanged(new EventArgs());
            }
        }

        public virtual bool Enabled
        {
            get { return p_Enabled; }
            set
            {
                p_Enabled = value;
                OnEnabledChanged(new EventArgs());
            }
        }

        public virtual bool Visible
        {
            get { return p_Visible; }
            set
            {
                p_Visible = value;
                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public ControlGroup()
        {
            p_Bounds = new Rectangle(0, 0, 0, 0);
            p_Enabled = true;
            p_Visible = true;
        }

        public virtual void Dispose()
        {
            p_Visible = false;
            p_Enabled = false;
            p_Bounds = new Rectangle(0, 0, 0, 0);
        }
        #endregion

        #region Public Methods
        public virtual void SetBounds(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnBoundsChanged(EventArgs e)
        {
            if (BoundsChanged != null) BoundsChanged.Invoke(this, e);
        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {
            if (EnabledChanged != null) EnabledChanged.Invoke(this, e);
        }

        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null) VisibleChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Dialog
    public class Dialog : Window
    {
        #region Constructor/Destructor
        public Dialog() : base()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(100, 100);
            Popup = true;
            Sizable = true;
            ShowInTaskbar = false;
        }

        public new virtual void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
    #endregion

    #region DropDown
    public class DropDown : Control
    {
        #region Events
        public event EventHandler SelectedIndexChanged;
        public event EventHandler ItemsChanged;
        public event EventHandler DrawArrowChanged;
        public event EventHandler UpdateTextChanged;
        #endregion

        #region Objects
        private List<string> p_Items;
        private int p_SelectedIndex;
        private bool p_DrawArrow, p_UpdateText;
        #endregion

        #region Properties
        public List<string> Items
        {
            get { return p_Items; }
        }

        public int SelectedIndex
        {
            get { return p_SelectedIndex; }
            set
            {
                if (value < 0) value = 0;
                if (value > p_Items.Count - 1) value = p_Items.Count - 1;

                p_SelectedIndex = value;
                if (p_UpdateText) Text = p_Items[p_SelectedIndex];
                OnSelectedIndexChanged(new EventArgs());
                ReDraw();
            }
        }

        //public string SelectedItem
        //{
            //get { return p_Items[p_SelectedIndex]; }
        //}

        public bool DrawArrow
        {
            get { return p_DrawArrow; }
            set
            {
                p_DrawArrow = value;
                OnDrawArrowChanged(new EventArgs());
                //ReDraw();
            }
        }

        public bool UpdateText
        {
            get { return p_UpdateText; }
            set
            {
                p_UpdateText = value;
                OnUpdateTextChanged(new EventArgs());
                //ReDraw();
            }
        }
        #endregion

        #region Constructors/Destructor
        public DropDown(Window owner, Control parent, string text) : base(owner, parent)
        {
            Text = text;

            p_Items = new List<string>();
            p_SelectedIndex = 0;
            p_DrawArrow = true;
            p_UpdateText = true;
        }

        public DropDown(Window owner, Control parent, string text, string toolTipText) : base(owner, parent)
        {
            Text = text;
            ToolTipText = toolTipText;

            p_Items = new List<string>();
            p_SelectedIndex = 0;
            p_DrawArrow = true;
            p_UpdateText = true;
        }

        public DropDown(Window owner, Control parent, string icon, string text, string toolTipText) : base(owner, parent)
        {
            Icon = icon;
            Text = text;
            ToolTipText = toolTipText;

            p_Items = new List<string>();
            p_SelectedIndex = 0;
            p_DrawArrow = true;
            p_UpdateText = true;
        }

        public override void Dispose()
        {
            p_UpdateText = false;
            p_DrawArrow = false;
            p_SelectedIndex = 0;
            p_Items.Clear();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            RectangleF rf = new RectangleF(bounds.X + 2, bounds.Y, bounds.Width - 20, bounds.Height);

            StringFormat sf = new StringFormat();
            if (RightToLeft) sf.Alignment = StringAlignment.Far;
            else sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Color bgColor = Global.Skin.Button.BGColor;
            Color bgColor2 = Global.Skin.Button.BGColor2;
            Color borderColor = Global.Skin.Button.BorderColor;
            Color textColor = Global.Skin.Button.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    bgColor = Global.Skin.Button.Down_BGColor;
                    bgColor2 = Global.Skin.Button.Down_BGColor2;
                    borderColor = Global.Skin.Button.Down_BorderColor;
                    textColor = Global.Skin.Button.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    bgColor = Global.Skin.Button.Over_BGColor;
                    bgColor2 = Global.Skin.Button.Over_BGColor2;
                    borderColor = Global.Skin.Button.Over_BorderColor;
                    textColor = Global.Skin.Button.Over_TextColor;
                }

                if (DrawBackground)
                {
                    Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, bgColor, bgColor2, LinearGradientMode.Vertical));
                    Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
                }
            }
            else
            {
                borderColor = Global.Skin.Button.BorderColor;
                textColor = Color.FromArgb(75, Global.Skin.Button.TextColor);

                if (DrawBackground) Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
            }

            g.DrawString(Icon, Skin.IconFont, new SolidBrush(textColor), bounds.X + 1, bounds.Y + 1);
            g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), rf, sf);
            if (p_DrawArrow) g.DrawString("6", Skin.IconFont, new SolidBrush(textColor), (bounds.X + bounds.Width) - 18, bounds.Y);

            base.OnDraw(g, bounds);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    SelectedIndex--;
                    break;
                case Keys.Down:
                    SelectedIndex++;
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Menu menu = new Menu(this);

                foreach (string item in p_Items)
                {
                    menu.Add(new MenuItem(string.Empty, item));
                }

                menu.Show();
            }

            base.OnMouseClick(e);
        }
        #endregion

        #region Public Methods
        public void Add(string item)
        {
            p_Items.Add(item);
            OnItemsChanged(new EventArgs());
        }

        public void AddRange(List<string> items)
        {
            p_Items.AddRange(items);
            OnItemsChanged(new EventArgs());
        }

        public void AddRange(string[] items)
        {
            p_Items.AddRange(items);
            OnItemsChanged(new EventArgs());
        }

        public void Clear()
        {
            p_Items.Clear();
            OnItemsChanged(new EventArgs());
        }

        public void Remove(string item)
        {
            p_Items.Remove(item);
            OnItemsChanged(new EventArgs());
        }

        public void RemoveAt(int index)
        {
            p_Items.RemoveAt(index);
            OnItemsChanged(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged.Invoke(this, e);
        }

        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (ItemsChanged != null) ItemsChanged.Invoke(this, e);
        }

        protected virtual void OnDrawArrowChanged(EventArgs e)
        {
            if (DrawArrowChanged != null) DrawArrowChanged.Invoke(this, e);
        }

        protected virtual void OnUpdateTextChanged(EventArgs e)
        {
            if (UpdateTextChanged != null) UpdateTextChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region DropDownGroup
    public class DropDownGroup : ControlGroup
    {
        #region Events
        public event EventHandler ItemsChanged;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler DefaultButtonClicked;
        public event EventHandler ShowDefaultButtonChanged;
        #endregion

        #region Objects
        private Label p_Label;
        private DropDown p_DropDown;
        private Button p_DefaultButton;
        private bool p_ShowDefaultButton;
        #endregion

        #region Properties
        public Label Label
        {
            get { return p_Label; }
        }

        public DropDown DropDown
        {
            get { return p_DropDown; }
        }

        public Button DefaultButton
        {
            get { return p_DefaultButton; }
        }

        public bool ShowDefaultButton
        {
            get { return p_ShowDefaultButton; }
            set
            {
                p_ShowDefaultButton = value;

                OnShowDefaultButtonChanged(new EventArgs());
            }
        }

        public override Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;

                if (p_ShowDefaultButton)
                {
                    p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                    p_DropDown.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 146, base.Bounds.Height);
                    p_DefaultButton.SetBounds(base.Bounds.X + (base.Bounds.Width - 20), base.Bounds.Y, 20, base.Bounds.Height);
                }
                else
                {
                    p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                    p_DropDown.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 120, base.Bounds.Height);
                    p_DefaultButton.SetBounds(0, 0, 0, 0);
                }

                OnBoundsChanged(new EventArgs());
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_Label.Enabled = base.Enabled;
                p_DropDown.Enabled = base.Enabled;
                p_DefaultButton.Enabled = base.Enabled;

                OnEnabledChanged(new EventArgs());
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_Label.Visible = base.Visible;
                p_DropDown.Visible = base.Visible;
                p_DefaultButton.Visible = base.Visible;

                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructors/Destructor
        public DropDownGroup(Window owner, Control parent, string text, string value, string toolTipText, string[] items) : base()
        {
            p_Label = new Label(owner, parent, text);

            p_DropDown = new DropDown(owner, parent, value, toolTipText);
            if (items != null) p_DropDown.AddRange(items);
            p_DropDown.ItemsChanged += new EventHandler(p_DropDown_ItemsChanged);
            p_DropDown.SelectedIndexChanged += new EventHandler(p_DropDown_SelectedIndexChanged);

            p_DefaultButton = new Button(owner, parent, "s", string.Empty, "Restore Default");
            p_DefaultButton.MouseClick += new MouseEventHandler(p_DefaultButton_MouseClick);

            p_ShowDefaultButton = true;
        }

        public DropDownGroup(Window owner, Control parent, string text, int index, string toolTipText, string[] items) : base()
        {
            p_Label = new Label(owner, parent, text);

            p_DropDown = new DropDown(owner, parent, string.Empty, toolTipText);
            if (items != null) p_DropDown.AddRange(items);
            p_DropDown.ItemsChanged += new EventHandler(p_DropDown_ItemsChanged);
            p_DropDown.SelectedIndexChanged += new EventHandler(p_DropDown_SelectedIndexChanged);
            p_DropDown.SelectedIndex = index;

            p_DefaultButton = new Button(owner, parent, "s", string.Empty, "Restore Default");
            p_DefaultButton.MouseClick += new MouseEventHandler(p_DefaultButton_MouseClick);

            p_ShowDefaultButton = true;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Child Events
        private void p_DropDown_ItemsChanged(object sender, EventArgs e)
        {
            OnItemsChanged(new EventArgs());
        }

        private void p_DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(new EventArgs());
        }

        private void p_DefaultButton_MouseClick(object sender, EventArgs e)
        {
            OnDefaultButtonClicked(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (ItemsChanged != null) ItemsChanged.Invoke(this, e);
        }
        
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged.Invoke(this, e);
        }

        protected virtual void OnDefaultButtonClicked(EventArgs e)
        {
            if (DefaultButtonClicked != null) DefaultButtonClicked.Invoke(this, e);
        }

        protected virtual void OnShowDefaultButtonChanged(EventArgs e)
        {
            if (ShowDefaultButtonChanged != null) ShowDefaultButtonChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ErrorDialog
    public class ErrorDialog : Dialog
    {
        #region Objects
        private GenericList<GenericListItem> p_ErrorList;
        private Menu p_ContextMenu;
        #endregion

        #region Constructor/Destructor
        public ErrorDialog(List<string> errors) : base()
        {
            MinimumSize = new Size(460, 240);
            Size = new Size(460, 240);
            Text = "Compiler Errors (" + errors.Count.ToString() + ")";

            p_ErrorList = new GenericList<GenericListItem>(this, null);
            p_ErrorList.MouseUp += new MouseEventHandler(p_ErrorList_MouseUp);

            p_ContextMenu = new Menu(null);
            p_ContextMenu.Add(new MenuItem(string.Empty, "Copy"));
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            foreach (string error in errors)
            {
                p_ErrorList.Add(new GenericListItem(error));
            }
        }

        ~ErrorDialog()
        {

        }
        #endregion

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            if (!Visible) return;

            p_ErrorList.SetBounds(12, 30, Width - 24, Height - 42);

            base.OnResize(e);
        }
        #endregion

        #region Child Events
        void p_ErrorList_MouseUp(object sender, MouseEventArgs e)
        {
            if (p_ErrorList.SelectedItem == null) return;

            if (e.Button == MouseButtons.Right) p_ContextMenu.Show();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Copy":
                    Clipboard.SetText(p_ErrorList.SelectedItem.Text);
                    break;
            }
        }
        #endregion
    }
    #endregion

    #region FilterBar
    public class FilterBar : Control
    {
        #region Events
        public event EventHandler SelectedIndexChanged;
        public new event EventHandler TextChanged;
        #endregion

        #region Objects
        private TextBox p_TextBox;
        private DropDown p_DropDown;
        #endregion

        #region Properties
        public TextBox TextBox
        {
            get { return p_TextBox; }
        }

        public DropDown DropDown
        {
            get { return p_DropDown; }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_TextBox.Enabled = base.Enabled;
                p_DropDown.Enabled = base.Enabled;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_TextBox.Visible = base.Visible;
                p_DropDown.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public FilterBar(Window owner, Control parent, string mask, string[] items, string toolTipText) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_TextBox = new TextBox(owner, parent, string.Empty, mask);
            p_TextBox.DrawBackground = false;
            p_TextBox.DrawFocusRect = false;
            p_TextBox.TextChanged += new EventHandler(p_TextBox_TextChanged);

            if (items.GetUpperBound(0) == 0) p_DropDown = new DropDown(owner, parent, "Filter", toolTipText);
            else p_DropDown = new DropDown(owner, parent, items[0], toolTipText);
            p_DropDown.AddRange(items);
            p_DropDown.DrawBackground = false;
            p_DropDown.DrawFocusRect = false;
            p_DropDown.RightToLeft = true;
            p_DropDown.SelectedIndexChanged += new EventHandler(p_DropDown_SelectedIndexChanged);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
            g.FillRectangle(new LinearGradientBrush(r, Global.Skin.ToolBar.BGColor, Global.Skin.ToolBar.BGColor2, LinearGradientMode.Vertical), r);

            r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            Global.DrawRoundedRectangle(g, r, new Pen(Global.Skin.ToolBar.BorderColor));

            base.OnDraw(g, bounds);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_TextBox.SetBounds(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 140, Bounds.Height - 2);
            p_DropDown.SetBounds(Bounds.X + (Bounds.Width - 134), Bounds.Y + 1, 133, Bounds.Height - 2);

            base.OnBoundsChanged(e);
        }
        #endregion

        #region Child Events
        private void p_TextBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        private void p_DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged.Invoke(this, e);
        }

        protected new virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null) TextChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region FolderBrowser
    public class FolderBrowser : Control
    {
        #region Events
        public event EventHandler FolderChanged;
        #endregion

        #region Objects
        //private FolderBrowserDialog p_Dialog;
        private System.Windows.Forms.FolderBrowserDialog p_Dialog;
        #endregion

        #region Properties
        //public FolderBrowserDialog Dialog
        //{
            //get { return p_Dialog; }
        //}

        public System.Windows.Forms.FolderBrowserDialog Dialog
        {
            get { return p_Dialog; }
        }
        #endregion

        #region Constructors/Destructor
        public FolderBrowser(Window owner, Control parent, string text) : base(owner, parent)
        {
            Text = text;

            p_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            p_Dialog.Description = "Select Music Folder";
            p_Dialog.ShowNewFolderButton = false;
            
            //p_Dialog = new FolderBrowserDialog("Select Music Folder");
        }

        public FolderBrowser(Window owner, Control parent, string text, string toolTipText) : base(owner, parent)
        {
            Text = text;
            ToolTipText = toolTipText;

            p_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            p_Dialog.Description = "Select Music Folder";
            p_Dialog.ShowNewFolderButton = false;

            //p_Dialog = new FolderBrowserDialog("Select Music Folder");
        }

        public override void Dispose()
        {
            p_Dialog.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            RectangleF rf = new RectangleF(bounds.X + 2, bounds.Y, bounds.Width - 20, bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Color bgColor = Global.Skin.Button.BGColor;
            Color bgColor2 = Global.Skin.Button.BGColor2;
            Color borderColor = Global.Skin.Button.BorderColor;
            Color textColor = Global.Skin.Button.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    bgColor = Global.Skin.Button.Down_BGColor;
                    bgColor2 = Global.Skin.Button.Down_BGColor2;
                    borderColor = Global.Skin.Button.Down_BorderColor;
                    textColor = Global.Skin.Button.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    bgColor = Global.Skin.Button.Over_BGColor;
                    bgColor2 = Global.Skin.Button.Over_BGColor2;
                    borderColor = Global.Skin.Button.Over_BorderColor;
                    textColor = Global.Skin.Button.Over_TextColor;
                }

                if (DrawBackground)
                {
                    Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, bgColor, bgColor2, LinearGradientMode.Vertical));
                    Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
                }
            }
            else
            {
                borderColor = Global.Skin.Button.BorderColor;
                textColor = Color.FromArgb(75, Global.Skin.Button.TextColor);

                if (DrawBackground) Global.DrawRoundedRectangle(g, r, new Pen(borderColor));
            }

            g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), rf, sf);
            g.DrawString("...", Skin.WindowFont, new SolidBrush(textColor), (bounds.X + bounds.Width) - 18, bounds.Y);

            base.OnDraw(g, bounds);
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            p_Dialog.SelectedPath = Global.Settings.MusicFolder;

            if (p_Dialog.ShowDialog(Owner) == DialogResult.OK)
            {
                //Text = p_Dialog.SelectedPath;
                OnFolderChanged(new EventArgs());
            }

            base.OnMouseClick(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnFolderChanged(EventArgs e)
        {
            if (FolderChanged != null) FolderChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region FolderBrowserDialog
    public class FolderBrowserDialog : Dialog
    {
        #region Objects
        private Tree p_Tree;
        private Button p_OKButton, p_CancelButton;
        #endregion

        #region Constructor/Destructor
        public FolderBrowserDialog(string text) : base()
        {
            Text = text;

            p_Tree = new Tree(this, null);
            p_Tree.Add(new TreeItem("Desktop", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)));
            p_Tree.Add(new TreeItem("My Documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
            p_Tree.Add(new TreeItem("My Music", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)));

            TreeItem item = new TreeItem("My Computer", string.Empty);
            item.Children.Add(new TreeItem("C:", "C:"));
            item.Expanded = true;

            p_Tree.Add(item);
            p_Tree.DrawFocusRect = false;

            p_OKButton = new Button(this, null, string.Empty, "OK", string.Empty);
            p_OKButton.Enabled = false;
            p_OKButton.MouseClick += new MouseEventHandler(p_OKButton_MouseClick);

            p_CancelButton = new Button(this, null, string.Empty, "Cancel", string.Empty);
            p_CancelButton.MouseClick += new MouseEventHandler(p_CancelButton_MouseClick);
        }

        ~FolderBrowserDialog()
        {

        }
        #endregion

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            int x = (Width - 36) / 2;

            p_Tree.SetBounds(12, 30, Width - 24, Height - 72);
            p_OKButton.SetBounds(12, Height - 32, x, 20);
            p_CancelButton.SetBounds(x + 24, Height - 32, x, 20);

            base.OnResize(e);
        }
        #endregion

        #region Child Events
        private void p_OKButton_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void p_CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion
    }
    #endregion

    #region FolderBrowserGroup
    public class FolderBrowserGroup : ControlGroup
    {
        #region Events
        public event EventHandler DefaultButtonClicked;
        public event EventHandler FolderChanged;
        #endregion

        #region Objects
        private Label p_Label;
        private FolderBrowser p_FolderBrowser;
        private Button p_DefaultButton;
        #endregion

        #region Properties
        public Label Label
        {
            get { return p_Label; }
        }

        public FolderBrowser FolderBrowser
        {
            get { return p_FolderBrowser; }
        }

        public Button DefaultButton
        {
            get { return p_DefaultButton; }
        }

        public override Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;

                p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                p_FolderBrowser.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 146, base.Bounds.Height);
                p_DefaultButton.SetBounds(base.Bounds.X + (base.Bounds.Width - 20), base.Bounds.Y, 20, base.Bounds.Height);

                OnBoundsChanged(new EventArgs());
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_Label.Enabled = base.Enabled;
                p_FolderBrowser.Enabled = base.Enabled;
                p_DefaultButton.Enabled = base.Enabled;

                OnEnabledChanged(new EventArgs());
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_Label.Visible = base.Visible;
                p_FolderBrowser.Visible = base.Visible;
                p_DefaultButton.Visible = base.Visible;

                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public FolderBrowserGroup(Window owner, Control parent, string text, string value, string toolTipText) : base()
        {
            p_Label = new Label(owner, parent, text);

            p_FolderBrowser = new FolderBrowser(owner, parent, value, toolTipText);
            p_FolderBrowser.FolderChanged += new EventHandler(p_FolderBrowser_FolderChanged);

            p_DefaultButton = new Button(owner, parent, "s", string.Empty, "Restore Default");
            p_DefaultButton.MouseClick += new MouseEventHandler(p_DefaultButton_MouseClick);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Child Events
        private void p_DefaultButton_MouseClick(object sender, EventArgs e)
        {
            OnDefaultButtonClicked(new EventArgs());
        }

        private void p_FolderBrowser_FolderChanged(object sender, EventArgs e)
        {
            OnFolderChanged(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnDefaultButtonClicked(EventArgs e)
        {
            if (DefaultButtonClicked != null) DefaultButtonClicked.Invoke(this, e);
        }

        protected virtual void OnFolderChanged(EventArgs e)
        {
            if (FolderChanged != null) FolderChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region GenericList
    public class GenericList<T> : Control where T : GenericListItem
    {
        #region Enums
        public enum FilterTypes : int
        {
            AllItems,
        }
        #endregion

        #region Events
        public event EventHandler CurrentItemChanged;
        public event EventHandler FirstItemChanged;
        public event EventHandler HotItemChanged;
        public event EventHandler PlayingItemChanged;
        public event EventHandler SelectedItemChanged;
        public event EventHandler ItemsChanged;
        public event EventHandler FilteredItemsChanged;
        public event EventHandler ItemHeightChanged;
        #endregion

        #region Objects
        private List<T> p_Items;
        private List<T> p_FilteredItems;
        private int p_ItemHeight;
        private T p_FirstItem, p_HotItem, p_PlayingItem, p_SelectedItem;
        private bool p_Refreshing;
        private ScrollBar p_ScrollBar;
        #endregion

        #region Properties
        public List<T> Items
        {
            get { return p_Items; }
        }

        public List<T> FilteredItems
        {
            get { return p_FilteredItems; }
        }

        public int ItemHeight
        {
            get { return p_ItemHeight; }
            set
            {
                p_ItemHeight = value;
                OnItemHeightChanged(new EventArgs());
                //ReDraw();
            }
        }

        public T FirstItem
        {
            get { return p_FirstItem; }
            set
            {
                p_FirstItem = value;
                UpdateFirstItem();
                ReDraw();
            }
        }

        public T HotItem
        {
            get { return p_HotItem; }
            set
            {
                p_HotItem = value;
                UpdateHotItem();
                ReDraw();
            }
        }

        public T PlayingItem
        {
            get { return p_PlayingItem; }
            set
            {
                p_PlayingItem = value;
                UpdatePlayingItem();
                ReDraw();
            }
        }

        public T SelectedItem
        {
            get { return p_SelectedItem; }
            set
            {
                p_SelectedItem = value;
                EnsureVisible();
                UpdateSelectedItem();
                ReDraw();
            }
        }

        public int FirstIndex
        {
            get
            {
                if (p_FirstItem == null) return 0;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (p_FilteredItems[i].Equals(p_FirstItem)) return i;
                }

                return 0;
            }
            set
            {
                if (value < 0) value = 0;
                if (value > p_FilteredItems.Count - GetVisibleItemsCount()) value = p_FilteredItems.Count - GetVisibleItemsCount();

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (i == value)
                    {
                        FirstItem = p_FilteredItems[i];
                        return;
                    }
                }

                FirstItem = null;
            }
        }

        public int HotIndex
        {
            get
            {
                if (p_HotItem == null) return -1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (p_FilteredItems[i].Equals(p_HotItem)) return i;
                }

                return -1;
            }
            set
            {
                if (value < -1) value = -1;
                if (value > p_FilteredItems.Count - 1) value = p_FilteredItems.Count - 1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (i == value)
                    {
                        HotItem = p_FilteredItems[i];
                        return;
                    }
                }

                HotItem = null;
            }
        }

        public int PlayingIndex
        {
            get
            {
                if (p_PlayingItem == null) return -1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (p_FilteredItems[i].Equals(p_PlayingItem)) return i;
                }

                return -1;
            }
            set
            {
                if (value < -1) value = -1;
                if (value > p_FilteredItems.Count - 1) value = p_FilteredItems.Count - 1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (i == value)
                    {
                        PlayingItem = p_FilteredItems[i];
                        return;
                    }
                }

                PlayingItem = null;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (p_SelectedItem == null) return -1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (p_FilteredItems[i].Equals(p_SelectedItem)) return i;
                }

                return -1;
            }
            set
            {
                if (value < 0) value = 0;
                if (value > p_FilteredItems.Count - 1) value = p_FilteredItems.Count - 1;

                for (int i = 0; i < p_FilteredItems.Count; i++)
                {
                    if (i == value)
                    {
                        SelectedItem = p_FilteredItems[i];
                        return;
                    }
                }

                SelectedItem = null;
            }
        }

        public virtual bool Refreshing
        {
            get { return p_Refreshing; }
            set
            {
                p_Refreshing = value;
                ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_ScrollBar.Visible = base.Visible;
            }
        }

        public ScrollBar ScrollBar
        {
            get { return p_ScrollBar; }
        }
        #endregion

        #region Constructor/Destructor
        public GenericList(Window owner, Control parent) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_Items = new List<T>();
            p_FilteredItems = new List<T>();
            p_ItemHeight = 18;
            p_FirstItem = null;
            p_HotItem = null;
            p_PlayingItem = null;
            p_SelectedItem = null;
            p_Refreshing = false;

            p_ScrollBar = new ScrollBar(Owner, this);
            p_ScrollBar.ValueChanged += new EventHandler(p_ScrollBar_ValueChanged);
        }

        public override void Dispose()
        {
            p_Refreshing = false;
            if (p_SelectedItem != null) p_SelectedItem.Dispose();
            if (p_PlayingItem != null) p_PlayingItem.Dispose();
            if (p_HotItem != null) p_HotItem.Dispose();
            if (p_FirstItem != null) p_FirstItem.Dispose();
            p_ItemHeight = 0;

            p_FilteredItems.Clear();

            foreach (T item in p_Items)
            {
                item.Dispose();
            }
            p_Items.Clear();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

            if (Enabled)
            {
                Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, Global.Skin.List.BGColor, Global.Skin.List.BGColor2, LinearGradientMode.Vertical));

                DrawItems(g, bounds);
            }

            Global.DrawRoundedRectangle(g, r, new Pen(Global.Skin.List.BorderColor));

            r = new Rectangle(bounds.X, (bounds.Y + bounds.Height) - 20, bounds.Width - 1, 19);
            LinearGradientBrush b = new LinearGradientBrush(r, Color.Transparent, Color.FromArgb(150, Global.Skin.List.BGColor2), LinearGradientMode.Vertical);
            b.WrapMode = WrapMode.TileFlipX;
            g.FillRectangle(b, r);

            base.OnDraw(g, bounds);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_ScrollBar.Enabled = p_FilteredItems.Count > GetVisibleItemsCount(); //?
            p_ScrollBar.Maximum = p_FilteredItems.Count - GetVisibleItemsCount(); //?
            p_ScrollBar.SetBounds((Bounds.X + Bounds.Width) - 16, Bounds.Y + 3, 13, Bounds.Height - 6);
            //p_ScrollBar.Slider.SliderHeight = GetVisibleItemsCount();

            base.OnBoundsChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    SelectedIndex--;
                    break;
                case Keys.Down:
                    SelectedIndex++;
                    break;
                case Keys.Home:
                    SelectedIndex = 0;
                    break;
                case Keys.End:
                    SelectedIndex = p_FilteredItems.Count - 1;
                    break;
                case Keys.PageUp:
                    SelectedIndex -= GetVisibleItemsCount();
                    break;
                case Keys.PageDown:
                    SelectedIndex += GetVisibleItemsCount();
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int x = (Bounds.Width - p_ScrollBar.Bounds.Width) - 8;

            if (e.X > Bounds.X + x) return;

            int y = 3;

            for (int i = FirstIndex; i < p_FilteredItems.Count; i++)
            {
                if (e.Y > Bounds.Y + y && e.Y <= (Bounds.Y + y) + p_ItemHeight)
                {
                    if (SelectedIndex != i) SelectedIndex = i;
                    return;
                }

                y += p_ItemHeight;
            }

            SelectedItem = null;

            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            HotItem = null;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int x = (Bounds.Width - p_ScrollBar.Bounds.Width) - 8;

            if (e.X > Bounds.X + x)
            {
                HotItem = null;
                return;
            }

            int y = 3;

            for (int i = FirstIndex; i < p_FilteredItems.Count; i++)
            {
                if (e.Y > Bounds.Y + y && e.Y <= (Bounds.Y + y) + p_ItemHeight)
                {
                    if (HotIndex != i) HotIndex = i;
                    return;
                }

                y += p_ItemHeight;
            }

            HotItem = null;

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0) FirstIndex += GetVisibleItemsCount();
            else FirstIndex -= GetVisibleItemsCount();

            base.OnMouseWheel(e);
        }
        #endregion

        #region Child Events
        private void p_ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (FirstIndex != p_ScrollBar.Value) FirstIndex = p_ScrollBar.Value;
        }
        #endregion

        #region Public Methods
        public void UpdateFirstItem()
        {
            p_ScrollBar.Maximum = p_FilteredItems.Count - GetVisibleItemsCount();
            p_ScrollBar.Value = FirstIndex;

            OnFirstItemChanged(new EventArgs());
        }

        public void UpdateHotItem()
        {
            OnHotItemChanged(new EventArgs());
        }

        public void UpdatePlayingItem()
        {
            OnCurrentItemChanged(new EventArgs());

            OnPlayingItemChanged(new EventArgs());
        }

        public void UpdateSelectedItem()
        {
            if (p_PlayingItem == null) OnCurrentItemChanged(new EventArgs());

            OnSelectedItemChanged(new EventArgs());
        }

        public void UpdateItems()
        {
            UpdateFilteredItems();

            OnItemsChanged(new EventArgs());
        }

        public void UpdateFilteredItems()
        {
            FilterItems();

            p_ScrollBar.Enabled = p_FilteredItems.Count > GetVisibleItemsCount();
            p_ScrollBar.Maximum = p_FilteredItems.Count - GetVisibleItemsCount();
            p_ScrollBar.Value = FirstIndex;

            OnFilteredItemsChanged(new EventArgs());
        }

        public void Add(T item)
        {
            p_Items.Add(item);
            UpdateItems();
        }

        public void AddRange(List<T> items)
        {
            p_Items.AddRange(items);
            UpdateItems();
        }

        public void AddRange(T[] items)
        {
            p_Items.AddRange(items);
            UpdateItems();
        }

        public void Clear()
        {
            p_Items.Clear();
            UpdateItems();
        }

        public void Remove(T item)
        {
            p_Items.Remove(item);
            UpdateItems();
        }

        public void RemoveAt(int index)
        {
            p_Items.RemoveAt(index);
            UpdateItems();
        }

        public T GetCurrentItem()
        {
            if (p_PlayingItem != null) return p_PlayingItem;
            else if (p_SelectedItem != null) return p_SelectedItem;
            return null;
        }

        public virtual int GetVisibleItemsCount()
        {
            int i = 0;

            for (int y = 3; y < (Bounds.Height - p_ItemHeight) - 4; y += p_ItemHeight)
            {
                i++;
            }

            return i;
        }

        public virtual bool IsBeforeBounds(int index)
        {
            if (index < FirstIndex) return true;

            return false;
        }

        public virtual bool IsAfterBounds(int index)
        {
            if (index <= FirstIndex) return false;
            int y = 0;

            for (int i = FirstIndex; i < p_FilteredItems.Count; i++)
            {
                if (i == index && y > (Bounds.Height - p_ItemHeight) - 4) return true;

                y += p_ItemHeight;
            }

            return false;
        }

        public virtual void EnsureVisible()
        {
            for (int i = 0; i < p_FilteredItems.Count; i++)
            {
                if (p_FilteredItems[i].Equals(p_SelectedItem))
                {
                    if (IsBeforeBounds(i)) FirstIndex = i;
                    else if (IsAfterBounds(i)) FirstIndex = i - (GetVisibleItemsCount() - 1);

                    return;
                }
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (bounds.Width - p_ScrollBar.Bounds.Width) - 8;
            int y = 0;

            if (p_Items.Count == 0) g.DrawString("No items found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (p_ItemHeight / 2), x, p_ItemHeight), sf);
            else if (p_Refreshing) g.DrawString("Refreshing, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (p_ItemHeight / 2), x, p_ItemHeight), sf);
            else if (p_FilteredItems.Count == 0) g.DrawString("No items found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (p_ItemHeight / 2), x, p_ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < p_FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - p_ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (p_FilteredItems[i].Equals(p_SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, p_ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (p_FilteredItems[i].Equals(p_PlayingItem)) textColor = Global.Skin.List.Play_TextColor;
                    if (p_FilteredItems[i].Equals(p_HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    g.DrawString(p_FilteredItems[i].ToString(), Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x, p_ItemHeight), sf);

                    y += p_ItemHeight;
                }
            }
        }

        protected virtual void FilterItems()
        {
            p_FilteredItems.Clear();
            p_FilteredItems.AddRange(p_Items);

            //FilteredItems.Sort(new GenericListSorter<GenericListItem>(GenericListSorter<GenericListItem>.SortMode.Ascending));
            ReDraw(); //?
        }

        protected virtual void OnCurrentItemChanged(EventArgs e)
        {
            if (CurrentItemChanged != null) CurrentItemChanged.Invoke(this, e);
        }

        protected virtual void OnFirstItemChanged(EventArgs e)
        {
            if (FirstItemChanged != null) FirstItemChanged.Invoke(this, e);
        }

        protected virtual void OnHotItemChanged(EventArgs e)
        {
            if (HotItemChanged != null) HotItemChanged.Invoke(this, e);
        }

        protected virtual void OnPlayingItemChanged(EventArgs e)
        {
            if (PlayingItemChanged != null) PlayingItemChanged.Invoke(this, e);
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            if (SelectedItemChanged != null) SelectedItemChanged.Invoke(this, e);
        }

        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (ItemsChanged != null) ItemsChanged.Invoke(this, e);
        }

        protected virtual void OnFilteredItemsChanged(EventArgs e)
        {
            if (FilteredItemsChanged != null) FilteredItemsChanged.Invoke(this, e);
        }

        protected virtual void OnItemHeightChanged(EventArgs e)
        {
            if (ItemHeightChanged != null) ItemHeightChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region GenericListItem
    public class GenericListItem
    {
        #region Objects
        private string p_Key, p_Text;
        private object p_Tag;
        #endregion

        #region Properties
        public string Key
        {
            get { return p_Key; }
        }

        public string Text
        {
            get { return p_Text; }
            set { p_Text = value; }
        }

        public object Tag
        {
            get { return p_Tag; }
            set { p_Tag = value; }
        }
        #endregion

        #region Constructors/Destructor
        public GenericListItem()
        {
            p_Key = Path.GetRandomFileName();
            p_Text = string.Empty;
            p_Tag = null;
        }

        public GenericListItem(string text)
        {
            p_Key = Path.GetRandomFileName();
            p_Text = text;
            p_Tag = null;
        }

        public virtual void Dispose()
        {
            p_Tag = null;
            p_Text = string.Empty;
            p_Key = string.Empty;
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            GenericListItem item = obj as GenericListItem;
            if (item == null) return false;

            return ToString().Equals(item.ToString());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return p_Text;
        }
        #endregion
    }
    #endregion

    #region GenericListSorter
    public class GenericListSorter<T> : IComparer<T> where T : GenericListItem
    {
        #region Enums
        public enum SortMode : int
        {
            None,
            Ascending,
            Descending,
        }
        #endregion

        #region Objects
        private SortMode p_Mode;
        #endregion

        #region Properties
        public SortMode Mode
        {
            get { return p_Mode; }
            set { p_Mode = value; }
        }
        #endregion

        #region Constructor/Destructor
        public GenericListSorter(SortMode mode) : base()
        {
            p_Mode = mode;
        }

        public virtual void Dispose()
        {
            p_Mode = SortMode.None;
        }
        #endregion

        #region Virtual Methods
        public virtual int Compare(T item1, T item2)
        {
            switch (p_Mode)
            {
                case SortMode.Ascending:
                    return item1.ToString().CompareTo(item2.ToString());
                case SortMode.Descending:
                    return item2.ToString().CompareTo(item1.ToString());
                default:
                    return 0;
            }
        }
        #endregion
    }
    #endregion

    #region Gripper
    internal class Gripper : Control
    {
        #region Constructor/Destructor
        public Gripper(Window owner, Control parent) : base(owner, parent)
        {
            //Cursor = Cursors.SizeNWSE;
            DrawFocusRect = false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            int x = bounds.X + bounds.Width;
            int y = bounds.Y + bounds.Height;
            SolidBrush b = new SolidBrush(Global.Skin.Window.TextColor);

            Rectangle r = new Rectangle(x - 14, y - 6, 2, 2);
            g.FillRectangle(b, r);

            r = new Rectangle(x - 6, y - 14, 2, 2);
            g.FillRectangle(b, r);

            r = new Rectangle(x - 10, y - 10, 2, 2);
            g.FillRectangle(b, r);

            r = new Rectangle(x - 6, y - 10, 2, 2);
            g.FillRectangle(b, r);

            r = new Rectangle(x - 10, y - 6, 2, 2);
            g.FillRectangle(b, r);

            r = new Rectangle(x - 6, y - 6, 2, 2);
            g.FillRectangle(b, r);

            base.OnDraw(g, bounds);
        }
        #endregion
    }
    #endregion

    #region GroupBox
    public class GroupBox : Control
    {
        #region Constructor/Destructor
        public GroupBox(Window owner, Control parent) : base(owner, parent)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
            g.FillRectangle(new LinearGradientBrush(r, Global.Skin.ToolBar.BGColor, Global.Skin.ToolBar.BGColor2, LinearGradientMode.Vertical), r);

            r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            Global.DrawRoundedRectangle(g, r, new Pen(Global.Skin.ToolBar.BorderColor));

            base.OnDraw(g, bounds);
        }
        #endregion
    }
    #endregion

    #region ID3Rating
    public class ID3Rating : Control
    {
        #region Events
        public event EventHandler RatingChanged;
        public event EventHandler ReadOnlyChanged;
        #endregion

        #region Objects
        private double p_Rating, p_NewRating;
        private bool p_ReadOnly;
        private Bitmap p_StarNone, p_StarHalf, p_StarFull;
        #endregion

        #region Properties
        public double Rating
        {
            get { return p_Rating; }
            set
            {
                p_Rating = value;
                OnRatingChanged(new EventArgs());
                ReDraw();
            }
        }

        public double NewRating
        {
            get { return p_NewRating; }
            set
            {
                p_NewRating = value;
                ReDraw();
            }
        }

        public bool ReadOnly
        {
            get { return p_ReadOnly; }
            set
            {
                p_ReadOnly = value;
                OnReadOnlyChanged(new EventArgs());
                //ReDraw();
            }
        }

        public Bitmap StarNone
        {
            get { return p_StarNone; }
            set { p_StarNone = value; }
        }

        public Bitmap StarHalf
        {
            get { return p_StarHalf; }
            set { p_StarHalf = value; }
        }

        public Bitmap StarFull
        {
            get { return p_StarFull; }
            set { p_StarFull = value; }
        }
        #endregion

        #region Constructor/Destructor
        public ID3Rating(Window owner, Control parent) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_Rating = 0.0d;
            p_NewRating = -1.0d;
            p_ReadOnly = false;
            p_StarNone = Global.AdjustColor(Properties.Resources.StarNone, Global.Skin.List.Over_TextColor);
            p_StarHalf = Global.AdjustColor(Properties.Resources.StarHalf, Global.Skin.List.Over_TextColor);
            p_StarFull = Global.AdjustColor(Properties.Resources.StarFull, Global.Skin.List.Over_TextColor);
        }

        public override void Dispose()
        {
            p_StarFull.Dispose();
            p_StarHalf.Dispose();
            p_StarNone.Dispose();
            p_ReadOnly = false;
            p_NewRating = 0.0d;
            p_Rating = 0.0d;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            double rating = p_NewRating;
            if (rating == -1.0d) rating = p_Rating;

            if (Enabled)
            {
                if (rating == 0.0d)
                {
                    g.DrawImage(p_StarNone, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 0.5d)
                {
                    g.DrawImage(p_StarHalf, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 1.0d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 1.5d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarHalf, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 2.0d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 2.5d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarHalf, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 3.0d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 3.5d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarHalf, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 4.0d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarNone, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 4.5d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarHalf, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
                else if (rating == 5.0d)
                {
                    g.DrawImage(p_StarFull, bounds.X + 2, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 18, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 34, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 50, bounds.Y + 2, 16, 16);
                    g.DrawImage(p_StarFull, bounds.X + 66, bounds.Y + 2, 16, 16);
                }
            }
            else
            {

            }

            base.OnDraw(g, bounds);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (p_ReadOnly) return;

            NewRating = -1.0d;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (p_ReadOnly) return;

            if (e.X < Bounds.X + 2) NewRating = 0.0d;
            else if (e.X < Bounds.X + 10) NewRating = 0.5d;
            else if (e.X < Bounds.X + 18) NewRating = 1.0d;
            else if (e.X < Bounds.X + 26) NewRating = 1.5d;
            else if (e.X < Bounds.X + 34) NewRating = 2.0d;
            else if (e.X < Bounds.X + 42) NewRating = 2.5d;
            else if (e.X < Bounds.X + 50) NewRating = 3.0d;
            else if (e.X < Bounds.X + 58) NewRating = 3.5d;
            else if (e.X < Bounds.X + 66) NewRating = 4.0d;
            else if (e.X < Bounds.X + 74) NewRating = 4.5d;
            else NewRating = 5.0d;

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (p_ReadOnly) return;

            Rating = p_NewRating;
            p_NewRating = -1.0d;

            base.OnMouseUp(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnRatingChanged(EventArgs e)
        {
            if (RatingChanged != null) RatingChanged.Invoke(this, e);
        }

        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            if (ReadOnlyChanged != null) ReadOnlyChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region InfoDialog
    public class InfoDialog : Dialog
    {
        #region Enums
        public enum InfoButtons
        {
            OK,
            OKCancel,
            YesNo,
            YesNoCancel,
            Abort,
            AbortRetry,
            AbortRetryIgnore,
        }
        #endregion

        #region Objects
        private Label p_Label;
        private InfoButtons p_InfoButtons;
        private Button p_Button1, p_Button2, p_Button3;
        #endregion

        #region Constructors/Destuctor
        public InfoDialog(string message) : base()
        {
            Sizable = false;
            Size = new Size(260, 160);

            int x = (Width / 3) - 12;

            p_Label = new Label(this, null, message);
            p_Label.SetBounds(12, 30, Width - 24, 80);

            p_InfoButtons = InfoButtons.OK;

            p_Button1 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button1.MouseClick += new MouseEventHandler(p_Button1_MouseClick);
            p_Button1.Visible = false;
            p_Button1.SetBounds(12, 126, x, 20);

            p_Button2 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button2.MouseClick += new MouseEventHandler(p_Button2_MouseClick);
            p_Button2.Visible = false;
            p_Button2.SetBounds(x + 18, 126, x, 20);

            p_Button3 = new Button(this, null, string.Empty, "OK", string.Empty);
            p_Button3.MouseClick += new MouseEventHandler(p_Button3_MouseClick);
            p_Button3.Result = DialogResult.OK;
            p_Button3.SetBounds((x * 2) + 24, 126, x, 20);
        }

        public InfoDialog(string text, string message) : base()
        {
            Sizable = false;
            Size = new Size(260, 160);
            Text = text;

            int x = (Width / 3) - 12;

            p_Label = new Label(this, null, message);
            p_Label.SetBounds(12, 30, Width - 24, 80);

            p_InfoButtons = InfoButtons.OK;

            p_Button1 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button1.MouseClick += new MouseEventHandler(p_Button1_MouseClick);
            p_Button1.Visible = false;
            p_Button1.SetBounds(12, 126, x, 20);

            p_Button2 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button2.MouseClick += new MouseEventHandler(p_Button2_MouseClick);
            p_Button2.Visible = false;
            p_Button2.SetBounds(x + 18, 126, x, 20);

            p_Button3 = new Button(this, null, string.Empty, "OK", string.Empty);
            p_Button3.MouseClick += new MouseEventHandler(p_Button3_MouseClick);
            p_Button3.Result = DialogResult.OK;
            p_Button3.SetBounds((x * 2) + 24, 126, x, 20);
        }

        public InfoDialog(string message, InfoButtons infoButtons) : base()
        {
            Sizable = false;
            Size = new Size(260, 160);

            int x = (Width / 3) - 12;

            p_Label = new Label(this, null, message);
            p_Label.SetBounds(12, 30, Width - 24, 80);

            p_InfoButtons = infoButtons;

            p_Button1 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button1.MouseClick += new MouseEventHandler(p_Button1_MouseClick);
            p_Button1.SetBounds(12, 126, x, 20);

            p_Button2 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button2.MouseClick += new MouseEventHandler(p_Button2_MouseClick);
            p_Button2.SetBounds(x + 18, 126, x, 20);

            p_Button3 = new Button(this, null, string.Empty, string.Empty, string.Empty);
            p_Button3.MouseClick += new MouseEventHandler(p_Button3_MouseClick);
            p_Button3.SetBounds((x * 2) + 24, 126, x, 20);

            switch (p_InfoButtons)
            {
                case InfoButtons.OK:
                    p_Button1.Visible = false;
                    p_Button2.Visible = false;
                    p_Button3.Text = "OK";
                    p_Button3.Result = DialogResult.OK;
                    break;
                case InfoButtons.OKCancel:
                    p_Button1.Visible = false;
                    p_Button2.Text = "OK";
                    p_Button2.Result = DialogResult.OK;
                    p_Button3.Text = "Cancel";
                    p_Button3.Result = DialogResult.Cancel;
                    break;
                case InfoButtons.YesNo:
                    p_Button1.Visible = false;
                    p_Button2.Text = "Yes";
                    p_Button2.Result = DialogResult.Yes;
                    p_Button3.Text = "No";
                    p_Button3.Result = DialogResult.No;
                    break;
                case InfoButtons.YesNoCancel:
                    p_Button1.Text = "Yes";
                    p_Button1.Result = DialogResult.Yes;
                    p_Button2.Text = "No";
                    p_Button2.Result = DialogResult.No;
                    p_Button3.Text = "Cancel";
                    p_Button3.Result = DialogResult.Cancel;
                    break;
                case InfoButtons.Abort:
                    p_Button1.Visible = false;
                    p_Button2.Visible = false;
                    p_Button3.Text = "Abort";
                    p_Button3.Result = DialogResult.Abort;
                    break;
                case InfoButtons.AbortRetry:
                    p_Button1.Visible = false;
                    p_Button2.Text = "Abort";
                    p_Button2.Result = DialogResult.Abort;
                    p_Button3.Text = "Retry";
                    p_Button3.Result = DialogResult.Retry;
                    break;
                case InfoButtons.AbortRetryIgnore:
                    p_Button1.Text = "Abort";
                    p_Button1.Result = DialogResult.Abort;
                    p_Button2.Text = "Retry";
                    p_Button2.Result = DialogResult.Retry;
                    p_Button3.Text = "Ignore";
                    p_Button3.Result = DialogResult.Ignore;
                    break;
            }
        }

        ~InfoDialog()
        {
            p_InfoButtons = InfoButtons.OK;
        }
        #endregion

        #region Child Events
        private void p_Button1_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = p_Button1.Result;
        }

        private void p_Button2_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = p_Button2.Result;
        }

        private void p_Button3_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = p_Button3.Result;
        }
        #endregion
    }
    #endregion

    #region InputDialog
    public class InputDialog : Dialog
    {
        #region Objects
        private Label p_Label;
        private TextBox p_TextBox;
        private Button p_ButtonOK, p_ButtonCancel;
        #endregion

        #region Properties
        public TextBox TextBox
        {
            get { return p_TextBox; }
        }
        #endregion

        #region Constructors/Destuctor
        public InputDialog(string message) : base()
        {
            Sizable = false;
            Size = new Size(260, 160);

            int x = (Width / 3) - 12;

            p_Label = new Label(this, null, message);
            p_Label.SetBounds(12, 30, Width - 24, 50);

            p_TextBox = new TextBox(this, null, string.Empty);
            p_TextBox.SetBounds(12, 90, Width - 24, 20);

            p_ButtonOK = new Button(this, null, string.Empty, "OK", string.Empty);
            p_ButtonOK.MouseClick += new MouseEventHandler(p_ButtonOK_MouseClick);
            p_ButtonOK.Result = DialogResult.OK;
            p_ButtonOK.SetBounds(x + 18, 126, x, 20);

            p_ButtonCancel  = new Button(this, null, string.Empty, "Cancel", string.Empty);
            p_ButtonCancel.MouseClick += new MouseEventHandler(p_ButtonCancel_MouseClick);
            p_ButtonCancel.Result = DialogResult.Cancel;
            p_ButtonCancel.SetBounds((x * 2) + 24, 126, x, 20);
        }

        public InputDialog(string message, string text) : base()
        {
            Sizable = false;
            Size = new Size(260, 160);

            int x = (Width / 3) - 12;

            p_Label = new Label(this, null, message);
            p_Label.SetBounds(12, 30, Width - 24, 50);

            p_TextBox = new TextBox(this, null, text);
            //p_TextBox.SelectionStart = p_TextBox.Text.Length;
            p_TextBox.SetBounds(12, 90, Width - 24, 20);

            p_ButtonOK = new Button(this, null, string.Empty, "OK", string.Empty);
            p_ButtonOK.MouseClick += new MouseEventHandler(p_ButtonOK_MouseClick);
            p_ButtonOK.Result = DialogResult.OK;
            p_ButtonOK.SetBounds(x + 18, 126, x, 20);

            p_ButtonCancel  = new Button(this, null, string.Empty, "Cancel", string.Empty);
            p_ButtonCancel.MouseClick += new MouseEventHandler(p_ButtonCancel_MouseClick);
            p_ButtonCancel.Result = DialogResult.Cancel;
            p_ButtonCancel.SetBounds((x * 2) + 24, 126, x, 20);
        }

        ~InputDialog()
        {
        }
        #endregion

        #region Child Events
        private void p_ButtonOK_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = p_ButtonOK.Result;
        }

        private void p_ButtonCancel_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = p_ButtonCancel.Result;
        }
        #endregion
    }
    #endregion

    #region Label
    public class Label : Control
    {
        #region Events
        public event EventHandler DrawSeparatorChanged;
        #endregion

        #region Objects
        private bool p_DrawSeparator;
        #endregion

        #region Properties
        public bool DrawSeparator
        {
            get { return p_DrawSeparator; }
            set
            {
                p_DrawSeparator = value;
                OnDrawSeparatorChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public Label(Window owner, Control parent, string text) : base(owner, parent)
        {
            DrawFocusRect = false;
            Text = text;

            p_DrawSeparator = false;
        }

        public override void Dispose()
        {
            p_DrawSeparator = false;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            RectangleF rf = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            if (RightToLeft) sf.Alignment = StringAlignment.Far;

            Color textColor = Global.Skin.Window.TextColor;

            if (!Enabled) textColor = Color.FromArgb(75, Global.Skin.Window.TextColor);

            g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), rf, sf);

            if (p_DrawSeparator) g.DrawLine(new Pen(Color.FromArgb(75, Global.Skin.Window.TextColor)), (bounds.X + Global.MeasureString(g, Text, Skin.WindowFont, rf).Width) + 10, bounds.Y + (bounds.Height / 2), bounds.Width, bounds.Y + (bounds.Height / 2));

            base.OnDraw(g, bounds);
        }
        #endregion

        #region Virtual Methods
        public virtual void OnDrawSeparatorChanged(EventArgs e)
        {
            if (DrawSeparatorChanged != null) DrawSeparatorChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Library
    public class Library : GenericList<LibraryItem>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllPlaylists = 0,
            WebPlaylists = 1,
            MyPlaylists = 2,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public Library(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Library", new string[] { "All Playlists (0)", "Web Playlists (0)", "My Playlists (0)" }, "Filter Library");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.AddRange(new UI.MenuItem[] { new UI.MenuItem("Í", "Load Library"), new UI.MenuItem("Í", "Edit Library..."), new UI.MenuItem("x", "Delete Library"), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("x", "New Library...") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public override void Dispose()
        {
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<LibraryItem> items = new List<LibraryItem>();

            foreach (LibraryItem item in Items)
            {
                if (item.Text.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllPlaylists:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.WebPlaylists:
                    foreach (LibraryItem item in items)
                    {
                        //if (item.Web) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.MyPlaylists:
                    foreach (LibraryItem item in items)
                    {
                        //if (item.Mine) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new GenericListSorter<LibraryItem>(GenericListSorter<LibraryItem>.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Playlists (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Web Playlists (" + Global.FormatNumber(GetWebPlaylistsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "My Playlists (" + Global.FormatNumber(GetMyPlaylistsCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SelectedItem != null) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Load Library":

                    break;
                case "Edit Library...":

                    break;
                case "Delete Library":

                    break;
                case "New Library...":

                    break;
            }
        }
        #endregion

        #region Public Methods
        public int GetWebPlaylistsCount()
        {
            int result = 0;

            return result;
        }

        public int GetMyPlaylistsCount()
        {
            int result = 0;

            return result;
        }
        #endregion
    }
    #endregion

    #region LibraryItem
    public class LibraryItem : GenericListItem
    {

    }
    #endregion

    #region List
    public class List : GenericList<ListItem>
    {
        #region Constructor/Destructor
        public List(Window owner, Control parent) : base(owner, parent)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
    #endregion

    #region ListItem
    public class ListItem : GenericListItem
    {
        #region Constructors/Destructor
        public ListItem() : base()
        {
        }

        public ListItem(string text) : base()
        {
            Text = text;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
    #endregion

    #region Menu
    public class Menu : Popup
    {
        #region Events
        public event EventHandler HotItemChanged;
        public event EventHandler SelectedItemChanged;
        public event EventHandler ItemsChanged;
        public event EventHandler ItemClicked;
        #endregion

        #region Objects
        private Control p_Parent;
        private List<MenuItem> p_Items;
        private int p_ItemHeight;
        private MenuItem p_HotItem, p_SelectedItem;
        #endregion

        #region Properties
        public List<MenuItem> Items
        {
            get { return p_Items; }
        }

        public int ItemHeight
        {
            get { return p_ItemHeight; }
            set
            {
                p_ItemHeight = value;
                //ReDraw();
            }
        }

        public MenuItem HotItem
        {
            get { return p_HotItem; }
            set
            {
                p_HotItem = value;
                OnHotItemChanged(new EventArgs());
                ReDraw();
            }
        }

        public MenuItem SelectedItem
        {
            get { return p_SelectedItem; }
            set
            {
                p_SelectedItem = value;
                OnSelectedItemChanged(new EventArgs());
                //ReDraw();
            }
        }

        public int HotIndex
        {
            get
            {
                if (p_HotItem == null) return -1;

                for (int i = 0; i < p_Items.Count; i++)
                {
                    if (p_Items[i].Equals(p_HotItem)) return i;
                }

                return -1;
            }
            set
            {
                if (value < -1) value = -1;
                if (value > p_Items.Count - 1) value = p_Items.Count - 1;

                for (int i = 0; i < p_Items.Count; i++)
                {
                    if (i == value)
                    {
                        if (p_Items[i].Text == "-")
                        {
                            HotItem = null;
                            return;
                        }

                        HotItem = p_Items[i];
                        return;
                    }
                }

                HotItem = null;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (p_SelectedItem == null) return -1;

                for (int i = 0; i < p_Items.Count; i++)
                {
                    if (p_Items[i].Equals(p_SelectedItem)) return i;
                }

                return -1;
            }
            set
            {
                if (value < -1) value = -1;
                if (value > p_Items.Count - 1) value = p_Items.Count - 1;

                for (int i = 0; i < p_Items.Count; i++)
                {
                    if (i == value)
                    {
                        if (p_Items[i].Text == "-")
                        {
                            SelectedItem = null;
                            return;
                        }

                        SelectedItem = p_Items[i];
                        return;
                    }
                }

                SelectedItem = null;
            }
        }
        #endregion

        #region Constructor/Destructor
        public Menu(Control parent) : base()
        {
            p_Parent = parent;
            p_Items = new List<MenuItem>();
            p_ItemHeight = 18;
            p_HotItem = null;
            p_SelectedItem = null;
        }

        public new void Dispose()
        {
            if (p_SelectedItem != null) p_SelectedItem.Dispose();
            if (p_HotItem != null) p_HotItem.Dispose();
            p_ItemHeight = 0;

            foreach (MenuItem item in p_Items)
            {
                item.Dispose();
            }
            p_Items.Clear();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void OnDraw(Graphics g)
        {
            DrawItems(g);

            base.OnDraw(g);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            HotItem = null;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int y = 3;

            for (int i = 0; i < p_Items.Count; i++)
            {
                if (e.Y > y && e.Y <= y + p_ItemHeight)
                {
                    if (HotIndex != i) HotIndex = i;
                    return;
                }

                y += p_ItemHeight;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                p_SelectedItem = p_HotItem;

                Visible = false;

                if (p_Parent != null && p_Parent is DropDown)
                {
                    DropDown parent = p_Parent as DropDown;
                    parent.SelectedIndex = SelectedIndex;
                }
                else
                {
                    if (p_SelectedItem != null)
                    {
                        OnItemClicked(new EventArgs());
                    }
                }

                Hide();
            }

            base.OnMouseUp(e);
        }
        #endregion

        #region Public Methods
        public void Add(MenuItem item)
        {
            p_Items.Add(item);
            OnItemsChanged(new EventArgs());
        }

        public void AddRange(MenuItem[] items)
        {
            p_Items.AddRange(items);
            OnItemsChanged(new EventArgs());
        }

        public void AddRange(List<MenuItem> items)
        {
            p_Items.AddRange(items);
            OnItemsChanged(new EventArgs());
        }

        public void Clear()
        {
            p_Items.Clear();
            OnItemsChanged(new EventArgs());
        }

        public void Remove(MenuItem item)
        {
            p_Items.Remove(item);
            OnItemsChanged(new EventArgs());
        }

        public void RemoveAt(int index)
        {
            p_Items.RemoveAt(index);
            OnItemsChanged(new EventArgs());
        }

        public new void Show()
        {
            if (p_Parent != null)
            {
                Location = p_Parent.Owner.PointToScreen(new Point(p_Parent.Bounds.X, p_Parent.Bounds.Y + p_Parent.Bounds.Height));
                if (p_Parent.Bounds.Width == 20) Size = new Size(80, (p_Items.Count * p_ItemHeight) + 6);
                else Size = new Size(p_Parent.Bounds.Width, (p_Items.Count * p_ItemHeight) + 6);

                if (Left > Screen.PrimaryScreen.Bounds.Width - Width) Left = Cursor.Position.X - Width;
                if (Top > Screen.PrimaryScreen.Bounds.Height - Height) Top = Cursor.Position.Y - Height;

                if (Handle.Equals(IntPtr.Zero)) base.CreateControl();
                Global.SetParent(Handle, IntPtr.Zero);
                Global.ShowWindow(Handle, 1);
                Global.SetForegroundWindow(Handle);
            }
            else
            {
                int width = 0;

                foreach (MenuItem item in p_Items)
                {
                    int x = Global.MeasureString(CreateGraphics(), item.Text, Skin.PopupFont, new RectangleF(0, 0, 1000, p_ItemHeight)).Width;

                    if (!string.IsNullOrEmpty(item.Icon)) x += 28;
                    else x += 8;
                    
                    if (x > width) width = x;
                }

                Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                Size = new Size(width, (p_Items.Count * p_ItemHeight) + 6);

                if (Left > Screen.PrimaryScreen.Bounds.Width - Width) Left = Cursor.Position.X - Width;
                if (Top > Screen.PrimaryScreen.Bounds.Height - Height) Top = Cursor.Position.Y - Height;

                if (Handle.Equals(IntPtr.Zero)) base.CreateControl();
                Global.SetParent(Handle, IntPtr.Zero);
                Global.ShowWindow(Handle, 1);
                Global.SetForegroundWindow(Handle);
            }
        }

        public new void Hide()
        {
            try
            {
                if (!Handle.Equals(IntPtr.Zero)) Global.ShowWindow(Handle, 0);
            }
            catch { }
        }
        #endregion

        #region Private Methods
        private void DrawItems(Graphics g)
        {
            if (p_Items.Count == 0) return; //?

            int y = 0;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            for (int i = 0; i < p_Items.Count; i++)
            {
                Color textColor = Global.Skin.Popup.TextColor;

                if (p_Items[i].Equals(p_HotItem))
                {
                    textColor = Global.Skin.Popup.Over_TextColor;

                    g.FillRectangle(new SolidBrush(Global.Skin.Popup.Over_BGColor), 3, y + 3, Width - 6, p_ItemHeight);
                }

                if (p_Items[i].Text == "-") g.DrawLine(new Pen(Global.Skin.Popup.LineColor), 3, (y + 3) + (p_ItemHeight / 2), Width - 6, (y + 3) + (p_ItemHeight / 2));
                else
                {
                    if (!string.IsNullOrEmpty(p_Items[i].Icon))
                    {
                        g.DrawString(p_Items[i].Icon, Skin.IconFont, new SolidBrush(textColor), 3, y + 3);
                        g.DrawString(p_Items[i].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(20, y + 3, Width - 6, p_ItemHeight), sf);
                    }
                    else g.DrawString(p_Items[i].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(3, y + 3, Width - 6, p_ItemHeight), sf);
                }

                y += p_ItemHeight;
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnHotItemChanged(EventArgs e)
        {
            if (HotItemChanged != null) HotItemChanged.Invoke(this, e);
        }
        
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            if (SelectedItemChanged != null) SelectedItemChanged.Invoke(this, e);
        }

        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (ItemsChanged != null) ItemsChanged.Invoke(this, e);
        }

        protected virtual void OnItemClicked(EventArgs e)
        {
            if (ItemClicked != null) ItemClicked.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region MenuItem
    public class MenuItem
    {
        #region Objects
        private string p_Icon;
        private string p_Text;
        #endregion

        #region Properties
        public string Icon
        {
            get { return p_Icon; }
            set { p_Icon = value; }
        }

        public string Text
        {
            get { return p_Text; }
            set { p_Text = value; }
        }
        #endregion

        #region Constructor/Destructor
        public MenuItem(string icon, string text)
        {
            p_Icon = icon;
            p_Text = text;
        }

        public void Dispose()
        {
            p_Text = string.Empty;
            p_Icon = string.Empty;
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            MenuItem item = obj as MenuItem;
            if (item == null) return false;

            return p_Text.ToLower().Equals(item.Text.ToLower());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return p_Text;
        }
        #endregion
    }
    #endregion

    #region Playlist
    public class Playlist : GenericList<PlaylistItem>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllMusic = 0,
            Favorites = 1,
            RecentlyPlayed = 2,
        }
        #endregion

        #region Callbacks
        public delegate void AddDirectoryCallback(string path);
        public delegate void RefreshCallback(bool chat);
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Button p_PlayButton;
        private TextBox p_ID3Title;
        private TextBox p_ID3Artist;
        private TextBox p_ID3Album;
        private TextBox p_ID3Genre;
        private TextBox p_ID3Track;
        private TextBox p_ID3TrackCount;
        private TextBox p_ID3Year;
        private ID3Rating p_ID3Rating;
        private Menu p_ContextMenu;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Button PlayButton
        {
            get { return p_PlayButton; }
        }

        public TextBox ID3Title
        {
            get { return p_ID3Title; }
        }

        public TextBox ID3Artist
        {
            get { return p_ID3Artist; }
        }

        public TextBox ID3Album
        {
            get { return p_ID3Album; }
        }

        public TextBox ID3Genre
        {
            get { return p_ID3Genre; }
        }

        public TextBox ID3Track
        {
            get { return p_ID3Track; }
        }

        public TextBox ID3TrackCount
        {
            get { return p_ID3TrackCount; }
        }

        public TextBox ID3Year
        {
            get { return p_ID3Year; }
        }

        public ID3Rating ID3Rating
        {
            get { return p_ID3Rating; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
                p_PlayButton.Visible = base.Visible;
                p_ID3Title.Visible = base.Visible;
                p_ID3Artist.Visible = base.Visible;
                p_ID3Album.Visible = base.Visible;
                p_ID3Genre.Visible = base.Visible;
                p_ID3Track.Visible = base.Visible;
                p_ID3TrackCount.Visible = base.Visible;
                p_ID3Year.Visible = base.Visible;
                p_ID3Rating.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public Playlist(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Playlist", new string[] { "All Music (0)", "Favorites (0)", "Recently Played (0)" }, "Filter Playlist");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_PlayButton = new Button(owner, this, "4", string.Empty, "Play");
            p_PlayButton.DrawDisabled = false;
            p_PlayButton.Enabled = false;
            p_PlayButton.MouseClick += new MouseEventHandler(p_PlayButton_MouseClick);

            p_ID3Title = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Title.DrawBackground = false;
            p_ID3Title.DrawFocusRect = true;
            p_ID3Title.Enabled = false;
            p_ID3Title.TextChanged += new EventHandler(p_ID3Title_TextChanged);

            p_ID3Artist = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Artist.DrawBackground = false;
            p_ID3Artist.Enabled = false;
            p_ID3Artist.TextChanged += new EventHandler(p_ID3Artist_TextChanged);

            p_ID3Album = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Album.DrawBackground = false;
            p_ID3Album.Enabled = false;
            p_ID3Album.TextChanged += new EventHandler(p_ID3Album_TextChanged);

            p_ID3Genre = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Genre.DrawBackground = false;
            p_ID3Genre.Enabled = false;
            p_ID3Genre.TextChanged += new EventHandler(p_ID3Genre_TextChanged);

            p_ID3Track = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Track.DrawBackground = false;
            p_ID3Track.Enabled = false;
            p_ID3Track.TextChanged += new EventHandler(p_ID3Track_TextChanged);

            p_ID3TrackCount = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3TrackCount.DrawBackground = false;
            p_ID3TrackCount.Enabled = false;
            p_ID3TrackCount.TextChanged += new EventHandler(p_ID3TrackCount_TextChanged);

            p_ID3Year = new TextBox(owner, this, string.Empty, string.Empty);
            p_ID3Year.DrawBackground = false;
            p_ID3Year.Enabled = false;
            p_ID3Year.TextChanged += new EventHandler(p_ID3Year_TextChanged);

            p_ID3Rating = new ID3Rating(owner, this);
            p_ID3Rating.Enabled = false;
            p_ID3Rating.RatingChanged += new EventHandler(p_ID3Rating_RatingChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.AddRange(new UI.MenuItem[] { new UI.MenuItem("4", "Play"), new UI.MenuItem(";", "Pause"), new UI.MenuItem("<", "Stop"), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("Í", "Rename..."), new UI.MenuItem("x", "Delete"), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("Y", "Add to Favorites") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public override void Dispose()
        {
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

            if (Enabled)
            {
                Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, Global.Skin.List.BGColor, Global.Skin.List.BGColor2, LinearGradientMode.Vertical));

                DrawID3(g, bounds);
                DrawItems(g, bounds);
            }

            Global.DrawRoundedRectangle(g, r, new Pen(Global.Skin.List.BorderColor));

            r = new Rectangle(bounds.X, (bounds.Y + bounds.Height) - 20, bounds.Width - 1, 19);
            LinearGradientBrush b = new LinearGradientBrush(r, Color.Transparent, Color.FromArgb(150, Global.Skin.List.BGColor2), LinearGradientMode.Vertical);
            b.WrapMode = WrapMode.TileFlipX;
            g.FillRectangle(b, r);

            //base.OnDraw(g, bounds);
        }

        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = bounds.Width / 2;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing playlist, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No files found, please refresh or change music folder.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if ((int)FilteredItems[i].Error != 0) textColor = Global.Skin.List.Stop_TextColor;
                    if (FilteredItems[i].Equals(PlayingItem)) textColor = Global.Skin.List.Play_TextColor;
                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    g.DrawString(FilteredItems[i].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight), sf);

                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<PlaylistItem> items = new List<PlaylistItem>();

            foreach (PlaylistItem item in Items)
            {
                item.Reformat();
                if (item.ToLongString().ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllMusic:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.Favorites:
                    foreach (PlaylistItem item in items)
                    {
                        if (item.Favorite) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.RecentlyPlayed:
                    foreach (PlaylistItem item in items)
                    {
                        if (item.PlayCount > 0) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new PlaylistSorter(PlaylistSorter.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            int x2 = Bounds.Width / 2;
            int x3 = (x2 - 106) / 2;

            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            if (Bounds.Height > 80) p_PlayButton.SetBounds(x2 + 34, 112, 20, 20);
            else p_PlayButton.SetBounds(x2 + 34, 112, 20, 0);

            if (Bounds.Height > 100) p_ID3Title.SetBounds(x2 + 96, 144, x2 - 106, 18);
            else p_ID3Title.SetBounds(x2 + 96, 144, x2 - 106, 0);

            if (Bounds.Height > 120) p_ID3Artist.SetBounds(x2 + 96, 164, x2 - 106, 18);
            else p_ID3Artist.SetBounds(x2 + 96, 164, x2 - 106, 0);

            if (Bounds.Height > 140) p_ID3Album.SetBounds(x2 + 96, 184, x2 - 106, 18);
            else p_ID3Album.SetBounds(x2 + 96, 184, x2 - 106, 0);

            if (Bounds.Height > 160) p_ID3Genre.SetBounds(x2 + 96, 204, x2 - 106, 18);
            else p_ID3Genre.SetBounds(x2 + 96, 204, x2 - 106, 0);

            if (Bounds.Height > 180)
            {
                p_ID3Track.SetBounds(x2 + 96, 224, x3 - 4, 18);
                p_ID3TrackCount.SetBounds((x2 + 96) + (x3 + 4), 224, x3 - 4, 18);
            }
            else
            {
                p_ID3Track.SetBounds(x2 + 96, 224, x3 - 4, 0);
                p_ID3TrackCount.SetBounds((x2 + 96) + (x3 + 4), 224, x3 - 4, 0);
            }

            if (Bounds.Height > 200) p_ID3Year.SetBounds(x2 + 96, 244, x2 - 106, 18);
            else p_ID3Year.SetBounds(x2 + 96, 244, x2 - 106, 0);

            if (Bounds.Height > 220 && Bounds.Width > 370) p_ID3Rating.SetBounds(x2 + 96, 264, 84, 20);
            else p_ID3Rating.SetBounds(x2 + 96, 264, 84, 0);

            base.OnBoundsChanged(e);
        }

        protected override void OnCurrentItemChanged(EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null)
            {
                p_PlayButton.Enabled = true;

                p_ID3Title.Enabled = true;
                p_ID3Title.Text = item.Title;

                p_ID3Artist.Enabled = true;
                p_ID3Artist.Text = item.Artist;

                p_ID3Album.Enabled = true;
                p_ID3Album.Text = item.Album;

                p_ID3Genre.Enabled = true;
                p_ID3Genre.Text = item.Genre;

                p_ID3Track.Enabled = true;
                p_ID3Track.Text = Global.FormatString(item.Track.ToString(), string.Empty);

                p_ID3TrackCount.Enabled = true;
                p_ID3TrackCount.Text = Global.FormatString(item.TrackCount.ToString(), string.Empty);

                p_ID3Year.Enabled = true;
                p_ID3Year.Text = Global.FormatString(item.Year.ToString(), string.Empty);

                p_ID3Rating.Enabled = true;
                p_ID3Rating.Rating = item.Rating;
            }
            else
            {
                p_PlayButton.Enabled = false;

                p_ID3Title.Text = string.Empty;
                p_ID3Title.Enabled = false;

                p_ID3Artist.Text = string.Empty;
                p_ID3Artist.Enabled = false;

                p_ID3Album.Text = string.Empty;
                p_ID3Album.Enabled = false;

                p_ID3Genre.Text = string.Empty;
                p_ID3Genre.Enabled = false;

                p_ID3Track.Text = string.Empty;
                p_ID3Track.Enabled = false;

                p_ID3TrackCount.Text = string.Empty;
                p_ID3TrackCount.Enabled = false;

                p_ID3Year.Text = string.Empty;
                p_ID3Year.Enabled = false;

                p_ID3Rating.Rating = 0.0d;
                p_ID3Rating.Enabled = false;
            }

            base.OnCurrentItemChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Music (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Favorites (" + Global.FormatNumber(GetFavoritesCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "Recently Played (" + Global.FormatNumber(GetRecentlyPlayedCount()) + ")";            
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int x = Bounds.Width / 2;

            if (e.X > Bounds.X + x) return;

            int y = 3;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (e.Y > Bounds.Y + y && e.Y <= (Bounds.Y + y) + ItemHeight)
                {
                    if (SelectedIndex != i) SelectedIndex = i;
                    return;
                }

                y += ItemHeight;
            }

            SelectedItem = null;

            //base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int x = Bounds.Width / 2;

            if (e.X > Bounds.X + x)
            {
                HotItem = null;
                return;
            }

            int y = 3;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (e.Y > Bounds.Y + y && e.Y <= (Bounds.Y + y) + ItemHeight)
                {
                    if (HotIndex != i)
                    {
                        HotIndex = i;

                        //if ((int)Owner.CreateGraphics().MeasureString(p_HotItem.Text, Global.Skin.TextFont).Width < Bounds.Width / 2) p_ToolTip.Hide();
                        //else if (Properties.Settings.Default.ToolTips)
                        //{
                        //p_ToolTip.Location = Owner.PointToScreen(new Point(Bounds.X + 3, (Bounds.Y + y)));
                        //p_ToolTip.Message = p_HotItem.Text;
                        //p_ToolTip.Show();
                        //}
                    }
                    return;
                }

                y += ItemHeight;
            }

            HotItem = null;

            //base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SelectedItem != null) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_PlayButton_MouseClick(object sender, MouseEventArgs e)
        {
            switch (p_PlayButton.Icon)
            {
                case "4":
                    if (Global.MediaPlayer.IsPaused()) Global.MediaPlayer.Resume("Resumed", true);
                    else Global.MediaPlayer.Play("Playing", true);
                    break;
                case ";":
                    if (Global.MediaPlayer.IsPlaying(false)) Global.MediaPlayer.Pause("Paused", true);
                    break;
            }
        }

        private void p_ID3Title_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Title != p_ID3Title.Text) item.Title = p_ID3Title.Text;
        }

        private void p_ID3Artist_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Artist != p_ID3Artist.Text) item.Artist = p_ID3Artist.Text;
        }

        private void p_ID3Album_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Album != p_ID3Album.Text) item.Album = p_ID3Album.Text;
        }

        private void p_ID3Genre_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Genre != p_ID3Genre.Text) item.Genre = p_ID3Genre.Text;
        }

        private void p_ID3Track_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Track.ToString() != p_ID3Track.Text) item.Track = Global.GetNullableShort(p_ID3Track.Text);
        }

        private void p_ID3TrackCount_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.TrackCount.ToString() != p_ID3TrackCount.Text) item.TrackCount = Global.GetNullableShort(p_ID3TrackCount.Text);
        }

        private void p_ID3Year_TextChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Year.ToString() != p_ID3Year.Text) item.Year = Global.GetNullableShort(p_ID3Year.Text);
        }

        private void p_ID3Rating_RatingChanged(object sender, EventArgs e)
        {
            PlaylistItem item = GetCurrentItem();

            if (item != null && item.Rating != p_ID3Rating.Rating) item.Rating = p_ID3Rating.Rating;
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            InfoDialog id = null;

            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Play":
                    Global.MediaPlayer.Play(SelectedItem, "Playing", true);
                    break;
                case "Pause":
                    if (Global.MediaPlayer.IsPlaying(false)) Global.MediaPlayer.Pause("Paused", true);
                    else Global.Steam.SendError("No file playing", true);
                    break;
                case "Stop":
                    if (Global.MediaPlayer.IsPlaying(true)) Global.MediaPlayer.Stop("Stopped", true);
                    else Global.Steam.SendError("No file playing", true);
                    break;
                case "Rename...":
                    InputDialog id2 = new InputDialog("Enter a new name for the file:" + Environment.NewLine + Environment.NewLine + Path.GetFileName(SelectedItem.URL), Path.GetFileNameWithoutExtension(SelectedItem.URL));
                    if (id2.ShowDialog(Owner) == DialogResult.OK)
                    {
                        string newFile = Path.GetDirectoryName(SelectedItem.URL) + "\\" + id2.TextBox.Text + Path.GetExtension(SelectedItem.URL);

                        if (Global.IsValidFile(id2.TextBox.Text.ToLower()))
                        {
                            File.Move(SelectedItem.URL, newFile);
                            SelectedItem.URL = newFile;
                        }
                        else
                        {
                            id = new InfoDialog("Invalid filename: " + newFile);
                            id.ShowDialog(Owner);
                            id.Dispose();
                        }
                    }

                    id2.Dispose();
                    break;
                case "Delete":
                    id = new InfoDialog("Are you sure you want to send this file to the recycle bin?" + Environment.NewLine + Environment.NewLine + Path.GetFileName(SelectedItem.URL), InfoDialog.InfoButtons.YesNo);
                    if (id.ShowDialog(Owner) == DialogResult.Yes)
                    {
                        if (Global.MediaPlayer.IsPlaying(true) && SelectedItem.Equals(PlayingItem)) Global.MediaPlayer.Stop(string.Empty, true);

                        Global.Recycle(SelectedItem.URL);
                        Remove(SelectedItem);

                        SelectedItem = null;
                    }

                    id.Dispose();
                    break;
                case "Add to favorites":
                    SelectedItem.Favorite = true;
                    break;
            }
        }
        #endregion

        #region Public Methods
        public int GetFavoritesCount()
        {
            int result = 0;

            foreach (PlaylistItem item in Items)
            {
                if (item.Favorite) result++;
            }

            return result;
        }

        public int GetRecentlyPlayedCount()
        {
            int result = 0;

            foreach (PlaylistItem item in Items)
            {
                if (item.PlayCount > 0) result++;
            }

            return result;
        }

        public void AddDirectory(string path)
        {
            if (Owner.InvokeRequired) Owner.Invoke(new AddDirectoryCallback(AddDirectory), new object[] { path });
            else
            {
                Refreshing = true;

                UI.RefreshDialog rd = new UI.RefreshDialog(path, "Searching for audio files, please wait...");
                rd.ShowDialog(Owner);

                AddRange(rd.Files);

                rd.Dispose();

                Refreshing = false;
            }
        }

        public void Refresh(bool chat)
        {
            if (Owner.InvokeRequired) Owner.Invoke(new RefreshCallback(Refresh), new object[] { chat });
            else
            {
                long size = 0;
                long duration = 0;

                Refreshing = true;

                RefreshDialog rd = new RefreshDialog(Global.Settings.MusicFolder, "Refreshing playlist, please wait...");
                rd.ShowDialog(Owner);

                foreach (PlaylistItem item in rd.Files)
                {
                    foreach (PlaylistItem item2 in Items)
                    {
                        if (item.Equals(item2))
                        {
                            item.Artist = item2.Artist;
                            item.Title = item2.Title;
                            item.Album = item2.Album;
                            item.Genre = item2.Genre;
                            item.Track = item2.Track;
                            item.Year = item2.Year;
                            item.Rating = item2.Rating;
                            item.PlayCount = item2.PlayCount;
                            item.LastPlayed = item2.LastPlayed;
                            item.Favorite = item2.Favorite;
                            item.Updated = item2.Updated;
                            item.Duration = item2.Duration; //?
                            item.Image = item2.Image;
                            //item.Alpha = item2.Alpha;

                            continue;
                        }
                    }

                    size += item.Size;
                    duration += item.Duration.Ticks;
                }

                Items.Clear();
                AddRange(rd.Files);

                Global.Steam.SendMessage("Playlist refreshed, Files: {ls}" + Global.FormatNumber(Items.Count) + "{rs}" + Environment.NewLine + "Size: {ls}" + Global.ConvertBytes(size) + "{rs} Duration: {ls}" + Global.ConvertTicks(duration, false) + "{rs} Time: {ls}" + Global.ConvertMilliseconds(rd.ElapsedTime, true) + "{rs}", chat);

                rd.Dispose();

                Refreshing = false;
            }
        }
        #endregion

        #region Private Methods
        private void DrawID3(Graphics g, Rectangle bounds)
        {
            Font f = new Font(Skin.WindowFont.Name, 14.0f, FontStyle.Regular);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.SetTabStops(34.0f, new float[] { 34.0f });

            int x = (bounds.Width / 2) - 80;
            int x2 = ((bounds.Width - x) - 36) / 2;
            int w = 0;
            PlaylistItem item = null;
            Color bgColor = Global.Skin.List.Play_BGColor;
            Color textColor = Global.Skin.List.Play_TextColor;
            Rectangle r;

            if (PlayingItem == null)
            {
                item = SelectedItem;
                bgColor = Global.Skin.List.Stop_BGColor;
                textColor = Global.Skin.List.Stop_TextColor;
            }
            else item = PlayingItem;

            if (item != null)
            {
                r = new Rectangle((bounds.X + x) + 6, bounds.Y + 1, (bounds.Width - x) - 7, bounds.Height - 2);

                LinearGradientBrush b = new LinearGradientBrush(r, Color.Transparent, Color.FromArgb(100, bgColor), LinearGradientMode.Horizontal);
                b.WrapMode = WrapMode.TileFlipX;

                g.FillRectangle(b, r);

                if (item.Image != null)
                {
                    var ratioX = (double)(bounds.Width - 1) / item.Image.Width;
                    var ratioY = (double)(bounds.Height - 1) / item.Image.Height;
                    var ratio = Math.Min(ratioX, ratioY);
                    var newWidth = (int)(item.Image.Width * ratio);
                    var newHeight = (int)(item.Image.Height * ratio);

                    if (newHeight < bounds.Height - 2) r = new Rectangle(bounds.X, bounds.Y + 1, bounds.Width - 1, bounds.Height - 1);
                    else r = new Rectangle((bounds.X + bounds.Width) - newWidth, bounds.Y + 1, newWidth, newHeight);

                    ColorMatrix cm = new ColorMatrix();

                    cm.Matrix00 = (float)bgColor.R / 255.0f;
                    cm.Matrix11 = (float)bgColor.G / 255.0f;
                    cm.Matrix22 = (float)bgColor.B / 255.0f;
                    cm.Matrix33 = 0.5f;

                    ImageAttributes ia = new ImageAttributes();
                    ia.SetColorMatrix(cm);

                    g.DrawImage(item.Image, r, 0, 0, item.Image.Width, item.Image.Height, GraphicsUnit.Pixel, ia);
                }

                x = bounds.Width / 2;

                if (bounds.Height > 40)
                {
                    w = (int)g.MeasureString(item.Text, f).Width + 6;
                    if (bounds.Width < (x + w) + 40) w = (bounds.Width - x) - 36;

                    r = new Rectangle((bounds.X + x) + 16, bounds.Y + 6, w, 34);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, Global.Skin.List.BGColor2)), r);

                    g.DrawString(item.Text, f, new SolidBrush(Global.Skin.List.Over_TextColor), RectangleF.FromLTRB(r.Left, r.Top, r.Right, r.Bottom), sf);
                }

                if (bounds.Height > 80)
                {
                    string s = "LAST PLAYED " + Global.FormatString(item.LastPlayed, "NEVER");

                    w = (int)g.MeasureString(s, Skin.WindowFont).Width + 36;
                    if (bounds.Width < (x + w) + 40) w = (bounds.Width - x) - 36;

                    r = new Rectangle((bounds.X + x) + 16, bounds.Y + 40, w, 34);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(150, Global.Skin.List.BGColor)), r);

                    g.DrawString(item.PlayCount.ToString() + " TIMES PLAYED", Skin.WindowFont, new SolidBrush(Global.Skin.List.Over_TextColor), new RectangleF((bounds.X + x) + 48, bounds.Y + 42, (bounds.Width - x) - 70, 15), sf);
                    g.DrawString(s, Skin.WindowFont, new SolidBrush(Global.Skin.List.Over_TextColor), new RectangleF((bounds.X + x) + 48, bounds.Y + 58, (bounds.Width - x) - 70, 15), sf);
                }

                if (bounds.Height > 100) g.DrawString("Title:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 80, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 120) g.DrawString("Artist:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 100, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 140) g.DrawString("Album:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 120, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 160) g.DrawString("Genre:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 140, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 180)
                {
                    g.DrawString("Track:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 160, (bounds.Width - x) - 36, 15), sf);
                    if (bounds.Width > 260) g.DrawString("/", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + (x2 + 5), bounds.Y + 160, 8, 15), sf);
                }
                if (bounds.Height > 200) g.DrawString("Year:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 180, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 220) g.DrawString("Rating:", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 200, (bounds.Width - x) - 36, 15), sf);

                if (bounds.Height > 240)
                {
                    r = new Rectangle((bounds.X + x) + 16, bounds.Y + 228, (bounds.Width - x) - 40, 1);
                    g.FillRectangle(new LinearGradientBrush(r, textColor, Color.Transparent, LinearGradientMode.Horizontal), r);
                }

                if (bounds.Height > 260) g.DrawString("Type: \t" + Global.FormatString(item.FileType, "N/A"), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 240, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 280) g.DrawString("Encoder: \t" + Global.FormatString(item.Encoder, "N/A"), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 260, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 300) g.DrawString("Bitrate: \t" + Global.FormatBitrate(item), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 280, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 320) g.DrawString("Frequency: \t" + item.Frequency.ToString() + " Hz", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 300, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 340) g.DrawString("Duration: \t" + Global.ConvertTime(item.Duration, false), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 320, (bounds.Width - x) - 36, 15), sf);
                if (bounds.Height > 360) g.DrawString("Size: \t" + Global.ConvertBytes(item.Size), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 16, bounds.Y + 340, (bounds.Width - x) - 36, 15), sf);
            }
        }
        #endregion
    }
    #endregion

    #region PlaylistItem
    public class PlaylistItem : GenericListItem
    {
        #region Enums
        public enum ErrorCodes : int
        {
            None = 0,
            FileNotFound = 101,
            ErrorReadingID3 = 102,
            ErrorReadingCache = 103,
        }

        public enum SearchProviders : int
        {
            LastFM,
            GoogleImages,
        }

        public enum ImageSizes : int
        {
            Small,
            Medium,
            Large,
        }
        #endregion

        #region Objects
        private ErrorCodes p_Error;
        private string p_URL, p_Artist, p_Title, p_Album, p_Genre;
        private short? p_Track, p_TrackCount, p_Year;
        private double p_Rating;
        private string p_FileType, p_Encoder;
        private int p_PlayCount;
        private string p_LastPlayed, p_Mode;
        private int p_Layers, p_Bitrate, p_Frequency;
        private bool p_Copyright, p_Original, p_Protection, p_VBR;
        private TimeSpan p_Duration;
        private long p_Size;
        private bool p_Favorite, p_Updated;
        private Bitmap p_Image;
        private WebClient p_WebClient;
        //private float p_Alpha;
        //private Timer p_AlphaTimer;
        private SearchProviders p_SearchProvider;
        private ImageSizes p_ImageSize;
        #endregion

        #region Properties
        public ErrorCodes Error
        {
            get { return p_Error; }
        }

        public string URL
        {
            get { return p_URL; }
            set
            {
                p_URL = value;
                //p_Updated = true;

                Reformat();
            }
        }

        public string Artist
        {
            get { return p_Artist; }
            set
            {
                p_Artist = value;
                p_Updated = true;

                Reformat();
            }
        }

        public string Title
        {
            get { return p_Title; }
            set
            {
                p_Title = value;
                p_Updated = true;

                Reformat();
            }
        }

        public string Album
        {
            get { return p_Album; }
            set
            {
                p_Album = value;
                p_Updated = true;

                Reformat();
            }
        }

        public string Genre
        {
            get { return p_Genre; }
            set
            {
                p_Genre = value;
                p_Updated = true;

                Reformat();
            }
        }

        public short? Track
        {
            get { return p_Track; }
            set
            {
                p_Track = value;
                p_Updated = true;

                Reformat();
            }
        }

        public short? TrackCount
        {
            get { return p_TrackCount; }
            set
            {
                p_TrackCount = value;
                p_Updated = true;

                Reformat();
            }
        }

        public short? Year
        {
            get { return p_Year; }
            set
            {
                p_Year = value;
                p_Updated = true;

                Reformat();
            }
        }

        public double Rating
        {
            get { return p_Rating; }
            set
            {
                p_Rating = value;
                p_Updated = true;

                Reformat();
            }
        }

        public string FileType
        {
            get { return p_FileType; }
        }

        public string Encoder
        {
            get { return p_Encoder; }
        }

        public int PlayCount
        {
            get { return p_PlayCount; }
            set
            {
                p_PlayCount = value;
                p_Updated = true;
            }
        }

        public string LastPlayed
        {
            get { return p_LastPlayed; }
            set { p_LastPlayed = value; }
        }

        public string Mode
        {
            get { return p_Mode; }
        }

        public int Layers
        {
            get { return p_Layers; }
        }

        public int Bitrate
        {
            get { return p_Bitrate; }
        }

        public int Frequency
        {
            get { return p_Frequency; }
        }

        public bool Copyright
        {
            get { return p_Copyright; }
        }

        public bool Original
        {
            get { return p_Original; }
        }

        public bool Protection
        {
            get { return p_Protection; }
        }

        public bool VBR
        {
            get { return p_VBR; }
        }

        public TimeSpan Duration
        {
            get { return p_Duration; }
            set { p_Duration = value; }
        }

        public long Size
        {
            get { return p_Size; }
        }

        public bool Favorite
        {
            get { return p_Favorite; }
            set { p_Favorite = value; }
        }

        public bool Updated
        {
            get { return p_Updated; }
            set { p_Updated = value; }
        }

        public Bitmap Image
        {
            get { return p_Image; }
            set { p_Image = value; }
        }

        //public float Alpha
        //{
            //get { return p_Alpha; }
            //set { p_Alpha = value; }
        //}
        #endregion

        #region Constructors/Destructor
        public PlaylistItem(string data, bool xml) : base()
        {
            p_Error = ErrorCodes.None;

            if (xml) LoadXML(data);
            else LoadURL(data);

            if (string.IsNullOrEmpty(p_URL) || !File.Exists(p_URL)) p_Error = ErrorCodes.FileNotFound;
            p_Updated = false;
            p_Image = null;
            p_WebClient = null;
            //p_Alpha = 0.0f;
            //p_AlphaTimer = null;

            p_SearchProvider = SearchProviders.LastFM;
            p_ImageSize = ImageSizes.Medium;

            //Reformat();
        }

        public PlaylistItem(string url, string artist, string title, string genre, int bitrate)
        {
            p_Error = ErrorCodes.None;
            p_URL = url;
            p_Artist = artist;
            p_Title = title;
            p_Album = string.Empty;
            p_Genre = genre;
            p_Track = null;
            p_TrackCount = null;
            p_Year = null;
            p_Rating = 0.0d;
            p_FileType = Global.GetFileDescription(p_URL); //?
            p_Encoder = string.Empty;
            p_PlayCount = 0; //?
            p_LastPlayed = string.Empty;
            p_Mode = string.Empty;
            p_Layers = 0;
            p_Bitrate = bitrate;
            p_Frequency = 0;
            p_Copyright = false;
            p_Original = false;
            p_Protection = false;
            p_VBR = false;
            p_Duration = new TimeSpan(0);
            p_Size = 0;
            p_Favorite = false;
            p_Updated = false;
            p_Image = null;
            p_WebClient = null;
            p_SearchProvider = SearchProviders.LastFM;
            p_ImageSize = ImageSizes.Medium;

            Reformat();
        }

        public override void Dispose()
        {
            p_ImageSize = ImageSizes.Medium;
            p_SearchProvider = SearchProviders.LastFM;
            //if (p_AlphaTimer != null) p_AlphaTimer.Dispose();
            //p_Alpha = 0.0f;
            if (p_WebClient != null) p_WebClient.Dispose();
            if (p_Image != null) p_Image.Dispose();
            p_Updated = false;
            p_Favorite = false;
            p_Size = 0;
            p_Duration = new TimeSpan(0);
            p_VBR = false;
            p_Protection = false;
            p_Original = false;
            p_Copyright = false;
            p_Frequency = 0;
            p_Bitrate = 0;
            p_Layers = 0;
            p_Mode = string.Empty;
            p_LastPlayed = string.Empty;
            p_PlayCount = 0;
            p_Encoder = string.Empty;
            p_FileType = string.Empty;
            p_Rating = 0.0d;
            p_Year = 0;
            p_TrackCount = 0;
            p_Track = 0;
            p_Genre = string.Empty;
            p_Album = string.Empty;
            p_Title = string.Empty;
            p_Artist = string.Empty;
            p_URL = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_URL;
        }
        #endregion

        #region Child Events
        private void p_WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MemoryStream ms = new MemoryStream(e.Result);

                p_Image = Global.AdjustAlpha((Bitmap)System.Drawing.Image.FromStream(ms));

                p_WebClient.Dispose(); //?

                //p_Alpha = 0.5f;
                Global.MainWindow.Playlist.ReDraw();

                //p_AlphaTimer = new Timer();
                //p_AlphaTimer.Interval = 100;
                //p_AlphaTimer.Tick += new EventHandler(p_AlphaTimer_Tick);
                //p_AlphaTimer.Start();

                //Global.MainWindow.Playlist.Fade();
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string size = string.Empty;

                switch (p_ImageSize)
                {
                    case ImageSizes.Small:
                        size = "medium";
                        break;
                    case ImageSizes.Medium:
                        size = "large";
                        break;
                    case ImageSizes.Large:
                        size = "extralarge";
                        break;
                }

                switch (p_SearchProvider)
                {
                    case SearchProviders.LastFM:
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(e.Result);

                        XmlNodeList nodeList = xml.SelectNodes("lfm/images/image");

                        if (nodeList != null && nodeList.Count > 0)
                        {
                            XmlNodeList nodeList2 = nodeList[Global.RandomNumber(nodeList.Count - 1, true)].SelectNodes("sizes/size");

                            if (nodeList2 != null && nodeList2.Count > 0)
                            {
                                foreach (XmlNode node in nodeList2)
                                {
                                    if (Global.GetXmlValue(node, "name", string.Empty) == size)
                                    {
                                        p_WebClient.DownloadDataAsync(new Uri(node.InnerText));
                                        return;
                                    }
                                }
                            }
                        }
                        break;
                    case SearchProviders.GoogleImages:
                        string[] result = e.Result.Split(new string[] { "unescapedUrl\":\"" }, StringSplitOptions.RemoveEmptyEntries);

                        int rand = Global.RandomNumber(result.GetUpperBound(0), false);

                        for (int i = 1; i < result.GetUpperBound(0); i++)
                        {
                            if (i == rand)
                            {
                                string[] result2 = result[i].Split(new string[] { "\",\"url" }, StringSplitOptions.RemoveEmptyEntries);

                                p_WebClient.DownloadDataAsync(new Uri(result2[0]));
                            }
                        }
                        break;
                }
            }
        }

        private void p_AlphaTimer_Tick(object sender, EventArgs e)
        {
            //if (p_Alpha < 0.5f)
            //{
                //p_Alpha += 0.05f;
                //Global.MainWindow.Playlist.ReDraw();
            //}
            //else
            //{
                //p_AlphaTimer.Stop();
                //p_AlphaTimer.Dispose();
            //}
        }
        #endregion

        #region Public Methods
        public void Reformat()
        {
            string text = Global.Settings.PlaylistFormat.ToLower();

            if (string.IsNullOrEmpty(p_Title) && string.IsNullOrEmpty(p_Artist))
            {
                string[] s = Path.GetFileNameWithoutExtension(p_URL).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                switch (s.GetUpperBound(0))
                {
                    case 1:
                        text = text.Replace("{artist}", Global.FormatString(s[0], "N/A"));
                        text = text.Replace("{title}", Global.FormatString(s[1], "N/A"));
                        text = text.Replace("{album}", "N/A");
                        text = text.Replace("{genre}", "N/A");
                        text = text.Replace("{track}", "N/A");
                        text = text.Replace("{trackcount}", "N/A");
                        text = text.Replace("{year}", "N/A");
                        break;
                    case 2:
                        text = text.Replace("{artist}", Global.FormatString(s[0], "N/A"));
                        text = text.Replace("{album}", Global.FormatString(s[1], "N/A"));
                        text = text.Replace("{title}", Global.FormatString(s[2], "N/A"));
                        text = text.Replace("{genre}", "N/A");
                        text = text.Replace("{track}", "N/A");
                        text = text.Replace("{trackcount}", "N/A");
                        text = text.Replace("{year}", "N/A");
                        break;
                    case 3:
                        text = text.Replace("{artist}", Global.FormatString(s[0], "N/A"));
                        text = text.Replace("{album}", Global.FormatString(s[1], "N/A"));
                        text = text.Replace("{track}", Global.FormatString(s[2], "N/A"));
                        text = text.Replace("{title}", Global.FormatString(s[3], "N/A"));
                        text = text.Replace("{genre}", "N/A");
                        text = text.Replace("{trackcount}", "N/A");
                        text = text.Replace("{year}", "N/A");
                        break;
                    default:
                        text = Path.GetFileNameWithoutExtension(p_URL);
                        break;
                }
            }
            else
            {
                text = text.Replace("{artist}", Global.FormatString(p_Artist, "N/A"));
                text = text.Replace("{title}", Global.FormatString(p_Title, "N/A"));
                text = text.Replace("{album}", Global.FormatString(p_Album, "N/A"));
                text = text.Replace("{genre}", Global.FormatString(p_Genre, "N/A"));
                text = text.Replace("{track}", Global.FormatTrack(p_Track, "N/A"));
                text = text.Replace("{trackcount}", Global.FormatTrack(p_TrackCount, "N/A"));
                text = text.Replace("{year}", Global.FormatTrack(p_Year, "N/A"));
            }

            text = text.Replace("{duration}", Global.ConvertTime(p_Duration, false));
            text = text.Replace("{url}", Path.GetFileNameWithoutExtension(p_URL));

            //if (string.IsNullOrEmpty(p_Title) && string.IsNullOrEmpty(p_Artist)) text = Path.GetFileNameWithoutExtension(p_URL);

            Text = text;
        }

        public string ToLongString()
        {
            string result = Text + Environment.NewLine;
            result += p_Artist + Environment.NewLine;
            result += p_Title + Environment.NewLine;
            result += p_Album + Environment.NewLine;
            result += p_Genre + Environment.NewLine;
            result += p_Track + Environment.NewLine;
            result += p_TrackCount + Environment.NewLine;
            result += p_Year + Environment.NewLine;
            result += p_URL;

            return result;
        }

        public XmlElement ToXml(XmlDocument xml)
        {
            XmlElement element = xml.CreateElement("File");

            element.SetAttribute("URL", p_URL);
            element.SetAttribute("Artist", p_Artist);
            element.SetAttribute("Title", p_Title);
            element.SetAttribute("Album", p_Album);
            element.SetAttribute("Genre", p_Genre);
            element.SetAttribute("Track", p_Track.ToString());
            element.SetAttribute("TrackCount", p_TrackCount.ToString());
            element.SetAttribute("Year", p_Year.ToString());
            element.SetAttribute("Rating", p_Rating.ToString());
            element.SetAttribute("FileType", p_FileType);
            element.SetAttribute("Encoder", p_Encoder);
            element.SetAttribute("PlayCount", p_PlayCount.ToString());
            element.SetAttribute("LastPlayed", p_LastPlayed);
            element.SetAttribute("Mode", p_Mode);
            element.SetAttribute("Layers", p_Layers.ToString());
            element.SetAttribute("Bitrate", p_Bitrate.ToString());
            element.SetAttribute("Frequency", p_Frequency.ToString());
            element.SetAttribute("Copyright", Global.BoolToString(p_Copyright, "0", "1"));
            element.SetAttribute("Original", Global.BoolToString(p_Original, "0", "1"));
            element.SetAttribute("Protection", Global.BoolToString(p_Protection, "0", "1"));
            element.SetAttribute("VBR", Global.BoolToString(p_VBR, "0", "1"));
            element.SetAttribute("Duration", p_Duration.Ticks.ToString());
            element.SetAttribute("Size", p_Size.ToString());
            element.SetAttribute("Favorite", Global.BoolToString(p_Favorite, "0", "1"));

            return element;
        }

        public void Update()
        {
            try
            {
                PlayCount++;

                p_LastPlayed = DateTime.Now.ToShortDateString();
                if (File.Exists(p_URL)) File.SetLastAccessTime(p_URL, DateTime.Now);

                if (p_Image == null) DownloadImage(SearchProviders.LastFM, ImageSizes.Large);
            }
            catch
            {
            }
        }
        #endregion

        #region Private Methods
        private void DownloadImage(SearchProviders searchProvider, ImageSizes imageSize)
        {
            try
            {
                string artist = p_Artist;
                string size = string.Empty;

                if (string.IsNullOrEmpty(artist))
                {
                    string[] s = Path.GetFileNameWithoutExtension(p_URL).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.GetUpperBound(0) > 0) artist = Global.FormatString(s[0], string.Empty);
                }

                if (string.IsNullOrEmpty(artist)) return;

                p_WebClient = new WebClient();
                p_WebClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(p_WebClient_DownloadDataCompleted);
                p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);

                p_SearchProvider = searchProvider;
                p_ImageSize = imageSize;

                switch (p_ImageSize)
                {
                    case ImageSizes.Small:
                        size = "small";
                        break;
                    case ImageSizes.Medium:
                        size = "medium";
                        break;
                    case ImageSizes.Large:
                        size = "large";
                        break;
                }

                switch (p_SearchProvider)
                {
                    case SearchProviders.LastFM:
                        p_WebClient.DownloadStringAsync(new Uri("http://ws.audioscrobbler.com/2.0/?method=artist.getimages&artist=" + HttpUtility.UrlEncode(artist) + "&api_key=ff35c0a7a8dcb9bd5689034b1fb35e88"));
                        break;
                    case SearchProviders.GoogleImages:
                        p_WebClient.DownloadStringAsync(new Uri("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&q=" + HttpUtility.UrlEncode(artist + " music") + "&as_filetype=jpg&imgsz=" + size + "&imgtype=face&safe=active&rsz=8")); //&as_sitesearch=last.fm&rsz=10&start=" + Global.RandomNumber(10, false).ToString())); //&key=INSERT-YOUR-KEY //&imgsz=medium|large|xlarge|xxlarge|huge
                        break;
                }
            }
            catch { }
        }

        private void LoadXML(string data)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            XmlNode node = xml.SelectSingleNode("File");
            if (node == null)
            {
                p_Error = ErrorCodes.ErrorReadingCache;

                p_URL = string.Empty;
                p_Artist = string.Empty;
                p_Title = string.Empty;
                p_Album = string.Empty;
                p_Genre = string.Empty;
                p_Track = null;
                p_TrackCount = null;
                p_Year = null;
                p_Rating = 0.0d;
                p_FileType = string.Empty;
                p_Encoder = string.Empty;
                p_PlayCount = 0;
                p_LastPlayed = string.Empty;
                p_Mode = string.Empty;
                p_Layers = 0;
                p_Bitrate = 0;
                p_Frequency = 0;
                p_Copyright = false;
                p_Original = false;
                p_Protection = false;
                p_VBR = false;
                p_Duration = new TimeSpan(0);
                p_Size = 0;
                p_Favorite = false;
            }
            else
            {
                try
                {
                    p_URL = Global.GetXmlValue(node, "URL", string.Empty);
                    p_Artist = Global.GetXmlValue(node, "Artist", string.Empty);
                    p_Title = Global.GetXmlValue(node, "Title", string.Empty);
                    p_Album = Global.GetXmlValue(node, "Album", string.Empty);
                    p_Genre = Global.GetXmlValue(node, "Genre", string.Empty);
                    p_Track = Global.GetNullableShort(Global.GetXmlValue(node, "Track", string.Empty));
                    p_TrackCount = Global.GetNullableShort(Global.GetXmlValue(node, "TrackCount", string.Empty));
                    p_Year = Global.GetNullableShort(Global.GetXmlValue(node, "Year", string.Empty));
                    p_Rating = Convert.ToDouble(Global.GetXmlValue(node, "Rating", "0"));
                    p_FileType = Global.GetXmlValue(node, "FileType", string.Empty);
                    p_Encoder = Global.GetXmlValue(node, "Encoder", string.Empty);
                    p_PlayCount = Convert.ToInt32(Global.GetXmlValue(node, "PlayCount", "0"));
                    p_LastPlayed = Global.GetXmlValue(node, "LastPlayed", string.Empty);
                    p_Mode = Global.GetXmlValue(node, "Mode", string.Empty);
                    p_Layers = Convert.ToInt32(Global.GetXmlValue(node, "Layers", "0"));
                    p_Bitrate = Convert.ToInt32(Global.GetXmlValue(node, "Bitrate", "0"));
                    p_Frequency = Convert.ToInt32(Global.GetXmlValue(node, "Frequency", "0"));
                    p_Copyright = Global.StringToBool(Global.GetXmlValue(node, "Copyright", "0"), "1");
                    p_Original = Global.StringToBool(Global.GetXmlValue(node, "Original", "0"), "1");
                    p_Protection = Global.StringToBool(Global.GetXmlValue(node, "Protection", "0"), "1");
                    p_VBR = Global.StringToBool(Global.GetXmlValue(node, "VBR", "0"), "1");
                    p_Duration = new TimeSpan(Convert.ToInt64(Global.GetXmlValue(node, "Duration", "0")));
                    p_Size = Convert.ToInt64(Global.GetXmlValue(node, "Size", "0"));
                    p_Favorite = Global.StringToBool(Global.GetXmlValue(node, "Favorite", "0"), "1");
                }
                catch
                {
                    p_Error = ErrorCodes.ErrorReadingCache;
                }
            }
        }

        private void LoadURL(string url)
        {
            p_URL = url;

            FileInfo fi = new FileInfo(p_URL);

            try
            {
                Global.MediaPlayer.ID3.Read(p_URL);

                p_Artist = Global.MediaPlayer.ID3.ID3v2Tag.Artist.Trim();
                p_Title = Global.MediaPlayer.ID3.ID3v2Tag.Title.Trim();
                p_Album = Global.MediaPlayer.ID3.ID3v2Tag.Album.Trim();
                p_Genre = Global.MediaPlayer.ID3.ID3v2Tag.Genre.Trim();
                p_Track = Global.MediaPlayer.ID3.ID3v2Tag.TrackNum;
                p_TrackCount = Global.MediaPlayer.ID3.ID3v2Tag.TrackCount;
                p_Year = Global.MediaPlayer.ID3.ID3v2Tag.Year;

                //ID3FrameCollection frames = Global.MediaPlayer.ID3.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Picture);
                //if (frames.Count > 0)
                //{
                    //ID3v23PictureFrame frame = (ID3v23PictureFrame)frames[0];
                    //p_Image = frame.Picture;
                //}
                //else p_Image = null;

                //if (p_Image == null)
                //{
                    //frames = Global.MediaPlayer.ID3.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v22Picture);
                    //if (frames.Count > 0)
                    //{
                        //ID3v22PictureFrame frame = (ID3v22PictureFrame)frames[0];
                        //p_Image = frame.Picture;
                    //}
                    //else (p_Image = null);
                //}

                ID3FrameCollection frames = Global.MediaPlayer.ID3.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Popularimeter);
                if (frames.Count > 0)
                {
                    ID3v23PopularimeterFrame ratingFrame = (ID3v23PopularimeterFrame)frames[0];

                    byte? rating = ratingFrame.Rating;
                    //int rating = Convert.ToInt32(ratingFrame.Rating);

                    if (rating == null) p_Rating = 0.0d;
                    else if (rating > 230) p_Rating = 5.0d;
                    else if (rating > 205) p_Rating = 4.5d;
                    else if (rating > 179) p_Rating = 4.0d;
                    else if (rating > 154) p_Rating = 3.5d;
                    else if (rating > 129) p_Rating = 3.0d;
                    else if (rating > 104) p_Rating = 2.5d;
                    else if (rating > 78) p_Rating = 2.0d;
                    else if (rating > 53) p_Rating = 1.5d;
                    else if (rating > 28) p_Rating = 1.0d;
                    else if (rating > 3) p_Rating = 0.5d;
                    else p_Rating = 0.0d;
                }
                else p_Rating = 0.0d;

                ID3v23EncodedByFrame encodedByFrame = (ID3v23EncodedByFrame)Global.MediaPlayer.ID3.ID3v2Tag.Frames.GetFrame(CommonSingleInstanceID3v2FrameTypes.EncodedBy);
                if (encodedByFrame != null && encodedByFrame.IsSet) p_Encoder = encodedByFrame.EncodedBy.Trim();
                else p_Encoder = string.Empty;

                ID3v23PlayCounterFrame playCountFrame = (ID3v23PlayCounterFrame)Global.MediaPlayer.ID3.ID3v2Tag.Frames.GetFrame(CommonSingleInstanceID3v2FrameTypes.PlayCounter);
                if (playCountFrame != null && playCountFrame.IsSet)
                {
                    if (playCountFrame.Counter == null) p_PlayCount = 0;
                    else p_PlayCount = Convert.ToInt32(playCountFrame.Counter);
                }
                else p_PlayCount = 0;

                p_Mode = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.Mode.ToString();

                switch (Global.MediaPlayer.ID3.FirstMPEGFrameInfo.Layer)
                {
                    case LayerTypes.MPEGLayer1:
                        p_Layers = 1;
                        break;
                    case LayerTypes.MPEGLayer2:
                        p_Layers = 2;
                        break;
                    case LayerTypes.MPEGLayer3:
                        p_Layers = 3;
                        break;
                    default:
                        p_Layers = 0;
                        break;
                }

                p_Bitrate = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.Bitrate;
                p_Frequency = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.Frequency;

                p_Copyright = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.CopyrightFlag;
                p_Original = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.OriginalFlag;
                p_Protection = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.ProtectionFlag;
                p_VBR = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.VBRInfo.WasFound;

                p_Duration = Global.MediaPlayer.ID3.FirstMPEGFrameInfo.Duration;
            }
            catch
            {
                p_Error = ErrorCodes.ErrorReadingID3;

                p_Artist = string.Empty;
                p_Title = string.Empty;
                p_Album = string.Empty;
                p_Genre = string.Empty;
                p_Track = null;
                p_TrackCount = null;
                p_Year = null;
                p_Rating = 0.0d;
                //p_FileType = string.Empty;
                p_Encoder = string.Empty;
                p_PlayCount = 0;
                //p_LastPlayed = string.Empty;
                p_Mode = string.Empty;
                p_Copyright = false;
                p_Original = false;
                p_Protection = false;
                p_VBR = false;
                p_Layers = 0;
                p_Bitrate = 0;
                p_Frequency = 0;
                p_Duration = new TimeSpan(0);
            }

            p_FileType = Global.GetFileDescription(p_URL);
            p_LastPlayed = fi.LastAccessTime.ToShortDateString();
            p_Size = fi.Length;
            p_Favorite = false;

            if (p_Duration.TotalMilliseconds == 0 && p_Bitrate != 0) //?
            {
                int kbFileSize = (int)((8 * p_Size) / 1000);
                p_Duration = new TimeSpan(0, 0, 0, (int)(kbFileSize / p_Bitrate));
            }
        }
        #endregion
    }
    #endregion

    #region PlaylistSorter
    public class PlaylistSorter : IComparer<PlaylistItem>
    {
        #region Enums
        public enum SortMode : int
        {
            None,
            Ascending,
            Descending,
            PlayCount,
            URL,
            Rating,
        }
        #endregion

        #region Objects
        private SortMode p_Mode;
        #endregion

        #region Properties
        public SortMode Mode
        {
            get { return p_Mode; }
            set { p_Mode = value; }
        }
        #endregion

        #region Constructor/Destructor
        public PlaylistSorter(SortMode mode) : base()
        {
            p_Mode = mode;
        }

        public void Dispose()
        {
            p_Mode = SortMode.None;
        }
        #endregion

        #region Public Methods
        public int Compare(PlaylistItem item1, PlaylistItem item2)
        {
            switch (p_Mode)
            {
                case SortMode.Ascending:
                    return item1.Text.ToLower().CompareTo(item2.Text.ToLower());
                case SortMode.Descending:
                    return item2.Text.ToLower().CompareTo(item1.Text.ToLower());
                case SortMode.PlayCount:
                    return item1.PlayCount.CompareTo(item2.PlayCount);
                case SortMode.URL:
                    return item1.URL.ToLower().CompareTo(item2.URL.ToLower());
                case SortMode.Rating:
                    return item1.Rating.CompareTo(item2.Rating);
                default:
                    return 0;
            }
        }
        #endregion
    }
    #endregion

    #region PluginList
    public class PluginList : GenericList<PluginListItem>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllPlugins,
            TrustedPlugins,
            LocalPlugins,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public PluginList(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Plug-ins", new string[] { "All Plug-ins (0)", "Trusted Plug-ins (0)", "Local Plug-ins (0)" }, "Filter Plug-ins");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            p_WebClient = new WebClient();
            p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);
        }

        public override void Dispose()
        {
            p_WebClient.Dispose();
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int x2 = x / 3;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing plug-ins, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No plug-ins found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (FilteredItems[i].IPlugin != null) textColor = Global.Skin.List.Play_TextColor;
                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    sf.Alignment = StringAlignment.Near;

                    g.DrawString(FilteredItems[i].Name, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x2, ItemHeight), sf);
                    g.DrawString(FilteredItems[i].Description, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + 3) + x2, (bounds.Y + y) + 3, (x2 * 2) - 34, ItemHeight), sf);

                    sf.Alignment = StringAlignment.Far;

                    if (FilteredItems[i].URL.StartsWith("http://")) g.DrawString("Trusted", Skin.WindowFont, new SolidBrush(Global.Skin.List.Play_TextColor), new RectangleF((bounds.X + x) - 60, (bounds.Y + y) + 3, 60, ItemHeight), sf);
                    //else g.DrawString(FilteredItems[i].Version, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 60, (bounds.Y + y) + 3, 60, ItemHeight), sf);
                    
                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<PluginListItem> items = new List<PluginListItem>();

            foreach (PluginListItem item in Items)
            {
                if (item.Name.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllPlugins:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.TrustedPlugins:
                    foreach (PluginListItem item in items)
                    {
                        if (item.URL.StartsWith("http://")) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.LocalPlugins:
                    foreach (PluginListItem item in items)
                    {
                        if (!item.URL.StartsWith("http://")) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new GenericListSorter<PluginListItem>(GenericListSorter<PluginListItem>.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Plug-ins (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Trusted Plug-ins (" + Global.FormatNumber(GetTrustedPluginsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "Local Plug-ins (" + Global.FormatNumber(GetLocalPluginsCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (SelectedItem == null) return;

            if (e.Button == MouseButtons.Left && Clicks > 1)
            {
                if (SelectedItem.IPlugin != null)
                {
                    Plugins.UIPlugin uiPlugin = SelectedItem.IPlugin as Plugins.UIPlugin;
                    if (uiPlugin != null)
                    {
                        if (uiPlugin.WindowState == FormWindowState.Minimized) uiPlugin.Restore();
                        Global.SetForegroundWindow(uiPlugin.Handle);
                    }
                    return;
                }

                SelectedItem.Run();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                p_ContextMenu.Clear();

                if (SelectedItem != null) p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("~", "Run Plug-in"), new MenuItem(string.Empty, "-") });
                p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("q", "Refresh Plug-ins"), new MenuItem(string.Empty, "-"), new MenuItem("Í", "Compile Plug-in...") });

                p_ContextMenu.Show();
            }

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Run Plug-in":
                    if (SelectedItem.IPlugin != null)
                    {
                        Plugins.UIPlugin uiPlugin = SelectedItem.IPlugin as Plugins.UIPlugin;
                        if (uiPlugin != null) Global.SetForegroundWindow(uiPlugin.Handle);
                        return;
                    }

                    SelectedItem.Run();
                    break;
                case "Refresh Plug-ins":
                    Refresh();
                    break;
                case "Compile Plug-in...":
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.CheckFileExists = true;
                    ofd.Filter = "C# Class (*.cs)|*.cs";
                    ofd.InitialDirectory = Application.StartupPath + "\\Plugins";
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog(Owner) == DialogResult.OK)
                    {
                        Plugins.Compiler compiler = new Plugins.Compiler(ofd.FileName);

                        if (compiler.Errors.Count > 0)
                        {
                            ErrorDialog ed = new ErrorDialog(compiler.Errors);
                            ed.ShowDialog(Owner);
                            ed.Dispose();
                        }
                        else
                        {
                            InfoDialog id = new InfoDialog("Plug-in '" + Path.GetFileNameWithoutExtension(ofd.FileName) + ".dll' compiled successfully.");
                            id.ShowDialog(Owner);
                            id.Dispose();

                            Refresh();
                        }

                        compiler.Dispose();
                    }

                    ofd.Dispose();
                    break;
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    List<PluginListItem> list = new List<PluginListItem>();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(e.Result);
                    XmlNodeList nodeList = xml.SelectNodes("Steamp3.TrustedPlugins/TrustedPlugin");

                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(new PluginListItem(Global.GetXmlValue(node, "URL", string.Empty), Global.GetXmlValue(node, "Name", "N/A"), Global.GetXmlValue(node, "Company", "N/A"), Global.GetXmlValue(node, "Desc", "N/A"), Global.GetXmlValue(node, "Copyright", "N/A"), Global.GetXmlValue(node, "Version", "1.0")));
                    }

                    if (Directory.Exists(Application.StartupPath + "\\Plugins"))
                    {
                        string[] files = Directory.GetFiles(Application.StartupPath + "\\Plugins", "*.dll");

                        foreach (string file in files)
                        {
                            list.Add(new PluginListItem(file));
                        }
                    }

                    Items.Clear();
                    AddRange(list);

                    list.Clear();
                }
                catch { }

                Refreshing = false;
            }
        }
        #endregion

        #region Public Methods
        public int GetTrustedPluginsCount()
        {
            int result = 0;

            foreach (PluginListItem item in Items)
            {
                if (item.URL.StartsWith("http://")) result++;
            }

            return result;
        }

        public int GetLocalPluginsCount()
        {
            int result = 0;

            foreach (PluginListItem item in Items)
            {
                if (!item.URL.StartsWith("http://")) result++;
            }

            return result;
        }

        public void Refresh()
        {
            if (!Global.MediaPlayer.IsOnline) return;

            Refreshing = true;

            p_WebClient.DownloadStringAsync(new Uri("http://steamp3.ta0soft.com/plugins/Steamp3.TrustedPlugins.xml"));
        }
        #endregion
    }
    #endregion

    #region PluginListItem
    public class PluginListItem : GenericListItem
    {
        #region Objects
        private string p_URL, p_Name, p_Company, p_Description, p_Copyright, p_Version;

        private List<string> p_Errors;
        //private Assembly p_Assembly;
        //private ConstructorInfo p_ConstructorInfo;
        private Plugins.IPlugin p_IPlugin;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public string Name
        {
            get { return p_Name; }
        }

        public string Company
        {
            get { return p_Company; }
        }

        public string Description
        {
            get { return p_Description; }
        }

        public string Copyright
        {
            get { return p_Copyright; }
        }

        public string Version
        {
            get { return p_Version; }
        }

        public List<string> Errors
        {
            get { return p_Errors; }
            set { p_Errors = value; }
        }

        //public Assembly Assembly
        //{
            //get { return p_Assembly; }
        //}

        //public ConstructorInfo ConstructorInfo
        //{
            //get { return p_ConstructorInfo; }
        //}

        public Plugins.IPlugin IPlugin
        {
            get { return p_IPlugin; }
            set { p_IPlugin = value; }
        }
        #endregion

        #region Constructors/Destructor
        public PluginListItem(string url) : base()
        {
            p_URL = url;

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(p_URL);

            p_Name = fvi.ProductName;
            p_Company = fvi.CompanyName;
            p_Description = fvi.Comments;
            p_Copyright = fvi.LegalCopyright;
            p_Version = fvi.ProductVersion;

            p_Errors = new List<string>();
            //p_Assembly = blah;
            //p_ConstructorInfo = blah;
            p_IPlugin = null;
            p_WebClient = null;
        }

        public PluginListItem(string url, string name, string company, string description, string copyright, string version) : base()
        {
            if (p_WebClient != null) p_WebClient.Dispose();
            p_URL = url;
            p_Name = name;
            p_Company = company;
            p_Description = description;
            p_Copyright = copyright;
            p_Version = version;

            p_Errors = new List<string>();
            //p_Assembly = blah;
            //p_ConstructorInfo = blah;
            p_IPlugin = null;
        }

        public override void Dispose()
        {
            //p_IPlugin.Dispose();
            //p_ConstructorInfo = null;
            //p_Assembly.Dispose();
            p_Errors.Clear();

            p_Version = string.Empty;
            p_Copyright = string.Empty;
            p_Description = string.Empty;
            p_Company = string.Empty;
            p_Name = string.Empty;
            p_URL = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_URL;
        }
        #endregion

        #region Child Events
        private void uiPlugin_FormClosing(object sender, FormClosingEventArgs e)
        {
            p_IPlugin = null;
        }

        private void p_WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                RunAssembly(Assembly.Load(e.Result));
            }
        }
        #endregion

        #region Public Methods
        public void Run()
        {
            if (p_URL.StartsWith("http://"))
            {
                p_WebClient = new WebClient();
                p_WebClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(p_WebClient_DownloadDataCompleted);

                p_WebClient.DownloadDataAsync(new Uri(p_URL));
            }
            else RunAssembly(Assembly.LoadFile(p_URL));
        }
        #endregion

        #region Private Methods
        private void RunAssembly(Assembly assembly)
        {
            try
            {
                p_Errors.Clear();

                foreach (Type type in assembly.GetExportedTypes())
                {
                    foreach (Type iface in type.GetInterfaces())
                    {
                        if (iface == typeof(Plugins.IPlugin))
                        {
                            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                            //MethodInfo methodInfo = type.GetMethod("Dispose");

                            if (constructorInfo == null || !constructorInfo.IsPublic)
                            {
                                p_Errors.Add("No public constructor found");
                                return;
                            }

                            foreach (MemberInfo memberInfo in type.GetMembers())
                            {
                                if (memberInfo.DeclaringType == typeof(Window)) // UIPlugin
                                {
                                    Plugins.UIPlugin uiPlugin = constructorInfo.Invoke(null) as Plugins.UIPlugin;
                                    uiPlugin.FormClosing += new FormClosingEventHandler(uiPlugin_FormClosing);
                                    uiPlugin.Show();

                                    p_IPlugin = uiPlugin;
                                    return;
                                }
                            }

                            // Plugin
                            p_IPlugin = constructorInfo.Invoke(null) as Plugins.Plugin;
                            p_IPlugin = null;
                            return;
                        }
                    }
                }

                p_Errors.Add("Class must inherit IPlugin interface");
            }
            catch (Exception ex)
            {
                p_Errors.Add(ex.ToString());
            }
        }
        #endregion
    }
    #endregion

    #region Popup
    public class Popup : Window
    {
        #region Objects
        private Timer p_Timer;
        #endregion

        #region Constructor
        public Popup() : base()
        {
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(0, 0);
            Popup = true;
            Movable = false;
            Sizable = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = string.Empty;

            p_Timer = new Timer();
            p_Timer.Interval = 100;
            p_Timer.Tick += new EventHandler(p_Timer_Tick);
            p_Timer.Start();
        }
        #endregion

        #region Overrides
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                //cp.ClassName = "TOOLTIPS_CLASS";
                cp.ExStyle = (int)Global.WindowStylesEx.WS_EX_NOACTIVATE | (int)Global.WindowStylesEx.WS_EX_TOOLWINDOW | (int)Global.WindowStylesEx.WS_EX_TOPMOST;
                cp.Style = (int)Global.WindowStyles.WS_POPUP;
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }

        //protected override void OnDeactivate(EventArgs e)
        //{
            ////p_Timer.Stop();
            //Hide();

            //base.OnDeactivate(e);
        //}
        #endregion

        #region Child Events
        private void p_Timer_Tick(object sender, EventArgs e)
        {
            if (!Visible) return;

            if (!Global.GetForegroundWindow().Equals(Handle))
            {
                //p_Timer.Stop();
                Hide();
            }
        }
        #endregion
    }
    #endregion

    #region RefreshDialog
    public class RefreshDialog : Dialog
    {
        #region Objects
        private string p_Path;
        private Label p_WaitLabel;
        private Label p_CurrentLabel;
        private Slider p_CurrentProgress;
        private Label p_FilesLabel;
        private Slider p_TotalProgress;
        private Label p_ElapsedLabel, p_RemainingLabel;
        private Button p_PauseButton, p_CancelButton;

        private List<PlaylistItem> p_Files;
        private int p_StartTime, p_ElapsedTime, p_DirectoryCount;
        private DirectoryInfo p_DirInfo;
        private int p_Level;
        private bool p_Paused;
        private Timer p_ETATimer;
        #endregion

        #region Properties
        public List<PlaylistItem> Files
        {
            get { return p_Files; }
        }

        public int StartTime
        {
            get { return p_StartTime; }
        }

        public int ElapsedTime
        {
            get { return p_ElapsedTime; }
        }
        #endregion

        #region Constructor/Destructor
        public RefreshDialog(string path, string message) : base()
        {
            Sizable = false;
            Size = new Size(300, 238);

            p_Path = path;

            p_WaitLabel = new Label(this, null, message);

            p_CurrentLabel = new Label(this, null, "Current directory: " + Environment.NewLine + p_Path);

            p_CurrentProgress = new Slider(this, null);

            p_FilesLabel = new Label(this, null, "0 Files found in 0 directories (0%)");

            p_TotalProgress = new Slider(this, null);

            p_ElapsedLabel = new Label(this, null, "Elapsed: 00:00");

            p_RemainingLabel = new Label(this, null, "Remaining: 00:00");
            p_RemainingLabel.RightToLeft = true;

            p_PauseButton = new Button(this, null, "Pause");
            p_PauseButton.MouseClick += new MouseEventHandler(p_PauseButton_MouseClick);
            p_PauseButton.Enabled = false;

            p_CancelButton = new Button(this, null, "Cancel");
            p_CancelButton.MouseClick += new MouseEventHandler(p_CancelButton_MouseClick);

            p_Files = new List<PlaylistItem>();
            p_StartTime = 0;
            p_ElapsedTime = 0;
            p_DirectoryCount = 0;
            p_DirInfo = null;
            p_Level = 0;
            p_Paused = false;

            p_ETATimer = new Timer();
            p_ETATimer.Interval = 1000;
            p_ETATimer.Tick += new EventHandler(p_ETATimer_Tick);
        }

        ~RefreshDialog()
        {
            p_ETATimer.Dispose();
            p_Paused = false;
            p_DirInfo = null;
            p_Level = 0;
            p_DirectoryCount = 0;
            p_ElapsedTime = 0;
            p_StartTime = 0;

            foreach (PlaylistItem item in p_Files)
            {
                item.Dispose();
            }
            p_Files.Clear();

            p_Path = string.Empty;
        }
        #endregion

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            if (!Visible) return;

            int x = (Width / 2) - 18;

            p_WaitLabel.SetBounds(12, 30, Width - 24, 20);
            p_CurrentLabel.SetBounds(12, 56, Width - 24, 40);
            p_CurrentProgress.SetBounds(12, 102, Width - 24, 20);
            p_FilesLabel.SetBounds(12, 128, Width - 24, 20);
            p_TotalProgress.SetBounds(12, 154, Width - 24, 20);
            p_ElapsedLabel.SetBounds(12, 180, x, 20);
            p_RemainingLabel.SetBounds(x + 24, 180, x, 20);
            p_PauseButton.SetBounds(12, 206, x, 20);
            p_CancelButton.SetBounds(x + 24, 206, x, 20);

            base.OnResize(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            p_ETATimer.Start();

            if (!Refresh())
            {
                if (p_Level == 0) DialogResult = DialogResult.Cancel;
            }
            else DialogResult = DialogResult.OK;
        }
        #endregion

        #region Child Events
        private void p_PauseButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (p_PauseButton.Text == "Pause")
            {
                p_Paused = true;
                p_PauseButton.Text = "Resume";
            }
            else if (p_DirInfo != null && p_Level > 0)
            {
                p_Paused = false;
                p_PauseButton.Text = "Pause";

                Refresh(p_DirInfo, p_Level);
            }
        }
        
        private void p_CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void p_ETATimer_Tick(object sender, EventArgs e)
        {
            if (p_TotalProgress.Value > 0)
            {
                int eta = (p_ElapsedTime / p_TotalProgress.Value) * (p_TotalProgress.Maximum - p_TotalProgress.Value);

                p_ElapsedLabel.Text = "Elapsed: " + Global.ConvertMilliseconds(p_ElapsedTime, false);
                p_RemainingLabel.Text = "Remaining: " + Global.ConvertMilliseconds(eta, false);
            }
        }
        #endregion

        #region Private Methods
        private new bool Refresh()
        {
            try
            {
                if (!Directory.Exists(p_Path)) return false; //?

                DirectoryInfo dirInfo = new DirectoryInfo(p_Path);

                p_StartTime = Environment.TickCount;

                FileInfo[] fileInfo = dirInfo.GetFiles();
                foreach (FileInfo fi in fileInfo)
                {
                    string[] filter = Global.Settings.MusicFilter.Split(new char[] { ';' });

                    foreach (string ext in filter)
                    {
                        if (Path.GetExtension(fi.FullName).ToLower().Equals(ext.ToLower())) p_Files.Add(new PlaylistItem(fi.FullName, false));
                    }
                }

                DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
                p_DirectoryCount += subDirInfo.Length;

                for (int i = 0; i < subDirInfo.Length; i++)
                {
                    if (p_Paused)
                    {
                        p_DirInfo = subDirInfo[i];
                        p_Level = 1;
                        return false;
                    }

                    if (DialogResult == DialogResult.Cancel) return false;

                    p_TotalProgress.Maximum = subDirInfo.Length - 1;
                    p_TotalProgress.Value = i;

                    if (!Refresh(subDirInfo[i])) return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            //catch (Exception ex)
            //{
            //}

            return true;
        }

        private bool Refresh(DirectoryInfo dirInfo, int level = 1)
        {
            try
            {
                FileInfo[] fileInfo = dirInfo.GetFiles();
                p_CurrentLabel.Text = "Current folder: " + Environment.NewLine + dirInfo.FullName;
                foreach (FileInfo fi in fileInfo)
                {
                    string[] filter = Global.Settings.MusicFilter.Split(new char[] { ';' });

                    foreach (string ext in filter)
                    {
                        if (Path.GetExtension(fi.FullName).ToLower().Equals(ext.ToLower())) p_Files.Add(new PlaylistItem(fi.FullName, false));
                    }
                }

                Application.DoEvents();

                DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
                p_DirectoryCount += subDirInfo.Length;

                p_FilesLabel.Text = Global.FormatNumber(p_Files.Count) + " Files found in " + Global.FormatNumber(p_DirectoryCount) + " directories (" + Global.GetPercent(p_TotalProgress.Value, p_TotalProgress.Maximum) + "%)";

                for (int i = 0; i < subDirInfo.Length; i++)
                {
                    if (p_Paused)
                    {
                        p_DirInfo = subDirInfo[i];
                        p_Level = level + 1;
                        return false;
                    }

                    if (DialogResult == DialogResult.Cancel) return false;

                    p_CurrentProgress.Maximum = subDirInfo.Length - 1;
                    p_CurrentProgress.Value = i;

                    if (!Refresh(subDirInfo[i], level + 1)) return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            //catch (Exception ex)
            //{
            //}

            p_ElapsedTime = Environment.TickCount - p_StartTime;

            return true;
        }
        #endregion
    }
    #endregion

    #region ScrollBar
    public class ScrollBar : ControlGroup
    {
        #region Events
        public event EventHandler MaximumChanged;
        public event EventHandler ValueChanged;
        #endregion

        #region Objects
        private int p_Maximum, p_Value;

        private ScrollBarButton p_UpButton;
        private ScrollBarSlider p_Slider;
        private ScrollBarButton p_DownButton;
        #endregion

        #region Properties
        public int Maximum
        {
            get { return p_Maximum; }
            set
            {
                if (value < 0) value = 0;
                if (value < p_Value) p_Value = 0;

                p_Maximum = value;
                p_Slider.Maximum = p_Maximum;

                OnMaximumChanged(new EventArgs());
                //ReDraw();
            }
        }

        public int Value
        {
            get { return p_Value; }
            set
            {
                if (value < 0) value = 0;
                if (value > p_Maximum) value = p_Maximum;

                p_Value = value;
                p_Slider.Value = p_Value;

                OnValueChanged(new EventArgs());
                //p_Slider.Owner.ReDrawControl(p_Slider);
            }
        }

        public ScrollBarButton UpButton
        {
            get { return p_UpButton; }
        }

        public ScrollBarSlider Slider
        {
            get { return p_Slider; }
        }

        public ScrollBarButton DownButton
        {
            get { return p_DownButton; }
        }

        public override Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;

                if (base.Bounds.Height > 80)
                {
                    p_UpButton.SetBounds(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, 22);
                    p_Slider.SetBounds(base.Bounds.X, base.Bounds.Y + 24, base.Bounds.Width, base.Bounds.Height - 48);
                    p_DownButton.SetBounds(base.Bounds.X, (base.Bounds.Y + base.Bounds.Height) - 22, base.Bounds.Width, 22);
                }
                else
                {
                    int y = (base.Bounds.Height / 3) - 3;

                    p_UpButton.SetBounds(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, y);
                    p_Slider.SetBounds(base.Bounds.X, (base.Bounds.Y + y) + 2, base.Bounds.Width, y + 5);
                    p_DownButton.SetBounds(base.Bounds.X, (base.Bounds.Y + base.Bounds.Height) - y, base.Bounds.Width, y);
                }

                OnBoundsChanged(new EventArgs());
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_UpButton.Enabled = base.Enabled;
                p_Slider.Enabled = base.Enabled;
                p_DownButton.Enabled = base.Enabled;

                OnEnabledChanged(new EventArgs());
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_UpButton.Visible = base.Visible;
                p_Slider.Visible = base.Visible;
                p_DownButton.Visible = base.Visible;

                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public ScrollBar(Window owner, Control parent) : base()
        {
            p_Maximum = 0;
            p_Value = 0;

            p_UpButton = new ScrollBarButton(owner, parent, true);
            p_UpButton.TimerTick += new EventHandler(p_UpButton_TimerTick);

            p_Slider = new ScrollBarSlider(owner, parent);
            p_Slider.ValueChanged += new EventHandler(p_Slider_ValueChanged);

            p_DownButton = new ScrollBarButton(owner, parent, false);
            p_DownButton.TimerTick += new EventHandler(p_DownButton_TimerTick);
        }

        public override void Dispose()
        {
            p_Value = 0;
            p_Maximum = 0;

            base.Dispose();
        }
        #endregion

        #region Child Events
        private void p_UpButton_TimerTick(object sender, EventArgs e)
        {
            Value--;
        }

        private void p_Slider_ValueChanged(object sender, EventArgs e)
        {
            p_Value = p_Slider.Value;
            OnValueChanged(new EventArgs());
        }

        private void p_DownButton_TimerTick(object sender, EventArgs e)
        {
            Value++;
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnMaximumChanged(EventArgs e)
        {
            if (MaximumChanged != null) MaximumChanged.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null) ValueChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ScrollBarButton
    public class ScrollBarButton : Control
    {
        #region Events
        public event EventHandler TimerTick;
        #endregion

        #region Objects
        private Timer p_Timer;
        #endregion

        #region Constructor/Destructor
        public ScrollBarButton(Window owner, Control parent, bool up) : base(owner, parent)
        {
            DrawFocusRect = false;

            if (up) Icon = "5";
            else Icon = "6";

            p_Timer = new Timer();
            p_Timer.Interval = 200;
            p_Timer.Tick += new EventHandler(p_Timer_Tick);
        }

        public override void Dispose()
        {
            p_Timer.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            int y = (Bounds.Height / 2) - 9;
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

            Color bgColor = Global.Skin.ScrollBar.BGColor;
            Color textColor = Global.Skin.ScrollBar.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    bgColor = Global.Skin.ScrollBar.Down_BGColor;
                    textColor = Global.Skin.ScrollBar.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    bgColor = Global.Skin.ScrollBar.Over_BGColor;
                    textColor = Global.Skin.ScrollBar.Over_TextColor;
                }
            }
            else textColor = Color.FromArgb(75, Global.Skin.ScrollBar.TextColor);

            Global.FillRoundedRectangle(g, r, new SolidBrush(bgColor));
            Global.DrawRoundedRectangle(g, r, new Pen(bgColor));

            g.DrawString(Icon, Skin.IconFont, new SolidBrush(textColor), bounds.X - 2, bounds.Y + y);

            base.OnDraw(g, bounds);
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnTimerTick(new EventArgs());

                p_Timer.Interval = 200;
                p_Timer.Start();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            p_Timer.Stop();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_Timer_Tick(object sender, EventArgs e)
        {
            if (p_Timer.Interval > 10) p_Timer.Interval -= 10;

            OnTimerTick(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnTimerTick(EventArgs e)
        {
            if (TimerTick != null) TimerTick.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ScrollBarSlider
    public class ScrollBarSlider : Control
    {
        #region Events
        public event EventHandler MaximumChanged;
        public event EventHandler ValueChanged;
        public event EventHandler SliderHeightChanged;
        #endregion

        #region Objects
        private int p_Maximum, p_Value, p_SliderHeight;
        #endregion

        #region Properties
        public int Maximum
        {
            get { return p_Maximum; }
            set
            {
                if (value < 0) value = 0;
                if (value < p_Value) p_Value = 0;

                p_Maximum = value;
                //SliderHeight = Global.GetPercent(p_Value, p_Maximum, Bounds.Height - 1);
                OnMaximumChanged(new EventArgs());
                ReDraw();
            }
        }

        public int Value
        {
            get { return p_Value; }
            set
            {
                if (value < 0) value = 0;
                if (value > p_Maximum) value = p_Maximum;

                p_Value = value;
                //SliderHeight = Global.GetPercent(p_Value, p_Maximum, Bounds.Height - 1);
                OnValueChanged(new EventArgs());
                ReDraw();
            }
        }

        public int SliderHeight
        {
            get { return p_SliderHeight; }
            set
            {
                if (value < 12) value = 12;
                if (value > Bounds.Height) value = Bounds.Height;

                p_SliderHeight = value;
                OnSliderHeightChanged(new EventArgs());
                //ReDraw();
            }
        }
        #endregion

        #region Constructor/Destructor
        public ScrollBarSlider(Window owner, Control parent) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_Maximum = 0;
            p_Value = 0;
            p_SliderHeight = 12;
        }

        public override void Dispose()
        {
            p_SliderHeight = 0;
            p_Value = 0;
            p_Maximum = 0;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            int y = (bounds.Y + Global.GetPercent(p_Value, p_Maximum, Bounds.Height - p_SliderHeight)) + (p_SliderHeight / 2) - 2;
            //int y = (bounds.Y - 4) + (p_SliderHeight / 2);
            Rectangle r = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
            LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.ScrollBar.BGColor2, Color.Transparent, LinearGradientMode.Vertical);
            b.WrapMode = WrapMode.TileFlipX;

            Color bgColor = Global.Skin.ScrollBar.BGColor;
            Color textColor = Global.Skin.ScrollBar.TextColor;

            if (Enabled)
            {
                if (IsMouseDown)
                {
                    bgColor = Global.Skin.ScrollBar.Down_BGColor;
                    textColor = Global.Skin.ScrollBar.Down_TextColor;
                }
                else if (IsMouseMoving)
                {
                    bgColor = Global.Skin.ScrollBar.Over_BGColor;
                    textColor = Global.Skin.ScrollBar.Over_TextColor;
                }
            }
            else textColor = Color.FromArgb(75, Global.Skin.ScrollBar.TextColor);

            g.FillRectangle(b, r);
            //Global.FillRoundedRectangle(g, r, b);

            r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            Global.DrawRoundedRectangle(g, r, new Pen(b));

            r = new Rectangle(bounds.X, bounds.Y + Global.GetPercent(p_Value, p_Maximum, Bounds.Height - p_SliderHeight), bounds.Width - 1, p_SliderHeight - 1);
            Global.FillRoundedRectangle(g, r, new SolidBrush(bgColor));
            Global.DrawRoundedRectangle(g, r, new Pen(bgColor));

            r = new Rectangle(bounds.X + 3, y, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 5, y, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 7, y, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 9, y, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 3, y + 2, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 5, y + 2, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 7, y + 2, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            r = new Rectangle(bounds.X + 9, y + 2, 1, 1);
            g.FillRectangle(new SolidBrush(textColor), r);

            if (p_SliderHeight > 20)
            {
                r = new Rectangle(bounds.X + 3, y - 2, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 5, y - 2, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 7, y - 2, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 9, y - 2, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 3, y + 4, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 5, y + 4, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 7, y + 4, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);

                r = new Rectangle(bounds.X + 9, y + 4, 1, 1);
                g.FillRectangle(new SolidBrush(textColor), r);
            }

            base.OnDraw(g, bounds);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            //SliderHeight = Global.GetPercent(p_Value, p_Maximum, Bounds.Height - 1);

            base.OnBoundsChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Value = Global.GetPercent(e.Y - Bounds.Y, Bounds.Height - 1, p_Maximum);

            base.OnMouseDown(e);
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Value = Global.GetPercent(e.Y - Bounds.Y, Bounds.Height - 1, p_Maximum);

            base.OnMouseMove(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnMaximumChanged(EventArgs e)
        {
            if (MaximumChanged != null) MaximumChanged.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null) ValueChanged.Invoke(this, e);
        }

        protected virtual void OnSliderHeightChanged(EventArgs e)
        {
            if (SliderHeightChanged != null) SliderHeightChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region SeekBar
    public class SeekBar : Slider
    {
        #region Events
        public event EventHandler PeakLeftChanged;
        public event EventHandler PeakRightChanged;
        #endregion

        #region Objects
        private int p_PeakLeft, p_PeakRight;
        #endregion

        #region Properties
        public int PeakLeft
        {
            get { return p_PeakLeft; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                p_PeakLeft = value;
                OnPeakLeftChanged(new EventArgs());
                //ReDraw();
            }
        }

        public int PeakRight
        {
            get { return p_PeakRight; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                p_PeakRight = value;
                OnPeakRightChanged(new EventArgs());
                ReDraw();
            }
        }
        #endregion

        #region Constructor/Destructor
        public SeekBar(Window owner, Control parent) : base(owner, parent)
        {
            p_PeakLeft = 0;
            p_PeakRight = 0;
        }

        public override void Dispose()
        {
            p_PeakRight = 0;
            p_PeakLeft = 0;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Color bgColor = Global.Skin.Slider.BGColor;
            Color bgColor2 = Global.Skin.Slider.BGColor2;
            Color borderColor = Global.Skin.Slider.BorderColor;
            Rectangle r;

            if (IsMouseMoving)
            {
                bgColor = Global.Skin.Slider.Over_BGColor;
                bgColor2 = Global.Skin.Slider.Over_BGColor2;
                borderColor = Global.Skin.Slider.Over_BorderColor;
            }

            r = new Rectangle(bounds.X, bounds.Y, Global.GetPercent(Value, Maximum, bounds.Width - 1), bounds.Height - 1);
            if (r.Width > 3) Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, bgColor2, bgColor, LinearGradientMode.Horizontal));

            r = new Rectangle(bounds.X, bounds.Y, Global.GetPercent(p_PeakLeft, 100, bounds.Width - 1), ((bounds.Height - 1) / 2) + 1);
            if (r.Width > 3) g.FillRectangle(new LinearGradientBrush(bounds, Color.FromArgb(100, Global.Skin.Slider.Play_BGColor), Color.FromArgb(100, Global.Skin.Slider.Stop_BGColor), LinearGradientMode.Horizontal), r);

            r = new Rectangle(bounds.X, bounds.Y + ((bounds.Height - 1) / 2) + 1, Global.GetPercent(p_PeakRight, 100, bounds.Width - 1), (bounds.Height - 1) / 2);
            if (r.Width > 3) g.FillRectangle(new LinearGradientBrush(bounds, Color.FromArgb(100, Global.Skin.Slider.Play_BGColor), Color.FromArgb(100, Global.Skin.Slider.Stop_BGColor), LinearGradientMode.Horizontal), r);

            //r = new Rectangle(bounds.X + Global.GetPercent(p_HighPeakLeft, 100, bounds.Width - 1), bounds.Y, 2, 10);
            //g.FillRectangle(new SolidBrush(Global.Skin.Slider.BorderColor), r);

            //r = new Rectangle(bounds.X + Global.GetPercent(p_HighPeakRight, 100, bounds.Width - 1), bounds.Y + 10, 2, 10);
            //g.FillRectangle(new SolidBrush(Global.Skin.Slider.BorderColor), r);
            
            r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            Global.DrawRoundedRectangle(g, r, new Pen(borderColor));

            //base.OnDraw(g, bounds);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnPeakLeftChanged(EventArgs e)
        {
            if (PeakLeftChanged != null) PeakLeftChanged.Invoke(this, e);
        }

        protected virtual void OnPeakRightChanged(EventArgs e)
        {
            if (PeakRightChanged != null) PeakRightChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Skin
    public class Skin : GenericListItem
    {
        #region Objects
        private static Font p_IconFont = new Font("Webdings", 10.0f, FontStyle.Regular);
        private static Font p_PopupFont = new Font("Tahoma", 8.25f, FontStyle.Bold);
        private static Font p_WindowFont = new Font("Tahoma", 8.25f, FontStyle.Regular);
        
        private string p_URL;
        private string p_Name;
        private string p_Author;
        private int p_Created;

        private ColorGroup p_Default;
        private ColorGroup p_Button;
        private ColorGroup p_CheckBox;
        private ColorGroup p_List;
        private ColorGroup p_Popup;
        private ColorGroup p_ScrollBar;
        private ColorGroup p_Slider;
        private ColorGroup p_Tab;
        private ColorGroup p_TextBox;
        private ColorGroup p_ToolBar;
        private ColorGroup p_Window;
        #endregion

        #region Properties
        public static Font IconFont
        {
            get { return p_IconFont; }
        }

        public static Font PopupFont
        {
            get { return p_PopupFont; }
        }

        public static Font WindowFont
        {
            get { return p_WindowFont; }
        }
        
        public string URL
        {
            get { return p_URL; }
        }

        public string Name
        {
            get { return p_Name; }
        }

        public string Author
        {
            get { return p_Author; }
        }

        public string AuthorPersonaName
        {
            get
            {
                if (Global.IsUInt64(p_Author) && p_Author.Length == 17) return Global.Steam.Client.GetFriendPersonaName(new SteamAPI.SteamID(Global.StringToUInt64(p_Author)));
                return p_Author;
            }
        }

        public string Created
        {
            get { return Global.ConvertUnixDate(p_Created, "Never"); }
        }

        public ColorGroup Default
        {
            get { return p_Default; }
        }

        public ColorGroup Button
        {
            get { return p_Button; }
        }

        public ColorGroup CheckBox
        {
            get { return p_CheckBox; }
        }

        public ColorGroup List
        {
            get { return p_List; }
        }

        public ColorGroup Popup
        {
            get { return p_Popup; }
        }

        public ColorGroup ScrollBar
        {
            get { return p_ScrollBar; }
        }

        public ColorGroup Slider
        {
            get { return p_Slider; }
        }

        public ColorGroup Tab
        {
            get { return p_Tab; }
        }

        public ColorGroup TextBox
        {
            get { return p_TextBox; }
        }

        public ColorGroup ToolBar
        {
            get { return p_ToolBar; }
        }

        public ColorGroup Window
        {
            get { return p_Window; }
        }
        #endregion

        #region Constructors/Destructor
        public Skin() : base()
        {
            Steam();

            //SetDefaults();
        }

        public Skin(string url) : base()
        {
            p_URL = url;

            if (string.IsNullOrEmpty(p_URL) || !p_URL.StartsWith("http://") || !Global.MediaPlayer.IsOnline) Steam();
            else
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(p_URL);
                    XmlNode node = xml.SelectSingleNode("Steamp3.Skin");

                    GetXmlValues(node, false);
                }
                catch
                {
                    Steam();
                }
            }

            SetDefaults();
        }

        public Skin(XmlNode node) : base()
        {
            GetXmlValues(node, true);
        }

        public override void Dispose()
        {
            if (p_Window != null) p_Window.Dispose();
            if (p_ToolBar != null) p_ToolBar.Dispose();
            if (p_TextBox != null) p_TextBox.Dispose();
            if (p_Tab != null) p_Tab.Dispose();
            if (p_Slider != null) p_Slider.Dispose();
            if (p_ScrollBar != null) p_ScrollBar.Dispose();
            if (p_Popup != null) p_Popup.Dispose();
            if (p_List != null) p_List.Dispose();
            if (p_CheckBox != null) p_CheckBox.Dispose();
            if (p_Button != null) p_Button.Dispose();
            if (p_Default != null) p_Default.Dispose();

            p_Created = 0;
            p_Author = string.Empty;
            p_Name = string.Empty;
            p_URL = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_URL;
        }
        #endregion

        #region Public Methods
        public void Generate()
        {
            Array colorsArray = Enum.GetValues(typeof(KnownColor));
            KnownColor[] allColors = new KnownColor[colorsArray.Length];

            Array.Copy(colorsArray, allColors, colorsArray.Length);

            Generate(Color.FromKnownColor(allColors[Global.RandomNumber(allColors.Length - 1, true)]));
        }

        public void Generate(Color c)
        {
            p_URL = string.Empty;
            p_Name = "Generated Skin";
            p_Author = "N/A";
            p_Created = 0;
            p_Default = new ColorGroup(null);
            p_Button = new ColorGroup(null);
            p_CheckBox = new ColorGroup(null);
            p_List = new ColorGroup(null);
            p_Popup = new ColorGroup(null);
            p_ScrollBar = new ColorGroup(null);
            p_Slider = new ColorGroup(null);
            p_Tab = new ColorGroup(null);
            p_TextBox = new ColorGroup(null);
            p_ToolBar = new ColorGroup(null);
            p_Window = new ColorGroup(null);

            Color contrastColor = Global.GetContrastColor(c);

            int rLight = c.R + 50;
            int gLight = c.G + 50;
            int bLight = c.B + 50;
            if (rLight > 255) rLight = 255;
            if (gLight > 255) gLight = 255;
            if (bLight > 255) bLight = 255;
            Color lightColor = Color.FromArgb(rLight, gLight, bLight);

            int rLightLight = c.R + 100;
            int gLightLight = c.G + 100;
            int bLightLight = c.B + 100;
            if (rLightLight > 255) rLightLight = 255;
            if (gLightLight > 255) gLightLight = 255;
            if (bLightLight > 255) bLightLight = 255;
            Color lightLightColor = Color.FromArgb(rLightLight, gLightLight, bLightLight);

            int rDark = c.R - 50;
            int gDark = c.G - 50;
            int bDark = c.B - 50;
            if (rDark < 0) rDark = 0;
            if (gDark < 0) gDark = 0;
            if (bDark < 0) bDark = 0;
            Color darkColor = Color.FromArgb(rDark, gDark, bDark);

            int rDarkDark = c.R - 100;
            int gDarkDark = c.G - 100;
            int bDarkDark = c.B - 100;
            if (rDarkDark < 0) rDarkDark = 0;
            if (gDarkDark < 0) gDarkDark = 0;
            if (bDarkDark < 0) bDarkDark = 0;
            Color darkDarkColor = Color.FromArgb(rDarkDark, gDarkDark, bDarkDark);

            p_Default.BGColor = lightColor;
            p_Default.BGColor2 = c;
            p_Default.BorderColor = darkColor;
            p_Default.BorderColor2 = lightColor;
            p_Default.LineColor = c;
            p_Default.TextColor = contrastColor;
            p_Default.Down_BGColor = darkDarkColor;
            p_Default.Down_BGColor2 = darkColor;
            p_Default.Down_BorderColor = darkColor;
            p_Default.Down_TextColor = contrastColor;
            p_Default.Over_BGColor = lightLightColor;
            p_Default.Over_BGColor2 = lightColor;
            p_Default.Over_BorderColor = darkDarkColor;
            p_Default.Over_TextColor = contrastColor;
            p_Default.Play_BGColor = darkDarkColor;
            p_Default.Play_TextColor = contrastColor;
            p_Default.Stop_BGColor = darkDarkColor;
            p_Default.Stop_TextColor = contrastColor;

            SetDefaults();
        }

        public void New()
        {
            p_URL = string.Empty;
            p_Name = "New Skin";
            p_Author = "N/A";
            p_Created = 0;
            p_Default = new ColorGroup();
            p_Button = new ColorGroup();
            p_CheckBox = new ColorGroup();
            p_List = new ColorGroup();
            p_Popup = new ColorGroup();
            p_ScrollBar = new ColorGroup();
            p_Slider = new ColorGroup();
            p_Tab = new ColorGroup();
            p_TextBox = new ColorGroup();
            p_ToolBar = new ColorGroup();
            p_Window = new ColorGroup();
        }

        public void Steam()
        {
            p_URL = string.Empty;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.Steam);
            XmlNode node = xml.SelectSingleNode("Steamp3.Skin");

            GetXmlValues(node, false);
        }

        public void SaveAs(string name)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                string data = "method=1&name=" + HttpUtility.UrlEncode(name) + "&author=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&url=" + HttpUtility.UrlEncode(p_URL) + p_Button.ToUrl("button") + p_CheckBox.ToUrl("checkBox") + p_List.ToUrl("list") + p_Popup.ToUrl("popup") + p_ScrollBar.ToUrl("scrollBar") + p_Slider.ToUrl("slider") + p_Tab.ToUrl("tab") + p_TextBox.ToUrl("textBox") + p_ToolBar.ToUrl("toolBar") + p_Window.ToUrl("window");
                string result = wc.UploadString("http://steamp3.ta0soft.com/skins/skins.php", "POST", data);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNode node = xml.SelectSingleNode("Steamp3.Skin");

                p_Name = Global.GetXmlValue(node, "Name", "N/A");
                p_Author = Global.GetXmlValue(node, "Author", "N/A");
                p_Created = Global.StringToInt(Global.GetXmlValue(node, "Created", "0"));

                wc.Dispose();
            }
            catch { }
        }

        public void Overwrite()
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                string data = "method=2&name=" + HttpUtility.UrlEncode(p_Name) + "&author=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&url=" + HttpUtility.UrlEncode(p_URL) + p_Button.ToUrl("button") + p_CheckBox.ToUrl("checkBox") + p_List.ToUrl("list") + p_Popup.ToUrl("popup") + p_ScrollBar.ToUrl("scrollBar") + p_Slider.ToUrl("slider") + p_Tab.ToUrl("tab") + p_TextBox.ToUrl("textBox") + p_ToolBar.ToUrl("toolBar") + p_Window.ToUrl("window");
                string result = wc.UploadString("http://steamp3.ta0soft.com/skins/skins.php", "POST", data);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNode node = xml.SelectSingleNode("Steamp3.Skin");

                p_Name = Global.GetXmlValue(node, "Name", "N/A");
                p_Author = Global.GetXmlValue(node, "Author", "N/A");
                p_Created = Global.StringToInt(Global.GetXmlValue(node, "Created", "0"));

                wc.Dispose();
            }
            catch { }
        }

        public void UpdateStars()
        {
            if (Global.MainWindow != null)
            {
                Global.MainWindow.Playlist.ID3Rating.StarNone = Global.AdjustColor(Properties.Resources.StarNone, p_List.Over_TextColor);
                Global.MainWindow.Playlist.ID3Rating.StarHalf = Global.AdjustColor(Properties.Resources.StarHalf, p_List.Over_TextColor);
                Global.MainWindow.Playlist.ID3Rating.StarFull = Global.AdjustColor(Properties.Resources.StarFull, p_List.Over_TextColor);
            }
        }
        #endregion

        #region Private Methods
        private void GetXmlValues(XmlNode node, bool setURL)
        {
            if (setURL) p_URL = Global.GetXmlValue(node, "URL", string.Empty);
            p_Name = Global.GetXmlValue(node, "Name", string.Empty);
            p_Author = Global.GetXmlValue(node, "Author", string.Empty);
            p_Created = Global.StringToInt(Global.GetXmlValue(node, "Created", "0"));
            p_Default = new ColorGroup(node.SelectSingleNode("Default"));
            p_Button = new ColorGroup(node.SelectSingleNode("Button"));
            p_CheckBox = new ColorGroup(node.SelectSingleNode("CheckBox"));
            p_List = new ColorGroup(node.SelectSingleNode("List"));
            p_Popup = new ColorGroup(node.SelectSingleNode("Popup"));
            p_ScrollBar = new ColorGroup(node.SelectSingleNode("ScrollBar"));
            p_Slider = new ColorGroup(node.SelectSingleNode("Slider"));
            p_Tab = new ColorGroup(node.SelectSingleNode("Tab"));
            p_TextBox = new ColorGroup(node.SelectSingleNode("TextBox"));
            p_ToolBar = new ColorGroup(node.SelectSingleNode("ToolBar"));
            p_Window = new ColorGroup(node.SelectSingleNode("Window"));
        }

        private void SetDefaults()
        {
            if (p_Default.BGColor == Color.Transparent) p_Default.BGColor = Color.Black;
            if (p_Default.BGColor2 == Color.Transparent) p_Default.BGColor2 = p_Default.BGColor;
            if (p_Default.BorderColor == Color.Transparent) p_Default.BorderColor = Color.Black;
            if (p_Default.BorderColor2 == Color.Transparent) p_Default.BorderColor2 = p_Default.BorderColor;
            if (p_Default.LineColor == Color.Transparent) p_Default.LineColor = Color.Black;
            if (p_Default.TextColor == Color.Transparent) p_Default.TextColor = Color.Black;
            if (p_Default.Down_BGColor == Color.Transparent) p_Default.Down_BGColor = Color.Black;
            if (p_Default.Down_BGColor2 == Color.Transparent) p_Default.Down_BGColor2 = p_Default.Down_BGColor;
            if (p_Default.Down_BorderColor == Color.Transparent) p_Default.Down_BorderColor = Color.Black;
            if (p_Default.Down_TextColor == Color.Transparent) p_Default.Down_TextColor = Color.Black;
            if (p_Default.Over_BGColor == Color.Transparent) p_Default.Over_BGColor = Color.Black;
            if (p_Default.Over_BGColor2 == Color.Transparent) p_Default.Over_BGColor2 = p_Default.Over_BGColor;
            if (p_Default.Over_BorderColor == Color.Transparent) p_Default.Over_BorderColor = Color.Black;
            if (p_Default.Over_TextColor == Color.Transparent) p_Default.Over_TextColor = Color.Black;
            if (p_Default.Play_BGColor == Color.Transparent) p_Default.Play_BGColor = Color.Black;
            if (p_Default.Play_TextColor == Color.Transparent) p_Default.Play_TextColor = Color.Black;
            if (p_Default.Stop_BGColor == Color.Transparent) p_Default.Stop_BGColor = Color.Black;
            if (p_Default.Stop_TextColor == Color.Transparent) p_Default.Stop_TextColor = Color.Black;

            if (p_Button.BGColor == Color.Transparent) p_Button.BGColor = p_Default.BGColor;
            if (p_Button.BGColor2 == Color.Transparent) p_Button.BGColor2 = p_Default.BGColor2;
            if (p_Button.BorderColor == Color.Transparent) p_Button.BorderColor = p_Default.BorderColor;
            if (p_Button.TextColor == Color.Transparent) p_Button.TextColor = p_Default.TextColor;
            if (p_Button.Down_BGColor == Color.Transparent) p_Button.Down_BGColor = p_Default.Down_BGColor;
            if (p_Button.Down_BGColor2 == Color.Transparent) p_Button.Down_BGColor2 = p_Default.Down_BGColor2;
            if (p_Button.Down_BorderColor == Color.Transparent) p_Button.Down_BorderColor = p_Default.Down_BorderColor;
            if (p_Button.Down_TextColor == Color.Transparent) p_Button.Down_TextColor = p_Default.Down_TextColor;
            if (p_Button.Over_BGColor == Color.Transparent) p_Button.Over_BGColor = p_Default.Over_BGColor;
            if (p_Button.Over_BGColor2 == Color.Transparent) p_Button.Over_BGColor2 = p_Default.Over_BGColor2;
            if (p_Button.Over_BorderColor == Color.Transparent) p_Button.Over_BorderColor = p_Default.Over_BorderColor;
            if (p_Button.Over_TextColor == Color.Transparent) p_Button.Over_TextColor = p_Default.Over_TextColor;

            if (p_CheckBox.BorderColor == Color.Transparent) p_CheckBox.BorderColor = p_Default.BorderColor;
            if (p_CheckBox.TextColor == Color.Transparent) p_CheckBox.TextColor = p_Default.TextColor;
            if (p_CheckBox.Down_BorderColor == Color.Transparent) p_CheckBox.Down_BorderColor = p_Default.Down_BorderColor;
            if (p_CheckBox.Down_TextColor == Color.Transparent) p_CheckBox.Down_TextColor = p_Default.Down_TextColor;
            if (p_CheckBox.Over_BorderColor == Color.Transparent) p_CheckBox.Over_BorderColor = p_Default.Over_BorderColor;
            if (p_CheckBox.Over_TextColor == Color.Transparent) p_CheckBox.Over_TextColor = p_Default.Over_TextColor;

            if (p_List.BGColor == Color.Transparent) p_List.BGColor = p_Default.BGColor;
            if (p_List.BGColor2 == Color.Transparent) p_List.BGColor2 = p_Default.BGColor2;
            if (p_List.BorderColor == Color.Transparent) p_List.BorderColor = p_Default.BorderColor;
            if (p_List.TextColor == Color.Transparent) p_List.TextColor = p_Default.TextColor;
            if (p_List.Down_BGColor == Color.Transparent) p_List.Down_BGColor = p_Default.Down_BGColor;
            if (p_List.Down_TextColor == Color.Transparent) p_List.Down_TextColor = p_Default.Down_TextColor;
            if (p_List.Over_TextColor == Color.Transparent) p_List.Over_TextColor = p_Default.Over_TextColor;
            if (p_List.Play_BGColor == Color.Transparent) p_List.Play_BGColor = p_Default.Play_BGColor;
            if (p_List.Play_TextColor == Color.Transparent) p_List.Play_TextColor = p_Default.Play_TextColor;
            if (p_List.Stop_BGColor == Color.Transparent) p_List.Stop_BGColor = p_Default.Stop_BGColor;
            if (p_List.Stop_TextColor == Color.Transparent) p_List.Stop_TextColor = p_Default.Stop_TextColor;

            if (p_Popup.BGColor == Color.Transparent) p_Popup.BGColor = p_Default.BGColor;
            if (p_Popup.BGColor2 == Color.Transparent) p_Popup.BGColor2 = p_Default.BGColor2;
            if (p_Popup.BorderColor == Color.Transparent) p_Popup.BorderColor = p_Default.BorderColor;
            if (p_Popup.LineColor == Color.Transparent) p_Popup.LineColor = p_Default.LineColor;
            if (p_Popup.TextColor == Color.Transparent) p_Popup.TextColor = p_Default.TextColor;
            if (p_Popup.Over_BGColor == Color.Transparent) p_Popup.Over_BGColor = p_Default.Over_BGColor;
            if (p_Popup.Over_TextColor == Color.Transparent) p_Popup.Over_TextColor = p_Default.Over_TextColor;

            if (p_ScrollBar.BGColor == Color.Transparent) p_ScrollBar.BGColor = p_Default.BGColor;
            if (p_ScrollBar.BGColor2 == Color.Transparent) p_ScrollBar.BGColor2 = p_Default.BGColor2;
            if (p_ScrollBar.TextColor == Color.Transparent) p_ScrollBar.TextColor = p_Default.TextColor;
            if (p_ScrollBar.Down_BGColor == Color.Transparent) p_ScrollBar.Down_BGColor = p_Default.Down_BGColor;
            if (p_ScrollBar.Down_TextColor == Color.Transparent) p_ScrollBar.Down_TextColor = p_Default.Down_TextColor;
            if (p_ScrollBar.Over_BGColor == Color.Transparent) p_ScrollBar.Over_BGColor = p_Default.Over_BGColor;
            if (p_ScrollBar.Over_TextColor == Color.Transparent) p_ScrollBar.Over_TextColor = p_Default.Over_TextColor;

            if (p_Slider.BGColor == Color.Transparent) p_Slider.BGColor = p_Default.BGColor;
            if (p_Slider.BGColor2 == Color.Transparent) p_Slider.BGColor2 = p_Default.BGColor2;
            if (p_Slider.BorderColor == Color.Transparent) p_Slider.BorderColor = p_Default.BorderColor;
            if (p_Slider.Over_BGColor == Color.Transparent) p_Slider.Over_BGColor = p_Default.Over_BGColor;
            if (p_Slider.Over_BGColor2 == Color.Transparent) p_Slider.Over_BGColor2 = p_Default.Over_BGColor2;
            if (p_Slider.Over_BorderColor == Color.Transparent) p_Slider.Over_BorderColor = p_Default.Over_BorderColor;
            if (p_Slider.Play_BGColor == Color.Transparent) p_Slider.Play_BGColor = p_Default.Play_BGColor;
            if (p_Slider.Stop_BGColor == Color.Transparent) p_Slider.Stop_BGColor = p_Default.Stop_BGColor;

            if (p_Tab.BGColor == Color.Transparent) p_Tab.BGColor = p_Default.BGColor;
            if (p_Tab.BGColor2 == Color.Transparent) p_Tab.BGColor2 = p_Default.BGColor2;
            if (p_Tab.LineColor == Color.Transparent) p_Tab.LineColor = p_Default.LineColor;
            if (p_Tab.TextColor == Color.Transparent) p_Tab.TextColor = p_Default.TextColor;
            if (p_Tab.Down_BGColor == Color.Transparent) p_Tab.Down_BGColor = p_Default.Down_BGColor;
            if (p_Tab.Down_TextColor == Color.Transparent) p_Tab.Down_TextColor = p_Default.Down_TextColor;
            if (p_Tab.Over_BGColor == Color.Transparent) p_Tab.Over_BGColor = p_Default.Over_BGColor;
            if (p_Tab.Over_TextColor == Color.Transparent) p_Tab.Over_TextColor = p_Default.Over_TextColor;

            if (p_TextBox.BorderColor == Color.Transparent) p_TextBox.BorderColor = p_Default.BorderColor;
            if (p_TextBox.TextColor == Color.Transparent) p_TextBox.TextColor = p_Default.TextColor;
            if (p_TextBox.Down_BGColor == Color.Transparent) p_TextBox.Down_BGColor = p_Default.Down_BGColor;
            if (p_TextBox.Down_TextColor == Color.Transparent) p_TextBox.Down_TextColor = p_Default.Down_TextColor;
            if (p_TextBox.Over_TextColor == Color.Transparent) p_TextBox.Over_TextColor = p_Default.Over_TextColor;

            if (p_ToolBar.BGColor == Color.Transparent) p_ToolBar.BGColor = p_Default.BGColor;
            if (p_ToolBar.BGColor2 == Color.Transparent) p_ToolBar.BGColor2 = p_Default.BGColor2;
            if (p_ToolBar.BorderColor == Color.Transparent) p_ToolBar.BorderColor = p_Default.BorderColor;
            if (p_ToolBar.TextColor == Color.Transparent) p_ToolBar.TextColor = p_Default.TextColor;

            if (p_Window.BGColor == Color.Transparent) p_Window.BGColor = p_Default.BGColor;
            if (p_Window.BGColor2 == Color.Transparent) p_Window.BGColor2 = p_Default.BGColor2;
            if (p_Window.BorderColor == Color.Transparent) p_Window.BorderColor = p_Default.BorderColor;
            if (p_Window.BorderColor2 == Color.Transparent) p_Window.BorderColor2 = p_Default.BorderColor2;
            if (p_Window.LineColor == Color.Transparent) p_Window.LineColor = p_Default.LineColor;
            if (p_Window.TextColor == Color.Transparent) p_Window.TextColor = p_Default.TextColor;
            if (p_Window.Down_TextColor == Color.Transparent) p_Window.Down_TextColor = p_Default.Down_TextColor;
            if (p_Window.Over_TextColor == Color.Transparent) p_Window.Over_TextColor = p_Default.Over_TextColor;

            UpdateStars();
        }
        #endregion
    }
    #endregion

    #region SkinDesigner
    public class SkinDesigner : Dialog
    {
        #region Enums
        enum ColorTypes : int
        {
            None,

            All_BGColor,
            All_BGColor2,
            All_BorderColor,
            All_BorderColor2,
            All_LineColor,
            All_TextColor,
            All_Over_BGColor,
            All_Over_BGColor2,
            All_Over_BorderColor,
            All_Over_TextColor,
            All_Down_BGColor,
            All_Down_BGColor2,
            All_Down_BorderColor,
            All_Down_TextColor,
            All_Play_BGColor,
            All_Play_TextColor,
            All_Stop_BGColor,
            All_Stop_TextColor,

            Button_BGColor,
            Button_BGColor2,
            Button_BorderColor,
            Button_TextColor,
            Button_Over_BGColor,
            Button_Over_BGColor2,
            Button_Over_BorderColor,
            Button_Over_TextColor,
            Button_Down_BGColor,
            Button_Down_BGColor2,
            Button_Down_BorderColor,
            Button_Down_TextColor,

            CheckBox_BorderColor,
            CheckBox_TextColor,
            CheckBox_Over_BorderColor,
            CheckBox_Over_TextColor,
            CheckBox_Down_BorderColor,
            CheckBox_Down_TextColor,

            List_BGColor,
            List_BGColor2,
            List_BorderColor,
            List_TextColor,
            List_Over_TextColor,
            List_Down_BGColor,
            List_Down_TextColor,
            List_Play_BGColor,
            List_Play_TextColor,
            List_Stop_BGColor,
            List_Stop_TextColor,

            Popup_BGColor,
            Popup_BGColor2,
            Popup_BorderColor,
            Popup_LineColor,
            Popup_TextColor,
            Popup_Over_BGColor,
            Popup_Over_TextColor,

            ScrollBar_BGColor,
            ScrollBar_BGColor2,
            ScrollBar_TextColor,
            ScrollBar_Over_BGColor,
            ScrollBar_Over_TextColor,
            ScrollBar_Down_BGColor,
            ScrollBar_Down_TextColor,

            Slider_BGColor,
            Slider_BGColor2,
            Slider_BorderColor,
            Slider_Over_BGColor,
            Slider_Over_BGColor2,
            Slider_Over_BorderColor,
            Slider_Play_BGColor,
            Slider_Stop_BGColor,

            Tab_BGColor,
            Tab_BGColor2,
            Tab_LineColor,
            Tab_TextColor,
            Tab_Over_BGColor,
            Tab_Over_TextColor,
            Tab_Down_BGColor,
            Tab_Down_TextColor,

            TextBox_BorderColor,
            TextBox_TextColor,
            TextBox_Over_TextColor,
            TextBox_Down_BGColor,
            TextBox_Down_TextColor,

            ToolBar_BGColor,
            ToolBar_BGColor2,
            ToolBar_BorderColor,
            ToolBar_TextColor,

            Window_BGColor,
            Window_BGColor2,
            Window_BorderColor,
            Window_BorderColor2,
            Window_LineColor,
            Window_TextColor,
            Window_Over_TextColor,
            Window_Down_TextColor,
        }
        #endregion

        #region Objects
        private DropDownGroup p_CategoryGroup, p_EventGroup;
        private ColorDropDownGroup p_ColorGroup1, p_ColorGroup2, p_ColorGroup3, p_ColorGroup4, p_ColorGroup5, p_ColorGroup6;
        private Button p_SaveButton, p_SaveAsButton, p_CancelButton;
        #endregion

        #region Constructor/Destructor
        public SkinDesigner() : base()
        {
            Sizable = false;
            Size = new Size(406, 204);
            Text = "STEAMp3 - Skin Designer";

            int x = (Width - 36) / 3;

            p_CategoryGroup = new DropDownGroup(this, null, "Category:", 0, string.Empty, new string[] { "All Colors (18)", "Button Colors (12)", "CheckBox Colors (6)", "List Colors (11)", "Popup Colors (7)", "ScrollBar Colors (7)", "Slider Colors (8)", "Tab Colors (8)", "TextBox Colors (5)", "ToolBar Colors (4)", "Window Colors (8)" });
            p_CategoryGroup.SelectedIndexChanged += new EventHandler(p_CategoryGroup_SelectedIndexChanged);
            p_CategoryGroup.SetBounds(12, 30, Width - 24, 20);

            p_EventGroup = new DropDownGroup(this, null, "Event:", 0, string.Empty, new string[] { "Normal/MouseUp", "Hot/MouseOver", "Selected/MouseDown", "Current/Playing", "Error/Stopped" });
            p_EventGroup.SelectedIndexChanged += new EventHandler(p_EventGroup_SelectedIndexChanged);
            p_EventGroup.SetBounds(12, 56, Width - 24, 20);

            p_ColorGroup1 = new ColorDropDownGroup(this, null, "1:", Color.Black);
            p_ColorGroup1.ColorChanged += new EventHandler(p_ColorGroup1_ColorChanged);
            p_ColorGroup1.SetBounds(12, 88, 214, 20);

            p_ColorGroup2 = new ColorDropDownGroup(this, null, "2:", Color.Black);
            p_ColorGroup2.ColorChanged += new EventHandler(p_ColorGroup2_ColorChanged);
            p_ColorGroup2.SetBounds(206, 88, 214, 20);

            p_ColorGroup3 = new ColorDropDownGroup(this, null, "3:", Color.Black);
            p_ColorGroup3.ColorChanged += new EventHandler(p_ColorGroup3_ColorChanged);
            p_ColorGroup3.SetBounds(12, 114, 214, 20);

            p_ColorGroup4 = new ColorDropDownGroup(this, null, "4:", Color.Black);
            p_ColorGroup4.ColorChanged += new EventHandler(p_ColorGroup4_ColorChanged);
            p_ColorGroup4.SetBounds(206, 114, 214, 20);

            p_ColorGroup5 = new ColorDropDownGroup(this, null, "5:", Color.Black);
            p_ColorGroup5.ColorChanged += new EventHandler(p_ColorGroup5_ColorChanged);
            p_ColorGroup5.SetBounds(12, 140, 214, 20);

            p_ColorGroup6 = new ColorDropDownGroup(this, null, "6:", Color.Black);
            p_ColorGroup6.ColorChanged += new EventHandler(p_ColorGroup6_ColorChanged);
            p_ColorGroup6.SetBounds(206, 140, 214, 20);

            p_SaveButton = new Button(this, null, "Save Skin");
            p_SaveButton.Enabled = Global.Skin.Author == Global.Steam.Client.GetSteamID().ToUInt64().ToString();
            p_SaveButton.MouseClick += new MouseEventHandler(p_SaveButton_MouseClick);
            p_SaveButton.SetBounds(12, 172, x, 20); 

            p_SaveAsButton = new Button(this, null, "Save Skin As...");
            p_SaveAsButton.MouseClick += new MouseEventHandler(p_SaveAsButton_MouseClick);
            p_SaveAsButton.SetBounds(x + 18, 172, x, 20);

            p_CancelButton = new Button(this, null, "Cancel");
            p_CancelButton.MouseClick += new MouseEventHandler(p_CancelButton_MouseClick);
            p_CancelButton.SetBounds((x * 2) + 24, 172, x, 20);
        }

        ~SkinDesigner()
        {

        }
        #endregion

        #region Overrides
        protected override void OnShown(EventArgs e)
        {
            UpdateGroups();

            base.OnShown(e);
        }
        #endregion

        #region Child Events
        private void p_CategoryGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGroups();
        }

        private void p_EventGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGroups();
        }

        private void p_ColorGroup1_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup1);
        }

        private void p_ColorGroup2_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup2);
        }

        private void p_ColorGroup3_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup3);
        }

        private void p_ColorGroup4_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup4);
        }

        private void p_ColorGroup5_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup5);
        }

        private void p_ColorGroup6_ColorChanged(object sender, EventArgs e)
        {
            UpdateColor(p_ColorGroup6);
        }

        private void p_SaveButton_MouseClick(object sender, MouseEventArgs e)
        {
            InfoDialog id = new InfoDialog("Are you sure you want to overwrite this skin?" + Environment.NewLine + Environment.NewLine + Global.Skin.Name, InfoDialog.InfoButtons.YesNo);
            if (id.ShowDialog(this) == DialogResult.Yes)
            {
                Global.Skin.Overwrite();
                DialogResult = DialogResult.OK;
            }

            id.Dispose();
        }

        private void p_SaveAsButton_MouseClick(object sender, MouseEventArgs e)
        {
            InputDialog id = new InputDialog("Enter a name for the skin:", Global.Skin.Name);
            if (id.ShowDialog(this) == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(id.TextBox.Text) || id.TextBox.Text.Length < 2 || id.TextBox.Text.Length > 20)
                {
                    InfoDialog id2 = new InfoDialog("Invalid skin name, please try again.");
                    id2.ShowDialog(this);
                    id2.Dispose();

                    goto dispose;
                }

                foreach (Skin skin in Global.MainWindow.SkinList.Items)
                {
                    if (skin.Name.ToLower() == id.TextBox.Text.ToLower())
                    {
                        InfoDialog id2 = new InfoDialog("Invalid skin name, please try again.");
                        id2.ShowDialog(this);
                        id2.Dispose();

                        goto dispose;
                    }
                }

                Global.Skin.SaveAs(id.TextBox.Text);
                DialogResult = DialogResult.OK;
            }

            dispose:
            id.Dispose();
        }

        private void p_CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Private Methods
        private void EmptyGroup(ColorDropDownGroup group)
        {
            UpdateGroup(group, Color.Black, string.Empty, ColorTypes.None);
        }

        private void UpdateGroup(ColorDropDownGroup group, Color color, string text, ColorTypes type)
        {
            group.Visible = !string.IsNullOrEmpty(text);
            group.DropDown.SetColor(color);
            group.Label.Text = text;
            group.Label.Tag = type;
        }

        private void UpdateGroups()
        {
            switch (p_CategoryGroup.DropDown.SelectedIndex)
            {
                case 0: // All
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Window.BGColor, "Background color:", ColorTypes.All_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Window.BGColor2, "Background gradient:", ColorTypes.All_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Window.BorderColor, "Border color:", ColorTypes.All_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Window.BorderColor2, "Border gradient:", ColorTypes.All_BorderColor2);
                            UpdateGroup(p_ColorGroup5, Global.Skin.Window.LineColor, "Line color:", ColorTypes.All_LineColor);
                            UpdateGroup(p_ColorGroup6, Global.Skin.Window.TextColor, "Text color:", ColorTypes.All_TextColor);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Button.Over_BGColor, "Background color:", ColorTypes.All_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Button.Over_BGColor2, "Background gradient:", ColorTypes.All_Over_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Button.Over_BorderColor, "Border color:", ColorTypes.All_Over_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Button.Over_TextColor, "Text color:", ColorTypes.All_Over_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Button.Down_BGColor, "Background color:", ColorTypes.All_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Button.Down_BGColor2, "Background gradient:", ColorTypes.All_Down_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Button.Down_BorderColor, "Border color:", ColorTypes.All_Down_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Button.Down_TextColor, "Text color:", ColorTypes.All_Down_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Play_BGColor, "Background color:", ColorTypes.All_Play_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.Play_TextColor, "Text color:", ColorTypes.All_Play_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Stop_BGColor, "Background color:", ColorTypes.All_Stop_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.Stop_TextColor, "Text color:", ColorTypes.All_Stop_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 1: // Button
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Button.BGColor, "Background color:", ColorTypes.Button_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Button.BGColor2, "Background gradient:", ColorTypes.Button_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Button.BorderColor, "Border color:", ColorTypes.Button_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Button.TextColor, "Text color:", ColorTypes.Button_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Button.Over_BGColor, "Background color:", ColorTypes.Button_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Button.Over_BGColor2, "Background gradient:", ColorTypes.Button_Over_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Button.Over_BorderColor, "Border color:", ColorTypes.Button_Over_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Button.Over_TextColor, "Text color:", ColorTypes.Button_Over_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Button.Down_BGColor, "Background color:", ColorTypes.Button_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Button.Down_BGColor2, "Background gradient:", ColorTypes.Button_Down_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Button.Down_BorderColor, "Border color:", ColorTypes.Button_Down_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Button.Down_TextColor, "Text color:", ColorTypes.Button_Down_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 2: // CheckBox
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.CheckBox.BorderColor, "Border color:", ColorTypes.CheckBox_BorderColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.CheckBox.TextColor, "Text color:", ColorTypes.CheckBox_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.CheckBox.Over_BorderColor, "Border color:", ColorTypes.CheckBox_Over_BorderColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.CheckBox.Over_TextColor, "Text color:", ColorTypes.CheckBox_Over_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.CheckBox.Down_BorderColor, "Border color:", ColorTypes.CheckBox_Down_BorderColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.CheckBox.Down_TextColor, "Text color:", ColorTypes.CheckBox_Down_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 3: // List
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.BGColor, "Background color:", ColorTypes.List_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.BGColor2, "Background gradient:", ColorTypes.List_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.List.BorderColor, "Border color:", ColorTypes.List_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.List.TextColor, "Text color:", ColorTypes.List_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Over_TextColor, "Text color:", ColorTypes.List_Over_TextColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Down_BGColor, "Background color:", ColorTypes.List_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.Down_TextColor, "Text color:", ColorTypes.List_Down_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Play_BGColor, "Background color:", ColorTypes.List_Play_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.Play_TextColor, "Text color:", ColorTypes.List_Play_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            UpdateGroup(p_ColorGroup1, Global.Skin.List.Stop_BGColor, "Background color:", ColorTypes.List_Stop_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.List.Stop_TextColor, "Text color:", ColorTypes.List_Stop_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 4: // Popup
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Popup.BGColor, "Background color:", ColorTypes.Popup_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Popup.BGColor2, "Background gradient:", ColorTypes.Popup_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Popup.BorderColor, "Border color:", ColorTypes.Popup_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Popup.LineColor, "Line color:", ColorTypes.Popup_LineColor);
                            UpdateGroup(p_ColorGroup5, Global.Skin.Popup.TextColor, "Text color:", ColorTypes.Popup_TextColor);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Popup.Over_BGColor, "Background color:", ColorTypes.Popup_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Popup.Over_TextColor, "Text color:", ColorTypes.Popup_Over_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 5: // ScrollBar
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.ScrollBar.BGColor, "Background color:", ColorTypes.ScrollBar_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.ScrollBar.BGColor2, "Background gradient:", ColorTypes.ScrollBar_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.ScrollBar.TextColor, "Text color:", ColorTypes.ScrollBar_TextColor);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.ScrollBar.Over_BGColor, "Background color:", ColorTypes.ScrollBar_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.ScrollBar.Over_TextColor, "Text color:", ColorTypes.ScrollBar_Over_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.ScrollBar.Down_BGColor, "Background color:", ColorTypes.ScrollBar_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.ScrollBar.Down_TextColor, "Text color:", ColorTypes.ScrollBar_Down_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 6: // Slider
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Slider.BGColor, "Background color:", ColorTypes.Slider_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Slider.BGColor2, "Background gradient:", ColorTypes.Slider_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Slider.BorderColor, "Border color:", ColorTypes.Slider_BorderColor);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Slider.Over_BGColor, "Background color:", ColorTypes.Slider_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Slider.Over_BGColor2, "Background gradient:", ColorTypes.Slider_Over_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Slider.Over_BorderColor, "Border color:", ColorTypes.Slider_Over_BorderColor);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Slider.Play_BGColor, "Background color:", ColorTypes.Slider_Play_BGColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Slider.Stop_BGColor, "Background color:", ColorTypes.Slider_Stop_BGColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 7: // Tab
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Tab.BGColor, "Background color:", ColorTypes.Tab_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Tab.BGColor2, "Background gradient:", ColorTypes.Tab_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Tab.LineColor, "Line color:", ColorTypes.Tab_LineColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Tab.TextColor, "Text color:", ColorTypes.Tab_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Tab.Over_BGColor, "Background color:", ColorTypes.Tab_Over_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Tab.Over_TextColor, "Text color:", ColorTypes.Tab_Over_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Tab.Down_BGColor, "Background color:", ColorTypes.Tab_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Tab.Down_TextColor, "Text color:", ColorTypes.Tab_Down_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 8: // TextBox
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.TextBox.BorderColor, "Border color:", ColorTypes.TextBox_BorderColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.TextBox.TextColor, "Text color:", ColorTypes.TextBox_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.TextBox.Over_TextColor, "Text color:", ColorTypes.TextBox_Over_TextColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.TextBox.Down_BGColor, "Background color:", ColorTypes.TextBox_Down_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.TextBox.Down_TextColor, "Text color:", ColorTypes.TextBox_Down_TextColor);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 9:  // ToolBar
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.ToolBar.BGColor, "Background color:", ColorTypes.ToolBar_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.ToolBar.BGColor2, "Background gradient:", ColorTypes.ToolBar_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.ToolBar.BorderColor, "Border color:", ColorTypes.ToolBar_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.ToolBar.TextColor, "Text color:", ColorTypes.ToolBar_TextColor);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 1:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
                case 10: // Window
                    switch (p_EventGroup.DropDown.SelectedIndex)
                    {
                        case 0:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Window.BGColor, "Background color:", ColorTypes.Window_BGColor);
                            UpdateGroup(p_ColorGroup2, Global.Skin.Window.BGColor2, "Background gradient:", ColorTypes.Window_BGColor2);
                            UpdateGroup(p_ColorGroup3, Global.Skin.Window.BorderColor, "Border color:", ColorTypes.Window_BorderColor);
                            UpdateGroup(p_ColorGroup4, Global.Skin.Window.BorderColor2, "Border gradient:", ColorTypes.Window_BorderColor2);
                            UpdateGroup(p_ColorGroup5, Global.Skin.Window.LineColor, "Line color:", ColorTypes.Window_LineColor);
                            UpdateGroup(p_ColorGroup6, Global.Skin.Window.TextColor, "Text color:", ColorTypes.Window_TextColor);
                            break;
                        case 1:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Window.Over_TextColor, "Text color:", ColorTypes.Window_Over_TextColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 2:
                            UpdateGroup(p_ColorGroup1, Global.Skin.Window.Down_TextColor, "Text color:", ColorTypes.Window_Down_TextColor);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 3:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                        case 4:
                            EmptyGroup(p_ColorGroup1);
                            EmptyGroup(p_ColorGroup2);
                            EmptyGroup(p_ColorGroup3);
                            EmptyGroup(p_ColorGroup4);
                            EmptyGroup(p_ColorGroup5);
                            EmptyGroup(p_ColorGroup6);
                            break;
                    }
                    break;
            }
        }

        private void UpdateColor(ColorDropDownGroup group)
        {
            Color c = group.DropDown.Color;

            switch ((ColorTypes)group.Label.Tag)
            {
                case ColorTypes.All_BGColor:
                    Global.Skin.Button.BGColor = c;
                    Global.Skin.List.BGColor = c;
                    Global.Skin.Popup.BGColor = c;
                    Global.Skin.ScrollBar.BGColor = c;
                    Global.Skin.Slider.BGColor = c;
                    Global.Skin.Tab.BGColor = c;
                    Global.Skin.ToolBar.BGColor = c;
                    Global.Skin.Window.BGColor = c;
                    break;
                case ColorTypes.All_BGColor2:
                    Global.Skin.Button.BGColor2 = c;
                    Global.Skin.List.BGColor2 = c;
                    Global.Skin.Popup.BGColor2 = c;
                    Global.Skin.ScrollBar.BGColor2 = c;
                    Global.Skin.Slider.BGColor2 = c;
                    Global.Skin.Tab.BGColor2 = c;
                    Global.Skin.ToolBar.BGColor2 = c;
                    Global.Skin.Window.BGColor2 = c;
                    break;
                case ColorTypes.All_BorderColor:
                    Global.Skin.Button.BorderColor = c;
                    Global.Skin.CheckBox.BorderColor = c;
                    Global.Skin.List.BorderColor = c;
                    Global.Skin.Popup.BorderColor = c;
                    Global.Skin.Slider.BorderColor = c;
                    Global.Skin.TextBox.BorderColor = c;
                    Global.Skin.ToolBar.BorderColor = c;
                    Global.Skin.Window.BorderColor = c;
                    break;
                case ColorTypes.All_BorderColor2:
                    Global.Skin.Window.BorderColor2 = c;
                    break;
                case ColorTypes.All_LineColor:
                    Global.Skin.Popup.LineColor = c;
                    Global.Skin.Tab.LineColor = c;
                    Global.Skin.Window.LineColor = c;
                    break;
                case ColorTypes.All_TextColor:
                    Global.Skin.Button.TextColor = c;
                    Global.Skin.CheckBox.TextColor = c;
                    Global.Skin.List.TextColor = c;
                    Global.Skin.Popup.TextColor = c;
                    Global.Skin.ScrollBar.TextColor = c;
                    Global.Skin.Tab.TextColor = c;
                    Global.Skin.TextBox.TextColor = c;
                    Global.Skin.ToolBar.TextColor = c;
                    Global.Skin.Window.TextColor = c;
                    break;
                case ColorTypes.All_Over_BGColor:
                    Global.Skin.Button.Over_BGColor = c;
                    Global.Skin.Popup.Over_BGColor = c;
                    Global.Skin.ScrollBar.Over_BGColor = c;
                    Global.Skin.Slider.Over_BGColor = c;
                    Global.Skin.Tab.Over_BGColor = c;
                    break;
                case ColorTypes.All_Over_BGColor2:
                    Global.Skin.Button.Over_BGColor2 = c;
                    Global.Skin.Slider.Over_BGColor2 = c;
                    break;
                case ColorTypes.All_Over_BorderColor:
                    Global.Skin.Button.Over_BorderColor = c;
                    Global.Skin.CheckBox.Over_BorderColor = c;
                    Global.Skin.Slider.Over_BorderColor = c;
                    break;
                case ColorTypes.All_Over_TextColor:
                    Global.Skin.Button.Over_TextColor = c;
                    Global.Skin.CheckBox.Over_TextColor = c;
                    Global.Skin.List.Over_TextColor = c;
                    Global.Skin.Popup.Over_TextColor = c;
                    Global.Skin.ScrollBar.Over_TextColor = c;
                    Global.Skin.Tab.Over_TextColor = c;
                    Global.Skin.TextBox.Over_TextColor = c;
                    Global.Skin.Window.Over_TextColor = c;

                    Global.Skin.UpdateStars(); //?
                    break;
                case ColorTypes.All_Down_BGColor:
                    Global.Skin.Button.Down_BGColor = c;
                    Global.Skin.List.Down_BGColor = c;
                    Global.Skin.ScrollBar.Down_BGColor = c;
                    Global.Skin.Tab.Down_BGColor = c;
                    Global.Skin.TextBox.Down_BGColor = c;
                    break;
                case ColorTypes.All_Down_BGColor2:
                    Global.Skin.Button.Down_BGColor2 = c;
                    break;
                case ColorTypes.All_Down_BorderColor:
                    Global.Skin.Button.Down_BorderColor = c;
                    Global.Skin.CheckBox.Down_BorderColor = c;
                    break;
                case ColorTypes.All_Down_TextColor:
                    Global.Skin.Button.Down_TextColor = c;
                    Global.Skin.CheckBox.Down_TextColor = c;
                    Global.Skin.List.Down_TextColor = c;
                    Global.Skin.ScrollBar.Down_TextColor = c;
                    Global.Skin.Tab.Down_TextColor = c;
                    Global.Skin.TextBox.Down_TextColor = c;
                    Global.Skin.Window.Down_TextColor = c;
                    break;
                case ColorTypes.All_Play_BGColor:
                    Global.Skin.List.Play_BGColor = c;
                    Global.Skin.Slider.Play_BGColor = c;
                    break;
                case ColorTypes.All_Play_TextColor:
                    Global.Skin.List.Play_TextColor = c;
                    break;
                case ColorTypes.All_Stop_BGColor:
                    Global.Skin.List.Stop_BGColor = c;
                    Global.Skin.Slider.Stop_BGColor = c;
                    break;
                case ColorTypes.All_Stop_TextColor:
                    Global.Skin.List.Stop_TextColor = c;
                    break;

                case ColorTypes.Button_BGColor:
                    Global.Skin.Button.BGColor = c;
                    break;
                case ColorTypes.Button_BGColor2:
                    Global.Skin.Button.BGColor2 = c;
                    break;
                case ColorTypes.Button_BorderColor:
                    Global.Skin.Button.BorderColor = c;
                    break;
                case ColorTypes.Button_TextColor:
                    Global.Skin.Button.TextColor = c;
                    break;
                case ColorTypes.Button_Over_BGColor:
                    Global.Skin.Button.Over_BGColor = c;
                    break;
                case ColorTypes.Button_Over_BGColor2:
                    Global.Skin.Button.Over_BGColor2 = c;
                    break;
                case ColorTypes.Button_Over_BorderColor:
                    Global.Skin.Button.Over_BorderColor = c;
                    break;
                case ColorTypes.Button_Over_TextColor:
                    Global.Skin.Button.Over_TextColor = c;
                    break;
                case ColorTypes.Button_Down_BGColor:
                    Global.Skin.Button.Down_BGColor = c;
                    break;
                case ColorTypes.Button_Down_BGColor2:
                    Global.Skin.Button.Down_BGColor2 = c;
                    break;
                case ColorTypes.Button_Down_BorderColor:
                    Global.Skin.Button.Down_BorderColor = c;
                    break;
                case ColorTypes.Button_Down_TextColor:
                    Global.Skin.Button.Down_TextColor = c;
                    break;

                case ColorTypes.CheckBox_BorderColor:
                    Global.Skin.CheckBox.BorderColor = c;
                    break;
                case ColorTypes.CheckBox_TextColor:
                    Global.Skin.CheckBox.TextColor = c;
                    break;
                case ColorTypes.CheckBox_Over_BorderColor:
                    Global.Skin.CheckBox.Over_BorderColor = c;
                    break;
                case ColorTypes.CheckBox_Over_TextColor:
                    Global.Skin.CheckBox.Over_TextColor = c;
                    break;
                case ColorTypes.CheckBox_Down_BorderColor:
                    Global.Skin.CheckBox.Down_BorderColor = c;
                    break;
                case ColorTypes.CheckBox_Down_TextColor:
                    Global.Skin.CheckBox.Down_TextColor = c;
                    break;

                case ColorTypes.List_BGColor:
                    Global.Skin.List.BGColor = c;
                    break;
                case ColorTypes.List_BGColor2:
                    Global.Skin.List.BGColor2 = c;
                    break;
                case ColorTypes.List_BorderColor:
                    Global.Skin.List.BorderColor = c;
                    break;
                case ColorTypes.List_TextColor:
                    Global.Skin.List.TextColor = c;
                    break;
                case ColorTypes.List_Over_TextColor:
                    Global.Skin.List.Over_TextColor = c;

                    Global.Skin.UpdateStars(); //?
                    break;
                case ColorTypes.List_Down_BGColor:
                    Global.Skin.List.Down_BGColor = c;
                    break;
                case ColorTypes.List_Down_TextColor:
                    Global.Skin.List.Down_TextColor = c;
                    break;
                case ColorTypes.List_Play_BGColor:
                    Global.Skin.List.Play_BGColor = c;
                    break;
                case ColorTypes.List_Play_TextColor:
                    Global.Skin.List.Play_TextColor = c;
                    break;
                case ColorTypes.List_Stop_BGColor:
                    Global.Skin.List.Stop_BGColor = c;
                    break;
                case ColorTypes.List_Stop_TextColor:
                    Global.Skin.List.Stop_TextColor = c;
                    break;

                case ColorTypes.Popup_BGColor:
                    Global.Skin.Popup.BGColor = c;
                    break;
                case ColorTypes.Popup_BGColor2:
                    Global.Skin.Popup.BGColor2 = c;
                    break;
                case ColorTypes.Popup_BorderColor:
                    Global.Skin.Popup.BorderColor = c;
                    break;
                case ColorTypes.Popup_LineColor:
                    Global.Skin.Popup.LineColor = c;
                    break;
                case ColorTypes.Popup_TextColor:
                    Global.Skin.Popup.TextColor = c;
                    break;
                case ColorTypes.Popup_Over_BGColor:
                    Global.Skin.Popup.Over_BGColor = c;
                    break;
                case ColorTypes.Popup_Over_TextColor:
                    Global.Skin.Popup.Over_TextColor = c;
                    break;

                case ColorTypes.ScrollBar_BGColor:
                    Global.Skin.ScrollBar.BGColor = c;
                    break;
                case ColorTypes.ScrollBar_BGColor2:
                    Global.Skin.ScrollBar.BGColor2 = c;
                    break;
                case ColorTypes.ScrollBar_TextColor:
                    Global.Skin.ScrollBar.TextColor = c;
                    break;
                case ColorTypes.ScrollBar_Over_BGColor:
                    Global.Skin.ScrollBar.Over_BGColor = c;
                    break;
                case ColorTypes.ScrollBar_Over_TextColor:
                    Global.Skin.ScrollBar.Over_TextColor = c;
                    break;
                case ColorTypes.ScrollBar_Down_BGColor:
                    Global.Skin.ScrollBar.Down_BGColor = c;
                    break;
                case ColorTypes.ScrollBar_Down_TextColor:
                    Global.Skin.ScrollBar.Down_TextColor = c;
                    break;

                case ColorTypes.Slider_BGColor:
                    Global.Skin.Slider.BGColor = c;
                    break;
                case ColorTypes.Slider_BGColor2:
                    Global.Skin.Slider.BGColor2 = c;
                    break;
                case ColorTypes.Slider_BorderColor:
                    Global.Skin.Slider.BorderColor = c;
                    break;
                case ColorTypes.Slider_Over_BGColor:
                    Global.Skin.Slider.Over_BGColor = c;
                    break;
                case ColorTypes.Slider_Over_BGColor2:
                    Global.Skin.Slider.Over_BGColor2 = c;
                    break;
                case ColorTypes.Slider_Over_BorderColor:
                    Global.Skin.Slider.Over_BorderColor = c;
                    break;
                case ColorTypes.Slider_Play_BGColor:
                    Global.Skin.Slider.Play_BGColor = c;
                    break;
                case ColorTypes.Slider_Stop_BGColor:
                    Global.Skin.Slider.Stop_BGColor = c;
                    break;

                case ColorTypes.Tab_BGColor:
                    Global.Skin.Tab.BGColor = c;
                    break;
                case ColorTypes.Tab_BGColor2:
                    Global.Skin.Tab.BGColor2 = c;
                    break;
                case ColorTypes.Tab_LineColor:
                    Global.Skin.Tab.LineColor = c;
                    break;
                case ColorTypes.Tab_TextColor:
                    Global.Skin.Tab.TextColor = c;
                    break;
                case ColorTypes.Tab_Over_BGColor:
                    Global.Skin.Tab.Over_BGColor = c;
                    break;
                case ColorTypes.Tab_Over_TextColor:
                    Global.Skin.Tab.Over_TextColor = c;
                    break;
                case ColorTypes.Tab_Down_BGColor:
                    Global.Skin.Tab.Down_BGColor = c;
                    break;
                case ColorTypes.Tab_Down_TextColor:
                    Global.Skin.Tab.Down_TextColor = c;
                    break;

                case ColorTypes.TextBox_BorderColor:
                    Global.Skin.TextBox.BorderColor = c;
                    break;
                case ColorTypes.TextBox_TextColor:
                    Global.Skin.TextBox.TextColor = c;
                    break;
                case ColorTypes.TextBox_Over_TextColor:
                    Global.Skin.TextBox.Over_TextColor = c;
                    break;
                case ColorTypes.TextBox_Down_BGColor:
                    Global.Skin.TextBox.Down_BGColor = c;
                    break;
                case ColorTypes.TextBox_Down_TextColor:
                    Global.Skin.TextBox.Down_TextColor = c;
                    break;

                case ColorTypes.ToolBar_BGColor:
                    Global.Skin.ToolBar.BGColor = c;
                    break;
                case ColorTypes.ToolBar_BGColor2:
                    Global.Skin.ToolBar.BGColor2 = c;
                    break;
                case ColorTypes.ToolBar_BorderColor:
                    Global.Skin.ToolBar.BorderColor = c;
                    break;
                case ColorTypes.ToolBar_TextColor:
                    Global.Skin.ToolBar.TextColor = c;
                    break;

                case ColorTypes.Window_BGColor:
                    Global.Skin.Window.BGColor = c;
                    break;
                case ColorTypes.Window_BGColor2:
                    Global.Skin.Window.BGColor2 = c;
                    break;
                case ColorTypes.Window_BorderColor:
                    Global.Skin.Window.BorderColor = c;
                    break;
                case ColorTypes.Window_BorderColor2:
                    Global.Skin.Window.BorderColor2 = c;
                    break;
                case ColorTypes.Window_LineColor:
                    Global.Skin.Window.LineColor = c;
                    break;
                case ColorTypes.Window_TextColor:
                    Global.Skin.Window.TextColor = c;
                    break;
                case ColorTypes.Window_Over_TextColor:
                    Global.Skin.Window.Over_TextColor = c;
                    break;
                case ColorTypes.Window_Down_TextColor:
                    Global.Skin.Window.Down_TextColor = c;
                    break;

            }

            Global.MainWindow.ReDraw();
        }
        #endregion
    }
    #endregion

    #region SkinList
    public class SkinList : GenericList<Skin>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllSkins,
            Ta0softSkins,
            ValveSkins,
            UserSkins,
            MySkins,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public SkinList(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new UI.FilterBar(owner, parent, "Search Skins", new string[] { "All Skins (0)", "Ta0Soft Skins (0)", "Valve Skins (0)", "User Skins (0)", "My Skins (0)" }, "Filter Skins");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new UI.Menu(null);
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            p_WebClient = new WebClient();
            p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);
        }

        public override void Dispose()
        {
            p_WebClient.Dispose();
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int x2 = x / 3;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing skins, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No skins found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (FilteredItems[i].URL == Global.Skin.URL) textColor = Global.Skin.List.Play_TextColor;
                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    sf.Alignment = StringAlignment.Near;

                    g.DrawString(FilteredItems[i].Name, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x2, ItemHeight), sf);
                    g.DrawString(FilteredItems[i].AuthorPersonaName, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + 3) + x2, (bounds.Y + y) + 3, (x2 * 2) - 80, ItemHeight), sf);

                    sf.Alignment = StringAlignment.Far;

                    g.DrawString(FilteredItems[i].Created, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 80, (bounds.Y + y) + 3, 80, ItemHeight), sf);

                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<Skin> items = new List<Skin>();

            foreach (Skin item in Items)
            {
                if (item.Name.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllSkins:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.Ta0softSkins:
                    foreach (Skin item in items)
                    {
                        if (item.URL.Contains("/ta0soft/")) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.ValveSkins:
                    foreach (Skin item in items)
                    {
                        if (item.URL.Contains("/valve/")) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.UserSkins:
                    foreach (Skin item in items)
                    {
                        if (item.URL.Contains("/user/")) FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.MySkins:
                    foreach (Skin item in items)
                    {
                        if (item.Author == Global.Steam.Client.GetSteamID().ToUInt64().ToString()) FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new SkinListSorter(SkinListSorter.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Skins (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "Ta0soft Skins (" + Global.FormatNumber(GetTa0softSkinsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "Valve Skins (" + Global.FormatNumber(GetValveSkinsCount()) + ")";
            p_FilterBar.DropDown.Items[3] = "User Skins (" + Global.FormatNumber(GetUserSkinsCount()) + ")";
            p_FilterBar.DropDown.Items[4] = "My Skins (" + Global.FormatNumber(GetMySkinsCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (SelectedItem == null) return;

            if (e.Button == MouseButtons.Left && Clicks > 1)
            {
                Global.Skin = new UI.Skin(SelectedItem.URL);
                Owner.ReDraw();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                p_ContextMenu.Clear();

                if (SelectedItem != null)
                {
                    //if (SelectedItem.Author == Global.Steam.Client.GetSteamID().ToInt64().ToString()) p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(">", "Load Skin"), new MenuItem("¤", "Edit Skin..."), new MenuItem("x", "Delete Skin"), new MenuItem(string.Empty, "-") });
                    //else p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(">", "Load Skin"), new MenuItem("¤", "Edit Skin..."), new MenuItem(string.Empty, "-") });
                    p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(">", "Load Skin"), new MenuItem("¤", "Edit Skin..."), new MenuItem(string.Empty, "-") });
                }
                p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("q", "Refresh Skins"), new MenuItem(string.Empty, "-"), new MenuItem("s", "Generate Skin"), new MenuItem(">", "New Skin...") });

                p_ContextMenu.Show();
            }

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            SkinDesigner sd = null;

            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Load Skin":
                    Global.Skin = new UI.Skin(SelectedItem.URL);
                    Owner.ReDraw();
                    break;
                case "Edit Skin...":
                    Global.Skin = new UI.Skin(SelectedItem.URL);
                    Owner.ReDraw();

                    sd = new UI.SkinDesigner();
                    if (sd.ShowDialog(Owner) == DialogResult.OK) Refresh();
                    sd.Dispose();
                    break;
                case "Delete Skin":
                    InfoDialog id = new InfoDialog("Are you sure you want to delete this skin?" + Environment.NewLine + Environment.NewLine + SelectedItem.Name, InfoDialog.InfoButtons.YesNo);
                    if (id.ShowDialog(Owner) == DialogResult.Yes)
                    {
                        //Global.Skin.Delete();

                        Refresh();
                    }
                    id.Dispose();
                    break;
                case "Refresh Skins":
                    Refresh();
                    break;
                case "Generate Skin":
                    Global.Skin.Generate();
                    Owner.ReDraw();
                    break;
                case "New Skin...":
                    Global.Skin.New();
                    Owner.ReDraw();

                    sd = new UI.SkinDesigner();
                    if (sd.ShowDialog(Owner) == DialogResult.OK) Refresh();
                    sd.Dispose();
                    break;
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    List<Skin> list = new List<Skin>();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(e.Result);
                    XmlNodeList nodeList = xml.SelectNodes("Steamp3.Skins/Skin");

                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(new UI.Skin(node));
                    }

                    Items.Clear();
                    AddRange(list);

                    list.Clear();
                }
                catch { }

                Refreshing = false;
            }
        }
        #endregion

        #region Public Methods
        public int GetTa0softSkinsCount()
        {
            int result = 0;

            foreach (Skin item in Items)
            {
                if (item.URL.Contains("/ta0soft/")) result++;
            }

            return result;
        }

        public int GetValveSkinsCount()
        {
            int result = 0;

            foreach (Skin item in Items)
            {
                if (item.URL.Contains("/valve/")) result++;
            }

            return result;
        }

        public int GetUserSkinsCount()
        {
            int result = 0;

            foreach (Skin item in Items)
            {
                if (item.URL.Contains("/user/")) result++;
            }

            return result;
        }

        public int GetMySkinsCount()
        {
            int result = 0;

            foreach (Skin item in Items)
            {
                if (item.URL.Contains("/user/") && item.Author == Global.Steam.Client.GetSteamID().ToUInt64().ToString()) result++;
            }

            return result;
        }

        public void Refresh()
        {
            if (!Global.MediaPlayer.IsOnline) return;

            Refreshing = true;

            p_WebClient.DownloadStringAsync(new Uri("http://steamp3.ta0soft.com/skins/skins.php?xml=1"));
        }
        #endregion
    }
    #endregion

    #region SkinListSorter
    public class SkinListSorter : IComparer<Skin>
    {
        #region Enums
        public enum SortMode : int
        {
            None,
            Ascending,
            Descending,
        }
        #endregion

        #region Objects
        private SortMode p_Mode;
        #endregion

        #region Properties
        public SortMode Mode
        {
            get { return p_Mode; }
            set { p_Mode = value; }
        }
        #endregion

        #region Constructor/Destructor
        public SkinListSorter(SortMode mode) : base()
        {
            p_Mode = mode;
        }

        public virtual void Dispose()
        {
            p_Mode = SortMode.None;
        }
        #endregion

        #region Public Methods
        public int Compare(Skin item1, Skin item2)
        {
            switch (p_Mode)
            {
                case SortMode.Ascending:
                    return item1.Name.ToLower().CompareTo(item2.Name.ToLower());
                case SortMode.Descending:
                    return item2.Name.ToLower().CompareTo(item1.Name.ToLower());
                default:
                    return 0;
            }
        }
        #endregion
    }
    #endregion

    #region Slider
    public class Slider : Control
    {
        #region Events
        public event EventHandler MaximumChanged;
        public event EventHandler ValueChanged;
        public event EventHandler SlidableChanged;
        #endregion

        #region Objects
        private int p_Maximum, p_Value;
        private bool p_Slidable;
        #endregion

        #region Properties
        public int Maximum
        {
            get { return p_Maximum; }
            set
            {
                if (value < 0) value = 0;
                if (value < p_Value) p_Value = 0;

                p_Maximum = value;
                OnMaximumChanged(new EventArgs());
                ReDraw();
            }
        }

        public int Value
        {
            get { return p_Value; }
            set
            {
                if (value < 0) value = 0;
                if (value > p_Maximum) value = p_Maximum;

                p_Value = value;
                OnValueChanged(new EventArgs());
                ReDraw();
            }
        }

        public bool Slidable
        {
            get { return p_Slidable; }
            set
            {
                p_Slidable = value;
                OnSlidableChanged(new EventArgs());
                //ReDraw();
            }
        }
        #endregion

        #region Constructor/Destructor
        public Slider(Window owner, Control parent) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_Maximum = 0;
            p_Value = 0;
            p_Slidable = false;
        }

        public override void Dispose()
        {
            p_Slidable = false;
            p_Value = 0;
            p_Maximum = 0;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Color bgColor = Global.Skin.Slider.BGColor;
            Color bgColor2 = Global.Skin.Slider.BGColor2;
            Color borderColor = Global.Skin.Slider.BorderColor;

            if (Enabled)
            {
                if (IsMouseMoving)
                {
                    bgColor = Global.Skin.Slider.Over_BGColor;
                    bgColor2 = Global.Skin.Slider.Over_BGColor2;
                    borderColor = Global.Skin.Slider.Over_BorderColor;
                }
            }
            else
            {

            }

            Rectangle r = new Rectangle(bounds.X, bounds.Y, Global.GetPercent(p_Value, p_Maximum, bounds.Width - 1), bounds.Height - 1);
            if (r.Width > 3) Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, bgColor2, bgColor, LinearGradientMode.Horizontal));

            r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            Global.DrawRoundedRectangle(g, r, new Pen(borderColor));

            base.OnDraw(g, bounds);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (p_Slidable && e.Button == MouseButtons.Left) Value = Global.GetPercent(e.X - Bounds.X, Bounds.Width - 1, p_Maximum);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (p_Slidable && e.Button == MouseButtons.Left) Value = Global.GetPercent(e.X - Bounds.X, Bounds.Width - 1, p_Maximum);

            base.OnMouseMove(e);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnMaximumChanged(EventArgs e)
        {
            if (MaximumChanged != null) MaximumChanged.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null) ValueChanged.Invoke(this, e);
        }

        protected virtual void OnSlidableChanged(EventArgs e)
        {
            if (SlidableChanged != null) SlidableChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Station
    public class Station : GenericListItem
    {
        #region Objects
        private string p_URL, p_Name, p_Genre, p_PlaylistData, p_StreamURL;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }
        
        public string Name
        {
            get { return p_Name; }
        }

        public string Genre
        {
            get { return p_Genre; }
        }

        public string PlaylistData
        {
            get
            {
                if (string.IsNullOrEmpty(p_PlaylistData))
                {
                    WebClient wc = new WebClient();
                    p_PlaylistData = wc.DownloadString(p_URL);

                    wc.Dispose();
                }

                return p_PlaylistData;
            }
        }

        public string StreamURL
        {
            get
            {
                if (string.IsNullOrEmpty(p_StreamURL))
                {
                    if (p_URL.ToLower().Contains(".pls")) p_StreamURL = Global.GetIniValue(PlaylistData, "File1");
                    else p_StreamURL = Global.GetFirstLine(PlaylistData);
                }

                return p_StreamURL;
            }
        }
        #endregion

        #region Constructor/Destructor
        public Station(string url, string name, string genre) : base()
        {
            p_URL = url;
            p_Name = name;
            p_Genre = genre;
            p_PlaylistData = string.Empty;
            p_StreamURL = string.Empty;
        }

        public override void Dispose()
        {
            p_StreamURL = string.Empty;
            p_PlaylistData = string.Empty;
            p_Genre = string.Empty;
            p_Name = string.Empty;
            p_URL = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_URL;
        }
        #endregion

        #region Public Methods
        public XmlElement ToXml(XmlDocument xml)
        {
            XmlElement element = xml.CreateElement("Station");

            element.SetAttribute("URL", p_URL);
            element.SetAttribute("Name", p_Name);
            element.SetAttribute("Genre", p_Genre);

            return element;
        }
        #endregion
    }
    #endregion

    #region StationDialog
    public class StationDialog : Dialog
    {
        #region Objects
        private TextBoxGroup p_URLGroup, p_NameGroup;
        private DropDownGroup p_GenreGroup;
        private Button p_AutoButton, p_AddButton, p_CancelButton;
        private Shoutcast p_Shoutcast;
        #endregion

        #region Properties
        public TextBoxGroup URLGroup
        {
            get { return p_URLGroup; }
        }

        public TextBoxGroup NameGroup
        {
            get { return p_NameGroup; }
        }

        public DropDownGroup GenreGroup
        {
            get { return p_GenreGroup; }
        }
        #endregion

        #region Constructor/Destructor
        public StationDialog()
        {
            Sizable = false;
            Size = new Size(460, 146);
            Text = "Add Radio Station";

            int x = (this.Width / 3) - 12;

            p_URLGroup = new TextBoxGroup(this, null, "URL (.pls/.m3u):", string.Empty);
            p_URLGroup.ShowDefaultButton = false;
            p_URLGroup.TextChanged += new EventHandler(p_URLGroup_TextChanged);
            p_URLGroup.SetBounds(12, 30, this.Width - 24, 20);

            p_NameGroup = new TextBoxGroup(this, null, "Station Name:", string.Empty);
            p_NameGroup.ShowDefaultButton = false;
            p_NameGroup.TextChanged += new EventHandler(p_NameGroup_TextChanged);
            p_NameGroup.SetBounds(12, 56, this.Width - 24, 20);

            p_GenreGroup = new DropDownGroup(this, null, "Station Genre:", Global.Settings.OutputDevice, "The best-matched genre for the station", new string[] { "70s/80s/90s", "Alternative", "Blues", "Classical", "Country", "Comedy", "Dance", "Gospel", "Hip-Hop/Rap", "Jazz/Latin", "Metal", "News/Talk", "Oldies", "Pop", "Punk/Ska", "R&B/Soul", "Reggae", "Rock", "Techno/Trance", "Top Hits/Classic", "Underground", "Unknown/Other" });
            p_GenreGroup.DefaultButtonClicked += new EventHandler(p_GenreGroup_DefaultButtonClicked);
            p_GenreGroup.DropDown.SelectedIndex = p_GenreGroup.DropDown.Items.Count - 1;
            p_GenreGroup.SetBounds(12, 82, this.Width - 24, 20);

            p_AutoButton = new Button(this, null, "Auto-fill Info");
            p_AutoButton.Enabled = false;
            p_AutoButton.MouseClick += new MouseEventHandler(p_AutoButton_MouseClick);
            p_AutoButton.SetBounds(12, 114, x, 20);

            p_AddButton = new Button(this, null, "Add Station");
            p_AddButton.Enabled = false;
            p_AddButton.MouseClick += new MouseEventHandler(p_AddButton_MouseClick);
            p_AddButton.SetBounds(x + 18, 114, x, 20);

            p_CancelButton = new Button(this, null, "Cancel");
            p_CancelButton.MouseClick += new MouseEventHandler(p_CancelButton_MouseClick);
            p_CancelButton.SetBounds((x * 2) + 24, 114, x, 20);

            p_Shoutcast = null;
        }

        ~StationDialog()
        {
            if (p_Shoutcast != null) p_Shoutcast.Dispose();
        }
        #endregion

        #region Child Events
        private void p_URLGroup_TextChanged(object sender, EventArgs e)
        {
            p_AutoButton.Enabled = p_URLGroup.TextBox.Text.ToLower().Contains(".pls");

            if (p_URLGroup.TextBox.Text.ToLower().Contains(".pls") || p_URLGroup.TextBox.Text.ToLower().Contains(".m3u"))
            {
                if (Global.ReplaceString(p_NameGroup.TextBox.Text, " ", "") != string.Empty) p_AddButton.Enabled = true;
                else p_AddButton.Enabled = false;
            }
            else p_AddButton.Enabled = false;
        }

        private void p_NameGroup_TextChanged(object sender, EventArgs e)
        {
            if (p_URLGroup.TextBox.Text.ToLower().Contains(".pls") || p_URLGroup.TextBox.Text.ToLower().Contains(".m3u"))
            {
                if (Global.ReplaceString(p_NameGroup.TextBox.Text, " ", "") != string.Empty) p_AddButton.Enabled = true;
                else p_AddButton.Enabled = false;
            }
            else p_AddButton.Enabled = false;
        }

        private void p_GenreGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            p_GenreGroup.DropDown.SelectedIndex = p_GenreGroup.DropDown.Items.Count - 1;
        }

        private void p_AutoButton_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();
                string result = wc.DownloadString(p_URLGroup.TextBox.Text);

                wc.Dispose();

                p_NameGroup.TextBox.Text = Global.GetIniValue(result, "Title1");

                p_Shoutcast = new Shoutcast(Global.GetIniValue(result, "File1"), 15000);
                p_Shoutcast.HeaderReceived += new Shoutcast.HeaderReceivedEventHandler(p_Shoutcast_HeaderReceived);
                p_Shoutcast.Connect(false);
            }
            catch { }
        }

        private void p_AddButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void p_CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void p_Shoutcast_HeaderReceived(object sender, int metaInt, string name, string genre, string url)
        {
            p_NameGroup.TextBox.Text = name;

            for (int i = 0; i < p_GenreGroup.DropDown.Items.Count; i++)
            {
                string[] s = p_GenreGroup.DropDown.Items[i].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s2 in s)
                {
                    if (genre.ToLower().Contains(s2.ToLower()))
                    {
                        p_GenreGroup.DropDown.SelectedIndex = i;
                        return;
                    }
                }
            }
        }
        #endregion
    }
    #endregion

    #region StationList
    public class StationList : GenericList<Station>
    {
        #region Enums
        public new enum FilterTypes : int
        {
            AllStations = 0,
            MyStations = 1,
            SeventiesEightiesNineties = 2,
            Alternative = 3,
            Blues = 4,
            Classical = 5,
            Country = 6,
            Comedy = 7,
            Dance = 8,
            Gospel = 9,
            HipHopRap = 10,
            JazzLatin = 11,
            Metal = 12,
            NewsTalk = 13,
            Oldies = 14,
            Pop = 15,
            PunkSka = 16,
            RnBSoul = 17,
            Reggae = 18,
            Rock = 19,
            TechnoTrance = 20,
            TopHitsClassic = 21,
            Underground = 22,
            UnknownOther = 23,
        }
        #endregion

        #region Objects
        private FilterBar p_FilterBar;
        private Menu p_ContextMenu;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public FilterBar FilterBar
        {
            get { return p_FilterBar; }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }

        public override bool Refreshing
        {
            get { return base.Refreshing; }
            set
            {
                base.Refreshing = value;

                p_FilterBar.Enabled = !base.Refreshing;
                //ReDraw();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_FilterBar.Visible = base.Visible;
            }
        }
        #endregion

        #region Constructor/Destructor
        public StationList(Window owner, Control parent) : base(owner, parent)
        {
            p_FilterBar = new FilterBar(owner, parent, "Search Stations", new string[] { "All Stations (0)", "My Stations (0)", "70s/80s/90s (0)", "Alternative (0)", "Blues (0)", "Classical (0)", "Country (0)", "Comedy (0)", "Dance (0)", "Gospel (0)", "Hip-Hop/Rap (0)", "Jazz/Latin (0)", "Metal (0)", "News/Talk (0)", "Oldies (0)", "Pop (0)", "Punk/Ska (0)", "R&B/Soul (0)", "Reggae (0)", "Rock (0)", "Techno/Trance (0)", "Top Hits/Classic (0)", "Underground (0)", "Unknown/Other (0)" }, "Filter Stations");
            p_FilterBar.SelectedIndexChanged += new EventHandler(p_FilterBar_SelectedIndexChanged);
            p_FilterBar.TextChanged += new EventHandler(p_FilterBar_TextChanged);

            p_ContextMenu = new Menu(null);
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);

            p_WebClient = new WebClient();
            p_WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(p_WebClient_DownloadStringCompleted);
        }

        public override void Dispose()
        {
            p_WebClient.Dispose();
            p_ContextMenu.Dispose();

            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int y = 0;

            if (Refreshing) g.DrawString("Refreshing stations, please wait...", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (Items.Count == 0) g.DrawString("No stations found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No " + Global.LeftOf(p_FilterBar.DropDown.Text, " (") + " found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Equals(SelectedItem))
                    {
                        textColor = Global.Skin.List.Down_TextColor;

                        Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, x, ItemHeight);
                        LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                        b.WrapMode = WrapMode.TileFlipX;

                        g.FillRectangle(b, r);
                    }

                    if (FilteredItems[i].Equals(PlayingItem)) textColor = Global.Skin.List.Play_TextColor;
                    if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    sf.Alignment = StringAlignment.Near;
                    g.DrawString(FilteredItems[i].Name, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x - 110, ItemHeight), sf);

                    sf.Alignment = StringAlignment.Far;
                    g.DrawString(FilteredItems[i].Genre, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 110, (bounds.Y + y) + 3, 110, ItemHeight), sf);

                    y += ItemHeight;
                }
            }
        }

        protected override void FilterItems()
        {
            FilteredItems.Clear();

            List<Station> items = new List<Station>();

            foreach (Station item in Items)
            {
                if (item.Name.ToLower().Contains(p_FilterBar.TextBox.Text.ToLower())) items.Add(item);
            }

            switch ((FilterTypes)p_FilterBar.DropDown.SelectedIndex)
            {
                case FilterTypes.AllStations:
                    FilteredItems.AddRange(items);
                    break;
                case FilterTypes.MyStations:
                    FilteredItems.AddRange(Global.Settings.RadioStations);
                    break;
                case FilterTypes.SeventiesEightiesNineties:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "70s/80s/90s") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Alternative:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Alternative") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Blues:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Blues") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Classical:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Classical") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Country:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Country") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Comedy:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Comedy") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Dance:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Dance") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Gospel:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Gospel") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.HipHopRap:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Hip-Hop/Rap") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.JazzLatin:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Jazz/Latin") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Metal:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Metal") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.NewsTalk:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "News/Talk") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Oldies:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Oldies") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Pop:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Pop") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.PunkSka:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Punk/Ska") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.RnBSoul:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "R&B/Soul") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Reggae:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Reggae") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Rock:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Rock") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.TechnoTrance:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Techno/Trance") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.TopHitsClassic:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Top Hits/Classic") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.Underground:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Underground") FilteredItems.Add(item);
                    }
                    break;
                case FilterTypes.UnknownOther:
                    foreach (Station item in items)
                    {
                        if (item.Genre == "Unknown/Other") FilteredItems.Add(item);
                    }
                    break;
            }

            FilteredItems.Sort(new StationListSorter(StationListSorter.SortMode.Ascending));
            ReDraw(); //?
        }

        protected override void OnPlayingItemChanged(EventArgs e)
        {
            if (PlayingItem != null)
            {
                Global.MainWindow.Playlist.ID3Title.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Artist.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Album.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Genre.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Track.ReadOnly = true;
                Global.MainWindow.Playlist.ID3TrackCount.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Year.ReadOnly = true;
                Global.MainWindow.Playlist.ID3Rating.ReadOnly = true;
            }
            else
            {
                Global.MainWindow.Playlist.ID3Title.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Artist.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Album.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Genre.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Track.ReadOnly = false;
                Global.MainWindow.Playlist.ID3TrackCount.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Year.ReadOnly = false;
                Global.MainWindow.Playlist.ID3Rating.ReadOnly = false;
            }

            base.OnPlayingItemChanged(e);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            p_FilterBar.SetBounds(Bounds.X, Bounds.Y - 28, Bounds.Width, 22);

            base.OnBoundsChanged(e);
        }

        protected override void OnFilteredItemsChanged(EventArgs e)
        {
            p_FilterBar.DropDown.Items[0] = "All Stations (" + Global.FormatNumber(Items.Count) + ")";
            p_FilterBar.DropDown.Items[1] = "My Stations (" + Global.FormatNumber(GetMyStationsCount()) + ")";
            p_FilterBar.DropDown.Items[2] = "70s/80s/90s (" + Global.FormatNumber(GetSeventiesEightiesNinetiesCount()) + ")";
            p_FilterBar.DropDown.Items[3] = "Alternative (" + Global.FormatNumber(GetAlternativeCount()) + ")";
            p_FilterBar.DropDown.Items[4] = "Blues (" + Global.FormatNumber(GetBluesCount()) + ")";
            p_FilterBar.DropDown.Items[5] = "Classical (" + Global.FormatNumber(GetClassicalCount()) + ")";
            p_FilterBar.DropDown.Items[6] = "Country (" + Global.FormatNumber(GetCountryCount()) + ")";
            p_FilterBar.DropDown.Items[7] = "Comedy (" + Global.FormatNumber(GetComedyCount()) + ")";
            p_FilterBar.DropDown.Items[8] = "Dance (" + Global.FormatNumber(GetDanceCount()) + ")";
            p_FilterBar.DropDown.Items[9] = "Gospel (" + Global.FormatNumber(GetGospelCount()) + ")";
            p_FilterBar.DropDown.Items[10] = "Hip-Hop/Rap (" + Global.FormatNumber(GetHipHopRapCount()) + ")";
            p_FilterBar.DropDown.Items[11] = "Jazz/Latin (" + Global.FormatNumber(GetJazzLatinCount()) + ")";
            p_FilterBar.DropDown.Items[12] = "Metal (" + Global.FormatNumber(GetMetalCount()) + ")";
            p_FilterBar.DropDown.Items[13] = "News/Talk (" + Global.FormatNumber(GetNewsTalkCount()) + ")";
            p_FilterBar.DropDown.Items[14] = "Oldies (" + Global.FormatNumber(GetOldiesCount()) + ")";
            p_FilterBar.DropDown.Items[15] = "Pop (" + Global.FormatNumber(GetPopCount()) + ")";
            p_FilterBar.DropDown.Items[16] = "Punk/Ska (" + Global.FormatNumber(GetPunkSkaCount()) + ")";
            p_FilterBar.DropDown.Items[17] = "R&B/Soul (" + Global.FormatNumber(GetRnBSoulCount()) + ")";
            p_FilterBar.DropDown.Items[18] = "Reggae (" + Global.FormatNumber(GetReggaeCount()) + ")";
            p_FilterBar.DropDown.Items[19] = "Rock (" + Global.FormatNumber(GetRockCount()) + ")";
            p_FilterBar.DropDown.Items[20] = "Techno/Trance (" + Global.FormatNumber(GetTechnoTranceCount()) + ")";
            p_FilterBar.DropDown.Items[21] = "Top Hits/Classic (" + Global.FormatNumber(GetTopHitsClassicCount()) + ")";
            p_FilterBar.DropDown.Items[22] = "Underground (" + Global.FormatNumber(GetUndergroundCount()) + ")";
            p_FilterBar.DropDown.Items[23] = "Unknown/Other (" + Global.FormatNumber(GetUnknownOtherCount()) + ")";
            p_FilterBar.DropDown.Text = p_FilterBar.DropDown.Items[p_FilterBar.DropDown.SelectedIndex];

            base.OnFilteredItemsChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (SelectedItem == null) return;

            if (e.Button == MouseButtons.Left && Clicks > 1)
            {
                Global.MediaPlayer.Stream(SelectedItem);
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                p_ContextMenu.Clear();

                //if (SelectedItem != null) p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("4", "Stream"), new MenuItem("x", "Delete Station"), new MenuItem(string.Empty, "-") });
                if (SelectedItem != null) p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("4", "Stream"), new MenuItem(string.Empty, "-") });
                p_ContextMenu.AddRange(new MenuItem[] { new MenuItem("q", "Refresh Stations"), new MenuItem(string.Empty, "-"), new MenuItem("º", "Add Station...") });

                p_ContextMenu.Show();
            }

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_FilterBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_FilterBar_TextChanged(object sender, EventArgs e)
        {
            UpdateFilteredItems();
        }

        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            InfoDialog id;

            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Stream":
                    Global.MediaPlayer.Stream(SelectedItem);
                    break;
                //case "Delete Station":
                    //id = new InfoDialog("Are you sure you want to delete this station?" + Environment.NewLine + Environment.NewLine + SelectedItem.Name, InfoDialog.InfoButtons.YesNo);
                    //if (id.ShowDialog(Owner) == DialogResult.Yes)
                    //{
                        //Global.Settings.RadioStations.Remove(SelectedItem);

                        //Refresh(); //?
                    //}

                    //id.Dispose();
                    //break;
                case "Refresh Stations":
                    Refresh();
                    break;
                case "Add Station...":
                    StationDialog sd = new StationDialog();
                    if (sd.ShowDialog(Owner) == DialogResult.OK)
                    {
                        foreach (Station station in Items)
                        {
                            if (station.URL.ToLower().Equals(sd.URLGroup.TextBox.Text.ToLower()))
                            {
                                id = new InfoDialog("Radio station already found, please try again.");
                                id.ShowDialog(Owner);
                                id.Dispose();
                                return;
                            }
                        }

                        Global.Settings.RadioStations.Add(new Station(sd.URLGroup.TextBox.Text, sd.NameGroup.TextBox.Text, sd.GenreGroup.DropDown.Text));

                        Refresh(); //?
                    }
                    sd.Dispose();
                    break;
            }
        }

        private void p_WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    List<Station> list = new List<Station>();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(e.Result);
                    XmlNodeList nodeList = xml.SelectNodes("Steamp3.Radio/Station");

                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(new Station(Global.GetXmlValue(node, "URL", string.Empty), Global.GetXmlValue(node, "Name", "N/A"), Global.GetXmlValue(node, "Genre", "N/A")));
                    }

                    list.AddRange(Global.Settings.RadioStations);

                    Items.Clear();
                    AddRange(list);

                    list.Clear();
                }
                catch { }

                Refreshing = false;
            }
        }
        #endregion

        #region Public Methods
        public int GetMyStationsCount()
        {
            return Global.Settings.RadioStations.Count;
        }

        public int GetSeventiesEightiesNinetiesCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "70s/80s/90s") result++;
            }

            return result;
        }

        public int GetAlternativeCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Alternative") result++;
            }

            return result;
        }

        public int GetBluesCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Blues") result++;
            }

            return result;
        }

        public int GetClassicalCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Classical") result++;
            }

            return result;
        }

        public int GetCountryCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Country") result++;
            }

            return result;
        }

        public int GetComedyCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Comedy") result++;
            }

            return result;
        }

        public int GetDanceCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Dance") result++;
            }

            return result;
        }

        public int GetGospelCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Gospel") result++;
            }

            return result;
        }

        public int GetHipHopRapCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Hip-Hop/Rap") result++;
            }

            return result;
        }

        public int GetJazzLatinCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Jazz/Latin") result++;
            }

            return result;
        }

        public int GetMetalCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Metal") result++;
            }

            return result;
        }

        public int GetNewsTalkCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "News/Talk") result++;
            }

            return result;
        }

        public int GetOldiesCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Oldies") result++;
            }

            return result;
        }

        public int GetPopCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Pop") result++;
            }

            return result;
        }

        public int GetPunkSkaCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Punk/Ska") result++;
            }

            return result;
        }

        public int GetRnBSoulCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "R&B/Soul") result++;
            }

            return result;
        }

        public int GetReggaeCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Reggae") result++;
            }

            return result;
        }

        public int GetRockCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Rock") result++;
            }

            return result;
        }

        public int GetTechnoTranceCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Techno/Trance") result++;
            }

            return result;
        }

        public int GetTopHitsClassicCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Top Hits/Classic") result++;
            }

            return result;
        }

        public int GetUndergroundCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Underground") result++;
            }

            return result;
        }

        public int GetUnknownOtherCount()
        {
            int result = 0;

            foreach (Station item in Items)
            {
                if (item.Genre == "Unknown/Other") result++;
            }

            return result;
        }

        public void Refresh()
        {
            if (!Global.MediaPlayer.IsOnline) return;

            Refreshing = true;

            p_WebClient.DownloadStringAsync(new Uri("http://steamp3.ta0soft.com/radio/Steamp3.Radio.xml"));
        }
        #endregion
    }
    #endregion

    #region StationListSorter
    public class StationListSorter : IComparer<Station>
    {
        #region Enums
        public enum SortMode : int
        {
            None,
            Ascending,
            Descending,
        }
        #endregion

        #region Objects
        private SortMode p_Mode;
        #endregion

        #region Properties
        public SortMode Mode
        {
            get { return p_Mode; }
            set { p_Mode = value; }
        }
        #endregion

        #region Constructor/Destructor
        public StationListSorter(SortMode mode)
            : base()
        {
            p_Mode = mode;
        }

        public void Dispose()
        {
            p_Mode = SortMode.None;
        }
        #endregion

        #region Public Methods
        public int Compare(Station item1, Station item2)
        {
            switch (p_Mode)
            {
                case SortMode.Ascending:
                    return item1.Name.ToLower().CompareTo(item2.Name.ToLower());
                case SortMode.Descending:
                    return item2.Name.ToLower().CompareTo(item1.Name.ToLower());
                default:
                    return 0;
            }
        }
        #endregion
    }
    #endregion

    #region SystemButton
    public class SystemButton : Control
    {
        #region Constructors/Destructor
        public SystemButton(Window owner, Control parent, string icon) : base(owner, parent)
        {
            DrawFocusRect = false;
            Icon = icon;
        }

        public SystemButton(Window owner, Control parent, string icon, string toolTipText) : base(owner, parent)
        {
            DrawFocusRect = false;
            Icon = icon;
            ToolTipText = toolTipText;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            RectangleF rf = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.None;

            Color textColor = Global.Skin.Window.TextColor;

            if (Enabled)
            {
                if (IsMouseDown) textColor = Global.Skin.Window.Down_TextColor;
                else if (IsMouseMoving) textColor = Global.Skin.Window.Over_TextColor;
            }
            else textColor = Color.FromArgb(75, Global.Skin.Window.TextColor);

            g.DrawString(Icon, Skin.IconFont, new SolidBrush(textColor), rf, sf);

            base.OnDraw(g, bounds);
        }
        #endregion
    }
    #endregion

    #region Tab
    public class Tab : GenericListItem
    {
        #region Objects
        private string p_Icon;
        private int p_TabWidth;
        #endregion

        #region Properties
        public string Icon
        {
            get { return p_Icon; }
        }

        public int TabWidth
        {
            get { return p_TabWidth; }
        }
        #endregion

        #region Constructor/Destructor
        public Tab(string icon, string text, int tabWidth) : base()
        {
            Text = text;

            p_Icon = icon;
            p_TabWidth = tabWidth;
        }

        public override void Dispose()
        {
            p_TabWidth = 0;
            p_Icon = null;

            base.Dispose();
        }
        #endregion
    }
    #endregion

    #region TabContainer
    public class TabContainer : GenericList<Tab>
    {
        #region Objects
        private SystemButton p_LeftButton, p_RightButton;
        #endregion

        #region Properties
        public SystemButton LeftButton
        {
            get { return p_LeftButton; }
        }

        public SystemButton RightButton
        {
            get { return p_RightButton; }
        }
        #endregion

        #region Constructor/Destructor
        public TabContainer(Window owner, Control parent) : base(owner, parent)
        {
            DrawFocusRect = false;

            p_LeftButton = new SystemButton(Owner, Parent, "3");
            p_LeftButton.MouseClick += new MouseEventHandler(p_LeftButton_MouseClick);

            p_RightButton = new SystemButton(Owner, Parent, "4");
            p_RightButton.MouseClick += new MouseEventHandler(p_RightButton_MouseClick);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        public override int GetVisibleItemsCount()
        {
            int x = 0;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (x > (Bounds.Width - FilteredItems[i].TabWidth) - 100) return i;

                x += FilteredItems[i].TabWidth;
            }

            return FilteredItems.Count;
        }

        public override bool IsBeforeBounds(int index)
        {
            if (index < FirstIndex) return true;

            return false;
        }

        public override bool IsAfterBounds(int index)
        {
            if (index <= FirstIndex) return false;
            int x = 0;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (i == index && x > (Bounds.Width - FilteredItems[i].TabWidth) - 100) return true;

                x += FilteredItems[i].TabWidth;
            }

            return false;
        }

        public override void EnsureVisible()
        {
            for (int i = 0; i < FilteredItems.Count; i++)
            {
                if (FilteredItems[i].Equals(SelectedItem))
                {
                    if (IsBeforeBounds(i)) FirstIndex = i;
                    else if (IsAfterBounds(i)) FirstIndex = i - (GetVisibleItemsCount() - 1);

                    return;
                }
            }
        }

        public override bool HitTest(MouseEventArgs e)
        {
            if (!Visible) return false;

            if (Bounds.Contains(e.X, e.Y))
            {
                if (e.X < Bounds.X + GetVisibleItemsWidth() && e.Y < Bounds.Y + 24) return true;
            }

            return false;
        }

        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            if (Enabled)
            {
                Rectangle r = new Rectangle(bounds.X, bounds.Y + 24, bounds.Width, bounds.Height - 24);
                LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.Tab.BGColor, Global.Skin.Tab.BGColor2, LinearGradientMode.Vertical);
                b.WrapMode = WrapMode.TileFlipX;

                g.FillRectangle(b, r);

                DrawItems(g, bounds);
            }
            else
            {

            }

            //base.OnDraw(g, bounds);
        }

        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            if (FilteredItems.Count == 0) return;

            int x = 0;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.Word;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (x > (bounds.Width - FilteredItems[i].TabWidth) - 100) break;

                Color textColor = Global.Skin.Tab.TextColor;
                Rectangle r;

                if (FilteredItems[i].Equals(SelectedItem))
                {
                    textColor = Global.Skin.Tab.Down_TextColor;

                    r = new Rectangle(bounds.X + x, bounds.Y, FilteredItems[i].TabWidth - 2, 24);
                    LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.Tab.Down_BGColor, Global.Skin.Tab.BGColor, LinearGradientMode.Vertical);
                    b.WrapMode = WrapMode.TileFlipX;

                    g.FillRectangle(b, r);

                    if (i > 0) g.DrawLine(new Pen(Global.Skin.Tab.LineColor), bounds.X, bounds.Y + 24, (bounds.X + x) - 1, bounds.Y + 24);
                    g.DrawLine(new Pen(Global.Skin.Tab.LineColor), (bounds.X + x + FilteredItems[i].TabWidth) - 2, bounds.Y + 24, (bounds.X + bounds.Width) - 1, bounds.Y + 24);
                }
                else if (FilteredItems[i].Equals(HotItem))
                {
                    textColor = Global.Skin.Tab.Over_TextColor;

                    r = new Rectangle(bounds.X + x, bounds.Y, FilteredItems[i].TabWidth - 2, 22);
                    g.FillRectangle(new SolidBrush(Global.Skin.Tab.Over_BGColor), r);
                }
                else
                {
                    textColor = Global.Skin.Tab.TextColor;

                    r = new Rectangle(bounds.X + x, bounds.Y, FilteredItems[i].TabWidth - 2, 22);
                    g.FillRectangle(new SolidBrush(Global.Skin.Tab.BGColor), r);
                }

                g.DrawString(FilteredItems[i].Icon, Skin.IconFont, new SolidBrush(textColor), (bounds.X + x) + 2, bounds.Y + 2);
                if (FilteredItems[i].TabWidth > 30) g.DrawString(FilteredItems[i].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) + 18, bounds.Y, FilteredItems[i].TabWidth - 22, 22), sf);

                x += FilteredItems[i].TabWidth;
            }

            //g.FillRectangle(Brushes.Red, x + 6, bounds.Y, 22, 22);
            //g.DrawString("3", Global.Skin.IconFont, Brushes.White, x + 4, bounds.Y + 1);
            //g.DrawString("4", Global.Skin.IconFont, Brushes.White, x + 12, bounds.Y + 1);
        }

        protected override void OnBoundsChanged(EventArgs e)
        {
            ScrollBar.SetBounds(0, 0, 0, 0);

            p_LeftButton.SetBounds((Bounds.X + GetVisibleItemsWidth()) + 3, Bounds.Y, 8, 18);
            p_RightButton.SetBounds((Bounds.X + GetVisibleItemsWidth()) + 12, Bounds.Y, 8, 18);

            p_LeftButton.Visible = GetVisibleItemsCount() < FilteredItems.Count;
            p_RightButton.Visible = p_LeftButton.Visible;

            //base.OnBoundsChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    SelectedIndex--;
                    break;
                case Keys.Right:
                    SelectedIndex++;
                    break;
                case Keys.Home:
                    SelectedIndex = 0;
                    break;
                case Keys.End:
                    SelectedIndex = FilteredItems.Count - 1;
                    break;
            }

            //base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int y = 22;

            if (e.Y > Bounds.Y + y)
            {
                HotItem = null;
                return;
            }

            int x = 0;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (e.X > Bounds.X + x && e.X <= (Bounds.X + x) + FilteredItems[i].TabWidth)
                {
                    if (HotIndex != i) HotIndex = i;
                    return;
                }

                x += FilteredItems[i].TabWidth;
            }

            //base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int y = 22;

            if (e.Y > Bounds.Y + y) return;

            int x = 0;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (e.X > Bounds.X + x && e.X <= (Bounds.X + x) + FilteredItems[i].TabWidth)
                {
                    if (SelectedIndex != i) SelectedIndex = i;
                    return;
                }

                x += FilteredItems[i].TabWidth;
            }

            //base.OnMouseDown(e);
        }

        protected override void OnSelectedItemChanged(EventArgs e)
        {
            p_LeftButton.Enabled = SelectedIndex > 0;
            p_RightButton.Enabled = SelectedIndex < FilteredItems.Count - 1;
            p_LeftButton.SetBounds((Bounds.X + GetVisibleItemsWidth()) + 3, Bounds.Y, 8, 18);
            p_RightButton.SetBounds((Bounds.X + GetVisibleItemsWidth()) + 12, Bounds.Y, 8, 18);

            base.OnSelectedItemChanged(e);
        }
        #endregion

        #region Child Events
        private void p_LeftButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedIndex--;
        }

        private void p_RightButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedIndex++;
        }
        #endregion

        #region Private Methods
        private int GetVisibleItemsWidth()
        {
            int x = 0;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (x > (Bounds.Width - FilteredItems[i].TabWidth) - 100) break;

                x += FilteredItems[i].TabWidth;
            }

            return x;
        }
        #endregion
    }
    #endregion

    #region TextBox
    public class TextBox : Control
    {
        #region Events
        public event EventHandler MaskChanged;
        public event EventHandler PasswordChanged;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectedTextChanged;
        #endregion

        #region Objects
        private string p_Mask;
        private bool p_Password, p_ReadOnly;
        private int p_SelectionStart, p_SelectionLength;
        private Menu p_ContextMenu;
        #endregion

        #region Properties
        public string Mask
        {
            get { return p_Mask; }
            set
            {
                p_Mask = value;
                OnMaskChanged(new EventArgs());
                //ReDraw();
            }
        }

        public bool Password
        {
            get { return p_Password; }
            set
            {
                p_Password = value;
                OnPasswordChanged(new EventArgs());
                //ReDraw();
            }
        }

        public bool ReadOnly
        {
            get { return p_ReadOnly; }
            set
            {
                p_ReadOnly = value;
                OnReadOnlyChanged(new EventArgs());
                //ReDraw();
            }
        }

        public int SelectionStart
        {
            get { return p_SelectionStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > Text.Length) value = Text.Length;

                p_SelectionStart = value;
                OnSelectionChanged(new EventArgs());
                ReDraw();
            }
        }

        public int SelectionLength
        {
            get { return p_SelectionLength; }
            set
            {
                if (value < 0) value = 0;
                if (value > Text.Length - p_SelectionStart) value = Text.Length - p_SelectionStart;

                p_SelectionLength = value;
                OnSelectionChanged(new EventArgs());
                ReDraw();
            }
        }

        public string SelectedText
        {
            get
            {
                if (p_SelectionLength == 0) return string.Empty;
                return Text.Substring(p_SelectionStart, p_SelectionLength);
            }
            set
            {
                Text = LeftOfSelectedText() + value + RightOfSelectedText();
                OnSelectedTextChanged(new EventArgs());
            }
        }

        public Menu ContextMenu
        {
            get { return p_ContextMenu; }
        }
        #endregion

        #region Constructors/Destructor
        public TextBox(Window owner, Control parent, string text) : base(owner, parent)
        {
            Cursor = Cursors.IBeam;
            Text = text;

            p_Mask = string.Empty;
            p_Password = false;
            p_ReadOnly = false;
            p_SelectionStart = 0;
            p_SelectionLength = 0;

            p_ContextMenu = new Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(string.Empty, "Cut"), new MenuItem(string.Empty, "Copy"), new MenuItem(string.Empty, "Paste"), new MenuItem(string.Empty, "-"), new MenuItem(string.Empty, "Select All") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public TextBox(Window owner, Control parent, string text, string mask) : base(owner, parent)
        {
            Cursor = Cursors.IBeam;
            Text = text;

            p_Mask = mask;
            p_Password = false;
            p_ReadOnly = false;
            p_SelectionStart = 0;
            p_SelectionLength = 0;

            p_ContextMenu = new Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(string.Empty, "Cut"), new MenuItem(string.Empty, "Copy"), new MenuItem(string.Empty, "Paste"), new MenuItem(string.Empty, "-"), new MenuItem(string.Empty, "Select All") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public TextBox(Window owner, Control parent, string text, string mask, bool password) : base(owner, parent)
        {
            Cursor = Cursors.IBeam;
            Text = text;

            p_Mask = mask;
            p_Password = password;
            p_ReadOnly = false;
            p_SelectionStart = 0;
            p_SelectionLength = 0;

            p_ContextMenu = new Menu(null);
            p_ContextMenu.AddRange(new MenuItem[] { new MenuItem(string.Empty, "Cut"), new MenuItem(string.Empty, "Copy"), new MenuItem(string.Empty, "Paste"), new MenuItem(string.Empty, "-"), new MenuItem(string.Empty, "Select All") });
            p_ContextMenu.ItemClicked += new EventHandler(p_ContextMenu_ItemClicked);
        }

        public override void Dispose()
        {
            p_ContextMenu.Dispose();
            p_SelectionLength = 0;
            p_SelectionStart = 0;
            p_ReadOnly = false;
            p_Password = false;
            p_Mask = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override void OnDraw(Graphics g, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            RectangleF rf = new RectangleF(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.Character;

            Color textColor = Global.Skin.TextBox.TextColor;

            if (Enabled)
            {
                if (Focused) textColor = Global.Skin.TextBox.Down_TextColor;
                else if (IsMouseMoving) textColor = Global.Skin.TextBox.Over_TextColor;
            }
            else textColor = Color.FromArgb(75, Global.Skin.TextBox.TextColor);

            if (DrawBackground) Global.DrawRoundedRectangle(g, r, new Pen(Global.Skin.TextBox.BorderColor));

            if (Focused)
            {
                if (string.IsNullOrEmpty(Text))
                {
                    if (!p_ReadOnly) g.DrawLine(new Pen(Global.Skin.TextBox.Down_TextColor), bounds.X + 2, bounds.Y + 2, bounds.X + 2, (bounds.Y + bounds.Height) - 3);
                }
                else
                {
                    CharacterRange[] ranges = { new CharacterRange(0, LeftOfSelectedText().Length), new CharacterRange(LeftOfSelectedText().Length, p_SelectionLength) };

                    sf.SetMeasurableCharacterRanges(ranges);

                    Region[] regions = g.MeasureCharacterRanges(Text, Skin.WindowFont, rf, sf);

                    if (p_SelectionLength > 0)
                    {
                        r = Rectangle.Round(regions[1].GetBounds(g));
                        g.FillRectangle(new SolidBrush(Global.Skin.TextBox.Down_BGColor), r);
                    }
                    else r = Rectangle.Round(regions[0].GetBounds(g));

                    if (r.Left + r.Width == 0) r = new Rectangle(bounds.X + 2, bounds.Y + 2, 0, 0); //?

                    if (!p_ReadOnly) g.DrawLine(new Pen(Global.Skin.TextBox.Down_TextColor), r.Left + r.Width, bounds.Y + 2, r.Left + r.Width, (bounds.Y + bounds.Height) - 3);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (p_Password)
                {
                    string result = string.Empty;

                    for (int i = 0; i < Text.Length; i++)
                    {
                        result += "*";
                    }

                    g.DrawString(result, Skin.WindowFont, new SolidBrush(textColor), rf, sf);
                }
                else g.DrawString(Text, Skin.WindowFont, new SolidBrush(textColor), rf, sf);
            }
            else if (!Focused) g.DrawString(p_Mask, Skin.WindowFont, new SolidBrush(textColor), rf, sf);

            base.OnDraw(g, bounds);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            if (Focused) SelectionStart = Text.Length;

            base.OnFocusedChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (p_ReadOnly) return;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    //if (e.Shift)
                    //{
                    //SelectionLength--;
                    //}
                    //else
                    //{
                    SelectionStart--;
                    SelectionLength = 0;
                    //}
                    break;
                case Keys.Right:
                    //if (e.Shift)
                    //{
                    //SelectionLength++;
                    //}
                    //else
                    //{
                    SelectionStart++;
                    SelectionLength = 0;
                    //}
                    break;
                case Keys.X:
                    if (e.Control)
                    {
                        Clipboard.SetText(SelectedText);
                        SelectedText = string.Empty;
                    }
                    break;
                case Keys.C:
                    if (e.Control)
                    {
                        Clipboard.SetText(SelectedText);
                    }
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        SelectedText = Clipboard.GetText();
                    }
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (p_ReadOnly) return;

            string s = string.Empty;

            switch (e.KeyChar)
            {
                case (char)Keys.Back:
                    if (p_SelectionStart > 0)
                    {
                        if (p_SelectionStart < Text.Length) s = Text.Substring(p_SelectionStart);

                        Text = Text.Substring(0, p_SelectionStart - 1) + s;
                        p_SelectionStart--;
                        SelectionLength = 0;
                    }
                    break;
                default:
                    if (p_SelectionStart < Text.Length) s = Text.Substring(p_SelectionStart);

                    Text = Text.Substring(0, p_SelectionStart) + e.KeyChar + s;
                    p_SelectionStart++;
                    SelectionLength = 0;
                    break;
            }

            base.OnKeyPress(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (p_ReadOnly) return;

            if (e.Button == MouseButtons.Right) p_ContextMenu.Show();

            base.OnMouseUp(e);
        }
        #endregion

        #region Child Events
        private void p_ContextMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_ContextMenu.SelectedItem.Text)
            {
                case "Cut":
                    Clipboard.SetText(SelectedText);
                    SelectedText = string.Empty;
                    break;
                case "Copy":
                    Clipboard.SetText(SelectedText);
                    break;
                case "Paste":
                    SelectedText = Clipboard.GetText();
                    break;
                case "Select All":
                    SelectionStart = 0;
                    SelectionLength = Text.Length;
                    break;
            }
        }
        #endregion

        #region Private Methods
        private string LeftOfSelectedText()
        {
            try
            {
                return Text.Substring(0, p_SelectionStart);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string RightOfSelectedText()
        {
            try
            {
                return Text.Substring(p_SelectionStart + p_SelectionLength);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnMaskChanged(EventArgs e)
        {
            if (MaskChanged != null) MaskChanged.Invoke(this, e);
        }

        protected virtual void OnPasswordChanged(EventArgs e)
        {
            if (PasswordChanged != null) PasswordChanged.Invoke(this, e);
        }

        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            if (ReadOnlyChanged != null) ReadOnlyChanged.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null) SelectionChanged.Invoke(this, e);
        }

        protected virtual void OnSelectedTextChanged(EventArgs e)
        {
            if (SelectedTextChanged != null) SelectedTextChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region TextBoxGroup
    public class TextBoxGroup : ControlGroup
    {
        #region Events
        public event EventHandler TextChanged;
        public event EventHandler DefaultButtonClicked;
        public event EventHandler ShowDefaultButtonChanged;
        #endregion

        #region Objects
        private Label p_Label;
        private TextBox p_TextBox;
        private Button p_DefaultButton;
        private bool p_ShowDefaultButton;
        #endregion

        #region Properties
        public Label Label
        {
            get { return p_Label; }
        }

        public TextBox TextBox
        {
            get { return p_TextBox; }
        }

        public Button DefaultButton
        {
            get { return p_DefaultButton; }
        }

        public bool ShowDefaultButton
        {
            get { return p_ShowDefaultButton; }
            set
            {
                p_ShowDefaultButton = value;

                OnShowDefaultButtonChanged(new EventArgs());
            }
        }

        public override Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;

                if (p_ShowDefaultButton)
                {
                    p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                    p_TextBox.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 146, base.Bounds.Height);
                    p_DefaultButton.SetBounds(base.Bounds.X + (base.Bounds.Width - 20), base.Bounds.Y, 20, base.Bounds.Height);
                }
                else
                {
                    p_Label.SetBounds(base.Bounds.X, base.Bounds.Y, 114, base.Bounds.Height);
                    p_TextBox.SetBounds(base.Bounds.X + 120, base.Bounds.Y, base.Bounds.Width - 120, base.Bounds.Height);
                    p_DefaultButton.SetBounds(0, 0, 0, 0);
                }

                OnBoundsChanged(new EventArgs());
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                p_Label.Enabled = base.Enabled;
                p_TextBox.Enabled = base.Enabled;
                p_DefaultButton.Enabled = base.Enabled;

                OnEnabledChanged(new EventArgs());
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                p_Label.Visible = base.Visible;
                p_TextBox.Visible = base.Visible;
                p_DefaultButton.Visible = base.Visible;

                OnVisibleChanged(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public TextBoxGroup(Window owner, Control parent, string text, string value) : base()
        {
            p_Label = new Label(owner, parent, text);

            p_TextBox = new TextBox(owner, parent, value);
            p_TextBox.TextChanged += new EventHandler(p_TextBox_TextChanged);

            p_DefaultButton = new Button(owner, parent, "s", string.Empty, "Restore Default");
            p_DefaultButton.MouseClick += new MouseEventHandler(p_DefaultButton_MouseClick);

            p_ShowDefaultButton = true;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Child Events
        private void p_TextBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(new EventArgs());
        }

        private void p_DefaultButton_MouseClick(object sender, EventArgs e)
        {
            OnDefaultButtonClicked(new EventArgs());
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null) TextChanged.Invoke(this, e);
        }

        protected virtual void OnDefaultButtonClicked(EventArgs e)
        {
            if (DefaultButtonClicked != null) DefaultButtonClicked.Invoke(this, e);
        }

        protected virtual void OnShowDefaultButtonChanged(EventArgs e)
        {
            if (ShowDefaultButtonChanged != null) ShowDefaultButtonChanged.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region ToolTip
    public class ToolTip : Popup
    {
        #region Objects
        private string p_Message;
        private int p_Delay;
        private Timer p_Timer;
        #endregion

        #region Properties
        public string Message
        {
            get { return p_Message; }
            set
            {
                p_Message = value;

                Rectangle r = Global.MeasureString(CreateGraphics(), p_Message, Skin.WindowFont, new RectangleF(0, 0, 1000, 18));

                int offset = 4;

                for (int i = 0; i < Width; i += 25)
                {
                    offset += 2;
                }

                Size = new Size(r.Width + offset, 18);

                ReDraw();
            }
        }

        public int Delay
        {
            get { return p_Delay; }
            set
            {
                p_Delay = value;
                p_Timer.Interval = p_Delay;
            }
        }
        #endregion

        #region Constructor/Destuctor
        public ToolTip() : base()
        {
            p_Message = string.Empty;
            p_Delay = 0;

            p_Timer = new Timer();
            p_Timer.Tick += new EventHandler(p_Timer_Tick);
        }

        ~ToolTip()
        {
            p_Timer.Dispose();
            p_Delay = 0;
            p_Message = string.Empty;
        }
        #endregion

        #region Overrides
        protected override void OnDraw(Graphics g)
        {
            g.DrawString(p_Message, Skin.WindowFont, new SolidBrush(Global.Skin.Popup.TextColor), 0, 2);

            base.OnDraw(g);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Hide();

            base.OnMouseDown(e);
        }
        #endregion

        #region Child Events
        private void p_Timer_Tick(object sender, EventArgs e)
        {
            p_Timer.Stop();

            Location = new Point(Cursor.Position.X, Cursor.Position.Y + 18);

            if (Left > Screen.PrimaryScreen.Bounds.Width - Width) Left = Cursor.Position.X - Width;
            if (Top > Screen.PrimaryScreen.Bounds.Height - Height) Top = Cursor.Position.Y - Height;

            if (Handle.Equals(IntPtr.Zero)) base.CreateControl();
            Global.SetParent(base.Handle, IntPtr.Zero);
            Global.ShowWindow(base.Handle, 1);
        }
        #endregion

        #region Public Methods
        public void Show(string message, int delay)
        {
            Message = message;
            Delay = delay;

            p_Timer.Start();
        }

        public new void Hide()
        {
            p_Timer.Stop();

            if (!base.Handle.Equals(IntPtr.Zero)) Global.ShowWindow(base.Handle, 0);
        }
        #endregion
    }
    #endregion

    #region Tree
    public class Tree : GenericList<TreeItem>
    {
        #region Constructor/Destructor
        public Tree(Window owner, Control parent) : base(owner, parent)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            int y = 0;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            for (int i = FirstIndex; i < FilteredItems.Count; i++)
            {
                if (y > (bounds.Height - ItemHeight) - 4) break;

                Color textColor = Global.Skin.List.TextColor;

                if (FilteredItems[i].Equals(SelectedItem))
                {
                    textColor = Global.Skin.List.Down_TextColor;

                    Rectangle r = new Rectangle(bounds.X + 3, (bounds.Y + y) + 3, bounds.Width - 22, ItemHeight);
                    LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.List.Down_BGColor, Color.Transparent, LinearGradientMode.Horizontal);
                    b.WrapMode = WrapMode.TileFlipX;

                    g.FillRectangle(b, r);
                }

                if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                g.DrawString(FilteredItems[i].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 16, (bounds.Y + y) + 3, bounds.Width - 38, ItemHeight), sf);

                y += ItemHeight;

                if (FilteredItems[i].Children.Count > 0)
                {
                    if (FilteredItems[i].Expanded)
                    {
                        //g.DrawString("-", Global.Skin.TextFont, new SolidBrush(textColor), bounds.X + 6, (bounds.Y + y) + 5);

                        for (int i2 = 0; i2 < FilteredItems[i].Children.Count; i2++)
                        {
                            if (y > (bounds.Height - ItemHeight) - 4) break;

                            g.DrawString(FilteredItems[i].Children[i2].Text, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 32, (bounds.Y + y) + 3, bounds.Width - 22, ItemHeight), sf);

                            y += ItemHeight;
                        }
                    }
                    else
                    {
                        //g.DrawString("+", Global.Skin.TextFont, new SolidBrush(textColor), bounds.X + 4, (bounds.Y + y) + 5);
                    }
                }
            }
        }
        #endregion
    }
    #endregion

    #region TreeItem
    public class TreeItem : GenericListItem
    {
        #region Objects
        private List<TreeItem> p_Children;
        private bool p_Expanded;
        #endregion

        #region Properties
        public List<TreeItem> Children
        {
            get { return p_Children; }
        }

        public bool Expanded
        {
            get { return p_Expanded; }
            set { p_Expanded = value; }
        }
        #endregion

        #region Constructors/Destructor
        public TreeItem(string text) : base()
        {
            Text = text;

            p_Children = new List<TreeItem>();
            p_Expanded = false;
        }

        public TreeItem(string text, object tag) : base()
        {
            Text = text;
            Tag = tag;

            p_Children = new List<TreeItem>();
            p_Expanded = false;
        }

        public override void Dispose()
        {
            p_Expanded = false;

            foreach (TreeItem item in p_Children)
            {
                item.Dispose();
            }
            p_Children.Clear();

            base.Dispose();
        }
        #endregion
    }
    #endregion

    #region UpdateDialog
    public class UpdateDialog : Dialog
    {
        #region Objects
        private Label p_StatusLabel, p_VersionLabel, p_PriorityLabel;
        private UpdateList p_UpdateList;
        private Label p_ProgressLabel;
        private Button p_UpdateButton, p_ReleaseNotesButton, p_CancelButton;
        #endregion

        #region Properties
        public UpdateList UpdateList
        {
            get { return p_UpdateList; }
        }
        #endregion

        #region Constructor/Destructor
        public UpdateDialog(XmlNode node) : base()
        {
            MinimumSize = new Size(360, 170);
            Size = new Size(360, 300);
            Text = "STEAMp3 - Live Update";

            p_StatusLabel = new Label(this, null, "A new version of STEAMp3 is available!" + Environment.NewLine + "Click Update Now to begin. STEAMp3 will exit when the download is complete.");

            p_VersionLabel = new Label(this, null, "Current version: " + Application.ProductVersion + Environment.NewLine + "New version: " + node.Attributes["Version"].Value);

            p_PriorityLabel = new Label(this, null, "Released: " + node.Attributes["Committed"].Value + Environment.NewLine + "Priority: " + node.Attributes["Priority"].Value);
            p_PriorityLabel.RightToLeft = true;

            p_UpdateList = new UpdateList(this, null);

            long size = 0;

            XmlNodeList nodeList = node.SelectNodes("File");
            foreach (XmlNode node2 in nodeList)
            {
                UpdateListItem item = new UpdateListItem(node2);
                item.ProgressChanged += new EventHandler(UpdateListItem_ProgressChanged);
                item.DownloadComplete += new EventHandler(UpdateListItem_DownloadComplete);

                size += item.Size;

                p_UpdateList.Add(item);
            }

            p_ProgressLabel = new Label(this, null, p_UpdateList.FilteredItems.Count.ToString() + " File(s) to download, " + Global.ConvertBytes(size) + " total");

            p_UpdateButton = new Button(this, null, "Update Now");
            p_UpdateButton.MouseClick += new MouseEventHandler(p_UpdateButton_MouseClick);

            p_ReleaseNotesButton = new Button(this, null, "Release Notes...");
            p_ReleaseNotesButton.MouseClick += new MouseEventHandler(p_ReleaseNotesButton_MouseClick);

            p_CancelButton = new Button(this, null, "Cancel");
            p_CancelButton.MouseClick += new MouseEventHandler(p_CancelButton_MouseClick);
        }

        ~UpdateDialog()
        {

        }
        #endregion

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            if (!Visible) return;

            int x = (Width - 30) / 2;

            p_StatusLabel.SetBounds(12, 30, Width - 24, 40);
            p_VersionLabel.SetBounds(12, 76, x, 30);
            p_PriorityLabel.SetBounds(x + 18, 76, x, 30);
            p_UpdateList.SetBounds(12, 112, Width - 24, Height - 174);
            p_ProgressLabel.SetBounds(12, Height - 58, Width - 24, 20);

            x = (Width - 36) / 3;

            p_UpdateButton.SetBounds(12, Height - 32, x, 20);
            p_ReleaseNotesButton.SetBounds(x + 18, Height - 32, x, 20);
            p_CancelButton.SetBounds((x * 2) + 24, Height - 32, x, 20);

            base.OnResize(e);
        }
        #endregion

        #region Child Events
        private void UpdateListItem_ProgressChanged(object sender, EventArgs e)
        {
            //UpdateListItem item = sender as UpdateListItem;
            int progress = 0;

            foreach (UpdateListItem item in p_UpdateList.FilteredItems)
            {
                progress += Global.GetPercent(item.BytesRead, item.Size, 100 / p_UpdateList.FilteredItems.Count);
            }

            p_ProgressLabel.Text = "Downloading " + p_UpdateList.FilteredItems.Count.ToString() + " file(s) (" + progress.ToString() + "%)...";
        }

        private void UpdateListItem_DownloadComplete(object sender, EventArgs e)
        {
            //UpdateListItem item = sender as UpdateListItem;
            int downloaded = 0;

            foreach (UpdateListItem item in p_UpdateList.FilteredItems)
            {
                if (item.Downloaded) downloaded++;
            }

            if (downloaded == p_UpdateList.FilteredItems.Count)
            {
                p_StatusLabel.Text = "STEAMp3 has been updated successfully!" + Environment.NewLine + "Click Finish to complete the installation and reload STEAMp3.";
                p_ProgressLabel.Text = p_UpdateList.FilteredItems.Count.ToString() + " File(s) downloaded successfully";
                p_CancelButton.Text = "Finish";
                p_CancelButton.Enabled = true;
            }

            //ReDrawControl(p_UpdateList);
        }

        private void p_UpdateButton_MouseClick(object sender, MouseEventArgs e)
        {
            ControlBox = false;

            p_StatusLabel.Text = "Downloading updates, please wait..." + Environment.NewLine + "Do not exit STEAMp3 during the installation.";
            p_UpdateButton.Enabled = false;
            p_CancelButton.Enabled = false;

            for (int i = 0; i < p_UpdateList.FilteredItems.Count; i++)
            {
                if (!p_UpdateList.FilteredItems[i].Download())
                {
                    ControlBox = true;

                    p_StatusLabel.Text = "An error occured while downloading updates, please check your internet connection.";
                    p_ProgressLabel.Text = "Unable to install updates";
                    p_CancelButton.Enabled = true;

                    return;
                }
            }
        }

        private void p_ReleaseNotesButton_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("http://steamp3.ta0soft.com/liveupdate/");
        }

        private void p_CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (p_CancelButton.Text == "Cancel") DialogResult = DialogResult.Cancel;
            else DialogResult = DialogResult.OK;
        }
        #endregion
    }
    #endregion

    #region UpdateList
    public class UpdateList : GenericList<UpdateListItem>
    {
        #region Constructor/Destructor
        public UpdateList(Window owner, Control parent) : base(owner, parent)
        {
            ItemHeight = 36;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Overrides
        protected override void DrawItems(Graphics g, Rectangle bounds)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            int x = (Bounds.Width - ScrollBar.Bounds.Width) - 8;
            int y = 0;

            if (Items.Count == 0) g.DrawString("No updates found, please try again later.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else if (FilteredItems.Count == 0) g.DrawString("No updates found.", Skin.WindowFont, new SolidBrush(Global.Skin.List.TextColor), new RectangleF(bounds.X + 3, bounds.Y + (bounds.Height / 2) - (ItemHeight / 2), x, ItemHeight), sf);
            else
            {
                for (int i = FirstIndex; i < FilteredItems.Count; i++)
                {
                    if (y > (bounds.Height - ItemHeight) - 4) break;

                    Color textColor = Global.Skin.List.TextColor;

                    if (FilteredItems[i].Downloaded) textColor = Global.Skin.List.Play_TextColor;
                    //if (FilteredItems[i].Equals(HotItem)) textColor = Global.Skin.List.Over_TextColor;

                    sf.Alignment = StringAlignment.Near;

                    g.DrawString(FilteredItems[i].Description, Skin.WindowFont, new SolidBrush(textColor), new RectangleF(bounds.X + 3, (bounds.Y + y) + 3, x - 110, ItemHeight / 2), sf);
                    g.DrawString(FilteredItems[i].URL, Skin.WindowFont, new SolidBrush(Color.FromArgb(100, Global.Skin.List.TextColor)), new RectangleF(bounds.X + 3, ((bounds.Y + y) + 3) + (ItemHeight / 2), x - 110, ItemHeight / 2), sf);

                    sf.Alignment = StringAlignment.Far;

                    if (FilteredItems[i].BytesRead > 0)
                    {
                        if (FilteredItems[i].Downloaded) g.DrawString("Installed", Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 120, (bounds.Y + y) + 3, 120, ItemHeight / 2), sf);
                        else g.DrawString(Global.ConvertBytes(FilteredItems[i].BytesRead) + " / " + Global.ConvertBytes(FilteredItems[i].Size), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 120, (bounds.Y + y) + 3, 120, ItemHeight / 2), sf);

                        int percent = Global.GetPercent(FilteredItems[i].BytesRead, FilteredItems[i].Size, 100);
                        if (percent == 0) percent = 1;

                        Rectangle r = new Rectangle((bounds.X + x) - 100, ((bounds.Y + y) + 3) + (ItemHeight / 2), percent, 16);
                        Global.FillRoundedRectangle(g, r, new LinearGradientBrush(r, Global.Skin.List.Play_BGColor, Global.Skin.List.Play_TextColor, LinearGradientMode.Horizontal));
                    }
                    else g.DrawString(Global.ConvertBytes(FilteredItems[i].Size), Skin.WindowFont, new SolidBrush(textColor), new RectangleF((bounds.X + x) - 120, (bounds.Y + y) + 3, 120, ItemHeight / 2), sf);

                    Global.DrawRoundedRectangle(g, new Rectangle((bounds.X + x) - 100, ((bounds.Y + y) + 3) + (ItemHeight / 2), 100, 16), new Pen(Global.Skin.Slider.BorderColor));

                    y += ItemHeight;
                }
            }
        }
        #endregion
    }
    #endregion

    #region UpdateListItem
    public class UpdateListItem : GenericListItem
    {
        #region Events
        public event EventHandler ProgressChanged;
        public event EventHandler DownloadComplete;
        #endregion

        #region Objects
        private string p_URL, p_Description;
        private long p_Size, p_BytesRead;
        private bool p_Backup;
        private bool p_Downloaded;
        private WebClient p_WebClient;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public string Description
        {
            get { return p_Description; }
        }

        public long Size
        {
            get { return p_Size; }
        }

        public long BytesRead
        {
            get { return p_BytesRead; }
            set
            {
                p_BytesRead = value;
                OnProgressChanged(new EventArgs());
            }
        }

        public bool Backup
        {
            get { return p_Backup; }
        }

        public bool Downloaded
        {
            get { return p_Downloaded; }
            set
            {
                p_Downloaded = value;
                OnDownloadComplete(new EventArgs());
            }
        }
        #endregion

        #region Constructor/Destructor
        public UpdateListItem(XmlNode node) : base()
        {
            p_URL = node.Attributes["URL"].Value;
            p_Description = node.Attributes["Desc"].Value;
            p_Size = Global.GetHttpFileSize("http://steamp3.ta0soft.com/liveupdate/" + p_URL);
            p_BytesRead = 0;
            p_Backup = Global.StringToBool(node.Attributes["Backup"].Value, "1");
            p_Downloaded = false;

            p_WebClient = new WebClient();
            p_WebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(p_WebClient_DownloadFileCompleted);
            p_WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(p_WebClient_DownloadProgressChanged);
        }

        public override void Dispose()
        {
            p_WebClient.Dispose();
            p_Downloaded = false;
            p_Backup = false;
            p_BytesRead = 0;
            p_Size = 0;
            p_Description = string.Empty;
            p_URL = string.Empty;

            base.Dispose();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return p_URL;
        }
        #endregion

        #region Child Events
        private void p_WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null) Downloaded = true;
        }

        private void p_WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            BytesRead = e.BytesReceived;
        }
        #endregion

        #region Public Methods
        public bool Download()
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return false;

                string filename = p_URL;
                if (p_Backup) filename += ".bak";

                p_WebClient.DownloadFileAsync(new Uri("http://ta0soft.com/steamp3/liveupdate/" + p_URL), Application.StartupPath + "\\" + filename);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnProgressChanged(EventArgs e)
        {
            if (ProgressChanged != null) ProgressChanged.Invoke(this, e);
        }

        protected virtual void OnDownloadComplete(EventArgs e)
        {
            if (DownloadComplete != null) DownloadComplete.Invoke(this, e);
        }
        #endregion
    }
    #endregion

    #region Window
    public class Window : Form
    {
        #region Delegates
        public delegate void DrawEventHandler(Graphics g);
        #endregion

        #region Events
        public event DrawEventHandler Draw;
        public event EventHandler HelpClicked;
        #endregion

        #region Objects
        private List<Control> p_Controls;
        private SystemButton p_HelpButton, p_MinButton, p_MaxButton, p_CloseButton;
        private Gripper p_Gripper;

        private Point p_HitPoint;
        private bool p_DrawText, p_Popup, p_Movable, p_Sizable, p_IsMouseDown, p_IsMouseMoving, p_HelpBox;
        #endregion

        #region Properties
        public bool HelpBox
        {
            get { return p_HelpBox; }
            set
            {
                p_HelpBox = value;
                p_HelpButton.Visible = p_HelpBox;
                //ReDraw();
            }
        }

        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
            set
            {
                base.MinimizeBox = value;
                p_MinButton.Visible = base.MinimizeBox;
                //ReDraw();
            }
        }

        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
            set
            {
                base.MaximizeBox = value;
                p_MaxButton.Visible = base.MaximizeBox;
                //ReDraw();
            }
        }

        public new bool ControlBox
        {
            get { return base.ControlBox; }
            set
            {
                base.ControlBox = value;
                p_CloseButton.Visible = base.ControlBox;
                //ReDraw();
            }
        }

        public new List<Control> Controls
        {
            get { return p_Controls; }
        }

        public Point HitPoint
        {
            get { return p_HitPoint; }
        }

        public bool DrawText
        {
            get { return p_DrawText; }
            set
            {
                p_DrawText = value;
                //ReDraw();
            }
        }

        public bool Popup
        {
            get { return p_Popup; }
            set { p_Popup = value; }
        }

        public bool Movable
        {
            get { return p_Movable; }
            set { p_Movable = value; }
        }

        public bool Sizable
        {
            get { return p_Sizable; }
            set
            {
                p_Sizable = value;
                p_Gripper.Visible = p_Sizable;
            }
        }

        public bool IsMouseDown
        {
            get { return p_IsMouseDown; }
        }

        public bool IsMouseMoving
        {
            get { return p_IsMouseMoving; }
        }
        #endregion

        #region Constructor/Destructor
        public Window() : base()
        {
            Icon = Properties.Resources.Icon;
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(252, 150);
            ResizeRedraw = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "STEAMp3";

            p_Controls = new List<Control>();

            p_HelpButton = new SystemButton(this, null, "s", "Help...");
            p_HelpButton.MouseClick += new MouseEventHandler(p_HelpButton_MouseClick);
            p_HelpButton.Visible = false;

            p_MinButton = new SystemButton(this, null, "0", "Minimize");
            p_MinButton.MouseClick += new MouseEventHandler(p_MinButton_MouseClick);

            p_MaxButton = new SystemButton(this, null, "1", "Maximize");
            p_MaxButton.MouseClick += new MouseEventHandler(p_MaxButton_MouseClick);

            p_CloseButton = new SystemButton(this, null, "r", "Close");
            p_CloseButton.MouseClick += new MouseEventHandler(p_CloseButton_MouseClick);

            p_Gripper = new Gripper(this, null);

            p_HitPoint = new Point(0, 0);
            p_DrawText = true;
            p_Popup = false;
            p_Movable = true;
            p_Sizable = true;
            p_IsMouseDown = false;
            p_IsMouseMoving = false;
            p_HelpBox = false;
        }

        ~Window()
        {
            p_HelpBox = false;
            p_IsMouseMoving = false;
            p_IsMouseDown = false;
            p_Sizable = false;
            p_Movable = false;
            p_Popup = false;
            p_DrawText = false;
            p_HitPoint = new Point(0, 0);

            foreach (Control control in p_Controls)
            {
                control.Dispose();
            }
            p_Controls.Clear();
        }
        #endregion

        #region Overrides
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= (int)Global.ClassStyles.DropShadow;
                return cp;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].KeyDownProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].KeyPressProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].MouseDownProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                if (e.Button == MouseButtons.Left)
                {
                    p_HitPoint = new Point(e.X, e.Y);
                    p_IsMouseDown = true;
                }

                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            bool result = false;

            if (!Bounds.Contains(Cursor.Position))
            {
                for (int i = p_Controls.Count - 1; i >= 0; i--)
                {
                    if (p_Controls[i].MouseMoveProc(new MouseEventArgs(MouseButtons.None, -1, -1, -1, -1)))
                    {
                        result = true;
                        break;
                    }
                }
            }

            if (!result)
            {
                p_IsMouseDown = false;
                p_IsMouseMoving = false;

                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].MouseMoveProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                p_IsMouseMoving = true;

                if (p_Movable && p_IsMouseDown)
                {
                    int x = Left + (e.X - p_HitPoint.X);
                    int y = Top + (e.Y - p_HitPoint.Y);

                    if (x > 0 && x < 10) x = 0;
                    if (x > (Screen.PrimaryScreen.WorkingArea.Width - Width) - 10 && x < Screen.PrimaryScreen.WorkingArea.Width - Width) x = Screen.PrimaryScreen.WorkingArea.Width - Width;
                    if (y > 0 && y < 10) y = 0;
                    if (y > (Screen.PrimaryScreen.WorkingArea.Height - Height) - 10 && y < Screen.PrimaryScreen.WorkingArea.Height - Height) y = Screen.PrimaryScreen.WorkingArea.Height - Height;

                    Location = new Point(x, y);
                }

                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].MouseUpProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                p_HitPoint = new Point(0, 0);
                p_IsMouseDown = false;

                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            bool result = false;

            for (int i = p_Controls.Count - 1; i >= 0; i--)
            {
                if (p_Controls[i].MouseWheelProc(e))
                {
                    result = true;
                    break;
                }
            }

            if (!result) base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawProc(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            int x = Width - 20;

            p_CloseButton.SetBounds(x, 5, 14, 14);
            if (MaximizeBox) x -= 18;
            p_MaxButton.SetBounds(x, 5, 14, 14);
            if (MinimizeBox) x -= 18;
            p_MinButton.SetBounds(x, 5, 14, 14);
            if (p_HelpBox) x -= 18;
            p_HelpButton.SetBounds(x, 5, 14, 14);

            //if (MaximizeBox) p_MinButton.SetBounds(Width - 56, 5, 14, 14);
            //else p_MinButton.SetBounds(Width - 38, 5, 14, 14);
            //p_MaxButton.SetBounds(Width - 38, 5, 14, 14);
            //p_CloseButton.SetBounds(Width - 20, 5, 14, 14);
            
            p_Gripper.SetBounds(Width - 18, Height - 18, 18, 18);

            base.OnResize(e);
        }

        protected override void OnShown(EventArgs e)
        {
            Global.SetForegroundWindow(Handle);
            OnResize(e);

            base.OnShown(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)Global.WindowsMessages.WM_ERASEBKGND:
                    m.Msg = 0;
                    break;
                case (int)Global.WindowsMessages.WM_NCHITTEST:
                    if (p_Sizable) m.Result = HitTestNCA(ref m);
                    else base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region Child Events
        private void p_HelpButton_MouseClick(object sender, MouseEventArgs e)
        {
            OnHelpClicked(new EventArgs());
        }
        
        private void p_MinButton_MouseClick(object sender, MouseEventArgs e)
        {
            Minimize();
        }

        private void p_MaxButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Maximize();
            }
            else
            {
                Restore();
            }
        }

        private void p_CloseButton_MouseClick(object sender, MouseEventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        #region Public Methods
        public void Minimize()
        {
            WindowState = FormWindowState.Minimized;
        }

        public void Maximize()
        {
            Sizable = false;
            WindowState = FormWindowState.Maximized;
            p_MaxButton.Icon = "2";
            p_MaxButton.ToolTipText = "Restore";
            p_Gripper.Visible = false;
        }

        public void Restore()
        {
            Sizable = true;
            WindowState = FormWindowState.Normal;
            p_MaxButton.Icon = "1";
            p_MaxButton.ToolTipText = "Maximize";
            p_Gripper.Visible = true;
        }

        public void ReDraw()
        {
            if (!Visible) return;
            if (Width < 6 || Height < 6) return;

            DrawProc(CreateGraphics());
        }

        public void ReDrawControl(Control control)
        {
            if (!Visible) return;
            if (Width < 6 || Height < 6) return;

            DrawControlProc(CreateGraphics(), control);
        }
        #endregion

        #region Private Methods
        private void DrawProc(Graphics g)
        {
            //if (!Visible) return;
            //if (Width < 6 || Height < 6) return;

            Bitmap buffer = new Bitmap(Width, Height);
            Graphics g2 = Graphics.FromImage(buffer);

            g2.SmoothingMode = SmoothingMode.None;
            g2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (p_Popup) DrawPopupBackground(g2, ClientRectangle);
            else DrawBackground(g2, ClientRectangle);

            foreach (Control control in p_Controls)
            {
                if (control.Parent == null && control.GetType() != typeof(Gripper))
                {
                    control.OnPaint(g2, control.Bounds);
                    p_Gripper.OnPaint(g2, ClientRectangle);

                    foreach (Control control2 in control.Controls)
                    {
                        control2.OnPaint(g2, control2.Bounds);

                        foreach (Control control3 in control2.Controls)
                        {
                            control3.OnPaint(g2, control3.Bounds);
                        }
                    }
                }
            }

            OnDraw(g2);

            g.DrawImage(buffer, 0, 0, Width, Height);

            g2.Dispose();
            buffer.Dispose();
        }

        private void DrawControlProc(Graphics g, Control control)
        {
            //if (!Visible) return;
            //if (Width < 6 || Height < 6) return;

            if (!control.Visible) return;
            if (control.Bounds.Width < 6 || control.Bounds.Height < 6) return;

            if (control.Parent == null)
            {
                DrawProc(g);
                return;
            }
            else
            {
                Bitmap buffer = new Bitmap(control.Bounds.Width, control.Bounds.Height);
                Graphics g2 = Graphics.FromImage(buffer);

                g2.SmoothingMode = SmoothingMode.None;
                g2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                Rectangle r = new Rectangle(control.Parent.Bounds.X - control.Bounds.X, control.Parent.Bounds.Y - control.Bounds.Y, control.Parent.Bounds.Width, control.Parent.Bounds.Height);

                control.Parent.OnPaint(g2, r);

                foreach (Control control2 in control.Parent.Controls)
                {
                    r = new Rectangle(control2.Bounds.X - control.Bounds.X, control2.Bounds.Y - control.Bounds.Y, control2.Bounds.Width, control2.Bounds.Height);

                    control2.OnPaint(g2, r);

                    foreach (Control control3 in control2.Controls)
                    {
                        r = new Rectangle(control3.Bounds.X - control.Bounds.X, control3.Bounds.Y - control.Bounds.Y, control3.Bounds.Width, control3.Bounds.Height);

                        control3.OnPaint(g2, r);
                    }
                }

                g.DrawImage(buffer, control.Bounds);

                g2.Dispose();
                buffer.Dispose();
            }
        }

        private void DrawBackground(Graphics g, Rectangle bounds)
        {
            if (!Visible) return;
            if (bounds.Width < 6 || bounds.Height < 6) return;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width, 100);
            Pen p = new Pen(new LinearGradientBrush(r, Global.Skin.Window.LineColor, Global.Skin.Window.BGColor, LinearGradientMode.Vertical));

            g.FillRectangle(new SolidBrush(Global.Skin.Window.BGColor), bounds);

            for (int i = bounds.X - 18; i < r.Width + 66; i += 4)
            {
                g.DrawLine(p, bounds.X, bounds.Y + (20 + i), bounds.X + (20 + i), bounds.Y);
            }

            r = new Rectangle(bounds.X + 1, bounds.Y + 100, bounds.Width - 2, bounds.Height - 100);
            LinearGradientBrush b = new LinearGradientBrush(r, Global.Skin.Window.BGColor, Global.Skin.Window.BGColor2, LinearGradientMode.Vertical);
            b.WrapMode = WrapMode.TileFlipX;

            g.FillRectangle(b, r);

            if (!string.IsNullOrEmpty(Text) && p_DrawText)
            {
                g.DrawString("¯", new Font(Skin.IconFont.Name, 60.0f, FontStyle.Regular), new SolidBrush(Color.FromArgb(30, Global.Skin.Window.TextColor)), (bounds.X + bounds.Width) - 80, bounds.Y - 27);
                if (MaximizeBox) g.DrawString(Text, Skin.WindowFont, new SolidBrush(Global.Skin.Window.TextColor), new RectangleF(bounds.X + 5, bounds.Y + 5, bounds.Width - 60, 20), sf);
                else g.DrawString(Text, Skin.WindowFont, new SolidBrush(Global.Skin.Window.TextColor), new RectangleF(bounds.X + 5, bounds.Y + 5, bounds.Width - 44, 20), sf);
            }

            r = new Rectangle(bounds.X, bounds.Y, 1, bounds.Height);
            g.FillRectangle(new SolidBrush(Global.Skin.Window.BorderColor), r);

            r = new Rectangle(bounds.X, bounds.Y, bounds.Width, 1);
            g.FillRectangle(new SolidBrush(Global.Skin.Window.BorderColor), r);

            r = new Rectangle((bounds.X + bounds.Width) - 1, bounds.Y, 1, bounds.Height);
            g.FillRectangle(new LinearGradientBrush(r, Global.Skin.Window.BorderColor, Global.Skin.Window.BorderColor2, LinearGradientMode.Vertical), r);

            r = new Rectangle(bounds.X, (bounds.Y + bounds.Height) - 1, bounds.Width, 1);
            g.FillRectangle(new LinearGradientBrush(r, Global.Skin.Window.BorderColor, Global.Skin.Window.BorderColor2, LinearGradientMode.Horizontal), r);
        }

        private void DrawPopupBackground(Graphics g, Rectangle bounds)
        {
            if (!Visible) return;
            if (bounds.Width < 6 || bounds.Height < 6) return;

            Rectangle r = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            g.FillRectangle(new SolidBrush(Global.Skin.Popup.BorderColor), bounds);

            g.FillRectangle(new LinearGradientBrush(r, Global.Skin.Popup.BGColor, Global.Skin.Popup.BGColor2, LinearGradientMode.Vertical), r);

            g.DrawString(Text, Skin.WindowFont, new SolidBrush(Global.Skin.Popup.TextColor), new RectangleF(bounds.X + 5, bounds.Y + 5, bounds.Width - 60, bounds.Y + 20), sf);
        }

        private IntPtr HitTestNCA(ref Message m)
        {
            Point ptMouse = new Point(Global.LoWord(m.LParam), Global.HiWord(m.LParam));

            Global.RECT rcWindow = new Global.RECT();
            Global.RECT rcFrame = new Global.RECT();
            ushort row = 1;
            ushort col = 1;
            bool onResizeBorder = false;

            Global.GetWindowRect(m.HWnd, ref rcWindow);
            Global.AdjustWindowRectEx(ref rcFrame, (uint)Global.WindowStyles.WS_OVERLAPPEDWINDOW & ~(uint)Global.WindowStyles.WS_CAPTION, false, 0);

            if (ptMouse.Y >= rcWindow.Top && ptMouse.Y < rcWindow.Top + 6) // top
            {
                onResizeBorder = ptMouse.Y < (rcWindow.Top - rcFrame.Top);
                row = 0;
            }
            else if (ptMouse.Y < rcWindow.Bottom && ptMouse.Y >= rcWindow.Bottom - 6) row = 2; // bottom

            if (ptMouse.X >= rcWindow.Left && ptMouse.X < rcWindow.Left + 6) col = 0; // left
            else if (ptMouse.X < rcWindow.Right && ptMouse.X >= rcWindow.Right - 6) col = 2; // right

            int[][] hitTests =
            {
                new int[3] { (int)Global.HitTestFlags.HTTOPLEFT, onResizeBorder ? (int)Global.HitTestFlags.HTTOP : (int)Global.HitTestFlags.HTCAPTION, (int)Global.HitTestFlags.HTTOPRIGHT },
                new int[3] { (int)Global.HitTestFlags.HTLEFT, (int)Global.HitTestFlags.HTCLIENT, (int)Global.HitTestFlags.HTRIGHT },
                new int[3] { (int)Global.HitTestFlags.HTBOTTOMLEFT, (int)Global.HitTestFlags.HTBOTTOM, (int)Global.HitTestFlags.HTBOTTOMRIGHT }
            };

            return new IntPtr(hitTests[row][col]);
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnDraw(Graphics g)
        {
            if (Draw != null) Draw.Invoke(g);
        }

        protected virtual void OnHelpClicked(EventArgs e)
        {
            if (HelpClicked != null) HelpClicked.Invoke(this, e);
        }
        #endregion
    }
    #endregion
}