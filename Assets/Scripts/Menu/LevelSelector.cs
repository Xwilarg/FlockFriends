using TouhouJam.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public void LoadLevel(int level)
        {
            GameManager.NextLevel = level;
            SceneManager.LoadScene("Main");
        }
    }
}
