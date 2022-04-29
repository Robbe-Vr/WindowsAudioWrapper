using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Inputs
{
    internal partial class WindowsAudioInputsControl
    {
        public static string Get(IEnumerable<string> parts)
        {
            switch (parts.FirstOrDefault())
            {
                case "All":
                    return String.Join(',', GetDevices());

                case "Name":
                    return GetName(parts.Skip(1)?.FirstOrDefault());

                case "Id":
                    return GetId(parts.Skip(1)?.FirstOrDefault());

                case "Volume":
                    return (InputDeviceControl.GetRecordingDeviceVolume(TranslateDeviceId(parts.Skip(1)?.FirstOrDefault()), InputDeviceControl.VolumeUnit.Scalar) * 100).ToString();

                case "Mute":
                    return InputDeviceControl.GetRecordingDeviceMute(TranslateDeviceId(parts.Skip(1)?.FirstOrDefault())).ToString().ToUpper();

                case "Solo":
                    string deviceId = parts.Skip(1)?.FirstOrDefault();
                    return (InputDeviceControl.SoloedDevice == (deviceId == "Default" ? InputDeviceControl.GetMainDevice() : TranslateDeviceId(deviceId))).ToString().ToUpper();
            }

            return "UNKNOWN GET";
        }

        public static string GetId(string name)
        {
            if (name == "Default") return InputDeviceControl.GetMainDevice().Split(" - ")?.ElementAtOrDefault(1) ?? string.Empty;

            return GetDevices().FirstOrDefault(x => { string[] data = x.Split(':'); return data[1] == name; }) ?? string.Empty;
        }

        public static string GetName(string id)
        {
            if (id == "Default") return InputDeviceControl.GetMainDevice().Split(" - ")?.FirstOrDefault() ?? string.Empty;

            return GetDevices().FirstOrDefault(x => { string[] data = x.Split(':'); return data[0] == id; }) ?? string.Empty;
        }

        public static IEnumerable<string> GetDevices()
        {
            return InputDeviceControl.GetDevices().Split(", ").Select(x =>
            {
                string[] data = x.Split(" - ");

                string id = data.Last().Trim();
                string name = String.Join(" - ", data.SkipLast(1)).Trim().Replace(',', '_');

                return $"{id}:{name}";
            });
        }

        private static string TranslateDeviceId(string deviceInfo)
        {
            if (deviceInfo == "Default") return deviceInfo;
            return GetDevices().FirstOrDefault(x => { string[] data = x.Split(':'); return data[0] == deviceInfo || data[1] == deviceInfo; })?.Split(':')[0] ?? "INVALID DATA";
        }
    }
}
