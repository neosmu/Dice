using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnOwner
{
    Player,
    AI
}

public class TurnManager : MonoBehaviour
{
    public TurnOwner CurrentTurn { get; private set; }

    public Action<TurnOwner> OnTurnStarted;

    public void StartGame()
    {
        CurrentTurn = TurnOwner.Player;
        OnTurnStarted?.Invoke(CurrentTurn);
    }

    public void EndTurn()
    {
        CurrentTurn = CurrentTurn == TurnOwner.Player ? TurnOwner.AI : TurnOwner.Player;
        OnTurnStarted?.Invoke(CurrentTurn);
    }
}
