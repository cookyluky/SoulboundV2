#  Metadata

## Export Information
**Type**: Scene
**Unity Path**: 
**Last Exported**: 2025-06-30T22:38:32Z
**Unity Version**: 6000.1.9f1
**Export Script**: MetadataExporter v1.0.0

## Hierarchy Structure
```
Bootstrapper (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [Bootstrapper.cs](../../Scripts/Core/Bootstrapper.cs)
    │   └── Enable Debug Logging: True
    │   └── Initial Scene Name: "MainMenu"
    │   └── Initialization Delay: 0.10
    │   └── Game Manager Prefab: GameManager_Prefab
    │   └── Audio Manager Prefab: AudioManager_Prefab
    │   └── Essence Manager Prefab: EssenceManager_Prefab
    │   └── Ui Manager Prefab: UIManager_Prefab
    │   └── Input Manager Prefab: InputManager_Prefab
    │   └── Save Manager Prefab: SaveManager_Prefab
    │   └── Scene Loader Prefab: SceneLoader_Prefab
Directional Light (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.00,3.00,0.00) Rot(0.41,-0.23,0.11,0.88) Scale(1.00,1.00,1.00)
  ├── [Light](https://docs.unity3d.com/ScriptReference/Light.html)
EssenceManager (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.51,0.00,-5.04) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [EssenceManager.cs](../../Scripts/Systems/EssenceManager.cs)
    │   └── Enable Debug Logging: True
    │   └── Absorption Time Window: 3.00
    │   └── Require Player Input: True
    │   └── Auto Absorption Delay: 1.50
    │   └── Max Absorption Range: 5.00
    │   └── Max Bank Capacity Per Type: 100.00
    │   └── Banking Efficiency: 0.90
    │   └── Essence Particle Prefab: null
    │   └── Absorption Sound: null
    │   └── Banking Sound: null
    │   └── Corruption Warning Sound: null
    │   └── Show Absorption Gizmos: True
EventSystem (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [EventSystem.cs](Packages/com.unity.ugui/Runtime/UGUI/EventSystem/EventSystem.cs)
  ├── [StandaloneInputModule.cs](Packages/com.unity.ugui/Runtime/UGUI/EventSystem/InputModules/StandaloneInputModule.cs)
Ground (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(2.00,1.00,2.00)
  ├── [MeshFilter](https://docs.unity3d.com/ScriptReference/MeshFilter.html)
  ├── [MeshRenderer](https://docs.unity3d.com/ScriptReference/MeshRenderer.html)
    │   └── Material: Default-Material
  ├── [MeshCollider](https://docs.unity3d.com/ScriptReference/MeshCollider.html)
    │   └── IsTrigger: False
InputManager (Layer: Default, Tag: Untagged)
├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [InputManager.cs](../../Scripts/Core/InputManager.cs)
    │   └── Enable Debug Logging: True
    │   └── Input Actions: InputActions
    │   └── Enable Input: True
Main Camera (Layer: Default, Tag: MainCamera)
├── Transform: Pos(0.00,1.00,-10.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [Camera](https://docs.unity3d.com/ScriptReference/Camera.html)
  ├── [AudioListener](https://docs.unity3d.com/ScriptReference/AudioListener.html)
Player (Layer: Player, Tag: Player)
├── Transform: Pos(0.00,1.50,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
  ├── [MeshFilter](https://docs.unity3d.com/ScriptReference/MeshFilter.html)
  ├── [MeshRenderer](https://docs.unity3d.com/ScriptReference/MeshRenderer.html)
    │   └── Material: Default-Material
  ├── [CapsuleCollider](https://docs.unity3d.com/ScriptReference/CapsuleCollider.html)
    │   └── IsTrigger: False
  ├── [CharacterController](https://docs.unity3d.com/ScriptReference/CharacterController.html)
    │   └── Height: 2.00
    │   └── Radius: 0.50
    │   └── Center: (0.00,0.00,0.00)
  ├── [PlayerMovement.cs](../../Scripts/PlayerMovement.cs)
    │   └── Move Speed: 5.00
    │   └── Jump Height: 2.00
    │   └── Gravity: -9.81
    │   └── Ground Check Distance: 0.20
    │   └── Ground Layers: 1
    │   └── Acceleration: 25.00
    │   └── Deceleration: 15.00
    │   └── Air Control: 0.30
    │   └── Enable Debug Logging: True
    │   └── Show Ground Check Gizmo: True
  ├── [PlayerController.cs](../../Scripts/PlayerController.cs)
    │   └── Max Health: 100.00
    │   └── Max Mana: 50.00
    │   └── Starting Level: 1
    │   └── Base Experience To Next Level: 100.00
    │   └── Soul Capacity: 10.00
    │   └── Corruption Threshold: 5.00
    │   └── Can Absorb Souls: True
    │   └── Player Movement: Player
    │   └── Enable Debug Logging: True
UI_Canvas (Layer: UI, Tag: Untagged)
├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(0.46,0.46,0.46)
  ├── [Canvas](https://docs.unity3d.com/ScriptReference/Canvas.html)
  ├── [CanvasScaler.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/CanvasScaler.cs)
  ├── [GraphicRaycaster.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/GraphicRaycaster.cs)
  EssenceInventoryPanel (Layer: UI, Tag: Untagged)
  ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
    ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
    ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    ├── [EssenceInventoryUI.cs](../../Scripts/UI/EssenceInventoryUI.cs)
      │   └── Inventory Panel: EssenceInventoryPanel
      │   └── Essence Slot Container: EssenceSlotContainer
      │   └── Essence Slot Prefab: EssenceSlotUI_Prefab 1
      │   └── Toggle Inventory Button: ToggleInventoryButton
      │   └── Total Essence Text: TotalEssenceText
      │   └── Conversion Panel: ConversionPanel
      │   └── Convert To Health Button: ConvertToHealthButton
      │   └── Convert To Mana Button: ConvertToManaButton
      │   └── Convert To Experience Button: ConvertToExperienceButton
      │   └── Conversion Amount Slider: ConversionAmountSlider
      │   └── Conversion Amount Text: ConversionAmountText
      │   └── Start Visible: False
      │   └── Enable Debug Logging: True
    ├── [CanvasGroup](https://docs.unity3d.com/ScriptReference/CanvasGroup.html)
    InventoryTitle (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,526.89,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ToggleInventoryButton (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Toggle Inventory  (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    TotalEssenceText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(409.36,526.89,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceSlotContainer (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(379.15,308.30,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [GridLayoutGroup.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/GridLayoutGroup.cs)
    ConversionPanel (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ConvertToHealthButton (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To Health (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConvertToManaButton (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To Mana (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConvertToExperienceButton (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To XP (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConversionAmountSlider (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [Slider.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
        Background (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        Fill Area (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(372.64,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          Fill (Layer: UI, Tag: Untagged)
          ├── Transform: Pos(340.11,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
            ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
            ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        Handle Slide Area (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          Handle (Layer: UI, Tag: Untagged)
          ├── Transform: Pos(342.43,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
            ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
            ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ConversionAmountText (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.96,71.11,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
  EssenceChoiceDialogPanel (Layer: UI, Tag: Untagged)
  ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
    ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
    ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    ├── [EssenceChoiceDialog.cs](../../Scripts/UI/EssenceChoiceDialog.cs)
      │   └── Dialog Panel: EssenceChoiceDialogPanel
      │   └── Essence Type Text: EssenceTypeText
      │   └── Essence Amount Text: EssenceAmountText
      │   └── Essence Description Text: EssenceDescriptionText
      │   └── Essence Icon: EssenceIcon
      │   └── Consume Immediately Button: ConsumeImmediatelyButton
      │   └── Bank Essence Button: BankEssenceButton
      │   └── Cancel Button: CancelButton
      │   └── Immediate Effect Text: ImmediateEffectText
      │   └── Banking Effect Text: BankingEffectText
      │   └── Corruption Warning Text: CorruptionWarningText
      │   └── Corruption Warning Panel: CorruptionWarningPanel
      │   └── Time Remaining Slider: TimeRemainingSlider
      │   └── Time Remaining Text: TimeRemainingText
      │   └── Timer Fill Image: Fill
      │   └── Safe Essence Color: Color(0.00,1.00,0.00,1.00)
      │   └── Dangerous Essence Color: Color(1.00,0.00,0.00,1.00)
      │   └── Auto Close Time: 5.00
    ├── [CanvasGroup](https://docs.unity3d.com/ScriptReference/CanvasGroup.html)
    EssenceTypeText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceIcon (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(577.97,459.04,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    EssenceAmountText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceDescriptionText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ImmediateEffectText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    BankingEffectText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    CorruptionWarningPanel (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      CorruptionWarningText (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    TimeRemainingSlider (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [Slider.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
      Background (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      Fill Area (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(372.18,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        Fill (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(339.64,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      Handle Slide Area (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        Handle (Layer: UI, Tag: Untagged)
        ├── Transform: Pos(341.97,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    TimeRemainingText (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ConsumeImmediatelyButton (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Consume Now (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    BankEssenceButton (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Bank for Later (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    CancelButton (Layer: UI, Tag: Untagged)
    ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Cancel (Layer: UI, Tag: Untagged)
      ├── Transform: Pos(374.50,299.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
```

