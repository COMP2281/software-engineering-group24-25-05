using UnityEngine;

public class SkillsBuildGuard : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        spriteRenderer.color = Color.red;

        this.PrintQuestions();
    }

    public void PrintQuestions()
    {
        SkillsBuilder.Instance.DebugLogEntries();
    }
}
