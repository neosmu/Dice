using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPannel : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text aiScoreText;

    public void Show(int playerScore, int aiScore)
    {
        gameObject.SetActive(true);

        playerScoreText.text = playerScore.ToString();
        aiScoreText.text = aiScore.ToString();

        if (playerScore > aiScore)
            resultText.text = "플레이어가 승리하였습니다.";
        else if (playerScore < aiScore)
            resultText.text = "AI가 승리하였습니다.";
        else
            resultText.text = "동점입니다.";
    }

    public void Close()
    {
        Application.Quit();
    }
}
