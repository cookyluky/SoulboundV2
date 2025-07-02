# Player Metadata

## Export Information
**Type**: Prefab
**Unity Path**: Assets/Prefabs/Player.prefab
**Last Exported**: 2025-06-30T22:38:32Z
**Unity Version**: 6000.1.9f1
**Export Script**: MetadataExporter v1.0.0

## Hierarchy Structure
```
Player (Layer: Player, Tag: Player) [INACTIVE]
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
```

## Asset Dependencies
### Materials
- Default-Material (Resources/unity_builtin_extra)

### Scripts
- PlayerController.cs (Assets/Scripts/PlayerController.cs)
- PlayerMovement.cs (Assets/Scripts/PlayerMovement.cs)

