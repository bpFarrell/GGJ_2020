using DG.Tweening;
using UnityEngine;

public class CarBehavior : MonoBehaviour {
    public Vector3 target;
    public Transform punchTransform;
    public DropBox dropBox;
    
    private void Start() {
        transform.DOLocalMove(target, 5f).onComplete += () => {
            if (punchTransform == null) return;
            punchTransform.DOPunchRotation(new Vector3(5F, 0F, 0F), .3f);
            punchTransform.DOPunchScale(new Vector3(0f, 0f, -.1f), .3f, 1, 0.2f);
        };
    }
    public void Depart() {
        transform.DOLocalMove(target + Vector3.forward * 7f, 5f).onComplete += () => {
            Destroy(gameObject);
        };
    }
}
