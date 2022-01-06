using System;
using System.Collections;
using System.Collections.Generic;
using LevelManagement.Missions;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManagement
{
    [RequireComponent(typeof(MissionSelector))]
    public class LevelSelectMenu : Menu<LevelSelectMenu>
    {
        [SerializeField] protected Text _nameText;
        [SerializeField] protected Text _descriptionText;
        [SerializeField] protected Image _previewImage;
        
        [SerializeField] protected float _playDelay = 0.5f;
        [SerializeField] protected TransitionFader _startTransitionPrefab;

        protected MissionSelector _missionSelector;
        protected MissionSpecs _currentMission;

        protected override void Awake()
        {
            base.Awake();
            _missionSelector = GetComponent<MissionSelector>();
            UpdateInfo();
        }

        void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            _currentMission = _missionSelector.GetCurrentMission();

            if (_currentMission == null) return;
            
            _nameText.text = _currentMission.Name;
            _descriptionText.text = _currentMission.Description;
            _previewImage.sprite = _currentMission.Image;
        }

        public void OnNextPressed()
        {
            _missionSelector.IncrementIndex();
            UpdateInfo();
        }
        
        public void OnPreviousPressed()
        {
            _missionSelector.DecrementIndex();
            UpdateInfo();
        }

        public void OnPlayPressed()
        {
            StartCoroutine(PlayMissionRoutine(_currentMission.SceneName));
        }

        IEnumerator PlayMissionRoutine(string sceneName)
        {
            TransitionFader.PlayTransition(_startTransitionPrefab);
            LevelLoader.LoadLevel(sceneName);
            yield return new WaitForSeconds(_playDelay);
            GameMenu.Open();
        }
    }
}
