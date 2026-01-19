using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class AudioSessionInfo
{
    public string Name { get; set; }
    public int Volume { get; set; }
    public bool Muted { get; set; } // Adicionado para o Checkbox
    public string ID { get; set; }  // Adicionado para controle
}

public static class VolumeMixer
{
    // --- 1. GET ALL SESSIONS (Lista Completa com PID e Mute) ---
    public static List<AudioSessionInfo> GetAllSessions()
    {
        var list = new List<AudioSessionInfo>();

        try
        {
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
            IMMDevice speakers;
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            object o;
            // Mantida a lógica original que funciona para você
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out o);
            IAudioSessionManager2 mgr = (IAudioSessionManager2)o;

            IAudioSessionEnumerator sessionEnumerator;
            mgr.GetSessionEnumerator(out sessionEnumerator);
            int count;
            sessionEnumerator.GetCount(out count);

            for (int i = 0; i < count; i++)
            {
                IAudioSessionControl2 ctl;
                sessionEnumerator.GetSession(i, out ctl);

                // 1. Tenta pegar o Nome Visual
                string dn;
                ctl.GetDisplayName(out dn);

                // 2. Tenta pegar o Nome do Processo (PID)
                string processName = "";
                int pid = 0;
                try
                {
                    ctl.GetProcessId(out pid);
                    if (pid > 0)
                    {
                        using (Process p = Process.GetProcessById(pid))
                        {
                            processName = p.ProcessName;
                            if (!string.IsNullOrEmpty(p.MainWindowTitle))
                                processName += $" ({p.MainWindowTitle})";
                        }
                    }
                }
                catch { }

                // 3. Decide qual nome mostrar
                string finalName = "";
                if (!string.IsNullOrEmpty(dn)) finalName = dn;
                else if (!string.IsNullOrEmpty(processName)) finalName = processName;
                else finalName = "Sistema / Desconhecido";

                // Adiciona o nome do executável para facilitar a identificação visual
                if (pid > 0 && finalName != processName) finalName += $" [{processName}.exe]";

                // 4. Pega o Volume e o Mute
                ISimpleAudioVolume volObj = ctl as ISimpleAudioVolume;
                float vol = 0;
                bool muted = false;

                if (volObj != null)
                {
                    volObj.GetMasterVolume(out vol);
                    volObj.GetMute(out muted); // Lê se está mutado
                }

                list.Add(new AudioSessionInfo
                {
                    Name = finalName,
                    Volume = (int)(vol * 100),
                    Muted = muted,
                    ID = (!string.IsNullOrEmpty(processName)) ? processName : finalName
                });

                Marshal.ReleaseComObject(ctl);
            }

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(mgr);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
        }
        catch (Exception ex)
        {
            list.Add(new AudioSessionInfo { Name = "Erro: " + ex.Message });
        }

