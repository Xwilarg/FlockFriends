using System;
using UnityEngine;
using UnityEngine.UI;

namespace TouhouJam.Player
{
	public abstract class Bird : MonoBehaviour
	{
		public float cooldownTime;

		public Sprite idle;

		public const float SWITCH_BIRD_ANIMATION_TIME = 0.25f;

		public EBird BirdEnum { get; private set; }

		[NonSerialized]
		protected PlayerController player;

		[NonSerialized]
		new public SpriteRenderer renderer;
		
		public Animator Anim { private set; get; }

		public void AnimateTransitionFrom() =>
			gameObject.AddComponent<TransitionFromAnimation>().renderer = renderer.transform;
		

		public void AnimateTransitionTo() =>
			gameObject.AddComponent<TransitionToAnimation>().renderer = renderer.transform;

		public abstract void UseAbility();

		protected virtual void Awake() {
			BirdEnum = (EBird)transform.GetSiblingIndex();
			player = GetComponentInParent<PlayerController>();
			renderer = GetComponentInChildren<SpriteRenderer>();
			Anim = GetComponentInChildren<Animator>();
			renderer.sprite = idle;
		}

		private class TransitionFromAnimation : MonoBehaviour {
			
			new public Transform renderer;

			float start, baseXScale;

			public void Start() {
				start = Time.time;
				baseXScale = renderer.localScale.x;
			}

			public void Update() {
				float t = Time.time - start;
				var scale = renderer.localScale;
				if (t > SWITCH_BIRD_ANIMATION_TIME / 2) {
					scale.x = baseXScale;
					Destroy(this);
					gameObject.SetActive(false);
				} else {
					scale.x = baseXScale * Mathf.Cos(Mathf.PI * t / SWITCH_BIRD_ANIMATION_TIME);
				}
				renderer.localScale = scale;
			}
		}

		private class TransitionToAnimation : MonoBehaviour {
			
			new public Transform renderer;

			float start, baseXScale;

			public void Start() {
				start = Time.time + SWITCH_BIRD_ANIMATION_TIME / 2;
				baseXScale = renderer.localScale.x;
			}

			public void Update() {
				float t = Time.time - start;
				var scale = renderer.localScale;
				if (t > SWITCH_BIRD_ANIMATION_TIME / 2) {
					scale.x = baseXScale;
					Destroy(this);
				} else if (t < 0) {
					scale.x = 0;
				} else {
					scale.x = baseXScale * Mathf.Sin(Mathf.PI * t / SWITCH_BIRD_ANIMATION_TIME);
				}
				renderer.localScale = scale;
			}
		}
	}
}