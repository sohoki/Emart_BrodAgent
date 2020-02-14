﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;


namespace sohoUniLib
{
    public class WindowInfo
    {
        public static List<hddInfo> comHddInfo()
        {
            List<hddInfo> Info = new List<WindowInfo.hddInfo>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach(DriveInfo drive in allDrives)
            {
                hddInfo hdd = new hddInfo();
                hdd.Drive = drive.Name;
                hdd.File_type = drive.DriveType.ToString();
                hdd.Volume_label = drive.VolumeLabel;
                hdd.TotalFreeSpace = Convert.ToString(drive.TotalFreeSpace);
                hdd.totalSize = Convert.ToString(drive.TotalSize);
                Info.Add(hdd);

            }            
            return Info;            
        }

        public class hddInfo
        {
            public string Drive { get; set; }
            public string File_type { get; set; }
            public string Volume_label { get; set; }
            public string TotalFreeSpace { get; set; }
            public string totalSize { get; set; }

        }

    }
    public sealed class SoundUtil
    {
        //public const int MMSYSERR_NOERROR = 0;
        //public const int MAXPNAMELEN = 32;
        //public const int MIXER_LONG_NAME_CHARS = 64;
        //public const int MIXER_SHORT_NAME_CHARS = 16;
        //public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 0x3;
        //public const int MIXER_GETCONTROLDETAILSF_VALUE = 0x0;
        //public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x2;
        //public const int MIXER_SETCONTROLDETAILSF_VALUE = 0x0;
        //public const int MIXERLINE_COMPONENTTYPE_DST_FIRST = 0x0;
        //public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x1000;
        //public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
        //public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
        //public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);
        //public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
        //public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x30000;
        //public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
        //public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerClose(int hmx);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetControlDetailsA(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetDevCapsA(int uMxId, MIXERCAPS pmxcaps, int cbmxcaps);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetID(int hmxobj, int pumxID, int fdwId);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetLineControlsA(int hmxobj, ref MIXERLINECONTROLS pmxlc, int fdwControls);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetLineInfoA(int hmxobj, ref MIXERLINE pmxl, int fdwInfo);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerGetNumDevs();

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerMessage(int hmx, int uMsg, int dwParam1, int dwParam2);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerOpen(out int phmx, int uMxId, int dwCallback, int dwInstance, int fdwOpen);

        //[DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        //private static extern int mixerSetControlDetails(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        //[DllImport("winmm.dll", EntryPoint = "sndPlaySoundA")]

        //private static extern int sndPlaySound(string lpszSoundName, int uFlags);

        //public enum SND
        //{
        //    SND_SYNC = 0x0000,/* play synchronously (default) */
        //    SND_ASYNC = 0x0001, /* play asynchronously */
        //    SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
        //    SND_MEMORY = 0x0004, /* pszSound points to a memory file */
        //    SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
        //    SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
        //    SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
        //    SND_ALIAS = 0x00010000,/* name is a registry alias */
        //    SND_ALIAS_ID = 0x00110000, /* alias is a pre d ID */
        //    SND_FILENAME = 0x00020000, /* name is file name */
        //    SND_RESOURCE = 0x00040004, /* name is resource name or atom */
        //    SND_PURGE = 0x0040,  /* purge non-static events for task */
        //    SND_APPLICATION = 0x0080  /* look for application specific association */
        //}

        //public struct MIXERCAPS
        //{
        //    public int wMid;
        //    public int wPid;
        //    public int vDriverVersion;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
        //    public string szPname;
        //    public int fdwSupport;
        //    public int cDestinations;
        //}

        //public struct MIXERCONTROL
        //{
        //    public int cbStruct;
        //    public int dwControlID;
        //    public int dwControlType;
        //    public int fdwControl;
        //    public int cMultipleItems;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
        //    public string szShortName;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
        //    public string szName;
        //    public int lMinimum;
        //    public int lMaximum;
        //    [MarshalAs(UnmanagedType.U4, SizeConst = 10)]
        //    public int reserved;
        //}

        //public struct MIXERCONTROLDETAILS
        //{
        //    public int cbStruct;
        //    public int dwControlID;
        //    public int cChannels;
        //    public int item;
        //    public int cbDetails;
        //    public IntPtr paDetails;
        //}

        //public struct MIXERCONTROLDETAILS_UNSIGNED
        //{
        //    public int dwValue;
        //}

