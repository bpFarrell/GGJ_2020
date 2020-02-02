using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    public static FocusCamera instance;
    public Camera cam;
    public RenderTexture rt;
    private void Awake() {
        instance = this;
        cam = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Object2RT(RenderTexture rt,GameObject focus) {
        cam.targetTexture = rt;
        int mask = focus.layer;
        focus.layer = LayerMask.NameToLayer("UI");
        foreach (Transform item in focus.transform.GetComponentsInChildren<Transform>()) {
            item.gameObject.layer = LayerMask.NameToLayer("UI");
        }
        transform.parent.position = focus.transform.position;
        cam.Render();
        cam.targetTexture = null;
        focus.layer = mask;
        foreach (Transform item in focus.transform.GetComponentsInChildren<Transform>()) {
            item.gameObject.layer = mask;
        }
        this.rt = rt;
    }
}
