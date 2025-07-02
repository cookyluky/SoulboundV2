# LayerTagValidator

## Object Information
**Type**: Editor Script
**Location**: Assets/Scripts/Editor/LayerTagValidator.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Purpose
Unity Editor utility script that validates and ensures proper configuration of project layers, tags, and physics settings for the SoulBound RPG project. Provides automated validation and user-friendly setup instructions to maintain consistency across the development team.

## Components

### LayerTagValidator Class
- **Script**: LayerTagValidator.cs
- **Purpose**: EditorWindow-based validation tool with GUI interface
- **Key Properties**:
  - RequiredLayers: Dictionary mapping layer indices to expected names
  - RequiredTags: Array of required tag names
  - scrollPosition: Vector2 for GUI scroll view
  - showValidation: bool for collapsible validation results
  - showInstructions: bool for collapsible setup instructions

## Hierarchy Structure
```
LayerTagValidator (EditorWindow)
├── Validation Results Section
│   ├── Layer Validation Display
│   └── Tag Validation Display
├── Setup Instructions Section
│   ├── Configuration Steps
│   ├── Open Tags/Layers Button
│   └── Open Physics Button
└── Refresh Validation Button
```

## Dependencies
- **Unity Engine**: EditorWindow, GUILayout, EditorGUILayout
- **Unity Editor**: SettingsService for opening project settings
- **System Collections**: Dictionary, List for data management
- **Unity Internal**: InternalEditorUtility for tag validation

## Integration Points
- **Unity Menu System**: Accessible via "SoulBound" menu
- **Project Settings**: Direct navigation to Tags/Layers and Physics
- **Console Output**: Quick validation results logging
- **Task System**: Validates @Task_21 requirements

## Usage Instructions

### Opening the Validator
1. **Via Unity Menu**: Window → SoulBound → Validate Layers & Tags
2. **Quick Validation**: Window → SoulBound → Quick Validate (console only)

### Using the Interface
1. **Validation Results**: View current configuration status
2. **Setup Instructions**: Follow guided configuration steps
3. **Direct Navigation**: Use buttons to open relevant Unity settings
4. **Refresh**: Update validation after making changes

### Required Configuration
**Layers to Configure**:
- Layer 8: Player
- Layer 9: Enemy
- Layer 10: Environment
- Layer 11: UI

**Tags to Configure**:
- Player
- Enemy
- NPC
- Interactable
- Projectile

## Features

### Validation Capabilities
- **Layer Verification**: Checks if required layers exist at correct indices
- **Tag Verification**: Validates presence of all required tags
- **Real-time Status**: Live updates as configuration changes
- **Error Reporting**: Clear indication of missing or incorrect settings
- **Success Confirmation**: Visual confirmation when all settings are correct

### User Experience
- **Collapsible Sections**: Organized interface with foldout sections
- **Color-coded Results**: ✅ Green for correct, ❌ Red for errors
- **Guided Instructions**: Step-by-step setup guidance
- **Direct Navigation**: One-click access to Unity settings panels
- **Batch Validation**: Check all settings simultaneously

### Development Integration
- **Team Consistency**: Ensures all developers have identical settings
- **Onboarding Tool**: Helps new team members configure projects correctly
- **CI/CD Validation**: Can be called programmatically for build validation
- **Documentation Reference**: Links to comprehensive configuration docs

## Technical Details

### Validation Logic
```csharp
// Layer validation
foreach (var requiredLayer in RequiredLayers)
{
    string actualName = LayerMask.LayerToName(layerIndex);
    // Compare expected vs actual layer names
}

// Tag validation
foreach (string requiredTag in RequiredTags)
{
    // Check if tag exists in Unity's tag system
    var tags = UnityEditorInternal.InternalEditorUtility.tags;
    bool exists = tags.Contains(requiredTag);
}
```

### Menu Integration
```csharp
[MenuItem("SoulBound/Validate Layers & Tags")]
public static void ShowWindow()

[MenuItem("SoulBound/Quick Validate")]
public static void QuickValidate()
```

## History Log

### 2025-01-27 16:30:00 - Initial Creation
**Purpose**: Created comprehensive layer and tag validation tool for @Task_21

**Features Implemented**:
- Complete EditorWindow interface with validation display
- Real-time layer and tag validation
- Guided setup instructions with direct Unity settings navigation
- Console-based quick validation for scripting integration
- Color-coded status display with clear error messaging

**Files Created**:
- Assets/Scripts/Editor/LayerTagValidator.cs
- Documentation/Tech/LayersTagsPhysics.md (configuration reference)

**Unity Editor Integration**:
- Added SoulBound menu category
- Provided direct navigation to Project Settings
- Implemented real-time validation feedback

**Validation Coverage**:
- Layer Index Verification: Checks layers 8-11 for correct names
- Tag Existence Validation: Verifies all 5 required tags
- Built-in Layer Support: Validates Layer 2 (Ignore Raycast)
- Error Reporting: Clear messaging for configuration issues

**Integration Impact**:
- Enables consistent project setup across team members
- Provides validation tool for @Task_21 completion
- Supports automated validation in development workflow
- Links to comprehensive physics configuration documentation

**Testing Requirements**:
- Manual testing with missing layers/tags
- Validation of Unity settings navigation
- Console output verification for quick validation
- Interface usability testing across different Unity versions

## Cross-References
**Created For**: @Task_21 - Configure Project Layers, Tags, and Physics Settings
**Documentation**: [LayersTagsPhysics.md](mdc:Documentation/Tech/LayersTagsPhysics.md)
**Related Systems**: @Physics Configuration, @Project Setup Validation
**Required For**: @Player Controller, @Enemy AI, @Combat System, @Interaction System 