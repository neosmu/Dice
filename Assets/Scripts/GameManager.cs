using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DiceSpawner diceSpawner;
    [SerializeField] private DiceController[] dices = new DiceController[5];

    [SerializeField] private GameObject holdPanel;
    [SerializeField] private DiceSlot[] diceUISlots;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button okButton;

    [Header("점수판")]
    [SerializeField] private ScoreBoard scoreBoard;
    [SerializeField] private Button scoreBoardCloseButton;

    [SerializeField] private TurnManager turnManager;
    [SerializeField] private DiceAutoAI diceAutoAI;

    private int stoppedDiceCount = 0;

    private bool[] holdStates = new bool[5];
    private int[] diceValues = new int[5];

    private int rollCount = 0;
    private const int maxRoll = 3;
    private ScoreData playerScoreData;
    private ScoreData aiScoreData;
    private ScoreData currentScoreData;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseHoldPanel);
        okButton.onClick.AddListener(OpenScoreBoard);
        scoreBoardCloseButton.onClick.AddListener(CloseScoreBoard);

        turnManager.OnTurnStarted += OnTurnStarted;

        holdPanel.SetActive(false);
        scoreBoard.gameObject.SetActive(false);
        playerScoreData = new ScoreData();
        aiScoreData = new ScoreData();
        turnManager.StartGame(); // 게임 시작
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
            if (holdStates[i])
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

    private void OnTurnStarted(TurnOwner owner)
    {
        ResetTurn();

        if (owner == TurnOwner.Player)
        {
            currentScoreData = playerScoreData;
        }
        else
        {
            currentScoreData = aiScoreData;
            StartCoroutine(StartAITurn());
        }
        scoreBoard.SetScoreData(currentScoreData);
    }
    public void DiceStop(int index, int value)
    {
        diceValues[index] = value;
        stoppedDiceCount++;

        if (stoppedDiceCount != 5)
            return;

        if (turnManager.CurrentTurn == TurnOwner.Player)
        {
            if (rollCount >= maxRoll)
            {
                OpenScoreBoardDirect();
            }
            else
            {
                OpenHoldPanel();
            }
        }
        else
        {
            CollectAll();
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

    private void ApplyHoldSelectionFromUI()
    {
        for (int i = 0; i < 5; i++)
        {
            holdStates[i] = diceUISlots[i].IsHoldSelected();
        }
    }

    private void CloseHoldPanel()
    {
        ApplyHoldSelectionFromUI();
        holdPanel.SetActive(false);
    }

    private void OpenScoreBoard()
    {
        ApplyHoldSelectionFromUI();
        holdPanel.SetActive(false);

        scoreBoard.gameObject.SetActive(true);
        scoreBoard.UpdateScoreBoard(diceValues);
    }
    private void OpenScoreBoardDirect()
    {
        holdPanel.SetActive(false);

        CollectAll();

        scoreBoard.gameObject.SetActive(true);
        scoreBoard.UpdateScoreBoard(diceValues);
    }
    private void CloseScoreBoard()
    {
        scoreBoard.Close();
        turnManager.EndTurn();
    }
    private void ResetTurn()
    {
        rollCount = 0;

        for (int i = 0; i < 5; i++)
        {
            holdStates[i] = false;
        }
    }
    private IEnumerator StartAITurn()
    {
        yield return new WaitForSeconds(0.5f);
        diceAutoAI.PlayAITurn();
    }
    public int[] GetCurrentDiceValues()
    {
        return diceValues;
    }

    // AI가 Roll 실행
    public void RollDice()
    {
        RollAll();
    }

    public bool IsAllDiceStopped()
    {
        return stoppedDiceCount == 5;
    }

    // 특정 숫자 주사위 Hold
    public void HoldValue(int value)
    {
        for (int i = 0; i < diceValues.Length; i++)
        {
            if (diceValues[i] == value)
            {
                holdStates[i] = true;
            }
        }
    }
}