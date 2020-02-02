using DG.Tweening;
using UnityEngine;

public class CarRumble : MonoBehaviour {
    [Range(0f, .2f)]
    public float rumble = 1.0f;

    public Transform target = null;

    private void Update() {
        if (target == null) target = transform;
        target.DOShakePosition(0.05f, rumble, 5, 45F);
    }
}
