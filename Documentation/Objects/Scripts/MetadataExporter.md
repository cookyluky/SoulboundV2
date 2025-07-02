# MetadataExporter

## Object Information
**Type**: Unity Editor Script
**Location**: Assets/Scripts/Editor/MetadataExporter.cs
**Created**: 2025-01-27
**Last Modified**: 2025-01-27

## Cross-References
**Implements Rule**: @UnityMetadataExport - [UnityMetadataExport.mdc](mdc:.cursor/rules/UnityMetadataExport.mdc)
**Referenced by**: @UnityContextAwareness - [UnityContextAwareness.mdc](mdc:.cursor/rules/UnityContextAwareness.mdc)
**Output Location**: Documentation/UnityExports/
**Related Systems**: @DocumentationSystem, @UnityEditorWorkflow

## Purpose
Unity Editor script that exports comprehensive metadata from scenes and prefabs to structured markdown files. Enables AI agents (like Claude) to understand Unity project structure, GameObject hierarchies, component configurations, and asset dependencies for context-aware code generation.

## Core Functionality

### Export Capabilities
- **Scene Metadata Export**: Complete scene hierarchy with all GameObjects, components, and properties
- **Prefab Metadata Export**: Full prefab structure including nested objects and dependencies
- **Batch Export**: Export all scenes and prefabs in one operation
- **Auto-Export**: Optional automatic export on scene save or hierarchy changes

### Export Content
- **GameObject Hierarchy**: Complete parent-child relationships with proper indentation
- **Component Details**: All attached components with script references and property values
- **Transform Data**: Position, rotation, and scale information
- **Asset Dependencies**: Materials, textures, scripts, audio clips, and other referenced assets
- **Metadata Properties**: Layers, tags, active states, and Unity version information

## Menu Integration

### Tools Menu Items
- **Tools → Export → Scene Metadata**: Export current active scene
- **Tools → Export → Prefab Metadata**: Export all prefabs in Assets/Prefabs/
- **Tools → Export → Export All Metadata**: Batch export scenes and prefabs
- **Tools → Export → Metadata Exporter Window**: Open configuration window

### Editor Window Features
- **Manual Export Controls**: Buttons for all export operations
- **Auto-Export Configuration**: Toggle automatic export on save/hierarchy changes
- **Directory Management**: Create and verify export directory structure
- **Status Display**: Show export paths and current settings

## File Output Structure

### Export Locations
```
Documentation/UnityExports/
├── Scenes/
│   ├── Scene_[SceneName]_Metadata.md
│   └── Scene_[SceneName]_LastExported.txt
└── Prefabs/
    ├── Prefab_[PrefabName]_Metadata.md
    └── Prefab_[PrefabName]_LastExported.txt
```

### Metadata File Format
```markdown
# [ObjectName] Metadata

## Export Information
**Type**: Scene/Prefab
**Unity Path**: [Path to asset]
**Last Exported**: [ISO 8601 timestamp]
**Unity Version**: [Unity version]
**Export Script**: MetadataExporter v1.0.0

## Hierarchy Structure
[Complete GameObject hierarchy with components]

## Asset Dependencies
[Categorized list of referenced assets]
```

## Component Analysis

### Main Classes
- **MetadataExporter**: Primary editor window and export functionality
- **MetadataExporterAutoInit**: Auto-initialization for callback setup

### Key Methods
- **ExportCurrentSceneMetadata()**: Export active scene to markdown
- **ExportAllPrefabMetadata()**: Export all prefabs in Assets/Prefabs/
- **ExportAllMetadata()**: Batch export all scenes and prefabs
- **ExportGameObjectHierarchy()**: Recursive hierarchy export with indentation
- **ExportComponentInfo()**: Component details and property extraction
- **CollectDependencies()**: Asset dependency analysis and categorization

### Data Collection
- **SerializedProperty Analysis**: Extract public and serialized field values
- **Component Inspection**: Analyze all attached components and their properties
- **Asset Reference Tracking**: Collect materials, textures, scripts, and audio dependencies
- **Hierarchy Traversal**: Recursive child object processing

## Integration Points

### Unity Editor Integration
- **EditorWindow**: Custom window for export control and configuration
- **MenuItem**: Menu integration for quick access to export functions
- **EditorCallbacks**: Scene save and hierarchy change event handlers
- **AssetDatabase**: Asset path resolution and GUID handling

### Documentation System Integration
- **Cross-Reference Support**: Links to related documentation files
- **Standardized Format**: Consistent markdown structure for AI consumption
- **Timestamp Tracking**: Export time tracking for currency validation
- **Directory Structure**: Organized export location matching project standards

## Usage Instructions

