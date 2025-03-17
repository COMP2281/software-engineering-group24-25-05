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

    private Action correctAnswerCallback;
    private Action incorrectAnswerCallback;

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

    public bool IsVisible()
    {
        return this.active;
    }

    private void hideUI()
    {
        this.MakeVisible(false);
    }

    public void SetCorrectAnswerCallback(Action callback)
    {
        this.correctAnswerCallback = callback;
    }

    public void SetIncorrectAnswerCallback(Action callback)
    {
        this.incorrectAnswerCallback = callback;
    }

    public void LoadNextQuestion()
    {
        currentQuestionIndex = this.skillsBuilder.GetRandomQuestionIndex();
        SkillsBuildEntry currentQuestion = this.skillsBuilder.GetQuestion(currentQuestionIndex);

        this.typewriter.Clear();
        foreach (var button in this.answerButtons)
        {
            button.GetComponentInChildren<UITypeWriter>().Clear();
        }

        Action<QuestionAPIResponse> correctCallback = (QuestionAPIResponse aiResponse) =>
        {
            SkillsBuildEntry aiEntry = new SkillsBuildEntry();

            aiEntry.question = aiResponse.question;
            aiEntry.answer = 0;
            aiEntry.possible_answers = new string[aiResponse.answer.incorrect.Length + 1];

            aiEntry.possible_answers[0] = aiResponse.answer.correct;

            for (int i = 0; i < aiResponse.answer.incorrect.Length; i++)
            {
                aiEntry.possible_answers[i + 1] = aiResponse.answer.incorrect[i];
            }

            aiEntry.Shuffle();

            this.skillsBuilder.SetQuestion(currentQuestionIndex, aiEntry);

            Debug.Log($"AI Response: {JsonUtility.ToJson(aiResponse)}");
            Debug.Log($"Current Question: {JsonUtility.ToJson(aiEntry)}");

            this.typewriter.typeWhenReady = true;
            this.typewriter.SetText(aiEntry.question);
            this.typewriter.SetCallback(() =>
            {
                StartButtonRendering(0);
            });
            this.typewriter.StartTyping();

            int display_answers = Math.Min(aiEntry.possible_answers.Length,
                                       answerButtons.Length);

            if (display_answers < aiEntry.possible_answers.Length)
            {
                Debug.Log("Not enough answer buttons to display all possible answers");
            }

            for (int i = 0; i < display_answers; i++)
            {
                UITypeWriter buttonWriter = answerButtons[i].GetComponentInChildren<UITypeWriter>();
                buttonWriter.typeWhenReady = false;
                buttonWriter.SetText(aiEntry.possible_answers[i]);
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
        };

        Debug.Log("Sending Request");
        QuestionAPIRequest request = new QuestionAPIRequest(currentQuestion.question);
        StartCoroutine(
            request.SendRequest(
                correctCallback,
                (string msg) => { Debug.Log($"Error: {msg}"); }
            )
        );
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

        for (int i = 0; i < this.answerButtons.Length; i++)
        {
            TextMeshProUGUI text = this.answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Image img = this.answerButtons[i].GetComponentInChildren<Image>();

            Color imgColor1 = this.defaultButtonColor;
            Color imgColor2;

            Color textColor1 = this.defaultTextColor;
            Color textColor2;

            if (i == currentQuestion.answer)
            {
                imgColor2 = this.correctButtonColor;
                textColor2 = this.correctTextColor;
            }
            else
            {
                imgColor2 = this.incorrectButtonColor;
                textColor2 = this.incorrectTextColor;
            }

            // Highlight the selected answer
            if (i == selectedIndex)
            {
                imgColor1 = imgColor2;
                textColor1 = textColor2;
            }

            Color imgColor = ColorUtil.BlendColors(imgColor1, imgColor2, 0.3f);
            Color textColor = ColorUtil.BlendColors(textColor1, textColor2, 0.3f);

            img.color = imgColor;
            text.color = textColor;
        }

        if (correct)
        {
            this.correctAnswerCallback.Invoke();
        }
        else
        {
            this.incorrectAnswerCallback.Invoke();
        }

        this.active = false; // Disable further selections
        Invoke("hideUI", 1.5f);
    }

    private void ClearButtonText(GameObject obj)
    {
        TextMeshProUGUI textComponent = obj.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = "";
    }
}
