using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI hook for OptionsLifecycle
/// </summary>
public class OptionsManager : MonoBehaviour {
    public void SetFontValue(float value) {
        Options.fontSize = value;
    }
    public void SetMusicValue(float value) {
        Options.musicSize = value * 5;
    }
    public void SetEffectValue(float value) {
        Options.effectSize = value * 5;
    }
    public void Serialize() {
        Options.Serialize();
    }
}
