using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SoulBound.Core
{
    /// <summary>
    /// Save/Load management system for SoulBound RPG
    /// Handles game state persistence using Unity's JsonUtility
    /// Supports multiple save slots and auto-save functionality
    /// </summary>
    public class SaveManager : BaseManager
    {
        [Header("Save Settings")]
        [SerializeField] private int _maxSaveSlots = 10;
        [SerializeField] private bool _enableAutoSave = true;
        [SerializeField] private float _autoSaveInterval = 300f; // 5 minutes
        // [SerializeField] private bool _compressData = true; // TODO: Implement compression in future update

        [Header("File Settings")]
        [SerializeField] private string _saveFileExtension = ".sav";
        [SerializeField] private string _saveFileName = "save_slot_";
        [SerializeField] private string _autoSaveFileName = "autosave";

        // Events
        public static event Action<int> OnGameSaved;
        public static event Action<int> OnGameLoaded;
        public static event Action<string> OnSaveError;

        // Save data
        private GameSaveData _currentSaveData;
        private Dictionary<int, SaveSlotInfo> _saveSlots = new Dictionary<int, SaveSlotInfo>();
        
        // Auto-save
        private float _lastAutoSaveTime;
        private bool _hasUnsavedChanges;

        // Properties
        public string SaveDirectory { get; private set; }
        public GameSaveData CurrentSaveData => _currentSaveData;
        public bool HasUnsavedChanges => _hasUnsavedChanges;
        public int MaxSaveSlots => _maxSaveSlots;

        protected override void OnInitialize()
        {
            LogInfo("Setting up save system");
            
            // Set up save directory
            SaveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            
            // Create save directory if it doesn't exist
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
                LogInfo($"Created save directory: {SaveDirectory}");
            }
            
            // Initialize save data
            _currentSaveData = new GameSaveData();
            
            // Scan for existing saves
            ScanExistingSaves();
            
            LogInfo($"Save system initialized. Save directory: {SaveDirectory}");
        }

        protected override void OnCleanup()
        {
            // Save any pending changes
            if (_hasUnsavedChanges && _enableAutoSave)
            {
                AutoSave();
            }
            
            // Clear events
            OnGameSaved = null;
            OnGameLoaded = null;
            OnSaveError = null;
        }

        private void Update()
        {
            // Handle auto-save
            if (_enableAutoSave && _hasUnsavedChanges)
            {
                if (Time.time - _lastAutoSaveTime >= _autoSaveInterval)
                {
                    AutoSave();
                }
            }
        }

        #region Save Operations

        /// <summary>
        /// Save game to specified slot
        /// </summary>
        /// <param name="slotIndex">Save slot index (0-based)</param>
        /// <param name="saveDescription">Optional description for the save</param>
        /// <returns>True if save was successful</returns>
        public bool SaveGame(int slotIndex, string saveDescription = "")
        {
            if (slotIndex < 0 || slotIndex >= _maxSaveSlots)
            {
                LogError($"Invalid save slot: {slotIndex}. Must be between 0 and {_maxSaveSlots - 1}");
                OnSaveError?.Invoke($"Invalid save slot: {slotIndex}");
                return false;
            }

            try
            {
                // Update save data with current game state
                UpdateSaveData();
                
                // Set save metadata
                _currentSaveData.slotIndex = slotIndex;
                _currentSaveData.saveDescription = saveDescription;
                _currentSaveData.saveDateTime = System.DateTime.Now.ToBinary().ToString();
                
                // Get file path
                string filePath = GetSaveFilePath(slotIndex);
                
                // Serialize and save using Unity's JsonUtility
                string jsonData = JsonUtility.ToJson(_currentSaveData, true);
                
                File.WriteAllText(filePath, jsonData);
                
                // Update save slot info
                var slotInfo = new SaveSlotInfo
                {
                    slotIndex = slotIndex,
                    saveDateTime = _currentSaveData.saveDateTime,
                    saveDescription = saveDescription,
                    gameVersion = _currentSaveData.gameVersion,
                    playTime = _currentSaveData.playTime,
                    currentLevel = _currentSaveData.currentLevel,
                    playerLevel = _currentSaveData.playerLevel
                };
                
                _saveSlots[slotIndex] = slotInfo;
                
                _hasUnsavedChanges = false;
                
                LogInfo($"Game saved to slot {slotIndex}: {filePath}");
                OnGameSaved?.Invoke(slotIndex);
                
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to save game to slot {slotIndex}: {ex.Message}");
                OnSaveError?.Invoke($"Save failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load game from specified slot
        /// </summary>
        /// <param name="slotIndex">Save slot index to load from</param>
        /// <returns>True if load was successful</returns>
        public bool LoadGame(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _maxSaveSlots)
            {
                LogError($"Invalid save slot: {slotIndex}. Must be between 0 and {_maxSaveSlots - 1}");
                return false;
            }

            string filePath = GetSaveFilePath(slotIndex);
            
            if (!File.Exists(filePath))
            {
                LogError($"Save file does not exist: {filePath}");
                return false;
            }

            try
            {
                string jsonData = File.ReadAllText(filePath);
                
                _currentSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);
                
                // Apply loaded data to game systems
                ApplyLoadedData();
                
                _hasUnsavedChanges = false;
                
                LogInfo($"Game loaded from slot {slotIndex}: {filePath}");
                OnGameLoaded?.Invoke(slotIndex);
                
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to load game from slot {slotIndex}: {ex.Message}");
                OnSaveError?.Invoke($"Load failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Auto-save to dedicated auto-save slot
        /// </summary>
        public void AutoSave()
        {
            try
            {
                UpdateSaveData();
                
                _currentSaveData.saveDescription = "Auto Save";
                _currentSaveData.saveDateTime = System.DateTime.Now.ToBinary().ToString();
                
                string filePath = Path.Combine(SaveDirectory, _autoSaveFileName + _saveFileExtension);
                string jsonData = JsonUtility.ToJson(_currentSaveData, false);
                
                File.WriteAllText(filePath, jsonData);
                
                _lastAutoSaveTime = Time.time;
                _hasUnsavedChanges = false;
                
                LogInfo("Auto-save completed");
            }
            catch (Exception ex)
            {
                LogError($"Auto-save failed: {ex.Message}");
            }
        }

        #endregion

        #region Save Data Management

        /// <summary>
        /// Update save data with current game state
        /// </summary>
        private void UpdateSaveData()
        {
            _currentSaveData.gameVersion = Application.version;
            _currentSaveData.playTime = Time.time; // This should be accumulated playtime
            
            // Get game state from GameManager
            var gameManager = ServiceLocator.TryGet<GameManager>();
            if (gameManager != null)
            {
                _currentSaveData.currentLevel = gameManager.CurrentLevel;
                _currentSaveData.currentAct = gameManager.CurrentAct;
                _currentSaveData.gameState = (int)gameManager.CurrentGameState;
            }
            
            // Update player data (this would be expanded based on player system)
            _currentSaveData.playerLevel = 1; // Placeholder
            _currentSaveData.playerPosition = Vector3.zero; // Placeholder
            
            // Mark as having changes
            _hasUnsavedChanges = true;
        }

        /// <summary>
        /// Apply loaded save data to game systems
        /// </summary>
        private void ApplyLoadedData()
        {
            // Apply data to GameManager
            var gameManager = ServiceLocator.TryGet<GameManager>();
            if (gameManager != null)
            {
                gameManager.SetCurrentLevel(_currentSaveData.currentLevel);
                gameManager.SetCurrentAct(_currentSaveData.currentAct);
                // Apply other game state as needed
            }
            
            // Apply player data (would be expanded based on player system)
            // This is where we'd restore player position, stats, inventory, etc.
            
            LogInfo("Save data applied to game systems");
        }

        #endregion

        #region Save Slot Management

        /// <summary>
        /// Get information about all save slots
        /// </summary>
        /// <returns>Dictionary of save slot information</returns>
        public Dictionary<int, SaveSlotInfo> GetSaveSlots()
        {
            return new Dictionary<int, SaveSlotInfo>(_saveSlots);
        }

        /// <summary>
        /// Get information about a specific save slot
        /// </summary>
        /// <param name="slotIndex">Slot index to get info for</param>
        /// <returns>Save slot info or null if slot is empty</returns>
        public SaveSlotInfo GetSaveSlotInfo(int slotIndex)
        {
            return _saveSlots.TryGetValue(slotIndex, out SaveSlotInfo info) ? info : null;
        }

        /// <summary>
        /// Check if a save slot has data
        /// </summary>
        /// <param name="slotIndex">Slot index to check</param>
        /// <returns>True if slot has save data</returns>
        public bool HasSaveData(int slotIndex)
        {
            return _saveSlots.ContainsKey(slotIndex);
        }

        /// <summary>
        /// Delete save data from a slot
        /// </summary>
        /// <param name="slotIndex">Slot to delete</param>
        /// <returns>True if deletion was successful</returns>
        public bool DeleteSave(int slotIndex)
        {
            try
            {
                string filePath = GetSaveFilePath(slotIndex);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                _saveSlots.Remove(slotIndex);
                
                LogInfo($"Deleted save slot {slotIndex}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to delete save slot {slotIndex}: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Get the file path for a save slot
        /// </summary>
        /// <param name="slotIndex">Save slot index</param>
        /// <returns>Full file path</returns>
        private string GetSaveFilePath(int slotIndex)
        {
            return Path.Combine(SaveDirectory, _saveFileName + slotIndex + _saveFileExtension);
        }

        /// <summary>
        /// Scan existing save files and populate save slot info
        /// </summary>
        private void ScanExistingSaves()
        {
            _saveSlots.Clear();
            
            for (int i = 0; i < _maxSaveSlots; i++)
            {
                string filePath = GetSaveFilePath(i);
                
                if (File.Exists(filePath))
                {
                    try
                    {
                        string jsonData = File.ReadAllText(filePath);
                        var saveData = JsonUtility.FromJson<GameSaveData>(jsonData);
                        
                        var slotInfo = new SaveSlotInfo
                        {
                            slotIndex = i,
                            saveDateTime = saveData.saveDateTime,
                            saveDescription = saveData.saveDescription,
                            gameVersion = saveData.gameVersion,
                            playTime = saveData.playTime,
                            currentLevel = saveData.currentLevel,
                            playerLevel = saveData.playerLevel
                        };
                        
                        _saveSlots[i] = slotInfo;
                    }
                    catch (Exception ex)
                    {
                        LogError($"Failed to read save slot {i}: {ex.Message}");
                    }
                }
            }
            
            LogInfo($"Found {_saveSlots.Count} existing save files");
        }

        /// <summary>
        /// Mark that changes have been made that need saving
        /// </summary>
        public void MarkDirty()
        {
            _hasUnsavedChanges = true;
        }

        #endregion

        #region Debug

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Save Directory: {SaveDirectory}");
            info.AppendLine($"Max Save Slots: {_maxSaveSlots}");
            info.AppendLine($"Auto Save Enabled: {_enableAutoSave}");
            info.AppendLine($"Auto Save Interval: {_autoSaveInterval}s");
            info.AppendLine($"Has Unsaved Changes: {_hasUnsavedChanges}");
            info.AppendLine($"Existing Saves: {_saveSlots.Count}");
            info.AppendLine($"Last Auto Save: {_lastAutoSaveTime}");
        }

        #endregion
    }

    /// <summary>
    /// Complete save data structure for SoulBound RPG
    /// Note: Using simple types for Unity JsonUtility compatibility
    /// </summary>
    [System.Serializable]
    public class GameSaveData
    {
        [Header("Save Metadata")]
        public int slotIndex = -1;
        public string saveDescription = "";
        public string saveDateTime = ""; // Stored as binary string
        public string gameVersion = "";
        public float playTime = 0f;

        [Header("Game State")]
        public string currentLevel = "";
        public int currentAct = 1;
        public int gameState = 0; // GameState enum as int

        [Header("Player Data")]
        public int playerLevel = 1;
        public Vector3 playerPosition = Vector3.zero;
        public Vector3 playerRotation = Vector3.zero;
        
        // Note: JsonUtility doesn't support Dictionary, so we'd need to use lists for complex data
        // For now, keeping it simple with basic types
    }

    /// <summary>
    /// Save slot information for UI display
    /// </summary>
    [System.Serializable]
    public class SaveSlotInfo
    {
        public int slotIndex;
        public string saveDateTime; // Stored as binary string
        public string saveDescription;
        public string gameVersion;
        public float playTime;
        public string currentLevel;
        public int playerLevel;
        
        /// <summary>
        /// Get formatted play time string
        /// </summary>
        public string GetFormattedPlayTime()
        {
            System.TimeSpan time = System.TimeSpan.FromSeconds(playTime);
            return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        
        /// <summary>
        /// Get formatted save date string
        /// </summary>
        public string GetFormattedSaveDate()
        {
            try
            {
                long dateTimeBinary = System.Convert.ToInt64(saveDateTime);
                System.DateTime dateTime = System.DateTime.FromBinary(dateTimeBinary);
                return dateTime.ToString("MM/dd/yyyy HH:mm");
            }
            catch
            {
                return "Unknown Date";
            }
        }
    }
} 