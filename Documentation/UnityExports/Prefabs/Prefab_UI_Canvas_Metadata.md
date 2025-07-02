# UI_Canvas Metadata

## Export Information
**Type**: Prefab
**Unity Path**: Assets/Prefabs/UI_Canvas.prefab
**Last Exported**: 2025-06-30T22:38:32Z
**Unity Version**: 6000.1.9f1
**Export Script**: MetadataExporter v1.0.0

## Hierarchy Structure
```
UI_Canvas (Layer: UI, Tag: Untagged) [INACTIVE]
├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(0.00,0.00,0.00)
  ├── [Canvas](https://docs.unity3d.com/ScriptReference/Canvas.html)
  ├── [CanvasScaler.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/CanvasScaler.cs)
  ├── [GraphicRaycaster.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/GraphicRaycaster.cs)
  EssenceInventoryPanel (Layer: UI, Tag: Untagged) [INACTIVE]
  ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
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
    InventoryTitle (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ToggleInventoryButton (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Toggle Inventory  (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    TotalEssenceText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceSlotContainer (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [GridLayoutGroup.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/GridLayoutGroup.cs)
    ConversionPanel (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ConvertToHealthButton (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To Health (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConvertToManaButton (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To Mana (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConvertToExperienceButton (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
        To XP (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
            │   └── Parent Linked Component: null
            │   └── Check Padding Required: False
      ConversionAmountSlider (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [Slider.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
        Background (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        Fill Area (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          Fill (Layer: UI, Tag: Untagged) [INACTIVE]
          ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
            ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
            ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
        Handle Slide Area (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          Handle (Layer: UI, Tag: Untagged) [INACTIVE]
          ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
            ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
            ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ConversionAmountText (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
  EssenceChoiceDialogPanel (Layer: UI, Tag: Untagged) [INACTIVE]
  ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
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
    EssenceTypeText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceIcon (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    EssenceAmountText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    EssenceDescriptionText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ImmediateEffectText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    BankingEffectText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    CorruptionWarningPanel (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      CorruptionWarningText (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    TimeRemainingSlider (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [Slider.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
      Background (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      Fill Area (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        Fill (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      Handle Slide Area (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        Handle (Layer: UI, Tag: Untagged) [INACTIVE]
        ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
          ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
          ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
    TimeRemainingText (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
        │   └── Parent Linked Component: null
        │   └── Check Padding Required: False
    ConsumeImmediatelyButton (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Consume Now (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    BankEssenceButton (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Bank for Later (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
    CancelButton (Layer: UI, Tag: Untagged) [INACTIVE]
    ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
      ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
      ├── [Image.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
      ├── [Button.cs](Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
      Cancel (Layer: UI, Tag: Untagged) [INACTIVE]
      ├── Transform: Pos(0.00,0.00,0.00) Rot(0.00,0.00,0.00,1.00) Scale(1.00,1.00,1.00)
        ├── [CanvasRenderer](https://docs.unity3d.com/ScriptReference/CanvasRenderer.html)
        ├── [TextMeshProUGUI.cs](Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)
          │   └── Parent Linked Component: null
          │   └── Check Padding Required: False
```

## Asset Dependencies
### Scripts
- Button.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Button.cs)
- CanvasScaler.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/CanvasScaler.cs)
- EssenceChoiceDialog.cs (Assets/Scripts/UI/EssenceChoiceDialog.cs)
- EssenceInventoryUI.cs (Assets/Scripts/UI/EssenceInventoryUI.cs)
- GraphicRaycaster.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/GraphicRaycaster.cs)
- GridLayoutGroup.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Layout/GridLayoutGroup.cs)
- Image.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Image.cs)
- Slider.cs (Packages/com.unity.ugui/Runtime/UGUI/UI/Core/Slider.cs)
- TextMeshProUGUI.cs (Packages/com.unity.ugui/Runtime/TMP/TextMeshProUGUI.cs)

