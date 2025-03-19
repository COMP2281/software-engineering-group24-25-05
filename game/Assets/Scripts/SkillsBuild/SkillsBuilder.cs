using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public enum SkillsBuildQuestionClass
{
    SampleQuestions,
    ArtificialIntelligence,
}

public static class SkillsBuildQuestionClassResolver
{
    public static string GetString(SkillsBuildQuestionClass questionClass)
    {
        return questionClass switch
        {
            SkillsBuildQuestionClass.ArtificialIntelligence => "SkillsBuild/artificial_intelligence.json",
            SkillsBuildQuestionClass.SampleQuestions or _ => "SkillsBuild/sample_questions.json",
        };
    }
}

public class SkillsBuilder : MonoBehaviour
{
    public static SkillsBuilder Instance { get; private set; }

    [SerializeField] private SkillsBuildQuestionClass questionClass;
    private string questionPath;

    private List<SkillsBuildEntry> skillEntries = new List<SkillsBuildEntry>();
    private List<double> entryWeights;

    private Unity.Mathematics.Random prng;

    private void Awake()
    {
        if (Instance == null)
        {
            // Keep the instance alive between scenes
            Instance = this;

            // I am going to make the very bold assumption that this is fine
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        this.questionPath = SkillsBuildQuestionClassResolver.GetString(this.questionClass);

        Instance.LoadSkills();

        var v = System.DateTime.Now.Ticks;
        var seed = (uint)(v ^ (v >> 7) ^ (v >> 17));
        this.prng = new Unity.Mathematics.Random(seed);
    }

    public void SetQuestionClass(SkillsBuildQuestionClass questionClass)
    {
        this.questionClass = questionClass;
        this.questionPath = SkillsBuildQuestionClassResolver.GetString(questionClass);
        this.LoadSkills();
    }

    public void LoadSkills()
    {
        SkillsBuildDataLoader loader = new SkillsBuildDataLoader();
        string path = Path.Combine(Application.streamingAssetsPath, this.questionPath);
        this.skillEntries = loader.LoadEntries(path);

        this.entryWeights = new List<double>(this.skillEntries.Count);
        this.entryWeights.AddRange(Enumerable.Repeat(1.0, this.skillEntries.Count));
    }

    public SkillsBuildEntry GetQuestion(int index)
    {
        return this.skillEntries[index];
    }

    public void SetQuestion(int index, SkillsBuildEntry question)
    {
        this.skillEntries[index] = question;
    }

    public int GetRandomQuestionIndex()
    {
        int numElements = this.skillEntries.Count;

        double cumsum = 0.0;
        List<double> prefixSum = new List<double>(numElements);

        for (int i = 0; i < numElements; i++)
        {
            cumsum += this.entryWeights[i];
            prefixSum.Add(cumsum);
        }

        double threshold = this.prng.NextDouble(cumsum);

        for (int i = 0; i < numElements; i++)
        {
            if (prefixSum[i] >= threshold)
            {
                return i;
            }
        }

        return numElements - 1;
    }


    public void QuestionAnswered(int index, bool correct)
    {
        // Things we care about:
        //  - How many times a question has been asked
        //  - How many times a question has been answered correctly
        //  - How long ago a question was last asked
        //
        //  Applying scaling factors to the entry weight every time a question
        //  is answered allows us to favour questions accordingly:
        //   - Every weight is scaled by 1.1
        //      - Less recently asked questions are more favourable
        //   - Correctly answered questions are multiplied by 0.7
        //      - Combined with the previous rule, this gives a 0.77x scale
        //        factor for correctly answered questions
        //      - Correctly answered questions are less likely to be shown
        //   - Incorrectly answered questions are multiplied by 1.2
        //      - Combined with the first rule, this gives a 1.32x scale factor
        //      - Incorrectly answered questions are more likely to be shown

        // This question was just seen, so decrease weighting. This makes other
        // questions more likely to show up
        this.entryWeights[index] *= 0.8;

        if (correct)
        {
            // Answer was correct ==> 0.64x
            this.entryWeights[index] *= 0.8;
        }
        else
        {
            // Answer was incorrect => 1.2x
            this.entryWeights[index] *= 1.5;
        }


        Debug.Log("Answering Question:");
        Debug.Log($"Correct? {correct}");
        string tmp_weights = string.Join(',', this.entryWeights);
        Debug.Log($"this.entryWeights: " + tmp_weights);
    }

    public void DebugLogEntries()
    {
        string log = "";

        Debug.Log($"Number of questions: {skillEntries.Count}");
        foreach (var entry in skillEntries)
        {
            log += "SkillsBuildEntry:\n";
            log += $"  > Question: {entry.question}\n";
            log += "  > Possible Answers:\n";

            foreach (var answer in entry.possible_answers)
            {
                log += $"    > {answer}\n";
            }
        }

        Debug.Log(log);
    }

    public List<SkillsBuildEntry> GetEntries()
    {
        return skillEntries;
    }
}
