using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("LevelSelector");
        }
    }
}
