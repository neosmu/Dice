using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceView : MonoBehaviour
{
    [SerializeField] private DiceModel model;

    public void OnValueChanged()
    {
        Debug.Log("DiceView: 값 업데이트됨 → " + model.Value);
    }
}
