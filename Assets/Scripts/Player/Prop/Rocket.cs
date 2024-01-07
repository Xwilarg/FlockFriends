using UnityEngine;

namespace TouhouJam.Player.Prop
{
    public class Rocket : MonoBehaviour
    {
        private float _rocketImpactMaxDistance = 3f;
        private float _rocketPropulsionForce = 20f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var contact = collision.contacts[0].point;
            var force = Vector2.Distance(contact, PlayerController.Instance.transform.position);
            if (force < _rocketImpactMaxDistance)
            {
                var propForce = (1f - force / _rocketImpactMaxDistance) * _rocketPropulsionForce;
                PlayerController.Instance.AddPropulsionForce(propForce, (Vector2)PlayerController.Instance.transform.position - contact, contact);
            }
            Destroy(gameObject);
        }
    }
}
