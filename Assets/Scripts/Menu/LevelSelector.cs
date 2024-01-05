using TouhouJam.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public static LevelSelector Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public void LoadLevel(int level)
        {
            GameManager.NextLevel = level;
            SceneManager.LoadScene("Main");
        }
    }
}
