# Subtask 18.2.1: Player Movement Controls

## Overview
**Task**: @Task_18 - Input System and Player Controller  
**Subtask**: 18.2 - Implement Player Movement Controls  
**Status**: In Progress  
**Dependencies**: ✅ @Task_18.1 (Input System Integration) Complete  
**Complexity**: 5/10  
**Priority**: High  

## Objective
Create a physics-based player movement system that integrates with the InputManager's event-driven architecture to provide responsive, smooth character movement.

## Technical Requirements

### Core Components
- **PlayerMovement.cs**: Main movement controller script
- **CharacterController**: Unity physics component for movement
- **InputManager Integration**: Event-based input handling
- **Ground Detection**: Physics-based ground checking for jumping
- **Movement Parameters**: Configurable speed, acceleration, and physics settings

### Input Integration
- **Subscribe to InputManager.OnMove**: Vector2 movement input (WASD/analog stick)
- **Subscribe to InputManager.OnAttack**: Basic attack action (spacebar/button)
- **Subscribe to InputManager.OnDodge**: Dodge/dash mechanics (shift/button)
- **Subscribe to InputManager.OnInteract**: Interaction system (E/button)
- **Context Awareness**: Respect current input context (Gameplay/UI/Dialogue)

### Movement Mechanics
- **Horizontal Movement**: WASD/analog stick with smooth acceleration/deceleration
- **Jump System**: Spacebar/button with ground detection and gravity
- **Physics Integration**: Use CharacterController for collision detection
- **Debug Visualization**: Console logging and optional visual indicators

## Implementation Steps

### Step 1: Create PlayerMovement Script
**File**: `Assets/Scripts/PlayerMovement.cs`

**Script Structure**:
```csharp
using UnityEngine;
using SoulBound.Core;

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
    
    // Components
    private CharacterController characterController;
    private InputManager inputManager;
    
    // Movement state
    private Vector3 velocity;
    private Vector2 moveInput;
    private bool isGrounded;
    
    // Events and input subscriptions
}
```

### Step 2: Input Manager Integration
**Event Subscription Pattern**:
```csharp
private void Start()
{
    // Get InputManager from ServiceLocator
    inputManager = ServiceLocator.GetManager<InputManager>();
    
    // Subscribe to input events
    inputManager.OnMove += HandleMoveInput;
    inputManager.OnAttack += HandleJumpInput; // Using Attack as Jump for now
    
    // Component references
    characterController = GetComponent<CharacterController>();
}

private void OnDestroy()
{
    // Unsubscribe from events
    if (inputManager != null)
    {
        inputManager.OnMove -= HandleMoveInput;
        inputManager.OnAttack -= HandleJumpInput;
    }
}
```

### Step 3: Movement Logic Implementation
**Physics-Based Movement**:
```csharp
private void Update()
{
    // Ground detection
    CheckGrounded();
    
    // Apply movement
    ApplyHorizontalMovement();
    ApplyVerticalMovement();
    
    // Move character
    characterController.Move(velocity * Time.deltaTime);
}

private void HandleMoveInput(Vector2 input)
{
    moveInput = input;
}

private void ApplyHorizontalMovement()
{
    Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
    
    if (move.magnitude >= 0.1f)
    {
        // Apply movement with acceleration
        float targetSpeed = moveSpeed;
        Vector3 targetVelocity = move * targetSpeed;
        
        float smoothTime = isGrounded ? acceleration : acceleration * airControl;
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, smoothTime * Time.deltaTime);
        velocity.z = Mathf.MoveTowards(velocity.z, targetVelocity.z, smoothTime * Time.deltaTime);
    }
    else
    {
        // Apply deceleration
        float decelerationRate = isGrounded ? deceleration : deceleration * airControl;
        velocity.x = Mathf.MoveTowards(velocity.x, 0, decelerationRate * Time.deltaTime);
        velocity.z = Mathf.MoveTowards(velocity.z, 0, decelerationRate * Time.deltaTime);
    }
}
```

### Step 4: Unity Editor Setup

#### Create Player GameObject
1. **Right-click in Hierarchy** → **3D Object** → **Capsule**
2. **Name it**: `Player`
3. **Position**: (0, 1, 0) - above ground level
4. **Add Component** → **Character Controller**
5. **Configure CharacterController**:
   - **Height**: 2.0
   - **Radius**: 0.5
   - **Center**: (0, 0, 0)

#### Attach PlayerMovement Script
1. **Select Player GameObject**
2. **Add Component** → **Scripts** → **Player Movement**
3. **Configure Movement Settings**:
   - **Move Speed**: 5.0
   - **Jump Height**: 8.0
   - **Gravity**: -9.81
   - **Ground Check Distance**: 0.2
   - **Ground Layers**: Default (Layer 0)

#### Create Ground Plane
1. **Right-click in Hierarchy** → **3D Object** → **Plane**
2. **Name it**: `Ground`
3. **Position**: (0, 0, 0)
4. **Scale**: (2, 1, 2) - larger ground area

### Step 5: Testing and Validation

#### Test Movement
1. **Enter Play Mode**
2. **Test WASD Movement**: Character should move horizontally
3. **Test Jump**: Spacebar should make character jump (if implemented)
4. **Check Console**: Look for movement debug logs

#### Expected Console Output
```
[PlayerMovement] Initialized successfully
[PlayerMovement] InputManager connection established
[PlayerMovement] Ground detected: True
[PlayerMovement] Move input: (1.0, 0.0) - Moving right
[PlayerMovement] Velocity: (5.0, 0.0, 0.0)
```

#### Common Issues and Solutions
- **No Movement**: Check InputActions asset assignment in InputManager
- **Sliding**: Adjust deceleration values
- **Jerky Movement**: Increase acceleration smoothing
- **No Ground Detection**: Verify Ground Layers settings

## Success Criteria
- ✅ **WASD Movement**: Character moves smoothly in all horizontal directions
- ✅ **Input Integration**: InputManager events properly connected
- ✅ **Physics Response**: CharacterController handles collisions
- ✅ **Ground Detection**: Character detects ground for jumping
- ✅ **Debug Logging**: Clear console output showing movement states
- ✅ **Performance**: Smooth movement at 60fps with no stuttering

## Cross-References
**Parent Task**: @Task_18 - [Task_18_Overview.md](mdc:Documentation/Tasks/Task_18/Task_18_Overview.md)  
**Previous Subtask**: @Task_18.1 - [Subtask_18.1.1.md](mdc:Documentation/Tasks/Task_18/Subtask_18.1.1.md)  
**Input System**: @InputManager - [InputManager.md](mdc:Documentation/Objects/Scripts/InputManager.md)  
**Implementation Log**: [Task_18_Implementation_log.md](mdc:Documentation/Tasks/Task_18/Task_18_Implementation_log.md)

## Notes
- This implementation focuses on basic movement mechanics
- Jump system can be enhanced in future subtasks
- Camera follow system will be implemented separately
- Animation integration planned for later tasks 