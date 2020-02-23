using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {
    public CarManager carManager;
    public EndScreenManager endManager;
    public float secondsLong = 180;
    private float startTime;
    public bool isRunning;
    public float timeLeft {
        get {
            return secondsLong - (Time.time - startTime);
        }
    }
    private void Awake() {
        carManager.onFirstLoad = StartTimer;
    }
    void StartTimer() {
        startTime = Time.time;
        carManager.onFirstLoad = null;
        isRunning = true;
    }
    public void Update() {
        if (!isRunning) return;
        if (timeLeft < 0) {
            //Cadence.StateMachine.CadenceHierarchyStateMachine.Instance?.ActivateState("Level1/End");
            endManager.Activate();
            isRunning = false;
        }
    }
}
