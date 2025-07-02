# SoulBound: Layers, Tags & Physics Configuration

## Overview
This document defines the standardized layers, tags, and physics collision matrix for the SoulBound RPG project. These configurations ensure proper object interactions, collision detection, and system organization throughout the game.

## Layer Configuration

### Required Layers
| Layer Index | Layer Name | Purpose | Usage |
|-------------|------------|---------|-------|
| 2 | Ignore Raycast | Unity built-in | Objects that should not be detected by raycasts |
| 8 | Player | Player character | Player GameObject, player-specific colliders |
| 9 | Enemy | Enemy entities | All enemy GameObjects and their colliders |
| 10 | Environment | World geometry | Terrain, walls, floors, static world objects |
| 11 | UI | User interface | UI elements that need physics interaction |

### Layer Assignment Guidelines
- **Player Layer (8)**: Assign to player character and player-specific objects (weapons, shields)
- **Enemy Layer (9)**: Assign to all enemy entities, including AI characters and hostile NPCs
- **Environment Layer (10)**: Assign to all static world geometry that should block movement
- **UI Layer (11)**: Assign to UI elements that require physics-based interaction

## Tag Configuration

### Required Tags
| Tag Name | Purpose | Usage Examples |
|----------|---------|-----------------|
| Player | Identify player objects | Player GameObject, player-controlled entities |
| Enemy | Identify enemy objects | Enemy GameObjects, AI-controlled hostiles |
| NPC | Identify non-player characters | Friendly NPCs, quest givers, merchants |
| Interactable | Objects players can interact with | Chests, doors, switches, collectibles |
| Projectile | Projectile objects | Arrows, spells, thrown objects |

### Tag Assignment Guidelines
- **Player**: Use for the main player GameObject and player-specific entities
- **Enemy**: Use for all hostile entities that can be targeted by player abilities
- **NPC**: Use for friendly or neutral characters that don't engage in combat
- **Interactable**: Use for any object that responds to player interaction (E key)
- **Projectile**: Use for all projectile objects that need special collision handling

## Physics Collision Matrix

### Collision Rules
The following collision matrix defines which layers can physically collide with each other:

| Layer | Player | Enemy | Environment | UI | Ignore Raycast | Projectile |
|-------|--------|-------|-------------|----|----|------------|
| **Player** | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Enemy** | ✅ | ❌ | ✅ | ❌ | ❌ | ✅ |
| **Environment** | ✅ | ✅ | ✅ | ❌ | ❌ | ✅ |
| **UI** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Ignore Raycast** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Projectile** | ❌ | ✅ | ✅ | ❌ | ❌ | ✅ |

### Collision Logic Explained

#### ✅ **Enabled Collisions**
- **Player ↔ Player**: Allow player collision with other player objects
- **Player ↔ Enemy**: Allow combat collision detection
- **Player ↔ Environment**: Prevent player from walking through walls
- **Enemy ↔ Environment**: Prevent enemies from walking through walls
- **Enemy ↔ Projectile**: Allow projectiles to hit enemies
- **Environment ↔ Projectile**: Allow projectiles to hit walls/terrain
- **Projectile ↔ Projectile**: Allow projectiles to collide with each other

#### ❌ **Disabled Collisions**
- **Player ↔ Projectile**: Player projectiles pass through player (prevents self-damage)
- **Enemy ↔ Enemy**: Enemies don't physically block each other (allows AI movement)
- **UI ↔ All**: UI elements don't interfere with physics
- **Ignore Raycast ↔ All**: Special layer for raycast-ignored objects

## Setup Instructions

### Unity Editor Configuration

#### 1. Configure Layers
```
Edit → Project Settings → Tags and Layers

Add the following layers:
• Layer 8: Player
• Layer 9: Enemy  
• Layer 10: Environment
• Layer 11: UI

Note: Layer 2 "Ignore Raycast" should already exist
```

#### 2. Configure Tags
```
In the same Tags and Layers window, add:
• Player
• Enemy
• NPC
• Interactable
• Projectile
```

