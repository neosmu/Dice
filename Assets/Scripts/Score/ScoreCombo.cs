using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCombo : MonoBehaviour
{
    public static void Print(int[] dice)
    {
        var scores = GetPossibleScores(dice);

        Debug.Log("Dice: " + string.Join(", ", dice));

        foreach (var s in scores)
        {
            int score = CalculateScore(s, dice);
            Debug.Log($"가능: {s} (점수: {score})");
        }
    }

    public static List<DiceScore> GetPossibleScores(int[] dice)
    {
        List<DiceScore> results = new List<DiceScore>();

        // 기본 점수 항목
        results.Add(DiceScore.Ones);
        results.Add(DiceScore.Twos);
        results.Add(DiceScore.Threes);
        results.Add(DiceScore.Fours);
        results.Add(DiceScore.Fives);
        results.Add(DiceScore.Sixes);

        results.Add(DiceScore.Choice);

        Dictionary<int, int> counts = CountDice(dice);
        HashSet<int> unique = new HashSet<int>(dice);

        if (IsYacht(counts)) results.Add(DiceScore.Yacht);
        if (IsFourOfKind(counts)) results.Add(DiceScore.FourOfKind);
        if (IsFullHouse(counts)) results.Add(DiceScore.FullHouse);
        if (IsSmallStraight(unique)) results.Add(DiceScore.SmallStraight);
        if (IsLargeStraight(unique)) results.Add(DiceScore.LargeStraight);

        return results;
    }

    // 점수 계산
    public static int CalculateScore(DiceScore type, int[] dice)
    {
        switch (type)
        {
            case DiceScore.Ones: return SumOf(dice, 1);
            case DiceScore.Twos: return SumOf(dice, 2);
            case DiceScore.Threes: return SumOf(dice, 3);
            case DiceScore.Fours: return SumOf(dice, 4);
            case DiceScore.Fives: return SumOf(dice, 5);
            case DiceScore.Sixes: return SumOf(dice, 6);

            case DiceScore.Choice:
                return dice.Sum();

            case DiceScore.FourOfKind:
                return dice.Sum();

            case DiceScore.FullHouse:
                return dice.Sum(); 

            case DiceScore.SmallStraight:
                return 15; 

            case DiceScore.LargeStraight:
                return 30; 

            case DiceScore.Yacht:
                return 50;
        }

        return 0;
    }

    private static int SumOf(int[] dice, int value)
    {
        int sum = 0;
        foreach (int d in dice)
        {
            if (d == value)
                sum += d;
        }
        return sum;
    }

    private static Dictionary<int, int> CountDice(int[] dice)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();

        foreach (var d in dice)
        {
            if (!dict.ContainsKey(d))
                dict.Add(d, 1);
            else
                dict[d]++;
        }

        return dict;
    }

    private static bool IsYacht(Dictionary<int, int> counts)
    {
        return counts.Values.Any(v => v == 5);
    }

    private static bool IsFourOfKind(Dictionary<int, int> counts)
    {
        return counts.Values.Any(v => v == 4);
    }

    private static bool IsFullHouse(Dictionary<int, int> counts)
    {
        bool three = counts.Values.Contains(3);
        bool two = counts.Values.Contains(2);
        return three && two;
    }

    private static bool IsSmallStraight(HashSet<int> unique)
    {
        int[][] seq = new int[][]
        {
            new int[]{1,2,3,4},
            new int[]{2,3,4,5},
            new int[]{3,4,5,6}
        };

        foreach (var s in seq)
        {
            if (s.All(v => unique.Contains(v)))
                return true;
        }

        return false;
    }

    private static bool IsLargeStraight(HashSet<int> unique)
    {
        int[] s1 = { 1, 2, 3, 4, 5 };
        int[] s2 = { 2, 3, 4, 5, 6 };

        if (s1.All(v => unique.Contains(v))) return true;
        if (s2.All(v => unique.Contains(v))) return true;

        return false;
    }
}
