using UnityEngine;

namespace TouhouJam.Player
{
    public class Kutaka : Bird
    {
        [SerializeField]
        private GameObject _vfx;

        public override void UseAbility()
        {
            Vector2 direction = player.LastKnownDirection;

            var hit = Physics2D.Raycast(transform.position, Vector2.right * direction.x, 1f, 1 << 7);

            Vector2 hitPoint;

            if (hit.collider != null)
            {
                hitPoint = hit.point;
                if (hit.collider.CompareTag("Rock"))
                {
                    hit.collider.GetComponent<Rigidbody2D>().velocity = (Vector2.right * direction.x).normalized * 10f;
                }
            }
            else
            {
                hitPoint = transform.position + (Vector3.right * direction.x).normalized;
            }

            Destroy(Instantiate(_vfx, hitPoint, Quaternion.identity), 1f);
        }
    }
}