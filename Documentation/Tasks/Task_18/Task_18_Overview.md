# Task 18 Overview: Input System & Player Controller

## Task Description
Wire up Unity's Input System into a PlayerController stub. Establish the complete input pipeline from hardware input to player character actions, supporting multiple input methods across all target platforms.

## Priority Level
**High** - Essential for any player interaction. Required before any gameplay systems can be tested or implemented.

## Dependencies
- Task 17: Core Manager Singletons (specifically InputManager)

## Detailed Breakdown

### Core Objectives
1. **Unity Input System Integration**
   - Import and configure Unity's new Input System package
   - Replace legacy input system throughout the project
   - Configure for cross-platform compatibility

2. **Input Action Asset Creation**
   - Comprehensive input mapping for all game controls
   - Support for keyboard/mouse, gamepad, and touch input
   - Configurable control schemes for different platforms

3. **InputManager Event System**
   - C# event-based input broadcasting
   - Type-safe input handling throughout the application
   - Input validation and filtering

4. **PlayerController Foundation**
   - Basic player character control stub
   - Input event subscription and handling
   - Foundation for movement, combat, and interaction systems

5. **Player Prefab Creation**
   - Complete player GameObject with all necessary components
   - Proper physics setup for character movement
   - Visual representation and basic animations

## Technical Requirements

### Input Action Asset Structure
```
InputActions.inputactions
├── Player Action Map
│   ├── Move (Vector2) - WASD, Left Stick, Touch Joystick
│   ├── Look (Vector2) - Mouse, Right Stick, Touch Drag
│   ├── Attack (Button) - Left Click, RT/R2, Touch Button
│   ├── Dodge (Button) - Space, B/Circle, Touch Button
│   ├── Interact (Button) - E, A/X, Touch Button
│   ├── Block (Button) - Right Click, LT/L2, Touch Button
│   └── Pause (Button) - Escape, Menu/Options, Touch Button
└── UI Action Map
    ├── Navigate (Vector2) - Arrow Keys, D-Pad
    ├── Submit (Button) - Enter, A/X
    ├── Cancel (Button) - Escape, B/Circle
    └── Point (Vector2) - Mouse Position
```

### Core Classes
```csharp
public class InputManager : MonoBehaviour {
    [SerializeField] private InputActionAsset inputActions;
    
    // Player Input Events
    public event System.Action<Vector2> OnMove;
    public event System.Action<Vector2> OnLook;
    public event System.Action OnAttack;
    public event System.Action OnDodge;
    public event System.Action OnInteract;
    public event System.Action OnBlock;
    public event System.Action OnPause;
    
    // Input State
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsAttacking { get; private set; }
    
    public void Initialize();
    public void SetInputEnabled(bool enabled);
    public void SwitchActionMap(string mapName);
}

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTarget;
    
    private Vector2 moveInput;
    private InputManager inputManager;
    
    public void Move(Vector2 input);
    public void Attack();
    public void Dodge();
    public void Interact();
    public void Block();
}
```

### Integration Points
- **InputManager**: Central input event broadcasting
- **ServiceLocator**: InputManager registration and access
- **Player Prefab**: PlayerController component integration
- **Camera System**: Input for camera control and targeting
- **UI System**: Input switching between gameplay and UI modes

## Success Criteria
- [ ] Unity Input System package imported and configured
- [ ] InputActions.inputactions asset created with complete control mapping
- [ ] InputManager properly broadcasts input events to subscribers
- [ ] PlayerController responds to all input events with stub implementations
- [ ] Player.prefab created with Rigidbody, Collider, and PlayerController
- [ ] Cross-platform input compatibility verified
- [ ] Input switching between gameplay and UI modes functional
- [ ] Input validation prevents invalid combinations
- [ ] Performance optimized for 60+ FPS input polling
- [ ] Memory allocation minimized during input processing

## Risk Factors
- **Platform Input Differences**: Touch, gamepad, and keyboard inputs behave differently
- **Input Lag**: Poor input system implementation can cause noticeable lag
- **Event Flooding**: High-frequency input events may impact performance
- **Input Conflicts**: Multiple systems trying to handle the same input
- **Device Detection**: Automatic detection of input device changes

## Related Systems
- **Combat System**: Will extend PlayerController with combat input handling
- **UI System**: Input switching and navigation integration
- **Camera System**: Look input for camera control
- **Movement System**: Move input for character locomotion
- **Interaction System**: Interact input for world object interaction

## Input System Architecture

### Control Schemes
1. **Keyboard & Mouse**
   - WASD movement, mouse look
   - Left/right click for primary/secondary actions
   - Space for dodge, E for interact
   - Escape for pause menu

2. **Gamepad**
   - Left stick movement, right stick look
   - RT/R2 for attack, LT/L2 for block
   - A/X for interact, B/Circle for dodge
   - Menu/Options for pause

3. **Touch (Mobile)**
   - Virtual joystick for movement
   - Touch drag for camera control
   - Touch buttons for actions
   - Gesture support for special actions

### Input Processing Pipeline
1. **Hardware Input** → Input System
2. **Input Actions** → Action callbacks
3. **InputManager** → C# events
4. **PlayerController** → Game actions
5. **Game Systems** → Visual feedback

### Input State Management
- **Action Maps**: Switch between Player, UI, and Menu input modes
- **Input Buffering**: Queue inputs during state transitions
- **Input Validation**: Prevent invalid input combinations
- **Input Recording**: Debug and replay functionality

## Performance Considerations
- **Event Allocation**: Minimize garbage collection from input events
- **Update Frequency**: Optimize input polling for consistent performance
- **Input Buffering**: Smooth input delivery during frame rate fluctuations
- **Device Polling**: Efficient detection of input device changes

## Estimated Completion Time
**4-6 days** - Includes Input System setup, InputManager implementation, PlayerController stub, Player prefab creation, cross-platform testing, and integration validation. 