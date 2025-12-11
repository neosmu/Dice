using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DiceSpawner diceSpawner;
    [SerializeField] private DiceController[] dices = new DiceController[5];
    [SerializeField] private GameObject holdPanel;
    [SerializeField] private DiceSlot[] diceUISlots;
    [SerializeField] private Button okButton;

    private int stoppedDiceCount = 0;

    private bool[] holdStates = new bool[5];  
    private int[] diceValues = new int[5];
    private int rollCount = 0;
    private const int maxRoll = 3;
    private void Start()
    {
        okButton.onClick.AddListener(ConfirmHoldSelection);
        holdPanel.SetActive(false);
    }

    public void RollAll()
    {
        if (rollCount >= maxRoll)
        {
            Debug.Log("더 이상 굴릴 수 없습니다.");
            return;
        }

        rollCount++;

        holdPanel.SetActive(false);
        stoppedDiceCount = 0;

        SpawnAndRoll();
    }

    private void SpawnAndRoll()
    {
        for (int i = 0; i < 5; i++)
        {
            bool hold = holdStates[i];
            if (hold)
            {
                dices[i] = null;
                DiceStop(i, diceValues[i]);
                continue;
            }

            DiceController dice = diceSpawner.SpawnDice(i);
            dice.gameManager = this;
            dice.diceIndex = i;

            dices[i] = dice;

            DiceModel model = dice.GetComponent<DiceModel>();
            model.SetHold(false);

            dice.Roll();
            model.AddRollCount();
        }
    }

    public void DiceStop(int index, int value)
    {
        diceValues[index] = value;
        stoppedDiceCount++;

        if (stoppedDiceCount == 5)
        {
            OpenHoldPanel();
        }
    }

    private void OpenHoldPanel()
    {
        holdPanel.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            diceUISlots[i].SetFace(diceValues[i]);
            diceUISlots[i].SetHold(holdStates[i]);
        }
        ScoreCombo.Print(diceValues);
        CollectAll();
    }

    private void CollectAll()
    {
        for (int i = 0; i < 5; i++)
        {
            if (dices[i] != null)
            {
                diceSpawner.Collect(dices[i]);
                dices[i] = null;
            }
        }
    }

    public void ConfirmHoldSelection()
    {
        for (int i = 0; i < 5; i++)
        {
            holdStates[i] = diceUISlots[i].IsHoldSelected();
        }

        holdPanel.SetActive(false);
    }
}