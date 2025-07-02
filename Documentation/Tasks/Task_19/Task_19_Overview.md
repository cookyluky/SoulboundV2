# Task 19 Overview: GameState & Save/Load Foundation

## Task Description
Central `GameState` class + stub `SaveManager` integration. Establish the foundation for persistent game data management, state tracking, and save/load functionality that will support the full game progression system.

## Priority Level
**High** - Essential for maintaining game progress and state across sessions. Required before any meaningful gameplay progression can be implemented.

## Dependencies
- Task 17: Core Manager Singletons (specifically SaveManager and GameManager)

## Detailed Breakdown

### Core Objectives
1. **GameState Class Design**
   - Comprehensive data structure for all persistent game information
   - Serializable format for save/load operations
   - Version-controlled data structure for future updates

2. **GameManager Integration**
   - GameState management and access through GameManager
   - State validation and error handling
   - State change event broadcasting

3. **SaveManager Implementation**
   - File-based save/load operations
   - Platform-specific save location handling
   - Basic save file validation and corruption detection

4. **Debug UI Integration**
   - MainMenu debug buttons for save/load testing
   - Developer tools for state manipulation
   - Save file management and debugging

## Technical Requirements

### Core Data Structures
```csharp
[System.Serializable]
public class GameState {
    [Header("Progress")]
    public int currentAct = 1;
    public int currentLevel = 1;
    public float completionPercentage = 0f;
    
    [Header("Character")]
    public float corruptionLevel = 0f;
    public List<string> unlockedAbilities = new List<string>();
    public Dictionary<string, int> characterStats = new Dictionary<string, int>();
    
    [Header("Inventory")]
    public List<string> inventory = new List<string>();
    public Dictionary<string, int> soulEssences = new Dictionary<string, int>();
    
    [Header("Story")]
    public List<string> completedQuests = new List<string>();
    public Dictionary<string, object> storyFlags = new Dictionary<string, object>();
    
    [Header("Settings")]
    public PlayerSettings playerSettings = new PlayerSettings();
    public DateTime lastSaveTime;
    public string saveVersion = "1.0.0";
    
    public GameState();
    public GameState Clone();
    public void Reset();
    public bool IsValid();
}

[System.Serializable]
public class PlayerSettings {
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public int qualityLevel = 2;
    public bool fullscreen = true;
    public KeyCode[] keyBindings = new KeyCode[10];
}
```

### Manager Integration
```csharp
public class GameManager : MonoBehaviour {
    [SerializeField] private GameState currentState;
    private SaveManager saveManager;
    
    public GameState CurrentState => currentState;
    public event System.Action<GameState> OnStateChanged;
    
    public void Initialize();
    public GameState GetState();
    public void SetState(GameState newState);
    public void ResetState();
    
    // Act progression
    public void AdvanceToAct(int actNumber);
    public void CompleteLevel(int levelNumber);
    
    // Character progression
    public void ModifyCorruption(float amount);
    public void UnlockAbility(string abilityId);
    public void AddSoulEssence(string essenceType, int amount);
}

public class SaveManager : MonoBehaviour {
    [SerializeField] private string saveFileName = "gamesave.json";
    private string savePath;
    
    public event System.Action OnSaveCompleted;
    public event System.Action<GameState> OnLoadCompleted;
    public event System.Action<string> OnSaveError;
    
    public void Initialize();
    public void SaveGame(GameState gameState);
    public GameState LoadGame();
    public bool HasSaveFile();
    public void DeleteSave();
    public void BackupSave();
    public void RestoreBackup();
}
```

## Success Criteria
- [ ] GameState class implemented with all essential game data fields
- [ ] GameState properly serializes to/from JSON format
- [ ] GameManager provides centralized access to current game state
- [ ] SaveManager successfully saves GameState to platform-appropriate location
- [ ] SaveManager loads GameState and validates data integrity
- [ ] Debug UI buttons functional for save/load testing
- [ ] Save file corruption detection and basic recovery implemented
- [ ] Platform-specific save locations properly configured
- [ ] State change events properly broadcast to interested systems
- [ ] Memory usage optimized for state management operations

## Risk Factors
- **Data Corruption**: Save files may become corrupted requiring validation
- **Platform Differences**: Save locations and permissions vary across platforms
- **Serialization Issues**: Complex data structures may not serialize properly
- **Version Compatibility**: Save files from different game versions may be incompatible
- **Performance Impact**: Frequent state changes may impact game performance

## Related Systems
- **All Game Systems**: Every system will need to persist some state data
- **UI System**: Save/load progress indication and error handling
- **Scene Management**: State persistence across scene transitions
- **Player Progression**: Character stats, abilities, and corruption tracking
- **Quest System**: Quest completion and story flag persistence

## Save System Architecture

### Save File Structure
```json
{
  "gameState": {
    "currentAct": 1,
    "currentLevel": 1,
    "corruptionLevel": 0.0,
    "unlockedAbilities": ["basic_attack", "dodge"],
    "characterStats": {
      "health": 100,
      "stamina": 50,
      "spirit": 25
    },
    "inventory": ["soul_fragment_001", "health_potion"],
    "soulEssences": {
      "wraith": 10,
      "hollow": 5,
      "arcanum": 2
    },
    "completedQuests": ["tutorial_combat", "first_essence"],
    "storyFlags": {
      "met_elysia": true,
      "corruption_warning_shown": false
    },
    "playerSettings": {
      "masterVolume": 1.0,
      "musicVolume": 0.8,
      "sfxVolume": 0.9
    },
    "lastSaveTime": "2025-01-27T15:30:00Z",
    "saveVersion": "1.0.0"
  }
}
```

### Platform Save Locations
- **Windows**: `%USERPROFILE%/AppData/LocalLow/[CompanyName]/[ProductName]/`
- **macOS**: `~/Library/Application Support/[CompanyName]/[ProductName]/`
- **Linux**: `~/.config/unity3d/[CompanyName]/[ProductName]/`
- **Mobile**: Platform-specific persistent data paths

### Save System Features
- **Auto-Save**: Automatic saving at key progression points
- **Manual Save**: Player-initiated save functionality
- **Multiple Slots**: Support for multiple save files
- **Backup System**: Automatic backup creation before overwriting
- **Validation**: Save file integrity checking and corruption detection

## Debug Integration

### Debug UI Features
- **Save Game**: Manual save trigger with success/failure feedback
- **Load Game**: Manual load trigger with validation
- **Reset State**: Clear current state and start fresh
- **State Inspector**: Display current state values for debugging
- **Save File Info**: Display save file location and metadata

### Developer Tools
- **State Manipulation**: Direct editing of GameState values
- **Save File Validation**: Check save file integrity
- **Migration Tools**: Convert save files between versions
- **Performance Monitoring**: Track save/load operation timing

## Estimated Completion Time
**4-6 days** - Includes GameState design, GameManager integration, SaveManager implementation, debug UI creation, platform testing, and validation systems. 