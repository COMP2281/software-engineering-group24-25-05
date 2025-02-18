using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SkillsBuildEntry
{
    public string question;
    public string answer;
    public string[] possible_answers;
    public int difficulty;
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
