using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulBound.Core
{
    /// <summary>
    /// Input management system for SoulBound RPG using Unity's Input System
    /// Provides centralized input handling and context switching for different game states
    /// </summary>
    public class InputManager : BaseManager
    {
        [Header("Input Settings")]
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private bool _enableInput = true;

        // Input Action Maps
        private InputActionMap _gameplayMap;
        private InputActionMap _uiMap;
        private InputActionMap _dialogueMap;

        // Core Input Actions
        private InputAction _moveAction;
        private InputAction _attackAction;
        private InputAction _dodgeAction;
        private InputAction _interactAction;
        private InputAction _pauseAction;

        // Public Events for game systems to subscribe to
        public event Action<Vector2> OnMove;
        public event Action OnAttack;
        public event Action OnDodge;
        public event Action OnInteract;
        public event Action OnPause;

        /// <summary>
        /// Current input context (Gameplay, UI, Dialogue)
        /// </summary>
        public InputContext CurrentContext { get; private set; } = InputContext.Gameplay;

        protected override void OnInitialize()
        {
            LogInfo("Starting InputManager initialization...");
            LogInfo($"InputActions null check: {_inputActions == null}");
            
            if (_inputActions == null)
            {
                LogError("InputActionAsset is not assigned! Please assign the InputActions asset in the Inspector.");
                LogError("Make sure to drag the InputActions.inputactions file to the Input Actions field!");
                return;
            }

            LogInfo($"InputActions asset found: {_inputActions.name}");

            // Cache action maps
            _gameplayMap = _inputActions.FindActionMap("Gameplay");
            _uiMap = _inputActions.FindActionMap("UI");
            _dialogueMap = _inputActions.FindActionMap("Dialogue");

            if (_gameplayMap == null)
            {
                LogError("Gameplay action map not found in InputActions asset!");
                return;
            }

            LogInfo($"Found action maps - Gameplay: {_gameplayMap != null}, UI: {_uiMap != null}, Dialogue: {_dialogueMap != null}");

            // Cache individual actions
            _moveAction = _gameplayMap.FindAction("Move");
            _attackAction = _gameplayMap.FindAction("Attack");
            _dodgeAction = _gameplayMap.FindAction("Dodge");
            _interactAction = _gameplayMap.FindAction("Interact");
            _pauseAction = _gameplayMap.FindAction("Pause");

            LogInfo($"Found actions - Move: {_moveAction != null}, Attack: {_attackAction != null}, Dodge: {_dodgeAction != null}, Interact: {_interactAction != null}, Pause: {_pauseAction != null}");

            // Subscribe to action events
            SetupActionCallbacks();

            // Enable input if specified
            if (_enableInput)
            {
                EnableInput();
                // Ensure the Gameplay context is active by default
                SwitchContext(InputContext.Gameplay);
            }

            LogInfo("InputManager initialized successfully");
        }

        private void SetupActionCallbacks()
        {
            if (_moveAction != null)
            {
                _moveAction.performed += OnMovePerformed;
                _moveAction.canceled += OnMoveCanceled;
            }

            if (_attackAction != null)
            {
                _attackAction.performed += OnAttackPerformed;
            }

            if (_dodgeAction != null)
            {
                _dodgeAction.performed += OnDodgePerformed;
            }

            if (_interactAction != null)
            {
                _interactAction.performed += OnInteractPerformed;
            }

            if (_pauseAction != null)
            {
                _pauseAction.performed += OnPausePerformed;
            }
        }

        private void RemoveActionCallbacks()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= OnMovePerformed;
                _moveAction.canceled -= OnMoveCanceled;
            }

            if (_attackAction != null)
            {
                _attackAction.performed -= OnAttackPerformed;
            }

            if (_dodgeAction != null)
            {
                _dodgeAction.performed -= OnDodgePerformed;
            }

            if (_interactAction != null)
            {
                _interactAction.performed -= OnInteractPerformed;
            }

            if (_pauseAction != null)
            {
                _pauseAction.performed -= OnPausePerformed;
            }
        }

        #region Input Action Callbacks

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            LogInfo($"Movement input received: {moveInput}"); // Debug logging
            OnMove?.Invoke(moveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(Vector2.zero);
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            OnAttack?.Invoke();
        }

        private void OnDodgePerformed(InputAction.CallbackContext context)
        {
            OnDodge?.Invoke();
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke();
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            OnPause?.Invoke();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enable input processing
        /// </summary>
        public void EnableInput()
        {
            if (_inputActions != null)
            {
                _inputActions.Enable();
                _enableInput = true;
                LogInfo("Input enabled");
            }
        }

        /// <summary>
        /// Disable input processing
        /// </summary>
        public void DisableInput()
        {
            if (_inputActions != null)
            {
                _inputActions.Disable();
                _enableInput = false;
                LogInfo("Input disabled");
            }
        }

        /// <summary>
        /// Switch input context (Gameplay, UI, Dialogue)
        /// </summary>
        public void SwitchContext(InputContext newContext)
        {
            if (CurrentContext == newContext) return;

            // Disable current context
            DisableCurrentContext();

            // Switch to new context
            CurrentContext = newContext;
            EnableCurrentContext();

            LogInfo($"Input context switched to: {newContext}");
        }

        /// <summary>
        /// Get current movement input (useful for continuous reading)
        /// </summary>
        public Vector2 GetMovementInput()
        {
            if (_moveAction != null && _moveAction.enabled)
            {
                return _moveAction.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Check if a specific action is currently pressed
        /// </summary>
        public bool IsActionPressed(string actionName)
        {
            var action = _gameplayMap?.FindAction(actionName);
            return action != null && action.IsPressed();
        }

        #endregion

        #region Context Management

        private void DisableCurrentContext()
        {
            switch (CurrentContext)
            {
                case InputContext.Gameplay:
                    _gameplayMap?.Disable();
                    break;
                case InputContext.UI:
                    _uiMap?.Disable();
                    break;
                case InputContext.Dialogue:
                    _dialogueMap?.Disable();
                    break;
            }
        }

        private void EnableCurrentContext()
        {
            switch (CurrentContext)
            {
                case InputContext.Gameplay:
                    _gameplayMap?.Enable();
                    break;
                case InputContext.UI:
                    _uiMap?.Enable();
                    break;
                case InputContext.Dialogue:
                    _dialogueMap?.Enable();
                    break;
            }
        }

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            if (_inputActions != null && _enableInput)
            {
                EnableCurrentContext();
            }
        }

        private void OnDisable()
        {
            if (_inputActions != null)
            {
                _inputActions.Disable();
            }
        }

        protected override void OnDestroy()
        {
            RemoveActionCallbacks();
            base.OnDestroy();
            if (_inputActions != null)
            {
                _inputActions.Disable();
            }
        }

        #endregion

        /// <summary>
        /// Add custom debug information to the debug output
        /// </summary>
        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Current Context: {CurrentContext}");
            info.AppendLine($"Input Enabled: {_enableInput}");
            info.AppendLine($"Input Actions Asset: {(_inputActions != null ? _inputActions.name : "Not Assigned")}");
            
            if (_inputActions != null)
            {
                info.AppendLine($"Gameplay Map Enabled: {(_gameplayMap != null && _gameplayMap.enabled)}");
                info.AppendLine($"UI Map Enabled: {(_uiMap != null && _uiMap.enabled)}");
                info.AppendLine($"Dialogue Map Enabled: {(_dialogueMap != null && _dialogueMap.enabled)}");
            }
        }
    }

    /// <summary>
    /// Input context enumeration for different game states
    /// </summary>
    public enum InputContext
    {
        Gameplay,
        UI,
        Dialogue
    }
} 