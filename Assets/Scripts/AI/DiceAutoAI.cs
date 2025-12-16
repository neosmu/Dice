using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAutoAI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private const int MAX_ROLL = 3;

    public void TestAI()
    {
        StartCoroutine(AIPlayOnce());
    }
    private IEnumerator AIPlayOnce()
    {
        for (int roll = 0; roll < MAX_ROLL; roll++)
        {
            Debug.Log($"[AI] Roll {roll + 1}");

            // 현재 주사위 값 확인
            int[] dice = gameManager.GetCurrentDiceValues();

            // Hold 판단
            DecideHold(dice);

            // Roll 실행
            gameManager.RollDice();

            // 모든 주사위가 멈출 때까지 대기
            yield return new WaitUntil(() => gameManager.IsAllDiceStopped());

            int[] result = gameManager.GetCurrentDiceValues();
            Debug.Log("[AI] Dice Result : " + string.Join(", ", result));
        }
    }

    // Hold 판단 로직
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
}
