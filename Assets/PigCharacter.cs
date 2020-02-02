using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PigCharacter : MonoBehaviour {
    public float delay = 0f;
    private void Start() {
        transform.DOLocalMove(transform.localPosition, delay).onComplete = DoCharacter;
    }

    private void DoCharacter() {
        transform.DOPunchPosition((Vector3.up / 3), 0.2f).onComplete = () => {
            transform.DOPunchPosition((Vector3.up / 3), 0.2f).onComplete = () => {
                float rand = Random.Range(2.5f, 2.7f);
                transform.DOLocalMove(transform.localPosition, rand).onComplete = DoCharacter;
            };
        };
    }
}
