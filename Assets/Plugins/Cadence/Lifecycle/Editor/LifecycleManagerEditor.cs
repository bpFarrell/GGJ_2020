using System;
using Cadence.Lifecycle;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LifecycleManager))]
public class LifecycleManagerEditor : Editor {
    private SerializedObject sObject;
    
    private void Awake() {
        sObject = serializedObject;
    }
    private void OnEnable() {
        EditorApplication.update += Update;
    }
    private void OnDisable() {
        EditorApplication.update -= Update;
    }
    public static void DrawUILine(Color color, int thickness = 2, int padding = 10) {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }
    public override void OnInspectorGUI() {
        LifecycleManager script = (LifecycleManager) target;
        script.LoadOnStart = EditorGUILayout.ToggleLeft("Load On Start", script.LoadOnStart);
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical();
        DrawUILine(Color.grey);
        Color returnColor = GUI.color;
        foreach (LifecycleModule lfo in script.lifecycleObjects) {
            EditorGUILayout.BeginVertical(); {
                EditorGUILayout.BeginHorizontal(); {
                    EditorGUILayout.LabelField(lfo.GetType().ToString());
                }
                EditorGUI.indentLevel++;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();{
                    if(lfo._initialized) GUI.color = Color.green;
                    else GUI.color = Color.red;
                    EditorGUILayout.TextField("Initialized: ", lfo._initialized.ToString());
                    GUI.color = returnColor;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(); {
                    EditorGUILayout.LabelField("Mask", GUILayout.Width(60f));
                    EditorGUILayout.TextField(lfo._dependentMask.ToString("X2"));
                    EditorGUILayout.LabelField("Final",GUILayout.Width(60f));
                    EditorGUILayout.TextField(lfo._decomMask.ToString("X2"));
                }
                EditorGUILayout.EndHorizontal();
                DrawUILine(Color.grey);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Initialize")) {
            script.InitializeDependencyTree();
        }
        if (GUILayout.Button("Decommission")) {
            script.DecommissionDependencyTree();
        }

        SetDirty();
    }

    void Update() {
        Repaint();
    }
}

[CustomPropertyDrawer(typeof(LifecycleModule))]
public class LifecycleObjectDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        float heightSegment = position.height / 3;

        var titleRect = new Rect(position.x, position.y, position.width, heightSegment);
        //
        var label1Rect = new Rect(position.x, position.y+heightSegment, position.width/2, heightSegment);
        var val1Rect = new Rect(position.x + position.width/2, position.y+heightSegment, position.width/2, heightSegment);
        //
        var label2Rect = new Rect(position.x, position.y+(2*heightSegment), position.width/4, heightSegment);
        var val2Rect = new Rect(position.x + position.width/4, (2*heightSegment), position.width/4, heightSegment);
        var label3Rect = new Rect(position.x + (2*position.width/4), position.y+(2*heightSegment), position.width/4, heightSegment);
        var val3Rect = new Rect(position.x + (3*position.width/4), (2*heightSegment), position.width/4, heightSegment);
        
        EditorGUI.LabelField(titleRect, property.serializedObject.targetObject.GetType().ToString());
        
        EditorGUI.LabelField(label1Rect, "Initialized: ");
        EditorGUI.PropertyField(val1Rect, property.FindPropertyRelative("_initialized"));
        EditorGUI.LabelField(label1Rect, "Mask: ");
        EditorGUI.PropertyField(val1Rect, property.FindPropertyRelative("_dependencyMask"));
        EditorGUI.LabelField(label1Rect, "Final: ");
        EditorGUI.PropertyField(val1Rect, property.FindPropertyRelative("_initMask"));
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label)*3;
    }
}