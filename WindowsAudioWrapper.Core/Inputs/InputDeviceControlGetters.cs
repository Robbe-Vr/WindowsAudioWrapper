using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Inputs
{
    internal static partial class InputDeviceControl
    {
        internal static bool VolumeChanging { get; private set; }

        internal static string SoloedDevice { get; private set; }

        private static readonly List<string> mutedBySolo = new List<string>();

        public enum VolumeUnit
        {
            Decibel,
            Scalar
        }

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern float GetMainRecordingDeviceVolume(VolumeUnit vUnit);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern float GetVolumeOfRecordingDevice(string deviceId, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMuteMainRecordingDevice();

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMuteRecordingDevice(string deviceId);

        
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetRecordingDevices(StringBuilder devicesStr, int length);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetMainRecordingDevice(StringBuilder deviceStr, int length);


        internal static string GetDevices()
        {
            StringBuilder strB = new StringBuilder(2000);
            GetRecordingDevices(strB, strB.Capacity);

            return strB.ToString();
        }

        internal static string GetMainDevice()
        {
            StringBuilder strB = new StringBuilder(2000);
            GetMainRecordingDevice(strB, strB.Capacity);

            return strB.ToString();
        }

        internal static float GetRecordingDeviceVolume(string deviceId, VolumeUnit vUnit)
        {
            return deviceId == "Default" ? GetMainRecordingDeviceVolume(vUnit) : GetVolumeOfRecordingDevice(deviceId, vUnit);
        }

        internal static bool GetRecordingDeviceMute(string deviceId)
        {
            return (deviceId == "Default" ? GetMuteMainRecordingDevice() : GetMuteRecordingDevice(deviceId)) == 1;
        }
    }
}
