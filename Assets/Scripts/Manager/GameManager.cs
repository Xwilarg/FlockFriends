﻿using TouhouJam.Persistency;
using TouhouJam.VN;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static int NextLevel { set; get; } = 1;

        [SerializeField]
        private TextAsset _intro;

        [SerializeField]
        private GameObject _victoryPanel;

        [SerializeField]
        private LevelInfo[] _levels;
        public LevelInfo[] Levels => _levels;

        public bool CanMove => !VNManager.Instance.IsPlayingStory && !DidWon;

        public bool DidWon => _victoryPanel.activeInHierarchy;

        public void Win()
        {
            _victoryPanel.SetActive(true);
            NextLevel++;

            if (PersistencyManager.Instance.SaveData.NextUnlockedLevel < NextLevel + 1)
            {
                PersistencyManager.Instance.SaveData.NextUnlockedLevel = NextLevel + 1;
            }
        }

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("VN", LoadSceneMode.Additive);
            SceneManager.LoadScene($"{NextLevel:00}", LoadSceneMode.Additive);
        }

        private void Start()
        {
            if (Levels[NextLevel - 1].IntroStory != null)
            {
                VNManager.Instance.ShowStory(Levels[NextLevel - 1].IntroStory, null);
            }
        }

        public void Replay()
        {
            SceneManager.LoadScene("Main");
        }

        public void PlayNextLevel()
        {
            SceneManager.LoadScene("Main");
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("LevelSelector");
        }
    }
}
