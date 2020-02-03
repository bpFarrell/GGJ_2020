using System;
using System.Collections;
using System.Collections.Generic;
using Cadence.Utility;
using DG.Tweening;
using UnityEngine;

public class PigTimer : SingletonBehaviour<PigTimer>
{
    public List<PigTimerInstance> pigList = new List<PigTimerInstance>();

    public bool start = false;
    private void Update() {
        if (start) {
            start = false;
            SetTimer(15f, () => {
                Debug.Log("Timer Complete");
            });
        }
    }

    public void SetTimer(float duration, Action callback) {
        Reset();
        float step = duration / pigList.Count;
        for (int i = 0; i < pigList.Count; i++) {
            int index = i;
            transform.DOLocalMove(transform.localPosition, step * (i + 1)).onComplete = () => {
                pigList[index].JumpOff();
                if(index == pigList.Count-1) callback.Invoke();
            };
        }
    }

    private void Reset() {
        foreach (var timerInstance in pigList) {
            timerInstance.Reset();
        }
    }
}
