using UnityEngine;

public class SkillsBuilderHook : MonoBehaviour
{
    public SkillsBuilder skillsBuilder;

    public void Start()
    {
        Debug.Log("Started up logging.");
        Debug.Log("Loading Skills");
        this.skillsBuilder.LoadSkills();
        this.PrintQuestions();
    }

    public void PrintQuestions()
    {
        if (skillsBuilder != null)
        {
            Debug.Log("Printing Questions");
            skillsBuilder.PrintQuestions();
        }
        else
        {
            Debug.LogError("SkillsBuilder reference is missing!");
        }
    }
}
