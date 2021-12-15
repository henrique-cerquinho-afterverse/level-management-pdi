using System.Collections;
using System.Collections.Generic;
using LevelManagement.Data;
using UnityEngine;
using SampleGame;
using UnityEngine.UI;

namespace LevelManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] float _playDelay = 0.5f;
        [SerializeField] TransitionFader _startTransitionPrefab;
        [SerializeField] InputField _playerNameInputField;

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
        
        public void OnPlayPressed()
        {
            StartCoroutine(OnPlayPressedRoutine());
        }

        IEnumerator OnPlayPressedRoutine()
        {
            TransitionFader.PlayTransition(_startTransitionPrefab);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.LoadNextLevel();
            GameMenu.Open();
        }

        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
        }

        public void OnCreditsPressed()
        {
            CreditsScreen.Open();
        }

        public override void OnBackPressed()
        {
            Application.Quit();
        }

        public void OnPlayerNameInputChanged(string name)
        {
            _dataManager.PlayerName = name;
        }
        
        public void OnPlayerNameInputEnd()
        {
            if (_dataManager != null)
            {
                _dataManager.Save();
            }
        }
        
        public void LoadData()
        {
            if (_dataManager == null || _playerNameInputField == null) return;
            _dataManager.Load();
            
            _playerNameInputField.text = _dataManager.PlayerName;
        }
    }
}
