using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TouhouJam.Manager;
using TouhouJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;
using TouhouJam.Level;

namespace TouhouJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        // Movements
        private Rigidbody2D _rb;
        public Vector2 InputDirection { get; private set; }

        // Jump info
        private const float _jumpMaxDistance = 1.01f;
        private const float _raycastXOffset = .45f;
        private readonly float[] _jumpOffsets = new[] { -_raycastXOffset, _raycastXOffset };
        private int _levelMask;

        private readonly Dictionary<PlayerAction, bool> _canDoAction = new();

        private Vector2 _initialPos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _levelMask = 1 << LayerMask.NameToLayer("Level");
            
            foreach (PlayerAction action in (PlayerAction[])Enum.GetValues(typeof(PlayerAction)))
            {
                _canDoAction.Add(action, true);
            }
        }

        private void Start()
        {
            SwitchToBird(LevelData.current.availableBirds[0]);
            transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
            _initialPos = transform.position;
        }

        private void FixedUpdate()
        {
            var movX = GameManager.Instance.CanMove ? InputDirection.x : 0f;
            _rb.velocity = new(movX * _info.Speed, _rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Trap"))
            {
                Loose();
            }
            else if (collision.collider.CompareTag("FinishLine"))
            {
                _rb.velocity = Vector2.zero;
                GameManager.Instance.Win();
            }
        }

        public void Loose()
        {
            if (!GameManager.Instance.DidWon)
            {
                _rb.velocity = Vector2.zero;
                transform.position = _initialPos;
            }
        }

        private IEnumerator ReloadAction(PlayerAction action, float duration)
        {
            _canDoAction[action] = false;
            yield return new WaitForSeconds(duration);
            _canDoAction[action] = true;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            InputDirection = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && IsOnGround && GameManager.Instance.CanMove)
            {
                _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                StartCoroutine(ReloadAction(PlayerAction.Jump, _info.JumpReloadTime));
            }
        }

        private enum PlayerAction
        {
            Jump, ChangeBird
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


        Bird currentBird;

        void SwitchToBird(EBird target) {
            Bird targetBird = transform.GetChild((int)target).GetComponent<Bird>();
            targetBird.gameObject.SetActive(true);

            if (currentBird) {
                currentBird.AnimateTransitionFrom();
                targetBird.AnimateTransitionTo();
            }

            currentBird = targetBird;
        }

        public void OnGoNextBird(InputAction.CallbackContext _) {
            if (LockBirdChangeActionIfAvailable()) {
                int index = LevelData.current.availableBirds.IndexOf(currentBird.BirdEnum);
                index = (index + 1) % LevelData.current.availableBirds.Count;
                SwitchToBird(LevelData.current.availableBirds[index]);
            }
        }

        public void OnGoPrevBird(InputAction.CallbackContext _) {
            if (LockBirdChangeActionIfAvailable()) {
                int index = LevelData.current.availableBirds.IndexOf(currentBird.BirdEnum);
                if (index == 0)
                    index = LevelData.current.availableBirds.Count;
                index--;
                SwitchToBird(LevelData.current.availableBirds[index]);
            }
        }

        bool LockBirdChangeActionIfAvailable() {
            if (GameManager.Instance.CanMove && _canDoAction[PlayerAction.ChangeBird]) {
                StartCoroutine(ReloadAction(PlayerAction.ChangeBird, Bird.SWITCH_BIRD_ANIMATION_TIME));
                return true;
            }
            return false;
        }

        public void OnUseBirdPower(InputAction.CallbackContext _) {
            if (GameManager.Instance.CanMove)
                currentBird.UseAbility();
        }
    }
}