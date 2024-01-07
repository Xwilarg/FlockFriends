using UnityEngine;

namespace TouhouJam.Player
{
	public class Aya : Bird
	{
		const float RAY_STEP = 0.25f;

		public override void UseAbility() {
			Vector2 direction = player.LastKnownDirection;

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

			var oldPos = transform.position;
			Vector2 np2;

			//if (float.IsFinite(minDistance)) {
				minDistance -= horizontal ? bb.extents.x : bb.extents.y;
				var rb = player.GetComponent<Rigidbody2D>();
				
				//col.enabled = false;
				rb.MovePosition(np2 = rb.position + direction * minDistance);
				//col.enabled = true;
				var vel = rb.velocity;
				if (horizontal)
					vel.y = 0;
				else
					vel.x = 0;
				rb.velocity = vel;
			//}

			Vector3 newPos = new(np2.x, np2.y, oldPos.z);

			if (Vector3.Distance(oldPos, newPos) > 0.5f) {
				var ps = GetComponent<ParticleSystem>();
				var shape = ps.shape;
				shape.position = (newPos - oldPos) / 2;
				shape.scale = horizontal ? new(Mathf.Abs(oldPos.x - newPos.x), 0.35f, 0) : new(0.35f, Mathf.Abs(oldPos.y - newPos.y), 0);
				ps.Play();
			}
		}

		private void Raycast(ref float minDistance, bool horizontal, Vector2 direction, float p, Bounds bb) {
			Vector2 origin = horizontal ? new(bb.center.x, p) : new(p, bb.center.y);
			var hit = Physics2D.Raycast(origin, direction, float.PositiveInfinity, 1 << 7);
			if (hit.collider)
				minDistance = Mathf.Min(minDistance, hit.distance);
		}
	}
}