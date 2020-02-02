using System.Collections;
using System.Collections.Generic;
using Cadence.StateMachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI hook for OptionsLifecycle
/// </summary>
public class OptionsManager : StateBase {
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

    public string state = "title/options";
    [Space] 
    public Slider fontSizeSlider;
    public Slider musicSizeSlider;
    public Slider effectSizeSlider;
    
    public override string stateName => state;
    public override void Activate() {
        fontSizeSlider.value = Options.fontSize;
        musicSizeSlider.value = Options.musicSize;
        effectSizeSlider.value = Options.effectSize;
    }

    public override void Deactivate() {
        
    }
}
