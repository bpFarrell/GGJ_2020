using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cadence.StateMachine;
using DG.Tweening;
using UnityEngine;

public class CadenceDelayedTransition : StateBase {
    public string state = "";
    public string targetState = "";
    public override string stateName => state;
    public override void Activate() {
        transform.DOLocalMove(transform.localPosition, 1.5f).onComplete = () => {
            CadenceHierarchyStateMachine.Instance.ActivateState(targetState);
        };
    }
    
    public override void Deactivate() {
        
    }
}
