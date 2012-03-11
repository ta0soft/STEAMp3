#region Using
using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Steam4NET;
#endregion

namespace Steamp3.SteamAPI
{
    #region SteamClient
    public class SteamClient
    {
        #region Objects
        private bool p_Connected;
        private ISteamClient008 p_SteamClient008;
        private int p_Pipe, p_User;
        private ISteamFriends002 p_SteamFriends002;
        private ISteamFriends005 p_SteamFriends005;
        private ISteamMatchmaking001 p_SteamMatchmaking001;
        private ISteamUser004 p_SteamUser004;
        private ISteamUtils004 p_SteamUtils004;

        private IClientEngine p_ClientEngine002;
        private IClientFriends p_ClientFriends001;
        private IClientUtils p_ClientUtils001;
        #endregion

        #region Properties
        public bool Connected
        {
            get { return p_Connected; }
        }

        public ISteamClient008 SteamClient008
        {
            get { return p_SteamClient008; }
        }

        public int Pipe
        {
            get { return p_Pipe; }
        }

        public int User
        {
            get { return p_User; }
        }

        public ISteamFriends002 SteamFriends002
        {
            get { return p_SteamFriends002; }
        }

        public ISteamFriends005 SteamFriends005
        {
            get { return p_SteamFriends005; }
        }

        public ISteamMatchmaking001 SteamMatchmaking001
        {
            get { return p_SteamMatchmaking001; }
        }

        public ISteamUser004 SteamUser004
        {
            get { return p_SteamUser004; }
        }

        public ISteamUtils004 SteamUtils004
        {
            get { return p_SteamUtils004; }
        }

        public IClientEngine ClientEngine002
        {
            get { return p_ClientEngine002; }
        }

        public IClientFriends ClientFriends001
        {
            get { return p_ClientFriends001; }
        }

        public IClientUtils ClientUtils001
        {
            get { return p_ClientUtils001; }
        }
        #endregion

        #region Constructor/Destructor
        public SteamClient()
        {
            Reset();

            if (!Steamworks.Load()) return;

            p_SteamClient008 = Steamworks.CreateInterface<ISteamClient008>("SteamClient008");
            if (p_SteamClient008 == null) return;

            p_Pipe = p_SteamClient008.CreateSteamPipe();
            p_User = p_SteamClient008.ConnectToGlobalUser(p_Pipe);

            p_SteamFriends002 = Steamworks.CastInterface<ISteamFriends002>(p_SteamClient008.GetISteamFriends(p_User, p_Pipe, "SteamFriends002"));
            p_SteamFriends005 = Steamworks.CastInterface<ISteamFriends005>(p_SteamClient008.GetISteamFriends(p_User, p_Pipe, "SteamFriends005"));
            p_SteamMatchmaking001 = Steamworks.CastInterface<ISteamMatchmaking001>(p_SteamClient008.GetISteamMatchmaking(p_User, p_Pipe, "SteamMatchMaking001"));
            p_SteamUser004 = Steamworks.CastInterface<ISteamUser004>(p_SteamClient008.GetISteamUser(p_User, p_Pipe, "SteamUser004"));
            p_SteamUtils004 = Steamworks.CastInterface<ISteamUtils004>(p_SteamClient008.GetISteamUtils(p_Pipe, "SteamUtils004"));

            // --------------

            p_ClientEngine002 = Steamworks.CreateInterface<IClientEngine>("CLIENTENGINE_INTERFACE_VERSION002");
            if (p_ClientEngine002 == null) return;

            p_ClientFriends001 = Steamworks.CastInterface<IClientFriends>(p_ClientEngine002.GetIClientFriends(p_User, p_Pipe, "CLIENTFRIENDS_INTERFACE_VERSION001"));
            p_ClientUtils001 = Steamworks.CastInterface<IClientUtils>(p_ClientEngine002.GetIClientUtils(p_Pipe, "CLIENTUTILS_INTERFACE_VERSION001"));
            if (p_ClientFriends001 == null) return;

            //p_ClientUtils.SetAppIDForCurrentPipe(550);

            CallbackDispatcher.SpawnDispatchThread(p_Pipe);
            p_Connected = true;
        }

