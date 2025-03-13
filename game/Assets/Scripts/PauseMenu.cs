using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;
    private bool isTransitioning = false;
    private Coroutine hideTransitionCoroutine = null;
    private UIDocument pauseMenuDocument;
    private VisualElement pauseMenu;
    private Button resumeButton;
    private Button settingsButton;
    private Button mainMenuButton;
    private Button exitButton;
    private List<Button> _menuButtons = new List<Button>();
    private List<Button> _backButtons = new List<Button>();
    private AudioSource _audioSource;
    
    // Settings submenu elements
    private VisualElement settingsSubmenu;
    private Button controlsButton;
    private Button videoButton;
    private Button audioButton;
    
    // Settings panels
    private VisualElement mainMenuContainer;
    private VisualElement controlsPanel;
    private VisualElement videoPanel;
    private VisualElement audioPanel;
    
    // Reference to our shared components
    private ChangeControls changeControls;
    private VideoSettings videoSettings;
    private AudioSettings audioSettings;

    // Reference to UserInput
    private UserInput userInput;

    void Start() {
        // Get reference to UserInput singleton
        userInput = UserInput.Instance;
        
        // Get references to settings singletons
        videoSettings = VideoSettings.Instance;
        audioSettings = AudioSettings.Instance;

        // Load the UXML and USS
        pauseMenuDocument = GetComponent<UIDocument>();
        var root = pauseMenuDocument.rootVisualElement;

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
            
            // Register settings submenu button events
            if (controlsButton != null) controlsButton.RegisterCallback<ClickEvent>(evt => ShowControlsPanel());
            if (videoButton != null) videoButton.RegisterCallback<ClickEvent>(evt => ShowVideoPanel());
            if (audioButton != null) audioButton.RegisterCallback<ClickEvent>(evt => ShowAudioPanel());
        }
        
        // Settings panels setup
        mainMenuContainer = root.Q<VisualElement>("menuContainer");
        controlsPanel = root.Q<VisualElement>("controlsPanel");
        videoPanel = root.Q<VisualElement>("videoPanel");
        audioPanel = root.Q<VisualElement>("audioPanel");
        pauseMenu = root.Q<VisualElement>("pauseMenu");
        
        // Initially hide the pause menu
        root.style.display = DisplayStyle.None;
        pauseMenu.AddToClassList("pause-menu-hidden");

        // Initialize with all panels hidden
        HideAllPanels();
        
        // Initialize the controls manager
        if (controlsPanel != null) {
            changeControls = gameObject.AddComponent<ChangeControls>();
            changeControls.Initialize(root, FindObjectOfType<PlayerInput>());
        }

        // Retrieve all back buttons
        root.Query<Button>("backButton").ForEach(button =>
        {
            _backButtons.Add(button);
            button.RegisterCallback<ClickEvent>(evt => GoBack());
        });

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

    private void Update() {
        // Use UserInput instead of direct input detection
        if(userInput.MenuOpenCloseInput) {
            if(isPaused) {
                GoBack();
            } else {
                Pause();
            }
        }
    }

    private void GoBack() {
        if (settingsSubmenu.style.display == DisplayStyle.Flex) {
            BackToMainPauseMenu();
        } else if (controlsPanel.style.display == DisplayStyle.Flex) {
            ShowSettingsMenu();
        } else if (videoPanel.style.display == DisplayStyle.Flex) {
            ShowSettingsMenu();
        } else if (audioPanel.style.display == DisplayStyle.Flex) {
            ShowSettingsMenu();
        } else {
            Resume();
        }
    }

    private void Resume() {
        // Add transition class first
        pauseMenu.AddToClassList("pause-menu-hidden");
        Time.timeScale = 1.0f;
        isPaused = false;
        isTransitioning = true;
        
        // Start coroutine to hide menu after transition and store the reference
        if (hideTransitionCoroutine != null) {
            StopCoroutine(hideTransitionCoroutine);
        }
        hideTransitionCoroutine = StartCoroutine(HideUIAfterTransition());
    }

    private IEnumerator HideUIAfterTransition() {
        // Wait for transition to complete (0.6 seconds)
        yield return new WaitForSecondsRealtime(0.6f);
        
        // Now hide the element completely
        pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
        isTransitioning = false;
        hideTransitionCoroutine = null;
    }

    private void Pause() {
        // If we're in the middle of hiding the menu, cancel that transition
        if (isTransitioning && hideTransitionCoroutine != null) {
            StopCoroutine(hideTransitionCoroutine);
            hideTransitionCoroutine = null;
            isTransitioning = false;
            pauseMenu.RemoveFromClassList("pause-menu-hidden");
        }

        // Show main pause menu, hide submenus
        pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        HideAllPanels();
        mainMenuContainer.style.display = DisplayStyle.Flex;
        pauseMenu.RemoveFromClassList("pause-menu-hidden");
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    private void HideAllPanels() {
        mainMenuContainer.style.display = DisplayStyle.None;
        settingsSubmenu.style.display = DisplayStyle.None;
        controlsPanel.style.display = DisplayStyle.None;
        videoPanel.style.display = DisplayStyle.None;
        audioPanel.style.display = DisplayStyle.None;
    }

    private void ShowSettingsMenu() {
        HideAllPanels();        
        settingsSubmenu.style.display = DisplayStyle.Flex;
    }

    private void BackToMainPauseMenu() {
        HideAllPanels();
        mainMenuContainer.style.display = DisplayStyle.Flex;
    }

    private void ShowControlsPanel() {
        HideAllPanels();
        controlsPanel.style.display = DisplayStyle.Flex;
    }

    private void ShowVideoPanel() {
        HideAllPanels();
        videoPanel.style.display = DisplayStyle.Flex;
        videoSettings.InitializeUI(pauseMenuDocument.rootVisualElement);
    }

    private void ShowAudioPanel() {
        HideAllPanels();
        audioPanel.style.display = DisplayStyle.Flex;
        audioSettings.InitializeUI(pauseMenuDocument.rootVisualElement);
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
