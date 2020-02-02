using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public Animation anim;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        anim["CameraPivot"].wrapMode = WrapMode.Once;

        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
