using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioClip[] BGMs;
    AudioSource audioSource;
    public AudioSource AudioSource { get { return audioSource; } set { audioSource = value; } }
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBGM(1);
    }

    public void ChangeBgm(string stage)
    {
        switch (stage)
        {
            case "EnterStage1":
                PlayBGM(1);
                break;

            case "EnterStage2":
                PlayBGM(2);
                break;

            case "EnterStage3":
                PlayBGM(3);
                break;
        }
        
    }

    void PlayBGM(int idx)
    {
        StartCoroutine(MusicFadeout());
        audioSource.clip = BGMs[idx];
        audioSource.Play();
        StartCoroutine(MusicFadein());
    }

    public void TurnOnUI(bool turn)
    {
        if (turn)
        {
            audioSource.volume = 0.03f;
        }
        else
        {
            audioSource.volume = 0.1f;
        }
    }

    IEnumerator MusicFadein()
    {
        while (audioSource.volume < 0.1)
        {
            Debug.Log("in");
            audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.2f);
        }
        StopCoroutine(MusicFadein());
    }

    IEnumerator MusicFadeout()
    {
        while (audioSource.volume > 0)
        {
            Debug.Log("out");
            audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.2f);
        }
        StopCoroutine(MusicFadeout());
    }
}
