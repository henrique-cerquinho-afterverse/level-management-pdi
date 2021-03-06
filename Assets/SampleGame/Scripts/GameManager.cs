using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;
using LevelManagement;

namespace SampleGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TransitionFader _endTransitionPrefab;
        
        public bool IsGameOver { get { return _isGameOver; } }
        public static GameManager Instance { get { return _instance; } }
        
        ThirdPersonCharacter _player;
        GoalEffect _goalEffect;
        Objective _objective;
        bool _isGameOver;
        
        // instance to be used as a singleton
        static GameManager _instance;
        
        // initialize references
        private void Awake()
        {
            // allow only one instance of the singleton
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                _player = Object.FindObjectOfType<ThirdPersonCharacter>();
                _objective = Object.FindObjectOfType<Objective>();
                _goalEffect = Object.FindObjectOfType<GoalEffect>();
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        // end the level
        public void EndLevel()
        {
            if (_player != null)
            {
                // disable the player controls
                ThirdPersonUserControl thirdPersonControl =
                    _player.GetComponent<ThirdPersonUserControl>();

                if (thirdPersonControl != null)
                {
                    thirdPersonControl.enabled = false;
                }

                // remove any existing motion on the player
                Rigidbody rbody = _player.GetComponent<Rigidbody>();
                if (rbody != null)
                {
                    rbody.velocity = Vector3.zero;
                }

                // force the player to a stand still
                _player.Move(Vector3.zero, false, false);
            }

            // check if we have set IsGameOver to true, only run this logic once
            if (_goalEffect != null && !_isGameOver)
            {
                _isGameOver = true;
                _goalEffect.PlayEffect();
                StartCoroutine(WinRoutine());
            }
        }

        IEnumerator WinRoutine()
        {
            TransitionFader.PlayTransition(_endTransitionPrefab);

            float fadeDelay = _endTransitionPrefab == null
                ? 0f
                : _endTransitionPrefab.Delay + _endTransitionPrefab.FadeOnDuration;
            yield return new WaitForSeconds(fadeDelay);
            WinScreen.Open();
        }

        // check for the end game condition on each frame
        private void Update()
        {
            if (_objective != null && _objective.IsComplete)
            {
                EndLevel();
            }
        }

    }
}