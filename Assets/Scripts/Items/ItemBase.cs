using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using DG.Tweening;

public class ItemBase : MonoBehaviour, IInteractable
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
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }
    public virtual void Dropped(PlayerController player) {
        heldBy = null;
    }

    public virtual void Thrown(Vector3 direction, float power)
    {
        Debug.Log("Thrown!");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = direction.normalized * power;
        }
        Collider bc = GetComponent<Collider>();
        if (bc != null) bc.isTrigger = false;
    }

    public virtual void StartAnimation() {
        transform.DOPunchPosition(-transform.forward*0.5F, 0.5f);
        transform.DOPunchRotation(new Vector3(30, 0, 0), 0.5F);
    }
    public void EnterRange(PlayerController player) {
        Debug.Log("Enter");
    }

    public void LeaveRange(PlayerController player) {
        Debug.Log("Leave");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " CollisionEnter " + collision.gameObject.name);
        if (!isHeld)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
            }
        }
    }
}
