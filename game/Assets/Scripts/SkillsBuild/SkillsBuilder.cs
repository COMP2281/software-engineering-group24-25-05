using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum QuestionRequestMode
{
    Random, // Uniformly random question
    WeightedIncorrect, // Weighted towards frequently incorrect questions
    WeightedCorrect, // Weighted towards frequently correct questions
}

public class SkillsBuilder : MonoBehaviour
{
    public static SkillsBuilder Instance { get; private set; }

    // IBM SkillsBuild question entries
    private List<SkillsBuildEntry> skillEntries = new List<SkillsBuildEntry>();

    // Number of attempts for skillEntries[i]
    private List<int> attempts = new List<int>();

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

    public void GetRandomQuestion(QuestionRequestMode mode)
    {
        int numElements = this.skillEntries.Count;
        int index = -1;

        switch (mode)
        {
            case QuestionRequestMode.Random:
                {
                    index = Random.Range(0, numElements);
                    break;
                }
            case QuestionRequestMode.WeightedCorrect:
                {
                    List<int> prefixSum = new List<int>(numElements);

                    for (int i = 0; i < numElements; i++)
                    {
                        prefixSum.Add(this.correct[i]);
                    }

                    index = this.RandomThresholdSearch(prefixSum);
                    break;
                }
            case QuestionRequestMode.WeightedIncorrect:
                {
                    {
                        List<int> prefixSum = new List<int>(numElements);

                        for (int i = 0; i < numElements; i++)
                        {
                            prefixSum.Add(this.attempts[i] - this.correct[i]);
                        }

                        index = this.RandomThresholdSearch(prefixSum);
                        break;
                    }
                }
        }
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

    int RandomThresholdSearch(List<int> prefixSum)
    {
        int numElements = prefixSum.Count;
        int threshold = Random.Range(0, prefixSum[numElements - 1]);

        for (int i = 0; i < numElements; i++)
        {
            if (prefixSum[i] >= threshold)
            {
                return i;
            }
        }

        return -1;
    }

}
