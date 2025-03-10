using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillsBuildUI : MonoBehaviour
{
    private int currentQuestionIndex;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public SkillsBuilder skillsBuilder;

    void Start()
    {
        this.currentQuestionIndex = -1;
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
        // currentQuestionIndex = this.skillsBuilder.GetRandomQuestionIndex(QuestionRequestMode.Random);
        currentQuestionIndex = this.skillsBuilder.GetRandomQuestionIndex(QuestionRequestMode.WeightedIncorrect);
        SkillsBuildEntry currentQuestion = this.skillsBuilder.GetQuestion(currentQuestionIndex);

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
        SkillsBuildEntry currentQuestion = this.skillsBuilder.GetQuestion(this.currentQuestionIndex);
        bool correct = selectedIndex == currentQuestion.answer;
        this.skillsBuilder.QuestionAnswered(this.currentQuestionIndex, correct);

        Invoke("hideUI", 0.75f);
    }
}
