using Cadence.Lifecycle;
using UnityEngine;


public class Options : LifecycleModule
{
    public static float fontSize;
    public static float musicSize;
    public static float effectSize;

    public static void Serialize() {
        
    }
    
    public static void Deserialize() {
        
    }
    
    protected override void OnEnable() {
        Deserialize();
    }

    protected override void OnDisable() {
    }
}