        public void Dispose()
        {
            if (p_Connected)
            {
                CallbackDispatcher.StopDispatchThread(p_Pipe);

                p_SteamClient008.ReleaseUser(p_Pipe, p_User);
                p_SteamClient008.ReleaseSteamPipe(p_Pipe);
            }

            Reset();
        }
        #endregion

        #region Public Methods
        public SteamID GetChatID(int index)
        {
            if (!p_Connected) return null;

            return new SteamID(p_ClientFriends001.GetChatRoomByIndex(index));
        }

        public int GetChatCount()
        {
            if (!p_Connected) return 0;

            return p_ClientFriends001.GetChatRoomCount();
        }

        public List<SteamID> GetOpenChats()
        {
            List<SteamID> result = new List<SteamID>();

            for (int i = 0; i < GetChatCount(); i++)
            {
                result.Add(GetChatID(i));
            }

            return result;
        }

        public SteamID GetLobbyID(int index)
        {
            if (!p_Connected) return null;

            return new SteamID(p_SteamMatchmaking001.GetLobbyByIndex(index));
        }

        public string GetInstallPath()
        {
            if (!p_Connected) return string.Empty;

            return p_ClientUtils001.GetUserBaseFolderInstallImage();
        }

        public void SetPersonaState(EPersonaState state)
        {
            if (!p_Connected) return;

            p_SteamFriends002.SetPersonaState(state);
        }

        public List<string> GetFriendAliases(SteamID steamID)
        {
            if (!p_Connected) return null;

            List<string> result = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                string alias = p_SteamFriends002.GetFriendPersonaNameHistory(steamID.CSteamID, i);
                if (!string.IsNullOrEmpty(alias)) result.Add(alias);
            }

            return result;
        }

        public bool GetFriendGamePlayed(SteamID steamID, ref string name, ref ulong id, ref uint ip, ref ushort port)
        {
            if (!p_Connected) return false;

            ushort queryPort = 0;

            name = p_ClientFriends001.GetFriendGamePlayedExtraInfo(steamID.CSteamID);
            return p_SteamFriends002.GetFriendGamePlayed(steamID.CSteamID, ref id, ref ip, ref port, ref queryPort);
        }

        public EPersonaState GetFriendPersonaState(SteamID steamID)
        {
            if (!p_Connected) return EPersonaState.k_EPersonaStateMax;

            return p_SteamFriends002.GetFriendPersonaState(steamID.CSteamID);
        }

        public bool IsFriendOnline(SteamID steamID)
        {
            if (!p_Connected) return false;

            switch (p_SteamFriends002.GetFriendPersonaState(steamID.CSteamID))
            {
                case EPersonaState.k_EPersonaStateAway:
                case EPersonaState.k_EPersonaStateBusy:
                case EPersonaState.k_EPersonaStateOnline:
                case EPersonaState.k_EPersonaStateSnooze:
                    return true;
            }

            return false;
        }

        public void AddFriend(SteamID steamID)
        {
            if (!p_Connected) return;

            p_SteamFriends002.AddFriend(steamID.CSteamID);
        }

        public void AddFriendByName(string name)
        {
            if (!p_Connected) return;

            p_SteamFriends002.AddFriendByName(name);
        }

        public void RemoveFriend(SteamID steamID)
        {
            if (!p_Connected) return;

            p_SteamFriends002.RemoveFriend(steamID.CSteamID);
        }

        public List<SteamID> GetFriends()
        {
            if (!p_Connected) return null;

            List<SteamID> result = new List<SteamID>();
            int count = p_SteamFriends005.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

            for (int i = 0; i < count; i++)
            {
                result.Add(new SteamID(p_SteamFriends005.GetFriendByIndex(i, (int)EFriendFlags.k_EFriendFlagImmediate)));
            }

            return result;
        }

        public SteamID GetFriendID(string name)
        {
            if (!p_Connected) return null;

            int count = p_SteamFriends002.GetFriendCount((int)EFriendFlags.k_EFriendFlagImmediate);

            for (int i = 0; i < count; i++)
            {
                SteamID friendID = new SteamID(p_SteamFriends002.GetFriendByIndex(i, (int)EFriendFlags.k_EFriendFlagImmediate));
                if (p_SteamFriends002.GetFriendPersonaName(friendID.CSteamID).ToLower() == name.ToLower()) return friendID;
            }

            return null;
        }

