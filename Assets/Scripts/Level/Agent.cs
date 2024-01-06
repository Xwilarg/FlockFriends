
using UnityEngine;

namespace TouhouJam.Level
{
	public class Agent : MonoBehaviour {
		
		protected bool blind;

		protected Rigidbody2D rb;

		public virtual void Blind() => blind = true;

		protected Vector2 spawnPosition;

		protected virtual void Awake() {
			rb = GetComponent<Rigidbody2D>();
			spawnPosition = rb.position;
		}

		public virtual void OnReset() {
			rb.velocity = Vector2.zero;
			rb.MovePosition(spawnPosition);
		}
	}
}