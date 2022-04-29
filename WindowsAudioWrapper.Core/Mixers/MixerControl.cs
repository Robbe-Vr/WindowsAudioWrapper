using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Mixers
{
    internal static partial class MixerControl
    {
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetVolumeForMixer(int mixerId, float newVolume, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MuteMixer(int mixerId, bool mute);

        public static string SetMixerVolume(int mixerId, float volume, VolumeUnit vUnit)
        {
            if (mixerId == -1) return "INVALID DATA";

            while (VolumeChanging)
            {
                Thread.Sleep(5);
            }

            VolumeChanging = true;

            SetVolumeForMixer(mixerId, volume, vUnit);

            VolumeChanging = false;

            return "EXECUTED";
        }

        public static string SetMuteMixer(int mixerId, bool? mute = null)
        {
            if (mixerId == -1) return "INVALID DATA";

            MuteMixer(mixerId, mute ?? !GetMixerMute(mixerId));

            return "EXECUTED";
        }

        public static string SoloMixer(int soloMixerId)
        {
            if (soloMixerId == -1) return "INVALID DATA";

            if (SoloedMixer != -1)
            {
                Unsolo();
            }

            mutedBySolo.Clear();

            foreach (string mixer in GetAppMixers().Split(", ").Select(x => x.Split(" - ")?.FirstOrDefault()))
            {
                int mixerId;
                if (int.TryParse(mixer, out mixerId))
                {
                    bool muted = GetMixerMute(mixerId);

                    if (mixerId == soloMixerId && muted)
                    {
                        MuteMixer(soloMixerId, mute: false);
                    }
                    else if (!muted)
                    {
                        MuteMixer(mixerId, mute: true);
                        mutedBySolo.Add(mixerId);
                    }
                }
            }

            SoloedMixer = soloMixerId;

            return "EXECUTED";
        }

        public static string Unsolo()
        {
            foreach (int mixerId in mutedBySolo)
            {
                MuteMixer(mixerId, mute: false);
            }
            mutedBySolo.Clear();
            SoloedMixer = -1;

            return "EXECUTED";
        }
    }
}
