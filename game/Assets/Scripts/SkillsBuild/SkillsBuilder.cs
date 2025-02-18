using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillsBuilder : MonoBehaviour
{
    public static SkillsBuilder Instance { get; private set; }

    // IBM SkillsBuild question entries
    private List<SkillsBuildEntry> skillEntries = new List<SkillsBuildEntry>();

    // Number of attempts for skillEntries[i]
    private List<int> attemmpts = new List<int>();

    // Number of correct answers for skillEntries[i]
    private List<int> correct = new List<int>();

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

    public void Start()
    {
        Instance.LoadSkills();
    }

    public void LoadSkills()
    {
        SkillsBuildDataLoader loader = new SkillsBuildDataLoader();
        string path = Path.Combine(Application.streamingAssetsPath, "SkillsBuild/sample_questions.json");
        this.skillEntries = loader.LoadEntries(path);
    }

    public void DebugLogQuestions()
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