        //public struct MIXERLINE
        //{
        //    public int cbStruct;
        //    public int dwDestination;
        //    public int dwSource;
        //    public int dwLineID;
        //    public int fdwLine;
        //    public int dwUser;
        //    public int dwComponentType;
        //    public int cChannels;
        //    public int cConnections;
        //    public int cControls;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
        //    public string szShortName;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
        //    public string szName;
        //    public int dwType;
        //    public int dwDeviceID;
        //    public int wMid;
        //    public int wPid;
        //    public int vDriverVersion;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
        //    public string szPname;
        //}

        //public struct MIXERLINECONTROLS
        //{
        //    public int cbStruct;
        //    public int dwLineID;

        //    public int dwControl;
        //    public int cControls;
        //    public int cbmxctrl;
        //    public IntPtr pamxctrl;
        //}

        //private static bool GetVolumeControl(int hmixer, int componentType, int ctrlType, out MIXERCONTROL mxc, out int vCurrentVol)
        //{
        //    // This function attempts to obtain a mixer control. 
        //    // Returns True if successful. 
        //    MIXERLINECONTROLS mxlc = new MIXERLINECONTROLS();
        //    MIXERLINE mxl = new MIXERLINE();
        //    MIXERCONTROLDETAILS pmxcd = new MIXERCONTROLDETAILS();
        //    MIXERCONTROLDETAILS_UNSIGNED du = new MIXERCONTROLDETAILS_UNSIGNED();
        //    mxc = new MIXERCONTROL();
        //    int rc;
        //    bool retValue;
        //    vCurrentVol = -1;

        //    mxl.cbStruct = Marshal.SizeOf(mxl);
        //    mxl.dwComponentType = componentType;

        //    rc = mixerGetLineInfoA(hmixer, ref mxl,MIXER_GETLINEINFOF_COMPONENTTYPE);

        //    if (MMSYSERR_NOERROR == rc)
        //    {
        //        int sizeofMIXERCONTROL = 152;
        //        int ctrl = Marshal.SizeOf(typeof(MIXERCONTROL));
        //        mxlc.pamxctrl = Marshal.AllocCoTaskMem(sizeofMIXERCONTROL);
        //        mxlc.cbStruct = Marshal.SizeOf(mxlc);
        //        mxlc.dwLineID = mxl.dwLineID;
        //        mxlc.dwControl = ctrlType;
        //        mxlc.cControls = 1;
        //        mxlc.cbmxctrl = sizeofMIXERCONTROL;

        //        // Allocate a buffer for the control 
        //        mxc.cbStruct = sizeofMIXERCONTROL;

        //        // Get the control 
        //        rc = mixerGetLineControlsA(hmixer, ref mxlc,
        //            MIXER_GETLINECONTROLSF_ONEBYTYPE);

        //        if (MMSYSERR_NOERROR == rc)
        //        {
        //            retValue = true;

        //            // Copy the control into the destination structure 
        //            mxc = (MIXERCONTROL)Marshal.PtrToStructure( mxlc.pamxctrl, typeof(MIXERCONTROL));
        //        }
        //        else
        //        {
        //            retValue = false;
        //        }
        //        int sizeofMIXERCONTROLDETAILS = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS));
        //        int sizeofMIXERCONTROLDETAILS_UNSIGNED = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED));
        //        pmxcd.cbStruct = sizeofMIXERCONTROLDETAILS;
        //        pmxcd.dwControlID = mxc.dwControlID;
        //        pmxcd.paDetails =

        //        Marshal.AllocCoTaskMem(sizeofMIXERCONTROLDETAILS_UNSIGNED);
        //        pmxcd.cChannels = 1;
        //        pmxcd.item = 0;
        //        pmxcd.cbDetails = sizeofMIXERCONTROLDETAILS_UNSIGNED;

        //        rc = mixerGetControlDetailsA(hmixer, ref pmxcd,
        //            MIXER_GETCONTROLDETAILSF_VALUE);

        //        du = (MIXERCONTROLDETAILS_UNSIGNED)Marshal.PtrToStructure( pmxcd.paDetails, typeof(MIXERCONTROLDETAILS_UNSIGNED));

        //        vCurrentVol = du.dwValue;

        //        return retValue;
        //    }

        //    retValue = false;
        //    return retValue;
        //}

        //private static bool SetVolumeControl(int hmixer, MIXERCONTROL mxc, int volume)
        //{
        //    // This function sets the value for a volume control. 
        //    // Returns True if successful 

        //    bool retValue;
        //    int rc;
        //    MIXERCONTROLDETAILS mxcd = new MIXERCONTROLDETAILS();
        //    MIXERCONTROLDETAILS_UNSIGNED vol = new MIXERCONTROLDETAILS_UNSIGNED();

        //    mxcd.item = 0;
        //    mxcd.dwControlID = mxc.dwControlID;
        //    mxcd.cbStruct = Marshal.SizeOf(mxcd);
        //    mxcd.cbDetails = Marshal.SizeOf(vol);

