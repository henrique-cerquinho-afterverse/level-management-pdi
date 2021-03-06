using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelManagement
{
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] ScreenFader _screenFader;
        [SerializeField] float _delay = 1;

        void Awake()
        {
            _screenFader = GetComponent<ScreenFader>();
        }

        void Start()
        {
            _screenFader.FadeOn();
        }

        /// <summary>
        /// Fades the screen in and loads the Main Menu scene. Runs after clicking the splash screen.
        /// </summary>
        public void FadeAndLoad()
        {
            StartCoroutine(FadeAndLoadRoutine());
        }

        IEnumerator FadeAndLoadRoutine()
        {
            yield return new WaitForSeconds(_delay);
            _screenFader.FadeOff();
            
            // wait for fade
            yield return new WaitForSeconds(_screenFader.FadeOnDuration);
            LevelLoader.LoadMainMenuLevel();
            
            // remove splash screen object
            Destroy(gameObject);
        }
    }
}
