using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Code from https://youtu.be/_jtj73lu2Ko?si=qFlQrmGtG8tRAwfv

public class MainMenuEvents : MonoBehaviour
{
    private Button artificialIntelligenceButton;
    private Button dataAnalyticsButton;
    private Button cyberSecurityButton;
    private Button settingsButton;
    private Button backButton;
    private VisualElement mainMenu;
    private VisualElement titleContainer;
    private VisualElement leftContainer;
    private VisualElement rightContainer;
    private Label artificialIntelligenceText;
    private Label dataAnalyticsText;
    private Label cyberSecurityText;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    void OnEnable()
    {
        // Load the UXML and USS
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Retrieve the buttons by their name
        artificialIntelligenceButton = root.Q<Button>("artificialIntelligenceButton");
        dataAnalyticsButton = root.Q<Button>("dataAnalyticsButton");
        cyberSecurityButton = root.Q<Button>("cyberSecurityButton");
        settingsButton = root.Q<Button>("settingsButton");
        backButton = root.Q<Button>("backButton");

        // Retrieve visual elements by their name
        mainMenu = root.Q<VisualElement>("mainMenu");
        titleContainer = root.Q<VisualElement>("titleContainer");
        leftContainer = root.Q<VisualElement>("leftContainer");
        rightContainer = root.Q<VisualElement>("rightContainer");
        artificialIntelligenceText = root.Q<Label>("artificialIntelligenceText");
        dataAnalyticsText = root.Q<Label>("dataAnalyticsText");
        cyberSecurityText = root.Q<Label>("cyberSecurityText");

        // Initially hide all tooltip texts
        HideAllTooltipTexts();

        // Register the button click event
        artificialIntelligenceButton.RegisterCallback<ClickEvent>(evt => LoadScene("SampleScene"));
        dataAnalyticsButton.RegisterCallback<ClickEvent>(evt => LoadScene("SampleScene"));
        cyberSecurityButton.RegisterCallback<ClickEvent>(evt => LoadScene("SampleScene"));
        settingsButton.RegisterCallback<ClickEvent>(evt => LoadScene("SettingsScene"));

        // Register specific mouse leave events for specialty buttons
        artificialIntelligenceButton.RegisterCallback<MouseLeaveEvent>(OnSpecialtyButtonLeave);
        dataAnalyticsButton.RegisterCallback<MouseLeaveEvent>(OnSpecialtyButtonLeave);
        cyberSecurityButton.RegisterCallback<MouseLeaveEvent>(OnSpecialtyButtonLeave);

        // Retrieve all buttons in the menu
        root.Query<Button>().ForEach(button =>
        {
            _menuButtons.Add(button);
            button.RegisterCallback<ClickEvent>(OnAllButtonClick);
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        });

        // Initialize the audio source
        _audioSource = GetComponent<AudioSource>();

        // Intialize visual element classes
        mainMenu.AddToClassList("main-menu");
        titleContainer.AddToClassList("title-container");
        leftContainer.AddToClassList("left-container");
        rightContainer.AddToClassList("right-container");

        mainMenu.RemoveFromClassList("main-menu-hidden");
        titleContainer.RemoveFromClassList("title-container-hidden");
        leftContainer.RemoveFromClassList("left-container-hidden");
        rightContainer.RemoveFromClassList("right-container-hidden");
    }

    private void LoadScene(string sceneName)
    {
        // Add transition classes
        // mainMenu.AddToClassList("main-menu-hidden");
        // titleContainer.AddToClassList("title-container-hidden");
        // leftContainer.AddToClassList("left-container-hidden");
        // rightContainer.AddToClassList("right-container-hidden");

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
        
        // Show appropriate description text based on which button is hovered
        if (evt.target == artificialIntelligenceButton && artificialIntelligenceText != null)
        {
            HideAllTooltipTexts();
            artificialIntelligenceText.style.display = DisplayStyle.Flex;
        }
        else if (evt.target == dataAnalyticsButton && dataAnalyticsText != null)
        {
            HideAllTooltipTexts();
            dataAnalyticsText.style.display = DisplayStyle.Flex;
        }
        else if (evt.target == cyberSecurityButton && cyberSecurityText != null)
        {
            HideAllTooltipTexts();
            cyberSecurityText.style.display = DisplayStyle.Flex;
        }
    }
    
    private void OnSpecialtyButtonLeave(MouseLeaveEvent evt)
    {
        // Hide all tooltip texts when mouse leaves any specialty button
        HideAllTooltipTexts();
    }
    
    private void HideAllTooltipTexts()
    {
        // Hide all specialty tooltip texts
        if (artificialIntelligenceText != null)
            artificialIntelligenceText.style.display = DisplayStyle.None;
            
        if (dataAnalyticsText != null)
            dataAnalyticsText.style.display = DisplayStyle.None;
            
        if (cyberSecurityText != null)
            cyberSecurityText.style.display = DisplayStyle.None;
    }
}
