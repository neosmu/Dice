using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreSlot : MonoBehaviour
{
    public DiceScore scoreType;

    public TextMeshProUGUI scoreText;   // 계산된 점수를 표시하는 텍스트
    public Toggle toggle;

    private int currentScore;
    private bool isLocked = false;     // 이미 선택되어있는지 파악 여부

    //  나온 주사위 값을 바탕으로 점수 표시
    public void SetScore(int score)
    {
        if (isLocked) return;

        currentScore = score;
        scoreText.text = score.ToString();
        toggle.interactable = true;
    }

    public void Lock()
    {
        isLocked = true;
        toggle.isOn = true;
        toggle.interactable = false;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public int GetScore()
    {
        return currentScore;
    }
}
