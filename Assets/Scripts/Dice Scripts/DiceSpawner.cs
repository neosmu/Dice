using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    [SerializeField] private DicePool dicePool;
    [SerializeField] private Transform[] spawnPoints;

    public DiceController SpawnDice(int index)
    {
        DiceController dice = dicePool.Get();
        dice.transform.position = spawnPoints[index].position;
        return dice;
    }

    public void Collect(DiceController dice)
    {
        dicePool.Return(dice);
    }
}
