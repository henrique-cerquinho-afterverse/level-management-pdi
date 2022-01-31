using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
    public class LevelLoader : MonoBehaviour
    {
        const int MAIN_MENU_INDEX = 1;
        
        public static void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public static void LoadLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
            {
                if (MainMenu.Instance != null && levelIndex == MAIN_MENU_INDEX)
                {
                    MainMenu.Open();
                }
                SceneManager.LoadScene(levelIndex);
            }
        }

        public static void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = 1;
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            if (currentSceneIndex + 1 <= sceneCount - 1) {
                nextSceneIndex = currentSceneIndex + 1;
            }
            LoadLevel(nextSceneIndex);
        }

        public static void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static void LoadMainMenuLevel()
        {
            LoadLevel(MAIN_MENU_INDEX);
        }
    }
}
