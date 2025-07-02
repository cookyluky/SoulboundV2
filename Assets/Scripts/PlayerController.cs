using System;
using UnityEngine;
using SoulBound.Core;
using SoulBound.Systems;
using System.Collections.Generic;

namespace SoulBound
{
    /// <summary>
    /// Central controller for all player-related functionality in SoulBound RPG
    /// Manages player state, coordinates between systems, and serves as the main interface for player interactions
    /// Works alongside PlayerMovement for complete player character functionality
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _maxMana = 50f;
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private float _baseExperienceToNextLevel = 100f;

        [Header("Soul-Binding Settings")]
        [SerializeField] private float _soulCapacity = 10f;
        [SerializeField] private float _corruptionThreshold = 5f;
        [SerializeField] private bool _canAbsorbSouls = true;

        [Header("Component References")]
        [SerializeField] private PlayerMovement _playerMovement;
        
        [Header("Debug Settings")]
        [SerializeField] private bool _enableDebugLogging = true;

        // Core Player State
        private float _currentHealth;
        private float _currentMana;
        private int _currentLevel;
        private float _currentExperience;
        private float _experienceToNextLevel;

        // Soul-Binding State
        private float _currentSoulEnergy;
        private float _currentCorruption;
        private bool _isDead;

        // Component References
        private InputManager _inputManager;
        private GameManager _gameManager;
        private SaveManager _saveManager;
        private EssenceManager _essenceManager;

        // Public Events for other systems to subscribe to
        public static event Action<float, float> OnHealthChanged; // (current, max)
        public static event Action<float, float> OnManaChanged; // (current, max)
        public static event Action<int> OnLevelUp; // (newLevel)
        public static event Action<float, float> OnExperienceChanged; // (current, toNext)
        public static event Action<float, float> OnSoulEnergyChanged; // (current, max)
        public static event Action<float> OnCorruptionChanged; // (currentCorruption)
        public static event Action OnPlayerDeath;
        public static event Action OnPlayerRespawn;

        // Public Properties
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public float CurrentMana => _currentMana;
        public float MaxMana => _maxMana;
        public int CurrentLevel => _currentLevel;
        public float CurrentExperience => _currentExperience;
        public float ExperienceToNextLevel => _experienceToNextLevel;
        public float CurrentSoulEnergy => _currentSoulEnergy;
        public float SoulCapacity => _soulCapacity;
        public float CurrentCorruption => _currentCorruption;
        public bool IsDead => _isDead;
        public bool CanAbsorbSouls => _canAbsorbSouls && !_isDead;

        private void Awake()
        {
            InitializeComponents();
            InitializeStats();
        }

        private void Start()
        {
            RegisterWithServices();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        #region Initialization

        private void InitializeComponents()
        {
            // Get PlayerMovement component if not assigned
            if (_playerMovement == null)
            {
                _playerMovement = GetComponent<PlayerMovement>();
            }

            // Get managers from ServiceLocator
            _inputManager = ServiceLocator.Get<InputManager>();
            _gameManager = ServiceLocator.Get<GameManager>();
            _saveManager = ServiceLocator.Get<SaveManager>();
            _essenceManager = ServiceLocator.Get<EssenceManager>();

            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Components initialized");
            }
        }

