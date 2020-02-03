using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float secondsLong = 180;
    private float startTime;
    public bool isRunning;
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
        isRunning = false;
    }
    public void Update() {
        if (!isRunning) return;
        if (timeLeft < 0) {
            Cadence.StateMachine.CadenceHierarchyStateMachine.Instance.ActivateState("Title/Main");
        }
    }
}
