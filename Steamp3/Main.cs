#region Using
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Web;

using HundredMilesSoftware.UltraID3Lib;
using WMPLib;
//using IrrKlang;
#endregion

namespace Steamp3
{
    #region Global
    public static class Global
    {
        #region API
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp); 

        [DllImport("user32.dll")]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DrawTextEx(IntPtr hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat, [In, Out] DRAWTEXTPARAMS lpDTParams);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        #region Enums
        public enum ClassStyles : int
        {
            DropShadow = 0x00020000,
        }

        public enum DisplaySettings : int
        {
            Current = -1,
            Registry = -2,
        }

        public enum FileOperations : int
        {
            FO_DELETE = 3,
        }

        public enum FileOperationFlags : int
        {
            FOF_ALLOWUNDO = 0x40,
            FOF_NOCONFIRMATION = 0x10,
        }

        public enum HitTestFlags : int
        {
            HTNOWHERE = 0,
            HTCLIENT = 0x1,
            HTCAPTION = 0x2,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 0x10,
            HTBOTTOMRIGHT = 17,
        }

        public enum ToolTipStyles : int
        {
            TTS_ALWAYSTIP = 0x01,
            TTS_NOPREFIX = 0x02,
            TTS_BALLOON = 0x40,
        }

        public enum WindowsMessages : int
        {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUERYOPEN = 0x0013,
            WM_ENDSESSION = 0x0016,
            WM_QUIT = 0x0012,
            WM_ERASEBKGND = 0x0014,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_SHOWWINDOW = 0x0018,
            WM_WININICHANGE = 0x001A,
            WM_SETTINGCHANGE = 0x001A,
            WM_DEVMODECHANGE = 0x001B,
            WM_ACTIVATEAPP = 0x001C,
            WM_FONTCHANGE = 0x001D,
            WM_TIMECHANGE = 0x001E,
            WM_CANCELMODE = 0x001F,
            WM_SETCURSOR = 0x0020,
            WM_MOUSEACTIVATE = 0x0021,
            WM_CHILDACTIVATE = 0x0022,
            WM_QUEUESYNC = 0x0023,
            WM_GETMINMAXINFO = 0x0024,
            WM_PAINTICON = 0x0026,
            WM_ICONERASEBKGND = 0x0027,
            WM_NEXTDLGCTL = 0x0028,
            WM_SPOOLERSTATUS = 0x002A,
            WM_DRAWITEM = 0x002B,
            WM_MEASUREITEM = 0x002C,
            WM_DELETEITEM = 0x002D,
            WM_VKEYTOITEM = 0x002E,
            WM_CHARTOITEM = 0x002F,
            WM_SETFONT = 0x0030,
            WM_GETFONT = 0x0031,
            WM_SETHOTKEY = 0x0032,
            WM_GETHOTKEY = 0x0033,
            WM_QUERYDRAGICON = 0x0037,
            WM_COMPAREITEM = 0x0039,
            WM_GETOBJECT = 0x003D,
            WM_COMPACTING = 0x0041,
            WM_COMMNOTIFY = 0x0044,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_POWER = 0x0048,
            WM_COPYDATA = 0x004A,
            WM_CANCELJOURNAL = 0x004B,
            WM_NOTIFY = 0x004E,
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_TCARD = 0x0052,
            WM_HELP = 0x0053,
            WM_USERCHANGED = 0x0054,
            WM_NOTIFYFORMAT = 0x0055,
            WM_CONTEXTMENU = 0x007B,
            WM_STYLECHANGING = 0x007C,
            WM_STYLECHANGED = 0x007D,
            WM_DISPLAYCHANGE = 0x007E,
            WM_GETICON = 0x007F,
            WM_SETICON = 0x0080,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCCALCSIZE = 0x0083,
            WM_NCHITTEST = 0x0084,
            WM_NCPAINT = 0x0085,
            WM_NCACTIVATE = 0x0086,
            WM_GETDLGCODE = 0x0087,
            WM_SYNCPAINT = 0x0088,
            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMBUTTONDBLCLK = 0x00A9,
            WM_NCXBUTTONDOWN = 0x00AB,
            WM_NCXBUTTONUP = 0x00AC,
            WM_NCXBUTTONDBLCLK = 0x00AD,
            WM_INPUT = 0x00FF,
            WM_KEYFIRST = 0x0100,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,
            WM_DEADCHAR = 0x0103,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCHAR = 0x0106,
            WM_SYSDEADCHAR = 0x0107,
            WM_UNICHAR = 0x0109,
            WM_KEYLAST_NT501 = 0x0109,
            UNICODE_NOCHAR = 0xFFFF,
            WM_KEYLAST_PRE501 = 0x0108,
            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_KEYLAST = 0x010F,
            WM_INITDIALOG = 0x0110,
            WM_COMMAND = 0x0111,
            WM_SYSCOMMAND = 0x0112,
            WM_TIMER = 0x0113,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_MENUSELECT = 0x011F,
            WM_MENUCHAR = 0x0120,
            WM_ENTERIDLE = 0x0121,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_MENUCOMMAND = 0x0126,
            WM_CHANGEUISTATE = 0x0127,
            WM_UPDATEUISTATE = 0x0128,
            WM_QUERYUISTATE = 0x0129,
            WM_CTLCOLORMSGBOX = 0x0132,
            WM_CTLCOLOREDIT = 0x0133,
            WM_CTLCOLORLISTBOX = 0x0134,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORDLG = 0x0136,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_CTLCOLORSTATIC = 0x0138,
            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MOUSEWHEEL = 0x020A,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C,
            WM_XBUTTONDBLCLK = 0x020D,
            WM_MOUSELAST_5 = 0x020D,
            WM_MOUSELAST_4 = 0x020A,
            WM_MOUSELAST_PRE_4 = 0x0209,
            WM_PARENTNOTIFY = 0x0210,
            WM_ENTERMENULOOP = 0x0211,
            WM_EXITMENULOOP = 0x0212,
            WM_NEXTMENU = 0x0213,
            WM_SIZING = 0x0214,
            WM_CAPTURECHANGED = 0x0215,
            WM_MOVING = 0x0216,
            WM_POWERBROADCAST = 0x0218,
            WM_DEVICECHANGE = 0x0219,
            WM_MDICREATE = 0x0220,
            WM_MDIDESTROY = 0x0221,
            WM_MDIACTIVATE = 0x0222,
            WM_MDIRESTORE = 0x0223,
            WM_MDINEXT = 0x0224,
            WM_MDIMAXIMIZE = 0x0225,
            WM_MDITILE = 0x0226,
            WM_MDICASCADE = 0x0227,
            WM_MDIICONARRANGE = 0x0228,
            WM_MDIGETACTIVE = 0x0229,
            WM_MDISETMENU = 0x0230,
            WM_ENTERSIZEMOVE = 0x0231,
            WM_EXITSIZEMOVE = 0x0232,
            WM_DROPFILES = 0x0233,
            WM_MDIREFRESHMENU = 0x0234,
            WM_IME_SETCONTEXT = 0x0281,
            WM_IME_NOTIFY = 0x0282,
            WM_IME_CONTROL = 0x0283,
            WM_IME_COMPOSITIONFULL = 0x0284,
            WM_IME_SELECT = 0x0285,
            WM_IME_CHAR = 0x0286,
            WM_IME_REQUEST = 0x0288,
            WM_IME_KEYDOWN = 0x0290,
            WM_IME_KEYUP = 0x0291,
            WM_MOUSEHOVER = 0x02A1,
            WM_MOUSELEAVE = 0x02A3,
            WM_NCMOUSEHOVER = 0x02A0,
            WM_NCMOUSELEAVE = 0x02A2,
            WM_WTSSESSION_CHANGE = 0x02B1,
            WM_TABLET_FIRST = 0x02c0,
            WM_TABLET_LAST = 0x02df,
            WM_CUT = 0x0300,
            WM_COPY = 0x0301,
            WM_PASTE = 0x0302,
            WM_CLEAR = 0x0303,
            WM_UNDO = 0x0304,
            WM_RENDERFORMAT = 0x0305,
            WM_RENDERALLFORMATS = 0x0306,
            WM_DESTROYCLIPBOARD = 0x0307,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_PAINTCLIPBOARD = 0x0309,
            WM_VSCROLLCLIPBOARD = 0x030A,
            WM_SIZECLIPBOARD = 0x030B,
            WM_ASKCBFORMATNAME = 0x030C,
            WM_CHANGECBCHAIN = 0x030D,
            WM_HSCROLLCLIPBOARD = 0x030E,
            WM_QUERYNEWPALETTE = 0x030F,
            WM_PALETTEISCHANGING = 0x0310,
            WM_PALETTECHANGED = 0x0311,
            WM_HOTKEY = 0x0312,
            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,
            WM_APPCOMMAND = 0x0319,
            WM_THEMECHANGED = 0x031A,
            WM_HANDHELDFIRST = 0x0358,
            WM_HANDHELDLAST = 0x035F,
            WM_AFXFIRST = 0x0360,
            WM_AFXLAST = 0x037F,
            WM_PENWINFIRST = 0x0380,
            WM_PENWINLAST = 0x038F,
            WM_APP = 0x8000,
            WM_USER = 0x0400,
        }

