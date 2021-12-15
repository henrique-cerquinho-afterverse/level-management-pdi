using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LevelManagement.Data
{
    [Serializable]
    public class SaveData
    {
        public string PlayerName;
        readonly string _defaultPlayerName = "Player";

        public float MasterVolume;
        public float SfxVolume;
        public float MusicVolume;

        public string HashValue;

        public SaveData()
        {
            PlayerName = _defaultPlayerName;
            MasterVolume = 0f;
            SfxVolume = 0f;
            MusicVolume = 0f;
            HashValue = string.Empty;
        }
    }
}
