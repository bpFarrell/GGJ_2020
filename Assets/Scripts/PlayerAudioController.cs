using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public GameObject player;
    public AudioClip[] footstepClips;
    public AudioSource footstepSource1;
    public AudioSource footstepSource2;
    public Vector3 pelvis;
    public GameObject leftFoot;
    public Vector3 leftFootTransform;
    public float leftPelvisDif;
    public GameObject rightFoot;
    public Vector3 rightFootTransform;
    public float rightPelvisDif;
    private bool footstepPlayed;
    public int randomValue;
    public int lastRandomValue;

    public AudioClip[] hoeClips;
    public AudioClip[] scytheClips;
    public AudioClip[] wateringCanClips;
    public AudioClip[] seedClips;
    public AudioClip[] playerTossClips;
    public AudioSource toolSource;
    public ItemBase heldItem;

    // Start is called before the first frame update
    void Start()
    {
        footstepPlayed = true;
    }

    // Update is called once per frame
    void Update()
    {
        leftPelvisDif = (pelvis.y - leftFootTransform.y);
        rightPelvisDif = (pelvis.y - rightFootTransform.y);
        leftFootTransform = leftFoot.transform.position;
        rightFootTransform = rightFoot.transform.position;
        if (leftPelvisDif <= -0.22f)
        {
            if (footstepPlayed == false)
            {
                {
                    SelectRandom();
                    PlayRandom();
                }
            }
        }
        if (rightPelvisDif <= -0.22f)
        {
            if (footstepPlayed == false)
            {
                {
                    SelectRandom();
                    PlayRandom();
                }
            }
        }
        if (leftFootTransform.y >= -0.42f && footstepSource1.isPlaying == false)
        {
            footstepPlayed = false;
        }
        if (rightFootTransform.y >= -0.42f && footstepSource2.isPlaying == false)
        {
            footstepPlayed = false;
        }

        heldItem = GetComponent<PlayerController>().heldItem;
    }

    void SelectRandom()
    {
        randomValue = Random.Range(0, 4);
        if(randomValue == lastRandomValue)
        {
            SelectRandom();
        }
    }

    void PlayRandom()
    {
        footstepSource1.clip = footstepClips[randomValue];
        footstepSource1.Play();
        footstepPlayed = true;

        if (!footstepSource1.isPlaying)
        {
            footstepSource1.clip = footstepClips[randomValue];
            footstepSource1.Play();
            footstepPlayed = true;
        }
        if(!footstepSource2.isPlaying)
        {
            footstepSource2.clip = footstepClips[randomValue];
            footstepSource2.Play();
            footstepPlayed = true;
        }
        lastRandomValue = randomValue;
    }

    void ToolSound()
    {
        if (!toolSource.isPlaying)
        {
            if (heldItem.name == "hoe")
            {
                toolSource.clip = hoeClips[Random.Range(0, 3)];
                toolSource.Play();
            }
            if (heldItem.name == "scythe")
            {
                toolSource.clip = scytheClips[Random.Range(0, 3)];
                toolSource.Play();
            }
            if (heldItem.name == "wateringcan")
            {
                toolSource.clip = wateringCanClips[Random.Range(0, 3)];
                toolSource.Play();
            }
            if (heldItem.name == "seeds")
            {
                toolSource.clip = seedClips[Random.Range(0, 3)];
                toolSource.Play();
            }
            if (heldItem.name.Contains("SimpleMovement"))
            {
                toolSource.clip = seedClips[Random.Range(0, 3)];
                toolSource.Play();
            }
        }
    }
}
