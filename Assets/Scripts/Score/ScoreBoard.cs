using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public List<ScoreSlot> slots;

    private bool hasSelected = false;
    private ScoreSlot selectedSlot;

    // 현재 주사위 기준으로 점수판 갱신
    public void UpdateScoreBoard(int[] dice)
    {
        hasSelected = false;
        selectedSlot = null;

        var possible = ScoreCombo.GetPossibleScores(dice);

        foreach (var slot in slots)
        {
            if (slot.IsLocked())
                continue;

            slot.toggle.isOn = false;
            slot.toggle.interactable = false;

            if (possible.Contains(slot.scoreType))
            {
                int score = ScoreCombo.CalculateScore(slot.scoreType, dice);
                slot.SetScore(score);
                slot.toggle.interactable = true;
            }
            else
            {
                slot.SetScore(0);
            }

            slot.toggle.onValueChanged.RemoveAllListeners();
            slot.toggle.onValueChanged.AddListener((isOn) => OnSelect(slot, isOn));
        }
    }

    // 점수 선택 처리
    private void OnSelect(ScoreSlot slot, bool isOn)
    {
        if (isOn == false)
            return; 

        if (hasSelected == true)
            return;

        hasSelected = true;
        selectedSlot = slot;

        slot.Lock();

        foreach (var s in slots)
        {
            if (s != slot && s.IsLocked() == false)
                s.toggle.interactable = false;
        }

        Debug.Log($"선택됨: {slot.scoreType}, 점수: {slot.GetScore()}");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
