using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceModel : MonoBehaviour
{
    [SerializeField] private int _value = 1; 

    public int Value => _value;


    public void SetValue(int v)
    {
        _value = v;
    }
}
