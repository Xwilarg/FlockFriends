using TouhouJam.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { private set; get; }

        [SerializeField]
        private bool _enabled;

        [SerializeField]
        [Tooltip("-1 to disable")]
        private int _startLevel;

        [SerializeField]
        private bool _skipIntro;
        public bool SkipIntro
        {
            get
            {
#if UNITY_EDITOR
                return _enabled && _skipIntro;
#else
                return false;
#endif
            }
        }

        private static bool _init;

        private void Awake()
        {
            Instance = this;
#if UNITY_EDITOR
            if (_enabled && !_init)
            {
                _init = true; // Bad code
                if (_startLevel != -1)
                {
                    GameManager.NextLevel = _startLevel;
                    SceneManager.LoadScene("Main");
                }
            }
#endif
        }
    }
}
