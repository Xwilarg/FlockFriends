using UnityEngine;
using TouhouJam.Player.Prop;

namespace TouhouJam.Player
{
    public class Hatate : Bird
    {
		new public HatateCamera camera;

		Vector3 camPos;

        public override void UseAbility() => camera.Trigger();

		protected override void Awake() {
			base.Awake();
			camPos = camera.transform.localPosition;
		}

		void FixedUpdate() {
			var targetCamPos = camPos;
			if (renderer.flipX)
				targetCamPos.x *= -1;
			camera.transform.localPosition = Vector3.LerpUnclamped(camera.transform.localPosition, targetCamPos, 0.5f);
		}
	}
}