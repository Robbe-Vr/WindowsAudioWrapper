using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Outputs
{
    internal static partial class OutputDeviceControl
    {
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetMainOutputDeviceVolume(float newVolume, VolumeUnit vUnit);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetVolumeForOutputDevice(string deviceId, float newVolume, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MuteMainOutputDevice(bool mute);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MuteOutputDevice(string deviceId, bool mute);

        public static string SetOutputDeviceVolume(string deviceId, float volume, VolumeUnit vUnit)
        {
            while (VolumeChanging)
            {
                Thread.Sleep(5);
            }

            VolumeChanging = true;

            if (deviceId == "Default")
            {
                SetMainOutputDeviceVolume(volume, vUnit);
            }
            else
            {
                SetVolumeForOutputDevice(deviceId, volume, vUnit);
            }

            VolumeChanging = false;

            return "EXECUTED";
        }

        public static string SetOutputDeviceMute(string deviceId, bool? mute = null)
        {
            if (deviceId == "Default")
            {
                MuteMainOutputDevice(mute ?? !GetOutputDeviceMute(deviceId));
            }
            else
            {
                MuteOutputDevice(deviceId, mute ?? !GetOutputDeviceMute(deviceId));
            }

            return "EXECUTED";
        }

        public static string SoloDevice(string deviceId)
        {
            if (SoloedDevice != null)
            {
                Unsolo();
            }

            mutedBySolo.Clear();

            if (deviceId == "Default")
            {
                deviceId = GetMainDevice();
            }

            foreach (string device in GetDevices().Split(", ").Select(x => x.Split(" - ")?.FirstOrDefault()))
            {
                bool muted = GetOutputDeviceMute(device);

                if (device == deviceId && muted)
                {
                    SetOutputDeviceMute(deviceId, mute: false);
                }
                else if (!muted)
                {
                    SetOutputDeviceMute(device, mute: true);
                    mutedBySolo.Add(device);
                }
            }

            SoloedDevice = deviceId;

            return "EXECUTED";
        }

        public static string Unsolo()
        {
            foreach (string deviceId in mutedBySolo)
            {
                SetOutputDeviceMute(deviceId, mute: false);
            }
            mutedBySolo.Clear();
            SoloedDevice = null;

            return "EXECUTED";
        }
    }
}
