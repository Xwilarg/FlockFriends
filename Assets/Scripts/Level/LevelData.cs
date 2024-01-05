using System.Collections.Generic;
using UnityEngine;
using TouhouJam.Player;
using System.Linq;
using TouhouJam.Manager;

namespace TouhouJam.Level
{
	public class LevelData : MonoBehaviour
	{
		public List<EBird> AvailableBirds { private set; get; }

		public static LevelData current;

		void Awake()
		{
            current = this;
        }

        private void Start()
        {
            AvailableBirds = GameManager.Instance.Levels.Take(GameManager.NextLevel).Select(x => x.UnlockedBird).Where(x => x != EBird.None).ToList();

            Debug.Log($"[LEV] Loading birds {string.Join(", ", AvailableBirds)}");
        }
    }
}