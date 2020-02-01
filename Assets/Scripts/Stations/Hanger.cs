using UnityEngine;
public class Hanger : Station
{
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        
        Debug.Log("Hanger Interact!");
    }
}