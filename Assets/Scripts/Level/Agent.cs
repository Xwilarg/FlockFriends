
using UnityEngine;

namespace TouhouJam.Level
{
	public class Agent : MonoBehaviour {
		
		protected bool blind;

		protected Rigidbody2D rb;

		public virtual void Blind() => blind = true;

		protected Vector2 spawnPosition;

		Collider2D colliderToReenable;

		protected virtual void Awake() {
			rb = GetComponent<Rigidbody2D>();
			spawnPosition = rb.position;
		}

		public virtual void OnReset() {
			rb.velocity = Vector2.zero;
			rb.MovePosition(spawnPosition);
			blind = false;
			if (colliderToReenable) {
				colliderToReenable.enabled = true;
				colliderToReenable = null;
			}
		}

		public virtual void Kill() {
			colliderToReenable = GetComponent<Collider2D>();
			colliderToReenable.enabled = false;
			rb.MovePosition(new(0, -1e6f));
		}
	}
}