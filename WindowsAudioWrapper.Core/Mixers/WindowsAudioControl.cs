using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace WindowsAudioWrapper.Core.Mixers
{
    internal partial class WindowsAudioMixersControl
    {
        internal WindowsAudioMixersControl()
        {
            
        }

        public static string Set(IEnumerable<string> parts)
        {
            switch (parts.ElementAtOrDefault(1))
            {
                case "Volume":
                    int value;
                    if (int.TryParse(parts.ElementAtOrDefault(2), out value))
                    {
                        return MixerControl.SetMixerVolume(TranslateMixerId(parts.FirstOrDefault()), value / 100.00f, MixerControl.VolumeUnit.Scalar);
                    }
                    break;

                case "Mute":
                    string muteValuePart = parts.ElementAtOrDefault(2);
                    if (muteValuePart == "Toggle")
                    {
                        return MixerControl.SetMuteMixer(TranslateMixerId(parts.FirstOrDefault()));
                    }
                    else
                    {
                        bool mute;
                        if (bool.TryParse(muteValuePart, out mute))
                        {
                            return MixerControl.SetMuteMixer(TranslateMixerId(parts.FirstOrDefault()), mute);
                        }
                    }
                    break;

                case "Solo":
                    int mixerId = TranslateMixerId(parts.FirstOrDefault());
                    if (mixerId == -1) return "INVALID DATA";

                    string soloValuePart = parts.ElementAtOrDefault(2);

                    if (soloValuePart == "Toggle")
                    {
                        if (MixerControl.SoloedMixer == mixerId)
                        {
                            return MixerControl.Unsolo();
                        }
                        return MixerControl.SoloMixer(mixerId);
                    }
                    else
                    {
                        bool solo;
                        if (bool.TryParse(soloValuePart, out solo))
                        {
                            return solo ?
                                MixerControl.SoloMixer(mixerId) :
                                MixerControl.Unsolo();
                        }
                    }
                    break;
            }

            return "INVALID DATA";
        }
    }
}
