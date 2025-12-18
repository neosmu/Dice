using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public List<ScoreSlot> slots;

    private bool hasSelected = false;
    private ScoreData scoreData;

    public void SetScoreData(ScoreData data)
    {
        scoreData = data;
    }
    // 현재 주사위 기준으로 점수판 갱신
    public void UpdateScoreBoard(int[] dice)
    {
        hasSelected = false;
        var possible = ScoreCombo.GetPossibleScores(dice);

        foreach (var slot in slots)
        {
            slot.toggle.onValueChanged.RemoveAllListeners();

            if (scoreData.IsLocked(slot.scoreType))
            {
                slot.SetLockedScore(scoreData.GetScore(slot.scoreType));
                continue;
            }

            if (possible.Contains(slot.scoreType))
            {
                int score = ScoreCombo.CalculateScore(slot.scoreType, dice);
                slot.SetScore(score);
                slot.toggle.onValueChanged.AddListener(
                    (isOn) => OnSelect(slot, isOn)
                );
            }
            else
            {
                slot.SetScore(0);
                slot.toggle.interactable = false;
            }
        }
    }

    // 점수 선택 처리
    private void OnSelect(ScoreSlot slot, bool isOn)
    {
        if (!isOn || hasSelected)
            return;

        hasSelected = true;

        scoreData.LockScore(slot.scoreType, slot.GetScore());

        foreach (var s in slots)
        {
            s.toggle.interactable = false;
        }
        Debug.Log($"[ScoreBoard]선택: {slot.scoreType}, 점수: {slot.GetScore()}");
    }
    public bool IsLocked(DiceScore type)
    {
        return scoreData != null && scoreData.IsLocked(type);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
