using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace LevelManagement
{
    [RequireComponent(typeof(Canvas))]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] MainMenu mainMenuPrefab;
        [SerializeField] SettingsMenu settingsMenuPrefab;
        [SerializeField] CreditsScreen creditsMenuPrefab;
        [SerializeField] GameMenu gameMenuPrefab;
        [SerializeField] PauseMenu pauseMenuPrefab;
        [SerializeField] WinScreen winScreenPrefab;
        [SerializeField] LevelSelectMenu levelSelectMenuPrefab;
        
        [SerializeField] Transform _menuParent;

        Stack<Menu> _menuStack = new Stack<Menu>();

        static MenuManager _instance;

        // instance to be used as a singleton
        public static MenuManager Instance => _instance;

        void Awake()
        {
            // allow only one instance of the singleton
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                InitializeMenus();
                Object.DontDestroyOnLoad(gameObject);
            }
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        void InitializeMenus()
        {
            if (_menuParent == null)
            {
                GameObject menuParentObject = new GameObject("Menus");
                _menuParent = menuParentObject.transform;
            }
            Object.DontDestroyOnLoad(_menuParent.gameObject);

            // finds fields dynamically from the given type (in this case, MenuManager) and iterates over the fields
            // and their values
            BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fields = this.GetType().GetFields(myFlags);

            foreach (FieldInfo field in fields)
            {
                Menu prefab = field.GetValue(this) as Menu;
                
                if (prefab != null)
                {
                    Menu menuInstance = Instantiate(prefab, _menuParent);
                    if (prefab != mainMenuPrefab)
                    {
                        menuInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        public void OpenMenu(Menu menuInstance)
        {
            if (menuInstance == null)
            {
                Debug.LogWarning("INVALID MENU BRO");
                return;
            }
            
            if (_menuStack.Count > 0)
            {
                foreach (Menu menu in _menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            menuInstance.gameObject.SetActive(true);
            _menuStack.Push(menuInstance);
        }

        public void CloseMenu()
        {
            if (_menuStack.Count == 0)
            {
                Debug.LogWarning("watchu doin???");
                return;
            }

            Menu topMenu = _menuStack.Pop();
            topMenu.gameObject.SetActive(false);
            
            if (_menuStack.Count > 0)
            {
                Menu nextMenu = _menuStack.Peek();
                nextMenu.gameObject.SetActive(true);
            }
        }
    }
}
