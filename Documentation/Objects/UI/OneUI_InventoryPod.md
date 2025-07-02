# OneUI_InventoryPod

## Cross-References
**Created In**: @Task_26.2 - [Task_26_Implementation_log.md](mdc:Documentation/Tasks/Task_26/Task_26_Implementation_log.md)
**Related Objects**: @OneUI_InventoryManager, @OneUI_InventoryCanvas, @EssenceManager
**Test Coverage**: @Test_UI_Pod_Animations, @Test_UI_Pod_Categories (planned)
**Script File**: [OneUI_InventoryPod.cs](mdc:Assets/Scripts/UI/OneUI_InventoryPod.cs)

## Object Information
**Type**: Component Script + GameObject Hierarchy
**Location**: Assets/Scripts/UI/OneUI_InventoryPod.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Individual pod container component for the organic-themed inventory system. Manages a single category of items (Combat Items, Consumables, Quest, or Essence) with smooth open/close animations, 5x3 curved grid layout, and category-specific visual theming.

## Component Structure

### **OneUI_InventoryPod Script**
- **Pod Configuration**: Category assignment, title, accent colors
- **Visual Components**: Header, background, accent elements, toggle button
- **Content Management**: Grid container, canvas group for animations
- **Animation System**: Smooth open/close with custom curves
- **Item Management**: Add/remove items, capacity management (15 items max)

### **Required GameObject Hierarchy**
```
PodContainer (OneUI_InventoryPod script)
├── PodHeader
│   ├── PodTitleText (TextMeshProUGUI)
│   ├── PodToggleButton (Button)
│   └── PodAccentImage (Image)
├── PodContentContainer (CanvasGroup)
│   └── ItemGridContainer (GridLayoutGroup)
│       └── [Item slots created dynamically]
```

## Configuration Properties

### **Pod Categories (Enum)**
- **CombatItems**: Red tint (0.8, 0.2, 0.2, 1.0)
- **Consumables**: Green tint (0.2, 0.8, 0.2, 1.0)
- **Quest**: Yellow tint (0.8, 0.8, 0.2, 1.0)
- **Essence**: Blue tint (0.2, 0.6, 0.8, 1.0)

### **Animation Settings**
- **Open/Close Duration**: 0.5 seconds
- **Animation Curve**: EaseInOut for organic feel
- **Hover Scale**: 1.05x multiplier
- **Hover Duration**: 0.2 seconds

### **Grid Layout (5x3 per PRD)**
- **Columns**: 5 (fixed column count)
- **Max Items**: 15 total (5 rows × 3 columns)
- **Spacing**: 8x8 pixels between items
- **Alignment**: Middle Center
- **Auto-sizing**: Based on container width

## Integration Points

### **OneUI_InventoryManager Integration**
- **Pod Registration**: Automatic registration in manager's pod lookup
- **Event Handling**: OnPodOpened, OnPodClosed events
- **Color Configuration**: Manager sets category-specific colors
- **State Management**: Manager can open/close specific pods

### **EssenceManager Integration**
- **Essence Pod**: Special integration for essence items display
- **Dynamic Updates**: Pod updates when essence amounts change
- **Visual Feedback**: Glowing effects for available essence

## Animation System

### **Open Animation**
1. **Size Expansion**: Header-only → Full content size
2. **Alpha Fade**: Content fades in (0 → 1)
3. **Interactivity**: Canvas group becomes interactive
4. **Event Firing**: OnPodOpened event triggered

### **Close Animation**
1. **Alpha Fade**: Content fades out (1 → 0)
2. **Size Collapse**: Full content → Header-only size
3. **Deactivation**: Content container deactivated
4. **Event Firing**: OnPodClosed event triggered

## Public API

### **State Management**
- `TogglePod()`: Toggle open/closed state
- `SetPodState(bool open)`: Set specific state
- `IsOpen`: Current open state
- `IsAnimating`: Animation in progress

### **Item Management**
- `AddItemSlot(GameObject itemSlot)`: Add item to grid
- `RemoveItemSlot(GameObject itemSlot)`: Remove specific item
- `ClearAllItems()`: Remove all items
- `ItemCount`: Current item count
- `MaxItems`: Maximum capacity (15)

### **Configuration**
- `ConfigurePod(category, title, accentColor)`: Set pod properties
- `Category`: Get pod category
- Events: `OnPodOpened`, `OnPodClosed`, `OnItemSlotSelected`

## Unity Editor Setup Instructions

### **Step 1: Create Pod Prefab Structure**
1. **Create Empty GameObject**: Right-click Hierarchy → Create Empty → Name: "InventoryPod"
2. **Add RectTransform**: Component should auto-add for UI objects
3. **Add Background Image**: Add Component → UI → Image
   - Color: (0.15, 0.15, 0.15, 0.9) - Semi-transparent dark charcoal
4. **Add OneUI_InventoryPod Script**: Add Component → Scripts → OneUI_InventoryPod

### **Step 2: Create Pod Header**
1. **Create Header Container**: Right-click InventoryPod → UI → Empty → Name: "PodHeader"
2. **Configure Header RectTransform**:
   - Anchor: Top stretch (min: 0,1 max: 1,1)
   - Height: 60 pixels
   - Offset Min: (0, -60), Offset Max: (0, 0)

3. **Add Pod Title Text**: Right-click PodHeader → UI → Text - TextMeshPro → Name: "PodTitleText"
   - Text: "Pod Title"
   - Font: Liberation Sans SDF
   - Color: White
   - Alignment: Middle Left
   - Anchor: Stretch (0,0 to 1,1)

