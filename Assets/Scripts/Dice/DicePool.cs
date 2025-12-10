using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePool : MonoBehaviour
{
    [SerializeField] private DiceController prefab;

    private Queue<DiceController> pool = new Queue<DiceController>();

    public DiceController Get()
    {
        if (pool.Count > 0)
        {
            DiceController dic = pool.Dequeue();
            dic.gameObject.SetActive(true);
            return dic;
        }

        return Instantiate(prefab);
    }

    public void Return(DiceController dice)
    {
        dice.gameObject.SetActive(false);
        pool.Enqueue(dice);
    }
}
