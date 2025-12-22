using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> diceRollByCount = new List<AudioClip>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayDiceRoll(int diceCount)
    {
        if (diceRollByCount == null || diceRollByCount.Count == 0)
            return;

        int index = Mathf.Clamp(diceCount - 1, 0, diceRollByCount.Count - 1);
        AudioClip clip = diceRollByCount[index];

        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
