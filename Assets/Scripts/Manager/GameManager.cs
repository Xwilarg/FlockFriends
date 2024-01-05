using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene("VN", LoadSceneMode.Additive);
        }
    }
}
