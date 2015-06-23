using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    public AudioClip bgm;
    public AudioClip lineClear;
    public AudioClip move;
    public AudioClip turn;
    public AudioClip gameOver;

    [HideInInspector]
    public AudioSource a;

	private void Start()
    {
        a = GetComponent<AudioSource>();
        a.clip = bgm;
        a.loop = true;
        a.Play();
    }

    public void PlayLineClear()
    {
        a.PlayOneShot(lineClear);
    }

    public void PlayMove()
    {
        a.PlayOneShot(move);
    }

    public void PlayTurn()
    {
        a.PlayOneShot(turn);
    }

    public void PlayGameOver()
    {
        a.PlayOneShot(gameOver);
    }
}
