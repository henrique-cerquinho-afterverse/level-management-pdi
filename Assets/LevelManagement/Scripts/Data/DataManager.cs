using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Data
{
    public class DataManager : MonoBehaviour
    {
        SaveData _saveData;
        JsonSaver _jsonSaver;
        
        public string PlayerName
        {
            get { return _saveData.PlayerName; }
            set { _saveData.PlayerName = value; }
        }
        
        public float MasterVolume
        {
            get { return _saveData.MasterVolume; }
            set { _saveData.MasterVolume = value; }
        }

        public float SfxVolume
        {
            get { return _saveData.SfxVolume; }
            set { _saveData.SfxVolume = value; }
        }
        
        public float MusicVolume
        {
            get { return _saveData.MusicVolume; }
            set { _saveData.MusicVolume = value; }
        }
        
        void Awake()
        {
            _saveData = new SaveData();
            _jsonSaver = new JsonSaver();
        }

        public void Save()
        {
            _jsonSaver.Save(_saveData);
        }

        public void Load()
        {
            _jsonSaver.Load(_saveData);
        }
    }
}
