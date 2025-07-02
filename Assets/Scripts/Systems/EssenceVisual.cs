using System.Collections;
using UnityEngine;

namespace SoulBound.Systems
{
    /// <summary>
    /// Visual component for essence drops that provides particle effects and visual feedback
    /// Used by EssenceManager to show essence collection opportunities
    /// </summary>
    public class EssenceVisual : MonoBehaviour
    {
        [Header("Visual Components")]
        [SerializeField] private ParticleSystem _mainParticles;
        [SerializeField] private ParticleSystem _absorptionEffect;
        [SerializeField] private Light _essenceLight;
        [SerializeField] private MeshRenderer _essenceCore;

        [Header("Animation Settings")]
        [SerializeField] private float _floatHeight = 0.5f;
        [SerializeField] private float _floatSpeed = 2f;
        [SerializeField] private float _pulseIntensity = 0.3f;
        [SerializeField] private float _pulseSpeed = 3f;
        [SerializeField] private float _absorptionDuration = 1f;

        [Header("Timer Visual")]
        [SerializeField] private GameObject _timerRing;
        [SerializeField] private Material _timerMaterial;

        // Internal state
        private SoulEssence _essence;
        private float _timeWindow;
        private float _timeRemaining;
        private Vector3 _basePosition;
        private EssenceDisplayInfo _displayInfo;
        private bool _isAbsorbing = false;
        private Material _timerMatInstance;

        private void Awake()
        {
            _basePosition = transform.position;
            
            // Create timer material instance
            if (_timerMaterial != null && _timerRing != null)
            {
                _timerMatInstance = Instantiate(_timerMaterial);
                _timerRing.GetComponent<MeshRenderer>().material = _timerMatInstance;
            }
        }

        private void Update()
        {
            if (_isAbsorbing) return;

            AnimateFloating();
            AnimatePulsing();
            UpdateTimerVisual();
        }

        /// <summary>
        /// Initialize the visual with essence information
        /// </summary>
        public void Initialize(SoulEssence essence, float timeWindow)
        {
            _essence = essence;
            _timeWindow = timeWindow;
            _timeRemaining = timeWindow;
            _displayInfo = essence.GetDisplayInfo();
            
            ConfigureVisualForEssenceType();
            StartCoroutine(TimerCoroutine());
        }

        /// <summary>
        /// Configure visual appearance based on essence type
        /// </summary>
        private void ConfigureVisualForEssenceType()
        {
            Color essenceColor = _displayInfo.typeColor;
            
            // Configure main particle system
            if (_mainParticles != null)
            {
                var main = _mainParticles.main;
                main.startColor = essenceColor;
                
                var emission = _mainParticles.emission;
                emission.rateOverTime = _essence.Quantity * 10f; // More particles for higher quantity
                
                _mainParticles.Play();
            }

            // Configure light
            if (_essenceLight != null)
            {
                _essenceLight.color = essenceColor;
                _essenceLight.intensity = _essence.Potency * 2f;
                _essenceLight.range = 3f + _essence.Quantity * 0.5f;
            }

            // Configure core material
            if (_essenceCore != null)
            {
                var material = _essenceCore.material;
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", essenceColor * _essence.Potency);
                }
                if (material.HasProperty("_Color"))
                {
                    material.color = essenceColor;
                }
            }

            // Show corruption warning for dangerous essence
            if (_essence.TotalCorruptionRisk > 0.1f)
            {
                AddCorruptionWarningEffects();
            }
        }

        /// <summary>
        /// Add visual warning effects for corrupted essence
        /// </summary>
        private void AddCorruptionWarningEffects()
        {
            // Add red warning particles or effects
            if (_mainParticles != null)
            {
                var velocityOverLifetime = _mainParticles.velocityOverLifetime;
                velocityOverLifetime.enabled = true;
                velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
                velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f);
            }

