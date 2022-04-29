using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Outputs
{
    internal static partial class OutputDeviceControl
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
        private static extern float GetMainOutputDeviceVolume(VolumeUnit vUnit);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern float GetVolumeOfOutputDevice(string deviceId, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMuteMainOutputDevice();

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMuteOutputDevice(string deviceId);

        
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetOutputDevices(StringBuilder devicesStr, int length);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetMainOutputDevice(StringBuilder deviceStr, int length);


        internal static string GetDevices()
        {
            StringBuilder strB = new StringBuilder(2000);
            GetOutputDevices(strB, strB.Capacity);

            return strB.ToString();
        }

        internal static string GetMainDevice()
        {
            StringBuilder strB = new StringBuilder(2000);
            GetMainOutputDevice(strB, strB.Capacity);

            return strB.ToString();
        }

        internal static float GetOutputDeviceVolume(string deviceId, VolumeUnit vUnit)
        {
            return deviceId == "Default" ? GetMainOutputDeviceVolume(vUnit) : GetVolumeOfOutputDevice(deviceId, vUnit);
        }

        internal static bool GetOutputDeviceMute(string deviceId)
        {
            return (deviceId == "Default" ? GetMuteMainOutputDevice() : GetMuteOutputDevice(deviceId)) == 1;
        }
    }
}
