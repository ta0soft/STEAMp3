#region Using
using System;
using System.Runtime.InteropServices;
#endregion

namespace Steamp3.COM
{
    #region Delegates
    public delegate void AudioEndpointVolumeNotificationDelegate(AudioVolumeNotificationData data);
    #endregion

    #region Enums
    public enum AudioSessionDisconnectReason
    {
        DisconnectReasonDeviceRemoval = 0,
        DisconnectReasonServerShutdown = (DisconnectReasonDeviceRemoval + 1),
        DisconnectReasonFormatChanged = (DisconnectReasonServerShutdown + 1),
        DisconnectReasonSessionLogoff = (DisconnectReasonFormatChanged + 1),
        DisconnectReasonSessionDisconnected = (DisconnectReasonSessionLogoff + 1),
        DisconnectReasonExclusiveModeOverride = (DisconnectReasonSessionDisconnected + 1)
    }

    public enum AudioSessionState
    {
        AudioSessionStateInactive = 0,
        AudioSessionStateActive = 1,
        AudioSessionStateExpired = 2
    }

    [Flags]
    internal enum CLSCTX : uint
    {
        INPROC_SERVER = 0x1,
        INPROC_HANDLER = 0x2,
        LOCAL_SERVER = 0x4,
        INPROC_SERVER16 = 0x8,
        REMOTE_SERVER = 0x10,
        INPROC_HANDLER16 = 0x20,
        RESERVED1 = 0x40,
        RESERVED2 = 0x80,
        RESERVED3 = 0x100,
        RESERVED4 = 0x200,
        NO_CODE_DOWNLOAD = 0x400,
        RESERVED5 = 0x800,
        NO_CUSTOM_MARSHAL = 0x1000,
        ENABLE_CODE_DOWNLOAD = 0x2000,
        NO_FAILURE_LOG = 0x4000,
        DISABLE_AAA = 0x8000,
        ENABLE_AAA = 0x10000,
        FROM_DEFAULT_CONTEXT = 0x20000,
        INPROC = INPROC_SERVER | INPROC_HANDLER,
        SERVER = INPROC_SERVER | LOCAL_SERVER | REMOTE_SERVER,
        ALL = SERVER | INPROC_HANDLER
    }

    public enum EDataFlow
    {
        eRender = 0,
        eCapture = 1,
        eAll = 2,
        EDataFlow_enum_count = 3
    }

    [Flags]
    public enum EDeviceState : uint
    {
        DEVICE_STATE_ACTIVE = 0x00000001,
        DEVICE_STATE_UNPLUGGED = 0x00000002,
        DEVICE_STATE_NOTPRESENT = 0x00000004,
        DEVICE_STATEMASK_ALL = 0x00000007
    }

    [Flags]
    public enum EEndpointHardwareSupport
    {
        Volume = 0x00000001,
        Mute = 0x00000002,
        Meter = 0x00000004
    }

    public enum ERole
    {
        eConsole = 0,
        eMultimedia = 1,
        eCommunications = 2,
        ERole_enum_count = 3
    }

    internal enum EStgmAccess
    {
        STGM_READ = 0x00000000,
        STGM_WRITE = 0x00000001,
        STGM_READWRITE = 0x00000002
    }
    #endregion

    #region Structures
    internal struct AUDIO_VOLUME_NOTIFICATION_DATA
    {
        public Guid guidEventContext;
        public bool bMuted;
        public float fMasterVolume;
        public uint nChannels;
        public float ChannelVolume;

        //Code Should Compile at warning level4 without any warnings, 
        //However this struct will give us Warning CS0649: Field [Fieldname] 
        //is never assigned to, and will always have its default value
        //You can disable CS0649 in the project options but that will disable
        //the warning for the whole project, it's a nice warning and we do want 
        //it in other places so we make a nice dummy function to keep the compiler
        //happy.
        private void FixCS0649()
        {
            guidEventContext = Guid.Empty;
            bMuted = false;
            fMasterVolume = 0;
            nChannels = 0;
            ChannelVolume = 0;
        }

    }

    internal struct Blob
    {
        public int Length;
        public IntPtr Data;

        //Code Should Compile at warning level4 without any warnings, 
        //However this struct will give us Warning CS0649: Field [Fieldname] 
        //is never assigned to, and will always have its default value
        //You can disable CS0649 in the project options but that will disable
        //the warning for the whole project, it's a nice warning and we do want 
        //it in other places so we make a nice dummy function to keep the compiler
        //happy.
        private void FixCS0649()
        {
            Length = 0;
            Data = IntPtr.Zero;
        }
    }

