using System;
using System.Collections.Generic;
using System.IO;
using Cadence.StateMachine;
using DG.Tweening;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour {
    public CarManager carManager;
    public Graphic fade;
    public TMP_Text scoreText;
    public List<TMP_Text> entryTextFields = new List<TMP_Text>(); 
    public string groupInput { get; set; }
    private string filePath => Path.Combine(Application.persistentDataPath, "score.json");
    private List<Tuple<string, int>> entries = new List<Tuple<string, int>>();
    
    public void Activate() {
        gameObject.SetActive(true);
        scoreText.text = "Score: " + carManager.numberOfCarsDelivered;
        entries = DeserializeGroup();
        for (var i = 0; i < entryTextFields.Count; i++) {
            TMP_Text entryTextField = entryTextFields[i];
            var entry = entries[i];
            entryTextField.text = entry.Item1 + " - " + entry.Item2;
        }
    }

    public void Submit() {
        if (!string.IsNullOrEmpty(groupInput) && !string.IsNullOrWhiteSpace(groupInput)) {
            SerializeGroup(groupInput, carManager.numberOfCarsDelivered);
        }
        Replay();
    }
    public void Replay() {
        if (fade != null) fade.DOColor(new Color(.14f, .14f, .14f, 1f), 0.15f);
        transform.DOScale(new Vector3(0, 0, 0), 0.3f).onComplete += () => {
            SceneManager.UnloadSceneAsync(1).completed += (handler) => {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
                CadenceHierarchyStateMachine.Instance.ActivateState("title");
                CadenceHierarchyStateMachine.Instance.GetActivatorList();
                CadenceHierarchyStateMachine.Instance.ActivateDefault();
            };
        };
    }
    private void SerializeGroup(string groupName, int score) {
        var json = new JSONObject();
        entries.Add(new Tuple<string, int>(groupName, score));
        for (var index = 0; index < entries.Count; index++) {
            var entry = entries[index];
            json["values"][index]["name"] = entry.Item1;
            json["values"][index]["score"].AsInt = entry.Item2;
        }
        File.WriteAllText(filePath, json.ToString());
    }
    private List<Tuple<string, int>> DeserializeGroup() {
        var workingList = new List<Tuple<string, int>>();
        if (!File.Exists(filePath)) {
            return workingList;
        }

        string fileOutput = File.ReadAllText(filePath);
        var json = JSONNode.Parse(fileOutput);
        
        for (var i = 0; i < json["values"].Count; i++) {
            var value = json["values"][i];
            workingList.Add(new Tuple<string, int>(value["name"], value["score"].AsInt));
        }
        
        workingList.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        
        return workingList;
    }
}
