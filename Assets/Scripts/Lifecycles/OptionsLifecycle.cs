using Cadence.Lifecycle;
using UnityEngine;


public class Options : LifecycleModule
{
    public float fontSize;

    public void Serialize() {
        
    }
    
    public void Deserialize() {
        
    }
    
    protected override void OnEnable()
    {
        Deserialize();
    }

    protected override void OnDisable()
    {
        throw new System.NotImplementedException();
    }
}