4. **Add Toggle Button**: Right-click PodHeader → UI → Button - TextMeshPro → Name: "PodToggleButton"
   - Button Text: "▼" (or custom expand/collapse icon)
   - Anchor: Right middle
   - Width: 40, Height: 40
   - Position: Right edge of header

5. **Add Accent Image**: Right-click PodHeader → UI → Image → Name: "PodAccentImage"
   - Width: 4 pixels (vertical accent bar)
   - Height: Full header height
   - Anchor: Left middle
   - Color: Will be set by script per category

### **Step 3: Create Content Container**
1. **Create Content Container**: Right-click InventoryPod → UI → Empty → Name: "PodContentContainer"
2. **Add CanvasGroup**: Add Component → Canvas Group
   - Alpha: 1.0
   - Interactable: ✓
   - Blocks Raycasts: ✓

3. **Configure Content RectTransform**:
   - Anchor: Fill except header (min: 0,0 max: 1,1)
   - Offset Min: (0, 0), Offset Max: (0, -60)

### **Step 4: Create Item Grid Container**
1. **Create Grid Container**: Right-click PodContentContainer → UI → Empty → Name: "ItemGridContainer"
2. **Add Grid Layout Group**: Add Component → Layout → Grid Layout Group
   - Cell Size: Will be calculated by script
   - Spacing: (8, 8)
   - Start Corner: Upper Left
   - Start Axis: Horizontal
   - Child Alignment: Middle Center
   - Constraint: Fixed Column Count = 5

3. **Configure Grid RectTransform**:
   - Anchor: Fill (0,0 to 1,1)
   - Add padding: Left=10, Right=10, Top=10, Bottom=10

### **Step 5: Configure OneUI_InventoryPod Script**
1. **Select InventoryPod GameObject** → Inspector → OneUI_InventoryPod component
2. **Assign References**:
   - Pod Header: Drag PodHeader GameObject
   - Pod Title Text: Drag PodTitleText GameObject
   - Pod Background: Drag InventoryPod Image component
   - Pod Accent Image: Drag PodAccentImage GameObject
   - Pod Toggle Button: Drag PodToggleButton GameObject
   - Pod Content Container: Drag PodContentContainer GameObject
   - Item Grid Container: Drag ItemGridContainer GameObject
   - Item Grid Layout: Drag GridLayoutGroup component
   - Content Canvas Group: Drag CanvasGroup component

3. **Configure Settings**:
   - Pod Category: (Set per pod instance)
   - Pod Title: (Set per pod instance)
   - Pod Accent Color: (Set per pod instance)
   - Start Open: ☐ (unchecked)
   - Max Items Per Pod: 15

### **Step 6: Create Pod Prefab**
1. **Create Prefab**: Drag InventoryPod from Hierarchy to Assets/Prefabs/UI/
2. **Name Prefab**: "OneUI_InventoryPod_Prefab"
3. **Create Category Variants**: Duplicate prefab for each category:
   - OneUI_CombatItemsPod_Prefab
   - OneUI_ConsumablesPod_Prefab
   - OneUI_QuestPod_Prefab
   - OneUI_EssencePod_Prefab

### **Step 7: Setup in OneUI_InventoryCanvas**
1. **Add to PodContainer**: Drag pod prefabs into OneUI_InventoryCanvas/PodContainer
2. **Assign to Manager**: Select OneUI_InventoryCanvas → OneUI_InventoryManager
   - Combat Items Pod: Drag CombatItemsPod GameObject
   - Consumables Pod: Drag ConsumablesPod GameObject
   - Quest Pod: Drag QuestPod GameObject
   - Essence Pod: Drag EssencePod GameObject

### **Step 8: Test Pod System**
1. **Play Scene**
2. **Check Console**: Should see "Pod-based container system initialized with 4 categories"
3. **Test Animations**: Click pod toggle buttons to test open/close
4. **Verify Colors**: Each pod should display with its category color tint
5. **Check Layout**: Grid should be properly sized for 5x3 item layout

## Testing Checklist

### **Visual Verification**
- [ ] Pod headers display correct titles
- [ ] Category colors applied correctly
- [ ] Smooth open/close animations
- [ ] Grid layout properly sized for 5x3

### **Functionality Testing**
- [ ] Pod toggle buttons work
- [ ] Only one animation at a time
- [ ] Content properly hidden when closed
- [ ] Grid container scales with content

### **Integration Testing**
- [ ] Manager can control individual pods
- [ ] Events fire correctly (check console logs)
- [ ] Pod lookup dictionary works
- [ ] No conflicts with existing UI

### **Performance Testing**
- [ ] Smooth 60fps during animations
- [ ] No memory leaks from pod operations
- [ ] Efficient grid layout updates

## History Log

### 2025-01-27 02:10:00 - Initial Pod System Implementation
**Task**: @Task_26.2 - Pod-Based Container System

**Files Created**:
- OneUI_InventoryPod.cs - Pod component script
- OneUI_InventoryPod.md - This documentation

**Integration**:
- Updated OneUI_InventoryManager for pod integration
- Added PodCategory enum for organization
- Implemented pod event system and lookup

**Features Implemented**:
- ✅ 4-category pod organization (Combat, Consumables, Quest, Essence)
- ✅ Smooth open/close animations with custom curves
- ✅ Category-specific color theming
- ✅ 5x3 grid layout system per PRD
- ✅ Item capacity management (15 items max)
- ✅ Event-driven architecture for integration
- ✅ Auto-component detection and validation
- ✅ Organic visual theming (dark charcoal + category tints)

**Unity Editor Requirements**:
- Complete pod prefab creation workflow
- Manager integration setup
- Animation testing procedures
- Performance validation steps 