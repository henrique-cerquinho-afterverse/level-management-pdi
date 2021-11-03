using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SampleGame;

namespace LevelManagement
{
    public class GameMenu : Menu<GameMenu>
    {
        public void OnPausePressed()
        {
            if (PauseMenu.Instance == null) return;
            Time.timeScale = 0;

            PauseMenu.Open();
        }
    }
}
