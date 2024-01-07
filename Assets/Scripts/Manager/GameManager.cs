using Assets.Scripts;
using TouhouJam.Persistency;
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
        private GameObject _victoryPanel;

        [SerializeField]
        private LevelInfo[] _levels;
        public LevelInfo[] Levels => _levels;

        [SerializeField]
        private AudioSource _bgm;

        [SerializeField]
        private AudioClip _bossBgm;

        public bool CanMove => !VNManager.Instance.IsPlayingStory && !DidWon;

        public bool DidWon => _victoryPanel.activeInHierarchy;

        public void Win()
        {
            void onEnd()
            {
                if (NextLevel == 6)
                {
                    SceneManager.LoadScene("MainMenu");
                    return;
                }

                _victoryPanel.SetActive(true);
                NextLevel++;

                if (PersistencyManager.Instance.SaveData.NextUnlockedLevel < NextLevel + 1)
                {
                    PersistencyManager.Instance.SaveData.NextUnlockedLevel = NextLevel + 1;
                    PersistencyManager.Instance.Save();
                }
            }

            if (Levels[NextLevel - 1].OutroStory != null)
            {
                VNManager.Instance.ShowStory(Levels[NextLevel - 1].OutroStory, onEnd);
            }
            else
            {
                onEnd();
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
            if (NextLevel == 6)
            {
                _bgm.clip = _bossBgm;
                _bgm.Play();
            }
            if (Levels[NextLevel - 1].IntroStory != null && !DebugManager.Instance.SkipIntro)
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
