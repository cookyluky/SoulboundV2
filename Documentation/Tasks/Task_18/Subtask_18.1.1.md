# Subtask 18.1.1: Create InputActions Asset

## Implementation Status
**Current Status**: Waiting for User Action
**Started Date**: 2025-01-27
**Dependencies**: Unity Input System package installed ✅

## Overview
This subtask requires manual Unity Editor actions to create the InputActions.inputactions asset file that the updated InputManager is expecting. The InputManager has been fully implemented and is ready to work with this asset.

## Unity Editor Steps Required

### Step 1: Create Input Actions Asset
1. **Navigate to Input Directory**:
   - In Unity Project window, navigate to `Assets/Input/`

2. **Create Input Actions Asset**:
   - Right-click in the `Assets/Input/` folder
   - Select **Create → Input Actions**
   - **Rename the file to**: `InputActions`
   - The file should be named: `InputActions.inputactions`

### Step 2: Configure Action Maps
Open the newly created `InputActions.inputactions` file by double-clicking it. You'll see the Input Actions editor. Configure the following:

#### Action Map 1: "Gameplay"
1. **Create Gameplay Action Map**:
   - Click the **"+"** button next to "Action Maps"
   - Name it: `Gameplay`
   - Make sure it's selected (highlighted)

2. **Add Movement Action**:
   - Click **"+"** next to "Actions" 
   - Name: `Move`
   - Action Type: **Value**
   - Control Type: **Vector2**
   - Add Binding: **WASD Composite**
     - Up: W
     - Down: S  
     - Left: A
     - Right: D
   - Add Binding: **2D Vector Composite** (for gamepad)
     - Up: Gamepad Left Stick Up
     - Down: Gamepad Left Stick Down
     - Left: Gamepad Left Stick Left
     - Right: Gamepad Left Stick Right

3. **Add Attack Action**:
   - Click **"+"** next to "Actions"
   - Name: `Attack`
   - Action Type: **Button**
   - Add Binding: **Left Mouse Button**
   - Add Binding: **Gamepad South Button (A/Cross)**

4. **Add Dodge Action**:
   - Click **"+"** next to "Actions"
   - Name: `Dodge`
   - Action Type: **Button**
   - Add Binding: **Space**
   - Add Binding: **Gamepad East Button (B/Circle)**

5. **Add Interact Action**:
   - Click **"+"** next to "Actions"
   - Name: `Interact`
   - Action Type: **Button**
   - Add Binding: **E**
   - Add Binding: **Gamepad West Button (X/Square)**

6. **Add Pause Action**:
   - Click **"+"** next to "Actions"
   - Name: `Pause`
   - Action Type: **Button**
   - Add Binding: **Escape**
   - Add Binding: **Gamepad Start Button**

#### Action Map 2: "UI"
1. **Create UI Action Map**:
   - Click **"+"** next to "Action Maps"
   - Name it: `UI`

2. **Add Navigate Action**:
   - Name: `Navigate`
   - Action Type: **Value**
   - Control Type: **Vector2**
   - Add Binding: **Arrow Keys Composite**
   - Add Binding: **Gamepad D-Pad**

3. **Add Submit Action**:
   - Name: `Submit`
   - Action Type: **Button**
   - Add Binding: **Enter**
   - Add Binding: **Gamepad South Button**

4. **Add Cancel Action**:
   - Name: `Cancel`
   - Action Type: **Button**
   - Add Binding: **Escape**
   - Add Binding: **Gamepad East Button**

#### Action Map 3: "Dialogue"
1. **Create Dialogue Action Map**:
   - Click **"+"** next to "Action Maps"
   - Name it: `Dialogue`

2. **Add Advance Action**:
   - Name: `Advance`
   - Action Type: **Button**
   - Add Binding: **Space**
   - Add Binding: **Enter**
   - Add Binding: **Left Mouse Button**
   - Add Binding: **Gamepad South Button**

