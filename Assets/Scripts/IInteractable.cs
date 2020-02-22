using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public interface IInteractable
{
    Vector3 position { get; }
    GameObject GetGameObject();
    bool isCurrentlyInteractable { get; }
    void Interact(PlayerController player);
    void EnterRange(PlayerController player);
    void LeaveRange(PlayerController player);
    void EnterPrimarySelect(PlayerController player);
    void LeavePrimarySelect(PlayerController player);

}