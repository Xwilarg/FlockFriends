using UnityEngine;
using UnityEngine.InputSystem;

namespace TouhouJam
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _movX;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new(_movX, _rb.velocity.y);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
        }
    }
}