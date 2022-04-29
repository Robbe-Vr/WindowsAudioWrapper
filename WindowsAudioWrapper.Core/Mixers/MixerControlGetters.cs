using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core.Mixers
{
    internal static partial class MixerControl
    {
        internal static bool VolumeChanging { get; private set; }

        internal static int SoloedMixer { get; private set; }

        private static readonly List<int> mutedBySolo = new List<int>();

        public enum VolumeUnit
        {
            Decibel,
            Scalar
        }

        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern float GetVolumeOfMixer(int mixerId, VolumeUnit vUnit);


        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMuteMixer(int mixerId);

        
        [DllImport(@"C:\\Users\\Robbe\source\\repos\WindowsVolumeChangerDLL\\Debug\\WindowsVolumeChangerDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetMixers(StringBuilder mixersStr, int length);


        internal static string GetAppMixers()
        {
            StringBuilder strB = new StringBuilder(2000);
            GetMixers(strB, strB.Capacity);

            return strB.ToString();
        }

        internal static float GetMixerVolume(int mixerId, VolumeUnit vUnit)
        {
            if (mixerId == -1) return 0;

            return GetVolumeOfMixer(mixerId, vUnit);
        }

        internal static bool GetMixerMute(int mixerId)
        {
            if (mixerId == -1) return false;

            return GetMuteMixer(mixerId) == 1;
        }
    }
}