        //    // Allocate a buffer for the control value buffer 
        //    mxcd.cChannels = 1;
        //    vol.dwValue = volume;

        //    // Copy the data into the control value buffer 
        //    mxcd.paDetails = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED)));
        //    Marshal.StructureToPtr(vol, mxcd.paDetails, false);

        //    // Set the control value 
        //    rc = mixerSetControlDetails(hmixer, ref mxcd, MIXER_SETCONTROLDETAILSF_VALUE);

        //    if (MMSYSERR_NOERROR == rc)
        //    {
        //        retValue = true;
        //    }
        //    else
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        ///// <summary>
        ///// 현재의 볼륨을 가져옴
        ///// </summary>
        ///// <returns></returns>
        //public static int GetVolume()
        //{
        //    int mixer;
        //    int currentVol;
        //    MIXERCONTROL volCtrl = new MIXERCONTROL();
        //    mixerOpen(out mixer, 0, 0, 0, 0);
        //    int type = MIXERCONTROL_CONTROLTYPE_VOLUME;
        //    GetVolumeControl(mixer, MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
        //    mixerClose(mixer);

        //    return currentVol;
        //}

        ///// <summary>
        ///// 볼륨값을 설정
        ///// </summary>
        ///// <param name="vVolume"></param>
        //public static void SetVolume(int vVolume)
        //{
        //    int mixer;
        //    MIXERCONTROL volCtrl = new MIXERCONTROL();
        //    int currentVol;
        //    mixerOpen(out mixer, 0, 0, 0, 0);
        //    int type = MIXERCONTROL_CONTROLTYPE_VOLUME;
        //    GetVolumeControl(mixer, MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
        //    if (vVolume > volCtrl.lMaximum) vVolume = volCtrl.lMaximum;
        //    if (vVolume < volCtrl.lMinimum) vVolume = volCtrl.lMinimum;
        //    SetVolumeControl(mixer, volCtrl, vVolume);
        //    GetVolumeControl(mixer, MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
        //    if (vVolume != currentVol)
        //    {
        //        throw new Exception("Cannot Set Volume");
        //    }
        //    mixerClose(mixer);
        //}

        ///// <summary>
        ///// 사운드볼륨을 퍼센트 단위로 설정
        ///// </summary>
        ///// <param name="iPercent">볼륨 퍼센트</param>
        //public static void SetVolumePercent(int iPercent)
        //{
        //    int iVolumn = 0;
        //    int mixer;
        //    int currentVol;

        //    if (iPercent < 0)
        //        iPercent = 0;
        //    else if (iPercent > 100)
        //        iPercent = 100;

        //    MIXERCONTROL volCtrl = new MIXERCONTROL();
        //    mixerOpen(out mixer, 0, 0, 0, 0);
        //    int type = MIXERCONTROL_CONTROLTYPE_VOLUME;
        //    GetVolumeControl(mixer, MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);

        //    iVolumn = volCtrl.lMaximum * iPercent / 100;

        //    SetVolumeControl(mixer, volCtrl, iVolumn);
        //    GetVolumeControl(mixer, MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
        //    if (iVolumn != currentVol)
        //    {
        //        throw new Exception("Cannot Set Volume");
        //    }
        //    mixerClose(mixer);

        //}

        ///// <summary>
        ///// WAV파일 수행
        ///// </summary>
        ///// <param name="filename">WAV파일명 또는 경로+파일명</param>
        ///// <param name="loop">true:무한제생, false:1번재생</param>
        //public static void PlaySound(string filename, bool loop)
        //{
        //    if (File.Exists(filename))
        //    {
        //        if (loop) 
        //            sndPlaySound(filename, (int)SND.SND_ASYNC | (int)SND.SND_LOOP);
        //        else
        //            sndPlaySound(filename, (int)SND.SND_ASYNC);
        //    }
        //}
        #region 사운드 관련
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        public static void SetSoundVolume(int volume)
        {
            try
            {
                int newVolume = ((ushort.MaxValue / 10) * volume);
                uint newVolumeAllChannels = (((uint)newVolume & 0x0000ffff) | ((uint)newVolume << 16));
                waveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);
            }
            catch (Exception) { }
        }

        public static int GetSoundVolume()
        {
            int value = 0;
            try
            {
                uint CurrVol = 0;
                waveOutGetVolume(IntPtr.Zero, out CurrVol);
                ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
                value = CalcVol / (ushort.MaxValue /100);
            }
            catch (Exception) { }
            return value;
        }
        #endregion
        
    }
}
