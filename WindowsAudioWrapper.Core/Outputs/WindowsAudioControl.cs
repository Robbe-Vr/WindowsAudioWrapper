using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace WindowsAudioWrapper.Core.Outputs
{
    internal partial class WindowsAudioOutputsControl
    {
        internal WindowsAudioOutputsControl()
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
                        return OutputDeviceControl.SetOutputDeviceVolume(TranslateDeviceId(parts.FirstOrDefault()), value / 100.00f, OutputDeviceControl.VolumeUnit.Scalar);
                    }
                    break;

                case "Mute":
                    string muteValuePart = parts.ElementAtOrDefault(2);
                    if (muteValuePart == "Toggle")
                    {
                        return OutputDeviceControl.SetOutputDeviceMute(TranslateDeviceId(parts.FirstOrDefault()));
                    }
                    else
                    {
                        bool mute;
                        if (bool.TryParse(muteValuePart, out mute))
                        {
                            return OutputDeviceControl.SetOutputDeviceMute(TranslateDeviceId(parts.FirstOrDefault()), mute);
                        }
                    }
                    break;

                case "Solo":
                    string device = TranslateDeviceId(parts.FirstOrDefault());
                    string soloValuePart = parts.ElementAtOrDefault(2);

                    if (soloValuePart == "Toggle")
                    {
                        if (OutputDeviceControl.SoloedDevice == device)
                        {
                            return OutputDeviceControl.Unsolo();
                        }
                        return OutputDeviceControl.SoloDevice(device);
                    }
                    else
                    {
                        bool solo;
                        if (bool.TryParse(soloValuePart, out solo))
                        {
                            return solo ?
                                OutputDeviceControl.SoloDevice(device) :
                                OutputDeviceControl.Unsolo();
                        }
                    }
                    break;
            }

            return "INVALID DATA";
        }
    }
}
