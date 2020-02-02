using DG.Tweening;
using UnityEngine;

public class CarBehavior : MonoBehaviour {
    public Vector3 target;
    public Transform punchTransform;
    public DropBox dropBox;
    
    private void Start() {
        transform.DOLocalMove(target, 5f).onComplete += () => {
            if (punchTransform == null) return;
            punchTransform.DOPunchRotation(new Vector3(3.5f, 0F, 0F), .3f).onComplete += () => {
                //transform.DOPunchRotation(new Vector3(-2.5f, 0F, 0F), .2f);
            };
            punchTransform.DOPunchScale(new Vector3(0f, 0f, -.25f), .2f, 1, 0.2f);
        };
    }
}
