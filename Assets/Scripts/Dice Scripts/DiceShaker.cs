using DG.Tweening;
using UnityEngine;

public class DiceShaker : MonoBehaviour
{
    [SerializeField] private Transform cup;

    [Header("흔들기")]
    [SerializeField] private float shakeHeight;
    [SerializeField] private float shakeDuration;
    [SerializeField] private int shakeVibrato;

    [Header("기울기")]
    [SerializeField] private float tiltAngle;
    [SerializeField] private float pourDuration;

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
        seq.Append(cup.DOLocalRotate(new Vector3(originRot.eulerAngles.x, originRot.eulerAngles.y, originRot.eulerAngles.z + tiltAngle), pourDuration).SetEase(Ease.OutCubic));
        seq.Append(cup.DOLocalRotate(originRot.eulerAngles, 0.25f).SetEase(Ease.InCubic));
        seq.Append(cup.DOLocalMove(originPos, 0.2f));
    }
}
