using DG.Tweening;
using UnityEngine;
using System;

public class DiceShaker : MonoBehaviour
{
    [SerializeField] private Transform cup;
    [SerializeField] private Transform pourPoint;

    [Header("흔들기")]
    [SerializeField] private float shakeHeight;
    [SerializeField] private float shakeDuration;
    [SerializeField] private int shakeVibrato;

    [Header("기울기")]
    [SerializeField] private float tiltAngle;
    [SerializeField] private float pourDuration;

    public Action OnPour; // ← 쏟는 타이밍 콜백

    private Vector3 originPos;
    private Quaternion originRot;
    private Sequence seq;

    private void Awake()
    {
        originPos = cup.localPosition;
        originRot = cup.localRotation;
    }

    public void PlayShakeAndPour()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }

        seq = DOTween.Sequence();
        seq.Append(cup.DOLocalMoveY(originPos.y + shakeHeight, shakeDuration).SetLoops(shakeVibrato, LoopType.Yoyo).SetEase(Ease.InOutSine));
        seq.Append(cup.DOLocalRotate(originRot.eulerAngles + new Vector3(0f, 0f, tiltAngle), pourDuration).SetEase(Ease.OutCubic));

        seq.AppendCallback(() =>
        {
            OnPour?.Invoke();
        });

        seq.Append(cup.DOLocalRotate(originRot.eulerAngles, 0.25f).SetEase(Ease.InCubic));
        seq.Append(cup.DOLocalMove(originPos, 0.2f));
    }

    public Vector3 GetPourPosition()
    {
        return pourPoint.position;
    }
}
