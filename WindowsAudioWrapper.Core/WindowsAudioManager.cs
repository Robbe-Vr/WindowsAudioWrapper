using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SYWPipeNetworkManager;
using WindowsAudioWrapper.Core.Mixers;
using WindowsAudioWrapper.Core.Inputs;
using WindowsAudioWrapper.Core.Outputs;

namespace WindowsAudioWrapper.Core
{
    public class WindowsAudioManager
    {
        private IEnumerable<string> validatedSources = new List<string>()
        {
            "MidiDomotica"
        };

        public bool Setup()
        {
            PipeMessageControl.Init("WindowsAudio");
            PipeMessageControl.StartClient(
                (sourceName, message) =>
                {
                    if (ValidateSource(sourceName))
                    {
                        return $"{message} -> " + ProcessMessage(message);
                    }
                    else return $"{message} -> NO";
                }
            );

            return true;
        }

        private bool ValidateSource(string source)
        {
            return validatedSources.Contains(source);
        }

        public string ProcessMessage(string message)
        {
            IEnumerable<string> parts = new Regex(@"(::\[|\]::|::)|]$").Split(message).Where(x => !String.IsNullOrWhiteSpace(x) && !x.Contains("::")).Select(x => x.Trim());

            switch (parts.FirstOrDefault())
            {
                case "Get":
                    return ProcessGetCommand(parts.Skip(1));

                case "Set":
                    return ProcessSetCommand(parts.Skip(1));
            }

            return "INVALID DATA";
        }

        private string ProcessGetCommand(IEnumerable<string> parts)
        {
            switch (parts.FirstOrDefault())
            {
                case "Mixer":
                    return WindowsAudioMixersControl.Get(parts.Skip(1));

                case "Output":
                    return WindowsAudioOutputsControl.Get(parts.Skip(1));

                case "Input":
                    return WindowsAudioInputsControl.Get(parts.Skip(1));
            }

            return "UNKNOWN GET";
        }

        private string ProcessSetCommand(IEnumerable<string> parts)
        {
            switch (parts.FirstOrDefault())
            {
                case "Mixer":
                    return WindowsAudioMixersControl.Set(parts.Skip(1));

                case "Output":
                    return WindowsAudioOutputsControl.Set(parts.Skip(1));

                case "Input":
                    return WindowsAudioInputsControl.Set(parts.Skip(1));
            }

            return "UNKNOWN GET";
        }
    }
}
