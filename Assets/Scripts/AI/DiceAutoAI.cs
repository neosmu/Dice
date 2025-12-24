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
        { DiceScore.Aces, 1 },
    };

    public void PlayAITurn()
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
            DecideHold(dice);
        }

        // Roll 종료 후 점수 선택
        int[] finalDice = gameManager.GetCurrentDiceValues();
        DiceScore bestScore = BestScore(finalDice);

        Debug.Log($"[AI] 선택 점수: {bestScore}");

        // 점수판 갱신 후 자동 선택
        FinishAITurn(bestScore);
    }

    // Hold 판단 
    private void DecideHold(int[] dice)
    {
        List<int> uniq = new List<int>();
        for (int i = 0; i < dice.Length; i++)
        {
            if (uniq.Contains(dice[i]) == false)
                uniq.Add(dice[i]);
        }
        uniq.Sort();
        int bestLen = 1;
        int bestStart = 0;

        int curLen = 1;
        int curStart = 0;

        for (int i = 1; i < uniq.Count; i++)
        {
            if (uniq[i] == uniq[i - 1] + 1)
            {
                curLen++;
            }
            else
            {
                if (curLen > bestLen)
                {
                    bestLen = curLen;
                    bestStart = curStart;
                }
                curStart = i;
                curLen = 1;
            }
        }

        if (curLen > bestLen)
        {
            bestLen = curLen;
            bestStart = curStart;
        }

        // 스트레이트 가능성 있으면 우선 홀드
        if (bestLen >= 3)
        {
            for (int i = bestStart; i < bestStart + bestLen; i++)
            {
                gameManager.HoldValue(uniq[i]);
            }
            return;
        }
        Dictionary<int, int> counts = ScoreCombo.CountDice(dice);
        foreach (var pair in counts)
        {
            if (pair.Value >= 2)
            {
                gameManager.HoldValue(pair.Key);
            }
        }
    }
    private bool IsLowValueHighCombo(DiceScore type, int score)
    {
        switch (type)
        {
            case DiceScore.FourOfKind:
                return score < 12;

            case DiceScore.FullHouse:
                return score < 10;

            case DiceScore.Choice:
                return score < 14;
        }
        return false;
    }

    // 가중치 기반 점수 판단
    private DiceScore BestScore(int[] dice)
    {
        List<DiceScore> possible = ScoreCombo.GetPossibleScores(dice);

        DiceScore best = DiceScore.Aces;
        int bestValue = int.MinValue;
        bool found = false;

        foreach (DiceScore type in possible)
        {
            if (IsScoreLocked(type))
                continue;

            int score = ScoreCombo.CalculateScore(type, dice);

            int weight = 0;
            if (scoreWeights.ContainsKey(type))
                weight = scoreWeights[type];

            int finalValue = score + weight;

            if (type == DiceScore.FourOfKind || type == DiceScore.FullHouse || type == DiceScore.Choice)
            {
                float ratio = Mathf.Clamp01(score / 20f);
                finalValue = (int)(weight * ratio) + score;

                if (IsLowValueHighCombo(type, score))
                    finalValue -= 20;
            }

            if (finalValue > bestValue)
            {
                bestValue = finalValue;
                best = type;
                found = true;
            }
        }

        if (!found)
        {
            return GetFirstEmptyScore();
        }

        return best;
    }

    private DiceScore GetFirstEmptyScore()
    {
        foreach (var slot in scoreBoard.slots)
        {
            if (!scoreBoard.IsLocked(slot.scoreType))
            {
                return slot.scoreType;
            }
        }
        return DiceScore.Aces;
    }

    // 점수 자동 선택
    private void AutoSelectScore(DiceScore type)
    {
        foreach (var slot in scoreBoard.slots)
        {
            if (slot.scoreType == type && !scoreBoard.IsLocked(type))
            {
                slot.toggle.isOn = true;
                break;
            }
        }
    }

    private bool IsScoreLocked(DiceScore type)
    {
        return scoreBoard.IsLocked(type);
    }
    private void FinishAITurn(DiceScore selectedScore)
    {
        int[] finalDice = gameManager.GetCurrentDiceValues();

        // 점수판 표시
        scoreBoard.gameObject.SetActive(true);
        scoreBoard.UpdateScoreBoard(finalDice);

        // 점수 자동 선택
        AutoSelectScore(selectedScore);
    }
}