        public string GetFriendPersonaName(SteamID steamID)
        {
            if (!p_Connected) return string.Empty;

            return p_SteamFriends002.GetFriendPersonaName(steamID.CSteamID);
        }

        public string GetFriendLogOffDate(SteamID steamID)
        {
            if (!p_Connected) return string.Empty;

            return Global.ConvertUnixDate((int)p_ClientFriends001.GetFriendLastLogoffTime(steamID.CSteamID), "Never");
        }

        public string GetFriendLogOnDate(SteamID steamID)
        {
            if (!p_Connected) return string.Empty;

            return Global.ConvertUnixDate((int)p_ClientFriends001.GetFriendLastLogonTime(steamID.CSteamID), "Never");
        }

        public void ActivateGameOverlay()
        {
            if (!p_Connected) return;

            p_SteamFriends005.ActivateGameOverlay("Friends");
        }

        public string GetPersonaName()
        {
            if (!p_Connected) return string.Empty;

            return p_SteamFriends002.GetPersonaName();
        }

        public void SetPersonaName(string name)
        {
            if (!p_Connected) return;

            p_SteamFriends005.SetPersonaName(name);
        }

        public SteamID GetSteamID()
        {
            if (!p_Connected) return null;

            return new SteamID(p_SteamUser004.GetSteamID());
        }
        #endregion