        return list;
    }

    // --- 2. SET VOLUME ---
    public static void SetApplicationVolume(string appName, float level)
    {
        try
        {
            ISimpleAudioVolume volume = GetVolumeObject(appName);
            if (volume == null) return;
            Guid guid = Guid.Empty;
            volume.SetMasterVolume(level / 100, ref guid);
            Marshal.ReleaseComObject(volume);
        }
        catch { }
    }

    // --- 3. SET MUTE (Novo) ---
    public static void SetApplicationMute(string appName, bool mute)
    {
        try
        {
            ISimpleAudioVolume volume = GetVolumeObject(appName);
            if (volume == null) return;
            Guid guid = Guid.Empty;
            volume.SetMute(mute, ref guid);
            Marshal.ReleaseComObject(volume);
        }
        catch { }
    }

    // --- 4. GET VOLUME ---
    public static float? GetApplicationVolume(string appName)
    {
        try
        {
            ISimpleAudioVolume volume = GetVolumeObject(appName);
            if (volume == null) return null;
            float level;
            volume.GetMasterVolume(out level);
            Marshal.ReleaseComObject(volume);
            return level * 100;
        }
        catch { return null; }
    }

    // --- 5. GET MUTE (Novo) ---
    public static bool? GetApplicationMute(string appName)
    {
        try
        {
            ISimpleAudioVolume volume = GetVolumeObject(appName);
            if (volume == null) return null;
            bool muted;
            volume.GetMute(out muted);
            Marshal.ReleaseComObject(volume);
            return muted;
        }
        catch { return null; }
    }

    // --- HELPER: Encontra o objeto de volume (Lógica idêntica à funcional) ---
    private static ISimpleAudioVolume GetVolumeObject(string name)
    {
        try
        {
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
            IMMDevice speakers;
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            object o;
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out o);
            IAudioSessionManager2 mgr = (IAudioSessionManager2)o;

            IAudioSessionEnumerator sessionEnumerator;
            mgr.GetSessionEnumerator(out sessionEnumerator);
            int count;
            sessionEnumerator.GetCount(out count);

            ISimpleAudioVolume volumeControl = null;
            for (int i = 0; i < count; i++)
            {
                IAudioSessionControl2 ctl;
                sessionEnumerator.GetSession(i, out ctl);

                string dn; ctl.GetDisplayName(out dn);
                int pid; ctl.GetProcessId(out pid);
                string procName = "";
                if (pid > 0) try { using (Process p = Process.GetProcessById(pid)) procName = p.ProcessName; } catch { }

                bool match = false;
                if (!string.IsNullOrEmpty(dn) && dn.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) match = true;
                if (!match && !string.IsNullOrEmpty(procName) && procName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) match = true;

                if (match)
                {
                    volumeControl = ctl as ISimpleAudioVolume;
                }

                if (volumeControl != null && (object)volumeControl == (object)ctl)
                {
                    Marshal.ReleaseComObject(sessionEnumerator);
                    Marshal.ReleaseComObject(mgr);
                    Marshal.ReleaseComObject(speakers);
                    Marshal.ReleaseComObject(deviceEnumerator);
                    return volumeControl;
                }
                Marshal.ReleaseComObject(ctl);
            }

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(mgr);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
            return null;
        }
        catch { return null; }
    }


    // ==========================================
    //    INTERFACES COM (Exatamente as que funcionaram)
    // ==========================================

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator { }

    internal enum EDataFlow { eRender, eCapture, eAll, EDataFlow_enum_count }
    internal enum ERole { eConsole, eMultimedia, eCommunications, ERole_enum_count }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int NotImpl1();
        [PreserveSig] int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig] int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
    }

    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionManager2
    {
        int NotImpl1();
        int NotImpl2();
        [PreserveSig] int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
    }

    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        [PreserveSig] int GetCount(out int SessionCount);
        [PreserveSig] int GetSession(int SessionCount, out IAudioSessionControl2 Session);
    }

    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionControl2
    {
        [PreserveSig] int GetState(out int pRetVal);
        [PreserveSig] int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);
        [PreserveSig] int SetDisplayName(IntPtr Value, IntPtr EventContext);
        [PreserveSig] int GetIconPath(out IntPtr pRetVal);
        [PreserveSig] int SetIconPath(IntPtr Value, IntPtr EventContext);
        [PreserveSig] int GetGroupingParam(out IntPtr pRetVal);
        [PreserveSig] int SetGroupingParam(IntPtr Override, IntPtr EventContext);
        [PreserveSig] int RegisterAudioSessionNotification(IntPtr NewNotifications);
        [PreserveSig] int UnregisterAudioSessionNotification(IntPtr NewNotifications);
        [PreserveSig] int GetSessionIdentifier(out IntPtr pRetVal);
        [PreserveSig] int GetSessionInstanceIdentifier(out IntPtr pRetVal);
        [PreserveSig] int GetProcessId(out int pRetVal);
    }

    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISimpleAudioVolume
    {
        [PreserveSig] int SetMasterVolume(float fLevel, ref Guid EventContext);
        [PreserveSig] int GetMasterVolume(out float pfLevel);
        [PreserveSig] int SetMute(bool bMute, ref Guid EventContext);
        [PreserveSig] int GetMute(out bool pbMute);
    }
}