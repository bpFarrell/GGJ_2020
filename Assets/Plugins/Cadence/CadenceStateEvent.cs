using System;
using UnityEngine;

namespace Cadence.StateMachine {
    public delegate void StateChangeDelegate(StateEvent e);
    public class StateEvent {
        public readonly StateBase.TransitionType type;

        public StateEvent(StateBase.TransitionType type) {
            this.type = type;
        }
    }

    public abstract class StateBase : MonoBehaviour {
        public enum TransitionType {
            ACTIVATE,
            DEACTIVATE
        } 
        
        public abstract string stateName { get; }
        public bool isActive { get; private set; }
        public event StateChangeDelegate PreStateChange;
        public event StateChangeDelegate PostStateChange;

        public void _Activate() {
            isActive = true;
            StateEvent stateEvent = new StateEvent(TransitionType.ACTIVATE);
            PreStateChange?.Invoke(stateEvent);
            Activate();
            PostStateChange?.Invoke(stateEvent);
        }
        public abstract void Activate();
        public void _Deactivate() {
            isActive = false;
            StateEvent stateEvent = new StateEvent(TransitionType.DEACTIVATE);
            PreStateChange?.Invoke(stateEvent);
            Deactivate();
            PostStateChange?.Invoke(stateEvent);
        }
        public abstract void Deactivate();
    }
}