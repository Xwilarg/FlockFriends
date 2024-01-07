using System;
using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Level.Enemies
{
	public class Shooting : MonoBehaviour
	{
		public float timeBetweenShots;

		public float bulletSpeed;

		public float rotateSpeed;

		public GameObject pfBullet;

		float nextShot;

		Vector2 velocity;

		void FixedUpdate() {
			if (Time.time > nextShot) {
				nextShot = Time.time + timeBetweenShots;
				Instantiate(pfBullet, transform.position + Vector3.forward, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = velocity;
			}
		}

		void Awake() {
			velocity = transform.right * bulletSpeed;
			Destroy(transform.GetChild(0).gameObject);
		}

		void Update() {
			transform.localEulerAngles += Vector3.forward * (Time.deltaTime * rotateSpeed);
		}
	}
}