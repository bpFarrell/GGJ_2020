using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Station
{
    public bool isCurrentlyInteractable => true;

    public override void Interact(PlayerController player)
    {
        if (player.heldItem is ItemWatteringCan can)
        {
            can.Fill();
            can.StartAnimation();
        }
    }
}
