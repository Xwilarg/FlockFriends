using UnityEngine;
using UnityEngine.UI;

namespace TouhouJam.Player
{
	public abstract class Bird : MonoBehaviour
	{
		public Sprite idle;

		public const float SWITCH_BIRD_ANIMATION_TIME = 0.5f;

		public EBird BirdEnum { get; private set; }

		protected PlayerController player;
		new protected SpriteRenderer renderer;

		public void AnimateTransitionFrom() {
			gameObject.SetActive(false);
		}

		public void AnimateTransitionTo() {
			
		}

		public abstract void UseAbility();

		protected virtual void Awake() {
			BirdEnum = (EBird)transform.GetSiblingIndex();
			player = GetComponentInParent<PlayerController>();
			renderer = GetComponentInChildren<SpriteRenderer>();
			renderer.sprite = idle;
		}
	}
}