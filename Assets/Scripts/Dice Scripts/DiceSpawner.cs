using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    [SerializeField] private DicePool dicePool;
    [SerializeField] private Transform diceCupIn;

    public DiceController SpawnDice(int index)
    {
        DiceController dice = dicePool.Get();
        dice.transform.position = diceCupIn.position;
        dice.transform.rotation = Random.rotation;
        return dice;
    }

    public void Collect(DiceController dice)
    {
        dicePool.Return(dice);
    }
}
