using System.Collections.Generic;
using System.Linq;
using TouhouJam.Boss;
using UnityEngine;

namespace TouhouJam.Player.Prop
{
	public class HatateCamera : MonoBehaviour
	{
		public Collider2D area;

		public Transform storage;

		public SpriteRenderer flash;

		int activeFor;

		List<Vector3> velocities = new();

		public void Trigger() {
			int i = 0;
			//Debug.Log($"{storage.childCount} {velocities.Count}");
			foreach (var captured in storage.Cast<Transform>().ToList()) {
				captured.parent = null;
				captured.GetComponent<Rigidbody2D>().velocity = velocities[i++];
				Physics2D.IgnoreCollision(captured.GetComponent<Collider2D>(), area);
			}

			activeFor = 2;
			velocities.Clear();
		}

		void FixedUpdate() {
			flash.color = new(1, 1, 1, activeFor / 2f);

			if (activeFor != 0)
				activeFor--;
		}

		void OnTriggerStay2D(Collider2D col) {
			if (activeFor != 0 && col.GetComponent<Bullet>())
			{
				velocities.Add(col.GetComponent<Rigidbody2D>().velocity);
				col.transform.parent = storage;
			}
		}
	}
}