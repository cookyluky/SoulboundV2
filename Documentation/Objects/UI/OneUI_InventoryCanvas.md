# OneUI_InventoryCanvas

## Cross-References
**Created In**: @Task_26.1 - [Task_26_Implementation_log.md](mdc:Documentation/Tasks/Task_26/Task_26_Implementation_log.md)
**Related Objects**: @OneUI_InventoryManager, @EssenceManager, @InputManager
**Test Coverage**: @Test_UI_OneUI_Integration (planned)
**Script File**: [OneUI_InventoryManager.cs](mdc:Assets/Scripts/UI/OneUI_InventoryManager.cs)

## Object Information
**Type**: GameObject with Canvas
**Location**: Scene Hierarchy (to be created)
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Main canvas for the organic-themed pod-based inventory system using OneUI Kit framework. Provides responsive design across all screen resolutions with accessibility support and integrates with existing essence management system.

## Components
### Canvas
- **Script**: Unity built-in
- **Purpose**: Primary rendering canvas for inventory UI
- **Key Properties**:
  - Render Mode: Screen Space - Overlay
  - Sorting Order: 100 (above other UI elements)
  - Target Display: Display 1

### CanvasScaler
- **Script**: Unity built-in
- **Purpose**: Responsive design scaling per PRD specifications
- **Key Properties**:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: (1920, 1080) - PRD base resolution
  - Screen Match Mode: Match Width Or Height
  - Match: 0.5 - Balanced scaling
  - Reference Pixels Per Unit: 100

### GraphicRaycaster
- **Script**: Unity built-in
- **Purpose**: Enable UI interaction and event handling
- **Key Properties**:
  - Ignore Reversed Graphics: True
  - Blocking Objects: None

### OneUI_InventoryManager
- **Script**: OneUI_InventoryManager.cs
- **Purpose**: Coordinate pod-based inventory system with OneUI integration
- **Key Properties**:
  - Inventory Canvas: Self-reference
  - Canvas Scaler: Self-reference  
  - Hotkey Slot Count: 4 (per PRD)
  - Enable Debug Logging: True
  - Enable Keyboard Navigation: True
  - Enable Screen Reader Support: True

## Hierarchy Structure
```
OneUI_InventoryCanvas
├── PodContainer (RectTransform)
│   ├── [Future: CombatItemsPod]
│   ├── [Future: ConsumablesPod]  
│   ├── [Future: QuestItemsPod]
│   └── [Future: EssencePod]
├── [Future: InspectorPanel]
└── [Future: HotkeyBar]
```

## Dependencies
- **OneUI Kit**: DevsDaddy.Shared.UIFramework namespace
- **EssenceManager**: For essence item display and integration
- **InputManager**: For keyboard/controller navigation
- **EventSystem**: Required for UI interaction

## Integration Points
- **EssenceManager**: Display and manage essence items from existing system
- **InputManager**: Handle inventory toggle and navigation input
- **Canvas Hierarchy**: Separate from existing UI_Canvas to avoid conflicts
- **Accessibility**: WCAG 2.1 compliance for screen readers and keyboard navigation

## Usage Instructions
1. Create canvas using Unity Editor steps below
2. Attach OneUI_InventoryManager script
3. Configure canvas scaler settings for responsive design
4. Test canvas scaling across different resolutions
5. Verify integration with existing managers

## Unity Editor Setup Steps

### **Step 1: Create OneUI Inventory Canvas**
1. Right-click in Hierarchy → UI → Canvas → Name: "OneUI_InventoryCanvas"
2. Select OneUI_InventoryCanvas → Inspector:
   - Render Mode: Screen Space - Overlay  
   - Sorting Order: 100
   - Target Display: Display 1

### **Step 2: Configure Canvas Scaler (Auto-created)**
1. Select OneUI_InventoryCanvas → Canvas Scaler component:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: X=1920, Y=1080
   - Screen Match Mode: Match Width Or Height  
   - Match: 0.5
   - Reference Pixels Per Unit: 100

### **Step 3: Add OneUI_InventoryManager Script**
1. Select OneUI_InventoryCanvas → Add Component → Scripts → OneUI_InventoryManager
2. Configure OneUI_InventoryManager properties:
   - Inventory Canvas: Drag OneUI_InventoryCanvas (self-reference)
   - Canvas Scaler: Drag Canvas Scaler component (auto-references)
   - Hotkey Slot Count: 4
   - Enable Debug Logging: ✓ (checked)
   - Enable Keyboard Navigation: ✓ (checked)
   - Enable Screen Reader Support: ✓ (checked)

### **Step 4: Verify EventSystem (Required for UI)**
1. Check Hierarchy for "EventSystem" - should already exist from existing UI
2. If missing: Right-click Hierarchy → UI → Event System

### **Step 5: Test Canvas Setup**
1. Play the scene
2. Check Console for "OneUI Inventory Canvas initialized successfully"
3. Test different Game window resolutions to verify responsive scaling
4. Verify no conflicts with existing UI_Canvas

### **Verification Checklist**
- [ ] OneUI_InventoryCanvas created with correct settings
- [ ] Canvas Scaler configured for 1920×1080 base resolution
- [ ] OneUI_InventoryManager script attached and configured
- [ ] EventSystem exists in scene
- [ ] Console shows successful initialization message
- [ ] No errors in Console when playing scene
- [ ] Canvas scales properly when Game window is resized

## History Log
### 2025-01-27 15:30:00 - Initial Creation
**Task**: @Task_26.1 - Set up OneUI Kit integration

**Files Created**: 
- OneUI_InventoryManager.cs - Main inventory management script
- OneUI_InventoryCanvas.md - This documentation file

**Unity Steps**: Canvas setup instructions provided for Unity Editor

**Integration Impact**: 
- New inventory system operates independently of existing UI_Canvas
- Prepares foundation for pod-based inventory organization
- Establishes responsive design baseline for organic UI theme

**Next Steps**: 
- Implement pod container system (Task 26.2)
- Add OneUI component integration
- Create seed-shaped item representations 