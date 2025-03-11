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

    // Lock icons for each specialty
    private VisualElement aiLockIcon;
    private VisualElement dataLockIcon;
    private VisualElement cyberLockIcon;
    
    // Audio clip for locked level attempt
    [SerializeField] private AudioClip lockedSound;

    // Notification popup
    private VisualElement notificationPopup;
    private Label notificationText;
    
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

        // Get lock icons
        aiLockIcon = root.Q<VisualElement>("aiLockIcon");
        dataLockIcon = root.Q<VisualElement>("dataLockIcon");
        cyberLockIcon = root.Q<VisualElement>("cyberLockIcon");

        // Initially hide all tooltip texts
        HideAllTooltipTexts();

        // Register the button click event with level lock checks
        artificialIntelligenceButton.RegisterCallback<ClickEvent>(evt => TryLoadLevel(LevelManager.LevelType.ArtificialIntelligence));
        dataAnalyticsButton.RegisterCallback<ClickEvent>(evt => TryLoadLevel(LevelManager.LevelType.DataAnalytics));
        cyberSecurityButton.RegisterCallback<ClickEvent>(evt => TryLoadLevel(LevelManager.LevelType.CyberSecurity));
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

        // Update lock visuals
        UpdateLockVisuals();

        // Create notification popup if it doesn't exist
        notificationPopup = root.Q<VisualElement>("notificationPopup");
        if (notificationPopup == null)
        {
            // Create popup elements
            notificationPopup = new VisualElement();
            notificationPopup.name = "notificationPopup";
            notificationPopup.AddToClassList("notification-popup");
            
            notificationText = new Label();
            notificationText.name = "notificationText";
            
            notificationPopup.Add(notificationText);
            root.Add(notificationPopup);
        }
        else
        {
            notificationText = notificationPopup.Q<Label>("notificationText");
        }

        // Hide popup initially
        notificationPopup.style.display = DisplayStyle.None;
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
        
        // Get the button that was hovered
        Button hoveredButton = evt.target as Button;
        
        // Show appropriate description text based on which button is hovered
        if (hoveredButton == artificialIntelligenceButton && artificialIntelligenceText != null)
        {
            HideAllTooltipTexts();
            artificialIntelligenceText.style.display = DisplayStyle.Flex;
            

        }
        else if (hoveredButton == dataAnalyticsButton && dataAnalyticsText != null)
        {
            HideAllTooltipTexts();
            dataAnalyticsText.style.display = DisplayStyle.Flex;
            
        }
        else if (hoveredButton == cyberSecurityButton && cyberSecurityText != null)
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

    private void UpdateLockVisuals()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("LevelManager instance not found!");
            return;
        }
        
        // Update AI button/lock
        bool aiUnlocked = LevelManager.Instance.IsLevelUnlocked(LevelManager.LevelType.ArtificialIntelligence);
        SetButtonLockState(artificialIntelligenceButton, aiLockIcon, aiUnlocked);
        
        // Update Data Analytics button/lock
        bool dataUnlocked = LevelManager.Instance.IsLevelUnlocked(LevelManager.LevelType.DataAnalytics);
        SetButtonLockState(dataAnalyticsButton, dataLockIcon, dataUnlocked);
        
        // Update Cyber Security button/lock
        bool cyberUnlocked = LevelManager.Instance.IsLevelUnlocked(LevelManager.LevelType.CyberSecurity);
        SetButtonLockState(cyberSecurityButton, cyberLockIcon, cyberUnlocked);
    }
    
    private void SetButtonLockState(Button button, VisualElement lockIcon, bool isUnlocked)
    {
        if (button == null) return;
        
        if (isUnlocked)
        {
            button.RemoveFromClassList("locked-button");
            button.AddToClassList("unlocked-button");
            button.SetEnabled(true); // Enable interaction for unlocked buttons
            if (lockIcon != null) lockIcon.style.display = DisplayStyle.None;
        }
        else
        {
            button.RemoveFromClassList("unlocked-button");
            button.AddToClassList("locked-button");
            button.SetEnabled(true); // Keep enabled to detect clicks
            if (lockIcon != null) 
            {
                lockIcon.style.display = DisplayStyle.Flex;
                lockIcon.AddToClassList("lock-icon");
            }
        }
    }

    private void TryLoadLevel(LevelManager.LevelType levelType)
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("LevelManager instance not found!");
            return;
        }
        
        if (LevelManager.Instance.IsLevelUnlocked(levelType))
        {
            LoadScene(LevelManager.Instance.GetSceneName(levelType));
        }
        else
        {
            // Play locked sound
            if (lockedSound != null && _audioSource != null)
            {
                _audioSource.clip = lockedSound;
                _audioSource.Play();
            }
            
            // Show popup instead of tooltip
            ShowLockedLevelPopup(levelType);
        }
    }
    
    private void ShowLockedLevelPopup(LevelManager.LevelType levelType)
    {
        string message = "";
        
        // Show custom locked message based on level type
        switch (levelType)
        {
            case LevelManager.LevelType.DataAnalytics:
                message = "Complete AI track to unlock Data Analytics";
                break;
                
            case LevelManager.LevelType.CyberSecurity:
                message = "Complete Data Analytics track to unlock Cyber Security";
                break;
                
            default:
                message = "This level is locked";
                break;
        }
        
        // Show the notification popup
        ShowNotificationPopup(message);
    }
    
    private void ShowNotificationPopup(string message)
    {
        // Stop any existing fade coroutines
        StopAllCoroutines();
        
        // Set popup text
        notificationText.text = message;
        
        // Show popup
        notificationPopup.style.display = DisplayStyle.Flex;
        notificationPopup.RemoveFromClassList("hiding");
        notificationPopup.AddToClassList("visible");
        
        // Start fade out coroutine
        StartCoroutine(HideNotificationAfterDelay(2.5f));
    }
    
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Start fade out
        notificationPopup.RemoveFromClassList("visible");
        notificationPopup.AddToClassList("hiding");
        
        // Wait for fade animation
        yield return new WaitForSeconds(0.5f);
        
        // Hide completely
        notificationPopup.style.display = DisplayStyle.None;
    }
    
    // Remove or modify the old ShowLockedLevelFeedback method
    private void ShowLockedLevelFeedback(LevelManager.LevelType levelType)
    {
        // We now handle this with the popup notification system
        // This method can be kept for backward compatibility or removed
    }
    
    private Button GetButtonForLevelType(LevelManager.LevelType levelType)
    {
        switch (levelType)
        {
            case LevelManager.LevelType.ArtificialIntelligence: return artificialIntelligenceButton;
            case LevelManager.LevelType.DataAnalytics: return dataAnalyticsButton;
            case LevelManager.LevelType.CyberSecurity: return cyberSecurityButton;
            default: return null;
        }
    }
}
