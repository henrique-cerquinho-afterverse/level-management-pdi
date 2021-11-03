using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SampleGame;

namespace LevelManagement
{
    public abstract class Menu<T>:Menu where T: Menu<T>
    {
        private static T _instance;

        // instance to be used as a singleton
        public static T Instance { get { return _instance; } }

        // derived classes can see protected methods, but other objects can't
        protected virtual void Awake()
        {
            // allow only one instance of the singleton
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        public static void Open()
        {
            if (MenuManager.Instance != null && Instance != null)
            {
                MenuManager.Instance.OpenMenu(Instance);
            }
            else
            {
                Debug.LogError("Menu manager instance not found!!");
            }
        }
    }

    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        public virtual void OnBackPressed()
        {
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.CloseMenu();
            }
        }
    }
}
