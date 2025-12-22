using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData
{
    private Dictionary<DiceScore, int> scores = new Dictionary<DiceScore, int>();
    private HashSet<DiceScore> locked = new HashSet<DiceScore>();

    public bool IsLocked(DiceScore type)
    {
        return locked.Contains(type);
    }

    public int GetScore(DiceScore type)
    {
        return scores.ContainsKey(type) ? scores[type] : 0;
    }

    public void LockScore(DiceScore type, int score)
    {
        locked.Add(type);
        scores[type] = score;
    }
    public int GetTotalScore()
    {
        int total = 0;
        foreach (int value in scores.Values)
        {
            total += value;
        }
        return total;
    }

    public bool IsAllLocked()
    {
        return locked.Count >= System.Enum.GetValues(typeof(DiceScore)).Length;
    }
}
