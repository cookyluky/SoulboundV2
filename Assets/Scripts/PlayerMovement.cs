using UnityEngine;
using SoulBound.Core;

namespace SoulBound
{
    /// <summary>
    /// Player movement controller that integrates with InputManager for event-driven input handling
    /// Provides physics-based movement using CharacterController with smooth acceleration and jumping
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpHeight = 8f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayers = 1;
        
        [Header("Acceleration Settings")]
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        [SerializeField] private float airControl = 0.3f;
        
        [Header("Debug Settings")]
        [SerializeField] private bool enableDebugLogging = true;
        [SerializeField] private bool showGroundCheckGizmo = true;
        
        // Components
        private CharacterController _characterController;
        private InputManager _inputManager;
        
        // Movement state
        private Vector3 _velocity;
        private Vector2 _moveInput;
        private bool _isGrounded;
        private bool _jumpRequested;
        
        // Debug info
        private Vector3 _lastPosition;
        private float _currentSpeed;

        #region Unity Lifecycle

        private void Start()
        {
            Debug.Log("[PlayerMovement] Starting PlayerMovement initialization...");
            
            // Initialize components first (CharacterController doesn't need ServiceLocator)
            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
            {
                Debug.LogError("[PlayerMovement] CharacterController component is required but not found!");
                enabled = false;
                return;
            }
            
            _lastPosition = transform.position;
            
            // Start coroutine to wait for InputManager
            StartCoroutine(WaitForInputManagerAndInitialize());
        }
        
        private System.Collections.IEnumerator WaitForInputManagerAndInitialize()
        {
            // Wait for ServiceLocator to register InputManager
            int attempts = 0;
            const int maxAttempts = 100; // 5 seconds max wait at 50fps
            
            while (_inputManager == null && attempts < maxAttempts)
            {
                // Try to get InputManager without try-catch in coroutine
                if (ServiceLocator.IsRegistered<InputManager>())
                {
                    _inputManager = ServiceLocator.Get<InputManager>();
                    break;
                }
                
                // InputManager not registered yet, wait one frame
                attempts++;
                yield return null;
            }
            
            if (_inputManager == null)
            {
                Debug.LogError("[PlayerMovement] Failed to find InputManager after waiting. Player movement will not work!");
                enabled = false;
                yield break;
            }
            
            // Now setup input subscriptions
            SetupInputSubscriptions();
            
            if (enableDebugLogging)
            {
                Debug.Log("[PlayerMovement] Initialized successfully");
                Debug.Log($"[PlayerMovement] CharacterController found: {_characterController != null}");
                Debug.Log($"[PlayerMovement] InputManager found: {_inputManager != null}");
                Debug.Log($"[PlayerMovement] Initialization took {attempts} frames");
            }
        }

        private void Update()
        {
            // Ground detection
            CheckGrounded();
            
            // Apply movement
            ApplyHorizontalMovement();
            ApplyVerticalMovement();
            
            // Move character
            _characterController.Move(_velocity * Time.deltaTime);
            
            // Update debug info
            UpdateDebugInfo();
        }

        private void OnDestroy()
        {
            // Clean up event subscriptions
            RemoveInputSubscriptions();
        }

        #endregion

        #region Initialization



        private void SetupInputSubscriptions()
        {
            if (_inputManager != null)
            {
                _inputManager.OnMove += HandleMoveInput;
                _inputManager.OnDodge += HandleJumpInput; // Space bar triggers Dodge action, using for Jump
                
                if (enableDebugLogging)
                {
                    Debug.Log("[PlayerMovement] Input event subscriptions established");
                }
            }
        }

        private void RemoveInputSubscriptions()
        {
            if (_inputManager != null)
            {
                _inputManager.OnMove -= HandleMoveInput;
                _inputManager.OnDodge -= HandleJumpInput;
            }
        }

        #endregion

        #region Input Handling

        private void HandleMoveInput(Vector2 input)
        {
            _moveInput = input;
            
            if (enableDebugLogging)
            {
                if (input.magnitude > 0.1f)
                {
                    Debug.Log($"[PlayerMovement] Move input received: ({input.x:F1}, {input.y:F1})");
                }
                else if (input == Vector2.zero && _moveInput.magnitude > 0.1f)
                {
                    Debug.Log("[PlayerMovement] Move input stopped");
                }
            }
        }

