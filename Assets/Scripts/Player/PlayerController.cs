using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TouhouJam.SO;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TouhouJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        // Movements
        private Rigidbody2D _rb;
        private float _movX;

        // Jump info
        private const float _jumpMaxDistance = 1.01f;
        private const float _raycastXOffset = .45f;
        private readonly float[] _jumpOffsets = new[] { -_raycastXOffset, _raycastXOffset };
        private int _levelMask;

        private readonly Dictionary<PlayerAction, bool> _canDoAction = new();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _levelMask = 1 << LayerMask.NameToLayer("Level");

            foreach (PlayerAction action in (PlayerAction[])Enum.GetValues(typeof(PlayerAction)))
            {
                _canDoAction.Add(action, true);
            }
        }

        private void FixedUpdate()
        {
            _rb.velocity = new(_movX * _info.Speed, _rb.velocity.y);
        }

        private IEnumerator ReloadAction(PlayerAction action, float duration)
        {
            _canDoAction[action] = false;
            yield return new WaitForSeconds(duration);
            _canDoAction[action] = true;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && IsOnGround)
            {
                _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                StartCoroutine(ReloadAction(PlayerAction.Jump, _info.JumpReloadTime));
            }
        }

        private enum PlayerAction
        {
            Jump
        }

        private bool IsOnGround
            => _jumpOffsets.Any(o => Physics2D.Raycast(transform.position + (Vector3.right * o), Vector2.down, _jumpMaxDistance, _levelMask).collider != null);

        private void OnDrawGizmos()
        {
            foreach (var o in _jumpOffsets)
            {
                var centerPoint = transform.position + (Vector3.right * o);
                var hit = Physics2D.Raycast(centerPoint, Vector2.down, _jumpMaxDistance, _levelMask);
                if (hit.collider != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(centerPoint, hit.point);
                }
                else
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(centerPoint, centerPoint + (Vector3.down * _jumpMaxDistance));
                }
            }
        }
    }
}