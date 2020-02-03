using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public CarManager carManager;
    public GameTimer gameTimer;

    private void Update() {
        scoreText.text = $"{carManager.numberOfCarsDelivered}";
        timeText.transform.parent.gameObject.SetActive(gameTimer.isRunning);
        System.TimeSpan span = new TimeSpan(0, 0, (int) gameTimer.timeLeft);
        timeText.text = $"{span.Minutes}:{span.Seconds}";
    }
}
