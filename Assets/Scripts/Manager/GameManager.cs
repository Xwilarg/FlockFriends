using TouhouJam.VN;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static int NextLevel => 1;

        [SerializeField]
        private TextAsset _intro;

        public bool CanMove => !VNManager.Instance.IsPlayingStory;

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("VN", LoadSceneMode.Additive);
            SceneManager.LoadScene($"{NextLevel:00}", LoadSceneMode.Additive);
        }

        private void Start()
        {
            if (NextLevel == 1)
            {
                VNManager.Instance.ShowStory(_intro, null);
            }
        }
    }
}
