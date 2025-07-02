using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoulBound.Core
{
    /// <summary>
    /// Audio management system for SoulBound RPG
    /// Handles background music, sound effects, and audio settings
    /// Supports dynamic music layering and context-aware audio
    /// </summary>
    public class AudioManager : BaseManager
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _ambientSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _uiSource;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixerGroup _masterMixerGroup;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        [SerializeField] private AudioMixerGroup _uiMixerGroup;

        [Header("Volume Settings")]
        [SerializeField, Range(0f, 1f)] private float _masterVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float _musicVolume = 0.7f;
        [SerializeField, Range(0f, 1f)] private float _sfxVolume = 0.8f;
        [SerializeField, Range(0f, 1f)] private float _uiVolume = 0.9f;

        [Header("Music Settings")]
        [SerializeField] private float _musicFadeDuration = 2f;
        [SerializeField] private bool _loopMusic = true;

        // Audio clip pools for SFX
        private Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _musicClips = new Dictionary<string, AudioClip>();
        
        // Current playing music
        private AudioClip _currentMusicClip;
        private Coroutine _musicFadeCoroutine;

        // Properties
        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;
        public float UiVolume => _uiVolume;
        public bool IsMusicPlaying => _musicSource != null && _musicSource.isPlaying;

        protected override void OnInitialize()
        {
            LogInfo("Setting up audio system");
            
            // Create audio sources if they don't exist
            CreateAudioSources();
            
            // Set up mixer groups
            SetupMixerGroups();
            
            // Apply volume settings
            ApplyVolumeSettings();
            
            // Load audio resources
            LoadAudioResources();
        }

        protected override void OnCleanup()
        {
            // Stop all audio
            StopAllAudio();
            
            // Clear audio dictionaries
            _sfxClips.Clear();
            _musicClips.Clear();
        }

        #region Audio Source Setup

        /// <summary>
        /// Create audio sources if they don't exist
        /// </summary>
        private void CreateAudioSources()
        {
            if (_musicSource == null)
            {
                _musicSource = CreateAudioSource("MusicSource");
                _musicSource.loop = _loopMusic;
                _musicSource.playOnAwake = false;
            }

            if (_ambientSource == null)
            {
                _ambientSource = CreateAudioSource("AmbientSource");
                _ambientSource.loop = true;
                _ambientSource.playOnAwake = false;
            }

            if (_sfxSource == null)
            {
                _sfxSource = CreateAudioSource("SFXSource");
                _sfxSource.loop = false;
                _sfxSource.playOnAwake = false;
            }

            if (_uiSource == null)
            {
                _uiSource = CreateAudioSource("UISource");
                _uiSource.loop = false;
                _uiSource.playOnAwake = false;
            }
        }

        /// <summary>
        /// Create an audio source component
        /// </summary>
        /// <param name="sourceName">Name for the audio source</param>
        /// <returns>Created AudioSource component</returns>
        private AudioSource CreateAudioSource(string sourceName)
        {
            GameObject sourceObject = new GameObject(sourceName);
            sourceObject.transform.SetParent(transform);
            
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            source.volume = 1f;
            source.pitch = 1f;
            
            LogInfo($"Created audio source: {sourceName}");
            return source;
        }

        /// <summary>
        /// Set up audio mixer groups
        /// </summary>
        private void SetupMixerGroups()
        {
            if (_musicMixerGroup != null && _musicSource != null)
                _musicSource.outputAudioMixerGroup = _musicMixerGroup;

            if (_sfxMixerGroup != null && _sfxSource != null)
                _sfxSource.outputAudioMixerGroup = _sfxMixerGroup;

            if (_uiMixerGroup != null && _uiSource != null)
                _uiSource.outputAudioMixerGroup = _uiMixerGroup;

            if (_sfxMixerGroup != null && _ambientSource != null)
                _ambientSource.outputAudioMixerGroup = _sfxMixerGroup;
        }

        #endregion

        #region Volume Control

        /// <summary>
        /// Set master volume
        /// </summary>
        /// <param name="volume">Volume value (0-1)</param>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            ApplyVolumeSettings();
            LogInfo($"Master volume set to: {_masterVolume:F2}");
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        /// <param name="volume">Volume value (0-1)</param>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            if (_musicSource != null)
                _musicSource.volume = _musicVolume * _masterVolume;
            LogInfo($"Music volume set to: {_musicVolume:F2}");
        }

        /// <summary>
        /// Set SFX volume
        /// </summary>
        /// <param name="volume">Volume value (0-1)</param>
        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            if (_sfxSource != null)
                _sfxSource.volume = _sfxVolume * _masterVolume;
            if (_ambientSource != null)
                _ambientSource.volume = _sfxVolume * _masterVolume;
            LogInfo($"SFX volume set to: {_sfxVolume:F2}");
        }

        /// <summary>
        /// Set UI volume
        /// </summary>
        /// <param name="volume">Volume value (0-1)</param>
        public void SetUiVolume(float volume)
        {
            _uiVolume = Mathf.Clamp01(volume);
            if (_uiSource != null)
                _uiSource.volume = _uiVolume * _masterVolume;
            LogInfo($"UI volume set to: {_uiVolume:F2}");
        }

        /// <summary>
        /// Apply all volume settings to audio sources
        /// </summary>
        private void ApplyVolumeSettings()
        {
            if (_musicSource != null)
                _musicSource.volume = _musicVolume * _masterVolume;

            if (_sfxSource != null)
                _sfxSource.volume = _sfxVolume * _masterVolume;

            if (_ambientSource != null)
                _ambientSource.volume = _sfxVolume * _masterVolume;

            if (_uiSource != null)
                _uiSource.volume = _uiVolume * _masterVolume;
        }

        #endregion

        #region Music Control

        /// <summary>
        /// Play background music with optional fade
        /// </summary>
        /// <param name="musicClip">Music clip to play</param>
        /// <param name="fadeIn">Whether to fade in the music</param>
        public void PlayMusic(AudioClip musicClip, bool fadeIn = true)
        {
            if (musicClip == null)
            {
                LogError("Attempted to play null music clip");
                return;
            }

            if (_musicFadeCoroutine != null)
            {
                StopCoroutine(_musicFadeCoroutine);
            }

            _currentMusicClip = musicClip;

            if (fadeIn && _musicSource.isPlaying)
            {
                _musicFadeCoroutine = StartCoroutine(FadeToNewMusic(musicClip));
            }
            else
            {
                _musicSource.clip = musicClip;
                _musicSource.Play();
                LogInfo($"Playing music: {musicClip.name}");
            }
        }

        /// <summary>
        /// Play music by name from loaded clips
        /// </summary>
        /// <param name="musicName">Name of the music clip</param>
        /// <param name="fadeIn">Whether to fade in the music</param>
        public void PlayMusic(string musicName, bool fadeIn = true)
        {
            if (_musicClips.TryGetValue(musicName, out AudioClip clip))
            {
                PlayMusic(clip, fadeIn);
            }
            else
            {
                LogError($"Music clip not found: {musicName}");
            }
        }

        /// <summary>
        /// Stop background music with optional fade
        /// </summary>
        /// <param name="fadeOut">Whether to fade out the music</param>
        public void StopMusic(bool fadeOut = true)
        {
            if (_musicFadeCoroutine != null)
            {
                StopCoroutine(_musicFadeCoroutine);
            }

            if (fadeOut)
            {
                _musicFadeCoroutine = StartCoroutine(FadeOutMusic());
            }
            else
            {
                _musicSource.Stop();
                LogInfo("Music stopped");
            }
        }

        /// <summary>
        /// Fade to new music
        /// </summary>
        /// <param name="newClip">New music clip to fade to</param>
        private IEnumerator FadeToNewMusic(AudioClip newClip)
        {
            // Fade out current music
            float startVolume = _musicSource.volume;
            for (float t = 0; t < _musicFadeDuration; t += Time.unscaledDeltaTime)
            {
                _musicSource.volume = Mathf.Lerp(startVolume, 0, t / _musicFadeDuration);
                yield return null;
            }

            // Switch to new clip
            _musicSource.clip = newClip;
            _musicSource.Play();

            // Fade in new music
            float targetVolume = _musicVolume * _masterVolume;
            for (float t = 0; t < _musicFadeDuration; t += Time.unscaledDeltaTime)
            {
                _musicSource.volume = Mathf.Lerp(0, targetVolume, t / _musicFadeDuration);
                yield return null;
            }

            _musicSource.volume = targetVolume;
            LogInfo($"Faded to music: {newClip.name}");
        }

        /// <summary>
        /// Fade out current music
        /// </summary>
        private IEnumerator FadeOutMusic()
        {
            float startVolume = _musicSource.volume;
            for (float t = 0; t < _musicFadeDuration; t += Time.unscaledDeltaTime)
            {
                _musicSource.volume = Mathf.Lerp(startVolume, 0, t / _musicFadeDuration);
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.volume = startVolume;
            LogInfo("Music faded out");
        }

        #endregion

        #region Sound Effects

        /// <summary>
        /// Play a one-shot sound effect
        /// </summary>
        /// <param name="sfxClip">Sound effect clip to play</param>
        /// <param name="volumeScale">Volume scale multiplier</param>
        public void PlaySFX(AudioClip sfxClip, float volumeScale = 1f)
        {
            if (sfxClip == null)
            {
                LogError("Attempted to play null SFX clip");
                return;
            }

            _sfxSource.PlayOneShot(sfxClip, volumeScale);
        }

        /// <summary>
        /// Play sound effect by name
        /// </summary>
        /// <param name="sfxName">Name of the SFX clip</param>
        /// <param name="volumeScale">Volume scale multiplier</param>
        public void PlaySFX(string sfxName, float volumeScale = 1f)
        {
            if (_sfxClips.TryGetValue(sfxName, out AudioClip clip))
            {
                PlaySFX(clip, volumeScale);
            }
            else
            {
                LogError($"SFX clip not found: {sfxName}");
            }
        }

        /// <summary>
        /// Play UI sound effect
        /// </summary>
        /// <param name="uiClip">UI sound clip to play</param>
        /// <param name="volumeScale">Volume scale multiplier</param>
        public void PlayUISFX(AudioClip uiClip, float volumeScale = 1f)
        {
            if (uiClip == null)
            {
                LogError("Attempted to play null UI SFX clip");
                return;
            }

            _uiSource.PlayOneShot(uiClip, volumeScale);
        }

        #endregion

        #region Audio Resource Management

        /// <summary>
        /// Load audio resources from Resources folder
        /// Override this method to implement custom loading logic
        /// </summary>
        protected virtual void LoadAudioResources()
        {
            LogInfo("Loading audio resources...");
            
            // Load music clips
            AudioClip[] musicClips = Resources.LoadAll<AudioClip>("Audio/Music");
            foreach (var clip in musicClips)
            {
                _musicClips[clip.name] = clip;
            }

            // Load SFX clips
            AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");
            foreach (var clip in sfxClips)
            {
                _sfxClips[clip.name] = clip;
            }

            LogInfo($"Loaded {_musicClips.Count} music clips and {_sfxClips.Count} SFX clips");
        }

        /// <summary>
        /// Register an audio clip manually
        /// </summary>
        /// <param name="clipName">Name to register the clip under</param>
        /// <param name="clip">Audio clip to register</param>
        /// <param name="isMusic">True if this is a music clip, false for SFX</param>
        public void RegisterAudioClip(string clipName, AudioClip clip, bool isMusic = false)
        {
            if (isMusic)
            {
                _musicClips[clipName] = clip;
            }
            else
            {
                _sfxClips[clipName] = clip;
            }

            LogInfo($"Registered {(isMusic ? "music" : "SFX")} clip: {clipName}");
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Stop all audio
        /// </summary>
        public void StopAllAudio()
        {
            _musicSource?.Stop();
            _ambientSource?.Stop();
            _sfxSource?.Stop();
            _uiSource?.Stop();

            LogInfo("All audio stopped");
        }

        /// <summary>
        /// Mute/unmute all audio
        /// </summary>
        /// <param name="mute">True to mute, false to unmute</param>
        public void SetMuteAll(bool mute)
        {
            AudioListener.pause = mute;
            LogInfo($"Audio {(mute ? "muted" : "unmuted")}");
        }

        #endregion

        #region Debug

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Master Volume: {_masterVolume:F2}");
            info.AppendLine($"Music Volume: {_musicVolume:F2}");
            info.AppendLine($"SFX Volume: {_sfxVolume:F2}");
            info.AppendLine($"UI Volume: {_uiVolume:F2}");
            info.AppendLine($"Music Playing: {IsMusicPlaying}");
            info.AppendLine($"Current Music: {(_currentMusicClip?.name ?? "None")}");
            info.AppendLine($"Loaded Music Clips: {_musicClips.Count}");
            info.AppendLine($"Loaded SFX Clips: {_sfxClips.Count}");
        }

        #endregion
    }
} 