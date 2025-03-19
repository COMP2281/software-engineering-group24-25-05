using UnityEngine;

public class SkillsBuildGuard : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public QuestionUI skillsBuildUI;

    public void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.skillsBuildUI.MakeVisible(false);
    }

    public void OnMouseDown()
    {
        spriteRenderer.color = Color.red;

        this.PrintQuestions();
        this.skillsBuildUI.LoadNextQuestion();
        this.skillsBuildUI.MakeVisible();
    }

    public void PrintQuestions()
    {
        SkillsBuilder.Instance.DebugLogEntries();
    }
}
