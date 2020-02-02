using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public float fade = 1;
    private void OnDisable() {

        Shader.SetGlobalFloat("_Fill", 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_Fill", fade);
    }
}
