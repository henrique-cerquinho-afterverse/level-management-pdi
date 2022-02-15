using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace LevelManagement
{
    /// <summary>
    /// Implements a manager for all the menu types. We serialize all the different menu prefabs here and pick them up
    /// dynamically on the initialize method. This class uses a stack to keep track of the opened menus and their order.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] MainMenu _mainMenuPrefab;
        [SerializeField] SettingsMenu _settingsMenuPrefab;
        [SerializeField] CreditsScreen _creditsMenuPrefab;
        [SerializeField] GameMenu _gameMenuPrefab;
        [SerializeField] PauseMenu _pauseMenuPrefab;
        [SerializeField] WinScreen _winScreenPrefab;
        [SerializeField] LevelSelectMenu _levelSelectMenuPrefab;
        
        [SerializeField] Transform _menuParent;

        // instance to be used as a singleton
        public static MenuManager Instance => _instance;
        
        Stack<Menu> _menuStack = new Stack<Menu>();

        static MenuManager _instance;

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
                    if (prefab != _mainMenuPrefab)
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

        /// <summary>
        /// Called by the different kinds of menus (ergo the Menu<T> class) to open them. Pushes the opened menu to
        /// the stack.
        /// </summary>
        /// <param name="menuInstance">Menu instance to be opened</param>
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

        /// <summary>
        /// Closes the current menu and opens the previous menu in the stack.
        /// </summary>
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