        #region Private Methods
        private void Reset()
        {
            p_Connected = false;
            p_SteamClient008 = null;
            p_Pipe = 0;
            p_User = 0;
            p_SteamFriends002 = null;
            p_SteamFriends005 = null;
            p_SteamMatchmaking001 = null;
            p_SteamUser004 = null;
            p_SteamUtils004 = null;
            p_ClientEngine002 = null;
            p_ClientFriends001 = null;
            p_ClientUtils001 = null;
        }
        #endregion
    }
    #endregion

    #region ChatRoom
    public class ChatRoom
    {
        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate Int32 NativeGetChatRoomEntry(IntPtr thisobj, UInt64 steamIDchat, Int32 iChatID, ref UInt64 steamIDuser, byte[] pvData, Int32 cubData, ref EChatEntryType peChatEntryType);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate string NativeGetChatRoomName(IntPtr thisobj, UInt64 steamIDchat);

        public delegate void ChatIDChangedEventHandler();
        public delegate void MessageReceivedEventHandler(SteamID chatID, SteamID sender, string message);
        #endregion

        #region Events
        public event ChatIDChangedEventHandler ChatIDChanged;
        public event MessageReceivedEventHandler MessageReceived;
        #endregion

        #region Objects
        private SteamClient p_Client;
        private SteamID p_ChatID;
        private Callback<ChatRoomMsg_t> p_Callback;
        private Native.VTable p_VTable;
        //private Native.VTScan p_VTScan;
        private NativeGetChatRoomEntry p_GetChatEntry;
        private NativeGetChatRoomName p_GetChatName;
        #endregion

        #region Properties
        public SteamID ChatID
        {
            get { return p_ChatID; }
        }
        #endregion

        #region Constructor/Destructor
        public ChatRoom(SteamClient client)
        {
            p_Client = client;
            if (!p_Client.Connected) return;

            p_ChatID = p_Client.GetChatID(0);
            p_Callback = new Callback<ChatRoomMsg_t>(GetMessage, ChatRoomMsg_t.k_iCallback);
            p_VTable = new Native.VTable(p_Client.ClientFriends001.Interface);
            p_GetChatEntry = p_VTable.GetFunc<NativeGetChatRoomEntry>(100);
            p_GetChatName = p_VTable.GetFunc<NativeGetChatRoomName>(118);
        }

        public void Dispose()
        {
            p_GetChatName = null;
            p_GetChatEntry = null;
            p_VTable = null;
            p_Callback = null;
            if (p_ChatID != null) p_ChatID.Dispose();
            p_ChatID = null;
            //p_Client = null;
        }
        #endregion

        #region Public Methods
        public bool BanUser(SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.BanChatMember(p_ChatID.CSteamID, steamID.CSteamID);
        }

        public bool BanUser(SteamID chatID, SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.BanChatMember(chatID.CSteamID, steamID.CSteamID);
        }

        public bool UnBanUser(SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.UnBanChatMember(p_ChatID.CSteamID, steamID.CSteamID);
        }

        public bool UnBanUser(SteamID chatID, SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.UnBanChatMember(chatID.CSteamID, steamID.CSteamID);
        }

        public bool KickUser(SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.KickChatMember(p_ChatID.CSteamID, steamID.CSteamID);
        }

        public bool KickUser(SteamID chatID, SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.ClientFriends001.KickChatMember(chatID.CSteamID, steamID.CSteamID);
        }

        public string GetChatName()
        {
            if (!p_Client.Connected) return string.Empty;

            return p_GetChatName(p_Client.ClientFriends001.Interface, p_ChatID.CSteamID);
        }

        public string GetChatName(SteamID chatID)
        {
            if (!p_Client.Connected) return string.Empty;

            return p_GetChatName(p_Client.ClientFriends001.Interface, chatID.CSteamID);
        }

        public List<SteamID> GetChatUsers()
        {
            if (!p_Client.Connected) return null;

            List<SteamID> result = new List<SteamID>();
            int count = p_Client.SteamFriends002.GetFriendCountFromSource(p_ChatID.CSteamID);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(new SteamID(p_Client.SteamFriends002.GetFriendFromSourceByIndex(p_ChatID.CSteamID, i)));
                }

                return result;
            }
            else return null;
        }

        public List<SteamID> GetChatUsers(SteamID chatID)
        {
            if (!p_Client.Connected) return null;

            List<SteamID> result = new List<SteamID>();
            int count = p_Client.SteamFriends002.GetFriendCountFromSource(chatID.CSteamID);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(new SteamID(p_Client.SteamFriends002.GetFriendFromSourceByIndex(chatID.CSteamID, i)));
                }

                return result;
            }
            else return null;
        }

        public bool IsUserInChat(SteamID steamID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.SteamFriends005.IsUserInSource(steamID.CSteamID, p_ChatID.CSteamID);
        }

        public bool IsUserInChat(SteamID steamID, SteamID chatID)
        {
            if (!p_Client.Connected) return false;

            return p_Client.SteamFriends005.IsUserInSource(steamID.CSteamID, chatID.CSteamID);
        }

        public void CloseChat()
        {
            if (!p_Client.Connected) return;

            p_Client.ClientFriends001.LeaveChatRoom(p_ChatID.CSteamID);
        }

        public void CloseChat(SteamID chatID)
        {
            if (!p_Client.Connected) return;

            p_Client.ClientFriends001.LeaveChatRoom(chatID.CSteamID);
        }

        public bool SendMessage(string message)
        {
            if (p_ChatID == null) return false;

            return SendMessage(p_ChatID, message);
        }

        public bool SendMessage(SteamID chatID, string message)
        {
            if (!p_Client.Connected) return false;

            p_ChatID = chatID; // raise event?

            EChatEntryType msgType = EChatEntryType.k_EChatEntryTypeChatMsg;

            if (message.ToLower().StartsWith("/me "))
            {
                msgType = EChatEntryType.k_EChatEntryTypeEmote;
                message = message.Substring(4);
            }

            byte[] messageData = Encoding.UTF8.GetBytes(message);
            return p_Client.ClientFriends001.SendChatMsg(p_ChatID.CSteamID, msgType, messageData, messageData.Length + 1);
        }
        #endregion

        #region Private Methods
        private void GetMessage(ChatRoomMsg_t msg)
        {
            try
            {
                ulong chatter = 0;
                byte[] msgData = new byte[1024 * 4];
                EChatEntryType type = EChatEntryType.k_EChatEntryTypeChatMsg;
                int len = p_GetChatEntry(p_Client.ClientFriends001.Interface, msg.m_ulSteamIDChat, (int)msg.m_iChatID, ref chatter, msgData, msgData.Length, ref type);

                if (type == EChatEntryType.k_EChatEntryTypeTyping || type == EChatEntryType.k_EChatEntryTypeEmote) return;
                if (len < 1) len = 1;

                if (p_ChatID == null || msg.m_ulSteamIDChat != p_ChatID.CSteamID)
                {
                    p_ChatID = new SteamID(msg.m_ulSteamIDChat);
                    OnChatIDChanged();
                }

                OnMessageReceived(p_ChatID, new SteamID(msg.m_ulSteamIDUser), Encoding.UTF8.GetString(msgData).Substring(0, len - 1));
            }
            catch { }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnChatIDChanged()
        {
            if (ChatIDChanged != null) ChatIDChanged.Invoke();
        }

        protected virtual void OnMessageReceived(SteamID chatID, SteamID sender, string message)
        {
            if (MessageReceived != null) MessageReceived.Invoke(chatID, sender, message);
        }
        #endregion
    }
    #endregion

    #region Lobby
    public class Lobby
    {
        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate Int32 NativeGetLobbyChatEntry(IntPtr thisobj, UInt64 steamIDLobby, Int32 iChatID, ref UInt64 steamIDuser, byte[] pvData, Int32 cubData, ref EChatEntryType peChatEntryType);

        public delegate void LobbyIDChangedEventHandler();
        public delegate void MessageReceivedEventHandler(SteamID lobbyID, SteamID sender, string message);
        #endregion

        #region Events
        public event LobbyIDChangedEventHandler LobbyIDChanged;
        public event MessageReceivedEventHandler MessageReceived;
        #endregion

        #region Objects
        private SteamClient p_Client;
        private SteamID p_LobbyID;
        private Callback<LobbyChatMsg_t> p_Callback;
        #endregion

        #region Properties
        public SteamID LobbyID
        {
            get { return p_LobbyID; }
        }
        #endregion

        #region Constructors/Destructor
        public Lobby(SteamClient client)
        {
            p_Client = client;
            if (!p_Client.Connected) return;

            p_LobbyID = p_Client.GetLobbyID(0);
            p_Callback = new Callback<LobbyChatMsg_t>(GetMessage, LobbyChatMsg_t.k_iCallback);
        }

        public Lobby(SteamClient client, SteamID lobbyID)
        {
            p_Client = client;
            if (!p_Client.Connected) return;

            p_LobbyID = lobbyID;
            p_Callback = new Callback<LobbyChatMsg_t>(GetMessage, LobbyChatMsg_t.k_iCallback);
        }

        public void Dispose()
        {
            p_Callback = null;
            if (p_LobbyID != null) p_LobbyID.Dispose();
            p_LobbyID = null;
            //p_Client = null;
        }
        #endregion

        #region Public Methods
        public bool SendMessage(string message)
        {
            if (p_LobbyID == null) return false;

            return SendMessage(p_LobbyID, message);
        }

        public bool SendMessage(SteamID lobbyID, string message)
        {
            if (!p_Client.Connected) return false;

            p_LobbyID = lobbyID; // raise event?

            byte[] messageData = Encoding.UTF8.GetBytes(message);
            return p_Client.SteamMatchmaking001.SendLobbyChatMsg(p_LobbyID.CSteamID, messageData, messageData.Length + 1);
        }
        #endregion

        #region Private Methods
        private void GetMessage(LobbyChatMsg_t msg)
        {
            try
            {
                ulong chatter = 0;
                byte[] msgData = new byte[1024 * 4];
                EChatEntryType type = EChatEntryType.k_EChatEntryTypeChatMsg;
                int len = p_Client.SteamMatchmaking001.GetLobbyChatEntry(msg.m_ulSteamIDLobby, (int)msg.m_iChatID, ref chatter, msgData, msgData.Length, ref type);

                if (type == EChatEntryType.k_EChatEntryTypeTyping || type == EChatEntryType.k_EChatEntryTypeEmote) return;
                if (len < 1) len = 1;

                if (p_LobbyID == null || msg.m_ulSteamIDLobby != p_LobbyID.CSteamID)
                {
                    p_LobbyID = new SteamID(msg.m_ulSteamIDLobby);
                    OnLobbyIDChanged();
                }

                OnMessageReceived(p_LobbyID, new SteamID(msg.m_ulSteamIDUser), Encoding.UTF8.GetString(msgData).Substring(0, len - 1));
            }
            catch { }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnLobbyIDChanged()
        {
            if (LobbyIDChanged != null) LobbyIDChanged.Invoke();
        }

        protected virtual void OnMessageReceived(SteamID lobbyID, SteamID sender, string message)
        {
            if (MessageReceived != null) MessageReceived.Invoke(lobbyID, sender, message);
        }
        #endregion
    }
    #endregion

    #region PrivateMessage
    public class PrivateMessage
    {
        #region Delegates
        public delegate void FriendIDChangedEventHandler();
        public delegate void MessageReceivedEventHandler(SteamID sender, SteamID receiver, string message);
        #endregion

        #region Events
        public event FriendIDChangedEventHandler FriendIDChanged;
        public event MessageReceivedEventHandler MessageReceived;
        #endregion

        #region Objects
        private SteamClient p_Client;
        private SteamID p_FriendID;
        private Callback<FriendChatMsg_t> p_Callback;
        #endregion

        #region Properties
        public SteamID FriendID
        {
            get { return p_FriendID; }
        }
        #endregion

        #region Constructors/Destructor
        public PrivateMessage(SteamClient client)
        {
            p_Client = client;
            if (!p_Client.Connected) return;

            p_FriendID = null;
            p_Callback = new Callback<FriendChatMsg_t>(GetMessage, FriendChatMsg_t.k_iCallback);
        }

        public PrivateMessage(SteamClient client, SteamID friendID)
        {
            p_Client = client;
            if (!p_Client.Connected) return;

            p_FriendID = friendID;
            p_Callback = new Callback<FriendChatMsg_t>(GetMessage, FriendChatMsg_t.k_iCallback);
        }

        public void Dispose()
        {
            p_Callback = null;
            if (p_FriendID != null) p_FriendID.Dispose();
            p_FriendID = null;
            //p_Client = null;
        }
        #endregion

        #region Public Methods
        public bool SendMessage(string message)
        {
            if (p_FriendID == null) return false; //?

            return SendMessage(p_FriendID, message);
        }

        public bool SendMessage(SteamID friendID, string message)
        {
            if (!p_Client.Connected) return false;

            p_FriendID = friendID; // raise event?

            EChatEntryType msgType = EChatEntryType.k_EChatEntryTypeChatMsg;

            if (message.ToLower().StartsWith("/me "))
            {
                msgType = EChatEntryType.k_EChatEntryTypeEmote;
                message = message.Substring(4);
            }

            byte[] messageData = Encoding.UTF8.GetBytes(message);

            return p_Client.SteamFriends002.SendMsgToFriend(p_FriendID.CSteamID, msgType, messageData, messageData.Length + 1);
        }
        #endregion

        #region Private Methods
        private void GetMessage(FriendChatMsg_t msg)
        {
            try
            {
                byte[] msgData = new byte[1024 * 4];
                EChatEntryType type = EChatEntryType.k_EChatEntryTypeChatMsg;
                int len = p_Client.SteamFriends002.GetChatMessage(msg.m_ulReceiver, (int)msg.m_iChatID, msgData, msgData.Length, ref type);

                if (type == EChatEntryType.k_EChatEntryTypeTyping || type == EChatEntryType.k_EChatEntryTypeEmote) return;
                if (len < 1) len = 1;

                if (p_FriendID == null || msg.m_ulReceiver != p_FriendID.CSteamID)
                {
                    p_FriendID = new SteamID(msg.m_ulReceiver);
                    OnFriendIDChanged();
                }

                OnMessageReceived(new SteamID(msg.m_ulSender), p_FriendID, Encoding.UTF8.GetString(msgData).Substring(0, len - 1));
            }
            catch { }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnFriendIDChanged()
        {
            if (FriendIDChanged != null) FriendIDChanged.Invoke();
        }

        protected virtual void OnMessageReceived(SteamID sender, SteamID receiver, string message)
        {
            if (MessageReceived != null) MessageReceived.Invoke(sender, receiver, message);
        }
        #endregion
    }
    #endregion

    #region SteamID
    public class SteamID
    {
        #region Objects
        private CSteamID p_CSteamID;
        #endregion

        #region Properties
        public CSteamID CSteamID
        {
            get { return p_CSteamID; }
        }
        #endregion

        #region Constructor/Destructor
        public SteamID(CSteamID cSteamID)
        {
            p_CSteamID = cSteamID;
        }

        public SteamID(ulong steamID64)
        {
            p_CSteamID = 0;
            p_CSteamID.SetFromUint64(steamID64);
        }

        public void Dispose()
        {
            p_CSteamID = null;
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return p_CSteamID.ToString();
        }

        public ulong ToUInt64()
        {
            return p_CSteamID.ConvertToUint64();
        }
        #endregion
    }
    #endregion

    #region Native
    namespace Native
    {
        #region ScanResultCollection
        class ScanResultCollection<T> : ReadOnlyCollection<ScanResult<T>>
        {
            public ScanResultCollection(IList<ScanResult<T>> list)
                : base(list)
            {
            }
        }
        #endregion

        #region ScanResult
        class ScanResult<T>
        {
            public T Delegate { get; private set; }
            public uint Address { get; private set; }

            public ScanResult(T dele, uint addr)
            {
                this.Delegate = dele;
                this.Address = addr;
            }
        }
        #endregion

        #region VTable (New)
        unsafe class VTable
        {
            private IntPtr p_ClassObject;
            private int* p_Table;

            public VTable(IntPtr classObject)
            {
                p_ClassObject = classObject;
                p_Table = ((int*)*(int*)p_ClassObject.ToInt32());
            }

            public T GetFunc<T>(int index)
            {
                IntPtr virtFunc = new IntPtr(p_Table[index]);

                return (T)(object)Marshal.GetDelegateForFunctionPointer(virtFunc, typeof(T));
            }
        }
        #endregion

        #region VTScan (Old)
        class VTScan
        {
            static class Native
            {
                [DllImport("kernel32.dll")]
                public static extern int VirtualQuery(
                    IntPtr lpAddress,
                    ref MEMORY_BASIC_INFORMATION lpBuffer,
                    uint dwLength
                );

                public const ushort IMAGE_DOS_SIGNATURE = 0x5A4D; // MZ
                public const uint IMAGE_NT_SIGNATURE = 0x00004550; // PE00

                [StructLayout(LayoutKind.Sequential)]
                public struct MEMORY_BASIC_INFORMATION
                {
                    public IntPtr BaseAddress;
                    public IntPtr AllocationBase;
                    public uint AllocationProtect;
                    public IntPtr RegionSize;
                    public uint State;
                    public uint Protect;
                    public uint Type;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct IMAGE_DOS_HEADER
                {
                    public UInt16 e_magic;       // Magic number
                    public UInt16 e_cblp;        // Bytes on last page of file
                    public UInt16 e_cp;          // Pages in file
                    public UInt16 e_crlc;        // Relocations
                    public UInt16 e_cparhdr;     // Size of header in paragraphs
                    public UInt16 e_minalloc;    // Minimum extra paragraphs needed
                    public UInt16 e_maxalloc;    // Maximum extra paragraphs needed
                    public UInt16 e_ss;          // Initial (relative) SS value
                    public UInt16 e_sp;          // Initial SP value
                    public UInt16 e_csum;        // Checksum
                    public UInt16 e_ip;          // Initial IP value
                    public UInt16 e_cs;          // Initial (relative) CS value
                    public UInt16 e_lfarlc;      // File address of relocation table
                    public UInt16 e_ovno;        // Overlay number
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                    public UInt16[] e_res1;        // Reserved words
                    public UInt16 e_oemid;       // OEM identifier (for e_oeminfo)
                    public UInt16 e_oeminfo;     // OEM information; e_oemid specific
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
                    public UInt16[] e_res2;        // Reserved words
                    public Int32 e_lfanew;      // File address of new exe header
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct IMAGE_NT_HEADERS
                {
                    public UInt32 Signature;
                    public IMAGE_FILE_HEADER FileHeader;
                    public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct IMAGE_FILE_HEADER
                {
                    public UInt16 Machine;
                    public UInt16 NumberOfSections;
                    public UInt32 TimeDateStamp;
                    public UInt32 PointerToSymbolTable;
                    public UInt32 NumberOfSymbols;
                    public UInt16 SizeOfOptionalHeader;
                    public UInt16 Characteristics;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct IMAGE_OPTIONAL_HEADER32
                {
                    //
                    // Standard fields.
                    //
                    public UInt16 Magic;
                    public Byte MajorLinkerVersion;
                    public Byte MinorLinkerVersion;
                    public UInt32 SizeOfCode;
                    public UInt32 SizeOfInitializedData;
                    public UInt32 SizeOfUninitializedData;
                    public UInt32 AddressOfEntryPoint;
                    public UInt32 BaseOfCode;
                    public UInt32 BaseOfData;
                    //
                    // NT additional fields.
                    //
                    public UInt32 ImageBase;
                    public UInt32 SectionAlignment;
                    public UInt32 FileAlignment;
                    public UInt16 MajorOperatingSystemVersion;
                    public UInt16 MinorOperatingSystemVersion;
                    public UInt16 MajorImageVersion;
                    public UInt16 MinorImageVersion;
                    public UInt16 MajorSubsystemVersion;
                    public UInt16 MinorSubsystemVersion;
                    public UInt32 Win32VersionValue;
                    public UInt32 SizeOfImage;
                    public UInt32 SizeOfHeaders;
                    public UInt32 CheckSum;
                    public UInt16 Subsystem;
                    public UInt16 DllCharacteristics;
                    public UInt32 SizeOfStackReserve;
                    public UInt32 SizeOfStackCommit;
                    public UInt32 SizeOfHeapReserve;
                    public UInt32 SizeOfHeapCommit;
                    public UInt32 LoaderFlags;
                    public UInt32 NumberOfRvaAndSizes;
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                    public IMAGE_DATA_DIRECTORY[] DataDirectory;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct IMAGE_DATA_DIRECTORY
                {
                    public UInt32 VirtualAddress;
                    public UInt32 Size;
                }
            }

            IntPtr vtObject;
            uint baseAddr, baseLen;

            List<IntPtr> vtFuncs = new List<IntPtr>();


            public unsafe VTScan(IntPtr vtObject)
            {
                this.vtObject = vtObject;

                int* vtable = (int*)*((int*)vtObject.ToInt32());
                for (int i = 0; vtable[i] != 0; ++i)
                {
                    vtFuncs.Add(new IntPtr(vtable[i]));
                }
            }


            public bool Init()
            {
                if (vtFuncs.Count == 0)
                    return false;

                Native.MEMORY_BASIC_INFORMATION memInfo = new Native.MEMORY_BASIC_INFORMATION();

                if (Native.VirtualQuery(vtFuncs[0], ref memInfo, (uint)Marshal.SizeOf(memInfo)) == 0)
                    return false;

                baseAddr = (uint)memInfo.AllocationBase.ToInt32();

                Native.IMAGE_DOS_HEADER dos = PtrToStruct<Native.IMAGE_DOS_HEADER>(memInfo.AllocationBase);

                if (dos.e_magic != Native.IMAGE_DOS_SIGNATURE)
                    return false;

                Native.IMAGE_NT_HEADERS pe = PtrToStruct<Native.IMAGE_NT_HEADERS>(new IntPtr(baseAddr + dos.e_lfanew));

                if (pe.Signature != Native.IMAGE_NT_SIGNATURE)
                    return false;

                baseLen = pe.OptionalHeader.SizeOfImage;

                return true;
            }

            public unsafe ScanResultCollection<T> DoScan<T>(string sig, string mask) where T : class
            {
                try
                {
                    Debug.Assert(sig.Length == mask.Length);
                    int sigLen = sig.Length;

                    List<ScanResult<T>> matches = new List<ScanResult<T>>();

                    foreach (IntPtr vFunc in vtFuncs)
                    {
                        byte* basePtr = (byte*)vFunc.ToInt32();
                        byte* endPtr = (byte*)(baseAddr + baseLen);
                        int i = 0;

                        while (basePtr < endPtr)
                        {
                            for (i = 0; i < sigLen; i++)
                            {
                                if ((mask[i] != '?') && (sig[i] != basePtr[i])) break;
                            }

                            ushort padding = *(ushort*)basePtr;

                            if (padding == 0xCCCC) break;

                            if (i == sigLen)
                            {
                                ScanResult<T> result = new ScanResult<T>((T)(object)Marshal.GetDelegateForFunctionPointer(vFunc, typeof(T)), (uint)vFunc.ToInt32());
                                matches.Add(result);
                            }

                            basePtr++;
                        }
                    }

                    ScanResultCollection<T> src = new ScanResultCollection<T>(matches);
                    return src;
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.ToString());
                    return null;
                }
            }

            T PtrToStruct<T>(IntPtr addr)
            {
                return (T)Marshal.PtrToStructure(addr, typeof(T));
            }
        }
        #endregion
    }
    #endregion
}