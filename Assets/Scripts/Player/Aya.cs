using UnityEngine;

namespace TouhouJam.Player
{
	public class Aya : Bird
	{
		const float RAY_STEP = 0.25f;

		public override void UseAbility() {
			Vector2 direction = player.InputDirection;

			float absX = Mathf.Abs(direction.x), absY = Mathf.Abs(direction.y);
			bool horizontal;

			if (absX == absY) {
				return;
			} else if (absX > absY) {
				direction.y = 0;
				horizontal = true;
			} else {
				direction.x = 0;
				horizontal = false;
			}
			
			direction.Normalize();

			var col = player.GetComponent<Collider2D>();
			Bounds bb = col.bounds;

			float minPos = horizontal ? bb.min.y : bb.min.x;
			float maxPos = horizontal ? bb.max.y : bb.max.x;
			float minDistance = 4.5f;

			for (float p = minPos + 0.1f; p < maxPos; p += RAY_STEP)
				Raycast(ref minDistance, horizontal, direction, p, bb);
			Raycast(ref minDistance, horizontal, direction, maxPos - 0.1f, bb);

			//if (float.IsFinite(minDistance)) {
				minDistance -= horizontal ? bb.extents.x : bb.extents.y;
				var rb = player.GetComponent<Rigidbody2D>();
				
				//col.enabled = false;
				rb.MovePosition(rb.position + direction * minDistance);
				//col.enabled = true;
				var vel = rb.velocity;
				if (horizontal)
					vel.y = 0;
				else
					vel.x = 0;
				rb.velocity = vel;
			//}
		}

		private void Raycast(ref float minDistance, bool horizontal, Vector2 direction, float p, Bounds bb) {
			Vector2 origin = horizontal ? new(bb.center.x, p) : new(p, bb.center.y);
			var hit = Physics2D.Raycast(origin, direction, float.PositiveInfinity, 1 << 7);
			if (hit.collider)
				minDistance = Mathf.Min(minDistance, hit.distance);
		}
	}
}