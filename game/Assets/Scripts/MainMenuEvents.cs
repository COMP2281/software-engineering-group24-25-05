using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Code from https://youtu.be/_jtj73lu2Ko?si=qFlQrmGtG8tRAwfv

public class MainMenuEvents : MonoBehaviour
{
    private Button startGameButton;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    void OnEnable()
    {
        // Load the UXML and USS
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Retrieve the start screen and button by their name
        startGameButton = root.Q<Button>("startGameButton");

        // Register the button click event
        startGameButton.RegisterCallback<ClickEvent>(OnStartButtonPressed);

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

    private void OnStartButtonPressed(ClickEvent evt)
    {
        Debug.Log("Play Game Button Clicked");
        // Hide the start screen by setting its display style to none
        SceneManager.LoadScene("GameScene");
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnButtonHover(MouseEnterEvent evt)
    {
        _audioSource.Play();
    }
}
