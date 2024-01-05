using Ink.Runtime;
using System;
using System.Linq;
using TMPro;
using TouhouJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TouhouJam.VN
{
    public class VNManager : MonoBehaviour
    {
        public static VNManager Instance { private set; get; }

        [Header("Characters")]
        [SerializeField]
        private VNCharacterInfo[] _characters;
        private VNCharacterInfo _currentCharacter;

        [Header("Components")]
        [SerializeField]
        private TextDisplay _display;

        private Story _story;

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private Image _bustImage;

        [SerializeField]
        private TMP_Text _nameText;

        private bool _isSkipEnabled;
        private float _skipTimer;
        private float _skipTimerRef = .1f;

        private Action _onDone;

        private void Awake()
        {
            Instance = this;
        }

        public bool IsPlayingStory => _container.activeInHierarchy;

        private void Update()
        {
            if (_isSkipEnabled)
            {
                _skipTimer -= Time.deltaTime;
                if (_skipTimer < 0)
                {
                    _skipTimer = _skipTimerRef;
                    DisplayNextDialogue();
                }
            }
        }

        public void ShowStory(TextAsset asset, Action onDone)
        {
            Debug.Log($"[STORY] Playing {asset.name}");
            _currentCharacter = null;
            _onDone = onDone;
            _story = new(asset.text);
            _isSkipEnabled = false;
            DisplayStory(_story.Continue());
        }

        private void DisplayStory(string text)
        {
            _container.SetActive(true);
            _currentCharacter = null;

            foreach (var tag in _story.currentTags)
            {
                var s = tag.Split(' ');
                var contentList = s.Skip(1).Select(x => x.ToUpperInvariant());
                var content = string.Join(' ', contentList);
                switch (s[0].ToUpperInvariant())
                {
                    case "SPEAKER":
                        _currentCharacter = _characters.FirstOrDefault(x => x.Name.ToUpperInvariant() == content);
                        if (_currentCharacter == null)
                        {
                            Debug.LogError($"[STORY] Unable to find character {_currentCharacter}");
                        }
                        break;

                    default:
                        Debug.LogError($"Unknown story key: {s[0]}");
                        break;
                }
            }
            _display.ToDisplay = text;
            if (_currentCharacter == null)
            {
                _nameText.text = string.Empty;
            }
            else
            {
                _nameText.text = _currentCharacter.DisplayName;
                _bustImage.gameObject.SetActive(true);
                _bustImage.sprite = _currentCharacter.Sprite;
            }
        }

        public void DisplayNextDialogue()
        {
            if (!_container.activeInHierarchy)
            {
                return;
            }
            if (!_display.IsDisplayDone)
            {
                // We are slowly displaying a text, force the whole display
                _display.ForceDisplay();
            }
            else if (_story.canContinue && // There is text left to write
                !_story.currentChoices.Any()) // We are not currently in a choice
            {
                DisplayStory(_story.Continue());
            }
            else if (!_story.canContinue && !_story.currentChoices.Any())
            {
                _container.SetActive(false);
                _onDone?.Invoke();
            }
        }

        public void ToggleSkip(bool value)
            => _isSkipEnabled = value;

        public void OnToggleSkip(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) ToggleSkip(true);
            else if (value.phase == InputActionPhase.Canceled) ToggleSkip(false);
        }

        public void OnNextDialogue(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                DisplayNextDialogue();
            }
        }
    }
}