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
        "You can move with the arrow keys or WASD.",
        "Press the space bar to jump.",
        "Good luck, agent!"
    };

    private int currentLine = 0;
    private bool isWaitingAfterLine = false;
    private bool isTyping = false;
    private Coroutine typeLineCoroutine;
    private Coroutine waitCoroutine; // Add this to track the wait coroutine

    void Start()
    {
        dialogueBox.SetActive(true);
        DisplayNextLine();
    }

    void Update()
    {
        // If Enter is pressed while text is typing, complete the text instantly
        if (Input.GetKeyDown(KeyCode.Return) && isTyping)
        {
            CompleteTyping();
        }
        // If Enter is pressed after a line is fully displayed, show the next line
        else if (Input.GetKeyDown(KeyCode.Return) && isWaitingAfterLine)
        {
            DisplayNextLine();
        }
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
        while (timer < 3f && !Input.GetKeyDown(KeyCode.Return))
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        // Only auto-advance if 3 seconds passed and Enter wasn't pressed
        if (!Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextLine();
        }
    }
}