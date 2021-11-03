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

        public void FadeAndLoad()
        {
            StartCoroutine(FadeAndLoadRoutine());
        }

        IEnumerator FadeAndLoadRoutine()
        {
            yield return new WaitForSeconds(_delay);
            _screenFader.FadeOff();
            LevelLoader.LoadMainMenuLevel();
            
            // wait for fade
            yield return new WaitForSeconds(_screenFader.FadeOnDuration);
            
            // remove splash screen object
            Destroy(gameObject);
        }
    }
}
