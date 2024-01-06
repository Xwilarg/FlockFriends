using UnityEngine;

namespace TouhouJam.Level.Enemies
{
	public class Leaping : Agent {
		float whenCanLeap;

		void FixedUpdate() {
			var hit = Physics2D.Raycast();
		}
	}
}