        private void HandleJumpInput()
        {
            if (_isGrounded)
            {
                _jumpRequested = true;
                
                if (enableDebugLogging)
                {
                    Debug.Log("[PlayerMovement] Jump requested");
                }
            }
        }

        #endregion

        #region Movement Logic

        private void CheckGrounded()
        {
            // Use CharacterController's built-in ground detection
            bool wasGrounded = _isGrounded;
            _isGrounded = _characterController.isGrounded;
            
            // Additional ground check using raycast for more reliable detection
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            RaycastHit hit;
            bool raycastGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, 
                groundCheckDistance + 0.1f, groundLayers);
            
            _isGrounded = _isGrounded || raycastGrounded;
            
            // Log ground state changes
            if (enableDebugLogging && wasGrounded != _isGrounded)
            {
                Debug.Log($"[PlayerMovement] Ground state changed: {_isGrounded}");
            }
        }

        private void ApplyHorizontalMovement()
        {
            // Convert input to world space movement
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
            
            if (move.magnitude >= 0.1f)
            {
                // Apply movement with acceleration
                float targetSpeed = moveSpeed;
                Vector3 targetVelocity = move * targetSpeed;
                
                // Use different acceleration based on grounded state
                float accelRate = _isGrounded ? acceleration : acceleration * airControl;
                
                _velocity.x = Mathf.MoveTowards(_velocity.x, targetVelocity.x, accelRate * Time.deltaTime);
                _velocity.z = Mathf.MoveTowards(_velocity.z, targetVelocity.z, accelRate * Time.deltaTime);
            }
            else
            {
                // Apply deceleration when no input
                float decelRate = _isGrounded ? deceleration : deceleration * airControl;
                
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, decelRate * Time.deltaTime);
                _velocity.z = Mathf.MoveTowards(_velocity.z, 0, decelRate * Time.deltaTime);
            }
        }

        private void ApplyVerticalMovement()
        {
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Small downward force to keep grounded
            }
            
            // Handle jumping
            if (_jumpRequested && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _jumpRequested = false;
                
                if (enableDebugLogging)
                {
                    Debug.Log($"[PlayerMovement] Jump executed with velocity: {_velocity.y:F1}");
                }
            }
            
            // Apply gravity
            _velocity.y += gravity * Time.deltaTime;
        }

        #endregion

        #region Debug and Utilities

        private void UpdateDebugInfo()
        {
            // Calculate current speed
            Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
            _currentSpeed = horizontalVelocity.magnitude;
            
            // Update last position
            _lastPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            if (showGroundCheckGizmo)
            {
                // Draw ground check ray
                Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
                Gizmos.color = _isGrounded ? Color.green : Color.red;
                Gizmos.DrawRay(rayOrigin, Vector3.down * (groundCheckDistance + 0.1f));
                
                // Draw ground check sphere
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _characterController ? _characterController.radius : 0.5f);
            }
        }

        /// <summary>
        /// Get current movement speed for external systems
        /// </summary>
        public float GetCurrentSpeed()
        {
            return _currentSpeed;
        }

        /// <summary>
        /// Get current movement velocity for external systems
        /// </summary>
        public Vector3 GetVelocity()
        {
            return _velocity;
        }

        /// <summary>
        /// Check if player is currently grounded
        /// </summary>
        public bool IsGrounded()
        {
            return _isGrounded;
        }

        /// <summary>
        /// Get debug information about current movement state
        /// </summary>
        public string GetDebugInfo()
        {
            return $"Speed: {_currentSpeed:F1} | Grounded: {_isGrounded} | Velocity: {_velocity} | Input: {_moveInput}";
        }

        #endregion

        #region Context Menu Items

        [ContextMenu("Print Movement Debug Info")]
        private void PrintDebugInfo()
        {
            Debug.Log($"[PlayerMovement] {GetDebugInfo()}");
        }

        [ContextMenu("Test Jump")]
        private void TestJump()
        {
            HandleJumpInput();
        }

        #endregion
    }
} 