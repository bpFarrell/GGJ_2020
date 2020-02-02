
using UnityEngine;

public class RepairBench : Station
{
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        
        Debug.Log("Repair Bench Interact!");
    }
}