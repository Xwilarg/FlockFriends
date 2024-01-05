using TouhouJam.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        private bool _enabled;

        [SerializeField]
        [Tooltip("-1 to disable")]
        private int _startLevel;

        private static bool _init;

        private void Awake()
        {
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
