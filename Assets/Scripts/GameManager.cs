using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DiceController[] dices;
    [SerializeField] private GameObject holdPanel;
    [SerializeField] private DiceSlot[] diceUISlots;
    [SerializeField] private Button okButton;

    private int stoppedDiceCount = 0;

    private void Start()
    {
        for (int i = 0; i < dices.Length; i++)
        {
            dices[i].gameManager = this;
            dices[i].diceIndex = i;
        }

        okButton.onClick.AddListener(ConfirmHoldSelection);
        holdPanel.SetActive(false);
    }

    public void RollAll()
    {
        holdPanel.SetActive(false);
        stoppedDiceCount = 0;

        for (int i = 0; i < dices.Length; i++)
        {
            DiceModel model = dices[i].GetComponent<DiceModel>();

            if (model.IsHold)
            {
                DiceStop();
            }
            else
            {
                dices[i].Roll();
                model.AddRollCount();
            }
        }
    }

    public void DiceStop()
    {
        stoppedDiceCount++;

        if (stoppedDiceCount == dices.Length)
        {
            OpenHoldPanel();
        }
    }

    private void OpenHoldPanel()
    {
        holdPanel.SetActive(true);

        for (int i = 0; i < dices.Length; i++)
        {
            DiceModel model = dices[i].GetComponent<DiceModel>();

            diceUISlots[i].SetFace(model.Value);
            diceUISlots[i].SetHold(model.IsHold);
        }
    }

    public void ConfirmHoldSelection()
    {
        for (int i = 0; i < dices.Length; i++)
        {
            bool hold = diceUISlots[i].IsHoldSelected();
            dices[i].GetComponent<DiceModel>().SetHold(hold);
        }

        holdPanel.SetActive(false);
    }

}