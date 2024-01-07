using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Level.Enemies
{
	public class Leaping : Agent {

		const float LEAP_FORCE = 1e3f;

		public Sprite idle, leaping, blinded;

		float whenCanLeap;

		new SpriteRenderer renderer;

		protected override void Awake() {
			base.Awake();
			renderer = GetComponent<SpriteRenderer>();
		}

		public override void Blind() {
			base.Blind();
			renderer.sprite = blinded;
		}

		public override void OnReset() {
			base.OnReset();
			renderer.sprite = idle;
		}

		void FixedUpdate() {
			if (!blind && Time.time > whenCanLeap) {
				if (Mathf.Abs(rb.velocity.y) > 0.01f) {
					whenCanLeap = Time.time + 0.2f;
				} else {
					var bb = GetComponent<BoxCollider2D>().bounds;
					if (CanSeeFrom(bb.min) && CanSeeFrom(bb.max) && CanSeeFrom(new(bb.min.x, bb.max.y)) && CanSeeFrom(new(bb.max.x, bb.min.y))) {
						var direction = PlayerController.Instance.Position - rb.position;
						direction.Normalize();
						direction.y = Mathf.Max(direction.y, 0.2f);
						rb.AddForce(direction.normalized * LEAP_FORCE);
						whenCanLeap = Time.time + 2f;
						renderer.sprite = leaping;
						renderer.flipX = direction.x < 0;
					} else {
						renderer.sprite = idle;
					}
				}
			}
		}

		bool CanSeeFrom(Vector2 source) {
			source = Vector2.LerpUnclamped(source, rb.position, 0.1f);
			var direction = PlayerController.Instance.Position - source;
			var hit = Physics2D.Raycast(source, direction, 12, 3 << 6);
			return hit.collider && hit.collider.GetComponent<PlayerController>();
		}
	}
}