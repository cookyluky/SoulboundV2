# OneUI_PodContainer

## Object Information
**Type**: UI Component Prefab
**Location**: Assets/Scripts/UI/OneUI_PodContainer.cs (planned)
**Created**: 2025-01-27 (planned)
**Last Modified**: 2025-01-27 (planned)

## Cross-References
**Created For**: @Task_26 - [Task_26_Implementation_log.md](mdc:Documentation/Tasks/Task_26/Task_26_Implementation_log.md)
**Parent System**: @OneUI_InventorySystem - [OneUI_EssenceInventory.md](mdc:Documentation/Objects/UI/OneUI_EssenceInventory.md)
**Related Objects**: @SeedItemDisplay, @InspectorPanel, @HotkeyBar
**Framework**: OneUI Kit by DevsDaddy
**Test Coverage**: @Test_PodContainer_Animation (planned)
**Subtask**: Task 26.2 - Pod-based container system

## Purpose
Reusable pod container component that serves as the main organizational unit for the inventory system. Each pod represents a category (Combat Items, Consumables, Quest, Essence) and can expand to show its contents in a curved grid layout.

## Visual Design Specifications

### Organic Theme
- **Shape**: Seed pod silhouette with vine/petal motifs
- **Border Style**: Softly curved organic borders inspired by Hollow Knight
- **Background**: Semi-opaque dark charcoal with blue-green tint
- **Expansion Effect**: Petal-like unfolding animation when opening

### Color Coding by Category
- **Combat Items**: Warm ochre accents
- **Consumables**: Soft teal accents  
- **Quest Items**: Muted lavender accents
- **Essence**: Pulsing cyan glow accents

### Animation Specifications
- **Open/Close Duration**: 0.3 seconds
- **Easing**: Ease-in-out for smooth transitions
- **Scale Effect**: Grows and "splits" open like petals
- **Highlight State**: Warm gold outline for selection

## Components Structure

### Unity Hierarchy
```
OneUI_PodContainer
├── PodBackground (OneUI Card Component)
│   ├── CategoryIcon (Image)
│   ├── CategoryLabel (TextMeshPro)
│   └── ItemCount (TextMeshPro)
├── ExpandButton (OneUI Button Component)
├── ContentContainer (Transform)
│   └── CurvedGridLayout (Custom Layout Group)
│       └── [SeedItemDisplay instances]
└── SelectionHighlight (OneUI Effect)
```

### Script Components
- **OneUI_PodContainer.cs**: Main container logic and animation control
- **CurvedGridLayoutGroup.cs**: Custom layout for 5×3 curved item arrangement
- **PodAnimationController.cs**: Smooth expand/collapse animations

## Technical Specifications

### Component Properties
```csharp
[Header("Pod Configuration")]
public PodCategory Category;              // Combat, Consumables, Quest, Essence
public int MaxItemsPerPage = 15;         // 5×3 grid layout
public float AnimationDuration = 0.3f;   // Open/close timing

[Header("OneUI Components")]
public OneUI.Card PodBackground;         // OneUI card component
public OneUI.Button ExpandButton;        // OneUI button for interaction
public OneUI.Effect SelectionHighlight; // OneUI highlight effect

[Header("Content Management")]
public Transform ContentContainer;        // Container for grid items
public CurvedGridLayoutGroup GridLayout; // Custom curved layout
public GameObject SeedItemPrefab;       // Template for items
```

### Animation States
- **Collapsed**: Default closed state showing only pod icon and item count
- **Expanding**: 0.3s transition with scale and fade effects
- **Expanded**: Full display with curved grid layout visible
- **Collapsing**: 0.3s transition back to collapsed state

### Performance Features
- **UI Pooling**: Efficient item instantiation for large inventories
- **Lazy Loading**: Content loaded only when pod is expanded
- **Memory Management**: Automatic cleanup of unused visual elements

## Integration Points