    public struct PropertyKey
    {
        public Guid fmtid;
        public int pid;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PropVariant
    {
        [FieldOffset(0)]
        short vt;
        [FieldOffset(2)]
        short wReserved1;
        [FieldOffset(4)]
        short wReserved2;
        [FieldOffset(6)]
        short wReserved3;
        [FieldOffset(8)]
        sbyte cVal;
        [FieldOffset(8)]
        byte bVal;
        [FieldOffset(8)]
        short iVal;
        [FieldOffset(8)]
        ushort uiVal;
        [FieldOffset(8)]
        int lVal;
        [FieldOffset(8)]
        uint ulVal;
        [FieldOffset(8)]
        long hVal;
        [FieldOffset(8)]
        ulong uhVal;
        [FieldOffset(8)]
        float fltVal;
        [FieldOffset(8)]
        double dblVal;
        [FieldOffset(8)]
        Blob blobVal;
        [FieldOffset(8)]
        DateTime date;
        [FieldOffset(8)]
        bool boolVal;
        [FieldOffset(8)]
        int scode;
        [FieldOffset(8)]
        System.Runtime.InteropServices.ComTypes.FILETIME filetime;
        [FieldOffset(8)]
        IntPtr everything_else;

        //I'm sure there is a more efficient way to do this but this works ..for now..
        internal byte[] GetBlob()
        {
            byte[] Result = new byte[blobVal.Length];
            for (int i = 0; i < blobVal.Length; i++)
            {
                Result[i] = Marshal.ReadByte((IntPtr)((long)(blobVal.Data) + i));
            }
            return Result;
        }

        public object Value
        {
            get
            {
                VarEnum ve = (VarEnum)vt;
                switch (ve)
                {
                    case VarEnum.VT_I1:
                        return bVal;
                    case VarEnum.VT_I2:
                        return iVal;
                    case VarEnum.VT_I4:
                        return lVal;
                    case VarEnum.VT_I8:
                        return hVal;
                    case VarEnum.VT_INT:
                        return iVal;
                    case VarEnum.VT_UI4:
                        return ulVal;
                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringUni(everything_else);
                    case VarEnum.VT_BLOB:
                        return GetBlob();
                }
                return "FIXME Type = " + ve.ToString();
            }
        }

    } 
    #endregion

