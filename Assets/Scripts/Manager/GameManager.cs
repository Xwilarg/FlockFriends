using TouhouJam.VN;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField]
        private TextAsset _intro;

        public bool CanMove => !VNManager.Instance.IsPlayingStory;

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("VN", LoadSceneMode.Additive);
        }

        private void Start()
        {
            VNManager.Instance.ShowStory(_intro, null);
        }
    }
}
