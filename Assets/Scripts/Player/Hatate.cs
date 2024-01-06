using UnityEngine;

namespace TouhouJam.Player
{
    public class Hatate : Bird
    {
        const float RAY_STEP = 0.26f;

        public override void UseAbility()
        {
            Vector2 direction = player.LastKnownDirection;

            var hit = Physics2D.Raycast(transform.position, Vector2.right * direction.x, 1f, 1 << 7);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Rock"))
                {
                    hit.collider.GetComponent<Rigidbody2D>().velocity = (Vector2.right * direction.x).normalized * 10f;
                }
            }
        }
    }
}