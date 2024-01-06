using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Boss
{
    public class Bullet : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.GetComponent<PlayerController>().Lose();
            }
            Destroy(gameObject);
        }
    }
}
