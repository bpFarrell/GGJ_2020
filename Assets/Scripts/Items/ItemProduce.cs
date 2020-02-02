using UnityEngine;

namespace Items
{
    public class ItemProduce : MonoBehaviour, IInteractable
    {
        public PlantType plantType;
        public bool isCurrentlyInteractable { get; }
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
}