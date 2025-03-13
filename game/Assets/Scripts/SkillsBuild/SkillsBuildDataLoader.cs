using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SkillsBuildEntry
{
    public string question;
    public string[] possible_answers;
    public int answer;
    public int difficulty;

    public void Shuffle()
    {
        for (int i = 0; i < this.possible_answers.Length; i++)
        {
            int idx = UnityEngine.Random.Range(0, i + 1);

            string tmp = possible_answers[idx];
            possible_answers[idx] = possible_answers[i];
            possible_answers[i] = tmp;

            if (i == this.answer)
            {
                this.answer = idx;
            }
            else if (idx == this.answer)
            {
                this.answer = i;
            }
        }
    }
}

[System.Serializable]
public class SkillsBuildEntriesWrapper
{
    public SkillsBuildEntry[] entries;
}

public class SkillsBuildDataLoader
{
    public List<SkillsBuildEntry> LoadEntries(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found at path: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        SkillsBuildEntriesWrapper wrapper = JsonUtility.FromJson<SkillsBuildEntriesWrapper>(json);
        return wrapper.entries.ToList<SkillsBuildEntry>();
    }
}
