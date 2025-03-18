using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillsBuildUI : MonoBehaviour
{
    private int currentQuestionIndex;
    public UITypeWriter questionTypewriter;
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

    public float minQuestionTime = 5.0f;
    private float timeSinceQuestion = 0.0f;
    private bool canQuestion = true;

    void Start()
    {
        this.currentQuestionIndex = -1;
    }

    void Update()
    {
        this.timeSinceQuestion += Time.deltaTime;

        if (!this.active && this.timeSinceQuestion > this.minQuestionTime)
        {
            this.canQuestion = true;
        }
    }

    public void MakeVisible(bool visible = true)
    {
        this.questionTypewriter.gameObject.SetActive(visible);

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

    public void SetTimeSinceQuestion(float time)
    {
        this.timeSinceQuestion = time;
    }

    public float GetTimeSinceQuestion()
    {
        return this.timeSinceQuestion;
    }

    public void SetCanQuestion(bool can)
    {
        this.canQuestion = can;
    }

    public bool GetCanQuestion()
    {
        return this.canQuestion;
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

        this.questionTypewriter.Clear();
        foreach (var button in this.answerButtons)
        {
            button.GetComponentInChildren<UITypeWriter>().Clear();
        }

        Debug.Log("Sending Request");

        // Start the loading spinner
        this.questionTypewriter.StartSpinner();

        QuestionAPIRequest request = new QuestionAPIRequest(currentQuestion.question);
        StartCoroutine(
            request.SendRequest(
                (QuestionAPIResponse response) =>
                {
                    this.questionTypewriter.StopSpinner();
                    SkillsBuildEntry entry = ProcessAIResponse(response);
                    UpdateUIEntry(entry);
                },
                (string msg) =>
                {
                    Debug.Log($"Error: {msg}");
                    this.questionTypewriter.StopSpinner();
                    UpdateUIEntry(currentQuestion);
                }
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

    public void ResetColors()
    {
        for (int i = 0; i < this.answerButtons.Length; i++)
        {
            TextMeshProUGUI text = this.answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Image img = this.answerButtons[i].GetComponentInChildren<Image>();

            img.color = this.defaultButtonColor;
            text.color = this.defaultTextColor;
        }
    }

    SkillsBuildEntry ProcessAIResponse(QuestionAPIResponse aiResponse)
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

        return aiEntry;
    }

    void UpdateUIEntry(SkillsBuildEntry entry)
    {
        this.skillsBuilder.SetQuestion(currentQuestionIndex, entry);

        this.questionTypewriter.typeWhenReady = true;
        this.questionTypewriter.SetText(entry.question);
        this.questionTypewriter.SetCallback(() =>
        {
            StartButtonRendering(0);
        });
        this.questionTypewriter.StartTyping();

        int display_answers = Math.Min(entry.possible_answers.Length,
                                   answerButtons.Length);

        if (display_answers < entry.possible_answers.Length)
        {
            Debug.Log("Not enough answer buttons to display all possible answers");
        }

        for (int i = 0; i < display_answers; i++)
        {
            UITypeWriter buttonWriter = answerButtons[i].GetComponentInChildren<UITypeWriter>();
            buttonWriter.typeWhenReady = false;
            buttonWriter.SetText(entry.possible_answers[i]);
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
}
