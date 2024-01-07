
using UnityEngine;

namespace TouhouJam.Level
{
	public class Agent : MonoBehaviour {
		
		protected bool blind;

		protected Rigidbody2D rb;

		public virtual void Blind() => blind = true;

		protected Vector3 spawnPosition;

		Collider2D colliderToReenable;

		protected virtual void Awake() {
			rb = GetComponent<Rigidbody2D>();
			spawnPosition = transform.position;
		}

		public virtual void OnReset() {
			rb.velocity = Vector2.zero;
			transform.position = spawnPosition;
			blind = false;
			if (colliderToReenable) {
				Invoke("Reenable", Time.fixedDeltaTime);
			}
		}

		void Reenable() {
			colliderToReenable.enabled = true;
			colliderToReenable = null;
		}

		public virtual void Kill() {
			colliderToReenable = GetComponent<Collider2D>();
			colliderToReenable.enabled = false;
			rb.MovePosition(new(0, -1e3f));
		}
	}
}