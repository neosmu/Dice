using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAutoAI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreBoard scoreBoard;

    private const int MAX_ROLL = 3;

    // 점수 항목별 가중치
    private Dictionary<DiceScore, int> scoreWeights = new Dictionary<DiceScore, int>()
    {
        { DiceScore.Yacht, 100 },
        { DiceScore.LargeStraight, 80 },
        { DiceScore.FullHouse, 70 },
        { DiceScore.FourOfKind, 60 },
        { DiceScore.SmallStraight, 50 },
        { DiceScore.Choice, 10 },

        { DiceScore.Sixes, 6 },
        { DiceScore.Fives, 5 },
        { DiceScore.Fours, 4 },
        { DiceScore.Threes, 3 },
        { DiceScore.Twos, 2 },
        { DiceScore.Ones, 1 },
    };

    public void TestAI()
    {
        StartCoroutine(AIPlayOnce());
    }

    private IEnumerator AIPlayOnce()
    {
        for (int roll = 0; roll < MAX_ROLL; roll++)
        {
            Debug.Log($"[AI] Roll {roll + 1}");

            gameManager.RollDice();
            yield return new WaitUntil(() => gameManager.IsAllDiceStopped());

            int[] dice = gameManager.GetCurrentDiceValues();
            Debug.Log("[AI] Dice Result : " + string.Join(", ", dice));

            DecideHold(dice);
        }

        // Roll 종료 후 점수 선택
        int[] finalDice = gameManager.GetCurrentDiceValues();
        DiceScore bestScore = DecideBestScore(finalDice);

        Debug.Log($"[AI] 선택 점수: {bestScore}");

        // 점수판 갱신 후 자동 선택
        scoreBoard.UpdateScoreBoard(finalDice);
        AutoSelectScore(bestScore);
    }

    // Hold 판단 
    private void DecideHold(int[] dice)
    {
        Dictionary<int, int> counts = CountDice(dice);

        foreach (var pair in counts)
        {
            if (pair.Value >= 2)
            {
                gameManager.HoldValue(pair.Key);
                Debug.Log($"Hold : {pair.Key}");
            }
        }
    }

    private Dictionary<int, int> CountDice(int[] dice)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();

        foreach (int d in dice)
        {
            if (!dict.ContainsKey(d))
                dict.Add(d, 1);
            else
                dict[d]++;
        }
        return dict;
    }

    // 가중치 기반 점수 판단
    private DiceScore DecideBestScore(int[] dice)
    {
        List<DiceScore> possible = ScoreCombo.GetPossibleScores(dice);

        DiceScore best = possible[0];
        int bestValue = -1;

        foreach (DiceScore type in possible)
        {
            int score = ScoreCombo.CalculateScore(type, dice);
            int weight = scoreWeights.ContainsKey(type) ? scoreWeights[type] : 0;

            int total = score + weight;

            Debug.Log($"[AI] {type} → 점수:{score}, 가중치:{weight}, 합:{total}");

            if (total > bestValue)
            {
                bestValue = total;
                best = type;
            }
        }

        return best;
    }

    // 점수 자동 선택
    private void AutoSelectScore(DiceScore type)
    {
        foreach (var slot in scoreBoard.slots)
        {
            if (slot.scoreType == type && slot.IsLocked() == false)
            {
                slot.toggle.isOn = true;
                break;
            }
        }
    }
}
