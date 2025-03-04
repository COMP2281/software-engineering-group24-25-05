using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillsBuildUI : MonoBehaviour
{
    private SkillsBuildEntry currentQuestion;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public SkillsBuilder skillsBuilder;

    void Start()
    {
        this.currentQuestion = new SkillsBuildEntry();
    }

    public void MakeVisible(bool visible = true)
    {
        this.questionText.gameObject.SetActive(visible);
        foreach (var button in this.answerButtons)
        {
            button.gameObject.SetActive(visible);
        }
    }

    private void hideUI()
    {
        this.MakeVisible(false);
    }

    public void LoadNextQuestion()
    {
        currentQuestion = this.skillsBuilder.GetRandomQuestion(QuestionRequestMode.Random);

        this.questionText.text = currentQuestion.question;

        int display_answers = Math.Min(currentQuestion.possible_answers.Length,
                                   answerButtons.Length);

        if (display_answers < currentQuestion.possible_answers.Length)
        {
            Debug.Log("Not enough answer buttons to display all possible answers");
        }

        for (int i = 0; i < display_answers; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.possible_answers[i];

            int choiceIndex = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(choiceIndex));
        }
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        Debug.Log($"Selected Answer: {selectedIndex}");

        Invoke("hideUI", 1.5f);

        if (selectedIndex == this.currentQuestion.answer)
        {
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Incorrect Answer :(");
        }
    }
}
