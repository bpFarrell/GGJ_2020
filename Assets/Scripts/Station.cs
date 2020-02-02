using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Station : MonoBehaviour, IInteractable
{
    public bool isCurrentlyInteractable { get; }
    
    virtual public void Interact(PlayerController player)
    {
        
    }

    virtual public void EnterRange(PlayerController player)
    {
        
    }

    virtual public void LeaveRange(PlayerController player)
    {
        
    }
}
