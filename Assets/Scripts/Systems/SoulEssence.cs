using System;
using UnityEngine;

namespace SoulBound.Systems
{
    /// <summary>
    /// Types of soul essence that can be collected and used in the soul-binding system
    /// Each type has different effects and applications
    /// </summary>
    public enum EssenceType
    {
        Vitality,   // Health and life force - immediate healing, health upgrades
        Strength,   // Physical power - damage enhancement, physical abilities
        Arcane,     // Magical energy - mana restoration, spell enhancement
        Forbidden   // Corrupted essence - powerful but increases corruption
    }

    /// <summary>
    /// Represents a collectible soul essence with specific properties and effects
    /// Dropped by defeated enemies and environmental sources
    /// </summary>
    [System.Serializable]
    public class SoulEssence
    {
        [Header("Essence Properties")]
        [SerializeField] private EssenceType _type;
        [SerializeField] private float _quantity;
        [SerializeField] private float _potency;
        [SerializeField] private float _corruptionRisk;

        [Header("Absorption Settings")]
        [SerializeField] private float _absorptionRate = 1.0f;
        [SerializeField] private float _baseValue = 10f;
        [SerializeField] private bool _requiresPlayerChoice = false;

        // Properties
        public EssenceType Type => _type;
        public float Quantity => _quantity;
        public float Potency => _potency;
        public float CorruptionRisk => _corruptionRisk;
        public float AbsorptionRate => _absorptionRate;
        public float BaseValue => _baseValue;
        public bool RequiresPlayerChoice => _requiresPlayerChoice;

        // Calculated properties
        public float EffectiveValue => _quantity * _potency * _absorptionRate;
        public bool IsForbidden => _type == EssenceType.Forbidden;
        public float TotalCorruptionRisk => _corruptionRisk * _quantity;

        /// <summary>
        /// Create a new soul essence with specified properties
        /// </summary>
        public SoulEssence(EssenceType type, float quantity, float potency = 1.0f, float corruptionRisk = 0f)
        {
            _type = type;
            _quantity = quantity;
            _potency = potency;
            _corruptionRisk = corruptionRisk;
            _absorptionRate = 1.0f;
            _baseValue = GetBaseValueForType(type);
            _requiresPlayerChoice = IsForbidden || _corruptionRisk > 0.1f;
        }

        /// <summary>
        /// Create essence from an enemy or source configuration
        /// </summary>
        public static SoulEssence CreateFromSource(EssenceType type, int sourceLevel, float qualityMultiplier = 1.0f)
        {
            var config = GetEssenceConfig(type);
            float quantity = config.baseQuantity * (1 + sourceLevel * 0.1f) * qualityMultiplier;
            float potency = config.basePotency * (1 + sourceLevel * 0.05f);
            float corruptionRisk = config.baseCorruption * qualityMultiplier;

            return new SoulEssence(type, quantity, potency, corruptionRisk);
        }

        /// <summary>
        /// Get the immediate consumption effects of this essence
        /// </summary>
        public EssenceEffects GetImmediateEffects()
        {
            float effectValue = EffectiveValue;
            
            return _type switch
            {
                EssenceType.Vitality => new EssenceEffects
                {
                    healthRestore = effectValue * 0.8f,
                    manaRestore = effectValue * 0.2f,
                    corruption = TotalCorruptionRisk
                },
                EssenceType.Strength => new EssenceEffects
                {
                    healthRestore = effectValue * 0.4f,
                    experienceGain = effectValue * 0.3f,
                    corruption = TotalCorruptionRisk
                },
                EssenceType.Arcane => new EssenceEffects
                {
                    manaRestore = effectValue * 0.8f,
                    experienceGain = effectValue * 0.2f,
                    corruption = TotalCorruptionRisk
                },
                EssenceType.Forbidden => new EssenceEffects
                {
                    healthRestore = effectValue * 0.6f,
                    manaRestore = effectValue * 0.6f,
                    experienceGain = effectValue * 0.4f,
                    corruption = TotalCorruptionRisk * 2.0f // Double corruption for forbidden
                },
                _ => new EssenceEffects { corruption = TotalCorruptionRisk }
            };
        }

        /// <summary>
        /// Get the banking value when storing this essence
        /// </summary>
        public float GetBankingValue()
        {
            // Banking preserves more value but loses some potency
            return EffectiveValue * 0.9f;
        }

