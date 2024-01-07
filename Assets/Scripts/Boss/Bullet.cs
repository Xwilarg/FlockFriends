using TouhouJam.Level;
using TouhouJam.Level.Enemies;
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
                collision.collider.GetComponent<PlayerController>().Lose();

            Agent agent = collision.collider.GetComponent<Leaping>();
            if (agent)
                agent.Kill();

            Destroy(gameObject);
        }

        void FixedUpdate() {
            lifetime -= Time.fixedDeltaTime;
            if (lifetime < 0)
                Destroy(gameObject);
        }
    }
}