3. **Add Skip Action**:
   - Name: `Skip`
   - Action Type: **Button**
   - Add Binding: **Tab**
   - Add Binding: **Gamepad North Button**

### Step 3: Save and Generate C# Class
1. **Save the Asset**:
   - Click **"Save Asset"** in the Input Actions editor
   - **File → Save** (Ctrl+S)

2. **Generate C# Class** (Optional but Recommended):
   - Check the **"Generate C# Class"** checkbox in the Input Actions editor
   - Set **C# Class Name**: `InputActions`
   - Set **C# Class Namespace**: `SoulBound.Input`
   - Set **C# Class File**: `Assets/Scripts/Generated/InputActions.cs`
   - Click **"Apply"**

### Step 4: Create InputManager GameObject & Assign Asset

#### Create InputManager GameObject
1. **In your Bootstrap/Sample Scene**:
   - Right-click in **Hierarchy** → **Create Empty**
   - **Name it**: `InputManager`

2. **Add InputManager Component**:
   - Select the **InputManager** GameObject
   - **Add Component** → **Scripts** → **Input Manager**

#### Assign InputActions Asset
1. **Select the InputManager GameObject**
2. **In the Inspector**, find the **Input Manager (Script)** component
3. **Assign the InputActions Asset**:
   - **Method 1**: Drag `InputActions.inputactions` from `Assets/Input/` to the **Input Actions** field
   - **Method 2**: Click the small circle icon next to **Input Actions** field → Select your InputActions asset
4. **Verify**: The **Input Actions** field should now show "InputActions (InputActionAsset)"
5. **Enable Input**: Ensure the **Enable Input** checkbox is checked

#### 🔧 **Important: Why Manual Setup is Required**
The **Bootstrapper** will automatically find and use your manually created InputManager. If no InputManager exists in the scene, Bootstrapper creates a basic one, but **cannot assign Inspector fields like InputActions assets**. This is why manual setup is required for managers that need asset assignments.

**Workflow Summary:**
- ✅ **Manual Setup**: Create GameObject → Add Component → Assign Assets → Bootstrapper finds and uses it
- ❌ **Auto-Creation**: Bootstrapper creates basic component with no asset assignments

### Step 5: Test Input System
1. **Play Scene**:
   - Press Play in Unity Editor
   - Check Console for InputManager initialization messages

2. **Expected Console Output**:
   ```
   [InputManager] InputManager initialized successfully
   [InputManager] Input enabled
   ```

3. **If Errors Occur**:
   - Check that InputActions asset is assigned
   - Verify all action maps and actions are named correctly
   - Ensure action types match the implementation

## Verification Checklist
- [ ] InputActions.inputactions file created in Assets/Input/
- [ ] Gameplay action map with Move, Attack, Dodge, Interact, Pause actions
- [ ] UI action map with Navigate, Submit, Cancel actions  
- [ ] Dialogue action map with Advance, Skip actions
- [ ] All actions have appropriate bindings for keyboard and gamepad
- [ ] Asset assigned to InputManager in Inspector
- [ ] No console errors when playing scene
- [ ] InputManager reports successful initialization

## Integration Points
- **InputManager**: Expects this exact asset structure and naming
- **PlayerController** (Subtask 18.3): Will subscribe to InputManager events
- **UI Systems**: Will use UI action map for navigation
- **Dialogue System**: Will use Dialogue action map for text advancement

## Notes
- The InputManager is designed to be flexible - you can add more actions later
- Action map switching is handled automatically by the InputManager
- This setup supports both keyboard/mouse and gamepad input out of the box
- The generated C# class (if enabled) provides type-safe access to actions

## Related Files
- **InputManager.cs**: `Assets/Scripts/Core/InputManager.cs`
- **Input Actions Asset**: `Assets/Input/InputActions.inputactions`
- **Generated Class**: `Assets/Scripts/Generated/InputActions.cs` (if enabled)

---
**Once these steps are completed, Subtask 18.1 will be fully finished and ready for PlayerController implementation.** 