using UnityEngine;

public class SkillsBuildGuard : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public void Start()
    {
        Debug.Log("Guard Start");
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        Debug.Log("Debug!");

        spriteRenderer.color = Color.red;
    }

    public void PrintQuestions()
    {
        Debug.Log("Printing Questions");
        SkillsBuilder.Instance.DebugLogQuestions();
    }
}
