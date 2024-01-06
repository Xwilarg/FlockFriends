using System.IO;
using UnityEngine;
using TouhouJam.Level;

namespace TouhouJam.Player
{
	public class Mystia : Bird
	{
		int blindFrames;

		public override void UseAbility() {
			blindFrames = 2;
			GetComponent<ParticleSystem>().Play();
		}

		void FixedUpdate() {
			if (blindFrames != 0)
				blindFrames--;
		}

		void OnTriggerStay2D(Collider2D col) {
			Agent agent;
			if (blindFrames != 0 && gameObject.activeInHierarchy && (agent = col.GetComponent<Agent>()))
				agent.Blind();
		}
	}
}