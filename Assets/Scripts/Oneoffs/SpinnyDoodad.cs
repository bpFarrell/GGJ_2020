using System;
using Cadence.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpinnyDoodad : UIMonoBehaviour
{
    private Image graphic;

    private void Update() {
        rectTransform.DORotate()
    }
}
