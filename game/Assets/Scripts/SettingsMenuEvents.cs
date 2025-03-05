using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Code from https://youtu.be/_jtj73lu2Ko?si=qFlQrmGtG8tRAwfv

public class SettingsMenuEvents : MonoBehaviour
{
    private Button backButton;
    private Button audioButton;
    private Button videoButton;
    private Button controlsButton;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    void OnEnable()
    {
        // Load the UXML and USS
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Retrieve the buttons by their name
        backButton = root.Q<Button>("BackButton");
        audioButton = root.Q<Button>("AudioButton");
        videoButton = root.Q<Button>("VideoButton");
        controlsButton = root.Q<Button>("ControlsButton");

        // Register the button click event
        backButton.RegisterCallback<ClickEvent>(evt => LoadScene("MainMenu"));


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

    private void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
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

    // Function to display the correct settings menu based on the button clicked
    private void DisplaySettingsMenu(string menuName)
    {
        // Hide all settings menus
        foreach (var button in _menuButtons)
        {
            button.style.display = DisplayStyle.None;
        }

        // Display the selected settings menu
        var selectedMenu = _menuButtons.Find(button => button.name == menuName);
        selectedMenu.style.display = DisplayStyle.Flex;
    }
}
