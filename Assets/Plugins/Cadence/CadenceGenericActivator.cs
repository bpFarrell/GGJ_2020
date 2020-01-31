using Cadence.StateMachine;

public class CadenceGenericActivator : StateBase {
    public string state = "default";
    public override string stateName => state;
    public override void Activate() {
        gameObject.SetActive(true);
    }

    public override void Deactivate() {
        gameObject.SetActive(false);
    }
}
