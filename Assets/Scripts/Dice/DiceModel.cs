using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiceModel : MonoBehaviour
{
    [SerializeField] private int _value = 1;

    public int Value => _value;
    public bool IsHold { get; private set; }
    public int RollCount { get; private set; }

    // 값 변경 / 홀드 변경 이벤트
    public event Action OnValueChanged;
    public event Action OnHoldChanged;

    public void SetValue(int v)
    {
        _value = v;
        OnValueChanged?.Invoke();
    }

    public void SetHold(bool hold)
    {
        IsHold = hold;
        OnHoldChanged?.Invoke();
    }

    public void AddRollCount()
    {
        RollCount++;
    }

    public void ResetRollCount()
    {
        RollCount = 0;
    }
}