### Manual Export Workflow
1. **Open Unity Editor** with target scene loaded
2. **Navigate to Tools → Export → Scene Metadata** for current scene
3. **Use Tools → Export → Prefab Metadata** for all prefabs
4. **Check Documentation/UnityExports/** for generated files
5. **Verify export success** in Unity Console

### Auto-Export Configuration
1. **Open Tools → Export → Metadata Exporter Window**
2. **Enable "Auto-export on Scene Save"** for automatic scene exports
3. **Apply Auto-Export Settings** to activate callbacks
4. **Save scenes normally** - metadata exports automatically
5. **Monitor Console** for auto-export notifications

### Batch Export Process
1. **Use Tools → Export → Export All Metadata** for complete export
2. **Wait for completion** (may take several seconds)
3. **Review Console logs** for export status and any errors
4. **Verify all expected files** are created in UnityExports directories

## Quality Standards

### Export Completeness
- **All GameObjects**: Every object in hierarchy included, even inactive ones
- **Component Coverage**: All components with their key properties exported
- **Asset References**: Complete dependency tracking for all referenced assets
- **Transform Accuracy**: Precise position, rotation, and scale data

### File Format Standards
- **Consistent Structure**: All exports follow identical markdown format
- **Readable Hierarchy**: Proper indentation and clear parent-child relationships
- **Accurate Metadata**: Correct timestamps, paths, and Unity version information
- **Valid References**: All asset paths verified and accessible

## Error Handling

### Common Issues
- **Missing Directories**: Automatically creates export directories if needed
- **Invalid Assets**: Graceful handling of missing or corrupted prefabs/scenes
- **Permission Errors**: Clear error messages for write access issues
- **Large Hierarchies**: Efficient processing for complex scenes with many objects

### Validation Features
- **Asset Existence**: Verify assets exist before attempting export
- **Path Validation**: Ensure export directories are accessible
- **Component Safety**: Handle null components and missing script references
- **Memory Management**: Efficient processing for large scenes and prefabs

## Performance Considerations

### Optimization Features
- **Selective Export**: Target specific assets rather than always exporting everything
- **Incremental Processing**: Export only changed assets when possible
- **Memory Efficiency**: Process large hierarchies without excessive memory usage
- **Background Processing**: Non-blocking export operations

### Scalability
- **Large Projects**: Handles projects with hundreds of prefabs and scenes
- **Complex Hierarchies**: Efficiently processes deeply nested GameObject structures
- **Batch Operations**: Optimized for bulk export operations
- **Resource Management**: Proper cleanup of temporary objects and references

## Maintenance Requirements

### Regular Tasks
- **Export Validation**: Verify exports reflect current Unity state
- **Directory Cleanup**: Remove exports for deleted assets
- **Format Updates**: Update export format as project needs evolve
- **Performance Monitoring**: Track export times and optimize as needed

### Integration Updates
- **Unity Version Compatibility**: Update for new Unity versions
- **Rule Compliance**: Ensure exports match current documentation rules
- **Cross-Reference Accuracy**: Maintain valid links to related documentation
- **Feature Enhancement**: Add new export capabilities as requirements emerge

## History Log

### 2025-01-27 15:30:00 - Initial Implementation
**Files Created**: Assets/Scripts/Editor/MetadataExporter.cs
**Unity Integration**: Menu items and editor window implementation
**Export Functionality**: Scene and prefab metadata export with comprehensive hierarchy analysis
**Documentation System**: Integration with Documentation/UnityExports/ structure

**Features Implemented**:
- Complete GameObject hierarchy export with proper indentation
- Component analysis including serialized field extraction
- Asset dependency tracking for materials, textures, scripts, and audio
- Batch export capabilities for all scenes and prefabs
- Auto-export configuration for scene save and hierarchy change events
- Structured markdown output following project documentation standards

**Integration Points**:
- Menu integration: Tools → Export → [Various Options]
- Editor window: Tools → Export → Metadata Exporter Window
- Auto-callbacks: Scene save and hierarchy change event handling
- Directory management: Automatic creation of export directory structure

**Testing Requirements**: @InteractiveTestDocumentation applies - test file needed for validation 

### 2025-01-27 16:45:00 - Component Markdown Linking Enhancement
**Files Modified**: Assets/Scripts/Editor/MetadataExporter.cs
**Enhancement Type**: AI Context Improvement
**Bug Fix**: CS8120 compilation error resolved

**Features Implemented**:
- **Script File Linking**: MonoBehaviour components now link to their source files
  - Format: `[ScriptName.cs](../../Scripts/Path/To/ScriptName.cs)`
  - Uses `MonoScript.FromMonoBehaviour()` and `AssetDatabase.GetAssetPath()`
  - Relative path conversion from Unity assets to documentation structure
- **Unity Documentation Linking**: Built-in Unity components link to official documentation
  - Format: `[Rigidbody](https://docs.unity3d.com/ScriptReference/Rigidbody.html)`
  - Automatic detection via assembly analysis in `IsBuiltInUnityComponent()`
- **Fallback Formatting**: Unknown components use monospace formatting (`ComponentName`)

**Code Architecture Enhancements**:
- **GetComponentMarkdownLink()**: New method generating appropriate markdown for each component type
- **GetRelativeScriptPath()**: Converts Unity asset paths to relative documentation paths
- **IsBuiltInUnityComponent()**: Detects Unity built-in components via assembly analysis
- **Switch Case Ordering**: Fixed inheritance hierarchy in `ExportBuiltInComponentProperties()`

**Bug Fixes**:
- **CS8120 Compilation Error**: Fixed unreachable switch case by moving `CharacterController` before `Collider`
- **Type Hierarchy Handling**: Added comments explaining inheritance-based case ordering requirements

**Integration Impact**:
- **Enhanced AI Context**: Clickable component references improve code generation accuracy
- **Improved Traceability**: Direct links between exported metadata and source code
- **Maintained Compatibility**: Backward compatible with existing export format
- **Better Documentation**: Clear distinction between custom scripts and Unity components

**Testing Requirements**: Validate new linking functionality and verify compilation success