using System.Collections.Generic;
using UnityEngine;
using TouhouJam.Player;

namespace TouhouJam.Level
{
	public class LevelData : MonoBehaviour
	{
		public List<EBird> availableBirds;

		public static LevelData current;

		void Awake() => current = this;
	}
}