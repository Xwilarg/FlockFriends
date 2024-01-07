using UnityEngine;
using UnityEngine.InputSystem;

namespace TouhouJam.Player
{
    public class Okii : Bird
    {
        [SerializeField]
        private GameObject _rocket;

        private Camera _cam;

        protected override void Awake()
        {
            base.Awake();
            _cam = Camera.main;
        }

        public override void UseAbility()
        {
            var screenPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var go = Instantiate(_rocket, transform.position, Quaternion.identity);
            Vector3 relPos = screenPos - transform.position;
            float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            go.GetComponent<Rigidbody2D>().AddForce(go.transform.right * 30f, ForceMode2D.Impulse);
        }
    }
}