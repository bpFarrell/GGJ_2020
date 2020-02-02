using System;
using System.IO;
using Cadence.Lifecycle;
using SimpleJSON;
using TMPro;
using UnityEngine;


public class Options : LifecycleModule
{
    public static float fontSize;
    public static float musicSize;
    public static float effectSize;
    
    public static readonly string OptionPath = Path.Combine(Application.persistentDataPath, "//options.json");

    public static void Serialize() {
        JSONObject json = new JSONObject();
        json["font_size"] = fontSize;
        json["music_size"] = musicSize;
        json["effect_size"] = effectSize;

        string jsonString = json.ToString();
        

        if (File.Exists(OptionPath)) {
            File.Create(OptionPath);
        }
        File.WriteAllText(OptionPath, jsonString);
    }
    
    public static void Deserialize() {
        if (!File.Exists(OptionPath)) {
            fontSize = 24;
            musicSize = 75;
            effectSize = 75;
            return;
        }

        String jsonString = File.ReadAllText(OptionPath);
        JSONNode json = JSONNode.Parse(jsonString);

        fontSize = json["font_size"].AsFloat;
        musicSize = json["music_size"].AsFloat;
        effectSize = json["effect_size"].AsFloat;
    }
    
    protected override void OnEnable() {
        Deserialize();
    }

    protected override void OnDisable() { }
}