## Asset Dependencies
### Materials
- Default-Material (Resources/unity_builtin_extra)

### Scripts
- Bootstrapper.cs (Assets/Scripts/Core/Bootstrapper.cs)
- Button.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
- CanvasScaler.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/CanvasScaler.cs)
- EssenceChoiceDialog.cs (Assets/Scripts/UI/EssenceChoiceDialog.cs)
- EssenceInventoryUI.cs (Assets/Scripts/UI/EssenceInventoryUI.cs)
- EssenceManager.cs (Assets/Scripts/Systems/EssenceManager.cs)
- EventSystem.cs (Packages/com.unity.ugui/Runtime/UGUI/EventSystem/EventSystem.cs)
- GraphicRaycaster.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/GraphicRaycaster.cs)
- GridLayoutGroup.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/GridLayoutGroup.cs)
- Image.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
- InputManager.cs (Assets/Scripts/Core/InputManager.cs)
- PlayerController.cs (Assets/Scripts/PlayerController.cs)
- PlayerMovement.cs (Assets/Scripts/PlayerMovement.cs)
- Slider.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
- StandaloneInputModule.cs (Packages/com.unity.ugui/Runtime/UGUI/EventSystem/InputModules/StandaloneInputModule.cs)
- TextMeshProUGUI.cs (Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)

