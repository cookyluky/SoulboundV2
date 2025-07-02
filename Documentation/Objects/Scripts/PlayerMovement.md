# PlayerMovement

## Object Information
**Type**: Script Component
**Location**: Assets/Scripts/PlayerMovement.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Physics-based player movement controller that integrates with the InputManager's event-driven architecture to provide responsive, smooth character movement with jumping mechanics.

## Components & Dependencies

### Required Components
- **CharacterController** - Unity built-in physics component for collision detection and movement
- **Transform** - Unity built-in for position, rotation, and scale

### System Dependencies
- **InputManager** - Retrieved via ServiceLocator for input event subscriptions
- **ServiceLocator** - Core dependency injection system for manager access

### Script Dependencies
```csharp
using UnityEngine;
using SoulBound.Core;
```

## Configuration Parameters

### Movement Settings
- **Move Speed**: `5f` - Base horizontal movement speed in units/second
- **Jump Height**: `8f` - Maximum jump height in units
- **Gravity**: `-9.81f` - Downward acceleration in units/second²
- **Ground Check Distance**: `0.2f` - Raycast distance for ground detection
- **Ground Layers**: `1` - LayerMask for what counts as ground

### Acceleration Settings
- **Acceleration**: `10f` - Rate of speed increase when moving
- **Deceleration**: `15f` - Rate of speed decrease when stopping
- **Air Control**: `0.3f` - Movement control multiplier when airborne

### Debug Settings
- **Enable Debug Logging**: `true` - Console output for movement events
- **Show Ground Check Gizmo**: `true` - Visual ground detection in Scene view

## Core Functionality

### Input Integration
- **Event Subscription**: Subscribes to InputManager.OnMove and InputManager.OnAttack events
- **Move Input**: Handles Vector2 input for WASD/analog stick movement
- **Jump Input**: Uses Attack event as jump trigger (temporary mapping)
- **Automatic Cleanup**: Unsubscribes from events on destroy

### Movement Mechanics
- **Horizontal Movement**: Smooth acceleration/deceleration with configurable parameters
- **Vertical Movement**: Physics-based jumping with gravity application
- **Ground Detection**: Dual system using CharacterController.isGrounded + raycast
- **Air Control**: Reduced movement responsiveness when airborne

### Physics Integration
- **CharacterController**: Used for collision detection and movement application
- **Raycast Ground Check**: Additional ground detection for reliability
- **Velocity Management**: Maintains 3D velocity vector with proper physics application

## Public Interface

### Methods
```csharp
public float GetCurrentSpeed()           // Current horizontal movement speed
public Vector3 GetVelocity()            // Current velocity vector
public bool IsGrounded()                // Ground detection state
public string GetDebugInfo()            // Formatted debug information
```

### Context Menu Items
- **Print Movement Debug Info** - Console output of current state
- **Test Jump** - Trigger jump for testing purposes

## Integration Points

### ServiceLocator Pattern
```csharp
_inputManager = ServiceLocator.GetManager<InputManager>();
```

### Event-Driven Input
```csharp
_inputManager.OnMove += HandleMoveInput;
_inputManager.OnAttack += HandleJumpInput;
```

### Component Validation
- Checks for required CharacterController component
- Validates InputManager availability via ServiceLocator
- Disables script if critical dependencies missing

## Debug Features

### Visual Debugging
- **Ground Check Ray**: Red/green ray showing ground detection
- **Collision Sphere**: Yellow wireframe showing character bounds
- **Gizmos**: Only visible when GameObject is selected

### Console Logging
- Initialization success/failure messages
- Input event logging with values
- Ground state changes
- Jump execution with velocity values
- Component validation errors

### Debug Information
Real-time movement state including:
- Current speed (units/second)
- Ground detection status
- 3D velocity vector
- Input values

## Usage Instructions

### Unity Setup
1. **Create Player GameObject** with Capsule mesh
2. **Add CharacterController** component with Height: 2.0, Radius: 0.5
3. **Add PlayerMovement** script component
4. **Configure parameters** in Inspector as needed
5. **Create Ground** plane for testing

### Runtime Behavior
- **WASD Movement**: Smooth horizontal movement with acceleration
- **Spacebar Jump**: Physics-based jumping (via Attack input)
- **Automatic Ground**: Continuous ground detection and physics
- **Debug Output**: Console logging of movement events

### Integration Requirements
- **InputManager**: Must be initialized and registered in ServiceLocator
- **InputActions**: Must have Gameplay action map with Move and Attack actions
- **Ground Objects**: Must be on layers included in Ground Layers mask

## Error Handling

### Component Validation
- **Missing CharacterController**: Logs error and disables script
- **Missing InputManager**: Logs error and disables script
- **ServiceLocator Failure**: Gracefully handles manager not found

### Runtime Safety
- **Null Checks**: All component references validated before use
- **Event Cleanup**: Proper unsubscription prevents memory leaks
- **Physics Validation**: Ground detection uses multiple methods

## Performance Considerations

### Optimization Features
- **Efficient Ground Check**: Uses CharacterController + single raycast
- **Input Caching**: Stores input values to minimize event overhead
- **Conditional Logging**: Debug output only when enabled
- **Gizmo Control**: Visual debugging can be disabled

### Frame Rate Impact
- **Update Frequency**: Runs in Update() for smooth movement
- **Physics Integration**: Uses Time.deltaTime for frame-rate independence
- **Minimal Allocations**: Reuses Vector3 and avoids new object creation

## Testing Strategy

### Unit Testing
- **Component Initialization**: Verify required components found
- **Input Handling**: Test event subscription and input processing
- **Movement Calculation**: Validate velocity calculations
- **Ground Detection**: Test ground state detection accuracy

### Integration Testing
- **InputManager Integration**: Verify event system connectivity
- **ServiceLocator Integration**: Test manager retrieval
- **Physics Integration**: Validate CharacterController behavior

### Manual Testing
- **Movement Responsiveness**: WASD input should feel smooth
- **Jump Mechanics**: Spacebar should produce consistent jumps
- **Ground Detection**: Character should stick to ground properly
- **Debug Output**: Console should show relevant movement information

## Cross-References
**Created For**: @Task_18.2 - [Subtask_18.2.1.md](mdc:Documentation/Tasks/Task_18/Subtask_18.2.1.md)
**Input System**: @InputManager - [InputManager.md](mdc:Documentation/Objects/Scripts/InputManager.md)
**Parent Task**: @Task_18 - [Task_18_Implementation_log.md](mdc:Documentation/Tasks/Task_18/Task_18_Implementation_log.md)
**Core Architecture**: @ServiceLocator - Core dependency injection system

## History Log

### 2025-01-27 17:00:00 - Initial Creation
**Task**: @Task_18.2 - Player Movement Controls implementation

**Implementation Features**:
- Event-driven input integration with InputManager
- Physics-based movement using CharacterController
- Smooth acceleration/deceleration system
- Ground detection with dual validation
- Comprehensive debug and visualization tools
- ServiceLocator integration for dependency injection

**Integration Points**:
- InputManager event subscription (OnMove, OnAttack)
- ServiceLocator for manager retrieval
- CharacterController for physics and collision
- LayerMask system for ground detection

**Testing Approach**:
- Component validation with error handling
- Runtime debugging with console logging
- Visual gizmos for ground detection verification
- Context menu items for manual testing

**Next Steps**:
- Unity Editor setup with Player GameObject
- Movement testing and parameter tuning
- Integration validation with InputManager
- Performance testing and optimization 