using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillsBuilder : MonoBehaviour
{
    public static SkillsBuilder Instance { get; private set; }

    private List<SkillsBuildEntry> skillEntries = new List<SkillsBuildEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            // Keep the instance alive between scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSkills()
    {
        SkillsBuildDataLoader loader = new SkillsBuildDataLoader();
        string path = Path.Combine(Application.streamingAssetsPath, "SkillsBuild/sample_questions.json");
        this.skillEntries = loader.LoadEntries(path);
    }

    public void PrintQuestions()
    {
        Debug.Log($"Number of questions: {skillEntries.Count}");
        foreach (var entry in skillEntries)
        {
            Debug.Log($"Question: {entry.question}");
        }
    }

    public List<SkillsBuildEntry> GetEntries()
    {
        return skillEntries;
    }
}
