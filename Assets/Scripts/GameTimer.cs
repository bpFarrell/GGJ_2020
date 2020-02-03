using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float secondsLong = 180;
    private float startTime;
    public float timeLeft {
        get {
            return secondsLong - (Time.time - startTime);
        }
    }
    private void OnEnable() {
        CarManager.onFirstLoad = StartTimer;
    }
    void StartTimer() {
        startTime = Time.time;
        CarManager.onFirstLoad = null;
    }
    public void Update() {
        if (timeLeft < 0) {
            Cadence.StateMachine.CadenceHierarchyStateMachine.Instance.ActivateState("Title/Main");
        }
    }
}
