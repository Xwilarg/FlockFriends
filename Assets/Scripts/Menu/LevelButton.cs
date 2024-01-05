using TouhouJam.Persistency;
using UnityEngine;
using UnityEngine.UI;

namespace TouhouJam.Menu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private int _levelNumber;

        private void Awake()
        {
            if (_levelNumber > PersistencyManager.Instance.SaveData.NextUnlockedLevel)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(new(() => { LevelSelector.Instance.LoadLevel(_levelNumber); }));
        }
    }
}
