using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace Cadence.StateMachine {
    using Utility;

    public class CadenceHierarchyStateMachine :  UISingletonBehaviour<CadenceHierarchyStateMachine> {
        public string defaultState = "default";
        public string currentState { get; private set; }
        
        [SerializeField] private Dictionary<string, List<StateBase>> activatorDict = new Dictionary<string, List<StateBase>>();
        public Dictionary<string, List<StateBase>> _activatorDict => activatorDict;

        private void OnEnable() {
            GetActivatorList();
            ActivateDefault();
        }

        public void ActivateDefault() {
            ActivateState(defaultState);
        }
        
        public void GetActivatorList() {
            activatorDict.Clear();
            List<StateBase> results = new List<StateBase>();
            for(int i = 0; i< SceneManager.sceneCount; i++)
            {
                 Scene s = SceneManager.GetSceneAt(i);
                 var allGameObjects = s.GetRootGameObjects();
                 foreach (GameObject go in allGameObjects) {
                     results.AddRange(go.GetComponentsInChildren<StateBase>(true));
                 }
            }
            foreach (StateBase activator in results) {
                if (activatorDict.ContainsKey(activator.stateName.ToLower())) {
                    activatorDict[activator.stateName.ToLower()].Add(activator);
                } else {
                    activatorDict.Add(activator.stateName.ToLower(), new List<StateBase> {activator});
                }
            }
        }
        
        public void ToggleState(string state) {
            if (string.Equals(state, defaultState, StringComparison.CurrentCultureIgnoreCase)) {                
                Debug.LogError($"No state found of name: {state.ToLower()}");
                return;
            }
            if(string.Equals(currentState, state, StringComparison.CurrentCultureIgnoreCase)) {
                ActivateDefault();
            } else {
                ActivateState(state);
            }
        }

        public void ActivateState(string state) {
            if (string.IsNullOrWhiteSpace(state)) {
                DeactivateAll();
                return;
            }

            if (!activatorDict.ContainsKey(state.ToLower())) {
                Debug.LogError($"No state found of name: {state.ToLower()}");
                return;
            }

            string majorState = "";
            var stateSplit = state.Split('/');
            if (stateSplit.Length > 1) {
                majorState = stateSplit[0];
            }

            Debug.Log($"Activating state: {state.ToLower()} | PreviousState: {currentState}");

            foreach (var stateBasekvp in activatorDict) {
                if (string.Equals(state, stateBasekvp.Key, StringComparison.CurrentCultureIgnoreCase) ||
                    (!string.IsNullOrEmpty(majorState) &&
                     stateBasekvp.Key.Equals(majorState, StringComparison.CurrentCultureIgnoreCase))) {
                    continue;
                }

                foreach (StateBase stateBase in stateBasekvp.Value) {
                    stateBase._Deactivate();
                }
            }

            currentState = state.ToLower();
            foreach (var stateBasekvp in activatorDict) {
                if (string.Equals(state, stateBasekvp.Key, StringComparison.CurrentCultureIgnoreCase) ||
                    (!string.IsNullOrEmpty(majorState) &&
                     stateBasekvp.Key.Equals(majorState, StringComparison.CurrentCultureIgnoreCase))) {
                    {
                        foreach (StateBase stateBase in stateBasekvp.Value) {
                            stateBase._Activate();
                        }
                    }
                }
            }
        }

        private void DeactivateAll() {
            foreach (StateBase stateBase in activatorDict.Values.SelectMany(stateBaseList => stateBaseList).Where(stateBase => stateBase.isActive)) {
                stateBase._Deactivate();
            }
        }
    }
}