    #region Interfaces
    #region IAudioEndpointVolume
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioEndpointVolume
    {
        [PreserveSig]
        int RegisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);
        [PreserveSig]
        int UnregisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);
        [PreserveSig]
        int GetChannelCount(out int pnChannelCount);
        [PreserveSig]
        int SetMasterVolumeLevel(float fLevelDB, Guid pguidEventContext);
        [PreserveSig]
        int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
        [PreserveSig]
        int GetMasterVolumeLevel(out float pfLevelDB);
        [PreserveSig]
        int GetMasterVolumeLevelScalar(out float pfLevel);
        [PreserveSig]
        int SetChannelVolumeLevel(uint nChannel, float fLevelDB, Guid pguidEventContext);
        [PreserveSig]
        int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, Guid pguidEventContext);
        [PreserveSig]
        int GetChannelVolumeLevel(uint nChannel, out float pfLevelDB);
        [PreserveSig]
        int GetChannelVolumeLevelScalar(uint nChannel, out float pfLevel);
        [PreserveSig]
        int SetMute([MarshalAs(UnmanagedType.Bool)] Boolean bMute, Guid pguidEventContext);
        [PreserveSig]
        int GetMute(out bool pbMute);
        [PreserveSig]
        int GetVolumeStepInfo(out uint pnStep, out uint pnStepCount);
        [PreserveSig]
        int VolumeStepUp(Guid pguidEventContext);
        [PreserveSig]
        int VolumeStepDown(Guid pguidEventContext);
        [PreserveSig]
        int QueryHardwareSupport(out uint pdwHardwareSupportMask);
        [PreserveSig]
        int GetVolumeRange(out float pflVolumeMindB, out float pflVolumeMaxdB, out float pflVolumeIncrementdB);
    }
    #endregion

    #region IAudioEndpointVolumeCallback
    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioEndpointVolumeCallback
    {
        [PreserveSig]
        int OnNotify(IntPtr pNotifyData);
    }
    #endregion

    #region IAudioMeterInformation
    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioMeterInformation
    {
        [PreserveSig]
        int GetPeakValue(out float pfPeak);
        [PreserveSig]
        int GetMeteringChannelCount(out int pnChannelCount);
        [PreserveSig]
        int GetChannelsPeakValues(int u32ChannelCount, [In]   IntPtr afPeakValues);
        [PreserveSig]
        int QueryHardwareSupport(out int pdwHardwareSupportMask);
    }
    #endregion

    #region IAudioSessionControl2
    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionControl2
    {
        //IAudioSession functions
        [PreserveSig]
        int GetState(out AudioSessionState state);
        [PreserveSig]
        int GetDisplayName(out IntPtr name);
        [PreserveSig]
        int SetDisplayName(string value, Guid EventContext);
        [PreserveSig]
        int GetIconPath(out IntPtr Path);
        [PreserveSig]
        int SetIconPath(string Value, Guid EventContext);
        [PreserveSig]
        int GetGroupingParam(out Guid GroupingParam);
        [PreserveSig]
        int SetGroupingParam(Guid Override, Guid Eventcontext);
        [PreserveSig]
        int RegisterAudioSessionNotification(IAudioSessionEvents NewNotifications);
        [PreserveSig]
        int UnregisterAudioSessionNotification(IAudioSessionEvents NewNotifications);
        //IAudioSession2 functions
        [PreserveSig]
        int GetSessionIdentifier(out IntPtr retVal);
        [PreserveSig]
        int GetSessionInstanceIdentifier(out IntPtr retVal);
        [PreserveSig]
        int GetProcessId(out UInt32 retvVal);
        [PreserveSig]
        int IsSystemSoundsSession();
        [PreserveSig]
        int SetDuckingPreference(bool optOut);
    }
    #endregion

    #region IAudioSessionEnumerator
    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        int GetCount(out int SessionCount);
        int GetSession(int SessionCount, out IAudioSessionControl2 Session);
    }
    #endregion

    #region IAudioSessionEvents
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioSessionEvents
    {
        [PreserveSig]
        int OnDisplayNameChanged([MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName, Guid EventContext);
        [PreserveSig]
        int OnIconPathChanged([MarshalAs(UnmanagedType.LPWStr)] string NewIconPath, Guid EventContext);
        [PreserveSig]
        int OnSimpleVolumeChanged(float NewVolume, bool newMute, Guid EventContext);
        [PreserveSig]
        int OnChannelVolumeChanged(UInt32 ChannelCount, IntPtr NewChannelVolumeArray, UInt32 ChangedChannel, Guid EventContext);
        [PreserveSig]
        int OnGroupingParamChanged(Guid NewGroupingParam, Guid EventContext);
        [PreserveSig]
        int OnStateChanged(AudioSessionState NewState);
        [PreserveSig]
        int OnSessionDisconnected(AudioSessionDisconnectReason DisconnectReason);
    }
    #endregion

    #region IAudioSessionManager2
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager2
    {
        [PreserveSig]
        int GetAudioSessionControl(ref Guid AudioSessionGuid, UInt32 StreamFlags, IntPtr ISessionControl);
        [PreserveSig]
        int GetSimpleAudioVolume(ref Guid AudioSessionGuid, UInt32 StreamFlags, IntPtr  /*ISimpleAudioVolume*/ SimpleAudioVolume);
        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
        [PreserveSig]
        int RegisterSessionNotification(IntPtr IAudioSessionNotification);
        [PreserveSig]
        int UnregisterSessionNotification(IntPtr IAudioSessionNotification);
        [PreserveSig]
        int RegisterDuckNotification(string sessionID, IntPtr IAudioVolumeDuckNotification);
        [PreserveSig]
        int UnregisterDuckNotification(IntPtr IAudioVolumeDuckNotification);
    }
    #endregion

    #region IMMDevice
    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig]
        int Activate(ref Guid iid, CLSCTX dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        [PreserveSig]
        int OpenPropertyStore(EStgmAccess stgmAccess, out IPropertyStore propertyStore);
        [PreserveSig]
        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);
        [PreserveSig]
        int GetState(out EDeviceState pdwState);
    }
    #endregion

    #region IMMEndpoint
    [Guid("1BE09788-6894-4089-8586-9A2A6C265AC5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMEndpoint
    {
        [PreserveSig]
        int GetDataFlow(out EDataFlow pDataFlow);
    }
    #endregion

    #region IMMDeviceCollection
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceCollection
    {
        [PreserveSig]
        int GetCount(out uint pcDevices);
        [PreserveSig]
        int Item(uint nDevice, out IMMDevice Device);
    }
    #endregion

    #region IMMDeviceEnumerator
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        [PreserveSig]
        int EnumAudioEndpoints(EDataFlow dataFlow, EDeviceState StateMask, out IMMDeviceCollection device);
        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppEndpoint);
        [PreserveSig]
        int GetDevice(string pwstrId, out IMMDevice ppDevice);
        [PreserveSig]
        int RegisterEndpointNotificationCallback(IntPtr pClient);
        [PreserveSig]
        int UnregisterEndpointNotificationCallback(IntPtr pClient);
    }
    #endregion

    #region IPropertyStore
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyStore
    {
        [PreserveSig]
        int GetCount(out Int32 count);
        [PreserveSig]
        int GetAt(int iProp, out PropertyKey pkey);
        [PreserveSig]
        int GetValue(ref PropertyKey key, out PropVariant pv);
        [PreserveSig]
        int SetValue(ref PropertyKey key, ref PropVariant propvar);
        [PreserveSig]
        int Commit();
    }
    #endregion

    #region ISimpleAudioVolume
    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISimpleAudioVolume
    {
        [PreserveSig]
        int SetMasterVolume(float fLevel, ref Guid EventContext);
        [PreserveSig]
        int GetMasterVolume(out float pfLevel);
        [PreserveSig]
        int SetMute(bool bMute, ref Guid EventContext);
        [PreserveSig]
        int GetMute(out bool bMute);
    }
    #endregion

    #region IWMPAudioRenderConfig
    [ComImport, Guid("E79C6349-5997-4CE4-917C-22A3391EC564"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IWMPAudioRenderConfig
    {
        int get_audioOutputDevice([MarshalAs(UnmanagedType.BStr)] out string pbstrOutputDevice);
        int put_audioOutputDevice(string bstrOutputDevice);
    }
    #endregion
    #endregion

    #region Classes
    #region AudioEndpointVolume
    public class AudioEndpointVolume : IDisposable
    {
        private IAudioEndpointVolume _AudioEndPointVolume;
        private AudioEndpointVolumeChannels _Channels;
        private AudioEndpointVolumeStepInformation _StepInformation;
        private AudioEndpointVolumeVolumeRange _VolumeRange;
        private EEndpointHardwareSupport _HardwareSupport;
        private AudioEndpointVolumeCallback _CallBack;
        public event AudioEndpointVolumeNotificationDelegate OnVolumeNotification;

        public AudioEndpointVolumeVolumeRange VolumeRange
        {
            get
            {
                return _VolumeRange;
            }
        }
        public EEndpointHardwareSupport HardwareSupport
        {
            get
            {
                return _HardwareSupport;
            }
        }
        public AudioEndpointVolumeStepInformation StepInformation
        {
            get
            {
                return _StepInformation;
            }
        }
        public AudioEndpointVolumeChannels Channels
        {
            get
            {
                return _Channels;
            }
        }
        public float MasterVolumeLevel
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.GetMasterVolumeLevel(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.SetMasterVolumeLevel(value, Guid.Empty));
            }
        }
        public float MasterVolumeLevelScalar
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.GetMasterVolumeLevelScalar(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.SetMasterVolumeLevelScalar(value, Guid.Empty));
            }
        }
        public bool Mute
        {
            get
            {
                bool result;
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.GetMute(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.SetMute(value, Guid.Empty));
            }
        }
        public void VolumeStepUp()
        {
            Marshal.ThrowExceptionForHR(_AudioEndPointVolume.VolumeStepUp(Guid.Empty));
        }
        public void VolumeStepDown()
        {
            Marshal.ThrowExceptionForHR(_AudioEndPointVolume.VolumeStepDown(Guid.Empty));
        }
        internal AudioEndpointVolume(IAudioEndpointVolume realEndpointVolume)
        {
            uint HardwareSupp;

            _AudioEndPointVolume = realEndpointVolume;
            _Channels = new AudioEndpointVolumeChannels(_AudioEndPointVolume);
            _StepInformation = new AudioEndpointVolumeStepInformation(_AudioEndPointVolume);
            Marshal.ThrowExceptionForHR(_AudioEndPointVolume.QueryHardwareSupport(out HardwareSupp));
            _HardwareSupport = (EEndpointHardwareSupport)HardwareSupp;
            _VolumeRange = new AudioEndpointVolumeVolumeRange(_AudioEndPointVolume);
            _CallBack = new AudioEndpointVolumeCallback(this);
            Marshal.ThrowExceptionForHR(_AudioEndPointVolume.RegisterControlChangeNotify(_CallBack));
        }
        internal void FireNotification(AudioVolumeNotificationData NotificationData)
        {
            AudioEndpointVolumeNotificationDelegate del = OnVolumeNotification;
            if (del != null)
            {
                del(NotificationData);
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            if (_CallBack != null)
            {
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.UnregisterControlChangeNotify(_CallBack));
                _CallBack = null;
            }
        }

        ~AudioEndpointVolume()
        {
            Dispose();
        }

        #endregion
    }
    #endregion

    #region AudioEndpointVolumeCallback
    internal class AudioEndpointVolumeCallback : IAudioEndpointVolumeCallback
    {
        private AudioEndpointVolume _Parent;

        internal AudioEndpointVolumeCallback(AudioEndpointVolume parent)
        {
            _Parent = parent;
        }

        [PreserveSig]
        public int OnNotify(IntPtr NotifyData)
        {
            //Since AUDIO_VOLUME_NOTIFICATION_DATA is dynamic in length based on the
            //number of audio channels available we cannot just call PtrToStructure 
            //to get all data, thats why it is split up into two steps, first the static
            //data is marshalled into the data structure, then with some IntPtr math the
            //remaining floats are read from memory.
            //
            AUDIO_VOLUME_NOTIFICATION_DATA data = (AUDIO_VOLUME_NOTIFICATION_DATA)Marshal.PtrToStructure(NotifyData, typeof(AUDIO_VOLUME_NOTIFICATION_DATA));

            //Determine offset in structure of the first float
            IntPtr Offset = Marshal.OffsetOf(typeof(AUDIO_VOLUME_NOTIFICATION_DATA), "ChannelVolume");
            //Determine offset in memory of the first float
            IntPtr FirstFloatPtr = (IntPtr)((long)NotifyData + (long)Offset);

            float[] voldata = new float[data.nChannels];

            //Read all floats from memory.
            for (int i = 0; i < data.nChannels; i++)
            {
                voldata[i] = (float)Marshal.PtrToStructure(FirstFloatPtr, typeof(float));
            }

            //Create combined structure and Fire Event in parent class.
            AudioVolumeNotificationData NotificationData = new AudioVolumeNotificationData(data.guidEventContext, data.bMuted, data.fMasterVolume, voldata);
            _Parent.FireNotification(NotificationData);
            return 0; //S_OK
        }
    }
    #endregion

    #region AudioEndpointVolumeChannel
    public class AudioEndpointVolumeChannel
    {
        private uint _Channel;
        private IAudioEndpointVolume _AudioEndpointVolume;

        internal AudioEndpointVolumeChannel(IAudioEndpointVolume parent, int channel)
        {
            _Channel = (uint)channel;
            _AudioEndpointVolume = parent;
        }

        public float VolumeLevel
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_AudioEndpointVolume.GetChannelVolumeLevel(_Channel, out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_AudioEndpointVolume.SetChannelVolumeLevel(_Channel, value, Guid.Empty));
            }
        }

        public float VolumeLevelScalar
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_AudioEndpointVolume.GetChannelVolumeLevelScalar(_Channel, out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_AudioEndpointVolume.SetChannelVolumeLevelScalar(_Channel, value, Guid.Empty));
            }
        }

    }
    #endregion

    #region AudioEndpointVolumeChannels
    public class AudioEndpointVolumeChannels
    {
        IAudioEndpointVolume _AudioEndPointVolume;
        AudioEndpointVolumeChannel[] _Channels;
        public int Count
        {
            get
            {
                int result;
                Marshal.ThrowExceptionForHR(_AudioEndPointVolume.GetChannelCount(out result));
                return result;
            }
        }

        public AudioEndpointVolumeChannel this[int index]
        {
            get
            {
                return _Channels[index];
            }
        }

        internal AudioEndpointVolumeChannels(IAudioEndpointVolume parent)
        {
            int ChannelCount;
            _AudioEndPointVolume = parent;

            ChannelCount = Count;
            _Channels = new AudioEndpointVolumeChannel[ChannelCount];
            for (int i = 0; i < ChannelCount; i++)
            {
                _Channels[i] = new AudioEndpointVolumeChannel(_AudioEndPointVolume, i);
            }
        }
    }
    #endregion

    #region AudioEndpointVolumeVolumeRange
    public class AudioEndpointVolumeVolumeRange
    {
        float _VolumeMindB;
        float _VolumeMaxdB;
        float _VolumeIncrementdB;

        internal AudioEndpointVolumeVolumeRange(IAudioEndpointVolume parent)
        {
            Marshal.ThrowExceptionForHR(parent.GetVolumeRange(out _VolumeMindB, out _VolumeMaxdB, out _VolumeIncrementdB));
        }

        public float MindB
        {
            get
            {
                return _VolumeMindB;
            }
        }

        public float MaxdB
        {
            get
            {
                return _VolumeMaxdB;
            }
        }

        public float IncrementdB
        {
            get
            {
                return _VolumeIncrementdB;
            }
        }
    }
    #endregion

    #region AudioEndpointVolumeStepInformation
    public class AudioEndpointVolumeStepInformation
    {
        private uint _Step;
        private uint _StepCount;
        internal AudioEndpointVolumeStepInformation(IAudioEndpointVolume parent)
        {
            Marshal.ThrowExceptionForHR(parent.GetVolumeStepInfo(out _Step, out _StepCount));
        }

        public uint Step
        {
            get
            {
                return _Step;
            }
        }

        public uint StepCount
        {
            get
            {
                return _StepCount;
            }
        }
    }
    #endregion

    #region AudioMeterInformation
    public class AudioMeterInformation
    {
        private IAudioMeterInformation _AudioMeterInformation;
        private EEndpointHardwareSupport _HardwareSupport;
        private AudioMeterInformationChannels _Channels;

        internal AudioMeterInformation(IAudioMeterInformation realInterface)
        {
            int HardwareSupp;

            _AudioMeterInformation = realInterface;
            Marshal.ThrowExceptionForHR(_AudioMeterInformation.QueryHardwareSupport(out HardwareSupp));
            _HardwareSupport = (EEndpointHardwareSupport)HardwareSupp;
            _Channels = new AudioMeterInformationChannels(_AudioMeterInformation);

        }

        public AudioMeterInformationChannels PeakValues
        {
            get
            {
                return _Channels;
            }
        }

        public EEndpointHardwareSupport HardwareSupport
        {
            get
            {
                return _HardwareSupport;
            }
        }

        public float MasterPeakValue
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_AudioMeterInformation.GetPeakValue(out result));
                return result;
            }
        }
    }
    #endregion

    #region AudioMeterInformationChannels
    public class AudioMeterInformationChannels
    {
        IAudioMeterInformation _AudioMeterInformation;

        public int Count
        {
            get
            {
                int result;
                Marshal.ThrowExceptionForHR(_AudioMeterInformation.GetMeteringChannelCount(out result));
                return result;
            }
        }

        public float this[int index]
        {
            get
            {
                float[] peakValues = new float[Count];
                GCHandle Params = GCHandle.Alloc(peakValues, GCHandleType.Pinned);
                Marshal.ThrowExceptionForHR(_AudioMeterInformation.GetChannelsPeakValues(peakValues.Length, Params.AddrOfPinnedObject()));
                Params.Free();
                return peakValues[index];
            }
        }

        internal AudioMeterInformationChannels(IAudioMeterInformation parent)
        {
            _AudioMeterInformation = parent;
        }
    }
    #endregion

    #region AudioSessionControl
    public class AudioSessionControl
    {
        internal IAudioSessionControl2 _AudioSessionControl;
        internal AudioMeterInformation _AudioMeterInformation;
        internal SimpleAudioVolume _SimpleAudioVolume;

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                return _AudioMeterInformation;
            }
        }

        public SimpleAudioVolume SimpleAudioVolume
        {
            get
            {
                return _SimpleAudioVolume;
            }
        }


        internal AudioSessionControl(IAudioSessionControl2 realAudioSessionControl)
        {
            IAudioMeterInformation _meters = realAudioSessionControl as IAudioMeterInformation;
            ISimpleAudioVolume _volume = realAudioSessionControl as ISimpleAudioVolume;
            if (_meters != null)
                _AudioMeterInformation = new AudioMeterInformation(_meters);
            if (_volume != null)
                _SimpleAudioVolume = new SimpleAudioVolume(_volume);
            _AudioSessionControl = realAudioSessionControl;

        }

        public void RegisterAudioSessionNotification(IAudioSessionEvents eventConsumer)
        {
            Marshal.ThrowExceptionForHR(_AudioSessionControl.RegisterAudioSessionNotification(eventConsumer));
        }

        public void UnregisterAudioSessionNotification(IAudioSessionEvents eventConsumer)
        {
            Marshal.ThrowExceptionForHR(_AudioSessionControl.UnregisterAudioSessionNotification(eventConsumer));
        }

        public AudioSessionState State
        {
            get
            {
                AudioSessionState res;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetState(out res));
                return res;
            }
        }

        public string DisplayName
        {
            get
            {
                IntPtr NamePtr;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetDisplayName(out NamePtr));
                string res = Marshal.PtrToStringAuto(NamePtr);
                Marshal.FreeCoTaskMem(NamePtr);
                return res;
            }
        }

        public string IconPath
        {
            get
            {
                IntPtr NamePtr;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetIconPath(out NamePtr));
                string res = Marshal.PtrToStringAuto(NamePtr);
                Marshal.FreeCoTaskMem(NamePtr);
                return res;
            }
        }

        public string SessionIdentifier
        {
            get
            {
                IntPtr NamePtr;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetSessionIdentifier(out NamePtr));
                string res = Marshal.PtrToStringAuto(NamePtr);
                Marshal.FreeCoTaskMem(NamePtr);
                return res;
            }
        }

        public string SessionInstanceIdentifier
        {
            get
            {
                IntPtr NamePtr;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetSessionInstanceIdentifier(out NamePtr));
                string res = Marshal.PtrToStringAuto(NamePtr);
                Marshal.FreeCoTaskMem(NamePtr);
                return res;
            }
        }

        public uint ProcessID
        {
            get
            {
                uint pid;
                Marshal.ThrowExceptionForHR(_AudioSessionControl.GetProcessId(out pid));
                return pid;
            }
        }

        public bool IsSystemIsSystemSoundsSession
        {
            get
            {
                return (_AudioSessionControl.IsSystemSoundsSession() == 0);  //S_OK
            }

        }
    }
    #endregion

    #region AudioSessionManager
    public class AudioSessionManager
    {
        private IAudioSessionManager2 _AudioSessionManager;
        private SessionCollection _Sessions;

        internal AudioSessionManager(IAudioSessionManager2 realAudioSessionManager)
        {
            _AudioSessionManager = realAudioSessionManager;
            IAudioSessionEnumerator _SessionEnum;
            Marshal.ThrowExceptionForHR(_AudioSessionManager.GetSessionEnumerator(out _SessionEnum));
            _Sessions = new SessionCollection(_SessionEnum);
        }

        public SessionCollection Sessions
        {
            get
            {
                return _Sessions;
            }
        }
    }
    #endregion

    #region AudioVolumeNotificationData
    public class AudioVolumeNotificationData
    {
        private Guid _EventContext;
        private bool _Muted;
        private float _MasterVolume;
        private int _Channels;
        private float[] _ChannelVolume;

        public Guid EventContext
        {
            get
            {
                return _EventContext;
            }
        }

        public bool Muted
        {
            get
            {
                return _Muted;
            }
        }

        public float MasterVolume
        {
            get
            {
                return _MasterVolume;
            }
        }
        public int Channels
        {
            get
            {
                return _Channels;
            }
        }

        public float[] ChannelVolume
        {
            get
            {
                return _ChannelVolume;
            }
        }
        public AudioVolumeNotificationData(Guid eventContext, bool muted, float masterVolume, float[] channelVolume)
        {
            _EventContext = eventContext;
            _Muted = muted;
            _MasterVolume = masterVolume;
            _Channels = channelVolume.Length;
            _ChannelVolume = channelVolume;
        }
    }
    #endregion

    #region MMDevice
    public class MMDevice
    {
        #region Variables
        private IMMDevice _RealDevice;
        private PropertyStore _PropertyStore;
        private AudioMeterInformation _AudioMeterInformation;
        private AudioEndpointVolume _AudioEndpointVolume;
        private AudioSessionManager _AudioSessionManager;

        #endregion

        #region Guids
        private static Guid IID_IAudioMeterInformation = typeof(IAudioMeterInformation).GUID;
        private static Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
        private static Guid IID_IAudioSessionManager = typeof(IAudioSessionManager2).GUID;
        #endregion

        #region Init
        private void GetPropertyInformation()
        {
            IPropertyStore propstore;
            Marshal.ThrowExceptionForHR(_RealDevice.OpenPropertyStore(EStgmAccess.STGM_READ, out propstore));
            _PropertyStore = new PropertyStore(propstore);
        }

        private void GetAudioSessionManager()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IID_IAudioSessionManager, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioSessionManager = new AudioSessionManager(result as IAudioSessionManager2);
        }

        private void GetAudioMeterInformation()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IID_IAudioMeterInformation, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioMeterInformation = new AudioMeterInformation(result as IAudioMeterInformation);
        }

        private void GetAudioEndpointVolume()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IID_IAudioEndpointVolume, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioEndpointVolume = new AudioEndpointVolume(result as IAudioEndpointVolume);
        }

        #endregion

        #region Properties
        public AudioSessionManager AudioSessionManager
        {
            get
            {
                if (_AudioSessionManager == null)
                    GetAudioSessionManager();

                return _AudioSessionManager;
            }
        }

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                if (_AudioMeterInformation == null)
                    GetAudioMeterInformation();

                return _AudioMeterInformation;
            }
        }

        public AudioEndpointVolume AudioEndpointVolume
        {
            get
            {
                if (_AudioEndpointVolume == null)
                    GetAudioEndpointVolume();

                return _AudioEndpointVolume;
            }
        }

        public PropertyStore Properties
        {
            get
            {
                if (_PropertyStore == null)
                    GetPropertyInformation();
                return _PropertyStore;
            }
        }

        public string FriendlyName
        {
            get
            {
                if (_PropertyStore == null)
                    GetPropertyInformation();
                if (_PropertyStore.Contains(PKEY.PKEY_DeviceInterface_FriendlyName))
                {
                    return (string)_PropertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
                }
                else
                    return "Unknown";
            }
        }


        public string ID
        {
            get
            {
                string Result;
                Marshal.ThrowExceptionForHR(_RealDevice.GetId(out Result));
                return Result;
            }
        }

        public EDataFlow DataFlow
        {
            get
            {
                EDataFlow Result;
                IMMEndpoint ep = _RealDevice as IMMEndpoint;
                ep.GetDataFlow(out Result);
                return Result;
            }
        }

        public EDeviceState State
        {
            get
            {
                EDeviceState Result;
                Marshal.ThrowExceptionForHR(_RealDevice.GetState(out Result));
                return Result;

            }
        }
        #endregion

        #region Constructor
        internal MMDevice(IMMDevice realDevice)
        {
            _RealDevice = realDevice;
        }
        #endregion
    }
    #endregion

    #region MMDeviceCollection
    public class MMDeviceCollection
    {
        private IMMDeviceCollection _MMDeviceCollection;

        public int Count
        {
            get
            {
                uint result;
                Marshal.ThrowExceptionForHR(_MMDeviceCollection.GetCount(out result));
                return (int)result;
            }
        }

        public MMDevice this[int index]
        {
            get
            {
                IMMDevice result;
                _MMDeviceCollection.Item((uint)index, out result);
                return new MMDevice(result);
            }
        }

        internal MMDeviceCollection(IMMDeviceCollection parent)
        {
            _MMDeviceCollection = parent;
        }
    }
    #endregion

    #region MMDeviceEnumerator
    //Marked as internal, since on its own its no good
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class _MMDeviceEnumerator
    {
    }

    //Small wrapper class
    public class MMDeviceEnumerator
    {
        private IMMDeviceEnumerator _realEnumerator = new _MMDeviceEnumerator() as IMMDeviceEnumerator;

        public MMDeviceCollection EnumerateAudioEndPoints(EDataFlow dataFlow, EDeviceState dwStateMask)
        {
            IMMDeviceCollection result;
            Marshal.ThrowExceptionForHR(_realEnumerator.EnumAudioEndpoints(dataFlow, dwStateMask, out result));
            return new MMDeviceCollection(result);
        }

        public MMDevice GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role)
        {
            IMMDevice _Device = null;
            Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).GetDefaultAudioEndpoint(dataFlow, role, out _Device));
            return new MMDevice(_Device);
        }

        public MMDevice GetDevice(string ID)
        {
            IMMDevice _Device = null;
            Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).GetDevice(ID, out _Device));
            return new MMDevice(_Device);
        }

        public MMDeviceEnumerator()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
            }
        }
    }
    #endregion

    #region PKEY
    public static class PKEY
    {
        public static readonly Guid PKEY_DeviceInterface_FriendlyName = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0);
        public static readonly Guid PKEY_AudioEndpoint_FormFactor = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_ControlPanelPageProvider = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_Association = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_PhysicalSpeakers = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_GUID = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_Disable_SysFx = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEndpoint_FullRangeSpeakers = new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
        public static readonly Guid PKEY_AudioEngine_DeviceFormat = new Guid(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c);

    }
    #endregion

    #region PropertyStore
    public class PropertyStore
    {
        private IPropertyStore _Store;

        public int Count
        {
            get
            {
                int Result;
                Marshal.ThrowExceptionForHR(_Store.GetCount(out Result));
                return Result;
            }
        }

        public PropertyStoreProperty this[int index]
        {
            get
            {
                PropVariant result;
                PropertyKey key = Get(index);
                Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
                return new PropertyStoreProperty(key, result);
            }
        }

        public bool Contains(Guid guid)
        {
            for (int i = 0; i < Count; i++)
            {
                PropertyKey key = Get(i);
                if (key.fmtid == guid)
                    return true;
            }
            return false;
        }

        public PropertyStoreProperty this[Guid guid]
        {
            get
            {
                PropVariant result;
                for (int i = 0; i < Count; i++)
                {
                    PropertyKey key = Get(i);
                    if (key.fmtid == guid)
                    {
                        Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
                        return new PropertyStoreProperty(key, result);
                    }
                }
                return null;
            }
        }

        public PropertyKey Get(int index)
        {
            PropertyKey key;
            Marshal.ThrowExceptionForHR(_Store.GetAt(index, out key));
            return key;
        }

        public PropVariant GetValue(int index)
        {
            PropVariant result;
            PropertyKey key = Get(index);
            Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
            return result;
        }

        internal PropertyStore(IPropertyStore store)
        {
            _Store = store;
        }
    }
    #endregion

    #region PropertyStoreProperty
    public class PropertyStoreProperty
    {
        private PropertyKey _PropertyKey;
        private PropVariant _PropValue;

        internal PropertyStoreProperty(PropertyKey key, PropVariant value)
        {
            _PropertyKey = key;
            _PropValue = value;
        }

        public PropertyKey Key
        {
            get
            {
                return _PropertyKey;
            }
        }

        public object Value
        {
            get
            {
                return _PropValue.Value;
            }
        }
    }
    #endregion

    #region SessionCollection
    public class SessionCollection
    {
        IAudioSessionEnumerator _AudioSessionEnumerator;
        internal SessionCollection(IAudioSessionEnumerator realEnumerator)
        {
            _AudioSessionEnumerator = realEnumerator;
        }

        public AudioSessionControl this[int index]
        {
            get
            {
                IAudioSessionControl2 _Result;
                Marshal.ThrowExceptionForHR(_AudioSessionEnumerator.GetSession(index, out _Result));
                return new AudioSessionControl(_Result);
            }
        }

        public int Count
        {
            get
            {
                int result;
                Marshal.ThrowExceptionForHR(_AudioSessionEnumerator.GetCount(out result));
                return (int)result;
            }
        }
    }
    #endregion

    #region SimpleAudioVolume
    public class SimpleAudioVolume
    {
        ISimpleAudioVolume _SimpleAudioVolume;
        internal SimpleAudioVolume(ISimpleAudioVolume realSimpleVolume)
        {
            _SimpleAudioVolume = realSimpleVolume;
        }

        public float MasterVolume
        {
            get
            {
                float ret;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.GetMasterVolume(out ret));
                return ret;
            }
            set
            {
                Guid Empty = Guid.Empty;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.SetMasterVolume(value, ref Empty));
            }
        }

        public bool Mute
        {
            get
            {
                bool ret;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.GetMute(out ret));
                return ret;
            }
            set
            {
                Guid Empty = Guid.Empty;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.SetMute(value, ref Empty));
            }
        }
    }
    #endregion
    #endregion
}
