using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> diceRollByCount = new List<AudioClip>();
    [SerializeField] private List<AudioClip> playerTurnNarrations = new List<AudioClip>();
    [SerializeField] private List<AudioClip> aiTurnNarrations = new List<AudioClip>();
    [SerializeField] private List<AudioClip> lastRollNarrations = new List<AudioClip>();
    [SerializeField] private AudioClip scoreSelectClip;
    [SerializeField] private AudioClip playerWinClip;
    [SerializeField] private AudioClip playerLoseClip;
    [SerializeField] private AudioClip drawClip;

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
    public void PlayPlayerTurnNarration()
    {
        if (playerTurnNarrations == null || playerTurnNarrations.Count == 0)
            return;

        int index = Random.Range(0, playerTurnNarrations.Count);
        sfxSource.PlayOneShot(playerTurnNarrations[index]);
    }
    public void PlayAITurnNarration()
    {
        if (aiTurnNarrations == null || aiTurnNarrations.Count == 0)
            return;

        int index = Random.Range(0, aiTurnNarrations.Count);
        sfxSource.PlayOneShot(aiTurnNarrations[index]);
    }
    public void PlayLastRollNarration()
    {
        if (lastRollNarrations == null || lastRollNarrations.Count == 0)
            return;

        int index = Random.Range(0, lastRollNarrations.Count);
        sfxSource.PlayOneShot(lastRollNarrations[index]);
    }
    public void PlayScoreSelectNarration()
    {
        if (scoreSelectClip == null)
            return;

        sfxSource.PlayOneShot(scoreSelectClip);
    }
    public void PlayPlayerWin()
    {
        if (playerWinClip != null)
            sfxSource.PlayOneShot(playerWinClip);
    }

    public void PlayPlayerLose()
    {
        if (playerLoseClip != null)
            sfxSource.PlayOneShot(playerLoseClip);
    }

    public void PlayDraw()
    {
        if (drawClip != null)
            sfxSource.PlayOneShot(drawClip);
    }
}
