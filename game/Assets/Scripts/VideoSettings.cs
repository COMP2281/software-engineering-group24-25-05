using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class VideoSettings : MonoBehaviour
{
    // Singleton pattern to ensure there's only one instance
    private static VideoSettings _instance;
    public static VideoSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject videoSettingsObj = new GameObject("VideoSettings");
                _instance = videoSettingsObj.AddComponent<VideoSettings>();
                DontDestroyOnLoad(videoSettingsObj);
            }
            return _instance;
        }
    }

    // Class to hold resolution information
    [Serializable]
    public class Resolution
    {
        public int width;
        public int height;
        
        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        
        public override string ToString()
        {
            return $"{width} x {height}";
        }
    }
    
    private List<Resolution> resolutions = new List<Resolution>
    {
        new Resolution(1280, 720),
        new Resolution(1366, 768),
        new Resolution(1600, 900),
        new Resolution(1920, 1080),
        new Resolution(2560, 1440),
        new Resolution(3840, 2160)
    };
    
    private Resolution currentResolution;
    private bool isFullscreen;
    
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
        
        // Initialize with current settings
        isFullscreen = Screen.fullScreen;
        currentResolution = new Resolution(Screen.width, Screen.height);
        
        // Load saved settings if available
        LoadSettings();
    }
    
    public void InitializeUI(VisualElement root)
    {
        var videoPanel = root.Q<VisualElement>("videoPanel");
        if (videoPanel == null)
        {
            Debug.LogError("Video panel not found in UI");
            return;
        }
        
        var resolutionDropdown = videoPanel.Q<DropdownField>("resolutionDropdown");
        var fullscreenToggle = videoPanel.Q<Toggle>("fullscreenToggle");
        
        if (resolutionDropdown != null)
        {
            // Populate resolution options
            List<string> options = new List<string>();
            foreach (var resolution in resolutions)
            {
                options.Add(resolution.ToString());
            }
            
            resolutionDropdown.choices = options;
            
            // Set current resolution in dropdown
            string currentResString = $"{currentResolution.width} x {currentResolution.height}";
            int currentIndex = options.IndexOf(currentResString);
            
            if (currentIndex >= 0)
            {
                resolutionDropdown.index = currentIndex;
            }
            else
            {
                // If current resolution isn't in our list, add it
                resolutions.Add(currentResolution);
                options.Add(currentResString);
                resolutionDropdown.choices = options;
                resolutionDropdown.index = options.Count - 1;
            }
            
            // Register event to apply settings when resolution changes
            resolutionDropdown.RegisterValueChangedCallback(evt => ApplySettings(resolutionDropdown, fullscreenToggle));
        }
        
        if (fullscreenToggle != null)
        {
            fullscreenToggle.value = isFullscreen;
            
            // Register event to apply settings when fullscreen toggle changes
            fullscreenToggle.RegisterValueChangedCallback(evt => ApplySettings(resolutionDropdown, fullscreenToggle));
        }
    }
    
    private void ApplySettings(DropdownField resolutionDropdown, Toggle fullscreenToggle)
    {
        if (resolutionDropdown == null || fullscreenToggle == null)
        {
            Debug.LogError("UI elements not found");
            return;
        }
        
        // Get selected resolution
        int selectedIndex = resolutionDropdown.index;
        if (selectedIndex >= 0 && selectedIndex < resolutions.Count)
        {
            Resolution selectedResolution = resolutions[selectedIndex];
            bool fullscreenSelected = fullscreenToggle.value;
            
            // Apply settings
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenSelected);
            
            // Update current settings
            currentResolution = selectedResolution;
            isFullscreen = fullscreenSelected;
            
            // Save settings
            SaveSettings();
        }
    }
    
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionWidth", currentResolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", currentResolution.height);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            int width = PlayerPrefs.GetInt("ResolutionWidth");
            int height = PlayerPrefs.GetInt("ResolutionHeight");
            currentResolution = new Resolution(width, height);
        }
        
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        
        // Apply loaded settings
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen);
    }
    
    public void ShowVideoPanel(VisualElement root)
    {
        // Get the video panel
        var videoPanel = root.Q<VisualElement>("videoPanel");
        if (videoPanel != null)
        {
            // Initialize UI components
            InitializeUI(root);
        }
        else
        {
            Debug.LogError("Video panel not found in UI");
        }
    }
}
