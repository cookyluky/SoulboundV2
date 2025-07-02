# Test MetadataExporter_Test: Unity Metadata Export Functionality

## Test Information
**Test ID**: MetadataExporter_Test  
**Test Type**: Unity Editor Tool Validation  
**Created**: 2025-01-27  
**Related Object**: @MetadataExporter - [MetadataExporter.md](mdc:Documentation/Objects/MetadataExporter.md)  
**Implements Rule**: @InteractiveTestDocumentation - [InteractiveTestDocumentation.mdc](mdc:.cursor/rules/InteractiveTestDocumentation.mdc)  
**Priority**: High (Critical Unity Editor tool)

## Test Objective
Validate that the MetadataExporter Unity Editor script correctly exports scene and prefab metadata to structured markdown files, enabling AI context awareness for code generation.

## Preconditions
- [ ] Unity Editor 2024.3 LTS (or compatible) is open
- [ ] Soulbound project is loaded in Unity
- [ ] Assets/Scripts/Editor/MetadataExporter.cs is present and compiled without errors
- [ ] SampleScene is available in Assets/Scenes/
- [ ] Documentation/UnityExports/ directory structure exists
- [ ] No console errors are present in Unity Editor

## Unity Editor Instructions
**Setup Requirements**: Unity Editor with Soulbound project loaded  
**Scene Configuration**: SampleScene or any available scene in Assets/Scenes/  
**Object Preparation**: Ensure at least one GameObject exists in the scene hierarchy  
**Component Settings**: Verify GameObjects have various components (Transform, scripts, etc.)  

## Test Steps

### Step 1: Verify Unity Editor Integration
**Unity Editor Action**: 
1. Open Unity Editor with Soulbound project
2. Navigate to top menu bar
3. Click on "Tools" menu
4. Verify "Export" submenu appears
5. Check that the following menu items are present:
   - Scene Metadata
   - Prefab Metadata  
   - Export All Metadata
   - Metadata Exporter Window

**Expected Result**: All MetadataExporter menu items are visible and accessible in Tools → Export submenu  
**Actual Result**: [Initially blank - filled during execution]

### Step 2: Open MetadataExporter Editor Window
**Unity Editor Action**:
1. Click Tools → Export → Metadata Exporter Window
2. Wait for the window to open
3. Verify window title shows "Metadata Exporter"
4. Check that all UI elements are present:
   - Export Options section with buttons
   - Auto-Export Settings with toggles
   - Export Directories section with paths
   - "Create Export Directories" button

**Expected Result**: MetadataExporter window opens with all UI controls visible and functional  
**Actual Result**: [Initially blank - filled during execution]

### Step 3: Test Export Directory Creation
**Unity Editor Action**:
1. In MetadataExporter window, click "Create Export Directories" button
2. Check Unity Console for any messages
3. Navigate to project root folder in Windows Explorer/Finder
4. Verify Documentation/UnityExports/ directory exists
5. Check that Scenes/ and Prefabs/ subdirectories are created

**Expected Result**: Export directories are created successfully, Console shows creation confirmation  
**Actual Result**: [Initially blank - filled during execution]

### Step 4: Export Current Scene Metadata
**Unity Editor Action**:
1. Ensure SampleScene is loaded (or any scene with GameObjects)
2. In MetadataExporter window, click "Export Current Scene Metadata" button
3. Monitor Unity Console for export messages
4. Navigate to Documentation/UnityExports/Scenes/ directory
5. Verify Scene_SampleScene_Metadata.md file is created
6. Check that Scene_SampleScene_LastExported.txt timestamp file is present

**Expected Result**: Scene metadata file and timestamp file are created successfully  
**Actual Result**: [Initially blank - filled during execution]

### Step 5: Validate Scene Metadata Content
**Unity Editor Action**:
1. Open Scene_SampleScene_Metadata.md in a text editor
2. Verify file contains the following sections:
   - Header with scene name
   - Export Information with Type, Unity Path, timestamp, Unity Version
   - Hierarchy Structure with GameObject tree
   - Asset Dependencies section
3. Check that GameObject hierarchy is properly indented
4. Verify component information includes script names and properties
5. Confirm Transform data is present for all GameObjects

**Expected Result**: Metadata file contains complete, properly formatted scene information  
**Actual Result**: [Initially blank - filled during execution]

### Step 6: Test Prefab Metadata Export
**Unity Editor Action**:
1. Verify at least one prefab exists in Assets/Prefabs/ (create simple cube prefab if needed)
2. In MetadataExporter window, click "Export All Prefab Metadata" button
3. Monitor Unity Console for export progress messages
4. Navigate to Documentation/UnityExports/Prefabs/ directory
5. Verify metadata files are created for each prefab
6. Check timestamp files are present alongside metadata files

**Expected Result**: Prefab metadata files are created for all prefabs in Assets/Prefabs/  
**Actual Result**: [Initially blank - filled during execution]

### Step 7: Test Batch Export Functionality
**Unity Editor Action**:
1. In MetadataExporter window, click "Export All Metadata (Scenes + Prefabs)" button
2. Wait for export process to complete (may take several seconds)
3. Monitor Unity Console for completion messages
4. Verify both Scenes/ and Prefabs/ directories contain updated files
5. Check that all timestamp files show recent export times

**Expected Result**: All scenes and prefabs are exported successfully in one batch operation  
**Actual Result**: [Initially blank - filled during execution]

### Step 8: Test Menu-Based Scene Export
**Unity Editor Action**:
1. Navigate to Tools → Export → Scene Metadata (direct menu item)
2. Verify export executes without opening the MetadataExporter window
3. Check Unity Console for export confirmation
4. Confirm scene metadata file is updated with new timestamp

**Expected Result**: Direct menu export works independently of the editor window  
**Actual Result**: [Initially blank - filled during execution]

### Step 9: Validate Auto-Export Configuration
**Unity Editor Action**:
1. In MetadataExporter window, check "Auto-export on Scene Save" toggle
2. Click "Apply Auto-Export Settings" button
3. Make a small change to the scene (add/move a GameObject)
4. Save the scene using Ctrl+S or File → Save
5. Monitor Unity Console for auto-export message
6. Verify scene metadata file timestamp is updated

**Expected Result**: Scene metadata is automatically exported when scene is saved  
**Actual Result**: [Initially blank - filled during execution]

### Step 10: Test Error Handling
**Unity Editor Action**:
1. Temporarily rename Documentation/UnityExports/ to test error handling
2. Attempt to export scene metadata
3. Check Unity Console for error messages
4. Verify MetadataExporter attempts to recreate directories
5. Restore original directory name

**Expected Result**: MetadataExporter handles missing directories gracefully and recreates them  
**Actual Result**: [Initially blank - filled during execution]

## Status
**Current Status**: Pending  
**Execution Date**: [To be filled during test execution]  
**Executed By**: [To be filled during test execution]

## Results Summary
**Total Steps**: 10  
**Steps Passed**: [To be filled during execution]  
**Steps Failed**: [To be filled during execution]  
**Overall Result**: [To be determined during execution]

## Test Results
[This section will be filled during interactive test execution]

## Issues Found
[To be documented if any issues are discovered during testing]

## Remediation Tasks
[New tasks will be created here if test failures require fixes]

## Changelog
### 2025-01-27 15:45:00 - Test Creation
**Created By**: Claude  
**Test Scope**: Complete MetadataExporter functionality validation  
**Test Type**: Interactive Unity Editor testing with step-by-step verification  
**Next Step**: Ready for interactive execution with user 