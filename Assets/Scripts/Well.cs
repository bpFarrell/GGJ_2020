using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour,IInteractable
{
    public bool isCurrentlyInteractable => true;

    public void EnterRange(PlayerController player) {

    }

    public void Interact(PlayerController player) {
        if(player.heldItem is ItemWatteringCan can) {
            can.Fill();
            can.StartAnimation();
        }
    }

    public void LeaveRange(PlayerController player) {

    }
}
