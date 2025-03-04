using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;
    private UIDocument pauseMenuDocument;
    private Button resumeButton;
    private Button settingsButton;
    private Button mainMenuButton;
    private Button exitButton;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    void Start() {
        // Load the UXML and USS
        pauseMenuDocument = GetComponent<UIDocument>();
        var root = pauseMenuDocument.rootVisualElement;

        // Initially hide the pause menu
        root.style.display = DisplayStyle.None;

        // Retrieve the resume button by its name
        resumeButton = root.Q<Button>("resumeButton");
        settingsButton = root.Q<Button>("settingsButton");
        mainMenuButton = root.Q<Button>("mainMenuButton");
        exitButton = root.Q<Button>("exitButton");

        // Register the button click event
        resumeButton.RegisterCallback<ClickEvent>(evt => Resume());
        settingsButton.RegisterCallback<ClickEvent>(evt => LoadScene("SettingsScene"));
        mainMenuButton.RegisterCallback<ClickEvent>(evt => LoadScene("MainMenu"));
        exitButton.RegisterCallback<ClickEvent>(evt => Application.Quit());

        // Retrieve all buttons in the menu
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
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                Resume();
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
        pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        Time.timeScale = 0.0f;
        isPaused = true;
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
