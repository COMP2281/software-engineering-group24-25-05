using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

// Code from https://youtu.be/_jtj73lu2Ko?si=qFlQrmGtG8tRAwfv

public class SettingsMenuEvents : MonoBehaviour
{
    private Button backButton;
    private Button audioButton;
    private Button videoButton;
    private Button controlsButton;
    
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    // Reference to our shared controls component
    private ChangeControls changeControls;
    
    // Reference to our shared settings components
    private VideoSettings videoSettings;
    private AudioSettings audioSettings;
    
    // UI Elements
    private VisualElement controlsPanel;
    private VisualElement videoPanel;
    private VisualElement audioPanel;
    private VisualElement settingsContainer;
    private UIDocument uiDocument;
    
    void OnEnable()
    {
        // Get references to singleton settings managers
        videoSettings = VideoSettings.Instance;
        audioSettings = AudioSettings.Instance;
        
        // Load the UXML and USS
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Retrieve the buttons by their name
        backButton = root.Q<Button>("BackButton");
        audioButton = root.Q<Button>("AudioButton");
        videoButton = root.Q<Button>("VideoButton");
        controlsButton = root.Q<Button>("ControlsButton");

        // Get references to panels
        settingsContainer = root.Q<VisualElement>("settingsContainer");
        controlsPanel = root.Q<VisualElement>("controlsPanel");
        videoPanel = root.Q<VisualElement>("videoPanel");
        audioPanel = root.Q<VisualElement>("audioPanel");

        // Register the button click events
        backButton.RegisterCallback<ClickEvent>(evt => LoadScene("MainMenu"));
        controlsButton.RegisterCallback<ClickEvent>(evt => ShowControlsPanel());
        videoButton.RegisterCallback<ClickEvent>(evt => ShowVideoPanel());
        audioButton.RegisterCallback<ClickEvent>(evt => ShowAudioPanel());
        
        // Initialize the controls manager
        changeControls = gameObject.AddComponent<ChangeControls>();
        changeControls.Initialize(root, FindObjectOfType<PlayerInput>());

        // Retrieve all buttons in the menu
        root.Query<Button>().ForEach(button =>
        {
            _menuButtons.Add(button);
            button.RegisterCallback<ClickEvent>(OnAllButtonClick);
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        });

        // Initialize the audio source
        _audioSource = GetComponent<AudioSource>();

        // Set ControlsButton as active at the start
        ShowControlsPanel();
    }
    
    private void OnDestroy()
    {
        if (changeControls != null)
        {
            Destroy(changeControls);
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnButtonHover(MouseEnterEvent evt)
    {
        _audioSource.Play();
    }
    
    // Function to display the controls panel
    private void ShowControlsPanel()
    {
        // Reset all button styles
        ResetButtonStyles();
        controlsButton.AddToClassList("active");
        
        // Hide all panels first
        HideAllPanels();
        
        // Show controls panel
        if (controlsPanel != null)
        {
            controlsPanel.style.display = DisplayStyle.Flex;
        }
    }
    
    // Function to display the video settings panel
    private void ShowVideoPanel()
    {
        // Reset all button styles
        ResetButtonStyles();
        videoButton.AddToClassList("active");
        
        // Hide all panels first
        HideAllPanels();
        
        // Show video panel
        if (videoPanel != null)
        {
            videoPanel.style.display = DisplayStyle.Flex;
            
            // Initialize video settings if not already done
            if (videoSettings != null)
            {
                videoSettings.InitializeUI(uiDocument.rootVisualElement);
            }
        }
    }
    
    // Function to display the audio settings panel
    private void ShowAudioPanel()
    {
        // Reset all button styles
        ResetButtonStyles();
        audioButton.AddToClassList("active");
        
        // Hide all panels first
        HideAllPanels();
        
        // Show audio panel
        if (audioPanel != null)
        {
            audioPanel.style.display = DisplayStyle.Flex;
            
            // Initialize audio settings
            if (audioSettings != null)
            {
                audioSettings.InitializeUI(uiDocument.rootVisualElement);
            }
        }
        else
        {
            Debug.Log("Audio settings panel not found in UI");
        }
    }
    
    // Reset the style of all navigation buttons
    private void ResetButtonStyles()
    {
        controlsButton.RemoveFromClassList("active");
        videoButton.RemoveFromClassList("active");
        audioButton.RemoveFromClassList("active");
    }
    
    // Hide all content panels
    private void HideAllPanels()
    {
        if (controlsPanel != null)
        {
            controlsPanel.style.display = DisplayStyle.None;
        }
        
        if (videoPanel != null)
        {
            videoPanel.style.display = DisplayStyle.None;
        }
        
        if (audioPanel != null)
        {
            audioPanel.style.display = DisplayStyle.None;
        }
    }
}
