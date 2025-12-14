using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public List<ScoreSlot> slots;

    private bool hasSelected = false;
    private ScoreSlot selectedSlot;

    public void UpdateScoreBoard(int[] dice)
    {
        hasSelected = false;
        selectedSlot = null;

        var possible = ScoreCombo.GetPossibleScores(dice);

        foreach (var row in slots)
        {
            row.toggle.isOn = false;
            row.toggle.interactable = false;

            if (possible.Contains(row.scoreType))
            {
                int score = ScoreCombo.CalculateScore(row.scoreType, dice);
                row.SetScore(score);
            }
            else
            {
                row.SetScore(0);
            }

            row.toggle.onValueChanged.RemoveAllListeners();
            row.toggle.onValueChanged.AddListener(
                (isOn) => OnSelect(row, isOn)
            );
        }
    }

    private void OnSelect(ScoreSlot slot, bool isOn)
    {
        if (!isOn || hasSelected) return;

        hasSelected = true;
        selectedSlot = slot;

        foreach (var r in slots)
        {
            r.Lock();
        }

        Debug.Log($"선택됨: {slot.scoreType}, 점수: {slot.GetScore()}");
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
