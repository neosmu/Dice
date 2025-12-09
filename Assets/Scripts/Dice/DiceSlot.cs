using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSlot : MonoBehaviour
{
    [SerializeField] private Image faceImage;
    [SerializeField] private Sprite[] faceSprites; 
    [SerializeField] private Button holdButton;  

    private bool isHold = false;

    private void Start()
    {
        holdButton.onClick.AddListener(Hold);
    }

    public void SetFace(int value)
    {
        faceImage.sprite = faceSprites[value - 1];
    }

    public void SetHold(bool hold)
    {
        isHold = hold;
    }

    private void Hold()
    {
        isHold = !isHold;
    }
    public bool IsHoldSelected()
    {
        return isHold;
    }
}
