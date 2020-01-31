using Cadence.StateMachine;

public class CadenceTextState : StateBase {
    public string state;
    public override string stateName => state;

    public TMPro.TMP_Text stateText;
    public string text;
    public override void Activate() {
        stateText.text = text;
    }

    public override void Deactivate() { }
}
