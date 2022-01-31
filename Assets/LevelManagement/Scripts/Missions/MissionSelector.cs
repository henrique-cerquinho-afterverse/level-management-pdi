using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Missions
{
    public class MissionSelector : MonoBehaviour
    {
        [SerializeField] protected MissionList _missionList;

        public int CurrentIndex => _currentIndex;
        
        protected int _currentIndex;

        public void ClampIndex()
        {
            if (_missionList.TotalMissions == 0)
            {
                Debug.LogWarning("MISSION SELECTOR ClampIndex: missing mission setup!");
            }

            if (_currentIndex >= _missionList.TotalMissions)
            {
                _currentIndex = 0;
            }

            if (_currentIndex < 0)
            {
                _currentIndex = _missionList.TotalMissions - 1;
            }
        }

        public void SetIndex(int index)
        {
            _currentIndex = index;
            ClampIndex();
        }

        public void IncrementIndex()
        {
            SetIndex(_currentIndex + 1);
        }
        
        public void DecrementIndex()
        {
            SetIndex(_currentIndex - 1);
        }

        public MissionSpecs GetMission(int index)
        {
            return _missionList.GetMission(index);
        }

        public MissionSpecs GetCurrentMission()
        {
            return GetMission(_currentIndex);
        }
    }
}
