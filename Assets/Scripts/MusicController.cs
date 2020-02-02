using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] musicTracks;
    public AudioSource musicOutput;

    public AudioSource MusicOutput { get => musicOutput; set => musicOutput = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
