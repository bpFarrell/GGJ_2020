using System.Linq;
using System.Runtime.InteropServices;
using Cadence.StateMachine;
using UnityEditor;
using UnityEngine;

namespace Cadence.Editor {
    [CustomEditor(typeof(CadenceHierarchyStateMachine))]
    public class CadenceHierarchyStateMachineEditor : UnityEditor.Editor {
        private int selectionIndex = 0;
        public override void OnInspectorGUI() {
            var script = (CadenceHierarchyStateMachine) target;
            var defaultStateProp = serializedObject.FindProperty("defaultState");
            EditorGUILayout.PropertyField(defaultStateProp, new GUIContent("Default State"));
            serializedObject.ApplyModifiedProperties();

            if(script._activatorDict.Count == 0) script.GetActivatorList();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            selectionIndex = EditorGUILayout.Popup("State", selectionIndex, script._activatorDict.Keys.ToArray());
            if (GUILayout.Button("SetState")) {
                script.ActivateState(script._activatorDict.Keys.ToArray()[selectionIndex]);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            using(new EditorGUILayout.VerticalScope()) {
                using(new EditorGUI.DisabledScope()) {
                    foreach (StateBase stateBase in script._activatorDict.Values.SelectMany(stateBaseList => stateBaseList)) {
                        EditorGUILayout.ObjectField(stateBase.stateName, stateBase, typeof(StateBase), true);
                    }
                }
            }
        }
    }
}