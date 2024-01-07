using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TouhouJam.Manager;
using TouhouJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;
using TouhouJam.Level;
using UnityEngine.Tilemaps;
using TouhouJam.Boss;
using TouhouJam.VN;

namespace TouhouJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField]
        private PlayerInfo _info;

        // Movements
        private Rigidbody2D _rb;
        private Animator _anim;
        private SpriteRenderer _sr;
        public Vector2 InputDirection { get; private set; }
        public Vector2 LastKnownDirection { get; private set; } = Vector2.right;
        public Vector2 Position { get => _rb.position; }

        // Jump info
        private const float _jumpMaxDistance = 1.25f;
        private const float _raycastXOffset = .45f;
        private readonly float[] _jumpOffsets = new[] { -_raycastXOffset, _raycastXOffset };
        private int _levelMask;

        private readonly Dictionary<PlayerAction, bool> _canDoAction = new();

        private Vector2 _initialPos;

        private float _fallBoundaryY;

        private void Awake()
        {
            Instance = this;

            _rb = GetComponent<Rigidbody2D>();
            _levelMask = 1 << LayerMask.NameToLayer("Level");

            foreach (PlayerAction action in (PlayerAction[])Enum.GetValues(typeof(PlayerAction)))
            {
                _canDoAction.Add(action, true);
            }

            foreach (var collider in FindObjectsOfType<TilemapCollider2D>())
            {
                _fallBoundaryY = Mathf.Min(_fallBoundaryY, collider.bounds.min.y);
            }

            _fallBoundaryY -= 25;
        }

        private void Start()
        {
            SwitchToBird(EBird.Mystia);
            transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
            _initialPos = transform.position;

            Destroy(GetComponent<SpriteRenderer>());
        }

        // For rocket jump (Okuu)
        public void AddPropulsionForce(float force, Vector2 direction, Vector2 contactPoint)
        {
            if (contactPoint.y < transform.position.y)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Abs(_rb.velocity.y)) / 1.5f;
            }
            _rb.AddForce(direction * force, ForceMode2D.Impulse);
        }

        private float FloatDirection(float x)
        {
            if (x == 0f) return 0f;
            if (x < 0f) return -1f;
            return 1f;
        }

        private void FixedUpdate()
        {
            // Ignore Y > 0
            var actualDir = new Vector2(FloatDirection(InputDirection.x), InputDirection.y < 0f ? -1f : 0f).normalized;

            var onFloor = IsOnGround;

            var movX = GameManager.Instance.CanMove ? actualDir.x : 0f;

            _rb.velocity = new(movX * _info.Speed, _rb.velocity.y + actualDir.y * 10f);

            var isMoving = movX != 0f;
            if (isMoving)
            {
                _sr.flipX = movX < 0f;
            }

            if (Position.y < _fallBoundaryY)
                Lose();

            if (_anim) {
                _anim.SetFloat("X", onFloor ? Mathf.Abs(movX) : 0f);
                _anim.SetFloat("Y", onFloor ? 0f : Mathf.Clamp01(_rb.velocity.y));
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Trap"))
            {
                Lose();
            }
            else if (collision.collider.CompareTag("FinishLine"))
            {
                _rb.velocity = Vector2.zero;
                GameManager.Instance.Win();
            }
        }

        public void Lose()
        {
            if (!GameManager.Instance.DidWon)
            {
                _rb.velocity = Vector2.zero;
                transform.position = _initialPos;

                foreach (var agent in FindObjectsOfType<Agent>())
                    agent.OnReset();

                foreach (var bullet in FindObjectsOfType<Bullet>())
                    Destroy(bullet.gameObject);
                
                foreach (Transform stored in transform.Find("Hatate/Camera/Storage"))
                    Destroy(stored.gameObject);
            }
        }

        private IEnumerator SeqReloadAction(PlayerAction action, float duration)
        {
            _canDoAction[action] = false;
            yield return new WaitForSeconds(duration);
            _canDoAction[action] = true;
        }

        private void ReloadAction(PlayerAction action, float duration) =>
            StartCoroutine(SeqReloadAction(action, duration)); 

        public void OnMove(InputAction.CallbackContext value)
        {
            InputDirection = value.ReadValue<Vector2>();
            if (InputDirection.magnitude != 0f)
            {
                LastKnownDirection = InputDirection;
            }
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && IsOnGround && GameManager.Instance.CanMove)
            {
                _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                ReloadAction(PlayerAction.Jump, _info.JumpReloadTime);
            }
        }

        private enum PlayerAction
        {
            Jump, ChangeBird, UseAbility
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
            _sr = currentBird.renderer;
            _anim = currentBird.Anim;
        }

        public void OnGoNextBird(InputAction.CallbackContext ctx) {
            if (ctx.performed && LockBirdChangeActionIfAvailable()) {
                int index = LevelData.current.AvailableBirds.IndexOf(currentBird.BirdEnum);
                index = (index + 1) % LevelData.current.AvailableBirds.Count;
                SwitchToBird(LevelData.current.AvailableBirds[index]);
            }
        }

        public void OnGoPrevBird(InputAction.CallbackContext ctx) {
            if (ctx.performed && LockBirdChangeActionIfAvailable()) {
                int index = LevelData.current.AvailableBirds.IndexOf(currentBird.BirdEnum);
                if (index == 0)
                    index = LevelData.current.AvailableBirds.Count;
                index--;
                SwitchToBird(LevelData.current.AvailableBirds[index]);
            }
        }

        bool LockBirdChangeActionIfAvailable() {
            if (GameManager.Instance.CanMove && _canDoAction[PlayerAction.ChangeBird] && LevelData.current.AvailableBirds.Count > 1) {
                ReloadAction(PlayerAction.ChangeBird, Bird.SWITCH_BIRD_ANIMATION_TIME);
                ReloadAction(PlayerAction.UseAbility, Bird.SWITCH_BIRD_ANIMATION_TIME);
                return true;
            }
            return false;
        }

        public void OnUseBirdPower(InputAction.CallbackContext ctx) {
            if (ctx.performed && GameManager.Instance.CanMove && _canDoAction[PlayerAction.UseAbility])
            {
                currentBird.UseAbility();
                ReloadAction(PlayerAction.UseAbility, currentBird.cooldownTime);
            }
        }

        public void OnToggleSkip(InputAction.CallbackContext value)
        {
            VNManager.Instance.OnToggleSkip(value);
        }

        public void OnNextDialogue(InputAction.CallbackContext value)
        {
            VNManager.Instance.OnNextDialogue(value);
        }
    }
}