        private void InitializeStats()
        {
            _currentHealth = _maxHealth;
            _currentMana = _maxMana;
            _currentLevel = _startingLevel;
            _currentExperience = 0f;
            _experienceToNextLevel = _baseExperienceToNextLevel;
            _currentSoulEnergy = 0f;
            _currentCorruption = 0f;
            _isDead = false;

            // Notify UI systems of initial values
            NotifyStatsChanged();

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Stats initialized - Level {_currentLevel}, Health {_currentHealth}/{_maxHealth}");
            }
        }

        private void RegisterWithServices()
        {
            // Register this PlayerController with ServiceLocator for other systems to access
            ServiceLocator.Register(this);

            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Registered with ServiceLocator");
            }
        }

        private void SubscribeToEvents()
        {
            // Subscribe to input events for player actions
            if (_inputManager != null)
            {
                _inputManager.OnAttack += HandleAttackInput;
                _inputManager.OnInteract += HandleInteractInput;
                _inputManager.OnPause += HandlePauseInput;
            }

            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Subscribed to input events");
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_inputManager != null)
            {
                _inputManager.OnAttack -= HandleAttackInput;
                _inputManager.OnInteract -= HandleInteractInput;
                _inputManager.OnPause -= HandlePauseInput;
            }
        }

        #endregion

        #region Input Handling

        private void HandleAttackInput()
        {
            if (_isDead) return;

            // TODO: Implement attack logic when combat system is ready
            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Attack input received");
            }
        }

        private void HandleInteractInput()
        {
            if (_isDead) return;

            // Try to absorb essence first
            if (_essenceManager != null && _essenceManager.TryAbsorbEssence(transform.position, false)) // Default to banking
            {
                if (_enableDebugLogging)
                {
                    Debug.Log("[PlayerController] Essence absorbed via interact input");
                }
                return;
            }

            // TODO: Implement other interaction logic (NPCs, objects, etc.)
            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Interact input received - no essence to absorb");
            }
        }

        private void HandlePauseInput()
        {
            if (_gameManager != null)
            {
                _gameManager.TogglePause();
            }
        }

        #endregion

        #region Health Management

        /// <summary>
        /// Take damage and update health
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        /// <param name="source">Source of the damage (optional)</param>
        public void TakeDamage(float damage, GameObject source = null)
        {
            if (_isDead) return;

            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Took {damage} damage. Health: {_currentHealth}/{_maxHealth}");
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Heal the player
        /// </summary>
        /// <param name="healAmount">Amount of health to restore</param>
        public void Heal(float healAmount)
        {
            if (_isDead) return;

            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + healAmount);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Healed {healAmount}. Health: {_currentHealth}/{_maxHealth}");
            }
        }

        /// <summary>
        /// Set player health directly (for loading saves, etc.)
        /// </summary>
        /// <param name="health">Health value to set</param>
        public void SetHealth(float health)
        {
            _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        #endregion

        #region Mana Management

        /// <summary>
        /// Use mana for abilities
        /// </summary>
        /// <param name="manaCost">Amount of mana to consume</param>
        /// <returns>True if there was enough mana, false otherwise</returns>
        public bool UseMana(float manaCost)
        {
            if (_isDead) return false;
            
            if (_currentMana >= manaCost)
            {
                _currentMana -= manaCost;
                OnManaChanged?.Invoke(_currentMana, _maxMana);
                
                if (_enableDebugLogging)
                {
                    Debug.Log($"[PlayerController] Used {manaCost} mana. Mana: {_currentMana}/{_maxMana}");
                }
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Restore mana
        /// </summary>
        /// <param name="manaAmount">Amount of mana to restore</param>
        public void RestoreMana(float manaAmount)
        {
            if (_isDead) return;

            _currentMana = Mathf.Min(_maxMana, _currentMana + manaAmount);
            OnManaChanged?.Invoke(_currentMana, _maxMana);

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Restored {manaAmount} mana. Mana: {_currentMana}/{_maxMana}");
            }
        }

        #endregion

        #region Experience and Leveling

        /// <summary>
        /// Add experience points
        /// </summary>
        /// <param name="experiencePoints">Experience to add</param>
        public void AddExperience(float experiencePoints)
        {
            if (_isDead) return;

            _currentExperience += experiencePoints;

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Gained {experiencePoints} XP. Total: {_currentExperience}/{_experienceToNextLevel}");
            }

            // Check for level up
            while (_currentExperience >= _experienceToNextLevel)
            {
                LevelUp();
            }

            OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
        }

        private void LevelUp()
        {
            _currentExperience -= _experienceToNextLevel;
            _currentLevel++;
            
            // Increase experience requirement for next level
            _experienceToNextLevel = _baseExperienceToNextLevel * _currentLevel;
            
            // Increase max health and mana on level up
            _maxHealth += 20f;
            _maxMana += 10f;
            
            // Restore health and mana on level up
            _currentHealth = _maxHealth;
            _currentMana = _maxMana;

            // Notify systems
            OnLevelUp?.Invoke(_currentLevel);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            OnManaChanged?.Invoke(_currentMana, _maxMana);

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] LEVEL UP! Now level {_currentLevel}. Next level: {_experienceToNextLevel} XP");
            }
        }

        #endregion

        #region Soul-Binding System

        /// <summary>
        /// Absorb soul energy from defeated enemies or soul fragments
        /// </summary>
        /// <param name="soulAmount">Amount of soul energy to absorb</param>
        /// <param name="corruptionRisk">Potential corruption from this soul</param>
        /// <param name="isConsumedImmediately">Whether soul is consumed for immediate benefit</param>
        public void AbsorbSoul(float soulAmount, float corruptionRisk = 0f, bool isConsumedImmediately = false)
        {
            if (!CanAbsorbSouls) return;

            if (isConsumedImmediately)
            {
                // Immediate consumption for healing/mana
                Heal(soulAmount * 0.5f);
                RestoreMana(soulAmount * 0.3f);
                
                // Apply corruption risk
                if (corruptionRisk > 0)
                {
                    AddCorruption(corruptionRisk);
                }

                if (_enableDebugLogging)
                {
                    Debug.Log($"[PlayerController] Consumed soul immediately: {soulAmount} energy, {corruptionRisk} corruption risk");
                }
            }
            else
            {
                // Bank the soul energy for later use
                _currentSoulEnergy = Mathf.Min(_soulCapacity, _currentSoulEnergy + soulAmount);
                OnSoulEnergyChanged?.Invoke(_currentSoulEnergy, _soulCapacity);

                if (_enableDebugLogging)
                {
                    Debug.Log($"[PlayerController] Banked soul energy: {soulAmount}. Total: {_currentSoulEnergy}/{_soulCapacity}");
                }
            }
        }

        /// <summary>
        /// Use banked soul energy for various purposes
        /// </summary>
        /// <param name="amount">Amount of soul energy to use</param>
        /// <returns>True if there was enough soul energy</returns>
        public bool UseSoulEnergy(float amount)
        {
            if (_currentSoulEnergy >= amount)
            {
                _currentSoulEnergy -= amount;
                OnSoulEnergyChanged?.Invoke(_currentSoulEnergy, _soulCapacity);
                
                if (_enableDebugLogging)
                {
                    Debug.Log($"[PlayerController] Used {amount} soul energy. Remaining: {_currentSoulEnergy}/{_soulCapacity}");
                }
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add corruption to the player
        /// </summary>
        /// <param name="corruptionAmount">Amount of corruption to add</param>
        public void AddCorruption(float corruptionAmount)
        {
            _currentCorruption += corruptionAmount;
            OnCorruptionChanged?.Invoke(_currentCorruption);

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Corruption increased by {corruptionAmount}. Total: {_currentCorruption}");
            }

            // Check for corruption penalties
            if (_currentCorruption >= _corruptionThreshold)
            {
                ApplyCorruptionPenalties();
            }
        }

        private void ApplyCorruptionPenalties()
        {
            // TODO: Implement corruption penalties (reduced max health, visual effects, etc.)
            if (_enableDebugLogging)
            {
                Debug.LogWarning($"[PlayerController] High corruption detected: {_currentCorruption}");
            }
        }

        #endregion

        #region Death and Respawn

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _currentHealth = 0;

            // Disable player movement
            if (_playerMovement != null)
            {
                _playerMovement.enabled = false;
            }

            OnPlayerDeath?.Invoke();

            if (_enableDebugLogging)
            {
                Debug.Log("[PlayerController] Player has died");
            }

            // TODO: Implement death screen, respawn logic, etc.
        }

        /// <summary>
        /// Respawn the player at a checkpoint or spawn point
        /// </summary>
        /// <param name="respawnPosition">Position to respawn at</param>
        public void Respawn(Vector3 respawnPosition)
        {
            _isDead = false;
            _currentHealth = _maxHealth * 0.5f; // Respawn with half health
            _currentMana = _maxMana * 0.5f; // Respawn with half mana

            // Move player to respawn position
            transform.position = respawnPosition;

            // Re-enable movement
            if (_playerMovement != null)
            {
                _playerMovement.enabled = true;
            }

            // Notify systems
            NotifyStatsChanged();
            OnPlayerRespawn?.Invoke();

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Player respawned at {respawnPosition}");
            }
        }

        #endregion

        #region Utility Methods

        private void NotifyStatsChanged()
        {
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            OnManaChanged?.Invoke(_currentMana, _maxMana);
            OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
            OnSoulEnergyChanged?.Invoke(_currentSoulEnergy, _soulCapacity);
            OnCorruptionChanged?.Invoke(_currentCorruption);
        }

        /// <summary>
        /// Get comprehensive player stats for UI or saving
        /// </summary>
        public PlayerStats GetPlayerStats()
        {
            return new PlayerStats
            {
                health = _currentHealth,
                maxHealth = _maxHealth,
                mana = _currentMana,
                maxMana = _maxMana,
                level = _currentLevel,
                experience = _currentExperience,
                experienceToNext = _experienceToNextLevel,
                soulEnergy = _currentSoulEnergy,
                corruption = _currentCorruption,
                isDead = _isDead,
                essenceData = _essenceManager.GetEssenceData()
            };
        }

        /// <summary>
        /// Load player stats from save data
        /// </summary>
        public void LoadPlayerStats(PlayerStats stats)
        {
            _currentHealth = stats.health;
            _maxHealth = stats.maxHealth;
            _currentMana = stats.mana;
            _maxMana = stats.maxMana;
            _currentLevel = stats.level;
            _currentExperience = stats.experience;
            _experienceToNextLevel = stats.experienceToNext;
            _currentSoulEnergy = stats.soulEnergy;
            _currentCorruption = stats.corruption;
            _isDead = stats.isDead;

            // Load essence data into EssenceManager
            if (_essenceManager != null && stats.essenceData.HasAnyEssence())
            {
                _essenceManager.SetEssenceData(stats.essenceData);
            }

            NotifyStatsChanged();

            if (_enableDebugLogging)
            {
                Debug.Log($"[PlayerController] Loaded player stats - Level {_currentLevel}, Health {_currentHealth}/{_maxHealth}");
                if (stats.essenceData.HasAnyEssence())
                {
                    Debug.Log($"[PlayerController] Loaded essence data - Total: {stats.essenceData.GetTotalEssence()}");
                }
            }
        }

        /// <summary>
        /// Get debug information about current player state
        /// </summary>
        public string GetDebugInfo()
        {
            return $"Level: {_currentLevel} | Health: {_currentHealth:F1}/{_maxHealth} | " +
                   $"Mana: {_currentMana:F1}/{_maxMana} | XP: {_currentExperience:F1}/{_experienceToNextLevel:F1} | " +
                   $"Soul: {_currentSoulEnergy:F1}/{_soulCapacity} | Corruption: {_currentCorruption:F1} | Dead: {_isDead}";
        }

        #endregion

        #region Context Menu Items

        [ContextMenu("Print Player Debug Info")]
        private void PrintDebugInfo()
        {
            Debug.Log($"[PlayerController] {GetDebugInfo()}");
        }

        [ContextMenu("Take 25 Damage")]
        private void TestTakeDamage()
        {
            TakeDamage(25f);
        }

        [ContextMenu("Heal to Full")]
        private void TestHeal()
        {
            Heal(_maxHealth);
        }

        [ContextMenu("Add 50 Experience")]
        private void TestAddExperience()
        {
            AddExperience(50f);
        }

        [ContextMenu("Absorb Soul (Immediate)")]
        private void TestAbsorbSoulImmediate()
        {
            AbsorbSoul(20f, 0.5f, true);
        }

        [ContextMenu("Absorb Soul (Banked)")]
        private void TestAbsorbSoulBanked()
        {
            AbsorbSoul(2f, 0f, false);
        }

        #endregion
    }

    /// <summary>
    /// Data structure for player stats (used for saving/loading)
    /// </summary>
    [System.Serializable]
    public struct PlayerStats
    {
        public float health;
        public float maxHealth;
        public float mana;
        public float maxMana;
        public int level;
        public float experience;
        public float experienceToNext;
        public float soulEnergy;
        public float corruption;
        public bool isDead;
        public EssenceData essenceData;
    }

    /// <summary>
    /// Serializable data structure for essence storage
    /// Stores banked essence amounts for each type
    /// </summary>
    [System.Serializable]
    public struct EssenceData
    {
        public float vitalityEssence;
        public float strengthEssence;
        public float arcaneEssence;
        public float forbiddenEssence;

        /// <summary>
        /// Convert EssenceData to Dictionary for EssenceManager
        /// </summary>
        public Dictionary<EssenceType, float> ToDictionary()
        {
            return new Dictionary<EssenceType, float>
            {
                { EssenceType.Vitality, vitalityEssence },
                { EssenceType.Strength, strengthEssence },
                { EssenceType.Arcane, arcaneEssence },
                { EssenceType.Forbidden, forbiddenEssence }
            };
        }

        /// <summary>
        /// Create EssenceData from Dictionary
        /// </summary>
        public static EssenceData FromDictionary(Dictionary<EssenceType, float> essenceDict)
        {
            return new EssenceData
            {
                vitalityEssence = essenceDict.GetValueOrDefault(EssenceType.Vitality, 0f),
                strengthEssence = essenceDict.GetValueOrDefault(EssenceType.Strength, 0f),
                arcaneEssence = essenceDict.GetValueOrDefault(EssenceType.Arcane, 0f),
                forbiddenEssence = essenceDict.GetValueOrDefault(EssenceType.Forbidden, 0f)
            };
        }

        /// <summary>
        /// Get total essence count across all types
        /// </summary>
        public float GetTotalEssence()
        {
            return vitalityEssence + strengthEssence + arcaneEssence + forbiddenEssence;
        }

        /// <summary>
        /// Check if any essence is available
        /// </summary>
        public bool HasAnyEssence()
        {
            return GetTotalEssence() > 0f;
        }
    }
} 