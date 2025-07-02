# Soul-Binding System Design

## Overview
The Soul-Binding System is a core gameplay mechanic that allows players to collect souls from defeated enemies and bind them using equipped Soul-Binding Gems. This system is distinct from the essence absorption mechanics and focuses on long-term character progression through gem-based soul collection and skill tree activation.

## System Architecture

### Core Components

#### 1. Soul-Binding Gems
- **Purpose**: Equipable items that activate specific soul-trees
- **Properties**: GemType (Wraith, Hollow, Arcanum), Tier (Common → Legendary), Associated Skill Tree
- **Functionality**: Each gem provides access to its skill tree when equipped
- **Storage**: Can store collected souls as XP for skill progression

#### 2. Soul Collection System
- **Soul Types**: Wraith, Hollow, Arcanum (and potentially others)
- **Soul Tiers**: Common, Uncommon, Rare, Epic, Legendary
- **Drop Mechanics**: Enemies drop souls based on their archetype and difficulty
- **Collection**: Players can collect souls through proximity or manual interaction

#### 3. Gem Socket System
- **Initial State**: Players start with zero gem sockets
- **Progression**: Unlock additional sockets through gameplay progression
- **Restrictions**: Gems can only be swapped at hub locations or save points
- **Activation**: Only equipped gems activate their associated soul-trees

#### 4. Soul Management
- **Banking**: Souls can be stored for later use in skill tree progression
- **Immediate Use**: Souls can be consumed for immediate buffs and benefits
- **Decision UI**: Interface allows players to choose between banking and immediate consumption

## Data Structures

### SoulBindingGem Class
```csharp
public class SoulBindingGem : MonoBehaviour
{
    public enum GemType { Wraith, Hollow, Arcanum }
    public enum Tier { Common, Uncommon, Rare, Epic, Legendary }
    
    [SerializeField] private GemType gemType;
    [SerializeField] private Tier gemTier;
    [SerializeField] private SkillTree associatedSkillTree;
    
    private Dictionary<SoulType, int> storedSouls;
    
    // Methods for soul storage and consumption
}
```

### PlayerGemInventory Class
```csharp
public class PlayerGemInventory : MonoBehaviour
{
    [SerializeField] private List<GemSocket> gemSlots;
    [SerializeField] private Dictionary<int, SoulBindingGem> equippedGems;
    
    // Methods for equipping/unequipping gems
    // Socket management functionality
}
```

### SoulManager Singleton
```csharp
public class SoulManager : MonoBehaviour
{
    private Dictionary<SoulType, Dictionary<SoulTier, int>> collectedSouls;
    
    // Methods for tracking souls, transfers to gems
    // Integration with soul drop system
}
```

## System Interactions

### Integration Points
1. **Task 2 (Essence Absorption)**: Separate but complementary resource system
2. **Task 4 (Skill Progression)**: Direct dependency for skill tree activation
3. **Task 7 (Enemy Archetypes)**: Enemies must drop appropriate soul types
4. **Task 12 (Save/Load)**: Persistence of gem and soul data
5. **Task 26 (Inventory UI)**: UI framework for gem management

### Workflow
1. **Enemy Defeat**: Enemy dies and drops soul based on archetype and tier
2. **Soul Collection**: Player collects soul through proximity or manual interaction
3. **Soul Decision**: Player chooses between immediate consumption or banking
4. **Gem Management**: Player equips gems at hub/save points to activate skill trees
5. **Skill Progression**: Banked souls are used as XP for skill tree advancement

## UI Design Requirements

### Gem Socket Display
- Visual representation of available gem slots
- Clear indication of equipped vs. empty slots
- Gem type and tier visualization

### Soul Inventory
- Categorized display of collected souls by type and tier
- Quantity indicators and rarity visual effects
- Drag-and-drop functionality for gem interaction

### Decision Interface
- Clear choice between immediate consumption and banking
- Preview of immediate benefits vs. skill tree XP value
- Confirmation dialogs for important decisions

### Skill Tree Activation
- Visual indicators showing which skill trees are currently active
- Clear connection between equipped gems and available skills
- Prerequisite indicators for cross-tree dependencies

## Performance Considerations

### Optimization Strategies
1. **Object Pooling**: Use pooling for soul drop instances to reduce memory allocation
2. **Efficient Storage**: Implement optimized data structures for soul tracking
3. **UI Virtualization**: Use virtual scrolling for large soul inventories
4. **Event-Driven Updates**: Minimize UI updates through event-based architecture

### Memory Management
- Proper disposal of soul drop objects
- Efficient serialization for save/load operations
- Cache management for gem and soul data

## Audio-Visual Feedback

### Visual Effects
- Particle effects for soul collection events
- Glow effects for gem rarity tiers
- Animation feedback for gem equipping/unequipping
- Skill tree activation visual indicators

### Audio Design
- Unique sound effects for different soul types
- Audio feedback for gem socket interactions
- Confirmation sounds for banking vs. immediate use decisions
- Ambient audio for skill tree activation

## Accessibility Features

### Visual Accessibility
- High-contrast mode for gem and soul visualization
- Color-blind friendly indicators using shapes and patterns
- Scalable UI elements for different screen sizes

### Input Accessibility
- Full keyboard/controller navigation support
- Screen reader compatibility for all UI elements
- Customizable input bindings for gem management

## Testing Strategy

### Unit Testing
- SoulBindingGem class functionality
- PlayerGemInventory management operations
- SoulManager soul tracking and transfer logic

### Integration Testing
- Cross-system interactions with save/load
- UI framework integration
- Skill tree activation mechanics

### Gameplay Testing
- Soul drop rates and collection feel
- Gem equipping and socket progression
- Banking vs. immediate use decision balance

## Cross-References
**Implemented In**: @Task_27 - [Task_27_Overview.md](mdc:Documentation/Tasks/Task_27/Task_27_Overview.md)
**Related Systems**: @Task_2, @Task_4, @Task_7, @Task_12, @Task_26
**Object Documentation**: @SoulBindingGem, @PlayerGemInventory, @SoulManager
**UI Components**: @GemSocketUI, @SoulInventoryUI, @SkillTreeActivationUI 