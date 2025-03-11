using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    // Singleton pattern to ensure there's only one instance
    private static AudioSettings _instance;
    public static AudioSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject audioSettingsObj = new GameObject("AudioSettings");
                _instance = audioSettingsObj.AddComponent<AudioSettings>();
                DontDestroyOnLoad(audioSettingsObj);
            }
            return _instance;
        }
    }

    // Audio mixer reference - assign this in the inspector
    public AudioMixer audioMixer;

    // Volume values
    private float masterVolume = 1.0f;
    private float musicVolume = 1.0f;
    private float sfxVolume = 1.0f;

    // Audio mixer parameter names
    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    private void Awake()
    {
        // Ensure singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load saved settings if available
        LoadSettings();
    }

    public void InitializeUI(VisualElement root)
    {
        var audioPanel = root.Q<VisualElement>("audioPanel");
        if (audioPanel == null)
        {
            Debug.LogError("Audio panel not found in UI");
            return;
        }

        // Get references to sliders
        var masterSlider = audioPanel.Q<Slider>("masterVolumeSlider");
        var musicSlider = audioPanel.Q<Slider>("musicVolumeSlider");
        var sfxSlider = audioPanel.Q<Slider>("sfxVolumeSlider");
        
        // Set up master volume slider
        if (masterSlider != null)
        {
            masterSlider.value = masterVolume * 100; // Convert to 0-100 range
            masterSlider.RegisterValueChangedCallback(evt => 
            {
                // Apply and save the volume change immediately
                SetMasterVolume(evt.newValue / 100f);
                SaveSettings();
            });
        }

        // Set up music volume slider
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume * 100;
            musicSlider.RegisterValueChangedCallback(evt =>
            {
                // Apply and save the volume change immediately
                SetMusicVolume(evt.newValue / 100f);
                SaveSettings();
            });
        }

        // Set up SFX volume slider
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume * 100;
            sfxSlider.RegisterValueChangedCallback(evt =>
            {
                // Apply and save the volume change immediately
                SetSFXVolume(evt.newValue / 100f);
                SaveSettings();
            });
        }
    }

    public void SetMasterVolume(float volume)
    {
        // Convert linear volume (0-1) to logarithmic (-80dB-0dB)
        float dbVolume = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        if (audioMixer != null)
        {
            audioMixer.SetFloat(MASTER_VOLUME, dbVolume);
        }
        masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        float dbVolume = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        if (audioMixer != null)
        {
            audioMixer.SetFloat(MUSIC_VOLUME, dbVolume);
        }
        musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        float dbVolume = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        if (audioMixer != null)
        {
            audioMixer.SetFloat(SFX_VOLUME, dbVolume);
        }
        sfxVolume = volume;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetMasterVolume(masterVolume);
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume(musicVolume);
        }
        
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume(sfxVolume);
        }
    }
    
    public void ShowAudioPanel(VisualElement root)
    {
        // Get the audio panel
        var audioPanel = root.Q<VisualElement>("audioPanel");
        if (audioPanel != null)
        {
            // Initialize UI components
            InitializeUI(root);
        }
        else
        {
            Debug.LogError("Audio panel not found in UI");
        }
    }
}
