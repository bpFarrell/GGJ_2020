using System;
using Cadence.Utility;
using UnityEngine;

namespace Cadence {
    [RequireComponent(typeof(RectTransform))]
    public class CadenceContentSizeFitter : UIMonoBehaviour {
        private void LateUpdate() {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, CalculateChildrenSize());
        }
        private float CalculateChildrenSize() {
            float result = 0f;
            foreach (RectTransform transform in rectTransform) {
                result += transform.sizeDelta.y;
            }
            return result;
        }
    }
}
