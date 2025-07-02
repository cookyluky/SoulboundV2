# Task 17 Overview: Core Manager Singletons

## Task Description
Scaffold & register every fundamental manager that will serve as the backbone of the game's architecture. These singleton managers handle core systems like game state, input, saving, UI, audio, and scene loading.

## Priority Level
**High** - Essential foundation managers that all other game systems depend on. Must be completed immediately after bootstrap system.

## Dependencies
- Task 16: Bootstrap Scene & Service Locator

## Detailed Breakdown

### Core Objectives
1. **GameManager Implementation**
   - Central game state management and act transitions
   - Handles game flow, pausing, and state persistence
   - Coordinates between different game systems

2. **InputManager Implementation**
   - Unity's new Input System integration with C# events
   - Cross-platform input handling (keyboard, mouse, gamepad, touch)
   - Input action mapping and event broadcasting

3. **SaveManager Implementation**
   - Game data serialization and persistence
   - Cross-platform save file management
   - Stub methods for save/load functionality

4. **UIManager Implementation**
   - Centralized UI control and navigation
   - Menu opening/closing and HUD updates
   - UI state management and transitions

5. **AudioManager Implementation**
   - Music and sound effect playback control
   - Audio mixing and volume management
   - Platform-specific audio optimization

6. **SceneLoader Implementation**
   - Asynchronous scene transitions with loading screens
   - Scene state management and cleanup
   - Performance-optimized scene loading

## Technical Requirements

### Core Manager Classes
```csharp
public class GameManager : MonoBehaviour, IGameManager {
    public GameState CurrentState { get; private set; }
    public bool IsPaused { get; private set; }
    
    public void ChangeState(GameState newState);
    public void PauseGame();
    public void ResumeGame();
    public void StartNewGame();
    public void LoadGame();
}

public class InputManager : MonoBehaviour, IInputManager {
    public event System.Action<Vector2> OnMove;
    public event System.Action OnAttack;
    public event System.Action OnDodge;
    public event System.Action OnInteract;
    public event System.Action OnPause;
    
    public void Initialize();
    public void SetInputEnabled(bool enabled);
}

public class SaveManager : MonoBehaviour, ISaveManager {
    public void SaveGame(GameData data);
    public GameData LoadGame();
    public bool HasSaveFile();
    public void DeleteSave();
}

public class UIManager : MonoBehaviour, IUIManager {
    public void OpenMenu(MenuType menuType);
    public void CloseMenu(MenuType menuType);
    public void UpdateHUD(HUDData data);
    public void ShowLoadingScreen(bool show);
}

public class AudioManager : MonoBehaviour, IAudioManager {
    public void PlayMusic(string trackName, bool loop = true);
    public void PlaySFX(string clipName, float volume = 1f);
    public void SetMusicVolume(float volume);
    public void SetSFXVolume(float volume);
}

public class SceneLoader : MonoBehaviour, ISceneLoader {
    public event System.Action<string> OnSceneLoadStarted;
    public event System.Action<string> OnSceneLoadCompleted;
    
    public void LoadSceneAsync(string sceneName);
    public void LoadSceneWithLoadingScreen(string sceneName);
}
```

## Success Criteria
- [ ] All six core managers implemented as MonoBehaviour singletons
- [ ] Each manager registers with ServiceLocator during initialization
- [ ] Proper interface definitions for all managers created
- [ ] Cross-manager dependencies handled correctly
- [ ] Stub implementations functional for basic testing
- [ ] Error handling and logging implemented for all managers
- [ ] Performance optimized for frequent access patterns
- [ ] Memory usage optimized with proper cleanup
- [ ] Platform compatibility verified across all targets
- [ ] Integration with Bootstrap system validated

## Risk Factors
- **Singleton Lifecycle**: Proper creation, registration, and cleanup of singleton instances
- **Circular Dependencies**: Managers may need to reference each other
- **Initialization Order**: Critical sequencing of manager startup
- **Memory Leaks**: Persistent objects must be properly managed
- **Platform Differences**: Input and audio systems vary across platforms

## Related Systems
- **ServiceLocator**: Registration and dependency injection
- **Bootstrap System**: Initialization and lifetime management
- **All Game Features**: Every system will depend on these core managers
- **Unity Lifecycle**: Integration with Unity's MonoBehaviour lifecycle

## Manager Implementation Details

### GameManager
- **State Management**: Enum-based game state tracking
- **Act Progression**: Integration with narrative system
- **Save Integration**: Coordination with SaveManager for game state persistence
- **Event Broadcasting**: Game state change notifications

### InputManager
- **Input Actions**: Complete mapping of all game controls
- **Platform Adaptation**: Automatic input scheme detection
- **Event Broadcasting**: Type-safe input event system
- **Input Validation**: Prevention of invalid input combinations

### SaveManager
- **Data Serialization**: JSON-based save data format
- **File Management**: Platform-specific save file locations
- **Encryption**: Basic save data protection
- **Backup System**: Redundant save file creation

### UIManager
- **Menu Stack**: LIFO menu management system
- **HUD Management**: Dynamic HUD element updates
- **Loading Screens**: Seamless loading screen integration
- **Accessibility**: Support for accessibility features

### AudioManager
- **Audio Sources**: Pooled audio source management
- **Music System**: Seamless music transitions and looping
- **SFX System**: Spatial and non-spatial sound effects
- **Volume Mixing**: Master, music, and SFX volume controls

### SceneLoader
- **Async Loading**: Non-blocking scene transitions
- **Progress Tracking**: Loading progress reporting
- **Memory Management**: Proper scene cleanup and garbage collection
- **Error Handling**: Graceful handling of loading failures

## Estimated Completion Time
**5-7 days** - Includes implementation of all six managers, interface definitions, ServiceLocator integration, basic testing, and documentation. 