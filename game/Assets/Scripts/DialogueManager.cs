using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    private string[] dialogueLines = {
        "Welcome to the game!",
        "Your mission is to steal the secret documents from the enemy base.",
        "Good luck, agent!"
    };

    private int currentLine = 0;
    private bool isWaitingAfterLine = false;
    private bool isTyping = false;
    private Coroutine typeLineCoroutine;
    private Coroutine waitCoroutine;
    
    // Track previous input state to detect button press
    private bool previousSubmitState = false;

    void Start()
    {
        dialogueBox.SetActive(true);
        DisplayNextLine();
    }
    
    void Update()
    {
        // Check for Submit input through UserInput singleton
        bool currentSubmitState = UserInput.Instace.SubmitInput;
        
        // Submit was pressed this frame
        if (currentSubmitState && !previousSubmitState)
        {
            // If text is typing, complete the text instantly
            if (isTyping)
            {
                CompleteTyping();
            }
            // If a line is fully displayed, show the next line
            else if (isWaitingAfterLine)
            {
                DisplayNextLine();
            }
        }
        
        // Update previous state for next frame
        previousSubmitState = currentSubmitState;
    }

    void CompleteTyping()
    {
        if (typeLineCoroutine != null)
        {
            StopCoroutine(typeLineCoroutine);
            typeLineCoroutine = null;
        }
        
        // Display the full line
        dialogueText.text = dialogueLines[currentLine];
        
        isTyping = false;
        isWaitingAfterLine = true;
        
        // Start waiting for 3 seconds before automatically displaying the next line
        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);
        waitCoroutine = StartCoroutine(WaitForNextLine());
    }

    void DisplayNextLine()
    {
        // Reset waiting state
        isWaitingAfterLine = false;
        
        // Stop any existing wait coroutine
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
        
        currentLine++;
        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }
        
        // Start typing the next line
        typeLineCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLine]));
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        
        isTyping = false;
        isWaitingAfterLine = true;
        
        // Start waiting for 3 seconds before automatically displaying the next line
        waitCoroutine = StartCoroutine(WaitForNextLine());
    }
    
    IEnumerator WaitForNextLine()
    {
        float timer = 0;
        while (timer < 3f && !UserInput.Instace.SubmitInput)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        // Only auto-advance if 3 seconds passed and Submit wasn't pressed
        if (!UserInput.Instace.SubmitInput)
        {
            DisplayNextLine();
        }
    }
}