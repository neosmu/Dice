using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreSlot : MonoBehaviour
{
    public DiceScore scoreType;

    public TextMeshProUGUI scoreText;
    public Toggle toggle;

    private int currentScore;
    private bool isLocked = false;

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
        toggle.interactable = false;
    }

    public int GetScore()
    {
        return currentScore;
    }
}
