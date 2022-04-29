﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAudioWrapper.Core
{
    internal class Preferences
    {
        

        internal void Load()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MidiDomotica-WindowsAudioWrapper", "preferences.json");

            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);

                Preferences pref = System.Text.Json.JsonSerializer.Deserialize<Preferences>(jsonString);

            }
            else
            {
                Store();
            }
        }

        internal void Store()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MidiDomotica-WindowsAudioWrapper");

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, "preferences.json");

            string content = System.Text.Json.JsonSerializer.Serialize(this);

            File.WriteAllText(path, content);
        }
    }
}
