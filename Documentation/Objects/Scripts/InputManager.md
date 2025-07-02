# InputManager

## Object Information
**Type**: Core Manager Script
**Location**: Assets/Scripts/Core/InputManager.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Centralized input management system for SoulBound RPG using Unity's modern Input System package. Provides event-driven input handling with context switching capabilities for different game states (Gameplay, UI, Dialogue). Serves as the foundation for all player input throughout the game.

## Components

### InputManager Class
- **Script**: InputManager.cs
- **Purpose**: Core input system manager with event-driven architecture
- **Inheritance**: Extends BaseManager for consistent lifecycle management
- **Key Properties**:
  - `_inputActions`: InputActionAsset - Main input action asset reference
  - `_enableInput`: bool - Global input enable/disable flag
  - `CurrentContext`: InputContext - Active input context (Gameplay/UI/Dialogue)

### Input Action Maps
- **Gameplay Map**: Core game actions (Move, Attack, Dodge, Interact, Pause)
- **UI Map**: User interface navigation (Navigate, Submit, Cancel)
- **Dialogue Map**: Text interaction (Advance, Skip)

### Public Events
- **OnMove**: Action<Vector2> - Movement input events
- **OnAttack**: Action - Attack button events  
- **OnDodge**: Action - Dodge button events
- **OnInteract**: Action - Interaction button events
- **OnPause**: Action - Pause button events

## Dependencies

### Required Packages
- **Unity Input System**: v1.7.0 (com.unity.inputsystem)

### Required Assets
- **InputActions Asset**: Assets/Input/InputActions.inputactions
  - Must contain exactly named action maps: "Gameplay", "UI", "Dialogue"
  - Must contain specific actions with correct naming convention

### Script Dependencies
- **BaseManager.cs**: Provides core manager functionality
- **ServiceLocator.cs**: For dependency injection registration
- **Bootstrapper.cs**: Handles initialization sequence

## Integration Points

### Systems
- **PlayerController**: Subscribes to movement and action events
- **UIManager**: Uses UI action map for menu navigation
- **DialogueSystem**: Uses Dialogue action map for text advancement
- **GameManager**: Receives pause events for game state management

### Events
- **Input Events**: Published to all subscribing systems
- **Context Switch Events**: Automatic action map switching
- **Lifecycle Events**: Proper enable/disable during scene transitions

### Interfaces
- **IManager**: Implemented through BaseManager inheritance
- **Unity Input System**: Direct integration with InputActionAsset

## Usage Instructions

### Developer Integration
```csharp
// Subscribe to input events in any system
var inputManager = ServiceLocator.Get<InputManager>();
inputManager.OnMove += HandleMovement;
inputManager.OnAttack += HandleAttack;

// Switch input contexts
inputManager.SwitchContext(InputContext.UI);

// Check continuous input
Vector2 currentMovement = inputManager.GetMovementInput();
```

### Unity Editor Setup
1. **Assign InputActions Asset**: Drag InputActions.inputactions to Input Manager component
2. **Verify Action Maps**: Ensure all required action maps exist in asset
3. **Test Integration**: Play scene and verify console output shows successful initialization

### Context Management
- **Gameplay Context**: Enabled during normal gameplay
- **UI Context**: Automatically switches when UI panels open
- **Dialogue Context**: Switches during conversation sequences

## History Log

### 2025-01-27 16:30:00 - Complete Unity Input System Integration

**Major Implementation**: Complete rewrite from fallback legacy input system to proper Unity Input System integration.

**Files Modified**:
- `Assets/Scripts/Core/InputManager.cs` - Completely rewritten with modern architecture
- `Packages/manifest.json` - Added Unity Input System package dependency
- `Assets/Input/` directory created for input assets

**Key Changes**:
- **Event-Driven Architecture**: Replaced polling-based input with event callbacks
- **Context Switching**: Added support for multiple input contexts (Gameplay/UI/Dialogue)
- **Action Maps**: Implemented proper separation of input contexts using Unity's action map system
- **Lifecycle Management**: Added proper enable/disable functionality with Unity lifecycle integration
- **Error Handling**: Comprehensive validation and error reporting
- **Debug Support**: Enhanced logging and debug information

**Integration Impact**:
- **Bootstrapper**: Requires InputActions asset assignment in Inspector
- **Future PlayerController**: Will subscribe to input events instead of polling
- **UI Systems**: Can now use dedicated UI action map for navigation
- **Dialogue System**: Has dedicated action map for text interaction

**Testing Results**:
- Code compilation successful after Unity Input System package installation
- Event system architecture validated through code review
- Requires manual Unity Editor steps to create InputActions asset
- Integration testing pending InputActions asset creation

**Technical Specifications**:
- **Package Version**: Unity Input System v1.7.0
- **Architecture**: Event-driven with action map separation
- **Supported Platforms**: Keyboard/Mouse + Gamepad
- **Context Types**: Gameplay, UI, Dialogue
- **Action Types**: Move (Vector2), Attack/Dodge/Interact/Pause (Button)

**Next Steps**:
- User must create InputActions.inputactions asset using provided instructions
- PlayerController implementation will integrate with this event system
- UI systems will leverage UI action map for navigation
- Testing and validation of complete input pipeline

---
**This implementation provides a robust, scalable input foundation for the entire SoulBound RPG project.** 