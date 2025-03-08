using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;
    private UIDocument pauseMenuDocument;
    private Button resumeButton;
    private Button settingsButton;
    private Button mainMenuButton;
    private Button exitButton;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    // Settings submenu elements
    private VisualElement settingsSubmenu;
    private Button controlsButton;
    private Button videoButton;
    private Button audioButton;
    private Button backButton;
    
    // Controls panel
    private VisualElement controlsPanel;
    
    // Reference to our shared controls component
    private ChangeControls changeControls;

    // Reference to UserInput
    private UserInput userInput;

    void Start() {
        // Get reference to UserInput singleton
        userInput = UserInput.Instance;

        // Load the UXML and USS
        pauseMenuDocument = GetComponent<UIDocument>();
        var root = pauseMenuDocument.rootVisualElement;

        // Initially hide the pause menu
        root.style.display = DisplayStyle.None;

        // Retrieve the main menu buttons
        resumeButton = root.Q<Button>("resumeButton");
        settingsButton = root.Q<Button>("settingsButton");
        mainMenuButton = root.Q<Button>("mainMenuButton");
        exitButton = root.Q<Button>("exitButton");

        // Register the button click events
        resumeButton.RegisterCallback<ClickEvent>(evt => Resume());
        settingsButton.RegisterCallback<ClickEvent>(evt => ShowSettingsMenu());
        mainMenuButton.RegisterCallback<ClickEvent>(evt => LoadScene("MainMenu"));
        exitButton.RegisterCallback<ClickEvent>(evt => Application.Quit());

        // Settings submenu setup
        settingsSubmenu = root.Q<VisualElement>("settingsSubmenu");
        if (settingsSubmenu != null) {
            settingsSubmenu.style.display = DisplayStyle.None;
            
            controlsButton = settingsSubmenu.Q<Button>("ControlsButton");
            videoButton = settingsSubmenu.Q<Button>("VideoButton");
            audioButton = settingsSubmenu.Q<Button>("AudioButton");
            backButton = settingsSubmenu.Q<Button>("BackButton");
            
            // Register settings submenu button events
            if (controlsButton != null) controlsButton.RegisterCallback<ClickEvent>(evt => ShowControlsPanel());
            if (videoButton != null) videoButton.RegisterCallback<ClickEvent>(evt => ShowVideoPanel());
            if (audioButton != null) audioButton.RegisterCallback<ClickEvent>(evt => ShowAudioPanel());
            if (backButton != null) backButton.RegisterCallback<ClickEvent>(evt => BackToMainPauseMenu());
        }
        
        // Controls panel setup
        controlsPanel = root.Q<VisualElement>("controlsPanel");
        if (controlsPanel != null) {
            controlsPanel.style.display = DisplayStyle.None;
            
            // Initialize the controls manager
            changeControls = gameObject.AddComponent<ChangeControls>();
            changeControls.Initialize(root, FindObjectOfType<PlayerInput>());
        }

        // Retrieve all buttons in the menu for general hover/click sounds
        root.Query<Button>().ForEach(button =>
        {
            _menuButtons.Add(button);
            button.RegisterCallback<ClickEvent>(OnAllButtonClick);
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        });

        // Initialize the audio source
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy() {
        if (changeControls != null) {
            Destroy(changeControls);
        }
    }

    private void Update() {
        // Use UserInput instead of direct input detection
        if(userInput.MenuOpenCloseInput) {
            if(isPaused) {
                if (settingsSubmenu != null && settingsSubmenu.style.display == DisplayStyle.Flex) {
                    // If in settings submenu, go back to main pause menu
                    BackToMainPauseMenu();
                } else if (controlsPanel != null && controlsPanel.style.display == DisplayStyle.Flex) {
                    // If in controls panel, go back to settings submenu
                    ShowSettingsMenu();
                } else {
                    // Otherwise resume the game
                    Resume();
                }
            } else {
                Pause();
            }
        }
    }

    private void Resume() {
        pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    private void Pause() {
        // Show main pause menu, hide submenus
        pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        if (settingsSubmenu != null) settingsSubmenu.style.display = DisplayStyle.None;
        if (controlsPanel != null) controlsPanel.style.display = DisplayStyle.None;
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    private void ShowSettingsMenu() {
        // Hide main pause menu elements except the root
        var mainMenuContainer = pauseMenuDocument.rootVisualElement.Q<VisualElement>("menuContainer");
        if (mainMenuContainer != null) mainMenuContainer.style.display = DisplayStyle.None;
        
        // Show settings submenu
        if (settingsSubmenu != null) {
            settingsSubmenu.style.display = DisplayStyle.Flex;
            controlsPanel.style.display = DisplayStyle.None;
        }
    }

    private void ShowControlsPanel() {
        if (controlsPanel != null) {
            controlsPanel.style.display = DisplayStyle.Flex;
            settingsSubmenu.style.display = DisplayStyle.None;
        }
    }

    private void ShowVideoPanel() {
        // For now just log that this is not implemented
        Debug.Log("Video settings not yet implemented");
    }

    private void ShowAudioPanel() {
        // For now just log that this is not implemented
        Debug.Log("Audio settings not yet implemented");
    }

    private void BackToMainPauseMenu() {
        // Hide settings submenu and controls panel
        if (settingsSubmenu != null) settingsSubmenu.style.display = DisplayStyle.None;
        if (controlsPanel != null) controlsPanel.style.display = DisplayStyle.None;
        
        // Show main pause menu
        var mainMenuContainer = pauseMenuDocument.rootVisualElement.Q<VisualElement>("menuContainer");
        if (mainMenuContainer != null) mainMenuContainer.style.display = DisplayStyle.Flex;
    }

    private void LoadScene(string sceneName) {
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    private void OnAllButtonClick(ClickEvent evt) {
        _audioSource.Play();
    }

    private void OnButtonHover(MouseEnterEvent evt) {
        _audioSource.Play();
    }
}