        public enum WindowStyles : int
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = unchecked((int)0x80000000),
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,
        }

        public enum WindowStylesEx : int
        {
            WS_EX_NOACTIVATE = 0x08000000,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_TOPMOST = 0x00000008,
        }
        #endregion

        #region Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DRAWTEXTPARAMS
        {
            public uint cbSize;
            public int iTabLength;
            public int iLeftMargin;
            public int iRightMargin;
            public uint uiLengthDrawn;

            public DRAWTEXTPARAMS()
            {
                this.cbSize = (uint)Marshal.SizeOf(typeof(DRAWTEXTPARAMS));
            }
        }

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //public struct MEMORYSTATUS
        //{
            //public uint dwLength;
            //public uint dwMemoryLoad;
            //public ulong dwTotalPhys;
            //public ulong dwAvailPhys;
            //public ulong dwTotalPageFile;
            //public ulong dwAvailPageFile;
            //public ulong dwTotalVirtual;
            //public ulong dwAvailVirtual;
        //}

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r)
            {
                Left = r.X;
                Top = r.Y;
                Right = r.Right;
                Bottom = r.Bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public short fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
        #endregion

        #region Objects
        public static MainWindow MainWindow = null;

        public static UI.Skin Skin = null;
        public static Settings Settings = null;
        public static MediaPlayer MediaPlayer = null;
        public static Steam Steam = null;
        public static Stats Stats = null;
        public static UI.ToolTip ToolTip = null;

        private static int RandomSeed = (Environment.TickCount & 0x7FFFFFFF);
        #endregion

        #region Main
        [STAThread]
        private static void Main()
        {
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.GetUpperBound(0) > 0)
            {
                foreach (Process process in processes)
                {
                    Global.SetForegroundWindow(process.MainWindowHandle);
                    //SendMessage(process.MainWindowHandle, 1337, IntPtr.Zero, IntPtr.Zero);
                }
                
                return;
            }

            MainWindow = new MainWindow();

            Application.Run(MainWindow);
        }
        #endregion

        #region Public Methods
        #region AdjustAlpha
        public static unsafe Bitmap AdjustAlpha(Bitmap b)
        {
            try
            {
                Bitmap result = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
                BitmapData bd = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                IntPtr scan = bd.Scan0;
                int offset = bd.Stride - result.Width * 4;

                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        // BGR format
                        int alpha = Marshal.ReadByte(scan, 3);
                        if (x < 255) alpha = x;

                        Marshal.WriteByte(scan, 3, (byte)alpha);

                        scan = ((IntPtr)(scan.ToInt32() + 4));
                    }
                    scan = ((IntPtr)(scan.ToInt32() + offset));
                }

                result.UnlockBits(bd);
                return result;
            }
            catch
            {
                return b;
            }
        }

        public static unsafe Bitmap AdjustAlpha(Bitmap b, int alpha)
        {
            try
            {
                Bitmap result = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
                BitmapData bd = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                IntPtr scan = bd.Scan0;
                int offset = bd.Stride - result.Width * 4;

                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        // BGR format
                        int alpha2 = Marshal.ReadByte(scan, 3);
                        if (alpha <= 255) alpha2 = alpha;

                        Marshal.WriteByte(scan, 3, (byte)alpha2);

                        scan = ((IntPtr)(scan.ToInt32() + 4));
                    }
                    scan = ((IntPtr)(scan.ToInt32() + offset));
                }

                result.UnlockBits(bd);
                return result;
            }
            catch
            {
                return b;
            }
        }
        #endregion

        #region AdjustColor
        public static unsafe Bitmap AdjustColor(Bitmap b, Color c)
        {
            try
            {
                Bitmap result = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
                BitmapData bd = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                IntPtr scan = bd.Scan0;
                int offset = bd.Stride - result.Width * 4;

                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        // BGR format
                        int blue = Marshal.ReadByte(scan, 0);
                        int green = Marshal.ReadByte(scan, 1);
                        int red = Marshal.ReadByte(scan, 2);

                        red = c.R;
                        green = c.G;
                        blue = c.B;

                        Marshal.WriteByte(scan, 0, (byte)blue);
                        Marshal.WriteByte(scan, 1, (byte)green);
                        Marshal.WriteByte(scan, 2, (byte)red);

                        scan = ((IntPtr)(scan.ToInt32() + 4));
                    }
                    scan = ((IntPtr)(scan.ToInt32() + offset));
                }

                result.UnlockBits(bd);
                return result;
            }
            catch
            {
                return b;
            }
        }
        #endregion

        #region AdjustHue
        public static unsafe Bitmap AdjustHue(Bitmap b, Color c)
        {
            try
            {
                Bitmap result = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppArgb);
                BitmapData bd = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                IntPtr scan = bd.Scan0;
                int offset = bd.Stride - result.Width * 4;

                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        // BGR format
                        int blue = Marshal.ReadByte(scan, 0);
                        int green = Marshal.ReadByte(scan, 1);
                        int red = Marshal.ReadByte(scan, 2);

                        red += c.R;
                        red = Math.Max(red, 0);
                        red = Math.Min(red, 255);

                        green += c.G;
                        green = Math.Max(green, 0);
                        green = Math.Min(green, 255);

                        blue += c.B;
                        blue = Math.Max(blue, 0);
                        blue = Math.Min(blue, 255);

                        Marshal.WriteByte(scan, 0, (byte)blue);
                        Marshal.WriteByte(scan, 1, (byte)green);
                        Marshal.WriteByte(scan, 2, (byte)red);

                        scan = ((IntPtr)(scan.ToInt32() + 4));
                    }
                    scan = ((IntPtr)(scan.ToInt32() + offset));
                }

                result.UnlockBits(bd);
                return result;
            }
            catch
            {
                return b;
            }
        }
        #endregion

        #region BoolToString
        public static string BoolToString(bool b, string falseString, string trueString)
        {
            if (b) return trueString;
            return falseString;
        }
        #endregion

        #region CharToInt
        public static int CharToInt(char c)
        {
            int result = 0;

            if (Int32.TryParse(c.ToString(), out result)) return result;
            return 0;
        }
        #endregion

        #region ColorToHex
        public static string ColorToHex(Color c)
        {
            return c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
        #endregion

        #region ColorToHSV
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }
        #endregion

        #region CommonDenominator
        public static int CommonDenominator(int a, int b)
        {
            return b == 0 ? a : CommonDenominator(b, a % b);
        }
        #endregion

        #region ConvertBytes
        public static string ConvertBytes(long bytes)
        {
            try
            {
                double num = bytes;
                if (num > Math.Pow(1024.0, 4.0))
                {
                    num /= Math.Pow(1024.0, 4.0);
                    return (Math.Round(num, 2).ToString("###,###,##0.00") + " TB");
                }
                if ((num > Math.Pow(1024.0, 3.0)) && (num < Math.Pow(1024.0, 4.0)))
                {
                    num /= Math.Pow(1024.0, 3.0);
                    return (Math.Round(num, 2).ToString("###,###,##0.00") + " GB");
                }
                if ((num > Math.Pow(1024.0, 2.0)) && (num < Math.Pow(1024.0, 3.0)))
                {
                    num /= Math.Pow(1024.0, 2.0);
                    return (Math.Round(num, 2).ToString("###,###,##0.00") + " MB");
                }
                if ((num > 1024.0) && (num < Math.Pow(1024.0, 2.0)))
                {
                    num /= 1024.0;
                    return (Math.Round(num, 2).ToString("###,###,##0.00") + " KB");
                }
                if (num < 1024.0)
                {
                    return (Math.Round(num, 2).ToString("###,###,##0.00") + " Bytes");
                }
            }
            catch
            {
                return "0 Bytes";
            }
            return null;
        }
        #endregion

        #region ConvertIP
        public static string ConvertIP(uint ip)
        {
            return (ip >> 24) + "." + ((ip >> 16) & 0xff) + "." + ((ip >> 8) & 0xff) + "." + (ip & 0xff);
        }
        #endregion

        #region ConvertMilliseconds
        public static string ConvertMilliseconds(int milliseconds, bool ms)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, milliseconds);

            return ConvertTime(ts, ms);
        }
        #endregion

        #region ConvertMinutes
        public static string ConvertMinutes(int minutes, bool ms)
        {
            TimeSpan ts = new TimeSpan(0, 0, minutes, 0);

            return ConvertTime(ts, ms);
        }
        #endregion

        #region ConvertSeconds
        public static string ConvertSeconds(int seconds, bool ms)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, seconds);

            return ConvertTime(ts, ms);
        }
        #endregion

        #region ConvertTicks
        public static string ConvertTicks(long ticks, bool ms)
        {
            TimeSpan ts = new TimeSpan(ticks);

            return ConvertTime(ts, ms);
        }
        #endregion

        #region ConvertTime
        public static string ConvertTime(TimeSpan ts, bool ms)
        {
            if (ts.Days > 0) return string.Format("{0:0#}", ts.Days) + ":" + string.Format("{0:0#}", ts.Hours) + ":" + string.Format("{0:0#}", ts.Minutes) + ":" + string.Format("{0:0#}", ts.Seconds);
            else if (ts.Hours > 0) return string.Format("{0:0#}", ts.Hours) + ":" + string.Format("{0:0#}", ts.Minutes) + ":" + string.Format("{0:0#}", ts.Seconds);
            else if (ts.Minutes > 0) return string.Format("{0:0#}", ts.Minutes) + ":" + string.Format("{0:0#}", ts.Seconds);
            else if (ts.Seconds > 0) return string.Format("{0:0#}", ts.Minutes) + ":" + string.Format("{0:0#}", ts.Seconds);
            else
            {
                if (ms) return "00:00." + ts.Milliseconds.ToString();
                return "00:00";
            }
        }
        #endregion

        #region ConvertUnixDate
        public static string ConvertUnixDate(int date, string defaultValue)
        {
            if (date == 0) return defaultValue;

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoch.AddSeconds(Convert.ToDouble(date)).ToShortDateString();
        }
        #endregion

        #region CurrentLeaks
        public static string CurrentLeaks(int count)
        {
            try
            {
                if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                if (count > 0 && count < 21)
                {
                    string result = "Current {ls}" + count + "{rs} Leaks:" + Environment.NewLine;

                    XmlDocument xml = new XmlDocument();
                    xml.Load("http://feed43.com/leaksallday.xml");
                    XmlNodeList list = xml.SelectNodes("rss/channel/item/title");

                    for (int i = 0; i < count; i++)
                    {
                        result += "{ls}" + (i + 1) + "{rs} " + list[i].InnerText.Replace("[", "{ls}").Replace("]", "{rs}") + Environment.NewLine;
                    }

                    return result.Substring(0, result.Length - 1);
                }
                else return "Invalid integer: {ls}1-20 Only{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region DrawString
        public static void DrawString(IntPtr hDC, string text, Rectangle r, TextFormatFlags flags)
        {
            RECT rect = new RECT(r);

            DRAWTEXTPARAMS dtp = new DRAWTEXTPARAMS();
            dtp.iLeftMargin = 0;
            dtp.iRightMargin = 0;
            dtp.iTabLength = 0;
            dtp.uiLengthDrawn = 0;

            DrawTextEx(hDC, text, text.Length, ref rect, (int)flags, dtp);
        }
        #endregion

        #region DrawRoundedRectangle
        public static void DrawRoundedRectangle(Graphics g, Rectangle r, Pen p)
        {
            GraphicsPath gp = new GraphicsPath();
            int edgeRounding = 4;

            gp.AddArc(r.X, r.Y, edgeRounding, edgeRounding, 180, 90);
            gp.AddArc(r.X + r.Width - edgeRounding, r.Y, edgeRounding, edgeRounding, 270, 90);
            gp.AddArc(r.X + r.Width - edgeRounding, r.Y + r.Height - edgeRounding, edgeRounding, edgeRounding, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - edgeRounding, edgeRounding, edgeRounding, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - edgeRounding, r.X, r.Y + edgeRounding / 2);

            g.DrawPath(p, gp);
            gp.Dispose();
        }
        #endregion

        #region FillRoundedRectangle
        public static void FillRoundedRectangle(Graphics g, Rectangle r, Brush b)
        {
            GraphicsPath gp = new GraphicsPath();
            int edgeRounding = 4;

            gp.AddArc(r.X, r.Y, edgeRounding, edgeRounding, 180, 90);
            gp.AddArc(r.X + r.Width - edgeRounding, r.Y, edgeRounding, edgeRounding, 270, 90);
            gp.AddArc(r.X + r.Width - edgeRounding, r.Y + r.Height - edgeRounding, edgeRounding, edgeRounding, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - edgeRounding, edgeRounding, edgeRounding, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - edgeRounding, r.X, r.Y + edgeRounding / 2);

            g.FillPath(b, gp);
            gp.Dispose();
        }
        #endregion

        #region FormatBitrate
        public static string FormatBitrate(UI.PlaylistItem item)
        {
            string result = item.Bitrate.ToString();
            if (result == "0") result = "N/A";
            else result += " Kbps";
            if (item.VBR) result = "Variable";

            return result;
        }
        #endregion

        #region FormatNumber
        public static string FormatNumber(int i)
        {
            if (i == 0) return "0";

            return string.Format("{0:###,###,###,###}", i);
        }
        #endregion

        #region FormatString
        public static string FormatString(string s, string empty)
        {
            if (string.IsNullOrEmpty(s)) return empty;
            else if (s == "0") return empty;

            return s;
        }
        #endregion

        #region FormatTrack
        public static string FormatTrack(short? s, string empty)
        {
            if (s == null || s == 0) return empty;
            else if (s < 10) return "0" + s.ToString();

            return s.ToString();
        }
        #endregion

        #region GenerateCommands
        public static void GenerateCommands(string url)
        {
            List<string> list = new List<string>();
            StreamReader reader = new StreamReader("..\\..\\Main.cs");
            string[] s = reader.ReadToEnd().Split(new string[] { "/// " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in s)
            {
                if (line.ToLower().Contains("<command id="))
                {
                    string[] s2 = line.Split(new string[] { "/>" }, StringSplitOptions.RemoveEmptyEntries);


                    list.Add(s2[0]);
                }
            }

            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Steamp3.Commands Date=\"" + DateTime.Now.ToShortDateString() + "\">" + Environment.NewLine;

            for (int i = 1; i < list.Count; i++)
            {
                result += "  " + list[i] + "/>" + Environment.NewLine;
            }

            result += "</Steamp3.Commands>";

            reader.Close();

            Global.SaveString(result, url);
        }
        #endregion

        #region GenerateConfigXml
        public static bool GenerateConfigXml(string url)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
                xml.AppendChild(declaration);

                XmlElement root = xml.CreateElement("configuration");
                XmlElement element = xml.CreateElement("system.net");
                XmlElement element2 = xml.CreateElement("settings");
                XmlElement element3 = xml.CreateElement("httpWebRequest");

                element3.SetAttribute("useUnsafeHeaderParsing", "true");

                element2.AppendChild(element3);
                element.AppendChild(element2);
                root.AppendChild(element);
                xml.AppendChild(root);

                return Global.SaveXml(xml, url, Encoding.UTF8);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region GetAverage
        public static int GetAverage(int[] arr)
        {
            int sum = 0;

            foreach (int i in arr)
            {
                sum += i;
            }

            return sum / arr.GetUpperBound(0);
        }
        #endregion

        #region GetBattery
        public static string GetBattery()
        {
            PowerStatus power = SystemInformation.PowerStatus;

            switch (power.PowerLineStatus)
            {
                case PowerLineStatus.Offline:
                    return "Available: {ls}" + (power.BatteryLifePercent * 100).ToString() + "%{rs} Status: {ls}" + power.BatteryChargeStatus.ToString() + "{rs}";
                default:
                    return "Error: {ls}Not running on battery{rs}";
            }
        }
        #endregion

        #region GetColor
        public static Color GetColor(string s)
        {
            string result = s;

            if (!result.StartsWith("#")) result = "#" + s;

            try { return ColorTranslator.FromHtml(result); }
            catch { return Color.Black; }
        }
        #endregion

        #region GetFont
        public static Font GetFont(string s)
        {
            try { return new Font(s, 8.25f, FontStyle.Regular); }
            catch { return new Font("Tahoma", 8.25f, FontStyle.Regular); }
        }
        #endregion

        #region GetCommunityID
        public static string GetCommunityID(SteamAPI.SteamID steamID)
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return string.Empty;

                WebClient wc = new WebClient();
                //string result = wc.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0001/?key=C341BF941A262342916AF6F150B59031&steamids=" + steamID.ToInt64().ToString() + "&format=xml");
                string result = wc.DownloadString("http://steamcommunity.com/profiles/" + steamID.ToUInt64().ToString() + "?xml=1");
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNode id = xml.SelectSingleNode("profile/customURL");

                if (id == null || string.IsNullOrEmpty(id.InnerText)) return steamID.ToUInt64().ToString();
                return id.InnerText;
            }
            catch
            {
                return steamID.ToUInt64().ToString();
            }
        }
        #endregion

        #region GetContrastColor
        public static Color GetContrastColor(Color c)
        {
            int[] i = { c.R, c.G, c.B, };
            Color result = Color.Black;
            if (Global.GetAverage(i) < 127) result = Color.White;

            return result;
        }
        #endregion

        #region GetCPU
        public static string GetCPU()
        {
            string name = string.Empty;

            string[] s = ReadRegKey(Registry.LocalMachine, "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "ProcessorNameString").Split(new char[] { ' ' });

            foreach (string s2 in s)
            {
                if (!string.IsNullOrEmpty(s2)) name += s2 + " ";
            }
            name = name.Substring(0, name.Length - 1);
            name = name.Replace("(R)", "®");
            name = name.Replace("(TM)", "™");

            return "Processor: {ls}" + name + "{rs}" + Environment.NewLine + "Vendor: {ls}" + ReadRegKey(Registry.LocalMachine, "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "VendorIdentifier") + "{rs} Cores: {ls}" + Environment.ProcessorCount.ToString() + "{rs}";
        }
        #endregion

        #region GetGasPrices
        public static string GetGasPrices(string zip)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            try
            {
                string result = string.Empty;
                XmlDocument xml = new XmlDocument();
                xml.Load("http://www.motortrend.com/widgetrss/gas-prices-" + zip + ".xml");
                XmlNodeList list = xml.SelectNodes("rss/channel/item");

                if (list.Count == 0) return "Invalid zip code: {ls}" + zip + "{rs}";

                foreach (XmlNode node in list)
                {
                    string title = GetXmlValue(node.SelectSingleNode("title"), string.Empty, "N/A");
                    string desc = GetXmlValue(node.SelectSingleNode("description"), string.Empty, "N/A");

                    if (desc.Contains("(Lowest)"))
                    {
                        string[] s = title.Split(new string[] { "   " }, StringSplitOptions.RemoveEmptyEntries);
                        string[] s2 = desc.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);

                        result += "Cheapest gas prices for: {ls}" + zip + "{rs}" + Environment.NewLine;
                        result += "{ls}" + s[0] + "{rs} Location: {ls}" + LeftOf(s[1], ", ") + "{rs}" + Environment.NewLine;

                        foreach (string price in s2)
                        {
                            string[] s3 = price.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                            string price2 = s3[1];

                            if (price2.Contains(" (")) price2 = LeftOf(price2, " (");
                            if (price2 != "N/A")
                            {
                                price2 = ParsePrice(zip, price2);

                                result += s3[0] + ": {ls}$" + price2 + "{rs} ";
                            }
                        }

                        return result;
                    }
                }

                return "Error: {ls}Unable to retrieve data{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetDrive
        public static string GetDrive(string drive)
        {
            try
            {
                DriveInfo di = new DriveInfo(drive);

                if (di.IsReady)
                {
                    string volume = di.VolumeLabel;
                    if (string.IsNullOrEmpty(volume)) volume = "Local disk";

                    return volume + ": {ls}" + drive.ToUpper() + ":\\{rs} Free: {ls}" + ConvertBytes(di.AvailableFreeSpace) + " / " + ConvertBytes(di.TotalSize) + "{rs} " + GetPercent(di.AvailableFreeSpace, di.TotalSize) + "%";
                }
                else return "Drive unavailable: {ls}" + drive.ToUpper() + ":\\{rs}";
            }
            catch
            {
                return "Invalid drive letter: {ls}" + drive + "{rs}";
            }
        }
        #endregion

        #region GetDrives
        public static string GetDrives()
        {
            string[] drives = Environment.GetLogicalDrives();
            int count = 0;
            string result = string.Empty;

            foreach (string drive in drives)
            {
                DriveInfo di = new DriveInfo(drive);

                if (di != null && di.IsReady)
                {
                    string volume = di.VolumeLabel;
                    if (string.IsNullOrEmpty(volume)) volume = "Local disk";
                    count++;

                    result += volume + ": {ls}" + drive + "{rs} Free: {ls}" + ConvertBytes(di.AvailableFreeSpace) + " / " + ConvertBytes(di.TotalSize) + "{rs} " + GetPercent(di.AvailableFreeSpace, di.TotalSize) + "%" + Environment.NewLine;
                }
            }

            result = "Drives found: {ls}" + count.ToString() + "{rs}" + Environment.NewLine + result;

            return result.Substring(0, result.Length - 1);
        }
        #endregion

        #region GetFileDescription
        public static string GetFileDescription(string filename)
        {
            if (filename.ToLower().EndsWith(".flac")) return "Lossless Audio (.flac)";
            else if (filename.ToLower().EndsWith(".m4a")) return "MPEG Layer 4 (.m4a)";
            else if (filename.ToLower().EndsWith(".mp2")) return "MPEG Layer 2 (.mp2)";
            else if (filename.ToLower().EndsWith(".mp3")) return "MPEG Layer 3 (.mp3)";
            else if (filename.ToLower().EndsWith(".mid")) return "MIDI Audio Sound (.mid)";
            else if (filename.ToLower().EndsWith(".midi")) return "MIDI Audio Sound (.midi)";
            else if (filename.ToLower().EndsWith(".wma")) return "Windows Media Audio (.wma)";
            else if (filename.ToLower().EndsWith(".wav")) return "Wave Audio Sound (.wav)";
            else return string.Empty;
        }
        #endregion

        #region GetHoroscope
        public static string GetHoroscope(string sign)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            string[] signs = { "aquarius", "pisces", "aries", "taurus", "gemini", "cancer", "leo", "virgo", "libra", "scorpio", "sagittarius", "capricorn" };

            foreach (string s in signs)
            {
                if (sign.ToLower() == s)
                {
                    try
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load("http://www.astrology.com/horoscopes/daily-horoscope.rss");
                        XmlNodeList list = xml.SelectNodes("rss/channel/item");

                        foreach (XmlNode node in list)
                        {
                            XmlNode title = node.SelectSingleNode("title");
                            XmlNode desc = node.SelectSingleNode("description");

                            if (title.InnerText.ToLower().StartsWith(sign.ToLower()))
                            {
                                string[] s2 = desc.InnerText.Split(new string[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);

                                return "Daily horoscope for: {ls}" + sign + "{rs}" + Environment.NewLine + s2[0].Substring(0, s2[0].Length - 4);
                            }
                        }

                        return "Error: {ls}Unable to retrieve data{rs}";
                    }
                    catch
                    {
                        return "Error: {ls}Unable to retrieve data{rs}";
                    }
                }
            }

            return "Invalid birth sign: {ls}" + sign + "{rs}";
        }
        #endregion

        #region GetHttpFileSize
        public static long GetHttpFileSize(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "HEAD";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.ContentLength;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region GetHttpTitle
        public static string GetHttpTitle(string url)
        {
            try
            {
                if (!MediaPlayer.IsOnline) return string.Empty;

                HttpDownloader hd = new HttpDownloader(url, 5000);
                string result = hd.DownloadUntil("</title>");

                //WebClient wc = new WebClient();
                //string result = wc.DownloadString(url);

                hd.Dispose();
                //wc.Dispose();

                string[] s = result.Split(new string[] { "<title>" }, StringSplitOptions.RemoveEmptyEntries);
                string[] s2 = s[1].Split(new string[] { "</title>" }, StringSplitOptions.RemoveEmptyEntries);
                string[] s3 = s2[0].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                if (s3.GetUpperBound(0) > 3) return string.Empty;

                return HttpUtility.HtmlDecode(s2[0]).TrimStart().TrimEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region GetPercent
        public static string GetPercent(double complete, double total)
        {
            if (complete == 0 || total == 0) return "0.00";

            double result = (complete / total) * 100.0d;

            if (result < 1.0d) return result.ToString("0.00");
            return result.ToString("0");
        }

        public static double GetPercent(double complete, double total, double max)
        {
            if (complete == 0 || total == 0) return 0;

            return (complete / total) * max;
        }

        public static int GetPercent(int complete, int total, int max)
        {
            if (complete == 0 || total == 0) return 0;

            double result = Convert.ToDouble(complete) / Convert.ToDouble(total);
            result *= Convert.ToDouble(max);

            return Convert.ToInt32(result);
        }

        public static int GetPercent(long complete, long total, int max)
        {
            if (complete == 0 || total == 0) return 0;

            double result = Convert.ToDouble(complete) / Convert.ToDouble(total);
            result *= Convert.ToDouble(max);

            return Convert.ToInt32(result);
        }
        #endregion

        #region GoogleSearch
        public static string GoogleSearch(string text)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            string apiKey = "AIzaSyDtpVgOkYypcwWPX8c_bLIDVLyEE-1XWGQ";
            string apiUrl = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q={0}&key={1}";
            string url = string.Format(apiUrl, text, apiKey);

            try
            {
                WebClient wc = new WebClient();
                string result = wc.DownloadString(url);

                wc.Dispose();

                string title = string.Empty;
                string url2 = string.Empty;
                string[] s = result.Split(new char[] { '\"' });

                for (int i = 0; i <= s.GetUpperBound(0); i++)
                {
                    if (s[i] == "unescapedUrl") url2 = s[i + 2];
                    else if (s[i] == "titleNoFormatting")
                    {
                        title = s[i + 2];
                        break;
                    }
                }

                url2 = ReplaceString(url2, "\\u003d", "=");

                if (!string.IsNullOrEmpty(url2)) return "First Google result for: {ls}" + text + "{rs}" + Environment.NewLine + title + Environment.NewLine + url2;

                return "No results containing: {ls}" + text + "{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region BingSearch
        public static string BingSearch(string text)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            string appId = "B48AD2EDE5EAD3DE87A8A2D4D178FE6104C10025";
            string apiUrl = "http://api.bing.net/xml.aspx?Appid={0}&query={1}&sources=web";
            string url = string.Format(apiUrl, appId, text);

            try
            {
                WebClient wc = new WebClient();
                string result = wc.DownloadString(url);

                wc.Dispose();

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);

                XmlNode root = xml.ChildNodes[2];
                if (root.Name == "SearchResponse")
                {
                    XmlNode node = root.ChildNodes[1];

                    if (node.Name == "web:Web")
                    {
                        XmlNode node2 = node.ChildNodes[2];

                        if (node2.Name == "web:Results")
                        {
                            XmlNode node3 = node2.ChildNodes[0];

                            if (node3.Name == "web:WebResult")
                            {
                                XmlNode node4 = node3.ChildNodes[0];
                                XmlNode node5 = node3.ChildNodes[2];

                                return "First Bing result for: {ls}" + text + "{rs}" + Environment.NewLine + node4.InnerText + Environment.NewLine + node5.InnerText;
                            }
                        }
                    }
                }

                return "No results containing: {ls}" + text + "{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetGPU
        public static string GetGPU()
        {
            List<string> result = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

            foreach (ManagementObject query in searcher.Get())
            {
                result.Add(query["Caption"].ToString());
                result.Add(query["AdapterRAM"].ToString());
            }

            return "Graphics device: {ls}" + result[0].Trim() + "{rs}";
        }
        #endregion

        #region GetNullableByte
        public static byte? GetNullableByte(string s)
        {
            byte result = 0;

            if (!string.IsNullOrEmpty(s))
            {
                if (byte.TryParse(s, out result)) return result;

                return null;
            }

            return null;
        }
        #endregion

        #region GetNullableShort
        public static short? GetNullableShort(string s)
        {
            short result = 0;

            if (!string.IsNullOrEmpty(s))
            {
                if (short.TryParse(s, out result)) return result;

                return null;
            }

            return null;
        }
        #endregion

        #region GetOnlineUsers
        public static string GetOnlineUsers()
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                int userCount = 0;
                int onlineUserCount = 0;
                string response = string.Empty;
                WebClient wc = new WebClient();
                string result = wc.DownloadString("http://steamp3.ta0soft.com/stats.php?xml=1");

                if (result.StartsWith("Error")) return "Error: {ls}Unable to retrieve data{rs}";

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNodeList users = xml.SelectNodes("Steamp3.Stats/Users/User");

                foreach (XmlNode user in users)
                {
                    userCount++;
                    if (Global.GetXmlValue(user, "Online", "0") == "1")
                    {
                        onlineUserCount++;
                        response += Global.GetXmlValue(user, "ID", "0") + ",";
                    }
                }

                xml = new XmlDocument();
                xml.Load("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0001/?key=C341BF941A262342916AF6F150B59031&steamids=" + response + "&format=xml");
                XmlNodeList players = xml.SelectNodes("response/players/player");

                response = string.Empty;

                foreach (XmlNode player in players)
                {
                    response += Global.GetXmlValue(player.SelectSingleNode("personaname"), string.Empty, "Unknown") + ", ";
                }

                response = response.Substring(0, response.Length - 2);

                return "Total users: {ls}" + Global.FormatNumber(userCount) + "{rs} Users online: {ls}" + Global.FormatNumber(onlineUserCount) + "{rs}" + Environment.NewLine + response;
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetAchieved
        public static string GetAchieved(SteamAPI.SteamID steamID)
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                WebClient wc = new WebClient();
                string result = wc.DownloadString("http://steamp3.ta0soft.com/stats.php?id=" + steamID.ToUInt64().ToString() + "&xml=1");

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNodeList users = xml.SelectNodes("Steamp3.Stats/Users/User");

                foreach (XmlNode user in users)
                {
                    if (GetXmlValue(user, "ID", "0") == steamID.ToUInt64().ToString())
                    {
                        result = "";
                        int achieved = 0;
                        XmlNode achievements = user.SelectSingleNode("Achievements");

                        int chattyCathy = StringToInt(GetXmlValue(achievements, "ChattyCathy", "0"));
                        int jukeboxHero = StringToInt(GetXmlValue(achievements, "JukeboxHero", "0"));
                        int leetHaxor = StringToInt(GetXmlValue(achievements, "LeetHaxor", "0"));
                        int mp3Playah = StringToInt(GetXmlValue(achievements, "MP3Playah", "0"));
                        int radioStar = StringToInt(GetXmlValue(achievements, "RadioStar", "0"));
                        int repetition = StringToInt(GetXmlValue(achievements, "Repetition", "0"));
                        int shamelessPlug = StringToInt(GetXmlValue(achievements, "ShamelessPlug", "0"));
                        int shortStop = StringToInt(GetXmlValue(achievements, "ShortStop", "0"));
                        int skinnyDipper = StringToInt(GetXmlValue(achievements, "SkinnyDipper", "0"));

                        UI.Achievement achievement = Global.MainWindow.AchievementList.GetItemByID("cc");
                        if (achievement != null && chattyCathy == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("jh");
                        if (achievement != null && jukeboxHero == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("lh");
                        if (achievement != null && leetHaxor == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("mp");
                        if (achievement != null && mp3Playah == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("rs");
                        if (achievement != null && radioStar == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("rp");
                        if (achievement != null && repetition == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("sh");
                        if (achievement != null && shamelessPlug == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("ss");
                        if (achievement != null && shortStop == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }

                        achievement = Global.MainWindow.AchievementList.GetItemByID("sd");
                        if (achievement != null && skinnyDipper == achievement.Maximum)
                        {
                            achieved++;
                            result += achievement.Name + ", ";
                        }


                        if (!string.IsNullOrEmpty(result)) result = result.Substring(0, result.Length - 2);

                        return "{ls}" + Global.Steam.Client.GetFriendPersonaName(steamID) + "{rs} Achievements earned: {ls}" + achieved.ToString() + " / " +  FormatNumber(Global.MainWindow.AchievementList.Items.Count) + "{rs}" + Environment.NewLine + result;
                    }
                }

                return "User not found: {ls}" + Steam.Client.GetFriendPersonaName(steamID) + "{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetStats
        public static string GetStats(SteamAPI.SteamID steamID)
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                WebClient wc = new WebClient();
                string result = wc.DownloadString("http://steamp3.ta0soft.com/stats.php?xml=1&id=" + steamID.ToUInt64().ToString());

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNodeList users = xml.SelectNodes("Steamp3.Stats/Users/User");

                foreach (XmlNode user in users)
                {
                    if (GetXmlValue(user, "ID", "0") == steamID.ToUInt64().ToString())
                    {
                        int achieved = 0;
                        XmlNode achievements = user.SelectSingleNode("Achievements");

                        int commandsUsed = StringToInt(GetXmlValue(user, "CommandsUsed", "0"));
                        int songsPlayed = StringToInt(GetXmlValue(user, "SongsPlayed", "0"));
                        int songsCompleted = StringToInt(GetXmlValue(user, "SongsCompleted", "0"));
                        string song = GetXmlValue(user, "Song", "N/A");
                        string userSince = ConvertUnixDate(StringToInt(GetXmlValue(user, "UserSince", "0")), "Never");
                        string lastOnline = ConvertUnixDate(StringToInt(GetXmlValue(user, "LastOnline", "0")), "Never");
                        string playTime = ConvertSeconds(StringToInt(GetXmlValue(user, "PlayTime", "0")), false);
                        string runTime = ConvertSeconds(StringToInt(GetXmlValue(user, "RunTime", "0")), false);

                        int chattyCathy = StringToInt(GetXmlValue(achievements, "ChattyCathy", "0"));
                        int jukeboxHero = StringToInt(GetXmlValue(achievements, "JukeboxHero", "0"));
                        int leetHaxor = StringToInt(GetXmlValue(achievements, "LeetHaxor", "0"));
                        int mp3Playah = StringToInt(GetXmlValue(achievements, "MP3Playah", "0"));
                        int radioStar = StringToInt(GetXmlValue(achievements, "RadioStar", "0"));
                        int repetition = StringToInt(GetXmlValue(achievements, "Repetition", "0"));
                        int shamelessPlug = StringToInt(GetXmlValue(achievements, "ShamelessPlug", "0"));
                        int shortStop = StringToInt(GetXmlValue(achievements, "ShortStop", "0"));
                        int skinnyDipper = StringToInt(GetXmlValue(achievements, "SkinnyDipper", "0"));

                        UI.Achievement achievement = Global.MainWindow.AchievementList.GetItemByID("cc");
                        if (achievement != null && chattyCathy == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("jh");
                        if (achievement != null && jukeboxHero == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("lh");
                        if (achievement != null && leetHaxor == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("mp");
                        if (achievement != null && mp3Playah == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("rs");
                        if (achievement != null && radioStar == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("rp");
                        if (achievement != null && repetition == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("sh");
                        if (achievement != null && shamelessPlug == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("ss");
                        if (achievement != null && shortStop == achievement.Maximum) achieved++;

                        achievement = Global.MainWindow.AchievementList.GetItemByID("sd");
                        if (achievement != null && skinnyDipper == achievement.Maximum) achieved++;

                        return "STEAMp3 stats for: {ls}" + Steam.Client.GetFriendPersonaName(steamID) + "{rs}" + Environment.NewLine + "Achievements: {ls}" + achieved.ToString() + " / " + FormatNumber(Global.MainWindow.AchievementList.Items.Count) + "{rs} Commands used: {ls}" + FormatNumber(commandsUsed) + "{rs}" + Environment.NewLine + "Songs played: {ls}" + FormatNumber(songsPlayed) + "{rs} Songs completed: {ls}" + FormatNumber(songsCompleted) + "{rs}" + Environment.NewLine + "User since: {ls}" + userSince + "{rs} Last online: {ls}" + lastOnline + "{rs}" + Environment.NewLine + "Play-time: {ls}" + playTime + "{rs} Run-time: {ls}" + runTime + "{rs}" + Environment.NewLine + "Recently played: {ls}" + song + "{rs}";
                    }
                }

                return "User not found: {ls}" + Steam.Client.GetFriendPersonaName(steamID) + "{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetRAM
        public static string GetRAM()
        {
            MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
            if (!GlobalMemoryStatusEx(ms)) return "Error: {ls}Unable to retrieve system memory{rs}"; ;

            string result = "Physical memory: {ls}" + ConvertBytes((long)ms.ullAvailPhys) + " / " + ConvertBytes((long)ms.ullTotalPhys) + "{rs} " + GetPercent(ms.ullAvailPhys, ms.ullTotalPhys) + "%" + Environment.NewLine;
            result += "Virtual memory: {ls}" + ConvertBytes((long)ms.ullAvailVirtual) + " / " + ConvertBytes((long)ms.ullTotalVirtual) + "{rs} " + GetPercent(ms.ullAvailVirtual, ms.ullTotalVirtual) + "%" + Environment.NewLine;
            result += "Page file: {ls}" + ConvertBytes((long)ms.ullAvailPageFile) + " / " + ConvertBytes((long)ms.ullTotalPageFile) + "{rs} " + GetPercent(ms.ullAvailPageFile, ms.ullTotalPageFile) + "%";

            return result;
        }
        #endregion

        #region GetNews
        public static string GetNews()
        {
            try
            {
                if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                XmlDocument xml = new XmlDocument();
                xml.Load("http://steamp3.ta0soft.com/news.rss");

                XmlNode node = xml.SelectSingleNode("rss/channel/item");

                return "Latest STEAMp3 news: {ls}" + Global.GetXmlValue(node.SelectSingleNode("title"), string.Empty, "N/A") + "{rs}" + Environment.NewLine + Global.GetXmlValue(node.SelectSingleNode("description"), string.Empty, "N/A") + Environment.NewLine + Global.GetXmlValue(node.SelectSingleNode("link"), string.Empty, string.Empty);
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }

        public static string GetNews(string url)
        {
            try
            {
                if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                XmlDocument xml = new XmlDocument();
                xml.Load(url);

                XmlNode node = xml.SelectSingleNode("rss/channel/item");

                return "News/RSS feed: {ls}" + Global.GetXmlValue(node.SelectSingleNode("title"), string.Empty, "N/A") + "{rs}" + Environment.NewLine + Global.GetXmlValue(node.SelectSingleNode("description"), string.Empty, "N/A") + Environment.NewLine + Global.GetXmlValue(node.SelectSingleNode("link"), string.Empty, string.Empty);
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetResolution
        public static string GetResolution()
        {
            DEVMODE devMode = new DEVMODE();
            EnumDisplaySettings(null, (int)DisplaySettings.Current, ref devMode);

            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            int ratio = y == 0 ? x : Global.CommonDenominator(y, x % y);
            x /= ratio;
            y /= ratio;

            if (x == 8 && y == 5)
            {
                x = 16;
                y = 10;
            }

            return "Screen resolution: {ls}" + devMode.dmPelsWidth.ToString() + "x" + devMode.dmPelsHeight + "{rs}" + Environment.NewLine + "Aspect ratio: {ls}" + x.ToString() + ":" + y.ToString() + "{rs} Refresh rate: {ls}" + devMode.dmDisplayFrequency.ToString() + "Hz{rs} Colors: {ls}" + devMode.dmBitsPerPel.ToString() + "-Bit{rs}";
        }
        #endregion

        #region GetUsage
        public static string GetUsage()
        {
            MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
            if (!GlobalMemoryStatusEx(ms)) return "Error: {ls}Unable to retrieve system memory{rs}"; ;

            Process p = Process.GetCurrentProcess();

            string result = "Memory usage: {ls}" + ConvertBytes(p.WorkingSet64) + " / " + ConvertBytes((long)ms.ullTotalPhys) + "{rs} " + GetPercent(p.WorkingSet64, ms.ullTotalPhys) + "%";

            return result;
        }
        #endregion

        #region GetWeather
        public static string GetWeather(string zip)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            try
            {
                string result = string.Empty;
                XmlDocument xml = new XmlDocument();
                xml.Load("http://www.google.com/ig/api?weather=" + zip);

                XmlNode forecastInfo = xml.SelectSingleNode("xml_api_reply/weather/forecast_information");
                if (forecastInfo == null) return "Invalid zip code: {ls}" + zip + "{rs}";

                XmlNode currentConditions = xml.SelectSingleNode("xml_api_reply/weather/current_conditions");
                XmlNodeList forecastConditions = xml.SelectNodes("xml_api_reply/weather/forecast_conditions");

                result += "Local weather for: {ls}" + GetXmlValue(forecastInfo.SelectSingleNode("city"), "data", "N/A") + "{rs}" + Environment.NewLine;
                result += "Current: {ls}" + GetXmlValue(currentConditions.SelectSingleNode("temp_f"), "data", "N/A") + " F{rs} ";
                result += "High: {ls}" + GetXmlValue(forecastConditions[0].SelectSingleNode("high"), "data", "N/A") + " F{rs} ";
                result += "Low: {ls}" + GetXmlValue(forecastConditions[0].SelectSingleNode("low"), "data", "N/A") + " F{rs}" + Environment.NewLine;
                result += "Wind: {ls}" + RightOf(GetXmlValue(currentConditions.SelectSingleNode("wind_condition"), "data", "N/A"), ": ") + "{rs} ";
                result += "Humidity: {ls}" + RightOf(GetXmlValue(currentConditions.SelectSingleNode("humidity"), "data", "N/A"), ": ") + "{rs} ";

                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetForecast
        public static string GetForecast(string zip)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            try
            {
                string result = string.Empty;
                XmlDocument xml = new XmlDocument();
                xml.Load("http://www.google.com/ig/api?weather=" + zip);

                XmlNode forecastInfo = xml.SelectSingleNode("xml_api_reply/weather/forecast_information");
                if (forecastInfo == null) return "Invalid zip code: {ls}" + zip + "{rs}";

                result += "Weather forecast for: {ls}" + GetXmlValue(forecastInfo.SelectSingleNode("city"), "data", "N/A") + "{rs}" + Environment.NewLine;

                XmlNodeList forecastConditions = xml.SelectNodes("xml_api_reply/weather/forecast_conditions");

                foreach (XmlNode node in forecastConditions)
                {
                    result += GetXmlValue(node.SelectSingleNode("day_of_week"), "data", "N/A") + ": {ls}" + GetXmlValue(node.SelectSingleNode("condition"), "data", "N/A") + "{rs} ";
                    result += "High: {ls}" + GetXmlValue(node.SelectSingleNode("high"), "data", "N/A") + " F{rs} ";
                    result += "Low: {ls}" + GetXmlValue(node.SelectSingleNode("low"), "data", "N/A") + " F{rs}" + Environment.NewLine;
                }

                return result;
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region GetFirstLine
        public static string GetFirstLine(string lines)
        {
            try
            {
                string[] s = lines.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (s.GetUpperBound(0) == 0) return string.Empty;

                return s[0];
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region GetIniValue
        public static string GetIniValue(string url, string section, string key, string defaultValue)
        {
            try
            {
                StringBuilder result = new StringBuilder(255);
                int i = GetPrivateProfileString(section, key, "", result, 255, url);

                if (string.IsNullOrEmpty(result.ToString())) return defaultValue;
                return result.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region GetIniValueFromString
        public static string GetIniValueFromString(string ini, string attribute, string defaultValue)
        {
            try
            {
                string result = string.Empty;
                string[] s = ini.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s2 in s)
                {
                    if (s2.ToLower().StartsWith(attribute.ToLower() + "=")) result = s2.Substring(attribute.Length + 1);
                }

                if (string.IsNullOrEmpty(result)) return defaultValue;
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region SetIniValue
        public static void SetIniValue(string url, string section, string key, string value)
        {
            try
            {
                WritePrivateProfileString(section, key, value, url);
            }
            catch { }
        }
        #endregion

        #region GetXmlValue
        public static string GetXmlValue(XmlNode node, string attribute, string defaultValue)
        {
            try
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(attribute)) result = HttpUtility.HtmlDecode(node.Attributes[attribute].InnerText);
                else result = HttpUtility.HtmlDecode(node.InnerText);

                if (string.IsNullOrEmpty(result)) result = defaultValue;

                return result;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region GetYouTubeTitle
        public static string GetYouTubeTitle(string url)
        {
            try
            {
                if (!url.Contains("youtube.com/watch?v=")) return string.Empty;

                string id = RightOf(url, "youtube.com/watch?v=");
                if (id.Contains("&")) id = LeftOf(id, "&");

                XmlDocument xml = new XmlDocument();
                xml.Load("http://gdata.youtube.com/feeds/api/videos/" + id);

                XmlNode firstChild = xml.FirstChild.NextSibling.FirstChild;
                XmlNode sibling = firstChild;
                string title = string.Empty;
                string author = string.Empty;
                int views = 0;

                for (int i = 0; i < 100; i++)
                {
                    sibling = sibling.NextSibling;
                    if (sibling == null) break;

                    switch (sibling.Name)
                    {
                        case "title":
                            title = sibling.InnerText;
                            break;
                        case "author":
                            author = sibling.FirstChild.InnerText;
                            break;
                        case "yt:statistics":
                            views = StringToInt(GetXmlValue(sibling, "viewCount", "0"));
                            break;
                    }
                }

                return "YouTube: {ls}" + title + "{rs} Author: {ls}" + author + "{rs} Views: {ls}" + FormatNumber(views) + "{rs}";
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region InUse
        public static bool InUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    //If required we can check for read/write by using fs.CanRead or fs.CanWrite
                }
                
                return false;
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("The process cannot access the file")) return true;
                
                return false;
            }
        }
        #endregion

        #region IsAlpha
        public static bool IsAlpha(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            Regex pattern = new Regex("^[A-Za-z]$");

            return (!pattern.IsMatch(s));
        }
        #endregion

        #region IsColor
        public static bool IsColor(string s)
        {
            Regex pattern = new Regex("^#[a-fA-F0-9]{6}$");
            return (pattern.IsMatch(s));
        }
        #endregion

        #region IsDirectory
        public static bool IsDirectory(string path)
        {
            return string.IsNullOrEmpty(Path.GetExtension(path));
        }
        #endregion

        #region IsValidDirectory
        public static bool IsValidDirectory(string path)
        {
            foreach (char c in path)
            {
                foreach (char c2 in Path.GetInvalidPathChars())
                {
                    if (c.Equals(c2)) return false;
                }
            }

            return true;
        }
        #endregion

        #region IsFile
        public static bool IsFile(string path)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(path));
        }
        #endregion

        #region IsRelativePath
        public static bool IsRelativePath(string path)
        {
            if (path.Length < 3) return false;
            
            string s = path.Substring(0, 2);

            if (s.EndsWith(":")) return false;
            return true;
        }
        #endregion

        #region IsValidFile
        public static bool IsValidFile(string path)
        {
            if (string.IsNullOrEmpty(path.Trim(new char[] { ' ' }))) return false;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (path.Contains(c.ToString())) return false;
            }

            return true;
        }
        #endregion

        #region IsInt32
        public static bool IsInt32(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            int result = 0;

            if (Int32.TryParse(s, out result)) return true;
            return false;
        }
        #endregion

        #region IsUInt64
        public static bool IsUInt64(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            ulong result = 0;

            if (UInt64.TryParse(s, out result)) return true;
            return false;
        }
        #endregion

        #region HSVToColor
        public static Color HSVToColor(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        #endregion

        #region LatestTweet
        public static string LatestTweet(string user)
        {
            try
            {
                if (user.StartsWith("@")) user = user.Replace("@", "");

                XmlDocument doc = new XmlDocument();
                doc.Load("http://twitter.com/status/user_timeline/" + user + "?count=1&callback=?");

                XmlNodeList dateCreated = doc.GetElementsByTagName("created_at");
                XmlNodeList tweetText = doc.GetElementsByTagName("text");

                string cd = dateCreated[0].InnerText;
                string dateData = cd.Substring(0, 10);
                string yrData = cd.Substring(26, 4);
                string timeData = cd.Substring(11, 8);
                string created = dateData + " " + yrData + " " + timeData;
                string result = string.Format("({0}): {1}", created, tweetText[0].InnerText);

                return "Latest tweet by: {ls}" + user + "{rs}" + Environment.NewLine + result;
            }
            catch
            {
                return "Invalid twitter account: {ls}" + user + "{rs}";
            }
        }
        #endregion

        #region LeftOf
        public static string LeftOf(string s, string search)
        {
            try
            {
                return s.Substring(0, s.IndexOf(search));
            }
            catch
            {
                return s;
            }
        }
        #endregion

        #region LoadArray
        public static string[] LoadArray(string filename)
        {
            try
            {
                List<string> result = new List<string>();
                StreamReader reader = new StreamReader(filename);

                while (reader.Peek() >= 0)
                {
                    result.Add(reader.ReadLine());
                }

                reader.Close();
                return result.ToArray();
            }
            catch
            {
                return new string[] { };
            }
        }
        #endregion


        #region LoadString
        public static string LoadString(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);
                string result = reader.ReadToEnd();

                reader.Close();
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region LoWord/HiWord
        public static int LoWord(int i)
        {
            return i & 0xffff;
        }

        public static int LoWord(IntPtr dWord)
        {
            return dWord.ToInt32() & 0xffff;
        }
        
        public static int HiWord(int i)
        {
            return (i >> 16) & 0xffff;
        }

        public static int HiWord(IntPtr dWord)
        {
            if ((dWord.ToInt32() & 0x80000000) == 0x80000000) return (dWord.ToInt32() >> 16);
            else return (dWord.ToInt32() >> 16) & 0xffff;
        }
        #endregion

        #region MeasureString
        public static Rectangle MeasureString(Graphics g, string s, Font f, RectangleF rf)
        {
            if (string.IsNullOrEmpty(s)) return new Rectangle(0, 0, 0, 0);

            CharacterRange[] ranges = { new CharacterRange(0, s.Length) };

            StringFormat sf = (StringFormat)StringFormat.GenericTypographic.Clone();
            sf.Alignment = StringAlignment.Near;
            //sf.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.FitBlackBox;
            sf.LineAlignment = StringAlignment.Near;
            sf.Trimming = StringTrimming.None;

            sf.SetMeasurableCharacterRanges(ranges);

            Region[] regions = g.MeasureCharacterRanges(s, f, rf, sf);

            return Rectangle.Round(regions[0].GetBounds(g));
        }

        public static Rectangle MeasureString(Graphics g, string s, Font f, RectangleF rf, StringFormat sf)
        {
            if (string.IsNullOrEmpty(s)) return new Rectangle(0, 0, 0, 0);

            CharacterRange[] ranges = { new CharacterRange(0, s.Length) };

            sf.SetMeasurableCharacterRanges(ranges);

            Region[] regions = g.MeasureCharacterRanges(s, f, rf, sf);

            return Rectangle.Round(regions[0].GetBounds(g));
        }
        #endregion

        #region Netflix
        public static string Netflix(int count)
        {
            try
            {
                if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                if (count > 0 && count < 21) // goes to 100
                {
                    string result = "Top {ls}" + count + "{rs} Netflix Movies:" + Environment.NewLine;

                    XmlDocument xml = new XmlDocument();
                    xml.Load("http://rss.netflix.com/Top100RSS");
                    XmlNodeList list = xml.SelectNodes("rss/channel/item/title");

                    for (int i = 0; i < count; i++)
                    {
                        result += "{ls}" + (i + 1) + "{rs} " + list[i].InnerText.Substring(5) + Environment.NewLine;
                    }

                    return result.Substring(0, result.Length - 1);
                }
                else return "Invalid integer: {ls}1-20 Only{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region ParsePrice
        public static string ParsePrice(string zip, string price)
        {
            string t = string.Empty;

            if (price[0] == 'Z')
            {
                price = price.Substring(1);
                t = "-";
            }

            for (int i = 0; i < price.Length; i++)
            {
                double i2 = (int)price[i] - CharToInt(zip[1]) % 6 - 65;
                t += i2.ToString();
            }

            double t1 = CharToInt(zip[2]) * 100 + CharToInt(zip[3]) * 10 + CharToInt(zip[1]);
            double t2 = CharToInt(zip[0]) * 100 + CharToInt(zip[4]) * 10 + CharToInt(zip[2]);
            double result = (StringToInt(t) / t2 + t1) / 100;

            return result.ToString();
        }
        #endregion

        #region RandomNumber
        public static int RandomNumber(int max, bool allowZero)
        {
            Randomize();

            int i = (int)Math.Round((double)(max * Rnd()));
            if (i == 0 && !allowZero) i = 1;

            return i;
        }

        public static int RandomNumber(int seed, int max, bool allowZero)
        {
            Randomize(seed);

            int i = (int)Math.Round((double)(max * Rnd()));
            if (i == 0 && !allowZero) i = 1;

            return i;
        }
        #endregion

        #region ReadRegKey
        public static string ReadRegKey(RegistryKey root, string key, string item)
        {
            RegistryKey rk = root.OpenSubKey(key);
            if (rk == null) return string.Empty;

            return (string)rk.GetValue(item);
        }
        #endregion

        #region Recycle
        public static bool Recycle(string filename)
        {
            if (!File.Exists(filename)) return false;

            try
            {
                SHFILEOPSTRUCT sh = new SHFILEOPSTRUCT();
                sh.wFunc = (int)FileOperations.FO_DELETE;
                sh.pFrom = filename + "\0";
                sh.pTo = string.Empty;
                sh.fFlags = (int)FileOperationFlags.FOF_ALLOWUNDO | (int)FileOperationFlags.FOF_NOCONFIRMATION;
                sh.lpszProgressTitle = "Sending " + filename + " to the Recycle Bin";

                SHFileOperation(ref sh);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ReplaceString
        public static string ReplaceString(string s, string find, string replace)
        {
            string s2 = s;
            string result = string.Empty;
            int index = 0;

            while (index > -1)
            {
                index = s2.ToLower().IndexOf(find.ToLower());
                if (index > -1) result = s2.Substring(0, index) + replace + s2.Substring(index + find.Length);
                else result = s2;

                s2 = result;
            }

            return result;
        }
        #endregion

        #region ResolveHost
        public static IPEndPoint ResolveHost(string host, int port)
        {
            IPAddress result = null;
            IPAddress[] ips = Dns.GetHostAddresses(host);

            foreach (IPAddress ip in ips)
            {
                result = ip;
            }

            if (result != null) return new IPEndPoint(result, port);

            return null;
        }
        #endregion

        #region RightOf
        public static string RightOf(string s, string search)
        {
            try
            {
                return s.Substring(s.IndexOf(search) + search.Length);
            }
            catch
            {
                return s;
            }
        }
        #endregion

        #region SaveString
        public static void SaveString(string s, string filename)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filename);

                writer.Write(s);
                writer.Close();
            }
            catch
            {
            }
        }
        #endregion

        #region SaveXml
        public static bool SaveXml(XmlDocument xml, string url, Encoding encoding)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = encoding;
                settings.Indent = true;
                settings.IndentChars = "  ";
                settings.NewLineChars = Environment.NewLine;
                settings.NewLineHandling = NewLineHandling.Replace;
                settings.NewLineOnAttributes = true; //?

                XmlWriter writer = XmlWriter.Create(url, settings);
                xml.Save(writer);

                writer.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ShortenURL
        public static string ShortenURL(string url)
        {
            string queryURL = "http://api.bit.ly/shorten?version=2.0.1&longUrl=" + HttpUtility.UrlEncode(url) + "&login=hikyle&apiKey=R_0ce842bd253d344cd442b4159478c931";
            WebClient wc = new WebClient();
            string result = wc.DownloadString(queryURL);

            wc.Dispose();

            int before = result.IndexOf("shortUrl\": \"") + 12;
            int after = result.IndexOf("\"", before);
            return result.Substring(before, after - before);
        }
        #endregion

        #region StringToBool
        public static bool StringToBool(string s, string trueString)
        {
            if (s.ToLower() == trueString.ToLower()) return true;
            return false;
        }
        #endregion

        #region StringToInt
        public static int StringToInt(string s)
        {
            int result = 0;

            if (Int32.TryParse(s, out result)) return result;
            return 0;
        }
        #endregion

        #region StringToUInt64
        public static ulong StringToUInt64(string s)
        {
            ulong result = 0;

            if (UInt64.TryParse(s, out result)) return result;
            return 0;
        }
        #endregion

        #region TopArtists
        public static string TopArtists(int count)
        {
            try
            {
                if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

                if (count > 0 && count < 21)
                {
                    string result = "Top {ls}" + count + "{rs} Artists:" + Environment.NewLine;

                    XmlDocument xml = new XmlDocument();
                    xml.Load("http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=ff35c0a7a8dcb9bd5689034b1fb35e88");
                    XmlNodeList list = xml.SelectNodes("lfm/artists/artist/name");

                    for (int i = 0; i < count; i++)
                    {
                        result += "{ls}" + (i + 1) + "{rs} " + list[i].InnerText + Environment.NewLine;
                    }

                    return result.Substring(0, result.Length - 1);
                }
                else return "Invalid integer: {ls}1-20 Only{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region TranslateText
        public static string TranslateText(string text, string from, string to, string language)
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            string appId = "B48AD2EDE5EAD3DE87A8A2D4D178FE6104C10025";
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate?appId=" + appId + "&text=" + text + "&from=" + from + "&to=" + to;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                string result = GetXmlValue(xml, string.Empty, string.Empty);

                if (string.IsNullOrEmpty(result) || result == text) return "No translation found for: {ls}" + text + "{rs}";
                else return language + " translation: {ls}" + result + "{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion

        #region Wait
        public static void Wait(int ms)
        {
            int startTime = Environment.TickCount;

            do
            {
                Application.DoEvents();
            }
            while (Environment.TickCount <= startTime + ms);
        }
        #endregion

        #region WordOfTheDay
        public static string WordOfTheDay()
        {
            if (!MediaPlayer.IsOnline) return "Error: {ls}Unable to retrieve data{rs}";

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("http://dictionary.reference.com/wordoftheday/wotd.rss");
                XmlNode node = xml.SelectSingleNode("rss/channel/item");

                if (node != null)
                {
                    string desc = GetXmlValue(node.SelectSingleNode("description"), string.Empty, "N/A");
                    string title = LeftOf(desc, ": ");

                    return "Word of the day: {ls}" + title + "{rs}" + Environment.NewLine + desc;
                }
                else return "Error: {ls}Unable to retrieve data{rs}";
            }
            catch
            {
                return "Error: {ls}Unable to retrieve data{rs}";
            }
        }
        #endregion
        #endregion

        #region Private Methods
        #region Randomize
        // Initialize random number generator
        private static void Randomize()
        {
            RandomSeed = (Environment.TickCount & 0x7FFFFFFF);
        }
        
        private static void Randomize(int number)
        {
            RandomSeed = (Environment.TickCount & number);
        }

        private static void Randomize(double number)
        {
            long value = BitConverter.DoubleToInt64Bits(number);
            value ^= value >> 32;
            RandomSeed = unchecked(((int)value) & 0x7FFFFFFF);
        }

        // Generate random number
        private static float Rnd()
        {
            return Next(RandomSeed, true);
        }

        private static float Rnd(float number)
        {
            if (number == 0.0f)
            {
                return Prev();
            }
            else if (number > 0.0f)
            {
                return Next(RandomSeed, true);
            }
            else
            {
                long value = BitConverter.DoubleToInt64Bits(number);
                value ^= value >> 32;
                int seed = unchecked(((int)value) & 0x7FFFFFFF);
                return Next(seed, false);
            }
        }

        // Get previous value in sequence
        private static float Prev()
        {
            return (float)((((double)RandomSeed) / 2147483648.0));
        }

        // Get next value in sequence
        private static float Next(int prevSeed, bool update)
        {
            int value = unchecked((int)((((uint)prevSeed) * 1103515245 + 12345) & (uint)0x7FFFFFFF));
            if (update) RandomSeed = value;
            return (float)((((double)value) / 2147483648.0));
        }
        #endregion
        #endregion
    }
    #endregion

    #region HttpDownloader
    public class HttpDownloader
    {
        #region Structures
        private struct RequestState
        {
            public const int BufferSize = 512;
            public byte[] Buffer;
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Stream ResponseStream;
            public StringBuilder RequestData;

            public RequestState(HttpWebRequest request)
            {
                Buffer = new byte[BufferSize];
                Request = request;
                Response = null;
                ResponseStream = null;
                RequestData = new StringBuilder(String.Empty);
            }
        }
        #endregion

        #region Delegates
        public delegate void DataReceivedEventHandler(object sender, string data);
        #endregion

        #region Events
        public event DataReceivedEventHandler DataReceived;
        #endregion

        #region Objects
        private static System.Threading.ManualResetEvent AllDone = new System.Threading.ManualResetEvent(false);
        private string p_URL;
        private int p_Timeout;
        private string p_Find;
        private StringBuilder p_RequestData;
        private byte[] p_Buffer;
        private const int BufferSize = 512;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public int Timeout
        {
            get { return p_Timeout; }
        }

        public string Find
        {
            get { return p_Find; }
        }
        #endregion

        #region Constructor/Destructor
        public HttpDownloader(string url, int timeout)
        {
            p_URL = url;
            p_Timeout = timeout;
            p_Find = string.Empty;
            p_RequestData = new StringBuilder(string.Empty);
            p_Buffer = new byte[BufferSize];
        }

        public virtual void Dispose()
        {
            p_Buffer = null;
            p_RequestData = null;
            p_Find = string.Empty;
            p_Timeout = 0;
            p_URL = string.Empty;
        }
        #endregion

        #region Public Methods
        public string DownloadUntil(string find)
        {
            p_Find = find;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p_URL);
                request.KeepAlive = false;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusDescription == "OK")
                {
                    Stream responseStream = response.GetResponseStream();

                    while (true)
                    {
                        int bytes = responseStream.Read(p_Buffer, 0, BufferSize);

                        if (bytes > 0)
                        {
                            Char[] charBuffer = new Char[BufferSize];
                            int len = Encoding.UTF8.GetDecoder().GetChars(p_Buffer, 0, bytes, charBuffer, 0);
                            //String str = new String(charBuffer, 0, len);

                            p_RequestData.Append(Encoding.ASCII.GetString(p_Buffer, 0, bytes));

                            if (p_RequestData.ToString().ToLower().Contains(p_Find.ToLower()))
                            {
                                responseStream.Close();
                                return p_RequestData.ToString();
                            }
                        }
                        else
                        {
                            responseStream.Close();
                            return p_RequestData.ToString();
                        }
                    }
                }
            }
            catch { }

            return string.Empty;
        }

        public void AsyncDownloadUntil(string find)
        {
            p_Find = find;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p_URL);
                request.KeepAlive = false;

                RequestState requestState = new RequestState(request);
                IAsyncResult result = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
                System.Threading.ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new System.Threading.WaitOrTimerCallback(TimeoutCallback), request, p_Timeout, true);

                // The response came in the allowed time.
                AllDone.WaitOne();

                if (requestState.Response != null) requestState.Response.Close();
            }
            catch { }
        }
        #endregion

        #region Private Methods
        private void ResponseCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)requestState.Request.EndGetResponse(ar);

                requestState.Response = response;

                if (response.StatusDescription == "OK")
                {
                    Stream responseStream = response.GetResponseStream();

                    requestState.ResponseStream = responseStream;

                    responseStream.BeginRead(requestState.Buffer, 0, RequestState.BufferSize, new AsyncCallback(ReadCallback), requestState);
                    return;
                }
            }
            catch { }

            AllDone.Set();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                Stream responseStream = requestState.ResponseStream;
                int bytes = responseStream.EndRead(ar);

                if (bytes > 0)
                {
                    Char[] charBuffer = new Char[RequestState.BufferSize];
                    int len = Encoding.UTF8.GetDecoder().GetChars(requestState.Buffer, 0, bytes, charBuffer, 0);
                    //String str = new String(charBuffer, 0, len);

                    requestState.RequestData.Append(Encoding.ASCII.GetString(requestState.Buffer, 0, bytes));

                    if (requestState.RequestData.ToString().ToLower().Contains(p_Find.ToLower()))
                    {
                        responseStream.Close();
                        OnDataReceived(requestState.RequestData.ToString());
                    }
                    else
                    {
                        responseStream.BeginRead(requestState.Buffer, 0, RequestState.BufferSize, new AsyncCallback(ReadCallback), requestState);

                        return;
                    }
                }
                else
                {
                    if (requestState.RequestData.Length > 0)
                    {
                        responseStream.Close();
                    }
                }
            }
            catch { }

            AllDone.Set();
        }

        private void TimeoutCallback(object state, bool timedOut)
        {
            // Abort the request if the timer fires.
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null) request.Abort();
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnDataReceived(string data)
        {
            if (DataReceived != null) DataReceived.Invoke(this, data);
        }
        #endregion
    }
    #endregion

    #region MainWindow
    public class MainWindow : UI.Window
    {
        #region Callbacks
        public delegate void ExitCallback();
        #endregion

        #region Objects
        private UI.TabContainer p_TabContainer;

        private UI.Playlist p_Playlist;
        private UI.Button p_BackButton;
        private UI.Button p_PlayButton;
        private UI.Button p_StopButton;
        private UI.Button p_NextButton;
        private UI.Button p_RandomButton;
        private UI.Button p_RecordButton;
        private UI.Button p_RefreshButton;
        private UI.DropDown p_VolumeDropDown;
        private UI.DropDown p_PlayModeDropDown;
        private UI.SeekBar p_SeekBar;

        private UI.Library p_Library;
        private UI.StationList p_StationList;
        private UI.CommandList p_CommandList;
        private UI.AchievementList p_AchievementList;
        private UI.SkinList p_SkinList;
        private UI.PluginList p_PluginList;

        private UI.Label p_GeneralSettingsLabel;
        private UI.FolderBrowserGroup p_MusicFolderGroup;
        private UI.DropDownGroup p_TriggerGroup;
        private UI.CheckBox p_GlobalCheckBox;
        private UI.CheckBox p_NotificationsCheckBox;
        private UI.CheckBox p_SilentCheckBox;

        private UI.Label p_WebSettingsLabel;
        private UI.DropDownGroup p_ImageLocationGroup;
        private UI.DropDownGroup p_ImageQualityGroup;

        private UI.Label p_AdvancedSettingsLabel;
        private UI.DropDownGroup p_MusicFilterGroup;
        private UI.DropDownGroup p_PlaylistFormatGroup;
        private UI.DropDownGroup p_OutputDeviceGroup;
        private UI.DropDownGroup p_RecordingDeviceGroup;

        private UI.Menu p_MainMenu;

        private NotifyIcon p_NotifyIcon;
        //private FileSystemWatcher p_FileSystemWatcher;
        #endregion

        #region Properties
        public UI.TabContainer TabContainer
        {
            get { return p_TabContainer; }
        }

        public UI.Playlist Playlist
        {
            get { return p_Playlist; }
        }

        public UI.Button BackButton
        {
            get { return p_BackButton; }
        }

        public UI.Button PlayButton
        {
            get { return p_PlayButton; }
        }

        public UI.Button StopButton
        {
            get { return p_StopButton; }
        }

        public UI.Button NextButton
        {
            get { return p_NextButton; }
        }

        public UI.Button RandomButton
        {
            get { return p_RandomButton; }
        }

        public UI.Button RecordButton
        {
            get { return p_RecordButton; }
        }

        public UI.Button RefreshButton
        {
            get { return p_RefreshButton; }
        }

        public UI.DropDown VolumeDropDown
        {
            get { return p_VolumeDropDown; }
        }

        public UI.DropDown PlayModeDropDown
        {
            get { return p_PlayModeDropDown; }
        }

        public UI.SeekBar SeekBar
        {
            get { return p_SeekBar; }
        }

        public UI.Library Library
        {
            get { return p_Library; }
        }

        public UI.StationList StationList
        {
            get { return p_StationList; }
        }

        public UI.CommandList CommandList
        {
            get { return p_CommandList; }
        }

        public UI.AchievementList AchievementList
        {
            get { return p_AchievementList; }
        }

        public UI.SkinList SkinList
        {
            get { return p_SkinList; }
        }

        public UI.PluginList PluginList
        {
            get { return p_PluginList; }
        }

        public UI.DropDownGroup OutputDeviceGroup
        {
            get { return p_OutputDeviceGroup; }
        }

        public UI.DropDownGroup RecordingDeviceGroup
        {
            get { return p_RecordingDeviceGroup; }
        }
        #endregion

        #region Constructor
        public MainWindow() : base()
        {
            try
            {
                Global.Skin = new UI.Skin();
                Global.Settings = new Settings(Application.StartupPath + "\\Steamp3.Settings.xml");
                Global.MediaPlayer = new Steamp3.MediaPlayer();
                Global.Steam = new Steamp3.Steam();
                Global.Stats = new Stats();
                Global.Skin = new UI.Skin(Global.Stats.Skin);
                Global.ToolTip = new UI.ToolTip();

                CheckForUpdates(false);

                MaximizeBox = false; //?
                Size = new Size(Global.Settings.WindowWidth, Global.Settings.WindowHeight);

                p_TabContainer = new UI.TabContainer(this, null);
                p_TabContainer.AddRange(new UI.Tab[] { new UI.Tab("¯", "Playlist"), new UI.Tab("", "Library"), new UI.Tab("º", "Radio"), new UI.Tab("^", "Commands"), new UI.Tab("%", "Achievements"), new UI.Tab(">", "Skins"), new UI.Tab("~", "Plug-ins"), new UI.Tab("@", "Settings") });
                p_TabContainer.SelectedItemChanged += new EventHandler(p_TabContainer_SelectedItemChanged);

                p_Playlist = new UI.Playlist(this, p_TabContainer);
                p_Playlist.Mask = "No files found.";

                p_BackButton = new UI.Button(this, p_TabContainer, "7", string.Empty, "Previous");
                p_BackButton.Enabled = false;
                p_BackButton.MouseClick += new MouseEventHandler(p_BackButton_MouseClick);

                p_PlayButton = new UI.Button(this, p_TabContainer, "4", string.Empty, "Play");
                p_PlayButton.Enabled = false;
                p_PlayButton.MouseClick += new MouseEventHandler(p_PlayButton_MouseClick);

                p_StopButton = new UI.Button(this, p_TabContainer, "<", string.Empty, "Stop");
                p_StopButton.Enabled = false;
                p_StopButton.MouseClick += new MouseEventHandler(p_StopButton_MouseClick);

                p_NextButton = new UI.Button(this, p_TabContainer, "8", string.Empty, "Next");
                p_NextButton.Enabled = false;
                p_NextButton.MouseClick += new MouseEventHandler(p_NextButton_MouseClick);

                p_RandomButton = new UI.Button(this, p_TabContainer, "s", string.Empty, "Random");
                p_RandomButton.Enabled = false;
                p_RandomButton.MouseClick += new MouseEventHandler(p_RandomButton_MouseClick);

                p_RecordButton = new UI.Button(this, p_TabContainer, "=", string.Empty, "Record");
                p_RecordButton.Enabled = false;

                p_RefreshButton = new UI.Button(this, p_TabContainer, "q", string.Empty, "Refresh Playlist");
                p_RefreshButton.Enabled = false;
                p_RefreshButton.MouseClick += new MouseEventHandler(p_RefreshButton_MouseClick);

                p_VolumeDropDown = new UI.DropDown(this, p_TabContainer, "Xð", string.Empty, "Volume");
                p_VolumeDropDown.AddRange(new string[] { "100%", "75%", "50%", "25%", "-", "0% (Mute)" });
                p_VolumeDropDown.DrawArrow = false;
                p_VolumeDropDown.SelectedIndexChanged += new EventHandler(p_VolumeDropDown_SelectedIndexChanged);
                p_VolumeDropDown.UpdateText = false;

                p_PlayModeDropDown = new UI.DropDown(this, p_TabContainer, "6", string.Empty, "Play-mode");
                p_PlayModeDropDown.AddRange(new string[] { "Continuous", "Reverse", "Repeat", "Shuffle", "-", "(None)" });
                p_PlayModeDropDown.DrawArrow = false;
                p_PlayModeDropDown.SelectedIndexChanged += new EventHandler(p_PlayModeDropDown_SelectedIndexChanged);
                p_PlayModeDropDown.UpdateText = false;

                p_SeekBar = new UI.SeekBar(this, p_TabContainer);
                p_SeekBar.Enabled = false;
                p_SeekBar.MouseDown += new MouseEventHandler(p_SeekBar_MouseDown);
                p_SeekBar.MouseMove += new MouseEventHandler(p_SeekBar_MouseMove);
                p_SeekBar.MouseUp += new MouseEventHandler(p_SeekBar_MouseUp);

                p_Library = new UI.Library(this, p_TabContainer);
                p_Library.Mask = "No playlists found.";
                p_Library.Visible = false;

                p_StationList = new UI.StationList(this, p_TabContainer);
                p_StationList.Mask = "No stations found.";
                p_StationList.Visible = false;

                p_CommandList = new UI.CommandList(this, p_TabContainer);
                p_CommandList.Mask = "No commands found.";
                p_CommandList.Visible = false;

                p_AchievementList = new UI.AchievementList(this, p_TabContainer);
                p_AchievementList.Mask = "No achievements found.";
                p_AchievementList.Visible = false;

                p_SkinList = new UI.SkinList(this, p_TabContainer);
                p_SkinList.Mask = "No skins found.";
                p_SkinList.Visible = false;

                p_PluginList = new UI.PluginList(this, p_TabContainer);
                p_PluginList.Mask = "No plug-ins found.";
                p_PluginList.Visible = false;

                p_GeneralSettingsLabel = new UI.Label(this, p_TabContainer, "General Settings");
                p_GeneralSettingsLabel.DrawSeparator = true;
                p_GeneralSettingsLabel.Visible = false;

                p_MusicFolderGroup = new UI.FolderBrowserGroup(this, p_TabContainer, "Music folder:", Global.Settings.MusicFolder, "The directory containing your playlists and music files");
                p_MusicFolderGroup.DefaultButtonClicked += new EventHandler(p_MusicFolderGroup_DefaultButtonClicked);
                p_MusicFolderGroup.FolderChanged += new EventHandler(p_MusicFolderGroup_FolderChanged);
                p_MusicFolderGroup.Visible = false;

                p_TriggerGroup = new UI.DropDownGroup(this, p_TabContainer, "Command trigger:", Global.FormatString(Global.Settings.Trigger, "(None)"), "A prefix can be used to trigger commands", new string[] { "(None)", "~", "!", "@", ".", "/" });
                p_TriggerGroup.Visible = false;
                p_TriggerGroup.DefaultButtonClicked += new EventHandler(p_TriggerGroup_DefaultButtonClicked);
                p_TriggerGroup.SelectedIndexChanged += new EventHandler(p_TriggerGroup_SelectedIndexChanged);

                p_GlobalCheckBox = new UI.CheckBox(this, p_TabContainer, "Enable global commands", "Global commands can be triggered by anyone");
                p_GlobalCheckBox.Checked = Global.Settings.GlobalCommands;
                p_GlobalCheckBox.MouseClick += new MouseEventHandler(p_GlobalCheckBox_MouseClick);
                p_GlobalCheckBox.Visible = false;

                p_NotificationsCheckBox = new UI.CheckBox(this, p_TabContainer, "Scroll media notifications", "Play music without disturbing your friends");
                p_NotificationsCheckBox.Checked = Global.Settings.Notifications;
                p_NotificationsCheckBox.MouseClick += new MouseEventHandler(p_NotificationsCheckBox_MouseClick);
                p_NotificationsCheckBox.Visible = false;

                p_SilentCheckBox = new UI.CheckBox(this, p_TabContainer, "Enable silent mode", "Disable all text sent to group chats and private messages");
                p_SilentCheckBox.Checked = Global.Settings.Silent;
                p_SilentCheckBox.MouseClick += new MouseEventHandler(p_SilentCheckBox_MouseClick);
                p_SilentCheckBox.Visible = false;

                p_WebSettingsLabel = new UI.Label(this, p_TabContainer, "Web Settings");
                p_WebSettingsLabel.DrawSeparator = true;
                p_WebSettingsLabel.Visible = false;

                p_ImageLocationGroup = new UI.DropDownGroup(this, p_TabContainer, "Artist images:", (int)Global.Settings.ImageLocation, "Choose how STEAMp3 should display artist images", new string[] { "(None)", "Download random image from Last.fm", "Download random image from Google", "Display ID3 album art (Not recommended)" });
                p_ImageLocationGroup.Enabled = false;
                p_ImageLocationGroup.DefaultButtonClicked += new EventHandler(p_PlaylistFormatGroup_DefaultButtonClicked);
                p_ImageLocationGroup.SelectedIndexChanged += new EventHandler(p_PlaylistFormatGroup_SelectedIndexChanged);
                p_ImageLocationGroup.Visible = false;

                p_ImageQualityGroup = new UI.DropDownGroup(this, p_TabContainer, "Image quality:", (int)Global.Settings.ImageQuality, "Choose the size of artist images", new string[] { "Low (<100 KB)", "Medium (100-500 KB)", "High (>500 KB)" });
                p_ImageQualityGroup.Enabled = false;
                p_ImageQualityGroup.DefaultButtonClicked += new EventHandler(p_PlaylistFormatGroup_DefaultButtonClicked);
                p_ImageQualityGroup.SelectedIndexChanged += new EventHandler(p_PlaylistFormatGroup_SelectedIndexChanged);
                p_ImageQualityGroup.Visible = false;

                p_AdvancedSettingsLabel = new UI.Label(this, p_TabContainer, "Advanced Settings");
                p_AdvancedSettingsLabel.DrawSeparator = true;
                p_AdvancedSettingsLabel.Visible = false;

                p_MusicFilterGroup = new UI.DropDownGroup(this, p_TabContainer, "Music filter:", Global.Settings.MusicFilter, "Types of files that will be added to the playlist", new string[] { ".mp3", ".mp3;.wav", ".mp3;.wma", ".mp3;.wav;.wma", ".mp3;.m4a;.wav;.wma", ".mp3;.m4a;.wav;.wma;.ogg;.flac" });
                p_MusicFilterGroup.Visible = false;
                p_MusicFilterGroup.DefaultButtonClicked += new EventHandler(p_MusicFilterGroup_DefaultButtonClicked);
                p_MusicFilterGroup.SelectedIndexChanged += new EventHandler(p_MusicFilterGroup_SelectedIndexChanged);

                p_PlaylistFormatGroup = new UI.DropDownGroup(this, p_TabContainer, "Playlist format:", Global.Settings.PlaylistFormat, "Customize the appearance of playlist items", new string[] { "{Artist} - {Title}", "{Artist} - {Album} - {Title}", "{Artist} - {Album} - {Track} - {Title}", "{Title} by {Artist}", "{URL}" });
                p_PlaylistFormatGroup.DefaultButtonClicked += new EventHandler(p_PlaylistFormatGroup_DefaultButtonClicked);
                p_PlaylistFormatGroup.SelectedIndexChanged += new EventHandler(p_PlaylistFormatGroup_SelectedIndexChanged);
                p_PlaylistFormatGroup.Visible = false;

                p_OutputDeviceGroup = new UI.DropDownGroup(this, p_TabContainer, "Output device*:", Global.Settings.OutputDevice, "The device used to play audio files (Windows 7/8 Only)", Global.MediaPlayer.GetDeviceNames());
                p_OutputDeviceGroup.DefaultButtonClicked += new EventHandler(p_OutputDeviceGroup_DefaultButtonClicked);
                p_OutputDeviceGroup.SelectedIndexChanged += new EventHandler(p_OutputDeviceGroup_SelectedIndexChanged);
                p_OutputDeviceGroup.Visible = false;
                SetOutputDevice(Global.Settings.OutputDevice); //?

                p_RecordingDeviceGroup = new UI.DropDownGroup(this, p_TabContainer, "Recording device*:", 0, "The device used to record sound (Windows 7/8 Only)", new string[] { "Microphone" });
                p_RecordingDeviceGroup.Enabled = false;
                p_RecordingDeviceGroup.Visible = false;
                //SetRecordingDevice(Global.Settings.RecordingDevice);

                p_MainMenu = new UI.Menu(null);
                p_MainMenu.AddRange(new UI.MenuItem[] { new UI.MenuItem("¯", "Playlist"), new UI.MenuItem("", "Library"), new UI.MenuItem("º", "Radio"), new UI.MenuItem("^", "Commands"), new UI.MenuItem("%", "Achievements"), new UI.MenuItem(">", "Skins"), new UI.MenuItem("~", "Plug-ins"), new UI.MenuItem("@", "Settings"), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("þ", "Check for Updates..."), new UI.MenuItem("", "Release Notes..."), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("i", "About STEAMp3..."), new UI.MenuItem(string.Empty, "-"), new UI.MenuItem("r", "Exit") });
                p_MainMenu.ItemClicked += new EventHandler(p_MainMenu_ItemClicked);

                p_NotifyIcon = new NotifyIcon();
                p_NotifyIcon.MouseClick += new MouseEventHandler(p_NotifyIcon_MouseClick);
                p_NotifyIcon.MouseDoubleClick += new MouseEventHandler(p_NotifyIcon_MouseDoubleClick);
                p_NotifyIcon.Icon = this.Icon;
                p_NotifyIcon.Text = "STEAMp3";
                p_NotifyIcon.Visible = true;

                //p_FileSystemWatcher = new FileSystemWatcher(Global.Settings.MusicFolder);
                //p_FileSystemWatcher.IncludeSubdirectories = true;
                //p_FileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //p_FileSystemWatcher.Created += new FileSystemEventHandler(p_FileSystemWatcher_Created);
                //p_FileSystemWatcher.Deleted += new FileSystemEventHandler(p_FileSystemWatcher_Deleted);
                //p_FileSystemWatcher.Renamed += new RenamedEventHandler(p_FileSystemWatcher_Renamed);
                //p_FileSystemWatcher.EnableRaisingEvents = false; //?
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\Plugins")) Directory.CreateDirectory(Application.StartupPath + "\\Plugins");
                if (File.Exists(Application.StartupPath + "\\LiveUpdate.exe")) File.Delete(Application.StartupPath + "\\LiveUpdate.exe");
                if (!File.Exists(Application.StartupPath + "\\Steamp3.exe.config")) Global.GenerateConfigXml(Application.StartupPath + "\\Steamp3.exe.config");

                base.OnLoad(e);
            }
            catch { }
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                base.OnShown(e);

                //p_TabContainer.SelectItemByText("Playlist"); //?

                //if (Global.Settings.Playlist.Count > 0) p_Playlist.AddRange(Global.Settings.Playlist);
                //else p_Playlist.AddDirectory(Global.Settings.MusicFolder);
                if (string.IsNullOrEmpty(Global.Settings.PlaylistURL)) p_Playlist.Load(null);
                else p_Playlist.Load(new UI.LibraryItem(Global.Settings.PlaylistURL)); //?

                //UpdateControls();

                p_Library.Refresh();
                p_StationList.Refresh();
                p_CommandList.Refresh();
                p_AchievementList.Refresh();
                p_SkinList.Refresh();
                p_PluginList.Refresh();

                Global.Steam.SendMessage("{steamp3} Online: {ls}" + Global.FormatNumber(Global.Stats.Loads) + "{rs} Files: {ls}" + Global.FormatNumber(p_Playlist.Items.Count) + "{rs} Skin: {ls}" + Global.Skin.Name + "{rs}", true);
                //Global.GenerateCommands("C:\\Users\\Tom\\Web\\ta0soft.com\\public_html\\steamp3\\commands\\commands.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ShowInTaskbar = false;
            Visible = false;
            p_NotifyIcon.Visible = false;

            Global.Steam.SendMessage("{steamp3} Offline: {ls}" + Global.FormatNumber(Global.Stats.Loads) + "{rs}", true);

            //p_FileSystemWatcher.Dispose();
            p_NotifyIcon.Dispose();

            p_MainMenu.Dispose();

            Global.ToolTip.Dispose();
            Global.Stats.Dispose();
            Global.Steam.Dispose();
            Global.MediaPlayer.Dispose();
            Global.Settings.Dispose(true);
            Global.Skin.Dispose();

            base.OnFormClosing(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) p_MainMenu.Show();

            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (!Visible) return;
            int y = 38;

            Global.Settings.WindowWidth = Width;
            Global.Settings.WindowHeight = Height;

            p_TabContainer.SetBounds(6, 6, Width - 12, Height - 12);

            p_Playlist.SetBounds(12, 66, Width - 24, Height - 104);
            p_BackButton.SetBounds(12, Height - 32, 20, 20);
            p_PlayButton.SetBounds(38, Height - 32, 20, 20);
            p_StopButton.SetBounds(64, Height - 32, 20, 20);
            p_NextButton.SetBounds(90, Height - 32, 20, 20);
            p_RandomButton.SetBounds(116, Height - 32, 20, 20);
            p_RecordButton.SetBounds(142, Height - 32, 20, 20);
            p_RefreshButton.SetBounds(168, Height - 32, 20, 20);
            p_VolumeDropDown.SetBounds(194, Height - 32, 20, 20);
            p_PlayModeDropDown.SetBounds(220, Height - 32, 20, 20);
            p_SeekBar.SetBounds(246, Height - 32, Width - 258, 20);

            p_Library.SetBounds(12, 66, Width - 24, Height - 78);
            p_StationList.SetBounds(12, 66, Width - 24, Height - 78);
            p_CommandList.SetBounds(12, 66, Width - 24, Height - 78);
            p_AchievementList.SetBounds(12, 66, Width - 24, Height - 78);
            p_SkinList.SetBounds(12, 66, Width - 24, Height - 78);
            p_PluginList.SetBounds(12, 66, Width - 24, Height - 78);

            if (Height > y + 26) p_GeneralSettingsLabel.SetBounds(12, y, Width - 24, 20);
            else p_GeneralSettingsLabel.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_MusicFolderGroup.SetBounds(12, y, Width - 24, 20);
            else p_MusicFolderGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_TriggerGroup.SetBounds(12, y, Width - 24, 20);
            else p_TriggerGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_GlobalCheckBox.SetBounds(132, y, Width - 140, 20);
            else p_GlobalCheckBox.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_NotificationsCheckBox.SetBounds(132, y, Width - 140, 20);
            else p_NotificationsCheckBox.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_SilentCheckBox.SetBounds(132, y, Width - 140, 20);
            else p_SilentCheckBox.SetBounds(0, 0, 0, 0);
            y += 26;

            if (Height > y + 26) p_WebSettingsLabel.SetBounds(12, y, Width - 24, 20);
            else p_WebSettingsLabel.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_ImageLocationGroup.SetBounds(12, y, Width - 24, 20);
            else p_ImageLocationGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_ImageQualityGroup.SetBounds(12, y, Width - 24, 20);
            else p_ImageQualityGroup.SetBounds(0, 0, 0, 0);
            y += 26;

            if (Height > y + 26) p_AdvancedSettingsLabel.SetBounds(12, y, Width - 24, 20);
            else p_AdvancedSettingsLabel.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_MusicFilterGroup.SetBounds(12, y, Width - 24, 20);
            else p_MusicFilterGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_PlaylistFormatGroup.SetBounds(12, y, Width - 24, 20);
            else p_PlaylistFormatGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_OutputDeviceGroup.SetBounds(12, y, Width - 24, 20);
            else p_OutputDeviceGroup.SetBounds(0, 0, 0, 0);
            y += 26;
            if (Height > y + 26) p_RecordingDeviceGroup.SetBounds(12, y, Width - 24, 20);
            else p_RecordingDeviceGroup.SetBounds(0, 0, 0, 0);

            base.OnResize(e);
        }
        #endregion

        #region Child Events
        #region TabContainer
        private void p_TabContainer_SelectedItemChanged(object sender, EventArgs e)
        {
            string text = p_TabContainer.SelectedItem.Text;

            p_Playlist.Visible = text == "Playlist";
            p_BackButton.Visible = text == "Playlist";
            p_PlayButton.Visible = text == "Playlist";
            p_StopButton.Visible = text == "Playlist";
            p_NextButton.Visible = text == "Playlist";
            p_RandomButton.Visible = text == "Playlist";
            p_RecordButton.Visible = text == "Playlist";
            p_RefreshButton.Visible = text == "Playlist";
            p_VolumeDropDown.Visible = text == "Playlist";
            p_PlayModeDropDown.Visible = text == "Playlist";
            p_SeekBar.Visible = text == "Playlist";

            p_Library.Visible = text == "Library";
            p_StationList.Visible = text == "Radio";
            p_CommandList.Visible = text == "Commands";
            p_AchievementList.Visible = text == "Achievements";
            p_SkinList.Visible = text == "Skins";
            p_PluginList.Visible = text == "Plug-ins";

            p_GeneralSettingsLabel.Visible = text == "Settings";
            p_MusicFolderGroup.Visible = text == "Settings";
            p_TriggerGroup.Visible = text == "Settings";
            p_GlobalCheckBox.Visible = text == "Settings";
            p_NotificationsCheckBox.Visible = text == "Settings";
            p_SilentCheckBox.Visible = text == "Settings";

            p_WebSettingsLabel.Visible = text == "Settings";
            p_ImageLocationGroup.Visible = text == "Settings";
            p_ImageQualityGroup.Visible = text == "Settings";

            p_AdvancedSettingsLabel.Visible = text == "Settings";
            p_MusicFilterGroup.Visible = text == "Settings";
            p_PlaylistFormatGroup.Visible = text == "Settings";
            p_OutputDeviceGroup.Visible = text == "Settings";
            p_RecordingDeviceGroup.Visible = text == "Settings";
        }
        #endregion

        #region BackButton
        private void p_BackButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.Settings.Notifications) Global.MediaPlayer.Previous("Previous", true);
            else Global.MediaPlayer.Previous(string.Empty, true);
        }
        #endregion

        #region PlayButton
        private void p_PlayButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.Settings.Notifications) Global.MediaPlayer.Play(p_Playlist.SelectedIndex, "Playing", true);
            else Global.MediaPlayer.Play(p_Playlist.SelectedIndex, string.Empty, true);
        }
        #endregion

        #region StopButton
        private void p_StopButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.Settings.Notifications) Global.MediaPlayer.Stop("Stopped", true);
            else Global.MediaPlayer.Stop(string.Empty, true);
        }
        #endregion

        #region NextButton
        private void p_NextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.Settings.Notifications) Global.MediaPlayer.Next("Next", true);
            else Global.MediaPlayer.Next(string.Empty, true);
        }
        #endregion

        #region RandomButton
        private void p_RandomButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.Settings.Notifications) Global.MediaPlayer.Random("Random", true);
            else Global.MediaPlayer.Random(string.Empty, true);
        }
        #endregion

        #region RefreshButton
        private void p_RefreshButton_MouseClick(object sender, MouseEventArgs e)
        {
            p_Playlist.Refresh(true);
        }
        #endregion

        #region VolumeDropDown
        private void p_VolumeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (p_VolumeDropDown.SelectedIndex)
            {
                case 0:
                    Global.MediaPlayer.Volume = 100;
                    break;
                case 1:
                    Global.MediaPlayer.Volume = 75;
                    break;
                case 2:
                    Global.MediaPlayer.Volume = 50;
                    break;
                case 3:
                    Global.MediaPlayer.Volume = 25;
                    break;
                case 5:
                    Global.MediaPlayer.Volume = 0;
                    break;
            }
        }
        #endregion

        #region PlayModeDropDown
        private void p_PlayModeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (p_PlayModeDropDown.SelectedIndex)
            {
                case 0:
                    Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Continuous;
                    break;
                case 1:
                    Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Reverse;
                    break;
                case 2:
                    Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Repeat;
                    break;
                case 3:
                    Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Shuffle;
                    break;
                case 5:
                    Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.None;
                    break;
            }
        }
        #endregion

        #region SeekBar
        private void p_SeekBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.MediaPlayer.IsPlaying(false) && e.Button == MouseButtons.Left)
            {
                Global.MediaPlayer.StopTimers();

                p_SeekBar.Value = Global.GetPercent(e.X - p_SeekBar.Bounds.X, p_SeekBar.Bounds.Width - 1, p_SeekBar.Maximum);

                if (Global.ToolTip != null) Global.ToolTip.Show(string.Empty, 10);
            }
        }

        private void p_SeekBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (Global.MediaPlayer.IsPlaying(false) && e.Button == MouseButtons.Left)
            {
                p_SeekBar.Value = Global.GetPercent(e.X - p_SeekBar.Bounds.X, p_SeekBar.Bounds.Width - 1, p_SeekBar.Maximum);

                if (Global.ToolTip != null)
                {
                    Point location = new Point(Cursor.Position.X, Top + (p_SeekBar.Bounds.Y + p_SeekBar.Bounds.Height) + 4);

                    if (location.X < Left + p_SeekBar.Bounds.X) location.X = Left + p_SeekBar.Bounds.X;
                    if (location.X > (Left + p_SeekBar.Bounds.X) + p_SeekBar.Bounds.Width) location.X = (Left + p_SeekBar.Bounds.X) + p_SeekBar.Bounds.Width;

                    Global.ToolTip.Location = location;
                    Global.ToolTip.Message = Global.ConvertSeconds(p_SeekBar.Value, false) + " / " + Global.ConvertSeconds(p_SeekBar.Maximum, false) + " (" + Global.GetPercent(p_SeekBar.Value, p_SeekBar.Maximum) + "%)";
                }
            }
        }

        private void p_SeekBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (Global.MediaPlayer.IsPlaying(false))
            {
                if (Global.ToolTip != null) Global.ToolTip.Hide();

                Global.MediaPlayer.Position = p_SeekBar.Value;
                Global.MediaPlayer.StartTimers();
            }
        }
        #endregion

        #region MusicFolderGroup
        private void p_MusicFolderGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            SetMusicFolder(string.Empty);
        }

        private void p_MusicFolderGroup_FolderChanged(object sender, EventArgs e)
        {
            SetMusicFolder(p_MusicFolderGroup.FolderBrowser.Dialog.SelectedPath);
        }
        #endregion

        #region TriggerGroup
        private void p_TriggerGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            SetTrigger(string.Empty);
        }

        private void p_TriggerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTrigger(p_TriggerGroup.DropDown.Text);
        }
        #endregion

        #region GlobalCheckBox
        private void p_GlobalCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Settings.GlobalCommands = p_GlobalCheckBox.Checked;
        }
        #endregion

        #region NotificationsCheckBox
        private void p_NotificationsCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Settings.Notifications = p_NotificationsCheckBox.Checked;
        }
        #endregion

        #region SilentCheckBox
        private void p_SilentCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Settings.Silent = p_SilentCheckBox.Checked;
        }
        #endregion

        #region ImageLocationGroup
        private void p_ImageLocationGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
        }

        private void p_ImageLocationGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region ImageQualityGroup
        private void p_ImageQualityGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
        }

        private void p_ImageQualityGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region MusicFilterGroup
        private void p_MusicFilterGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            SetMusicFilter(string.Empty);
        }

        private void p_MusicFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMusicFilter(p_MusicFilterGroup.DropDown.Text);
        }
        #endregion

        #region PlaylistFormatGroup
        private void p_PlaylistFormatGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            SetPlaylistFormat(string.Empty);
        }

        private void p_PlaylistFormatGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPlaylistFormat(p_PlaylistFormatGroup.DropDown.Text);
        }
        #endregion

        #region OutputDeviceGroup
        private void p_OutputDeviceGroup_DefaultButtonClicked(object sender, EventArgs e)
        {
            SetOutputDevice(0);
        }

        private void p_OutputDeviceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOutputDevice(p_OutputDeviceGroup.DropDown.SelectedIndex);
        }
        #endregion

        #region MainMenu
        private void p_MainMenu_ItemClicked(object sender, EventArgs e)
        {
            switch (p_MainMenu.SelectedItem.Text)
            {
                case "Playlist":
                case "Library":
                case "Radio":
                case "Commands":
                case "Achievements":
                case "Skins":
                case "Plug-ins":
                case "Settings":
                    p_TabContainer.SelectItemByText(p_MainMenu.SelectedItem.Text);
                    Global.SetForegroundWindow(Handle);
                    break;
                case "Check for Updates...":
                    CheckForUpdates(true);
                    break;
                case "Release Notes...":
                    Process.Start("http://steamp3.ta0soft.com/liveupdate/");
                    break;
                case "About STEAMp3...":
                    UI.AboutDialog ad = new UI.AboutDialog();
                    ad.ShowDialog(this);
                    ad.Dispose();
                    break;
                case "Exit":
                    Exit();
                    break;
            }
        }
        #endregion

        #region NotifyIcon
        private void p_NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) p_MainMenu.Show();
        }

        void p_NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Global.SetForegroundWindow(Handle);
        }
        #endregion

        #region FileSystemWatcher
        private void p_FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (Global.IsDirectory(e.FullPath))
            {
                p_Playlist.AddDirectory(e.FullPath);

                //Global.Steam.SendMessage("Folder added/created: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
            }
            else
            {
                foreach (string ext in Global.Settings.MusicFilter.Split(new char[] { ';' }))
                {
                    if (ext.Length > 1 && Path.GetExtension(e.FullPath).ToLower().Equals(ext.ToLower()))
                    {
                        p_Playlist.Add(new UI.PlaylistItem(e.FullPath, false));
                        
                        //Global.Steam.SendMessage("File added/created: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
                        return;
                    }
                }
            }
        }

        private void p_FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (Global.IsDirectory(e.FullPath))
            {
                for (int i = 0; i < p_Playlist.Items.Count; i++)
                {
                    if (p_Playlist.Items[i].URL.ToLower().StartsWith(e.FullPath.ToLower()))
                    {
                        p_Playlist.RemoveAt(i);
                        i = 0;
                    }
                }

                //Global.Steam.SendMessage("Folder moved/deleted: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
            }
            else
            {
                foreach (string ext in Global.Settings.MusicFilter.Split(new char[] { ';' }))
                {
                    if (ext.Length > 1 && Path.GetExtension(e.FullPath).ToLower().Equals(ext.ToLower()))
                    {
                        for (int i = 0; i < p_Playlist.Items.Count; i++)
                        {
                            if (p_Playlist.Items[i].URL.ToLower().Equals(e.FullPath.ToLower()))
                            {
                                p_Playlist.RemoveAt(i);

                                //Global.Steam.SendMessage("File moved/deleted: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
                                return;
                            }
                        }

                        return;
                    }
                }
            }
        }

        private void p_FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (Global.IsDirectory(e.FullPath))
            {
                foreach (UI.PlaylistItem item in p_Playlist.Items)
                {
                    if (item.URL.ToLower().StartsWith(e.OldFullPath.ToLower())) item.URL = e.FullPath + item.URL.Substring(e.OldFullPath.Length); //?
                }

                //Global.Steam.SendMessage("Folder renamed: {ls}" + Path.GetFileName(e.OldFullPath) + "{rs}" + Environment.NewLine + "To: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
            }
            else
            {
                foreach (string ext in Global.Settings.MusicFilter.Split(new char[] { ';' }))
                {
                    if (ext.Length > 1 && Path.GetExtension(e.FullPath).ToLower().Equals(ext.ToLower()))
                    {
                        foreach (UI.PlaylistItem item in p_Playlist.Items)
                        {
                            if (item.URL.ToLower().Equals(e.OldFullPath.ToLower())) //?
                            {
                                item.URL = e.FullPath;

                                //Global.Steam.SendMessage("File renamed: {ls}" + Path.GetFileName(e.OldFullPath) + "{rs}" + Environment.NewLine + "To: {ls}" + Path.GetFileName(e.FullPath) + "{rs}", true);
                                return;
                            }
                        }

                        return;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Public Methods
        #region CheckForUpdates
        public void CheckForUpdates(bool info)
        {
            if (!Global.MediaPlayer.IsOnline) return;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("http://steamp3.ta0soft.com/liveupdate/liveupdate.xml");
                XmlNode node = xml.SelectSingleNode("Steamp3.LiveUpdate/Update");

                string[] currentVersion = Application.ProductVersion.Split(new char[] { '.' });
                string[] newVersion = node.Attributes["Version"].Value.Split(new char[] { '.' });

                if (currentVersion.GetUpperBound(0) != newVersion.GetUpperBound(0)) return;

                for (int i = 0; i < 3; i++)
                {
                    if (Global.StringToInt(newVersion[i]) > Global.StringToInt(currentVersion[i]))
                    {
                        UI.UpdateDialog ud = new UI.UpdateDialog(node);
                        if (ud.ShowDialog(this) == DialogResult.OK)
                        {
                            string args = string.Empty;
                            FileStream fs = new FileStream(Application.StartupPath + "\\LiveUpdate.exe", FileMode.Create);
                            
                            fs.Write(Properties.Resources.LiveUpdate, 0, Properties.Resources.LiveUpdate.Length);
                            fs.Close();

                            foreach (UI.UpdateListItem item in ud.UpdateList.FilteredItems)
                            {
                                if (item.Backup) args += "\"" + Application.StartupPath + "\\" + item.URL + ".bak\" ";
                            }
                            args = args.Substring(0, args.Length - 1);

                            Process.Start(Application.StartupPath + "\\LiveUpdate.exe", args);

                            Process.GetCurrentProcess().Kill(); //?
                            //Exit();
                        }

                        ud.Dispose();
                        return;
                    }
                }

                if (info)
                {
                    UI.InfoDialog id = new UI.InfoDialog("This version of STEAMp3 (" + Application.ProductVersion + ") is up to date, no new updates available.");
                    id.ShowDialog(this);
                    id.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Exit
        public void Exit()
        {
            if (InvokeRequired) Invoke(new ExitCallback(Exit));
            else Close();
        }
        #endregion

        #region SetMusicFolder
        public void SetMusicFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder)) folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            Global.Settings.MusicFolder = folder;
            p_MusicFolderGroup.FolderBrowser.Text = folder;

            //p_FileSystemWatcher.Path = folder;

            //p_Playlist.Refresh(true);
            p_Library.Refresh();
        }
        #endregion

        #region SetMusicFilter
        public void SetMusicFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter)) filter = ".mp3;.m4a;.wav;.wma;.ogg;.flac";

            Global.Settings.MusicFilter = filter;
            p_MusicFilterGroup.DropDown.Text = filter;

            p_Playlist.Refresh(true);
        }
        #endregion

        #region SetOutputDevice
        public void SetOutputDevice(int index)
        {
            if (index < 0 || index > Global.MediaPlayer.Devices.Count - 1) index = 0;

            Global.Settings.OutputDevice = index;
            p_OutputDeviceGroup.DropDown.Text = p_OutputDeviceGroup.DropDown.Items[index];

            Global.MediaPlayer.ChangeDevice(index);
        }
        #endregion

        #region SetPlaylistFormat
        public void SetPlaylistFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) format = "{Artist} - {Title}";

            Global.Settings.PlaylistFormat = format;
            p_PlaylistFormatGroup.DropDown.Text = format;

            foreach (UI.PlaylistItem item in p_Playlist.Items)
            {
                item.Reformat();
            }

            p_Playlist.UpdateItems();
        }
        #endregion

        #region SetTrigger
        public void SetTrigger(string trigger)
        {
            if (trigger.Length > 1) trigger = string.Empty;

            Global.Settings.Trigger = trigger;
            p_TriggerGroup.DropDown.Text = Global.FormatString(trigger, "(None)");
        }
        #endregion

        #region ProcessGlobalCommand
        public bool ProcessGlobalCommand(string msg, bool chat)
        {
            if (!Global.Settings.GlobalCommands) return false;

            string[] s = msg.Split(new char[] { ' ' });
            string command = string.Empty;
            if (s.GetUpperBound(0) == 0) command = msg.ToLower();
            else command = s[0].ToLower();

            if (!command.StartsWith(Global.Settings.Trigger)) return false;
            command = command.Substring(Global.Settings.Trigger.Length);

            string response = string.Empty;

            if (s.GetUpperBound(0) > 0)
            {
                string data = String.Empty;
                for (int i = 1; i <= s.GetUpperBound(0); i++)
                {
                    data += s[i] + " ";
                }
                data = data.Substring(0, data.Length - 1);

                switch (command)
                {
                    default:
                        return false;
                }
            }
            else
            {
                switch (command)
                {
                    /// <Command ID="{YouTube URL}" Param="" Desc="Scroll YouTube video title" Type="0" New="1" />
                    /// <Command ID="{URL}" Param="" Desc="Scroll HTTP web title" Type="0" New="1" />

                    /// <Command ID="adv/advertise" Param="" Desc="Advertise STEAMp3" Type="0" New="0" />
                    case "adv":
                    case "advertise":
                        Global.Steam.SendMessage("{steamp3} Only available at: {ls}steamp3.ta0soft.com{rs}", chat);

                        if (!chat) p_AchievementList.IncreaseShamelessPlug(Global.Steam.PM.FriendID);
                        break;

                    /// <Command ID="build/version" Param="" Desc="Scroll current STEAMp3 build number" Type="0" New="0" />
                    case "build":
                    case "version":
                        Global.Steam.SendMessage("{steamp3} Version: {ls}" + Application.ProductVersion.Substring(0, 3) + "{rs} Build: {ls}" + Application.ProductVersion.Substring(4) + "{rs}", chat);
                        break;
                    default:
                        if (msg.StartsWith("http://") || msg.StartsWith("www."))
                        {
                            if (msg.Contains(" ") || !msg.Contains(".")) return false;

                            if (msg.Contains("youtube.com/watch?v=")) response = Global.GetYouTubeTitle(msg);
                            else response = Global.GetHttpTitle(msg);

                            if (!string.IsNullOrEmpty(response)) Global.Steam.SendMessage(response, chat);
                        }
                        return false;
                }
            }

            return true;
        }
        #endregion

        #region ProcessLocalCommand
        public bool ProcessLocalCommand(string msg, bool chat)
        {
            string[] s = msg.Split(new char[] { ' ' });
            string command = string.Empty;
            if (s.GetUpperBound(0) == 0) command = msg.ToLower();
            else command = s[0].ToLower();

            if (!command.StartsWith(Global.Settings.Trigger)) return false;
            command = command.Substring(Global.Settings.Trigger.Length);

            int index = 0;
            string response = string.Empty;
            List<UI.PlaylistItem> list = null;
            TimeSpan ts = new TimeSpan();

            if (s.GetUpperBound(0) > 0)
            {
                string data = String.Empty;
                for (int i = 1; i <= s.GetUpperBound(0); i++)
                {
                    data += s[i] + " ";
                }
                data = data.Substring(0, data.Length - 1);

                switch (command)
                {
                    /// <Command ID="p/play" Param="Index" Desc="Play file by {param}" Type="1" New="1" />
                    /// <Command ID="p/play" Param="SearchTerm" Desc="Play file containing {param}" Type="1" New="0" />
                    case "p":
                    case "play":
                        if (Global.IsInt32(data))
                        {
                            index = Global.StringToInt(data) - 1;
                            if (index <= 0) index = 0;
                            if (index >= p_Playlist.FilteredItems.Count) index = 0;

                            Global.MediaPlayer.Play(p_Playlist.FilteredItems[index], "Playing", chat);
                        }
                        else
                        {
                            list = new List<UI.PlaylistItem>();

                            foreach (UI.PlaylistItem item in p_Playlist.Items)
                            {
                                if (item.ToLongString().ToLower().Contains(data.ToLower())) list.Add(item);
                            }

                            if (list.Count == 0) Global.Steam.SendStatus("No matches containing", data, chat);
                            else if (list.Count == 1) Global.MediaPlayer.Play(list[0], "Playing", chat);
                            else
                            {
                                p_Playlist.FilterBar.TextBox.Text = data;

                                Global.Steam.SendMessage(Global.FormatNumber(p_Playlist.FilteredItems.Count) + " / " + Global.FormatNumber(p_Playlist.Items.Count) + " matches containing: {ls}" + data + "{rs} " + Global.GetPercent(p_Playlist.FilteredItems.Count, p_Playlist.Items.Count) + "%" + Environment.NewLine + "Type: {ls}p/play {1-" + p_Playlist.FilteredItems.Count.ToString() + "}{rs} To play or: {ls}cf/clear{rs} To clear", chat);
                            }
                        }
                        break;

                    /// <Command ID="r/random" Param="SearchTerm" Desc="Play random file containing {param}" Type="1" New="0" />
                    case "r":
                    case "random":
                        list = new List<UI.PlaylistItem>();

                        foreach (UI.PlaylistItem item in p_Playlist.Items)
                        {
                            if (item.ToLongString().ToLower().Contains(data.ToLower())) list.Add(item);
                        }

                        if (list.Count == 0) Global.Steam.SendStatus("No matches containing", data, chat);
                        else Global.MediaPlayer.Play(list[Global.RandomNumber(list.Count - 1, true)], "Random", chat);
                        break;

                    /// <Command ID="c/count" Param="SearchTerm" Desc="Count files containing {param}" Type="1" New="0" />
                    case "c":
                    case "count":
                        list = new List<UI.PlaylistItem>();

                        foreach (UI.PlaylistItem item in p_Playlist.Items)
                        {
                            if (item.ToLongString().ToLower().Contains(data.ToLower())) list.Add(item);
                        }

                        Global.Steam.SendMessage(Global.FormatNumber(list.Count) + " / " + Global.FormatNumber(p_Playlist.Items.Count) + " matches containing: {ls}" + data + "{rs} " + Global.GetPercent(list.Count, p_Playlist.Items.Count) + "%", chat);
                        break;

                    /// <Command ID="b/back" Param="Index" Desc="Play previous file by {param}" Type="1" New="1" />
                    /// <Command ID="b/back" Param="SearchTerm" Desc="Play previous file containing {param}" Type="1" New="1" />
                    case "b":
                    case "back":
                        if (Global.IsInt32(data))
                        {
                            index = p_Playlist.SelectedIndex - Global.StringToInt(data);
                            if (index <= 0) index = p_Playlist.FilteredItems.Count - 1;

                            Global.MediaPlayer.Play(p_Playlist.FilteredItems[index], "Previous", chat);
                        }
                        else
                        {
                            list = new List<UI.PlaylistItem>();

                            index = p_Playlist.SelectedIndex - 1;
                            if (index <= 0) index = p_Playlist.FilteredItems.Count - 1;

                            for (int i = index; i >= 0; i--)
                            {
                                if (p_Playlist.FilteredItems[i].ToLongString().ToLower().Contains(data.ToLower())) list.Add(p_Playlist.FilteredItems[i]);
                            }

                            if (list.Count == 0) Global.Steam.SendStatus("No matches containing", data, chat);
                            else Global.MediaPlayer.Play(list[0], "Previous", chat);
                        }
                        break;

                    /// <Command ID="n/next" Param="Index" Desc="Play next file by {param}" Type="1" New="1" />
                    /// <Command ID="n/next" Param="SearchTerm" Desc="Play next file containing {param}" Type="1" New="1" />
                    case "n":
                    case "next":
                        if (Global.IsInt32(data))
                        {
                            index = p_Playlist.SelectedIndex + Global.StringToInt(data);
                            if (index >= p_Playlist.FilteredItems.Count) index = 0;

                            Global.MediaPlayer.Play(p_Playlist.FilteredItems[index], "Next", chat);
                        }
                        else
                        {
                            list = new List<UI.PlaylistItem>();

                            index = p_Playlist.SelectedIndex + 1;
                            if (index >= p_Playlist.FilteredItems.Count) index = 0;

                            for (int i = index; i < p_Playlist.FilteredItems.Count; i++)
                            {
                                if (p_Playlist.FilteredItems[i].ToLongString().ToLower().Contains(data.ToLower())) list.Add(p_Playlist.FilteredItems[i]);
                            }

                            if (list.Count == 0) Global.Steam.SendStatus("No matches containing", data, chat);
                            else Global.MediaPlayer.Play(list[0], "Next", chat);
                        }
                        break;

                    /// <Command ID="ren/rename" Param="NewName" Desc="Re-name current file to {param}" Type="1" New="0" />
                    case "ren":
                    case "rename":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot rename stream", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            response = Path.GetDirectoryName(p_Playlist.PlayingItem.URL) + "\\" + data + Path.GetExtension(p_Playlist.PlayingItem.URL);

                            if (Global.IsValidFile(data.ToLower()))
                            {
                                File.Move(p_Playlist.PlayingItem.URL, response);
                                Global.Steam.SendMessage("Renamed: {ls}" + Path.GetFileName(p_Playlist.PlayingItem.URL) + "{rs}" + Environment.NewLine + "To: {ls}" + Path.GetFileName(response) + "{rs}", chat);
                                p_Playlist.PlayingItem.URL = response;
                            }
                            else Global.Steam.SendStatus("Invalid filename", data, chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="title" Param="NewTitle" Desc="Update current title" Type="1" New="0" />
                    case "title":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Title = string.Empty;
                            else p_Playlist.PlayingItem.Title = data;

                            Global.Steam.SendStatus("Title saved", Global.FormatString(p_Playlist.PlayingItem.Title, "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="artist" Param="NewArtist" Desc="Update current artist" Type="1" New="0" />
                    case "artist":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Artist = string.Empty;
                            else p_Playlist.PlayingItem.Artist = data;

                            Global.Steam.SendStatus("Artist saved", Global.FormatString(p_Playlist.PlayingItem.Artist, "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="album" Param="NewAlbum" Desc="Update current album" Type="1" New="0" />
                    case "album":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Album = string.Empty;
                            else p_Playlist.PlayingItem.Album = data;

                            Global.Steam.SendStatus("Album saved", Global.FormatString(p_Playlist.PlayingItem.Album, "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="genre" Param="NewGenre" Desc="Update current genre" Type="1" New="0" />
                    case "genre":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Genre = string.Empty;
                            else p_Playlist.PlayingItem.Genre = data;

                            Global.Steam.SendStatus("Genre saved", Global.FormatString(p_Playlist.PlayingItem.Genre, "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="track" Param="0-9999" Desc="Update current track" Type="1" New="0" />
                    case "track":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Track = null;
                            else if (Global.IsInt32(data)) p_Playlist.PlayingItem.Track = Global.GetNullableShort(data);
                            else
                            {
                                Global.Steam.SendStatus("Invalid track", "0-9999 Only", chat);
                                return true;
                            }

                            Global.Steam.SendStatus("Track saved", Global.FormatString(p_Playlist.PlayingItem.Track.ToString(), "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="tc/trackcount" Param="0-9999" Desc="Update current track count" Type="1" New="0" />
                    case "tc":
                    case "trackcount":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.TrackCount = null;
                            else if (Global.IsInt32(data)) p_Playlist.PlayingItem.TrackCount = Global.GetNullableShort(data);
                            else
                            {
                                Global.Steam.SendStatus("Invalid track count", "0-9999 Only", chat);
                                return true;
                            }

                            Global.Steam.SendStatus("Track count saved", Global.FormatString(p_Playlist.PlayingItem.TrackCount.ToString(), "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="year" Param="0-9999" Desc="Update current year" Type="1" New="0" />
                    case "year":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (data.ToLower() == "none") p_Playlist.PlayingItem.Year = null;
                            else if (Global.IsInt32(data)) p_Playlist.PlayingItem.Year = Global.GetNullableShort(data);
                            else
                            {
                                Global.Steam.SendStatus("Invalid year", "0-9999 Only", chat);
                                return true;
                            }

                            Global.Steam.SendStatus("Year saved", Global.FormatString(p_Playlist.PlayingItem.Year.ToString(), "None"), chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="rating" Param="0.0-5.0" Desc="Update current rating" Type="1" New="0" />
                    case "rating":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot edit stream ID3", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            switch (data.ToLower())
                            {
                                case "none":
                                case "0":
                                case "0.0":
                                    p_Playlist.PlayingItem.Rating = 0.0d;
                                    break;
                                case "0.5":
                                    p_Playlist.PlayingItem.Rating = 0.5d;
                                    break;
                                case "1":
                                case "1.0":
                                    p_Playlist.PlayingItem.Rating = 1.0d;
                                    break;
                                case "1.5":
                                    p_Playlist.PlayingItem.Rating = 1.5d;
                                    break;
                                case "2":
                                case "2.0":
                                    p_Playlist.PlayingItem.Rating = 2.0d;
                                    break;
                                case "2.5":
                                    p_Playlist.PlayingItem.Rating = 2.5d;
                                    break;
                                case "3":
                                case "3.0":
                                    p_Playlist.PlayingItem.Rating = 3.0d;
                                    break;
                                case "3.5":
                                    p_Playlist.PlayingItem.Rating = 3.5d;
                                    break;
                                case "4":
                                case "4.0":
                                    p_Playlist.PlayingItem.Rating = 4.0d;
                                    break;
                                case "4.5":
                                    p_Playlist.PlayingItem.Rating = 4.5d;
                                    break;
                                case "5":
                                case "5.0":
                                    p_Playlist.PlayingItem.Rating = 5.0d;
                                    break;
                                default:
                                    Global.Steam.SendStatus("Invalid rating", "0.0-5.0 Only", chat);
                                    return true;
                            }

                            Global.Steam.SendStatus("Rating saved", Global.FormatString(p_Playlist.PlayingItem.Rating.ToString(), "0") + " / 5", chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="folder" Param="Path" Desc="Update current music folder" Type="1" New="0" />
                    case "folder":
                        if (Directory.Exists(data))
                        {
                            Global.Steam.SendStatus("Music folder saved", data, chat);
                            SetMusicFolder(data);
                        }
                        else Global.Steam.SendStatus("Invalid music folder", "Path not found", chat);
                        break;

                    /// <Command ID="filter" Param="Filter" Desc="Update current music filter" Type="1" New="0" />
                    case "filter":
                        if (data.Contains(";"))
                        {
                            string[] types = data.Split(new char[] { ';' });

                            foreach (string type in types)
                            {
                                if (!type.StartsWith("."))
                                {
                                    Global.Steam.SendStatus("Invalid music filter", "Must begin with '.'", chat);
                                    return true;
                                }
                            }

                            Global.Steam.SendStatus("Music filter saved", data, chat);
                            SetMusicFilter(data);
                        }
                        else
                        {
                            if (data.StartsWith("."))
                            {
                                Global.Steam.SendStatus("Music filter saved", data, chat);
                                SetMusicFilter(data);
                            }
                            else Global.Steam.SendStatus("Invalid music filter", "Must begin with '.'", chat);
                        }
                        break;

                    /// <Command ID="format" Param="Format" Desc="Update current playlist format" Type="1" New="0" />
                    case "format":
                        if (data.ToLower().Contains("{artist}") || data.ToLower().Contains("{url}"))
                        {
                            Global.Steam.SendStatus("Playlist format saved", data, chat);
                            SetPlaylistFormat(data);
                        }
                        else Global.Steam.SendStatus("Invalid playlist format", "Must contain {Artist} or {URL}", chat);
                        break;

                    /// <Command ID="device/output" Param="DeviceName" Desc="Update current output device" Type="1" New="1" />
                    case "device":
                    case "output":
                        for (int i = 0; i < p_OutputDeviceGroup.DropDown.Items.Count; i++)
                        {
                            if (p_OutputDeviceGroup.DropDown.Items[i].ToLower().Contains(data.ToLower()))
                            {
                                SetOutputDevice(i);
                                Global.Steam.SendStatus("Output device saved", p_OutputDeviceGroup.DropDown.Items[i], chat);
                                return true;
                            }
                        }

                        Global.Steam.SendError("Invalid output device", chat);
                        break;

                    /// <Command ID="trig/trigger" Param="Trigger" Desc="Update current command trigger" Type="4" New="0" />
                    case "trig":
                    case "trigger":
                        if (data.Length > 1)
                        {
                            if (data.ToLower() == "none") SetTrigger(string.Empty);
                            else
                            {
                                Global.Steam.SendStatus("Invalid trigger length", "0-1 Only", chat);
                                return true;
                            }
                        }
                        else SetTrigger(data);

                        Global.Steam.SendStatus("Command trigger saved", Global.FormatString(Global.Settings.Trigger, "None"), chat);
                        break;

                    /// <Command ID="ls" Param="Bracket" Desc="Update current left separator" Type="4" New="0" />
                    case "ls":
                        if (data.ToLower() == "none") Global.Settings.LeftSeparator = string.Empty;
                        else Global.Settings.LeftSeparator = data;

                        Global.Steam.SendStatus("Left separator saved", Global.FormatString(Global.Settings.LeftSeparator, "None"), chat);
                        break;

                    /// <Command ID="rs" Param="Bracket" Desc="Update current right separator" Type="4" New="0" />
                    case "rs":
                        if (data.ToLower() == "none") Global.Settings.RightSeparator = string.Empty;
                        else Global.Settings.RightSeparator = data;

                        Global.Steam.SendStatus("Right separator saved", Global.FormatString(Global.Settings.RightSeparator, "None"), chat);
                        break;

                    /// <Command ID="pl/playlist" Param="SearchTerm" Desc="Load playlist containing {param}" Type="1" New="1" />
                    case "pl":
                    case "playlist":
                        foreach (UI.LibraryItem item in p_Library.Items)
                        {
                            if (item.Text.ToLower().Contains(data.ToLower()))
                            {
                                p_Playlist.Load(item); //?

                                Global.Steam.SendMessage("Playlist loaded: {ls}" + item.Text + "{rs} Files: {ls}" + Global.FormatNumber(p_Playlist.Items.Count) + "{rs}", chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="station" Param="SearchTerm" Desc="Stream radio station containing {param}" Type="1" New="1" />
                    case "station":
                        foreach (UI.Station station in p_StationList.Items)
                        {
                            if (station.Name.ToLower().Contains(data.ToLower()))
                            {
                                Global.MediaPlayer.Stream(station, "Station", chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="skin" Param="SearchTerm" Desc="Load skin containing {param}" Type="4" New="0" />
                    case "skin":
                        if (p_SkinList.SkinDesigner == null || p_SkinList.SkinDesigner.IsDisposed)
                        {
                            foreach (UI.Skin skin in p_SkinList.Items)
                            {
                                if (skin.Name.ToLower().Contains(data.ToLower()))
                                {
                                    Global.Skin = new UI.Skin(skin.URL);

                                    ReDraw();

                                    Global.Steam.SendMessage("Skin loaded: {ls}" + Global.Skin.Name + "{rs} Author: {ls}" + Global.Skin.AuthorPersonaName + "{rs}", chat);
                                    return true;
                                }
                            }

                            Global.Steam.SendStatus("No matches containing", data, chat);
                        }
                        else Global.Steam.SendError("Unable to load skin", chat);
                        break;

                    /// <Command ID="vol/volume" Param="0-100" Desc="Update current volume" Type="1" New="0" />
                    case "vol":
                    case "volume":
                        int vol = 0;

                        if (Int32.TryParse(data, out vol))
                        {
                            if (vol >= 0 && vol <= 100)
                            {
                                Global.MediaPlayer.Volume = vol;

                                Global.Steam.SendStatus("Volume saved", vol.ToString() + "%", chat);
                            }
                            else Global.Steam.SendStatus("Invalid volume", "0-100 Only", chat);
                        }
                        else Global.Steam.SendStatus("Invalid volume", "0-100 Only", chat);
                        break;

                    /// <Command ID="url/web" Param="URL" Desc="Launch {param} in default browser" Type="3" New="0" />
                    case "url":
                    case "web":
                        if (!data.Contains("http://")) data = "http://" + data;
                        if (!data.Contains(".")) data = data + ".com";
                        Global.Steam.SendStatus("Launching URL", data, chat);
                        Process.Start(data);
                        break;

                    /// <Command ID="news/rss" Param="URL" Desc="Scroll RSS feed for {param}" Type="3" New="1" />
                    case "news":
                    case "rss":
                        if (!data.Contains("http://")) data = "http://" + data;
                        else Global.Steam.SendMessage(Global.GetNews(data), chat);
                        break;

                    /// <Command ID="drive/free" Param="DriveLetter" Desc="Scroll drive capacity for {param}" Type="4" New="0" />
                    case "drive":
                    case "free":
                        Global.Steam.SendMessage(Global.GetDrive(data), chat);
                        break;

                    /// <Command ID="google" Param="SearchTerm" Desc="Scroll first Google result for {param}" Type="3" New="0" />
                    case "google":
                        Global.Steam.SendMessage(Global.GoogleSearch(data), chat);
                        break;

                    /// <Command ID="bing" Param="SearchTerm" Desc="Scroll first Bing result for {param}" Type="3" New="1" />
                    case "bing":
                        Global.Steam.SendMessage(Global.BingSearch(data), chat);
                        break;

                    /// <Command ID="horoscope" Param="BirthSign" Desc="Scroll daily horoscope for {param}" Type="3" New="0" />
                    case "horoscope":
                        Global.Steam.SendMessage(Global.GetHoroscope(data), chat);
                        break;

                    /// <Command ID="gas" Param="ZipCode" Desc="Scroll cheapest gas prices for {param}" Type="3" New="0" />
                    case "gas":
                        Global.Steam.SendMessage(Global.GetGasPrices(data), chat);
                        break;

                    /// <Command ID="weather" Param="ZipCode" Desc="Scroll local weather for {param}" Type="3" New="0" />
                    case "weather":
                        Global.Steam.SendMessage(Global.GetWeather(data), chat);
                        break;

                    /// <Command ID="forecast" Param="ZipCode" Desc="Scroll weather forecast for {param}" Type="3" New="0" />
                    case "forecast":
                        Global.Steam.SendMessage(Global.GetForecast(data), chat);
                        break;

                    /// <Command ID="tweet" Param="User" Desc="Scroll latest twitter post from {param}" Type="3" New="0" />
                    case "tweet":
                        Global.Steam.SendMessage(Global.LatestTweet(data), chat);
                        break;

                    /// <Command ID="top" Param="1-20" Desc="Scroll top {param} music artists" Type="3" New="0" />
                    case "top":
                        if (Global.IsInt32(data)) Global.Steam.SendMessage(Global.TopArtists(Convert.ToInt32(data)), chat);
                        else Global.Steam.SendStatus("Invalid integer", "1-20 Only", chat);
                        break;

                    /// <Command ID="leaks" Param="1-20" Desc="Scroll top {param} album leaks" Type="3" New="0" />
                    case "leaks":
                        if (Global.IsInt32(data)) Global.Steam.SendMessage(Global.CurrentLeaks(Convert.ToInt32(data)), chat);
                        else Global.Steam.SendStatus("Invalid integer", "1-20 Only", chat);
                        break;

                    /// <Command ID="netflix" Param="1-20" Desc="Scroll top {param} Netflix movies" Type="3" New="0" />
                    case "netflix":
                        if (Global.IsInt32(data)) Global.Steam.SendMessage(Global.Netflix(Convert.ToInt32(data)), chat);
                        else Global.Steam.SendStatus("Invalid integer", "1-20 Only", chat);
                        break;

                    /// <Command ID="arabic" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "arabic":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ar", "Arabic"), chat);
                        break;

                    /// <Command ID="bulgarian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "bulgarian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "bg", "Bulgarian"), chat);
                        break;

                    /// <Command ID="catalan" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "catalan":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ca", "Catalan"), chat);
                        break;

                    /// <Command ID="chinese" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "chinese":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "zh-CHS", "Chinese"), chat);
                        break;

                    /// <Command ID="czech" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "czech":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "cs", "Czech"), chat);
                        break;

                    /// <Command ID="danish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "danish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "da", "Danish"), chat);
                        break;

                    /// <Command ID="dutch" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "dutch":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "nl", "Dutch"), chat);
                        break;

                    /// <Command ID="estonian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "estonian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "et", "Estonian"), chat);
                        break;

                    /// <Command ID="finnish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "finnish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "fi", "Finnish"), chat);
                        break;

                    /// <Command ID="french" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "french":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "fr", "French"), chat);
                        break;

                    /// <Command ID="german" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "german":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "de", "German"), chat);
                        break;

                    /// <Command ID="greek" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "greek":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "el", "Greek"), chat);
                        break;

                    /// <Command ID="haitian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "haitian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ht", "Haitian"), chat);
                        break;

                    /// <Command ID="hebrew" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "hebrew":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "he", "Hebrew"), chat);
                        break;

                    /// <Command ID="hindi" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "hindi":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "hi", "Hindi"), chat);
                        break;

                    /// <Command ID="hungarian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "hungarian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "hu", "Hungarian"), chat);
                        break;

                    /// <Command ID="indonesian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "indonesian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "id", "Indonesian"), chat);
                        break;

                    /// <Command ID="italian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "italian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "it", "Italian"), chat);
                        break;

                    /// <Command ID="japanese" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "japanese":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ja", "Japanese"), chat);
                        break;

                    /// <Command ID="korean" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "korean":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ko", "Korean"), chat);
                        break;

                    /// <Command ID="latvian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "latvian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "lv", "Latvian"), chat);
                        break;

                    /// <Command ID="lithuanian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "lithuanian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "lt", "Lithuanian"), chat);
                        break;

                    /// <Command ID="norwegian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "norwegian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "no", "Norwegian"), chat);
                        break;

                    /// <Command ID="polish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "polish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "pl", "Polish"), chat);
                        break;

                    /// <Command ID="portuguese" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "portuguese":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "pt", "Portuguese"), chat);
                        break;

                    /// <Command ID="romanian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "romanian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ro", "Romanian"), chat);
                        break;

                    /// <Command ID="russian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "russian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "ru", "Russian"), chat);
                        break;

                    /// <Command ID="slovakian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "slovakian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "sk", "Slovakian"), chat);
                        break;

                    /// <Command ID="slovenian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "slovenian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "sl", "Slovenian"), chat);
                        break;

                    /// <Command ID="spanish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "spanish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "es", "Spanish"), chat);
                        break;

                    /// <Command ID="swedish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "swedish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "sv", "Swedish"), chat);
                        break;

                    /// <Command ID="thai" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "thai":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "th", "Thai"), chat);
                        break;

                    /// <Command ID="turkish" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "turkish":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "tr", "Turkish"), chat);
                        break;

                    /// <Command ID="ukrainian" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "ukrainian":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "uk", "Ukrainian"), chat);
                        break;

                    /// <Command ID="vietnamese" Param="Text" Desc="Translate english to {id}" Type="3" New="0" />
                    case "vietnamese":
                        Global.Steam.SendMessage(Global.TranslateText(data, "en", "vi", "Vietnamese"), chat);
                        break;

                    /// <Command ID="name" Param="NewName" Desc="Update Steam persona name" Type="2" New="0" />
                    case "name":
                        Global.Steam.Client.SetPersonaName(data);
                        Global.Steam.SendStatus("Persona name saved", data, chat);
                        break;

                    /// <Command ID="alias" Param="FriendName" Desc="Scroll friend aliases" Type="2" New="0" />
                    case "alias":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                List<string> result = Global.Steam.Client.GetFriendAliases(id);
                                response = string.Empty;

                                foreach (string alias in result)
                                {
                                    response += alias + ", ";
                                }
                                response = response.Substring(0, response.Length - 2);

                                Global.Steam.SendMessage("{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Is known by: {ls}" + result.Count.ToString() + "{rs} Aliases " + Environment.NewLine + response, chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="game" Param="FriendName" Desc="Scroll game info for {param}" Type="2" New="0" />
                    case "game":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                string gameName = string.Empty;
                                ulong gameID = 0;
                                uint gameIP = 0;
                                ushort gamePort = 0;

                                if (Global.Steam.Client.GetFriendGamePlayed(id, ref gameName, ref gameID, ref gameIP, ref gamePort))
                                {
                                    if (gameIP != 0) response = "{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} In-game: {ls}" + gameName + "{rs} IP: {ls}" + Global.ConvertIP(gameIP) + "{rs} Port: {ls}" + gamePort.ToString() + "{rs}";
                                    else response = "{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Is not in-game";
                                }
                                else response = "Error: {ls}Unable to retrieve game info{rs}";

                                Global.Steam.SendMessage(response, chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="joingame" Param="FriendName" Desc="Join current game for {param}" Type="2" New="0" />
                    case "joingame":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                string gameName = string.Empty;
                                ulong gameID = 0;
                                uint gameIP = 0;
                                ushort gamePort = 0;

                                if (Global.Steam.Client.GetFriendGamePlayed(id, ref gameName, ref gameID, ref gameIP, ref gamePort))
                                {
                                    if (gameIP != 0)
                                    {
                                        response = "Joining game: {ls}" + gameName + "{rs} IP: {ls}" + Global.ConvertIP(gameIP) + "{rs} Port: {ls}" + gamePort.ToString() + "{rs}";
                                        Process.Start("steam://connect/" + Global.ConvertIP(gameIP) + ":" + gamePort.ToString());
                                    }
                                    else response = "{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Is not in-game";
                                }
                                else response = "Error: {ls}Unable to retrieve game info{rs}";

                                Global.Steam.SendMessage(response, chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="profile" Param="FriendName" Desc="View STEAMp3 profile for {param}" Type="3" New="1" />
                    case "profile":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                response = "http://steamp3.ta0soft.com/profiles/" + Global.GetCommunityID(id);

                                Global.Steam.SendMessage("Viewing STEAMp3 profile for: {ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs}" + Environment.NewLine + response, chat);
                                Process.Start(response);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="achieved" Param="FriendName" Desc="Scroll earned achievements for {param}" Type="3" New="1" />
                    case "achieved":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                Global.Steam.SendMessage(Global.GetAchieved(id), chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="stats" Param="FriendName" Desc="Scroll STEAMp3 user stats for {param}" Type="3" New="1" />
                    case "stats":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                Global.Steam.SendMessage(Global.GetStats(id), chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="seen/lastseen" Param="FriendName" Desc="Scroll friend last seen date" Type="2" New="0" />
                    case "seen":
                    case "lastseen":
                        foreach (SteamAPI.SteamID id in Global.Steam.Client.GetFriends())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                response = Global.Steam.Client.GetFriendLogOffDate(id);

                                if (Global.Steam.Client.IsFriendOnline(id)) Global.Steam.SendMessage("{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Last seen online: {ls}Online now{rs}", chat);
                                else if (response.Equals(DateTime.Now.ToShortDateString())) Global.Steam.SendMessage("{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Last seen online: {ls}Today{rs}", chat);
                                else Global.Steam.SendMessage("{ls}" + Global.Steam.Client.GetFriendPersonaName(id) + "{rs} Last seen online: {ls}" + response + "{rs}", chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="kick" Param="UserName" Desc="Kick {param} from current chat" Type="2" New="0" />
                    case "kick":
                        foreach (SteamAPI.SteamID id in Global.Steam.Chat.GetChatUsers())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                if (Global.Steam.Chat.KickUser(id)) Global.Steam.SendStatus("Kicked", Global.Steam.Client.GetFriendPersonaName(id), chat);
                                else Global.Steam.SendStatus("Unable to kick", Global.Steam.Client.GetFriendPersonaName(id), chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    /// <Command ID="ban" Param="UserName" Desc="Ban {param} from current chat" Type="2" New="0" />
                    case "ban":
                        foreach (SteamAPI.SteamID id in Global.Steam.Chat.GetChatUsers())
                        {
                            if (Global.Steam.Client.GetFriendPersonaName(id).ToLower().Contains(data.ToLower()))
                            {
                                if (Global.Steam.Chat.BanUser(id)) Global.Steam.SendStatus("Banned", Global.Steam.Client.GetFriendPersonaName(id), chat);
                                else Global.Steam.SendStatus("Unable to ban", Global.Steam.Client.GetFriendPersonaName(id), chat);
                                return true;
                            }
                        }

                        Global.Steam.SendStatus("No matches containing", data, chat);
                        break;

                    default:
                        return false;
                }
            }
            else
            {
                switch (command)
                {
                    /// <Command ID="cf/clear" Param="" Desc="Clear playlist search filter" Type="1" New="0" />
                    case "cf":
                    case "clear":
                        p_Playlist.FilterBar.TextBox.Text = string.Empty;
                        Global.Steam.SendStatus("Playlist search filter", "Cleared", chat);
                        break;

                    /// <Command ID="b/back" Param="" Desc="Play previous file" Type="1" New="0" />
                    case "b":
                    case "back":
                        Global.MediaPlayer.Previous("Previous", chat);
                        break;

                    /// <Command ID="p/play" Param="" Desc="Play selected file" Type="1" New="0" />
                    case "p":
                    case "play":
                        if (Global.MediaPlayer.IsPaused()) Global.MediaPlayer.Resume("Resumed", chat);
                        else Global.MediaPlayer.Play("Playing", chat);
                        break;

                    /// <Command ID="ps/pause" Param="" Desc="Pause current file" Type="1" New="0" />
                    case "ps":
                    case "pause":
                        if (Global.MediaPlayer.IsPlaying(false)) Global.MediaPlayer.Pause("Paused", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="s/stop" Param="" Desc="Stop current file" Type="1" New="0" />
                    case "s":
                    case "stop":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.MediaPlayer.Stop("Stopped", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="n/next" Param="" Desc="Play next file" Type="1" New="0" />
                    case "n":
                    case "next":
                        Global.MediaPlayer.Next("Next", chat);
                        break;

                    /// <Command ID="r/random" Param="" Desc="Play random file" Type="1" New="0" />
                    case "r":
                    case "random":
                        Global.MediaPlayer.Random("Random", chat);
                        break;

                    /// <Command ID="ref/refresh" Param="" Desc="Refresh playlist" Type="1" New="0" />
                    case "ref":
                    case "refresh":
                        if (p_Playlist.Folders.Count > 0) p_Playlist.Refresh(chat);
                        else Global.Steam.SendError("Unable to refresh playlist", chat);
                        break;

                    /// <Command ID="none" Param="" Desc="Update play-mode to {id}" Type="1" New="0" />
                    case "none":
                        Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.None;
                        Global.Steam.SendStatus("Play-mode saved", "None", chat);
                        break;

                    /// <Command ID="cont/continuous" Param="" Desc="Update play-mode to {id}" Type="1" New="0" />
                    case "cont":
                    case "continuous":
                        Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Continuous;
                        Global.Steam.SendStatus("Play-mode saved", "Continuous", chat);
                        break;

                    /// <Command ID="rev/reverse" Param="" Desc="Update play-mode to {id}" Type="1" New="0" />
                    case "rev":
                    case "reverse":
                        Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Reverse;
                        Global.Steam.SendStatus("Play-mode saved", "Reverse", chat);
                        break;

                    /// <Command ID="shuf/shuffle" Param="" Desc="Update play-mode to {id}" Type="1" New="0" />
                    case "shuf":
                    case "shuffle":
                        Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Shuffle;
                        Global.Steam.SendStatus("Play-mode saved", "Shuffle", chat);
                        break;

                    /// <Command ID="rep/repeat" Param="" Desc="Update play-mode to {id}" Type="1" New="0" />
                    case "rep":
                    case "repeat":
                        Global.MediaPlayer.PlayMode = MediaPlayer.PlayModes.Repeat;
                        Global.Steam.SendStatus("Play-mode saved", "Repeat", chat);
                        break;

                    /// <Command ID="mode/playmode" Param="" Desc="Scroll current play-mode" Type="1" New="0" />
                    case "mode":
                    case "playmode":
                        Global.Steam.SendStatus("Current play-mode", Global.MediaPlayer.PlayMode.ToString(), chat);
                        break;

                    /// <Command ID="exit" Param="" Desc="Exit STEAMp3" Type="4" New="0" />
                    case "exit":
                        Exit();
                        break;

                    /// <Command ID="pl/playlist" Param="" Desc="View Playlist tab" Type="4" New="0" />
                    case "pl":
                    case "playlist":
                        p_TabContainer.SelectItemByText("Playlist");
                        Global.Steam.SendStatus("Viewing", "Playlist", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="lib/library" Param="" Desc="View Library tab" Type="4" New="1" />
                    case "lib":
                    case "library":
                        p_TabContainer.SelectItemByText("Library");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_Library.Items.Count) + " Playlists", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="radio" Param="" Desc="View Radio tab" Type="4" New="0" />
                    case "radio":
                        p_TabContainer.SelectItemByText("Radio");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_StationList.Items.Count) + " Radio stations", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="coms/commands" Param="" Desc="View Commands tab" Type="4" New="0" />
                    case "coms":
                    case "commands":
                        p_TabContainer.SelectItemByText("Commands");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_CommandList.Items.Count) + " Commands", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="achievements" Param="" Desc="View Achievements tab" Type="4" New="0" />
                    case "achievements":
                        p_TabContainer.SelectItemByText("Achievements");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_AchievementList.Items.Count) + " Achievements", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="skins" Param="" Desc="View Skins tab" Type="4" New="0" />
                    case "skins":
                        p_TabContainer.SelectItemByText("Skins");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_SkinList.Items.Count) + " Skins", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="plugins" Param="" Desc="View Plug-ins tab" Type="4" New="0" />
                    case "plugins":
                        p_TabContainer.SelectItemByText("Plug-ins");
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_PluginList.Items.Count) + " Plug-ins", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="settings" Param="" Desc="View Settings tab" Type="4" New="0" />
                    case "settings":
                        p_TabContainer.SelectItemByText("Settings");
                        Global.Steam.SendStatus("Viewing", "Settings", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="all/allmusic" Param="" Desc="Filter playlist by all music" Type="4" New="0" />
                    case "all":
                    case "allmusic":
                        p_Playlist.FilterBar.DropDown.SelectedIndex = 0;
                        Global.Steam.SendStatus("Viewing", "All Music", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="favs/favorites" Param="" Desc="Filter playlist by favorites" Type="4" New="0" />
                    case "favs":
                    case "favorites":
                        p_Playlist.FilterBar.DropDown.SelectedIndex = 1;
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_Playlist.FilteredItems.Count) + " Favorites", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="recent" Param="" Desc="Filter playlist by recently played" Type="4" New="0" />
                    case "recent":
                        p_Playlist.FilterBar.DropDown.SelectedIndex = 2;
                        Global.Steam.SendStatus("Viewing", Global.FormatNumber(p_Playlist.FilteredItems.Count) + " Recently Played", chat);
                        Global.SetForegroundWindow(Handle);
                        break;

                    /// <Command ID="del/delete" Param="" Desc="Send current file to recycle bin" Type="1" New="0" />
                    case "del":
                    case "delete":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot delete stream", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            p_Playlist.Remove(p_Playlist.PlayingItem);
                            Global.Recycle(p_Playlist.PlayingItem.URL);

                            Global.Steam.SendMessage("Deleted: {ls}" + Path.GetFileName(p_Playlist.PlayingItem.URL) + "{rs}", chat);

                            Global.MediaPlayer.Stop(string.Empty, true);

                            p_Playlist.SelectedItem = null; //?
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="played/playcount" Param="" Desc="Scroll current play count" Type="1" New="0" />
                    case "played":
                    case "playcount":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendMessage("Play count: {ls}" + p_Playlist.PlayingItem.PlayCount.ToString() + "{rs} Last played: {ls}" + Global.FormatString(p_Playlist.PlayingItem.LastPlayed, "Never") + "{rs}", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="size" Param="" Desc="Scroll current file size" Type="1" New="0" />
                    case "size":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current file size", Global.ConvertBytes(p_Playlist.PlayingItem.Size), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="path" Param="" Desc="Scroll current file path" Type="1" New="0" />
                    case "path":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current file path", Path.GetDirectoryName(p_Playlist.PlayingItem.URL), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="url/file" Param="" Desc="Scroll current file URL" Type="1" New="0" />
                    case "url":
                    case "file":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current file URL", p_Playlist.PlayingItem.URL, chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="song" Param="" Desc="Scroll current song name" Type="1" New="0" />
                    case "song":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current song", p_Playlist.PlayingItem.Text, chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="station" Param="" Desc="Scroll current radio station" Type="1" New="1" />
                    case "station":
                        if (p_StationList.PlayingItem != null) Global.Steam.SendStatus("Current station", p_StationList.PlayingItem.Name, chat);
                        else Global.Steam.SendError("No file streaming", chat);
                        break;

                    /// <Command ID="rst/rstation" Param="" Desc="Stream random radio station" Type="1" New="1" />
                    case "rst":
                    case "rstation":
                        Global.MediaPlayer.Stream(p_StationList.FilteredItems[Global.RandomNumber(p_StationList.FilteredItems.Count - 1, true)], "Station", chat);
                        break;

                    /// <Command ID="ext/extension" Param="" Desc="Scroll current file extension" Type="1" New="0" />
                    case "ext":
                    case "extension":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current file extension", Path.GetExtension(p_Playlist.PlayingItem.URL), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="type" Param="" Desc="Scroll current file type" Type="1" New="0" />
                    case "type":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Current file type", Global.FormatString(p_Playlist.PlayingItem.FileType, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="pos/position" Param="" Desc="Scroll current file position" Type="1" New="0" />
                    case "pos":
                    case "position":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendMessage("Current position: {ls}" + Global.ConvertSeconds(Global.MediaPlayer.Position, false) + " / " + Global.ConvertSeconds(Global.MediaPlayer.Duration, false) + "{rs} " + Global.GetPercent(Global.MediaPlayer.Position, Global.MediaPlayer.Duration) + "%", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="id3" Param="" Desc="Scroll current ID3 tag info" Type="1" New="0" />
                    case "id3":
                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (string.IsNullOrEmpty(p_Playlist.PlayingItem.Artist) && string.IsNullOrEmpty(p_Playlist.PlayingItem.Title)) Global.Steam.SendError("No ID3 tag found", chat);
                            else Global.Steam.SendMessage("Artist: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Artist, "N/A") + "{rs}" + Environment.NewLine + "Title: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Title, "N/A") + "{rs}" + Environment.NewLine + "Album: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Album, "N/A") + "{rs} Genre: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Genre, "N/A") + "{rs}" + Environment.NewLine + "Track: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Track.ToString(), "N/A") + " / " + Global.FormatString(p_Playlist.PlayingItem.TrackCount.ToString(), "N/A") + "{rs} Year: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Year.ToString(), "N/A") + "{rs}" + Environment.NewLine + "Encoder: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Encoder, "N/A") + "{rs} Rating: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Rating.ToString(), "0") + " / 5{rs}", chat);
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="title" Param="" Desc="Scroll current title" Type="1" New="0" />
                    case "title":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Title", Global.FormatString(p_Playlist.PlayingItem.Title, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="artist" Param="" Desc="Scroll current artist" Type="1" New="0" />
                    case "artist":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Artist", Global.FormatString(p_Playlist.PlayingItem.Artist, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="album" Param="" Desc="Scroll current album" Type="1" New="0" />
                    case "album":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Album", Global.FormatString(p_Playlist.PlayingItem.Album, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="genre" Param="" Desc="Scroll current genre" Type="1" New="0" />
                    case "genre":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Genre", Global.FormatString(p_Playlist.PlayingItem.Genre, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="track" Param="" Desc="Scroll current track" Type="1" New="0" />
                    case "track":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Track", Global.FormatString(p_Playlist.PlayingItem.Track.ToString(), "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="tc/trackcount" Param="" Desc="Scroll current track count" Type="1" New="0" />
                    case "tc":
                    case "trackcount":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Track count", Global.FormatString(p_Playlist.PlayingItem.TrackCount.ToString(), "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="year" Param="" Desc="Scroll current year" Type="1" New="0" />
                    case "year":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Year", Global.FormatString(p_Playlist.PlayingItem.Year.ToString(), "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="encoder" Param="" Desc="Scroll current encoder" Type="1" New="0" />
                    case "encoder":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Encoder", Global.FormatString(p_Playlist.PlayingItem.Encoder, "N/A"), chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="rating" Param="" Desc="Scroll current rating" Type="1" New="0" />
                    case "rating":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendStatus("Rating", Global.FormatString(p_Playlist.PlayingItem.Rating.ToString(), "0") + " / 5", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="info/mpeg" Param="" Desc="Scroll current MPEG info" Type="1" New="0" />
                    case "info":
                    case "mpeg":
                        if (Global.MediaPlayer.IsPlaying(true)) Global.Steam.SendMessage("MPEG layers: {ls}" + p_Playlist.PlayingItem.Layers.ToString() + "{rs} Mode: {ls}" + Global.FormatString(p_Playlist.PlayingItem.Mode, "N/A") + "{rs}" + Environment.NewLine + "Bitrate: {ls}" + Global.FormatBitrate(p_Playlist.PlayingItem) + "{rs} Frequency: {ls}" + p_Playlist.PlayingItem.Frequency.ToString() + " Hz{rs} Size: {ls}" + Global.ConvertBytes(p_Playlist.PlayingItem.Size) + "{rs}" + Environment.NewLine + "Copyright: {ls}" + Global.BoolToString(p_Playlist.PlayingItem.Copyright, "No", "Yes") + "{rs} Original: {ls}" + Global.BoolToString(p_Playlist.PlayingItem.Original, "No", "Yes") + "{rs} CRC: {ls}" + Global.BoolToString(p_Playlist.PlayingItem.Protection, "No", "Yes") + "{rs} VBR: {ls}" + Global.BoolToString(p_Playlist.PlayingItem.VBR, "No", "Yes") + "{rs}", chat);
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="af/addfav" Param="" Desc="Add current file to favorites" Type="1" New="0" />
                    case "af":
                    case "addfav":
                        if (p_StationList.PlayingItem != null)
                        {
                            Global.Steam.SendError("Cannot add stream to favorites", chat);
                            return true;
                        }

                        if (Global.MediaPlayer.IsPlaying(true))
                        {
                            if (p_Playlist.PlayingItem.Favorite) Global.Steam.SendError("Favorite already found", chat);
                            else
                            {
                                int favorites = 0;

                                p_Playlist.PlayingItem.Favorite = true;
                                p_Playlist.UpdateItems();

                                foreach (UI.PlaylistItem item in p_Playlist.Items)
                                {
                                    if (item.Favorite) favorites++;
                                }

                                Global.Steam.SendStatus("Favorite added, Total", Global.FormatNumber(favorites), chat);
                            }
                        }
                        else Global.Steam.SendError("No file playing", chat);
                        break;

                    /// <Command ID="rf/rfav" Param="" Desc="Play random favorite" Type="1" New="1" />
                    case "rf":
                    case "rfav":
                        list = new List<UI.PlaylistItem>();

                        foreach (UI.PlaylistItem item in p_Playlist.Items)
                        {
                            if (item.Favorite) list.Add(item);
                        }

                        if (list.Count > 0) Global.MediaPlayer.Play(list[Global.RandomNumber(list.Count - 1, true)], "Random favorite", chat);
                        else Global.Steam.SendError("No favorites found", chat);

                        list.Clear();
                        break;

                    /// <Command ID="folder" Param="" Desc="Scroll current music folder" Type="1" New="0" />
                    case "folder":
                        Global.Steam.SendStatus("Music folder", Global.Settings.MusicFolder, chat);
                        break;

                    /// <Command ID="filter" Param="" Desc="Scroll current music filter" Type="1" New="0" />
                    case "filter":
                        Global.Steam.SendStatus("Music filter", Global.Settings.MusicFilter, chat);
                        break;

                    /// <Command ID="format" Param="" Desc="Scroll current playlist format" Type="1" New="0" />
                    case "format":
                        Global.Steam.SendStatus("Playlist format", Global.Settings.PlaylistFormat, chat);
                        break;

                    /// <Command ID="device/output" Param="" Desc="Scroll current output device" Type="1" New="1" />
                    case "device":
                    case "output":
                        Global.Steam.SendStatus("Output device", p_OutputDeviceGroup.DropDown.Text, chat);
                        break;

                    /// <Command ID="trig/trigger" Param="" Desc="Scroll current command trigger" Type="4" New="0" />
                    case "trig":
                    case "trigger":
                        Global.Steam.SendStatus("Command trigger", Global.FormatString(Global.Settings.Trigger, "None"), chat);
                        break;

                    /// <Command ID="ls" Param="" Desc="Scroll current left separator" Type="4" New="0" />
                    case "ls":
                        Global.Steam.SendStatus("Left separator", Global.FormatString(Global.Settings.LeftSeparator, "None"), chat);
                        break;

                    /// <Command ID="rs" Param="" Desc="Scroll current right separator" Type="4" New="0" />
                    case "rs":
                        Global.Steam.SendStatus("Right separator", Global.FormatString(Global.Settings.RightSeparator, "None"), chat);
                        break;

                    /// <Command ID="vol/volume" Param="" Desc="Scroll current volume" Type="1" New="0" />
                    case "vol":
                    case "volume":
                        Global.Steam.SendStatus("Volume", Global.MediaPlayer.Volume.ToString() + "%", chat);
                        break;

                    /// <Command ID="files" Param="" Desc="Count all files in playlist" Type="1" New="0" />
                    case "files":
                        long size = 0;
                        long duration = 0;
                        foreach (UI.PlaylistItem item in p_Playlist.Items)
                        {
                            size += item.Size;
                            duration += item.Duration.Ticks;
                        }

                        Global.Steam.SendMessage("Files: {ls}" + Global.FormatNumber(p_Playlist.Items.Count) + "{rs} Size: {ls}" + Global.ConvertBytes(size) + "{rs} Duration: {ls}" + Global.ConvertTicks(duration, false) + "{rs}", chat);
                        break;

                    /// <Command ID="global" Param="" Desc="Enable/disable global commands" Type="4" New="0" />
                    case "global":
                        Global.Settings.GlobalCommands = !Global.Settings.GlobalCommands;
                        p_GlobalCheckBox.Checked = Global.Settings.GlobalCommands;

                        Global.Steam.SendStatus("Global commands", Global.BoolToString(Global.Settings.GlobalCommands, "Disabled", "Enabled"), chat);
                        break;

                    /// <Command ID="silent" Param="" Desc="Enable/disable silent mode" Type="4" New="0" />
                    case "silent":
                        if (Global.Settings.Silent)
                        {
                            Global.Settings.Silent = false;
                            p_SilentCheckBox.Checked = false;

                            Global.Steam.SendStatus("Silent mode", "Disabled", chat);
                        }
                        else
                        {
                            Global.Steam.SendStatus("Silent mode", "Enabled", chat);

                            Global.Settings.Silent = true;
                            p_SilentCheckBox.Checked = true;
                        }
                        break;

                    /// <Command ID="notify" Param="" Desc="Enable/disable media notifications" Type="4" New="1" />
                    case "notify":
                        Global.Settings.Notifications = !Global.Settings.Notifications;
                        p_NotificationsCheckBox.Checked = Global.Settings.Notifications;

                        Global.Steam.SendStatus("Media notifications", Global.BoolToString(Global.Settings.Notifications, "Disabled", "Enabled"), chat);
                        break;

                    /// <Command ID="skin" Param="" Desc="Scroll current skin info" Type="4" New="0" />
                    case "skin":
                        Global.Steam.SendMessage("Current skin: {ls}" + Global.Skin.Name + "{rs} Author: {ls}" + Global.Skin.AuthorPersonaName + "{rs} Created: {ls}" + Global.Skin.Created + "{rs}", chat);
                        break;

                    /// <Command ID="rsk/rskin" Param="" Desc="Load random skin" Type="4" New="1" />
                    case "rsk":
                    case "rskin":
                        if (p_SkinList.SkinDesigner == null || p_SkinList.SkinDesigner.IsDisposed)
                        {
                            UI.Skin skin = p_SkinList.Items[Global.RandomNumber(p_SkinList.Items.Count - 1, true)];

                            Global.Skin = new UI.Skin(skin.URL);
                            ReDraw();

                            Global.Steam.SendMessage("Skin loaded: {ls}" + Global.Skin.Name + "{rs} Author: {ls}" + Global.Skin.AuthorPersonaName + "{rs}", chat);
                        }
                        else Global.Steam.SendError("Unable to load skin", chat);
                        break;

                    /// <Command ID="rpl/rplaylist" Param="" Desc="Load random playlist" Type="1" New="1" />
                    case "rpl":
                    case "rplaylist":
                        if (p_Library.Items.Count > 0)
                        {
                            UI.LibraryItem item = p_Library.Items[Global.RandomNumber(p_Library.Items.Count - 1, true)];

                            p_Playlist.Load(item);

                            Global.Steam.SendMessage("Playlist loaded: {ls}" + item.Text + "{rs} Files: {ls}" + Global.FormatNumber(p_Playlist.Items.Count) + "{rs}", chat);
                        }
                        else Global.Steam.SendError("No playlists found", chat);
                        break;

                    /// <Command ID="gskin/genskin" Param="" Desc="Generate random skin colour" Type="4" New="0" />
                    case "gskin":
                    case "genskin":
                        if (p_SkinList.SkinDesigner == null || p_SkinList.SkinDesigner.IsDisposed)
                        {
                            Global.Skin.Generate();
                            ReDraw();

                            Global.Steam.SendStatus("Random skin generated", Global.Skin.Window.BGColor2.ToString(), chat);
                        }
                        else Global.Steam.SendError("Unable to generate skin", chat);
                        break;

                    /// <Command ID="home/steamp3" Param="" Desc="View STEAMp3 Homepage" Type="4" New="0" />
                    case "home":
                    case "steamp3":
                        Global.Steam.SendStatus("Viewing", "steamp3.ta0soft.com", chat);
                        Process.Start("http://steamp3.ta0soft.com/");
                        break;

                    /// <Command ID="ut/uptime" Param="" Desc="Scroll current Windows up-time" Type="4" New="0" />
                    case "ut":
                    case "uptime":
                        Global.Steam.SendStatus("Windows up-time", Global.ConvertMilliseconds(Environment.TickCount, false), chat);
                        break;

                    /// <Command ID="rt/runtime" Param="" Desc="Scroll STEAMp3 run-time" Type="4" New="0" />
                    case "rt":
                    case "runtime":
                        ts = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
                        Global.Steam.SendStatus("STEAMp3 run-time", Global.ConvertSeconds((int)ts.TotalSeconds, false), chat);
                        break;

                    /// <Command ID="calc/calculator" Param="" Desc="Launch Windows Calculator" Type="4" New="0" />
                    case "calc":
                    case "calculator":
                        Global.Steam.SendStatus("Launching", "Windows Calculator", chat);
                        Process.Start(Environment.SystemDirectory + "\\calc.exe");
                        break;

                    /// <Command ID="cmd" Param="" Desc="Launch DOS Command Prompt" Type="4" New="0" />
                    case "cmd":
                        Global.Steam.SendStatus("Launching", "DOS Command Prompt", chat);
                        Process.Start(Environment.SystemDirectory + "\\cmd.exe");
                        break;

                    /// <Command ID="cp/controlpanel" Param="" Desc="Launch Windows Control Panel" Type="4" New="0" />
                    case "cp":
                    case "controlpanel":
                        Global.Steam.SendStatus("Launching", "Windows Pontrol Panel", chat);
                        Process.Start(Environment.SystemDirectory + "\\control.exe");
                        break;

                    /// <Command ID="char/charmap" Param="" Desc="Launch Windows Character Map" Type="4" New="0" />
                    case "char":
                    case "charmap":
                        Global.Steam.SendStatus("Launching", "Windows Character Map", chat);
                        Process.Start(Environment.SystemDirectory + "\\charmap.exe");
                        break;

                    /// <Command ID="explorer" Param="" Desc="Launch Windows Explorer" Type="4" New="0" />
                    case "explorer":
                        Global.Steam.SendStatus("Launching", "Windows Explorer", chat);
                        Process.Start(Environment.GetEnvironmentVariable("windir") + "\\explorer.exe");
                        break;

                    /// <Command ID="notepad" Param="" Desc="Launch Notepad" Type="4" New="0" />
                    case "notepad":
                        Global.Steam.SendStatus("Launching", "Notepad", chat);
                        Process.Start(Environment.GetEnvironmentVariable("windir") + "\\notepad.exe");
                        break;

                    /// <Command ID="paint/mspaint" Param="" Desc="Launch Microsoft Paint" Type="4" New="0" />
                    case "paint":
                    case "mspaint":
                        Global.Steam.SendStatus("Launching", "Microsoft Paint", chat);
                        Process.Start(Environment.SystemDirectory + "\\mspaint.exe");
                        break;

                    /// <Command ID="regedit" Param="" Desc="Launch Windows Registry Editor" Type="4" New="0" />
                    case "regedit":
                        Global.Steam.SendStatus("Launching", "Windows Registry Editor", chat);
                        Process.Start(Environment.GetEnvironmentVariable("windir") + "\\regedit.exe");
                        break;

                    /// <Command ID="tasks/taskmgr" Param="" Desc="Launch Windows Task Manager" Type="4" New="0" />
                    case "tasks":
                    case "taskmgr":
                        Global.Steam.SendStatus("Launching", "Windows Task Manager", chat);
                        Process.Start(Environment.SystemDirectory + "\\taskmgr.exe");
                        break;

                    /// <Command ID="write/wordpad" Param="" Desc="Launch Wordpad" Type="4" New="0" />
                    case "write":
                    case "wordpad":
                        Global.Steam.SendStatus("Launching", "Wordpad", chat);
                        Process.Start(Environment.SystemDirectory + "\\write.exe");
                        break;

                    /// <Command ID="battery/power" Param="" Desc="Scroll current battery power" Type="4" New="0" />
                    case "battery":
                    case "power":
                        Global.Steam.SendMessage(Global.GetBattery(), chat);
                        break;

                    /// <Command ID="cpu/proc" Param="" Desc="Scroll current processor name" Type="4" New="0" />
                    case "cpu":
                    case "proc":
                        Global.Steam.SendMessage(Global.GetCPU(), chat);
                        break;

                    /// <Command ID="gpu/graphics" Param="" Desc="Scroll current graphics device" Type="4" New="0" />
                    case "gpu":
                    case "graphics":
                        Global.Steam.SendMessage(Global.GetGPU(), chat);
                        break;

                    /// <Command ID="drives" Param="" Desc="Scroll all installed drives" Type="4" New="0" />
                    case "drives":
                        Global.Steam.SendMessage(Global.GetDrives(), chat);
                        break;

                    /// <Command ID="date" Param="" Desc="Scroll current date" Type="4" New="0" />
                    case "date":
                        Global.Steam.SendStatus("Current date", DateTime.Now.ToLongDateString(), chat);
                        break;

                    /// <Command ID="time" Param="" Desc="Scroll current time" Type="4" New="0" />
                    case "time":
                        Global.Steam.SendStatus("Time", DateTime.Now.ToShortTimeString(), chat);
                        break;

                    /// <Command ID="dt/datetime" Param="" Desc="Scroll current date and time" Type="4" New="0" />
                    case "dt":
                    case "datetime":
                        Global.Steam.SendMessage("Date: {ls}" + DateTime.Now.ToLongDateString() + "{rs} Time: {ls}" + DateTime.Now.ToShortTimeString() + "{rs}", chat);
                        break;

                    /// <Command ID="os" Param="" Desc="Scroll current operating system" Type="4" New="0" />
                    case "os":
                        Global.Steam.SendMessage("OS: {ls}" + Global.ReadRegKey(Registry.LocalMachine, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "productname") + "{rs} Build: {ls}" + Environment.OSVersion.Version.ToString() + "{rs}", chat);
                        break;

                    /// <Command ID="processes" Param="" Desc="Scroll total running processes" Type="4" New="0" />
                    case "processes":
                        Global.Steam.SendStatus("Total running processes", Process.GetProcesses().Length.ToString(), chat);
                        break;

                    /// <Command ID="ram/memory" Param="" Desc="Scroll current memory usage" Type="4" New="0" />
                    case "ram":
                    case "memory":
                        Global.Steam.SendMessage(Global.GetRAM(), chat);
                        break;

                    /// <Command ID="usage" Param="" Desc="Scroll STEAMp3 memory usage" Type="4" New="0" />
                    case "usage":
                        Global.Steam.SendMessage(Global.GetUsage(), chat);
                        break;

                    /// <Command ID="res/screen" Param="" Desc="Scroll current screen resolution" Type="4" New="0" />
                    case "res":
                    case "screen":
                        Global.Steam.SendMessage(Global.GetResolution(), chat);
                        break;

                    /// <Command ID="away" Param="" Desc="Set Steam status to Away" Type="2" New="0" />
                    case "away":
                        Global.Steam.Client.SetPersonaState(Steam4NET.EPersonaState.k_EPersonaStateAway);
                        Global.Steam.SendStatus("Steam status set", "Away", chat);
                        break;

                    /// <Command ID="snooze" Param="" Desc="Set Steam status to Snooze" Type="2" New="0" />
                    case "snooze":
                        Global.Steam.Client.SetPersonaState(Steam4NET.EPersonaState.k_EPersonaStateSnooze);
                        Global.Steam.SendStatus("Steam status set", "Snooze", chat);
                        break;

                    /// <Command ID="busy" Param="" Desc="Set Steam status to Busy" Type="2" New="0" />
                    case "busy":
                        Global.Steam.Client.SetPersonaState(Steam4NET.EPersonaState.k_EPersonaStateBusy);
                        Global.Steam.SendStatus("Steam status set", "Busy", chat);
                        break;

                    /// <Command ID="offline" Param="" Desc="Set Steam status to Offline" Type="2" New="0" />
                    case "offline":
                        Global.Steam.Client.SetPersonaState(Steam4NET.EPersonaState.k_EPersonaStateOffline);
                        Global.Steam.SendStatus("Steam status set", "Offline", chat);
                        break;

                    /// <Command ID="online" Param="" Desc="Set Steam status to Online" Type="2" New="0" />
                    case "online":
                        Global.Steam.Client.SetPersonaState(Steam4NET.EPersonaState.k_EPersonaStateOnline);
                        Global.Steam.SendStatus("Steam status set", "Online", chat);
                        break;

                    /// <Command ID="stats" Param="" Desc="Scroll STEAMp3 user stats" Type="3" New="1" />
                    case "stats":
                        ts = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);

                        Global.Steam.SendMessage("Achievements: {ls}" + Global.FormatNumber(AchievementList.GetAchievedCount()) + " / " + Global.FormatNumber(AchievementList.Items.Count) + "{rs} Commands used: {ls}" + Global.FormatNumber(Global.Stats.CommandsUsed) + "{rs}" + Environment.NewLine + "Songs played: {ls}" + Global.FormatNumber(Global.Stats.SongsPlayed) + "{rs} Songs completed: {ls}" + Global.FormatNumber(Global.Stats.SongsCompleted) + "{rs}" + Environment.NewLine + "User since: {ls}" + Global.Stats.UserSince + "{rs} Last online: {ls}" + Global.Stats.LastOnline + "{rs}" + Environment.NewLine + "Play-time: {ls}" + Global.ConvertSeconds(Global.Stats.PlayTime, false) + "{rs} Run-time: {ls}" + Global.ConvertSeconds(Global.Stats.RunTime + (int)ts.TotalSeconds, false) + "{rs}", chat);
                        break;

                    /// <Command ID="users" Param="" Desc="Scroll STEAMp3 user count" Type="3" New="0" />
                    case "users":
                        Global.Steam.SendMessage(Global.GetOnlineUsers(), chat);
                        break;

                    /// <Command ID="wotd" Param="" Desc="Scroll word of the day" Type="3" New="0" />
                    case "wotd":
                        Global.Steam.SendMessage(Global.WordOfTheDay(), chat);
                        break;

                    /// <Command ID="news/rss" Param="" Desc="Scroll latest STEAMp3 news &amp; updates" Type="3" New="1" />
                    case "news":
                    case "rss":
                        Global.Steam.SendMessage(Global.GetNews(), chat);
                        break;

                    /// <Command ID="profile" Param="" Desc="View STEAMp3 profile" Type="3" New="1" />
                    case "profile":
                        response = "http://steamp3.ta0soft.com/profiles/" + Global.GetCommunityID(Global.Steam.Client.GetSteamID());

                        Global.Steam.SendMessage("Viewing STEAMp3 profile for: {ls}" + Global.Steam.Client.GetPersonaName() + "{rs}" + Environment.NewLine + response, chat);
                        Process.Start(response);
                        break;

                    /// <Command ID="release/notes" Param="" Desc="View STEAMp3 release notes" Type="3" New="1" />
                    case "release":
                    case "notes":
                        Global.Steam.SendMessage("Viewing: {ls}Release notes{rs}", chat);
                        Process.Start("http://steamp3.ta0soft.com/liveupdate/");
                        break;

                    /// <Command ID="achieved" Param="" Desc="Scroll earned achievements" Type="3" New="1" />
                    case "achieved":
                        response = "Achievements earned: {ls}" + Global.FormatNumber(AchievementList.GetAchievedCount()) + " / " + Global.FormatNumber(AchievementList.Items.Count) + "{rs}" + Environment.NewLine;

                        foreach (UI.Achievement item in AchievementList.Items)
                        {
                            if (item.Value == item.Maximum) response += item.Name + ", ";
                        }
                        response = response.Substring(0, response.Length - 2);

                        Global.Steam.SendMessage(response, chat);
                        break;

                    default:
                        return false;
                }
            }

            return true;
        }
        #endregion

        #region UpdateControls
        public void UpdateControls()
        {
            p_BackButton.Enabled = p_Playlist.FilteredItems.Count > 0;
            p_PlayButton.Enabled = p_Playlist.FilteredItems.Count > 0;
            p_NextButton.Enabled = p_Playlist.FilteredItems.Count > 0;
            p_RandomButton.Enabled = p_Playlist.FilteredItems.Count > 0;

            p_StopButton.Enabled = Global.MediaPlayer.IsPlaying(true);

            p_RefreshButton.Enabled = p_Playlist.Folders.Count > 0; //?

            if (Global.MediaPlayer.IsPlaying(false))
            {
                p_Playlist.PlayButton.Icon = ";";
                p_Playlist.PlayButton.ToolTipText = "Pause";

                p_SeekBar.Enabled = true;

                Global.MediaPlayer.StartTimers(); //?
            }
            else
            {
                Global.MediaPlayer.StopTimers(); //?

                p_Playlist.PlayButton.Icon = "4";
                p_Playlist.PlayButton.ToolTipText = "Play";

                p_SeekBar.Enabled = false;
                //p_SeekBar.Maximum = 0; //?
            }

            //p_VolumeButton.Enabled = true;
        }
        #endregion
        #endregion
    }
    #endregion

    #region MediaPlayer
    public class MediaPlayer
    {
        #region Enums
        public enum PlayModes : int
        {
            None = 0,
            Continuous = 1,
            Reverse = 2,
            Repeat = 3,
            Shuffle = 4,
        }
        #endregion

        #region Events
        public event EventHandler MediaError;
        public event EventHandler PlayStateChanged;
        public event EventHandler PlayModeChanged;
        public event EventHandler PositionChanged;
        public event EventHandler VolumeChanged;
        //public event EventHandler MediaChanged;
        #endregion

        #region Objects
        private UltraID3 p_ID3;
        private WindowsMediaPlayer p_WMP;
        //private ISoundEngine p_SoundEngine;
        //private ISound p_Sound;
        private PlayModes p_PlayMode;
        private Timer p_Timer, p_Timer2, p_StreamTimer;
        private List<COM.MMDevice> p_Devices;
        private COM.MMDevice p_CurrentDevice, p_DefaultDevice;
        private Shoutcast p_Shoutcast;
        private string p_StreamTitle;
        //private WebClient p_WebClient;
        #endregion

        #region Properties
        public UltraID3 ID3
        {
            get { return p_ID3; }
        }

        public WindowsMediaPlayer WMP
        {
            get { return p_WMP; }
        }

        //public ISoundEngine SoundEngine
        //{
            //get { return p_SoundEngine; }
        //}

        //public ISound Sound
        //{
            //get { return p_Sound; }
        //}

        public PlayModes PlayMode
        {
            get { return p_PlayMode; }
            set
            {
                p_PlayMode = value;
                OnPlayModeChanged(new EventArgs());
            }
        }


        public int Position
        {
            get
            {
                return Convert.ToInt32(p_WMP.controls.currentPosition);
                //if (IsPlaying(true)) return (int)p_Sound.PlayPosition; //?
                //return 0;
            }
            set
            {
                p_WMP.controls.currentPosition = value;
                OnPositionChanged(new EventArgs());
                //if (IsPlaying(true))
                //{
                    //p_Sound.PlayPosition = (uint)value; //?
                    //OnPositionChanged(new EventArgs());
                //}
            }
        }

        public int Duration
        {
            get
            {
                return Convert.ToInt32(p_WMP.currentMedia.duration);
                //if (IsPlaying(true)) return (int)p_Sound.PlayLength;
                //return 0;
            }
        }

        public bool IsOnline
        {
            get
            {
                return p_WMP.isOnline;
                //return true; //?
            }
        }

        public int Volume
        {
            get
            {
                return p_WMP.settings.volume;
                //return (int)(p_SoundEngine.SoundVolume * 100);
            }
            set
            {
                Global.Settings.Volume = value;
                p_WMP.settings.volume = value;
                //p_SoundEngine.SoundVolume = (float)value / 100.0f;
                OnVolumeChanged(new EventArgs());
            }
        }

        public List<COM.MMDevice> Devices
        {
            get { return p_Devices; }
        }

        public COM.MMDevice CurrentDevice
        {
            get { return p_CurrentDevice; }
        }

        public COM.MMDevice DefaultDevice
        {
            get { return p_DefaultDevice; }
        }

        public Shoutcast Shoutcast
        {
            get { return p_Shoutcast; }
        }

        public string StreamTitle
        {
            get { return p_StreamTitle; }
        }
        #endregion

        #region Constructor/Destructor
        public MediaPlayer()
        {
            try
            {
                p_ID3 = new UltraID3();

                p_WMP = new WindowsMediaPlayer();
                p_WMP.MediaError += new _WMPOCXEvents_MediaErrorEventHandler(p_WMP_MediaError);
                p_WMP.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(p_WMP_PlayStateChange);
                p_WMP.settings.autoStart = false;
                p_WMP.settings.enableErrorDialogs = false;
                p_WMP.uiMode = "invisible";
                //p_SoundEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.DefaultOptions);
                //p_Sound = null;

                Volume = Global.Settings.Volume;
                PlayMode = (PlayModes)Global.Settings.PlayMode;

                p_Timer = new Timer();
                p_Timer.Interval = 1000;
                p_Timer.Tick += new EventHandler(p_Timer_Tick);

                p_Timer2 = new Timer();
                p_Timer2.Interval = 100;
                p_Timer2.Tick += new EventHandler(p_Timer2_Tick);

                p_StreamTimer = new Timer();
                p_StreamTimer.Interval = 30000;
                p_StreamTimer.Tick += new EventHandler(p_StreamTimer_Tick);

                p_Devices = new List<COM.MMDevice>();
                p_Shoutcast = null;
                p_StreamTitle = string.Empty;
                //p_WebClient = null;
                
                COM.MMDeviceEnumerator devEnum = new COM.MMDeviceEnumerator();
                COM.MMDeviceCollection devCollection = devEnum.EnumerateAudioEndPoints(COM.EDataFlow.eRender, COM.EDeviceState.DEVICE_STATE_ACTIVE);

                p_CurrentDevice = devEnum.GetDefaultAudioEndpoint(COM.EDataFlow.eRender, COM.ERole.eMultimedia);
                p_DefaultDevice = p_CurrentDevice;

                p_Devices.Add(p_DefaultDevice);

                for (int i = 0; i < devCollection.Count; i++)
                {
                    if (devCollection[i].ID != p_DefaultDevice.ID) p_Devices.Add(devCollection[i]);
                }
            }
            catch { }
        }

        public void Dispose()
        {
            Global.Settings.PlayMode = (int)p_PlayMode;

            //if (p_WebClient != null) p_WebClient.Dispose();
            p_StreamTitle = string.Empty;
            if (p_Shoutcast != null) p_Shoutcast.Dispose();
            p_DefaultDevice = null;
            p_CurrentDevice = null;
            p_Devices.Clear();
            p_Timer2.Dispose();
            p_Timer.Dispose();
            p_PlayMode = PlayModes.None;
            DisposeMediaPlayer();
            DisposeID3();
        }
        #endregion

        #region Child Events
        private void p_WMP_MediaError(object pMediaObject)
        {
            OnMediaError(new EventArgs());
        }

        private void p_WMP_PlayStateChange(int NewState)
        {
            switch ((WMPPlayState)NewState)
            {
                case WMPPlayState.wmppsPaused:
                case WMPPlayState.wmppsPlaying:
                    Global.MainWindow.UpdateControls();
                    break;
            }

            OnPlayStateChanged(new EventArgs());
        }

        private void p_Timer_Tick(object sender, EventArgs e)
        {
            if (IsPlaying(false))
            {
                Global.Stats.PlayTime++;
                Global.MainWindow.AchievementList.IncreaseRadioStar(Global.MainWindow.StationList.PlayingItem); //?

                //if (Global.MainWindow.Playlist.PlayingItem.Duration.TotalSeconds == 0) Global.MainWindow.Playlist.PlayingItem.Duration = new TimeSpan(0, 0, 0, Duration);

                Global.MainWindow.SeekBar.Maximum = Duration;
                Global.MainWindow.SeekBar.Value = Position;
                OnPositionChanged(new EventArgs());
            }
            else //if (p_WMP.playState == WMPPlayState.wmppsStopped || p_WMP.playState == WMPPlayState.wmppsReady)
            {
                Global.Stats.SongsCompleted++; //?

                switch (p_PlayMode)
                {
                    case PlayModes.None:
                        Stop(string.Empty, true);
                        break;
                    case PlayModes.Continuous:
                        if (Global.Settings.Notifications) Next("Continuous", true);
                        else Next(string.Empty, true);
                        break;
                    case PlayModes.Reverse:
                        if (Global.Settings.Notifications) Previous("Reverse", true);
                        else Previous(string.Empty, true);
                        break;
                    case PlayModes.Shuffle:
                        if (Global.Settings.Notifications) Random("Shuffle", true);
                        else Random(string.Empty, true);
                        break;
                    case PlayModes.Repeat:
                        if (Global.Settings.Notifications) Play(Global.MainWindow.Playlist.PlayingItem, "Repeat", true);
                        else Play(Global.MainWindow.Playlist.PlayingItem, string.Empty, true);
                        break;
                }
            }
        }

        private void p_Timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (p_CurrentDevice == null) return;

                Global.MainWindow.SeekBar.PeakLeft = (int)(p_CurrentDevice.AudioMeterInformation.PeakValues[0] * 100);
                Global.MainWindow.SeekBar.PeakRight = (int)(p_CurrentDevice.AudioMeterInformation.PeakValues[1] * 100);
            }
            catch { }
        }

        private void p_StreamTimer_Tick(object sender, EventArgs e)
        {
            if (Global.MainWindow.StationList.PlayingItem == null) return;

            p_Shoutcast = new Shoutcast(Global.MainWindow.StationList.PlayingItem.StreamURL, 15000);
            p_Shoutcast.TitleReceived += new Shoutcast.TitleReceivedEventHandler(p_Shoutcast_TitleReceived);
            p_Shoutcast.Connect(true);
        }

        private void p_Shoutcast_TitleReceived(object sender, string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title)) return;
                if (Global.MainWindow.StationList.PlayingItem == null) return;

                if (string.IsNullOrEmpty(p_StreamTitle) || !p_StreamTitle.Equals(title))
                {
                    p_StreamTitle = title;

                    string art = string.Empty;
                    string tit = p_StreamTitle;

                    if (p_StreamTitle.Contains(" - "))
                    {
                        art = Global.LeftOf(p_StreamTitle, " - ");
                        tit = Global.RightOf(p_StreamTitle, " - ");
                    }

                    Global.Stats.SongsPlayed++;
                    Global.Stats.SongsCompleted++; //?

                    UI.PlaylistItem item = new UI.PlaylistItem(Global.MainWindow.StationList.PlayingItem.StreamURL, art, tit, Global.MainWindow.StationList.PlayingItem.Genre, 0);

                    item.Update();

                    Global.MainWindow.Playlist.PlayingItem = item;

                    Global.MainWindow.AchievementList.IncreaseJukeboxHero(item); //?

                    if (Global.Settings.Notifications) Global.Steam.SendMessage("Streaming: {ls}" + item.Text + "{rs}", true);

                    //if (p_WebClient != null && p_WebClient.IsBusy) p_WebClient.CancelAsync(); //?
                    WebClient wc = new WebClient();
                    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    wc.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                    string data = "update=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&cu=" + Global.Stats.CommandsUsed.ToString() + "&pt=" + Global.Stats.PlayTime.ToString() + "&so=" + HttpUtility.UrlEncode(item.Text) + "&sc=" + Global.Stats.SongsCompleted.ToString() + "&sp=" + Global.Stats.SongsPlayed.ToString();

                    foreach (UI.Achievement item2 in Global.MainWindow.AchievementList.Items)
                    {
                        data += item2.Save();
                    }

                    wc.UploadStringAsync(new Uri("http://steamp3.ta0soft.com/stats.php"), "POST", data);
                    wc.Dispose();
                }
            }
            catch { }
        }
        #endregion

        #region Public Methods
        public bool IsPlaying(bool orPaused)
        {
            if (orPaused)
            {
                //if (p_Sound != null) return true;
                if (p_WMP.playState == WMPPlayState.wmppsPlaying || p_WMP.playState == WMPPlayState.wmppsPaused) return true;
            }
            else
            {
                //if (p_Sound != null && !p_Sound.Paused) return true;
                if (p_WMP.playState == WMPPlayState.wmppsPlaying) return true;
            }

            return false;
        }

        public bool IsPaused()
        {
            if (p_WMP.playState == WMPPlayState.wmppsPaused) return true;
            //if (p_Sound != null && p_Sound.Paused) return true;

            return false;
        }

        public bool Next(string status, bool chat)
        {
            int index = 0;

            if (IsPlaying(true)) index = Global.MainWindow.Playlist.PlayingIndex + 1;
            else index = Global.MainWindow.Playlist.SelectedIndex + 1;

            if (index > Global.MainWindow.Playlist.FilteredItems.Count - 1) index = 0;

            return Play(index, status, chat);
        }

        public bool Pause(string status, bool chat)
        {
            if (IsPlaying(false))
            {
                p_WMP.controls.pause();
                //p_Sound.Paused = true;

                //UpdateControls();

                if (!string.IsNullOrEmpty(status)) Global.Steam.SendMessage(status + ": {ls}" + Global.MainWindow.Playlist.PlayingItem.Text + "{rs}", chat);

                return true;
            }

            return false;
        }

        public bool Play(string status, bool chat)
        {
            int index = 0;

            if (IsPlaying(false)) index = Global.MainWindow.Playlist.PlayingIndex;
            else index = Global.MainWindow.Playlist.SelectedIndex;

            //if (index < 0) index = 0;

            return Play(index, status, chat);
        }

        public bool Play(int index, string status, bool chat)
        {
            if (index < 0 || index > Global.MainWindow.Playlist.FilteredItems.Count - 1) return false;

            return Play(Global.MainWindow.Playlist.FilteredItems[index], status, chat);
        }

        public bool Play(UI.PlaylistItem item, string status, bool chat)
        {
            try
            {
                if (Global.MainWindow.Playlist.FilteredItems.Count == 0)
                {
                    Global.Steam.SendError("No files found", chat);
                    return false;
                }

                Stop(string.Empty, true);
                //Pause(string.Empty, true);
                Global.MainWindow.SeekBar.Maximum = 0; //?

                if (!File.Exists(item.URL))
                {
                    Global.Steam.SendError("File moved/deleted", chat);
                    return false;
                }

                item.Update();

                Global.MainWindow.Playlist.PlayingItem = item;
                Global.MainWindow.Playlist.SelectedItem = item;

                Global.MainWindow.AchievementList.IncreaseJukeboxHero(item);
                Global.MainWindow.AchievementList.IncreaseLeetHaxor(item);
                Global.MainWindow.AchievementList.IncreaseMP3Playah(item);
                Global.MainWindow.AchievementList.IncreaseRepetition(item, status);
                Global.MainWindow.AchievementList.IncreaseShortStop(item);

                Global.Stats.SongsPlayed++; //?

                p_WMP.URL = item.URL;
                p_WMP.controls.play();
                //p_Sound = p_SoundEngine.Play2D(item.URL, false);

                if (!string.IsNullOrEmpty(status)) Global.Steam.SendMessage(status + ": {ls}" + item.Text + "{rs}", chat);

                //UpdateControls();

                //if (p_WebClient != null && p_WebClient.IsBusy) p_WebClient.CancelAsync(); //?
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                string data = "update=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&cu=" + Global.Stats.CommandsUsed.ToString() + "&pt=" + Global.Stats.PlayTime.ToString() + "&so=" + HttpUtility.UrlEncode(item.Text) + "&sc=" + Global.Stats.SongsCompleted.ToString() + "&sp=" + Global.Stats.SongsPlayed.ToString();

                foreach (UI.Achievement item2 in Global.MainWindow.AchievementList.Items)
                {
                    data += item2.Save();
                }

                wc.UploadStringAsync(new Uri("http://steamp3.ta0soft.com/stats.php"), "POST", data);
                wc.Dispose();

                return true;
            }
            catch
            {
                return false; //?
            }
        }

        public bool Previous(string status, bool chat)
        {
            int index = 0;

            if (IsPlaying(true)) index = Global.MainWindow.Playlist.PlayingIndex - 1;
            else index = Global.MainWindow.Playlist.SelectedIndex - 1;

            if (index < 0) index = Global.MainWindow.Playlist.FilteredItems.Count - 1;

            return Play(index, status, chat);
        }

        public bool Random(string status, bool chat)
        {
            return Play(Global.RandomNumber(Global.MainWindow.Playlist.FilteredItems.Count - 1, true), status, chat);
        }

        public bool Resume(string status, bool chat)
        {
            if (IsPaused())
            {
                p_WMP.controls.play();
                //p_Sound.Paused = false;

                if (!string.IsNullOrEmpty(status)) Global.Steam.SendMessage(status + ": {ls}" + Global.MainWindow.Playlist.PlayingItem.Text + "{rs}", chat);

                //UpdateControls();
                return true;
            }

            return false;
        }

        public bool Stop(string status, bool chat)
        {
            p_WMP.controls.stop();
            p_WMP.URL = string.Empty;
            p_WMP.close();
            //if (p_Sound != null)
            //{
                //p_Sound.Stop();
                //p_Sound.Dispose();
                //p_Sound = null; //?
            //}

            Global.MainWindow.UpdateControls();
            Global.MainWindow.SeekBar.Maximum = 0; //?
            Global.MainWindow.SeekBar.PeakLeft = 0;
            Global.MainWindow.SeekBar.PeakRight = 0;

            if (!string.IsNullOrEmpty(status)) Global.Steam.SendMessage(status + ": {ls}" + Global.MainWindow.Playlist.PlayingItem.Text + "{rs}", chat);

            Global.MainWindow.Playlist.PlayingItem = null;
            Global.MainWindow.StationList.PlayingItem = null;
            return true;
        }

        public bool Stream(UI.Station station, string status, bool chat)
        {
            Stop(string.Empty, true);
            //Pause(string.Empty, true);
            Global.MainWindow.SeekBar.Maximum = 0; //?

            Global.MainWindow.Playlist.PlayingItem = new UI.PlaylistItem(station.StreamURL, string.Empty, station.Name, station.Genre, 0);
            Global.MainWindow.StationList.PlayingItem = station;

            p_WMP.URL = station.StreamURL;
            p_WMP.controls.play();
            //p_Sound = p_SoundEngine.Play2D(station.StreamURL, false, false, StreamMode.Streaming);

            Global.MainWindow.TabContainer.SelectItemByText("Playlist"); //?

            if (!string.IsNullOrEmpty(status)) Global.Steam.SendMessage(status + ": {ls}" + station.Name + "{rs}", chat);

            //UpdateControls();

            p_StreamTimer_Tick(this, new EventArgs());

            return true;
        }

        public bool ChangeDevice(int index)
        {
            try
            {
                if (p_Devices.Count == 0) return false;
                if (index < 0 || index > p_Devices.Count - 1) index = 0;

                COM.IWMPAudioRenderConfig ac = p_WMP as COM.IWMPAudioRenderConfig;
                if (ac != null)
                {
                    ac.put_audioOutputDevice(p_Devices[index].ID);
                    p_CurrentDevice = p_Devices[index];
                    return true;
                }
                //p_SoundEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.DefaultOptions, p_Devices[index].ID);
            }
            catch { }

            return false;
        }

        public string[] GetDeviceNames()
        {
            try
            {
                List<string> names = new List<string>();

                foreach (COM.MMDevice device in p_Devices)
                {
                    names.Add(device.FriendlyName);
                }

                if (names.Count == 0) names.Add("Speakers");

                return names.ToArray();
            }
            catch
            {
                return new string[] { "Speakers" };
            }
        }

        public void StartTimers()
        {
            p_Timer.Start();
            p_Timer2.Start();
            p_StreamTimer.Start();
        }

        public void StopTimers()
        {
            p_Timer.Stop();
            p_Timer2.Stop();
            p_StreamTimer.Stop();
        }
        #endregion

        #region Private Methods
        private void DisposeMediaPlayer()
        {
            if (IsPlaying(true)) Stop(string.Empty, true);
            p_WMP.close();
            p_WMP = null;
            //if (p_Sound != null)
            //{
                //p_Sound.Dispose(); //?
                //p_Sound = null;
            //}
            //p_SoundEngine = null; //?

            System.Threading.Thread.Sleep(500);
        }

        private void DisposeID3()
        {
            //Global.Settings.Playlist.Clear();

            foreach (UI.PlaylistItem item in Global.MainWindow.Playlist.Items)
            {
                //Global.Settings.Playlist.Add(item);

                if (item.Updated)
                {
                    int retries = 0;

                    while (Global.InUse(item.URL))
                    {
                        System.Threading.Thread.Sleep(500);

                        retries++;
                        if (retries > 10) break;
                    }

                    try
                    {
                        p_ID3.Read(item.URL);
                        p_ID3.ID3v2Tag.Artist = item.Artist;
                        p_ID3.ID3v2Tag.Title = item.Title;
                        p_ID3.ID3v2Tag.Album = item.Album;
                        p_ID3.ID3v2Tag.Genre = item.Genre;
                        p_ID3.ID3v2Tag.TrackNum = item.Track;
                        p_ID3.ID3v2Tag.TrackCount = item.TrackCount;
                        p_ID3.ID3v2Tag.Year = item.Year;

                        ID3FrameCollection frames = p_ID3.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Popularimeter);
                        if (frames.Count > 0)
                        {
                            ID3v23PopularimeterFrame ratingFrame = (ID3v23PopularimeterFrame)frames[0];

                            switch (item.Rating.ToString())
                            {
                                case "0.0":
                                    ratingFrame.Rating = null;
                                    break;
                                case "0.5":
                                    ratingFrame.Rating = 2;
                                    break;
                                case "1.0":
                                    ratingFrame.Rating = 53;
                                    break;
                                case "1.5":
                                    ratingFrame.Rating = 73;
                                    break;
                                case "2.0":
                                    ratingFrame.Rating = 104;
                                    break;
                                case "2.5":
                                    ratingFrame.Rating = 124;
                                    break;
                                case "3.0":
                                    ratingFrame.Rating = 154;
                                    break;
                                case "3.5":
                                    ratingFrame.Rating = 174;
                                    break;
                                case "4.0":
                                    ratingFrame.Rating = 205;
                                    break;
                                case "4.5":
                                    ratingFrame.Rating = 225;
                                    break;
                                case "5.0":
                                    ratingFrame.Rating = 255;
                                    break;
                            }
                        }

                        ID3v23PlayCounterFrame playCountFrame = (ID3v23PlayCounterFrame)p_ID3.ID3v2Tag.Frames.GetFrame(CommonSingleInstanceID3v2FrameTypes.PlayCounter);
                        if (playCountFrame != null) playCountFrame.Counter = item.PlayCount;

                        p_ID3.Write();
                    }
                    catch { }
                }
            }

            //p_ID3.Clear();
            p_ID3 = null;
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnMediaError(EventArgs e)
        {
            if (MediaError != null) MediaError.Invoke(this, e);
        }

        protected virtual void OnPlayStateChanged(EventArgs e)
        {
            if (PlayStateChanged != null) PlayStateChanged.Invoke(this, e);
        }

        protected virtual void OnPlayModeChanged(EventArgs e)
        {
            if (PlayModeChanged != null) PlayModeChanged.Invoke(this, e);
        }

        protected virtual void OnPositionChanged(EventArgs e)
        {
            if (PositionChanged != null) PositionChanged.Invoke(this, e);
        }

        protected virtual void OnVolumeChanged(EventArgs e)
        {
            if (VolumeChanged != null) VolumeChanged.Invoke(this, e);
        }

        //protected virtual void OnMediaChanged(EventArgs e)
        //{
        //if (MediaChanged != null) MediaChanged.Invoke(this, e);
        //}
        #endregion
    }
    #endregion

    #region Settings
    public class Settings
    {
        #region Enums
        public enum ImageLocations : int
        {
            None,
            LastFM,
            GoogleImages,
            ID3,
        }

        public enum ImageQualities : int
        {
            Low,
            Medium,
            High,
        }
        #endregion

        #region Objects
        private string p_URL;
        private bool p_GlobalCommands;
        private ImageLocations p_ImageLocation;
        private ImageQualities p_ImageQuality;
        private string p_MusicFilter, p_MusicFolder;
        private bool p_Notifications;
        private int p_OutputDevice;
        private string p_PlaylistFormat;
        //private List<UI.PlaylistItem> p_Playlist;
        private string p_PlaylistURL;
        private int p_PlayMode;
        private List<UI.Station> p_RadioStations;
        private string p_LeftSeparator, p_RightSeparator;
        private bool p_Silent;
        private string p_Trigger;
        private int p_Volume;
        private int p_WindowWidth, p_WindowHeight;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public bool GlobalCommands
        {
            get { return p_GlobalCommands; }
            set { p_GlobalCommands = value; }
        }

        public ImageLocations ImageLocation
        {
            get { return p_ImageLocation; }
            set { p_ImageLocation = value; }
        }

        public ImageQualities ImageQuality
        {
            get { return p_ImageQuality; }
            set { p_ImageQuality = value; }
        }

        public string MusicFilter
        {
            get { return p_MusicFilter; }
            set { p_MusicFilter = value; }
        }

        public string MusicFolder
        {
            get { return p_MusicFolder; }
            set { p_MusicFolder = value; }
        }

        public bool Notifications
        {
            get { return p_Notifications; }
            set { p_Notifications = value; }
        }

        public int OutputDevice
        {
            get { return p_OutputDevice; }
            set { p_OutputDevice = value; }
        }

        public string PlaylistFormat
        {
            get { return p_PlaylistFormat; }
            set { p_PlaylistFormat = value; }
        }

        //public List<UI.PlaylistItem> Playlist
        //{
            //get { return p_Playlist; }
            //set { p_Playlist = value; }
        //}

        public string PlaylistURL
        {
            get { return p_PlaylistURL; }
            set { p_PlaylistURL = value; }
        }

        public int PlayMode
        {
            get { return p_PlayMode; }
            set { p_PlayMode = value; }
        }

        public List<UI.Station> RadioStations
        {
            get { return p_RadioStations; }
            set { p_RadioStations = value; }
        }

        public string LeftSeparator
        {
            get { return p_LeftSeparator; }
            set { p_LeftSeparator = value; }
        }

        public string RightSeparator
        {
            get { return p_RightSeparator; }
            set { p_RightSeparator = value; }
        }

        public bool Silent
        {
            get { return p_Silent; }
            set { p_Silent = value; }
        }

        public string Trigger
        {
            get { return p_Trigger; }
            set { p_Trigger = value; }
        }

        public int Volume
        {
            get { return p_Volume; }
            set { p_Volume = value; }
        }

        public int WindowWidth
        {
            get { return p_WindowWidth; }
            set { p_WindowWidth = value; }
        }

        public int WindowHeight
        {
            get { return p_WindowHeight; }
            set { p_WindowHeight = value; }
        }
        #endregion

        #region Constructor/Destructor
        public Settings(string url)
        {
            if (!Load(url)) Default();
        }

        public void Dispose(bool save)
        {
            if (save) Save();

            p_WindowHeight = 0;
            p_WindowWidth = 0;
            p_Volume = 0;
            p_Trigger = string.Empty;
            p_Silent = false;
            p_RightSeparator = string.Empty;
            p_LeftSeparator = string.Empty;
            p_RadioStations.Clear();
            p_PlayMode = 0;
            p_PlaylistURL = string.Empty;

            //foreach (UI.PlaylistItem item in p_Playlist)
            //{
                //item.Dispose();
            //}
            //p_Playlist.Clear();

            p_PlaylistFormat = string.Empty;
            p_OutputDevice = 0;
            p_Notifications = false;
            p_MusicFolder = string.Empty;
            p_MusicFilter = string.Empty;
            p_ImageQuality = ImageQualities.Low;
            p_ImageLocation = ImageLocations.None;
            p_GlobalCommands = false;

            p_URL = string.Empty;
        }
        #endregion

        #region Public Methods
        public void Default()
        {
            //p_URL = string.Empty;
            p_GlobalCommands = true;
            p_ImageLocation = ImageLocations.LastFM;
            p_ImageQuality = ImageQualities.Medium;
            p_MusicFilter = ".mp3;.m4a;.wav;.wma;.ogg;.flac";
            p_MusicFolder = p_MusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            p_Notifications = false;
            p_OutputDevice = 0;
            p_PlaylistFormat = "{Artist} - {Title}";
            //p_Playlist = new List<UI.PlaylistItem>();
            p_PlaylistURL = string.Empty;
            p_PlayMode = 1;
            p_RadioStations = new List<UI.Station>();
            p_LeftSeparator = "[";
            p_RightSeparator = "]";
            p_Silent = false;
            p_Trigger = string.Empty;
            p_Volume = 100;
            p_WindowWidth = 680;
            p_WindowHeight = 472;
        }

        public bool Load(string url)
        {
            p_URL = url;

            if (!File.Exists(p_URL)) return false;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(p_URL);
                XmlNode node = xml.SelectSingleNode("Steamp3.Settings");

                p_GlobalCommands = Global.StringToBool(Global.GetXmlValue(node.SelectSingleNode("GlobalCommands"), string.Empty, "1"), "1");
                p_ImageLocation = (ImageLocations)Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("Image"), "Location", "1"));
                p_ImageQuality = (ImageQualities)Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("Image"), "Quality", "1"));
                p_MusicFilter = Global.GetXmlValue(node.SelectSingleNode("Music"), "Filter", ".mp3;.m4a;.wav;.wma;.ogg;.flac");
                p_MusicFolder = Global.GetXmlValue(node.SelectSingleNode("Music"), "Folder", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
                p_Notifications = Global.StringToBool(Global.GetXmlValue(node.SelectSingleNode("Notifications"), string.Empty, "1"), "1");
                p_OutputDevice = Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("OutputDevice"), string.Empty, "0"));
                p_PlaylistFormat = Global.GetXmlValue(node.SelectSingleNode("Playlist"), "Format", "{Artist} - {Title}");
                //LoadPlaylist(node.SelectSingleNode("Playlist"));
                p_PlaylistURL = Global.GetXmlValue(node.SelectSingleNode("Playlist"), "URL", string.Empty);
                p_PlayMode = Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("PlayMode"), string.Empty, "1"));
                LoadRadioStations(node.SelectSingleNode("Radio"));
                p_LeftSeparator = Global.GetXmlValue(node.SelectSingleNode("Separator"), "Left", "[");
                p_RightSeparator = Global.GetXmlValue(node.SelectSingleNode("Separator"), "Right", "]");
                p_Silent = Global.StringToBool(Global.GetXmlValue(node.SelectSingleNode("Silent"), string.Empty, "0"), "1");
                p_Trigger = Global.GetXmlValue(node.SelectSingleNode("Trigger"), string.Empty, string.Empty);
                p_Volume = Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("Volume"), string.Empty, "100"));
                p_WindowWidth = Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("Window"), "Width", "680"));
                p_WindowHeight = Global.StringToInt(Global.GetXmlValue(node.SelectSingleNode("Window"), "Height", "472"));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
                xml.AppendChild(declaration);

                XmlElement root = xml.CreateElement("Steamp3.Settings");
                xml.AppendChild(root);

                XmlElement element = xml.CreateElement("GlobalCommands");
                element.InnerText = Global.BoolToString(p_GlobalCommands, "0", "1");
                root.AppendChild(element);

                element = xml.CreateElement("Image");
                element.SetAttribute("Location", ((int)p_ImageLocation).ToString());
                element.SetAttribute("Quality", ((int)p_ImageQuality).ToString());
                root.AppendChild(element);

                element = xml.CreateElement("Music");
                element.SetAttribute("Filter", p_MusicFilter);
                element.SetAttribute("Folder", p_MusicFolder);
                root.AppendChild(element);

                element = xml.CreateElement("Notifications");
                element.InnerText = Global.BoolToString(p_Notifications, "0", "1");
                root.AppendChild(element);

                element = xml.CreateElement("OutputDevice");
                element.InnerText = p_OutputDevice.ToString();
                root.AppendChild(element);

                element = xml.CreateElement("Playlist");
                element.SetAttribute("Format", p_PlaylistFormat);
                element.SetAttribute("URL", p_PlaylistURL);
                //foreach (UI.PlaylistItem item in p_Playlist)
                //{
                    //element.AppendChild(item.ToXml(xml));
                //}
                root.AppendChild(element);

                element = xml.CreateElement("PlayMode");
                element.InnerText = p_PlayMode.ToString();
                root.AppendChild(element);

                element = xml.CreateElement("Radio");
                foreach (UI.Station item in p_RadioStations)
                {
                    element.AppendChild(item.ToXml(xml));
                }
                root.AppendChild(element);

                element = xml.CreateElement("Separator");
                element.SetAttribute("Left", p_LeftSeparator);
                element.SetAttribute("Right", p_RightSeparator);
                root.AppendChild(element);

                element = xml.CreateElement("Silent");
                element.InnerText = Global.BoolToString(p_Silent, "0", "1");
                root.AppendChild(element);

                element = xml.CreateElement("Trigger");
                element.InnerText = p_Trigger;
                root.AppendChild(element);

                element = xml.CreateElement("Volume");
                element.InnerText = p_Volume.ToString();
                root.AppendChild(element);

                element = xml.CreateElement("Window");
                element.SetAttribute("Width", p_WindowWidth.ToString());
                element.SetAttribute("Height", p_WindowHeight.ToString());
                root.AppendChild(element);

                return Global.SaveXml(xml, p_URL, Encoding.UTF8);
            }
            catch
            {
                return false;
            }
        }

        public void Synchronize()
        {
            //if (File.Exists(p_URL)) File.Delete(p_URL);

            Default();

            Save();
        }
        #endregion

        #region Private Methods
        //private void LoadPlaylist(XmlNode node)
        //{
            //p_Playlist = new List<UI.PlaylistItem>();

            //foreach (XmlNode node2 in node.SelectNodes("File"))
            //{
                //p_Playlist.Add(new UI.PlaylistItem(node2.OuterXml, true));
            //}
        //}

        private void LoadRadioStations(XmlNode node)
        {
            p_RadioStations = new List<UI.Station>();

            foreach (XmlNode node2 in node.SelectNodes("Station"))
            {
                p_RadioStations.Add(new UI.Station(Global.GetXmlValue(node2, "URL", "N/A"), Global.GetXmlValue(node2, "Name", "N/A"), Global.GetXmlValue(node2, "Genre", "N/A"), true));
            }
        }
        #endregion
    }
    #endregion

    #region Shoutcast
    public class Shoutcast
    {
        #region Structures
        private struct RequestState
        {
            public const int BufferSize = 512;
            public byte[] Buffer;
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Stream ResponseStream;
            public int MetaInt;
            public int MetaLength;
            public string MetaHeader;
            public int MetaCount;

            public RequestState(HttpWebRequest request)
            {
                Buffer = new byte[BufferSize];
                Request = request;
                Response = null;
                ResponseStream = null;
                MetaInt = 0;
                MetaLength = 0;
                MetaHeader = string.Empty;
                MetaCount = 0;
            }
        }
        #endregion

        #region Delegates
        public delegate void HeaderReceivedEventHandler(object sender, int metaInt, string name, string genre, string url);
        public delegate void TitleReceivedEventHandler(object sender, string title);
        #endregion

        #region Events
        public event HeaderReceivedEventHandler HeaderReceived;
        public event TitleReceivedEventHandler TitleReceived;
        #endregion

        #region Objects
        private static System.Threading.ManualResetEvent AllDone = new System.Threading.ManualResetEvent(false);
        private string p_URL;
        private int p_Timeout;
        #endregion

        #region Properties
        public string URL
        {
            get { return p_URL; }
        }

        public int Timeout
        {
            get { return p_Timeout; }
        }
        #endregion

        #region Constructor/Destructor
        public Shoutcast(string url, int timeout)
        {
            p_URL = url;
            p_Timeout = timeout;
        }

        public virtual void Dispose()
        {
            p_Timeout = 0;
            p_URL = string.Empty;
        }
        #endregion

        #region Public Methods
        public void Connect(bool async)
        {
            try
            {
                if (string.IsNullOrEmpty(p_URL)) return;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p_URL);
                request.Headers.Clear();
                request.Headers.Add("GET", "/ HTTP/1.0");
                request.Headers.Add("Icy-MetaData", "1");
                request.UserAgent = "WinampMPEG/5.09";
                request.KeepAlive = false;

                if (async)
                {
                    RequestState requestState = new RequestState(request);

                    // Start the asynchronous request.
                    IAsyncResult result = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);

                    // If there is a timeout, the callback fires and the request becomes aborted.
                    System.Threading.ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new System.Threading.WaitOrTimerCallback(TimeoutCallback), request, p_Timeout, true);

                    // The response came in the allowed time.
                    AllDone.WaitOne();

                    //if (requestState.ResponseStream != null) requestState.ResponseStream.Close();
                    if (requestState.Response != null) requestState.Response.Close();
                }
                else
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusDescription == "OK" && response.Headers.Count > 0)
                    {
                        int metaInt = Global.StringToInt(response.GetResponseHeader("icy-metaint"));
                        string name = response.GetResponseHeader("icy-name");
                        string genre = response.GetResponseHeader("icy-genre");
                        string url = response.GetResponseHeader("icy-url");

                        OnHeaderReceived(metaInt, name, genre, url);
                    }

                    response.Close();
                }
            }
            catch { }
        }
        #endregion

        #region Private Methods
        private void ResponseCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)requestState.Request.EndGetResponse(ar);

                requestState.Response = response;

                if (response.StatusDescription == "OK" && response.Headers.Count > 0)
                {
                    int metaInt = Global.StringToInt(response.GetResponseHeader("icy-metaint"));
                    string name = response.GetResponseHeader("icy-name");
                    string genre = response.GetResponseHeader("icy-genre");
                    string url = response.GetResponseHeader("icy-url");

                    OnHeaderReceived(metaInt, name, genre, url);

                    if (metaInt > 0)
                    {
                        Stream responseStream = response.GetResponseStream();

                        requestState.ResponseStream = responseStream;
                        requestState.MetaInt = metaInt;

                        responseStream.BeginRead(requestState.Buffer, 0, RequestState.BufferSize, new AsyncCallback(ReadCallback), requestState);
                        return;
                    }
                }
            }
            catch { }

            AllDone.Set();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                Stream responseStream = requestState.ResponseStream;
                int bytes = responseStream.EndRead(ar);

                if (bytes > 0)
                {
                    for (int i = 0; i < bytes; i++)
                    {
                        if (requestState.MetaLength != 0)
                        {
                            requestState.MetaHeader += Convert.ToChar(requestState.Buffer[i]);
                            requestState.MetaLength--;

                            if (requestState.MetaLength <= 0)
                            {
                                string title = Regex.Match(requestState.MetaHeader, "(StreamTitle=')(.*)(';StreamUrl)").Groups[2].Value.Trim();

                                responseStream.Close(); //?
                                AllDone.Set(); //?

                                OnTitleReceived(title);
                                return;
                            }
                        }
                        else
                        {
                            if (Math.Max(System.Threading.Interlocked.Increment(ref requestState.MetaCount), requestState.MetaCount - 1) >= requestState.MetaInt)
                            {
                                requestState.MetaLength = Convert.ToInt32(requestState.Buffer[i]) * 16;
                                requestState.MetaCount = 0;
                            }
                        }
                    }

                    responseStream.BeginRead(requestState.Buffer, 0, RequestState.BufferSize, new AsyncCallback(ReadCallback), requestState);
                    return;
                }
                else
                {
                    responseStream.Close();
                }
            }
            catch { }

            AllDone.Set();
        }

        private void TimeoutCallback(object state, bool timedOut)
        {
            // Abort the request if the timer fires.
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null) request.Abort();
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnHeaderReceived(int metaInt, string name, string genre, string url)
        {
            if (HeaderReceived != null) HeaderReceived.Invoke(this, metaInt, name, genre, url);
        }

        protected virtual void OnTitleReceived(string title)
        {
            if (TitleReceived != null) TitleReceived.Invoke(this, title);
        }
        #endregion
    }
    #endregion

    #region Stats
    public class Stats
    {
        #region Objects
        internal int p_CommandsUsed;
        internal int p_LastOnline;
        internal int p_Loads;
        internal bool p_Online;
        internal int p_PlayTime;
        internal int p_RunTime;
        internal string p_Skin;
        internal int p_SongsCompleted;
        internal int p_SongsPlayed;
        internal int p_UserSince;

        internal int p_ChattyCathy;
        internal int p_JukeboxHero;
        internal int p_LeetHaxor;
        internal int p_MP3Playah;
        internal int p_RadioStar;
        internal int p_Repetition;
        internal int p_ShamelessPlug;
        internal int p_ShortStop;
        internal int p_SkinnyDipper;

        private WebClient p_WebClient;
        #endregion

        #region Properties
        internal int CommandsUsed
        {
            get { return p_CommandsUsed; }
            set { p_CommandsUsed = value; }
        }

        internal string LastOnline
        {
            get
            {
                return Global.ConvertUnixDate(p_LastOnline, "Never");
            }
        }

        internal int Loads
        {
            get { return p_Loads; }
        }

        internal bool Online
        {
            get { return p_Online; }
        }

        internal int PlayTime
        {
            get { return p_PlayTime; }
            set { p_PlayTime = value; }
        }

        internal int RunTime
        {
            get { return p_RunTime; }
            set { p_RunTime = value; }
        }

        internal string Skin
        {
            get { return p_Skin; }
            set { p_Skin = value; }
        }

        internal int SongsCompleted
        {
            get { return p_SongsCompleted; }
            set { p_SongsCompleted = value; }
        }

        internal int SongsPlayed
        {
            get { return p_SongsPlayed; }
            set { p_SongsPlayed = value; }
        }

        internal string UserSince
        {
            get
            {
                return Global.ConvertUnixDate(p_UserSince, "Never");
            }
        }

        internal int ChattyCathy
        {
            get { return p_ChattyCathy; }
            set { p_ChattyCathy = value; }
        }

        internal int JukeboxHero
        {
            get { return p_JukeboxHero; }
            set { p_JukeboxHero = value; }
        }

        internal int LeetHaxor
        {
            get { return p_LeetHaxor; }
            set { p_LeetHaxor = value; }
        }

        internal int MP3Playah
        {
            get { return p_MP3Playah; }
            set { p_MP3Playah = value; }
        }

        internal int RadioStar
        {
            get { return p_RadioStar; }
            set { p_RadioStar = value; }
        }

        internal int Repetition
        {
            get { return p_Repetition; }
            set { p_Repetition = value; }
        }

        internal int ShamelessPlug
        {
            get { return p_ShamelessPlug; }
            set { p_ShamelessPlug = value; }
        }

        internal int ShortStop
        {
            get { return p_ShortStop; }
            set { p_ShortStop = value; }
        }

        internal int SkinnyDipper
        {
            get { return p_SkinnyDipper; }
            set { p_SkinnyDipper = value; }
        }
        #endregion

        #region Constructor/Destructor
        public Stats()
        {
            p_CommandsUsed = 0;
            p_LastOnline = 0;
            p_Loads = 1;
            p_Online = false;
            p_PlayTime = 0;
            p_RunTime = 0;
            p_Skin = string.Empty;
            p_SongsCompleted = 0;
            p_SongsPlayed = 0;
            p_UserSince = 0;

            p_ChattyCathy = 0;
            p_JukeboxHero = 0;
            p_LeetHaxor = 0;
            p_MP3Playah = 0;
            p_RadioStar = 0;
            p_Repetition = 0;
            p_ShamelessPlug = 0;
            p_ShortStop = 0;
            p_SkinnyDipper = 0;

            retry:
            if (!Global.MediaPlayer.IsOnline)
            {
                UI.InfoDialog id = new UI.InfoDialog("You must be connected to the internet for STEAMp3 to run properly." + Environment.NewLine + Environment.NewLine + "Click Abort to exit, or Retry to try again.", UI.InfoDialog.InfoButtons.AbortRetry);
                if (id.ShowDialog() == DialogResult.Retry) goto retry;
                else
                {
                    Process.GetCurrentProcess().Kill(); //?
                    //Exit();
                    return;
                }
            }

            try
            {
                p_WebClient = new WebClient();
                p_WebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                p_WebClient.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                string data = "login=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&bd=" + Application.ProductVersion;
                string result = p_WebClient.UploadString("http://steamp3.ta0soft.com/stats.php", "POST", data);
                p_WebClient.Dispose();

                if (result.Equals("Failure")) return;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlNodeList users = xml.SelectNodes("Steamp3.Stats/Users/User");

                foreach (XmlNode user in users)
                {
                    if (Global.GetXmlValue(user, "ID", "0") == Global.Steam.Client.GetSteamID().ToUInt64().ToString())
                    {
                        p_CommandsUsed = Global.StringToInt(Global.GetXmlValue(user, "CommandsUsed", "0"));
                        p_LastOnline = Global.StringToInt(Global.GetXmlValue(user, "LastOnline", "0"));
                        p_Loads = Global.StringToInt(Global.GetXmlValue(user, "Loads", "1"));
                        p_Online = Global.StringToBool(Global.GetXmlValue(user, "Online", "0"), "1");
                        p_PlayTime = Global.StringToInt(Global.GetXmlValue(user, "PlayTime", "0"));
                        p_RunTime = Global.StringToInt(Global.GetXmlValue(user, "RunTime", "0"));
                        p_Skin = Global.GetXmlValue(user, "Skin", string.Empty);
                        p_SongsCompleted = Global.StringToInt(Global.GetXmlValue(user, "SongsCompleted", "0"));
                        p_SongsPlayed = Global.StringToInt(Global.GetXmlValue(user, "SongsPlayed", "0"));
                        p_UserSince = Global.StringToInt(Global.GetXmlValue(user, "UserSince", "0"));

                        XmlNode achievements = user.SelectSingleNode("Achievements");

                        p_ChattyCathy = Global.StringToInt(Global.GetXmlValue(achievements, "ChattyCathy", "0"));
                        p_JukeboxHero = Global.StringToInt(Global.GetXmlValue(achievements, "JukeboxHero", "0"));
                        p_LeetHaxor = Global.StringToInt(Global.GetXmlValue(achievements, "LeetHaxor", "0"));
                        p_MP3Playah = Global.StringToInt(Global.GetXmlValue(achievements, "MP3Playah", "0"));
                        p_RadioStar = Global.StringToInt(Global.GetXmlValue(achievements, "RadioStar", "0"));
                        p_Repetition = Global.StringToInt(Global.GetXmlValue(achievements, "Repetition", "0"));
                        p_ShamelessPlug = Global.StringToInt(Global.GetXmlValue(achievements, "ShamelessPlug", "0"));
                        p_ShortStop = Global.StringToInt(Global.GetXmlValue(achievements, "ShortStop", "0"));
                        p_SkinnyDipper = Global.StringToInt(Global.GetXmlValue(achievements, "SkinnyDipper", "0"));
                        return;
                    }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                TimeSpan ts = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
                p_RunTime += (int)ts.TotalSeconds;

                p_Skin = Global.Skin.URL; //?

                p_WebClient = new WebClient();
                p_WebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                p_WebClient.Headers.Add("User-Agent", "STEAMp3/" + Application.ProductVersion);

                string data = "logout=" + Global.Steam.Client.GetSteamID().ToUInt64().ToString() + "&cu=" + p_CommandsUsed.ToString() + "&pt=" + p_PlayTime.ToString() + "&rt=" + p_RunTime.ToString() + "&sk=" + HttpUtility.UrlEncode(p_Skin) + "&sc=" + p_SongsCompleted.ToString() + "&sp=" + p_SongsPlayed.ToString();

                foreach (UI.Achievement item in Global.MainWindow.AchievementList.Items)
                {
                    data += item.Save();
                }
                
                p_WebClient.UploadString("http://steamp3.ta0soft.com/stats.php", "POST", data);
                p_WebClient.Dispose();
            }
            catch { }

            p_SkinnyDipper = 0;
            p_ShortStop = 0;
            p_ShamelessPlug = 0;
            p_Repetition = 0;
            p_RadioStar = 0;
            p_MP3Playah = 0;
            p_LeetHaxor = 0;
            p_JukeboxHero = 0;
            p_ChattyCathy = 0;

            p_UserSince = 0;
            p_SongsPlayed = 0;
            p_SongsCompleted = 0;
            p_Skin = string.Empty;
            p_RunTime = 0;
            p_PlayTime = 0;
            p_Online = false;
            p_Loads = 0;
            p_LastOnline = 0;
            p_CommandsUsed = 0;
        }
        #endregion
    }
    #endregion

    #region Steam
    public class Steam
    {
        #region Objects
        private SteamAPI.SteamClient p_Client;
        private SteamAPI.ChatRoom p_Chat;
        private SteamAPI.Lobby p_Lobby;
        private SteamAPI.PrivateMessage p_PM;
        #endregion

        #region Properties
        public SteamAPI.SteamClient Client
        {
            get { return p_Client; }
        }

        public SteamAPI.ChatRoom Chat
        {
            get { return p_Chat; }
        }

        public SteamAPI.Lobby Lobby
        {
            get { return p_Lobby; }
        }

        public SteamAPI.PrivateMessage PM
        {
            get { return p_PM; }
        }
        #endregion

        #region Constructor/Destructor
        public Steam()
        {
            retry:
            p_Client = new SteamAPI.SteamClient();
            if (!p_Client.Connected)
            {
                UI.InfoDialog id = new UI.InfoDialog("Steam must be loaded for STEAMp3 to run properly, it was not found." + Environment.NewLine + Environment.NewLine + "Click Abort to exit, or Retry to try again.", UI.InfoDialog.InfoButtons.AbortRetry);
                if (id.ShowDialog() == DialogResult.Retry) goto retry;
                else
                {
                    Process.GetCurrentProcess().Kill(); //?
                    //Exit();
                    return;
                }
            }

            p_Chat = new SteamAPI.ChatRoom(p_Client);
            //p_Chat.ChatIDChanged += new SteamAPI.ChatRoom.ChatIDChangedEventHandler(p_Chat_ChatIDChanged);
            p_Chat.MessageReceived += new SteamAPI.ChatRoom.MessageReceivedEventHandler(p_Chat_MessageReceived);

            p_Lobby = new SteamAPI.Lobby(p_Client);
            //p_Lobby.LobbyIDChanged += new SteamAPI.Lobby.LobbyIDChangedEventHandler(p_Lobby_LobbyIDChanged);
            p_Lobby.MessageReceived += new SteamAPI.Lobby.MessageReceivedEventHandler(p_Lobby_MessageReceived);

            p_PM = new SteamAPI.PrivateMessage(p_Client);
            //p_PM.FriendIDChanged += new SteamAPI.PrivateMessage.FriendIDChangedEventHandler(p_PM_FriendIDChanged);
            p_PM.MessageReceived += new SteamAPI.PrivateMessage.MessageReceivedEventHandler(p_PM_MessageReceived);
        }

        public void Dispose()
        {
            p_PM.Dispose();
            p_Lobby.Dispose();
            p_Chat.Dispose();
            p_Client.Dispose();
        }
        #endregion

        #region Child Events
        //private void p_Chat_ChatIDChanged()
        //{
        //}

        private void p_Chat_MessageReceived(SteamAPI.SteamID chatID, SteamAPI.SteamID sender, string message)
        {
            if (!Global.MainWindow.ProcessGlobalCommand(message, true))
            {
                if (sender.ToString() == p_Client.GetSteamID().ToString())
                {
                    if (Global.MainWindow.ProcessLocalCommand(message, true))
                    {
                        Global.Stats.CommandsUsed++;

                        Global.MainWindow.AchievementList.IncreaseChattyCathy(chatID);
                    }
                }
            }
        }

        //private void p_Lobby_LobbyIDChanged()
        //{
            //MessageBox.Show("LobbyID: " + p_Lobby.LobbyID.ToString());
        //}

        private void p_Lobby_MessageReceived(SteamAPI.SteamID lobbyID, SteamAPI.SteamID sender, string message)
        {
            //MessageBox.Show("LobbyMessage: " + message);
        }

        //private void p_PM_FriendIDChanged()
        //{
        //}

        private void p_PM_MessageReceived(SteamAPI.SteamID sender, SteamAPI.SteamID receiver, string message)
        {
            if (!Global.MainWindow.ProcessGlobalCommand(message, false))
            {
                if (sender.ToString() == p_Client.GetSteamID().ToString())
                {
                    if (Global.MainWindow.ProcessLocalCommand(message, false))
                    {
                        Global.Stats.CommandsUsed++;
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        public void SendError(string error, bool chat)
        {
            SendMessage("Error: {ls}" + error + "{rs}", chat);
        }

        public void SendStatus(string status, string param, bool chat)
        {
            SendMessage(status + ": {ls}" + param + "{rs}", chat);
        }

        public void SendMessage(string msg, bool chat)
        {
            if (p_Chat == null) return;
            if (p_PM == null) return;
            if (Global.Settings.Silent) return;

            string s = "/me " + msg;

            s = Global.ReplaceString(s, "{STEAMP3}", "{ls}STEAMp3{rs}");
            s = Global.ReplaceString(s, "{LS}", Global.Settings.LeftSeparator);
            s = Global.ReplaceString(s, "{RS}", Global.Settings.RightSeparator);

            //p_Lobby.SendMessage(s);

            if (chat) p_Chat.SendMessage(s);
            else p_PM.SendMessage(s);
        }
        #endregion
    }
    #endregion
}