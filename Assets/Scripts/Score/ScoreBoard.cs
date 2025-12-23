using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private RawImage[] diceResultImages;
    [SerializeField] private Texture[] diceFaceTextures;
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

        for (int i = 0; i < diceResultImages.Length && i < dice.Length; i++)
        {
            int value = dice[i] - 1;
            diceResultImages[i].texture = diceFaceTextures[value];
            diceResultImages[i].gameObject.SetActive(true);
        }

        foreach (var slot in slots)
        {
            slot.toggle.onValueChanged.RemoveAllListeners();

            if (scoreData.IsLocked(slot.scoreType))
            {
                slot.SetLockedScore(scoreData.GetScore(slot.scoreType));
                continue;
            }

            int score = 0;

            // 조합이 성립하면 점수 계산
            if (possible.Contains(slot.scoreType))
            {
                score = ScoreCombo.CalculateScore(slot.scoreType, dice);
            }

            // 조합 여부 상관없이 점수 세팅
            slot.SetScore(score);

            // 아직 Lock 안 된 슬롯은 전부 선택 가능
            slot.toggle.interactable = true;
            slot.toggle.onValueChanged.AddListener((isOn) => OnSelect(slot, isOn));
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