#### 3. Configure Physics Collision Matrix
```
Edit → Project Settings → Physics

In the Layer Collision Matrix:
• Uncheck UI row/column (disable all UI collisions)
• Uncheck Player-Projectile intersection
• Uncheck Enemy-Enemy intersection
• Ensure Environment collides with Player, Enemy, Projectile
• Uncheck all Ignore Raycast intersections
```

### Validation
Use the built-in LayerTagValidator tool to verify configuration:

```
Unity Menu: SoulBound → Validate Layers & Tags
Or: SoulBound → Quick Validate (console output only)
```

## Usage Examples

### Player Setup
```csharp
// Player GameObject setup
GameObject player = new GameObject("Player");
player.layer = LayerMask.NameToLayer("Player");
player.tag = "Player";

// Player collider
CapsuleCollider collider = player.AddComponent<CapsuleCollider>();
// Physics interactions will follow Player layer rules
```

### Enemy Setup
```csharp
// Enemy GameObject setup
GameObject enemy = new GameObject("Orc");
enemy.layer = LayerMask.NameToLayer("Enemy");
enemy.tag = "Enemy";

// Enemy can be detected by player targeting systems
```

### Environment Setup
```csharp
// Environment GameObject setup
GameObject wall = new GameObject("Wall");
wall.layer = LayerMask.NameToLayer("Environment");
// No tag needed for basic environment objects

// Will block both players and enemies
```

### Projectile Setup
```csharp
// Projectile GameObject setup
GameObject arrow = new GameObject("Arrow");
arrow.layer = LayerMask.NameToLayer("Enemy"); // Use Enemy layer for projectiles
arrow.tag = "Projectile";

// Will hit enemies and environment, pass through player
```

## Collision Detection Patterns

### Player Combat Detection
```csharp
// Detect enemies within attack range
Collider[] enemies = Physics.OverlapSphere(
    transform.position, 
    attackRange, 
    LayerMask.GetMask("Enemy")
);
```

### Projectile Hit Detection
```csharp
// Raycast for projectile hits (ignoring player)
LayerMask hitLayers = LayerMask.GetMask("Enemy", "Environment");
if (Physics.Raycast(origin, direction, out hit, maxDistance, hitLayers))
{
    // Handle projectile hit
}
```

### Interaction Detection
```csharp
// Find interactable objects near player
GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
```

## Best Practices

### Layer Assignment
1. **Assign layers in Awake()** to ensure consistency
2. **Use LayerMask.NameToLayer()** instead of hardcoded indices
3. **Validate layer assignments** in editor scripts
4. **Document layer changes** in commit messages

### Tag Usage
1. **Use tags for identification**, not complex logic
2. **Prefer layer-based collision** over tag-based detection
3. **Use CompareTag()** instead of string comparison
4. **Keep tag logic simple** and readable

### Physics Configuration
1. **Test collision matrix** with simple test objects
2. **Verify raycast behavior** across all layers
3. **Performance test** with many objects per layer
4. **Document collision logic** for team members

## Troubleshooting

### Common Issues
- **Objects falling through floors**: Check Environment layer assignment
- **Player hitting own projectiles**: Verify Player-Projectile collision is disabled
- **Enemies clustering**: Confirm Enemy-Enemy collision is disabled
- **UI blocking clicks**: Ensure UI layer collision is disabled

### Validation Errors
Use the LayerTagValidator to identify and fix configuration issues:
- Missing layers will show specific layer indices to configure
- Missing tags will list which tags need to be added
- Physics matrix issues require manual verification

## Related Documentation
- [Unity Physics Documentation](https://docs.unity3d.com/Manual/PhysicsSection.html)
- [Layer and Tag Best Practices](https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity6.html)
- @LayerTagValidator - Editor validation tool
- @Task_21 - Implementation task documentation

---
**Last Updated**: 2025-01-27  
**Applies To**: All SoulBound project GameObjects and physics interactions 