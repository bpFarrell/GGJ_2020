using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cadence.StateMachine;
using UnityEngine;

public class CadenceDelayedTransition : StateBase {
    public string state = "";
    public string targetState = "";
    public override string stateName => state;
    public override void Activate() {
        CadenceHierarchyStateMachine.Instance.ActivateState(targetState);
    }
    
    
    public override void Deactivate() {
        
    }
}