### Data Binding
- **EssenceManager**: Direct connection for Essence pod content
- **InventoryManager**: General item data for other pod categories
- **SaveManager**: Persistence of expansion state and item organization

### Event System
```csharp
// Pod-level events
public static event Action<PodCategory> OnPodExpanded;
public static event Action<PodCategory> OnPodCollapsed;
public static event Action<PodCategory, int> OnItemCountChanged;

// Item interaction events
public static event Action<ItemData> OnItemSelected;
public static event Action<ItemData> OnItemHover;
```

### Input Handling
- **Mouse**: Click to expand/collapse, hover for preview
- **Keyboard**: Arrow keys for navigation, Enter to expand
- **Controller**: D-pad navigation, A button to expand

## Accessibility Features

### Visual Accessibility
- **High-Contrast Mode**: Enhanced borders and stronger color contrasts
- **Scale Support**: UI scales up to 130% without clipping
- **Color-Blind Support**: Icon overlays supplement color coding

### Input Accessibility
- **Keyboard Navigation**: Full functionality without mouse
- **Screen Reader**: ARIA labels and role descriptions
- **Controller Support**: Console-style navigation patterns

## Usage Instructions

### Developer Setup
1. **Import OneUI Kit**: Ensure framework is properly imported
2. **Create Pod Instance**: Instantiate PodContainer prefab
3. **Configure Category**: Set pod category and accent colors
4. **Bind Data Source**: Connect to appropriate data manager
5. **Test Animations**: Verify smooth expand/collapse behavior

### Runtime Behavior
1. **Initial State**: Pod appears collapsed with category icon
2. **User Interaction**: Click or controller input expands pod
3. **Content Display**: Items appear in curved grid layout
4. **Navigation**: Users can navigate between items within pod
5. **Collapse**: Clicking pod header or other pod collapses current

## Testing Requirements

### Unit Testing
- [ ] Pod expand/collapse animations work smoothly
- [ ] Curved grid layout positions items correctly
- [ ] Item count updates accurately when content changes
- [ ] Color themes apply correctly for each category

### Integration Testing
- [ ] Data binding with EssenceManager functions properly
- [ ] Event system communicates with other UI components
- [ ] Save/load maintains pod expansion state
- [ ] Performance remains stable with maximum item count

### Accessibility Testing
- [ ] Keyboard navigation works for all functions
- [ ] Screen reader announces pod state changes
- [ ] High-contrast mode provides clear visibility
- [ ] Controller input responds correctly

## Performance Targets

### Animation Performance
- **Target FPS**: 60fps during all animations
- **Memory Allocation**: < 1KB per expand/collapse cycle
- **Response Time**: < 100ms for user input acknowledgment

### Content Performance  
- **Large Inventories**: Smooth performance with 100+ items per pod
- **UI Pooling**: 50% reduction in instantiation overhead
- **Memory Usage**: Stable memory footprint during extended use

## Dependencies

### Framework Dependencies
- **OneUI Kit**: Core UI component framework
- **Unity UI**: Base Unity UI system
- **TextMeshPro**: Text rendering and localization

### Project Dependencies
- **@EssenceManager**: Data source for essence pod content
- **@SaveManager**: Persistence for UI state
- **@InputManager**: Cross-platform input handling
- **@AudioManager**: Sound effects for interactions

## Implementation History

### Planned Development (Task 26.2)
- **Foundation**: Basic pod structure with OneUI Kit integration
- **Animation System**: Smooth expand/collapse with organic theme
- **Grid Layout**: Custom curved arrangement for items
- **Category Support**: Four main inventory categories

### Future Enhancements
- **Dynamic Categories**: Runtime category addition/removal
- **Advanced Animations**: More sophisticated organic transitions
- **Customization**: User-configurable pod colors and layouts
- **Mobile Optimization**: Touch-specific interaction improvements

---

*This component serves as the foundational element of the organic inventory system, providing intuitive organization and smooth user interaction while maintaining the established organic aesthetic theme.* 