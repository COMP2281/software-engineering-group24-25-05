using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public Button nextButton;

    private string[] dialogueLines = {
        "Welcome to the game!",
        "Your mission is to steal the secret documents from the enemy base.",
        "You can move with the arrow keys or WASD.",
        "Press the space bar to jump.",
        "Good luck, agent!"
    };

    private int currentLineIndex = 0;

    void Start()
    {
        dialogueBox.SetActive(true);
        nextButton.onClick.AddListener(DisplayNextLine);
        DisplayNextLine(); // Show first line
    }

IEnumerator TypeSentence(string sentence)
{
    dialogueText.text = "";
    foreach (char letter in sentence.ToCharArray())
    {
        dialogueText.text += letter;
        yield return new WaitForSeconds(0.05f); // Adjust speed
    }
}

void DisplayNextLine()
{
    if (currentLineIndex < dialogueLines.Length)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
        currentLineIndex++;
    }
    else
    {
        dialogueBox.SetActive(false);
    }
}

}