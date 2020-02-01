using UnityEngine;

namespace Stations
{
    public class Tree : Station
    {
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            Debug.Log("Tree Interaction!");
        }
    }
}