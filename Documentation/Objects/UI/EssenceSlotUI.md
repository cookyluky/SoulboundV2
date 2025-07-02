# EssenceSlotUI

## Object Information
**Type**: Prefab
**Location**: Assets/Prefabs/UI/EssenceSlotUI_Prefab.prefab
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Individual slot component for displaying essence types and amounts in the essence inventory system. Provides interactive selection and visual feedback for essence management.

## Components
### Button (Unity Built-in)
- **Purpose**: Handle slot selection clicks
- **Key Properties**:
  - Interactable: Dynamic based on essence amount
  - Navigation: Automatic
  - Transition: Color Tint

### Image - Slot Background
- **Purpose**: Visual background for the slot
- **Key Properties**:
  - Color: Dynamic (Normal/Selected/Empty states)
  - Raycast Target: ✓ (enabled for button interaction)

### Image - Essence Icon
- **Purpose**: Visual representation of essence type
- **Key Properties**:
  - Color: Type-specific (Red/Orange/Blue/Purple)
  - Alpha: Dynamic based on essence availability
  - Raycast Target: ✗ (disabled)

### TextMeshPro - Essence Amount
- **Purpose**: Display current essence quantity
- **Key Properties**:
  - Text: Dynamic amount value
  - Font Size: 14
  - Color: White
  - Alignment: Center

### TextMeshPro - Essence Type
- **Purpose**: Display essence type name
- **Key Properties**:
  - Text: Vitality/Strength/Arcane/Forbidden
  - Font Size: 12
  - Color: White
  - Alignment: Center

### Image - Selection Highlight
- **Purpose**: Visual highlight for selected slot
- **Key Properties**:
  - Color: Yellow with transparency
  - Active: Dynamic based on selection state
  - Raycast Target: ✗ (disabled)

### EssenceSlotUI Script
- **Script**: EssenceSlotUI.cs
- **Purpose**: Manage slot state and user interaction
- **Key Properties**:
  - Slot Button: Reference to Button component
  - Essence Icon: Reference to icon Image
  - Slot Background: Reference to background Image
  - Essence Amount Text: Reference to amount TextMeshPro
  - Essence Type Text: Reference to type TextMeshPro
  - Selection Highlight: Reference to highlight GameObject

## Hierarchy Structure
```
EssenceSlotUI_Prefab
├── SlotBackground (Image)
├── EssenceIcon (Image)
├── SelectionHighlight (Image)
├── EssenceAmountText (TextMeshPro)
└── EssenceTypeText (TextMeshPro)
```

## Dependencies
- **Script Dependencies**: EssenceSlotUI.cs, TextMeshPro
- **Asset Dependencies**: UI sprites for backgrounds and icons
- **System Integration**: EssenceInventoryUI system

## Integration Points
- **Systems**: EssenceInventoryUI creates and manages multiple instances
- **Events**: Slot selection callbacks to parent inventory
- **Interfaces**: ISelectableUI pattern for slot management

## Usage Instructions
**For Developers**: This prefab is instantiated dynamically by EssenceInventoryUI. Do not place directly in scenes.
**For Designers**: Modify visual properties through the prefab to affect all instances.

## History Log
### 2025-01-27 17:00:00 - Initial Creation
Created EssenceSlotUI prefab with complete component hierarchy and script integration.

**Files Modified**: 
- Assets/Prefabs/UI/EssenceSlotUI_Prefab.prefab (created)
- EssenceSlotUI.cs script integration

**Unity Steps**: Complete prefab creation with proper component setup and script assignment

**Integration Impact**: Enables dynamic essence slot creation in EssenceInventoryUI system

## Cross-References
**Created For**: @Task_2.2 - Essence Storage System Implementation
**Used By**: @EssenceInventoryUI - Main inventory management system
**Script Documentation**: [EssenceSlotUI.cs](mdc:Documentation/Objects/Scripts/EssenceSlotUI.md)
**Related Systems**: @EssenceManager, @PlayerController 