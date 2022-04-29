using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace WindowsAudioWrapper.Core.Mixers
{
    internal partial class WindowsAudioMixersControl
    {
        public static string Get(IEnumerable<string> parts)
        {
            switch (parts.FirstOrDefault())
            {
                case "All":
                    return String.Join(',', GetMixers());

                case "Name":
                    return GetName(parts.Skip(1)?.FirstOrDefault());

                case "Id":
                    return GetId(parts.Skip(1)?.FirstOrDefault());

                case "Volume":
                    return (MixerControl.GetMixerVolume(TranslateMixerId(parts.Skip(1)?.FirstOrDefault()), MixerControl.VolumeUnit.Scalar) * 100).ToString();

                case "Mute":
                    return MixerControl.GetMixerMute(TranslateMixerId(parts.Skip(1)?.FirstOrDefault())).ToString().ToUpper();

                case "Solo":
                    return (MixerControl.SoloedMixer == TranslateMixerId(parts.Skip(1)?.FirstOrDefault())).ToString().ToUpper();
            }

            return "UNKNOWN GET";
        }

        public static string GetId(string name)
        {
            return GetMixers().FirstOrDefault(x => { string[] data = x.Split(':'); return data[1] == name; }) ?? "";
        }

        public static string GetName(string id)
        {
            return GetMixers().FirstOrDefault(x => { string[] data = x.Split(':'); return data[0] == id; }) ?? "";
        }

        public static IEnumerable<string> GetMixers()
        {
            return MixerControl.GetAppMixers().Split(", ").Select(x =>
            {
                string[] data = x.Split(" - ");

                string processId = data[0].Trim();
                string name = String.Join(" - ", data.Skip(1)).Trim().Replace(',', '_');

                if (String.IsNullOrWhiteSpace(name))
                {
                    name = GetProcessName(processId.All(c => char.IsDigit(c)) ? int.Parse(processId) : -1);
                }
                return $"{processId}:{name}";
            });
        }

        private static int TranslateMixerId(string mixerInfo)
        {
            string id = GetMixers().FirstOrDefault(x => { string[] data = x.Split(':'); return data[0] == mixerInfo || data[1] == mixerInfo; })?.Split(':')[0] ?? string.Empty;

            int mixerId;
            if (int.TryParse(id, out mixerId))
            {
                return mixerId;
            }

            return -1;
        }

        private static string GetProcessName(int processId)
        {
            if (processId < 0) return "%UNKNOWN%";

            try
            {
                Process process = Process.GetProcessById(processId);

                return process == null ? "%UNKNOWN%" : process.ProcessName;
            }
            catch (Exception e)
            {
                Logger.Log("Could not get process name for process with id: " + processId + "!\nError: " + e.Message);
                return "%ERROR%";
            }
        }
    }
}
