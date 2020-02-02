using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using DG.Tweening;

public class ItemBase : MonoBehaviour ,IInteractable
{
    private const float ANIMATION_DURATION = 0.5f;
    private float nextAnimationAllowed = 0;
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
    public virtual void StartAnimation(ItemAnimationType animType = ItemAnimationType.Success) {
        if (Time.time < nextAnimationAllowed)
        {
            return;
        }
        
        switch (animType)
        {
            case ItemAnimationType.Success:
                transform.DOPunchPosition(-transform.forward*0.5F, ANIMATION_DURATION);
                transform.DOPunchRotation(new Vector3(30, 0, 0), ANIMATION_DURATION);
                break;
            case ItemAnimationType.Failure:
                transform.DOShakePosition(ANIMATION_DURATION, 0.5f);
                transform.DOShakeRotation(ANIMATION_DURATION, 0.5f);
                break;
            default:
                Debug.Log($"unimplemented animation type {animType}");
                break;
        }

        nextAnimationAllowed = Time.time + ANIMATION_DURATION;
    }
    public void EnterRange(PlayerController player) {
        Debug.Log("Enter");
    }

    public void LeaveRange(PlayerController player) {
        Debug.Log("Leave");
    }
}
