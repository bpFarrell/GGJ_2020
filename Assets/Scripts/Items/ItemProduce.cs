using UnityEngine;

public class ItemProduce : MonoBehaviour, IInteractable
{
    public PlantType plantType;
    public bool isCurrentlyInteractable { get; }
    public Vector3 position { get { return transform.position; } }
    public GameObject GetGameObject() { return gameObject; }

    public void Interact(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void EnterRange(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void LeaveRange(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}