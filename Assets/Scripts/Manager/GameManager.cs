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
    [SerializeField] private ResultPannel resultPannel;
    [SerializeField] private Transform[] holdSlotPoints;
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
        if (rollCount >= maxRoll || (rollCount > 0 && stoppedDiceCount < 5))
            return;

        rollCount++;

        holdPanel.SetActive(false);
        stoppedDiceCount = 0;
        int rollingCount = GetRollingDiceCount();
        AudioManager.Instance.PlayDiceRoll(rollingCount);

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
    }

    private void CollectAll()
    {
        int slotIndex = 0; // 빈 슬롯 순서용

        for (int i = 0; i < 5; i++)
        {
            if (dices[i] == null)
                continue;

            if (holdStates[i])
            {
                // 다음 빈 슬롯으로 이동
                dices[i].HoldTo(holdSlotPoints[slotIndex].position);
                slotIndex++;
            }
            else
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
        CollectAll();
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

        // 모든 주사위 무조건 회수
        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i] != null)
            {
                diceSpawner.Collect(dices[i]);
                dices[i] = null;
            }
            holdStates[i] = false;
        }

        ResetTurn();

        if (IsGameFinished())
        {
            ShowResultPanel();
            return;
        }

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
    public bool CanRoll()
    {
        // AI 턴이면 Roll 불가
        if (turnManager.CurrentTurn != TurnOwner.Player)
            return false;

        // 주사위가 아직 굴러가는 중이면 Roll 불가
        if (stoppedDiceCount < 5 && rollCount > 0)
            return false;

        // Roll 횟수 초과
        if (rollCount >= maxRoll)
            return false;

        return true;
    }
    private bool IsGameFinished()
    {
        return playerScoreData.IsAllLocked() && aiScoreData.IsAllLocked();
    }
    private void ShowResultPanel()
    {
        int playerTotal = playerScoreData.GetTotalScore();
        int aiTotal = aiScoreData.GetTotalScore();

        resultPannel.Show(playerTotal, aiTotal);
    }
    private int GetRollingDiceCount()
    {
        int count = 0;
        for (int i = 0; i < holdStates.Length; i++)
        {
            if (!holdStates[i])
                count++;
        }
        return count;
    }
}