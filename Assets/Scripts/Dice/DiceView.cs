using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceView : MonoBehaviour
{
    [SerializeField] private DiceModel model;

    public void OnValueChanged()
    {
        Debug.Log("값: " + model.Value);
    }
}