        /// <summary>
        /// Scale this essence based on player progression
        /// </summary>
        public SoulEssence ScaleToPlayerLevel(int playerLevel, float absorptionBonus = 0f)
        {
            float levelMultiplier = 1.0f + (playerLevel - 1) * 0.05f;
            float newAbsorptionRate = _absorptionRate * (1.0f + absorptionBonus);
            
            var scaledEssence = new SoulEssence(_type, _quantity * levelMultiplier, _potency, _corruptionRisk)
            {
                _absorptionRate = newAbsorptionRate
            };
            
            return scaledEssence;
        }

        /// <summary>
        /// Check if this essence can be safely absorbed without significant corruption risk
        /// </summary>
        public bool IsSafeToAbsorb(float currentCorruption, float corruptionThreshold)
        {
            return currentCorruption + TotalCorruptionRisk < corruptionThreshold * 0.8f;
        }

        /// <summary>
        /// Get display information for UI
        /// </summary>
        public EssenceDisplayInfo GetDisplayInfo()
        {
            return new EssenceDisplayInfo
            {
                typeName = GetTypeDisplayName(),
                typeColor = GetTypeColor(),
                quantity = _quantity,
                potency = _potency,
                corruptionWarning = TotalCorruptionRisk > 0.1f,
                effectPreview = GetEffectPreview()
            };
        }

        /// <summary>
        /// Get a text preview of what this essence will do when consumed
        /// </summary>
        public string GetEffectPreview()
        {
            var effects = GetImmediateEffects();
            var preview = "";

            if (effects.healthRestore > 0)
                preview += $"+{effects.healthRestore:F0} Health ";
            if (effects.manaRestore > 0)
                preview += $"+{effects.manaRestore:F0} Mana ";
            if (effects.experienceGain > 0)
                preview += $"+{effects.experienceGain:F0} XP ";
            if (effects.corruption > 0)
                preview += $"⚠️ +{effects.corruption:F1} Corruption";

            return preview.Trim();
        }

        // Static configuration methods
        private static EssenceConfig GetEssenceConfig(EssenceType type)
        {
            return type switch
            {
                EssenceType.Vitality => new EssenceConfig(5f, 1.0f, 0f),
                EssenceType.Strength => new EssenceConfig(4f, 1.2f, 0.1f),
                EssenceType.Arcane => new EssenceConfig(6f, 0.9f, 0.05f),
                EssenceType.Forbidden => new EssenceConfig(8f, 1.5f, 0.5f),
                _ => new EssenceConfig(3f, 1.0f, 0f)
            };
        }

        private static float GetBaseValueForType(EssenceType type)
        {
            return type switch
            {
                EssenceType.Vitality => 10f,
                EssenceType.Strength => 12f,
                EssenceType.Arcane => 8f,
                EssenceType.Forbidden => 15f,
                _ => 5f
            };
        }

        private string GetTypeDisplayName()
        {
            return _type switch
            {
                EssenceType.Vitality => "Vitality Essence",
                EssenceType.Strength => "Strength Essence", 
                EssenceType.Arcane => "Arcane Essence",
                EssenceType.Forbidden => "Forbidden Essence",
                _ => "Unknown Essence"
            };
        }

        private Color GetTypeColor()
        {
            return _type switch
            {
                EssenceType.Vitality => Color.green,
                EssenceType.Strength => Color.red,
                EssenceType.Arcane => Color.blue,
                EssenceType.Forbidden => Color.magenta,
                _ => Color.white
            };
        }

        public override string ToString()
        {
            return $"{GetTypeDisplayName()} (Qty: {_quantity:F1}, Potency: {_potency:F1}, Risk: {TotalCorruptionRisk:F1})";
        }
    }

    /// <summary>
    /// Effects that can be applied when consuming essence
    /// </summary>
    [System.Serializable]
    public struct EssenceEffects
    {
        public float healthRestore;
        public float manaRestore;
        public float experienceGain;
        public float corruption;
    }

    /// <summary>
    /// Configuration for essence generation
    /// </summary>
    [System.Serializable]
    public struct EssenceConfig
    {
        public float baseQuantity;
        public float basePotency;
        public float baseCorruption;

        public EssenceConfig(float quantity, float potency, float corruption)
        {
            baseQuantity = quantity;
            basePotency = potency;
            baseCorruption = corruption;
        }
    }

    /// <summary>
    /// Display information for UI systems
    /// </summary>
    [System.Serializable]
    public struct EssenceDisplayInfo
    {
        public string typeName;
        public Color typeColor;
        public float quantity;
        public float potency;
        public bool corruptionWarning;
        public string effectPreview;
    }
} 