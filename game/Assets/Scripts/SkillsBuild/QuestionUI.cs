using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsBuildUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public SkillsBuildEntry currentQuestion;
    public Button[] answerButtons;
    private SkillsBuilder skillsBuilder;

    void LoadNextQuestion()
    {
        currentQuestion = this.skillsBuilder.GetRandomQuestion(QuestionRequestMode.Random);
        questionText.text = currentQuestion.question;

        for (int i = 0; i < currentQuestion.possible_answers.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.possible_answers[i];

            // int choiceIndex = i;
            // answerButtons[i].onClick.RemoveAllListeners();
            // answerButtons[i].onClick.AddListener(() => OnAnswerSelected(choiceIndex));
        }
    }
}

