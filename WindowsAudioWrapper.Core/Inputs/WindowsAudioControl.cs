using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace WindowsAudioWrapper.Core.Inputs
{
    internal partial class WindowsAudioInputsControl
    {
        internal WindowsAudioInputsControl()
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
                        return InputDeviceControl.SetRecordingDeviceVolume(TranslateDeviceId(parts.FirstOrDefault()), value / 100.00f, InputDeviceControl.VolumeUnit.Scalar);
                    }
                    break;

                case "Mute":
                    string muteValuePart = parts.ElementAtOrDefault(2);
                    if (muteValuePart == "Toggle")
                    {
                        return InputDeviceControl.SetRecordingDeviceMute(TranslateDeviceId(parts.FirstOrDefault()));
                    }
                    else
                    {
                        bool mute;
                        if (bool.TryParse(muteValuePart, out mute))
                        {
                            return InputDeviceControl.SetRecordingDeviceMute(TranslateDeviceId(parts.FirstOrDefault()), mute);
                        }
                    }
                    break;

                case "Solo":
                    string device = TranslateDeviceId(parts.FirstOrDefault());
                    string soloValuePart = parts.ElementAtOrDefault(2);

                    if (soloValuePart == "Toggle")
                    {
                        if (InputDeviceControl.SoloedDevice == device)
                        {
                            return InputDeviceControl.Unsolo();
                        }
                        return InputDeviceControl.SoloDevice(device);
                    }
                    else
                    {
                        bool solo;
                        if (bool.TryParse(soloValuePart, out solo))
                        {
                            return solo ?
                                InputDeviceControl.SoloDevice(device) :
                                InputDeviceControl.Unsolo();
                        }
                    }
                    break;
            }

            return "INVALID DATA";
        }
    }
}
