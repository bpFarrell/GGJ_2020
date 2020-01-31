using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cadence.Utility {
        
    /// <summary>
    /// Base class with a lazy singleton reference.
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new() {
        protected static T _Instance = null;

        public static T instance => _Instance == null ? _Instance = new T() : _Instance;
    }

    /// <summary>
    /// MonoBehaviour with a lazy singleton reference.
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
        protected static T _Instance = null;

        public static T Instance {
            get {
                T[] objects;
                if (_Instance == null) {
                    objects = FindObjectsOfType<T>();
                    if (objects.Length <= 0) return _Instance = null;
                    if (objects.Length > 1) {
                        for (int i = 1; i < objects.Length; i++) {
                            Debug.Log("Destroying duplicates of " + typeof(T) +
                                      ". Copies of SingletonBehaviours are not advised.");
                            Destroy(objects[i]);
                        }
                    }

                    return _Instance = objects[0];
                }

                return _Instance;
            }
        }

        private void OnDisable() {
            _Instance = null;
        }

        private void OnEnable() {
            var i = Instance;
        }
    }

    /// <summary>
    /// UIMonoBehaviour with a lazy singleton reference.
    /// </summary>
    public abstract class UISingletonBehaviour<T> : UIMonoBehaviour where T : UISingletonBehaviour<T> {
        protected static T _Instance = null;

        public static T Instance {
            get {
                T[] objects;
                if (_Instance == null) {
                    objects = FindObjectsOfType<T>();
                    if (objects.Length <= 0) return _Instance = null;
                    else if (objects.Length > 1) {
                        for (int i = 1; i < objects.Length; i++) {
                            Debug.Log("#UI MonoBehaviour#Destroying duplicates of " + typeof(T).ToString() +
                                      ". Copies of UISingletonBehaviours are not advised.");
                            Destroy(objects[i]);
                        }
                    }

                    return (_Instance = objects[0]);
                }

                return _Instance;
            }
        }

        private void OnDisable() {
            _Instance = null;
        }
    }

    /// <summary>
    ///  Abstract class with Arena specific helper functions for implementing UI behaviors
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIMonoBehaviour : MonoBehaviour {
        private RectTransform _RectTransform;
        private Canvas _LocalCanvas;
        private GraphicRaycaster _LocalRaycaster;
        private static Canvas _MainCanvas;
        private static GraphicRaycaster _MainRaycaster;

        /// <summary>
        /// Defined in UIMonoBehavior base class. Traverses parents to find immediate canvas in hierarchy to define this variable on first use...
        /// </summary>
        public Canvas localCanvas => _LocalCanvas ?? (_LocalCanvas = GetLocalCanvas());

        /// <summary>
        /// Defined in UIMonoBehavior base class. Uses FindGameObjectWithTag() to define this static variable on first use. (Sorry Brandon)...
        /// </summary>
        public static Canvas mainCanvas => _MainCanvas ?? (_MainCanvas = GetMainCanvas());

        /// <summary>
        /// Retrieves attached RectTransform on component's gameObject. Returns Null if none with Debug.Log warning.
        /// </summary>
        public RectTransform rectTransform => _RectTransform ?? (_RectTransform = GetRectTransform());

        /// <summary>
        /// Retrieves attached GraphicRaycaster on the MainCanvas Object.
        /// </summary>
        public static GraphicRaycaster mainRaycaster => _MainRaycaster ?? (_MainRaycaster = GetMainRaycaster());

        private static GraphicRaycaster GetMainRaycaster() {
            return _MainCanvas.GetComponent<GraphicRaycaster>();
        }

        /// <summary>
        /// Returns True if either the MainCanvas or the provided local canvas return any hit info
        /// at the provided input location.
        /// </summary>
        /// <param name="canvas">Canvas to queury.</param>
        /// <param name="input">The screen space position for the query.</param>
        /// <returns>True if a UI element was hit on either canvas</returns>
        public static bool RaycastBothCanvasForUI(Canvas canvas, Vector3 input) {
            return (RaycastMainCanvasForUI(input) || RaycastLocalCanvasForUI(canvas, input));
        }

        /// <summary>
        /// Returns True if the MainCanvas has any UI Elements at the provided screen space position.
        /// </summary>
        /// <param name="input">The screen space position for the query.</param>
        /// <returns>True if a UI element was hit in the Arena canvas canvas</returns>
        public static bool RaycastMainCanvasForUI(Vector3 input) {
            return RaycastLocalCanvasForUI(mainCanvas, input);
        }

        /// <summary>
        /// Returns True if the provided canvas has any UI Elements at the provided screen space position.
        /// </summary>
        /// <param name="canvas">Canvas to queury.</param>
        /// <param name="input">The screen space position for the query.</param>
        /// <returns>True if a UI element was hit in the provided canvas</returns>
        public static bool RaycastLocalCanvasForUI(Canvas canvas, Vector3 input) {
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> hit = new List<RaycastResult>();
            PointerEventData ped = new PointerEventData(null);
            ped.position = input;
            gr.Raycast(ped, hit);
            return hit.Count > 0;
        }

        /// <summary>
        /// Returns a screenspace Rect from a RectTransform
        /// </summary>
        /// <param name="rectTransform">RectTransform to generate a Rect from</param>
        /// <returns></returns>
        public static Rect RectTransformToScreenSpace(RectTransform rectTransform) {
            Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
            var position = rectTransform.position;
            return new Rect(position.x, Screen.height - position.y, size.x, size.y);
            
        }

        private static Canvas GetMainCanvas() {
            Canvas target = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            if (target == null)
                Debug.Log(
                    "#UI MonoBehaviour#There is no object in scene with \"MainCanvas\" Tag. Please assign a canvas with the Tag.");
            return target;
        }

        private RectTransform GetRectTransform() {
            RectTransform target = transform as RectTransform;
            if (target == null)
                Debug.Log("#UI MonoBehaviour#UI Element \"" + transform.name + "\" does not have a RectTransform");
            return target;
        }
        private Canvas GetLocalCanvas() {
            Canvas target = rectTransform.GetComponentInParent<Canvas>();
            if (target == null)
                Debug.Log("#UI MonoBehaviour#UI Element \"" + transform.name + "\" does not have a local canvas");
            return target;
        }
    }

    public static class TransformEX {
        public static Transform Find(this Transform transform, string name, bool searchInactive = false) {
            if (!searchInactive) return transform.Find(name);
            Transform target = null;
            Transform[] trs = transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs) {
                if (t.name == name) {
                    target = t;
                }
            }

            return target;
        }
    }

}