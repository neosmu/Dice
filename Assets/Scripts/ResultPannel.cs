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
            resultText.text = "Player Win";
        else if (playerScore < aiScore)
            resultText.text = "AI Win";
        else
            resultText.text = "Draw";
    }

    public void Close()
    {
        Application.Quit();
    }
}
