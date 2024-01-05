using TouhouJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TouhouJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _player;

        private Rigidbody2D _rb;
        private float _movX;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new(_movX * _player.Speed, _rb.velocity.y);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                // TODO
            }
        }
    }
}