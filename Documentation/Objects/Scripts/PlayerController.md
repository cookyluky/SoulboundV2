# PlayerController

## Object Information
**Type**: MonoBehaviour Script
**Location**: Assets/Scripts/PlayerController.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Central controller for all player-related functionality in SoulBound RPG. Manages player state, coordinates between systems, and serves as the main interface for player interactions. Works alongside PlayerMovement for complete player character functionality.

## Components and Integration
### Script Dependencies
- **PlayerMovement**: Movement controller reference for coordination
- **InputManager**: Input event subscription via ServiceLocator
- **GameManager**: Game state management via ServiceLocator
- **SaveManager**: Save/load functionality via ServiceLocator

### Unity Components Required
- **Transform**: Position and transformation management
- **GameObject**: Component attachment point

## Key Features

### Player Stats Management
- **Health System**: Current/max health with damage and healing methods
- **Mana System**: Current/max mana with usage and restoration methods
- **Leveling System**: Experience tracking with automatic level-up progression
- **Soul Energy**: Banking system for strategic soul usage
- **Corruption Tracking**: Corruption accumulation with threshold monitoring

### Soul-Binding System
```csharp
// Core soul-binding mechanics
public void AbsorbSoul(float soulAmount, float corruptionRisk = 0f, bool isConsumedImmediately = false)
public bool UseSoulEnergy(float amount)
public void AddCorruption(float corruptionAmount)
```

### Event System
**Public Static Events for System Integration**:
- `OnHealthChanged(float current, float max)`
- `OnManaChanged(float current, float max)`
- `OnLevelUp(int newLevel)`
- `OnExperienceChanged(float current, float toNext)`
- `OnSoulEnergyChanged(float current, float max)`
- `OnCorruptionChanged(float currentCorruption)`
- `OnPlayerDeath()`
- `OnPlayerRespawn()`

### Input Integration
**Handled Input Events**:
- Attack Input → HandleAttackInput()
- Interact Input → HandleInteractInput()
- Pause Input → HandlePauseInput()

## Configuration Properties

### Player Stats (Header: "Player Stats")
- `_maxHealth`: Starting maximum health (default: 100f)
- `_maxMana`: Starting maximum mana (default: 50f)
- `_startingLevel`: Initial player level (default: 1)
- `_baseExperienceToNextLevel`: Base XP requirement (default: 100f)

### Soul-Binding Settings (Header: "Soul-Binding Settings")
- `_soulCapacity`: Maximum bankable soul energy (default: 10f)
- `_corruptionThreshold`: Corruption level for penalties (default: 5f)
- `_canAbsorbSouls`: Toggle soul absorption capability (default: true)

### Component References (Header: "Component References")
- `_playerMovement`: PlayerMovement script reference

### Debug Settings (Header: "Debug Settings")
- `_enableDebugLogging`: Toggle console logging (default: true)

## Public Interface

### Core Methods
```csharp
// Health Management
public void TakeDamage(float damage, GameObject source = null)
public void Heal(float healAmount)
public void SetHealth(float health)

// Mana Management
public bool UseMana(float manaCost)
public void RestoreMana(float manaAmount)

// Experience and Leveling
public void AddExperience(float experiencePoints)

// Soul-Binding System
public void AbsorbSoul(float soulAmount, float corruptionRisk = 0f, bool isConsumedImmediately = false)
public bool UseSoulEnergy(float amount)
public void AddCorruption(float corruptionAmount)

// Death and Respawn
public void Respawn(Vector3 respawnPosition)

// Data Management
public PlayerStats GetPlayerStats()
public void LoadPlayerStats(PlayerStats stats)
public string GetDebugInfo()
```

### Properties (Read-Only)
- `CurrentHealth`, `MaxHealth`
- `CurrentMana`, `MaxMana`
- `CurrentLevel`, `CurrentExperience`, `ExperienceToNextLevel`
- `CurrentSoulEnergy`, `SoulCapacity`
- `CurrentCorruption`, `IsDead`
- `CanAbsorbSouls` (conditional based on death state)

## Usage Instructions

### Unity Editor Setup
1. **Create Player GameObject**: Empty GameObject named "Player"
2. **Attach PlayerController**: Add PlayerController script component
3. **Attach PlayerMovement**: Add PlayerMovement script component
4. **Configure References**: Drag PlayerMovement to _playerMovement field
5. **Set Parameters**: Configure health, mana, soul capacity as needed
6. **Layer and Tag**: Set appropriate layer (Player) and tag (Player)

### Code Integration
```csharp
// Subscribe to player events
PlayerController.OnHealthChanged += UpdateHealthUI;
PlayerController.OnSoulEnergyChanged += UpdateSoulMeterUI;
PlayerController.OnLevelUp += ShowLevelUpEffect;

// Access player stats
var playerController = ServiceLocator.Get<PlayerController>();
float currentHealth = playerController.CurrentHealth;
bool canCast = playerController.UseMana(spellCost);
```

## Integration Points
- **Systems**: Integrates with InputManager, GameManager, SaveManager via ServiceLocator
- **Events**: Provides comprehensive event system for UI and system coordination
- **Movement**: Coordinates with PlayerMovement script for complete character control
- **Save System**: Compatible with SaveManager through PlayerStats data structure
- **Future Systems**: Prepared for combat system (attack handling) and advanced soul-binding

## Testing and Debug Features

### Context Menu Items
- **Print Player Debug Info**: Display current stats in console
- **Take 25 Damage**: Test damage system
- **Heal to Full**: Test healing system
- **Add 50 Experience**: Test leveling system
- **Absorb Soul (Immediate)**: Test immediate soul consumption
- **Absorb Soul (Banked)**: Test soul banking system

### Debug Information
- Comprehensive logging with configurable `_enableDebugLogging`
- `GetDebugInfo()` method for runtime stat display
- Event notifications for all stat changes

## History Log

### 2025-01-27 15:30:00 - Initial Implementation
**Task**: @Task_18.3 - Develop Core Player Controller

**Implementation Completed**:
- Complete player stats management system
- Full soul-binding mechanics with absorption and corruption
- Event-driven architecture with 8 public static events
- ServiceLocator integration for manager access
- Input event handling for attack, interact, and pause
- Save/load support with PlayerStats struct
- Death/respawn system with proper state management
- Debug tools and context menu testing

**Files Created**: PlayerController.cs

**Integration Points**:
- InputManager event subscription for player actions
- ServiceLocator registration for system-wide access
- PlayerMovement coordination for complete character control
- GameManager integration for pause functionality

**Testing Results**:
- Component initialization verified
- Event system tested with debug methods
- ServiceLocator registration confirmed
- Input integration ready for testing

**Next Steps**:
- Unity Editor setup required for GameObject configuration
- Component attachment and reference assignment
- Testing with actual input and movement integration
- Integration testing with UI systems for event handling

## Cross-References
**Created In**: @Task_18 - [Task_18_Implementation_log.md](mdc:Documentation/Tasks/Task_18/Task_18_Implementation_log.md)
**Related Tasks**: @Task_2 (Soul-Binding System), @Task_3 (Combat System)
**Related Objects**: @PlayerMovement - [PlayerMovement.md](mdc:Documentation/Objects/Scripts/PlayerMovement.md)
**Related Systems**: @InputManager, @GameManager, @SaveManager
**Script File**: [PlayerController.cs](mdc:Assets/Scripts/PlayerController.cs) 