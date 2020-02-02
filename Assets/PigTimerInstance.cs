using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PigTimerInstance : MonoBehaviour {
    public bool jump = false;
    public bool reset = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private void Awake() {
        var transform1 = transform;
        startPosition = transform1.position;
        startRotation = transform1.rotation;
    }

    private void Update() {
        if (jump) {
            jump = false;
            JumpOff();
        }

        if (reset) {
            reset = false;
            Reset();
        }
    }


    public void Reset() {
        transform.DOKill(false);
        transform.DOJump(startPosition, 0.7f, 1, 0.5f);
    }

    public void JumpOff() {
        transform.DOJump(transform.position + (Vector3.forward * 0.3f) + (Vector3.down * transform.position.y), 1.5f, 1, 0.7f);
    }
}
