using System;
using System.Collections;
using System.Collections.Generic;
using LevelManagement.Data;
using UnityEngine;
using UnityEngine.UI;
using SampleGame;

namespace LevelManagement
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] Slider _masterSlider;
        [SerializeField] Slider _sfxSlider;
        [SerializeField] Slider _musicSlider;

        DataManager _dataManager;

        protected override void Awake()
        {
            base.Awake();
            _dataManager = FindObjectOfType<DataManager>();
        }

        void Start()
        {
            LoadData();
        }

        public void OnMasterVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.MasterVolume = volume;
            }
        }
        public void OnSfxVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.SfxVolume = volume;
            }
        }
        public void OnMusicVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.MusicVolume = volume;
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if (_dataManager != null)
            {
                _dataManager.Save();
            }
        }

        public void LoadData()
        {
            if (_dataManager == null || _masterSlider == null || _sfxSlider == null || _musicSlider == null) return;
            _dataManager.Load();
            
            _masterSlider.value = _dataManager.MasterVolume;
            _sfxSlider.value = _dataManager.SfxVolume;
            _musicSlider.value = _dataManager.MusicVolume;
        }
    }
}
