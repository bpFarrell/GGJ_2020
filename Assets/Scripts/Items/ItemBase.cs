using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class ItemBase : MonoBehaviour ,IInteractable
{
    public bool isHeld { get { return heldBy != null; } }
    public PlayerController heldBy;
    public virtual bool isCurrentlyInteractable => true;
    public virtual void Interact(PlayerController player) {
        player.AssignToHand(this);
        PickedUp(player);
    }
    public virtual void PickedUp(PlayerController player) {
        heldBy = player;
    }
    public virtual void Dropped(PlayerController player) {
        heldBy = null;
    }

    public void EnterRange(PlayerController player) {
        Debug.Log("Enter");
    }

    public void LeaveRange(PlayerController player) {
        Debug.Log("Leave");
    }
}
