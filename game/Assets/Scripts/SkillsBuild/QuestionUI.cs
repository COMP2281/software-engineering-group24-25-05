using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillsBuildUI : MonoBehaviour
{
    private int currentQuestionIndex;
    public UITypeWriter typewriter;
    public Button[] answerButtons;
    public SkillsBuilder skillsBuilder;

    public Color defaultTextColor = Color.black;
    public Color defaultButtonColor = Color.white;
    public Color correctTextColor = Color.black;
    public Color incorrectTextColor = Color.black;
    public Color correctButtonColor = Color.green;
    public Color incorrectButtonColor = Color.red;

    private bool active = false;

    void Start()
    {
        this.currentQuestionIndex = -1;
    }

    public void MakeVisible(bool visible = true)
    {
        this.typewriter.gameObject.SetActive(visible);

        foreach (var button in this.answerButtons)
        {
            button.gameObject.SetActive(visible);
        }

        this.active = visible;
    }

    private void hideUI()
    {
        this.MakeVisible(false);
    }

    public void LoadNextQuestion()
    {
        currentQuestionIndex = this.skillsBuilder.GetRandomQuestionIndex();
        SkillsBuildEntry currentQuestion = this.skillsBuilder.GetQuestion(currentQuestionIndex);

        this.typewriter.typeWhenReady = true;
        this.typewriter.SetText(currentQuestion.question);
        this.typewriter.SetCallback(() =>
        {
            StartButtonRendering(0);
        });

        int display_answers = Math.Min(currentQuestion.possible_answers.Length,
                                   answerButtons.Length);

        if (display_answers < currentQuestion.possible_answers.Length)
        {
            Debug.Log("Not enough answer buttons to display all possible answers");
        }

        for (int i = 0; i < display_answers; i++)
        {
            UITypeWriter buttonWriter = answerButtons[i].GetComponentInChildren<UITypeWriter>();
            buttonWriter.typeWhenReady = false;
            buttonWriter.SetText(currentQuestion.possible_answers[i]);
            buttonWriter.GetComponentInChildren<TextMeshProUGUI>().text = "";
            buttonWriter.GetComponentInChildren<TextMeshProUGUI>().color = this.defaultTextColor;
            buttonWriter.GetComponentInChildren<Image>().color = this.defaultButtonColor;

            // NOTE: For some reason this requires a local copy
            int localI = i;
            buttonWriter.SetCallback(() =>
            {
                StartButtonRendering(localI + 1);
            });

            // NOTE: Button presses are valid while the text is still typing
            int choiceIndex = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() =>
                {
                    if (this.active) { OnAnswerSelected(choiceIndex); }
                }
            );
        }
    }

    private void StartButtonRendering(int currentIndex)
    {
        if (currentIndex < this.answerButtons.Length)
        {
            Debug.Log($"Starting button {currentIndex} rendering");
            UITypeWriter buttonWriter = this.answerButtons[currentIndex].GetComponentInChildren<UITypeWriter>();
            buttonWriter.typeWhenReady = true;
            buttonWriter.StartTyping();
        }
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        SkillsBuildEntry currentQuestion = this.skillsBuilder.GetQuestion(this.currentQuestionIndex);
        bool correct = selectedIndex == currentQuestion.answer;
        this.skillsBuilder.QuestionAnswered(this.currentQuestionIndex, correct);

        TextMeshProUGUI text = this.answerButtons[selectedIndex].GetComponentInChildren<TextMeshProUGUI>();
        Image img = this.answerButtons[selectedIndex].GetComponentInChildren<Image>();

        if (correct)
        {
            img.color = correctButtonColor;
            text.color = this.correctTextColor;
        }
        else
        {
            img.color = incorrectButtonColor;
            text.color = this.incorrectTextColor;
        }

        this.active = false; // Disable further selections
        Invoke("hideUI", 1f);
    }

    private void ClearButtonText(GameObject obj)
    {
        TextMeshProUGUI textComponent = obj.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = "";
    }
}
