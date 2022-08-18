using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LevelManagement.Model;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace LevelManagement
{
    /// <summary>
    /// Implements a manager for all the menu types. We serialize all the different menu prefabs here and pick them up
    /// dynamically on the initialize method. This class uses a stack to keep track of the opened menus and their order.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class MenuManager : MonoBehaviour
    {
        const string INITIAL_MENU_NAME = "MainMenu";
        
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
                var menuParentObject = new GameObject("Menus");
                _menuParent = menuParentObject.transform;
            }
            Object.DontDestroyOnLoad(_menuParent.gameObject);
            
            var assetGuids = AssetDatabase.FindAssets("t:" + nameof(MenuAsset));
            foreach (var guid in assetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var menuAsset = AssetDatabase.LoadAssetAtPath<MenuAsset>(path);
                if (menuAsset != null)
                {
                    var menuInstance = menuAsset.AddressableAsset.Instantiate(_menuParent);
                    if (!menuAsset.name.Contains(INITIAL_MENU_NAME))
                    { 
                        menuInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        OpenMenu(menuInstance.GetComponent<Menu>());
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
                Debug.LogError("INVALID MENU BRO");
                return;
            }
            
            if (_menuStack.Count > 0)
            {
                foreach (Menu menu in _menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            // menuInstance.enabled = true;
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
