using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowLogic : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        Vector3 pos = target.transform.position;
        pos.y = 0.1f;
        transform.position = pos;
        transform.parent = target;
        transform.eulerAngles = new Vector3(90, 0, 0);
        float dist = target.position.y - pos.y;
        transform.localScale = Vector3.one * (0.5f + dist);
    }
}