            // Make light flicker for corruption warning
            if (_essenceLight != null)
            {
                StartCoroutine(CorruptionFlickerEffect());
            }
        }

        /// <summary>
        /// Animate the essence floating up and down
        /// </summary>
        private void AnimateFloating()
        {
            float yOffset = Mathf.Sin(Time.time * _floatSpeed) * _floatHeight;
            transform.position = _basePosition + Vector3.up * yOffset;
        }

        /// <summary>
        /// Animate pulsing effect
        /// </summary>
        private void AnimatePulsing()
        {
            if (_essenceLight != null)
            {
                float pulse = 1f + Mathf.Sin(Time.time * _pulseSpeed) * _pulseIntensity;
                _essenceLight.intensity = _essence.Potency * 2f * pulse;
            }
        }

        /// <summary>
        /// Update the timer ring visual
        /// </summary>
        private void UpdateTimerVisual()
        {
            if (_timerMatInstance != null && _timeWindow > 0)
            {
                float progress = 1f - (_timeRemaining / _timeWindow);
                _timerMatInstance.SetFloat("_Progress", progress);
                
                // Color the timer based on urgency
                Color timerColor = Color.Lerp(Color.green, Color.red, progress);
                _timerMatInstance.SetColor("_Color", timerColor);
            }
        }

        /// <summary>
        /// Timer coroutine that updates remaining time
        /// </summary>
        private IEnumerator TimerCoroutine()
        {
            while (_timeRemaining > 0 && !_isAbsorbing)
            {
                _timeRemaining -= Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Corruption flicker effect for dangerous essence
        /// </summary>
        private IEnumerator CorruptionFlickerEffect()
        {
            Color originalColor = _essenceLight.color;
            
            while (!_isAbsorbing && _essence.TotalCorruptionRisk > 0.1f)
            {
                // Flicker to red occasionally
                if (Random.value < 0.1f)
                {
                    _essenceLight.color = Color.red;
                    yield return new WaitForSeconds(0.1f);
                    _essenceLight.color = originalColor;
                }
                
                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// Play absorption effect and destroy the visual
        /// </summary>
        public void PlayAbsorptionEffect()
        {
            if (_isAbsorbing) return;
            
            _isAbsorbing = true;
            StartCoroutine(AbsorptionEffectCoroutine());
        }

        /// <summary>
        /// Absorption effect coroutine
        /// </summary>
        private IEnumerator AbsorptionEffectCoroutine()
        {
            // Stop main particles
            if (_mainParticles != null)
            {
                var emission = _mainParticles.emission;
                emission.enabled = false;
            }

            // Play absorption particle effect
            if (_absorptionEffect != null)
            {
                _absorptionEffect.Play();
            }

            // Animate shrinking and light dimming
            Vector3 originalScale = transform.localScale;
            float originalLightIntensity = _essenceLight != null ? _essenceLight.intensity : 0f;
            
            float elapsedTime = 0f;
            while (elapsedTime < _absorptionDuration)
            {
                float progress = elapsedTime / _absorptionDuration;
                
                // Shrink the visual
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
                
                // Dim the light
                if (_essenceLight != null)
                {
                    _essenceLight.intensity = Mathf.Lerp(originalLightIntensity, 0f, progress);
                }
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Destroy the visual
            Destroy(gameObject);
        }

        /// <summary>
        /// Get the current time remaining for this essence
        /// </summary>
        public float GetTimeRemaining()
        {
            return _timeRemaining;
        }

        /// <summary>
        /// Check if this essence is still available for collection
        /// </summary>
        public bool IsAvailable()
        {
            return _timeRemaining > 0 && !_isAbsorbing;
        }

        /// <summary>
        /// Get the essence information
        /// </summary>
        public SoulEssence GetEssence()
        {
            return _essence;
        }

        private void OnDestroy()
        {
            // Clean up material instance
            if (_timerMatInstance != null)
            {
                Destroy(_timerMatInstance);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw collection range
            Gizmos.color = _displayInfo.typeColor;
            Gizmos.DrawWireSphere(transform.position, 1f);
            
            // Draw timer indicator
            if (_timeWindow > 0)
            {
                float timerHeight = (_timeRemaining / _timeWindow) * 2f;
                Gizmos.color = Color.Lerp(Color.red, Color.green, _timeRemaining / _timeWindow);
                Gizmos.DrawLine(transform.position + Vector3.up * 1.5f, 
                               transform.position + Vector3.up * (1.5f + timerHeight));
            }
        }
    }
} 