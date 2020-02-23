using Cadence.StateMachine;
using UnityEngine.SceneManagement;

public class CadenceAdditiveSceneLoader : StateBase {
    public string state;
    public int sceneIndex;
    public override string stateName => state;
    public override void Activate() {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive).completed += (handler) => {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
            CadenceHierarchyStateMachine.Instance.GetActivatorList();
        };
    }
    public override void Deactivate() {
        if(SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded) {
            SceneManager.UnloadSceneAsync(sceneIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            CadenceHierarchyStateMachine.Instance.GetActivatorList();
        }
    }
}
