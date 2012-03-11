#region Using
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Steamp3.Network
{
    #region FTPClient
    public class FTPClient
    {
        #region Delegates
        public delegate void DirectoryChangedEventHandler(string[] files);
        public delegate void SocketErrorEventHandler(SocketException se);
        #endregion

        #region Events
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event DirectoryChangedEventHandler DirectoryChanged;
        public event SocketErrorEventHandler SocketError;
        #endregion

        #region Structures
        private struct StateObject
        {
            public Socket Socket;
            public byte[] DataBuffer;

            public StateObject(Socket socket)
            {
                Socket = socket;
                DataBuffer = new byte[1024];
            }
        }
        #endregion

        #region Objects
        private string p_Host, p_User, p_Password, p_Path;
        private int p_Port;
        private Socket p_Socket;
        private string p_Response;
        private int p_ResponseCode;

        //private int p_BlockSize;
        //private byte[] p_Buffer;
        #endregion

        #region Properties
        public string Host
        {
            get { return p_Host; }
        }

        public string Path
        {
            get { return p_Path; }
        }

        public string User
        {
            get { return p_User; }
        }

        public string Password
        {
            get { return p_Password; }
        }

        public int Port
        {
            get { return p_Port; }
        }

        public Socket Socket
        {
            get { return p_Socket; }
        }
        #endregion

        #region Constructor/Destructor
        public FTPClient(string host, string user, string password, string path, int port)
        {
            p_Host = host;
            p_User = user;
            p_Password = password;
            p_Path = path;
            p_Port = port;
            p_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            p_Response = string.Empty;
            p_ResponseCode = 0;

            //p_BlockSize = 512;
            //p_Buffer = new byte[p_BlockSize];
        }

        public void Dispose()
        {
            //p_Buffer = null;
            //p_BlockSize = 0;

            p_ResponseCode = 0;
            p_Response = string.Empty;
            p_Socket = null;
            p_Port = 0;
            p_Path = string.Empty;
            p_Password = string.Empty;
            p_User = string.Empty;
            p_Host = string.Empty;
        }
        #endregion

        #region Public Methods
        #region Connect
        public void Connect()
        {
            try
            {
                IPEndPoint ep = Global.ResolveHost(p_Host, p_Port);

                if (ep != null) p_Socket.BeginConnect(ep, new AsyncCallback(ConnectCallback), p_Socket);
                else OnSocketError(new SocketException(87)); // WSA_INVALID_PARAMETER
            }
            catch (SocketException se)
            {
                OnSocketError(se);
            }
        }
        #endregion

        #region ChangeDirectory
        public void ChangeDirectory(string path)
        {
            if (path.Equals(".")) return;

            if (p_Socket.Connected)
            {
                p_Path = path;

                SendCommand("CWD " + p_Path);

                if (p_ResponseCode == 250) ListFiles(string.Empty);
            }
        }
        #endregion

        #region Upload
        public void Upload(string s, string filename)
        {
            if (p_Socket.Connected)
            {
                Socket dataSocket = CreateDataSocket();

                if (dataSocket != null)
                {
                    SendCommand("STOR " + filename);

                    if (!(p_ResponseCode == 150 || p_ResponseCode == 125)) CloseSocket(dataSocket);
                    else
                    {
                        byte[] data = Encoding.ASCII.GetBytes(s);

                        dataSocket.Send(data, data.Length, 0);

                        WaitForResponse(dataSocket);

                        //CloseSocket(dataSocket);
                    }
                }
            }
        }
        #endregion

        #region Disconnect
        public void Disconnect()
        {
            if (p_Socket.Connected)
            {
                SendCommand("QUIT");
            }
        }
        #endregion
        #endregion

        #region Private Methods
        #region CloseSocket
        private void CloseSocket(Socket socket)
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }
        #endregion

        #region CreateDataSocket
        private Socket CreateDataSocket()
        {
            SendCommand("PASV");

            if (p_ResponseCode != 227) return null;

            int index1 = p_Response.IndexOf("(");
            int index2 = p_Response.IndexOf(")");
            string ipData = p_Response.Substring(index1 + 1, (index2 - index1) - 1);
            int partCount = 0;
            string buffer = string.Empty;
            int[] parts = new int[6];

            for (int i = 0; i < ipData.Length && partCount <= 6; i++)
            {
                char c = char.Parse(ipData.Substring(i, 1));
                if (char.IsDigit(c)) buffer += c;
                else if (c != ',') return null;

                if (c == ',' || ipData.Length == i + 1)
                {
                    try
                    {
                        parts[partCount] = Int32.Parse(buffer);
                        partCount++;
                        buffer = string.Empty;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            string ip = parts[0].ToString() + "." + parts[1].ToString() + "." + parts[2].ToString() + "." + parts[3].ToString();
            int port = parts[4] << 8;
            port += parts[5];

            Socket result = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = Global.ResolveHost(ip, port);

            try
            {
                result.Connect(ep);
            }
            catch (SocketException se)
            {
                CloseSocket(result);

                OnSocketError(se);
            }

            return result;
        }
        #endregion

        #region ListFiles
        private void ListFiles(string mask)
        {
            if (p_Socket.Connected)
            {
                Socket dataSocket = CreateDataSocket();

                if (dataSocket != null)
                {
                    SendCommand("NLST " + mask);

                    if (!(p_ResponseCode == 150 || p_ResponseCode == 125)) CloseSocket(dataSocket);
                    else WaitForResponse(dataSocket);
                }
            }
        }
        #endregion

        #region SendCommand
        private void SendCommand(string command)
        {
            command += Environment.NewLine;

            p_Socket.Send(Encoding.ASCII.GetBytes(command));

            p_Response = string.Empty;
            p_ResponseCode = 0;

            WaitForResponse(p_Socket);

            while (p_ResponseCode == 0)
            {
                //Application.DoEvents();
            }
        }
        #endregion

        #region WaitForResponse
        private void WaitForResponse(Socket socket)
        {
            try
            {
                StateObject stateObject = new StateObject(socket);
                IAsyncResult result = socket.BeginReceive(stateObject.DataBuffer, 0, stateObject.DataBuffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), stateObject);
            }
            catch (SocketException se)
            {
                OnSocketError(se);
            }
        }
        #endregion
        #endregion

        #region Callbacks
        #region ConnectCallback
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;

                socket.EndConnect(ar);

                WaitForResponse(p_Socket);
            }
            catch (SocketException se)
            {
                OnSocketError(se);
            }
        }
        #endregion

        #region DataReceived
        private void DataReceived(IAsyncResult ar)
        {
            try
            {
                StateObject stateObject = (StateObject)ar.AsyncState;
                int bufferSize = stateObject.Socket.EndReceive(ar);
                char[] chars = new char[bufferSize + 1];
                int charLength = Encoding.ASCII.GetChars(stateObject.DataBuffer, 0, bufferSize, chars, 0);
                string data = new string(chars);
                // if (string.IsNullOrEmpty(data))
                //string[] s = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                //if (data.Length > 2) p_Response = s[s.Length - 2];
                //else p_Response = s[0];

                p_Response = data;

                if (Int32.TryParse(p_Response.Substring(0, 3), out p_ResponseCode))
                {
                    switch (p_ResponseCode)
                    {
                        case 220: // Service ready for new user.
                            SendCommand("USER " + p_User);
                            break;
                        case 331: // User name okay, need password.
                            SendCommand("PASS " + p_Password);
                            break;
                        case 230: // User logged in, proceed.
                            OnConnected(new EventArgs());

                            ChangeDirectory(p_Path);
                            break;
                        case 530: // User not logged in.
                        case 202: // Command not implemented.
                        case 221: // Logout.
                            CloseSocket(p_Socket);

                            OnDisconnected(new EventArgs());
                            break;
                        default:
                            MessageBox.Show("Unhandled response: " + p_Response);
                            break;
                    }
                }
                else
                {
                    OnDirectoryChanged(p_Response.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                    //MessageBox.Show("Data response: " + p_Response);
                }
            }
            catch (SocketException se)
            {
                OnSocketError(se);
            }
        }
        #endregion
        #endregion

        #region Virtual Methods
        protected virtual void OnConnected(EventArgs e)
        {
            if (Connected != null) Connected.Invoke(this, e);
        }

        protected virtual void OnDisconnected(EventArgs e)
        {
            if (Disconnected != null) Disconnected.Invoke(this, e);
        }

        protected virtual void OnDirectoryChanged(string[] files)
        {
            if (DirectoryChanged != null) DirectoryChanged.Invoke(files);
        }

        protected virtual void OnSocketError(SocketException se)
        {
            if (SocketError != null) SocketError.Invoke(se);
        }
        #endregion
    }
    #endregion

    #region HTTPClient
    public class HTTPClient
    {
        #region Delegates
        public delegate void ProgressChangedEventHandler(int bytesRead);
        public delegate void WebErrorEventHandler(WebException we);
        #endregion

        #region Events
        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler DownloadComplete;
        public event WebErrorEventHandler WebError;
        #endregion

        #region Structures
        private struct StateObject
        {
            public string Filename;
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Stream ResponseStream;
            public byte[] DataBuffer;
            public int BytesRead;

            public StateObject(string filename, HttpWebRequest request)
            {
                Filename = filename;
                Request = request;
                Response = null;
                ResponseStream = null;
                DataBuffer = new byte[1024];
                BytesRead = 0;
            }
        }
        #endregion

        #region Constructor/Destructor
        public HTTPClient()
        {
        }

        public void Dispose()
        {
        }
        #endregion

        #region Public Methods
        public bool DownloadFile(string url, string filename)
        {
            try
            {
                if (!Global.MediaPlayer.IsOnline) return false;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                StateObject stateObject = new StateObject(filename, request);

                request.BeginGetResponse(new AsyncCallback(ResponseCallback), stateObject);

                return true;
            }
            catch (WebException we)
            {
                OnWebError(we);
                return false;
            }
        }
        #endregion

        #region Callbacks
        private void ResponseCallback(IAsyncResult ar)
        {
            try
            {
                StateObject stateObject = (StateObject)ar.AsyncState;
                stateObject.Response = (HttpWebResponse)stateObject.Request.EndGetResponse(ar);
                Stream responseStream = stateObject.Response.GetResponseStream();
                stateObject.ResponseStream = responseStream;

                responseStream.BeginRead(stateObject.DataBuffer, 0, 1024, new AsyncCallback(ReadCallback), stateObject);
            }
            catch (WebException we)
            {
                MessageBox.Show(we.ToString());

                OnWebError(we);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                StateObject stateObject = (StateObject)ar.AsyncState;
                Stream responseStream = stateObject.ResponseStream;

                int bytesRead = responseStream.EndRead(ar);

                if (bytesRead > 0)
                {
                    stateObject.BytesRead += bytesRead;

                    OnProgressChanged(stateObject.BytesRead);

                    responseStream.BeginRead(stateObject.DataBuffer, 0, 1024, new AsyncCallback(ReadCallback), stateObject);
                }
                else
                {
                    //Global.SaveString(stateObject.Filename, stateObject.Data);
                    //Global.SaveStream(stateObject.Filename, responseStream);

                    responseStream.Close();
                    stateObject.Response.Close();
                    //stateObject.Done();

                    OnDownloadComplete(new EventArgs());
                }
            }
            catch (WebException we)
            {
                OnWebError(we);
            }
        }
        #endregion

        #region Virtual Methods
        protected virtual void OnProgressChanged(int bytesRead)
        {
            if (ProgressChanged != null) ProgressChanged.Invoke(bytesRead);
        }

        protected virtual void OnDownloadComplete(EventArgs e)
        {
            if (DownloadComplete != null) DownloadComplete.Invoke(this, e);
        }

        protected virtual void OnWebError(WebException we)
        {
            if (WebError != null) WebError.Invoke(we);
        }
        #endregion
    }
    #endregion
}