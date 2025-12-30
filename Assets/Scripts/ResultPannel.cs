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
        {
            resultText.text = "플레이어가 승리하였습니다.";
            AudioManager.Instance.PlayPlayerWin();
        }
        else if (playerScore < aiScore)
        {
            resultText.text = "AI가 승리하였습니다.";
            AudioManager.Instance.PlayPlayerLose();
        }
        else
        {
            resultText.text = "동점입니다.";
            AudioManager.Instance.PlayDraw();
        }
    }

    public void Close()
    {
        Application.Quit();
    }
}
