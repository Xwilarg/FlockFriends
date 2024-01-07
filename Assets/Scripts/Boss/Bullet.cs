using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Boss
{
    public class Bullet : MonoBehaviour
    {
        public float lifetime;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.GetComponent<PlayerController>().Lose();
            }
            Destroy(gameObject);
        }

        void FixedUpdate() {
            lifetime -= Time.fixedDeltaTime;
            if (lifetime < 0)
                Destroy(gameObject);
        }
    }
}
