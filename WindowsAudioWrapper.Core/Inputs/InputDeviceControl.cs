using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Inputs
{
    internal static partial class InputDeviceControl
    {
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetMainRecordingDeviceVolume(float newVolume, VolumeUnit vUnit);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetVolumeForRecordingDevice(string deviceId, float newVolume, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MuteMainRecordingDevice(bool mute);

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MuteRecordingDevice(string deviceId, bool mute);

        public static string SetRecordingDeviceVolume(string deviceId, float volume, VolumeUnit vUnit)
        {
            while (VolumeChanging)
            {
                Thread.Sleep(5);
            }

            VolumeChanging = true;

            if (deviceId == "Default")
            {
                SetMainRecordingDeviceVolume(volume, vUnit);
            }
            else
            {
                SetVolumeForRecordingDevice(deviceId, volume, vUnit);
            }

            VolumeChanging = false;

            return "EXECUTED";
        }

        public static string SetRecordingDeviceMute(string deviceId, bool? mute = null)
        {
            if (deviceId == "Default")
            {
                MuteMainRecordingDevice(mute ?? !GetRecordingDeviceMute(deviceId));
            }
            else
            {
                MuteRecordingDevice(deviceId, mute ?? !GetRecordingDeviceMute(deviceId));
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
                bool muted = GetRecordingDeviceMute(device);

                if (device == deviceId && muted)
                {
                    SetRecordingDeviceMute(deviceId, mute: false);
                }
                else if (!muted)
                {
                    SetRecordingDeviceMute(device, mute: true);
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
                SetRecordingDeviceMute(deviceId, mute: false);
            }
            mutedBySolo.Clear();
            SoloedDevice = null;

            return "EXECUTED";
        }
    }
}
