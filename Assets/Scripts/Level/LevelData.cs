using System.Collections.Generic;
using UnityEngine;
using TouhouJam.Player;
using System.Linq;
using TouhouJam.Manager;

namespace TouhouJam.Level
{
	public class LevelData : MonoBehaviour
	{
		[SerializeField]
		private EBird[] _newBirds;

		public List<EBird> AvailableBirds { private set; get; }

		public static LevelData current;

		void Awake()
		{
            current = this;
			AvailableBirds = _newBirds.Take(GameManager.NextLevel).Where(x => x != EBird.None).ToList();

			Debug.Log($"[LEV] Loading birds {string.Join(", ", AvailableBirds)}");
        }
	}
}