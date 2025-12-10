using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private DiceModel model;
    [SerializeField] private DiceView view;
    [SerializeField] private Transform[] faces;
    [SerializeField] private Rigidbody rb;

    public GameManager gameManager;
    public int diceIndex;

    private bool isRolling = false;
    private float rollStartTime;
    private float blockDuration = 0.25f;

    private void Update()
    {
        if (!isRolling)
            return;
        if (Time.time - rollStartTime < blockDuration)
            return;

        if (rb.velocity.magnitude < 0.1f && rb.angularVelocity.magnitude < 0.1f)
        {
            isRolling = false;

            int topValue = GetTopFace();
            model.SetValue(topValue);
            view.OnValueChanged();

            gameManager.DiceStop(diceIndex, topValue);
        }
    }

    public void Roll()
    {
        isRolling = true;
        rollStartTime = Time.time;

        transform.position += Vector3.up * 0.3f;

        rb.AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(4f, 7f), Random.Range(-2f, 2f)), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
    }

    private int GetTopFace()
    {
        float maxDot = -1f;
        int result = 0;

        foreach (var face in faces)
        {
            float dot = Vector3.Dot(face.up, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                result = face.GetComponent<FaceId>().value;
            }
        }

        return result;
    }
}
