using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] musicTracks;
    public AudioSource musicOutput;

    public AudioSource MusicOutput { get => musicOutput; set => musicOutput = value; }
    void Update()
    {
        if(!MusicOutput.isPlaying)
        {
            SelectRandom();
        }
    }

    void SelectRandom()
    {
        MusicOutput.clip = musicTracks[Random.Range(0,5)];
        MusicOutput.Play();
    }
}
