using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSlot : MonoBehaviour
{
    [SerializeField] private Image faceImage;
    [SerializeField] private Sprite[] faceSprites;
    [SerializeField] private Button holdButton;
    [SerializeField] private GameObject holdText;

    private bool isHold = false;

    private void Start()
    {
        holdButton.onClick.AddListener(Hold);
        UpdateHoldUI();
    }

    // 주사위 값에 따른 이미지 변경
    public void SetFace(int value)
    {
        faceImage.sprite = faceSprites[value - 1];
    }

    public void SetHold(bool hold)
    {
        isHold = hold;
        UpdateHoldUI();
    }

    private void Hold()
    {
        isHold = !isHold;
        UpdateHoldUI();
    }

    public bool IsHoldSelected()
    {
        return isHold;
    }

    private void UpdateHoldUI()
    {
        if (holdText != null)
            holdText.SetActive(isHold);
    }
}
