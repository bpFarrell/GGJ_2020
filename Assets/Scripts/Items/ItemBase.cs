using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using DG.Tweening;


public class ItemBase : MonoBehaviour, IInteractable
{
    private const float ANIMATION_DURATION = 0.5f;
    private float nextAnimationAllowed = 0;
    public bool isHeld { get { return heldBy != null; } }
    public bool isFlyingStart = false;
    public PlayerController heldBy;
    public GameObject lastHeld;
    public virtual bool isCurrentlyInteractable => true;

    public Vector3 position { get { return transform.position; } }
    //public GameObject gameObject { get { return base.gameObject; } }
    public GameObject GetGameObject() { return gameObject; }

    Rigidbody rb;
    Collider col;
    float defaultY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Debug.Log("Awake ItemBase "+name);
        defaultY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.Log("NULL RB!!!");
            rb = GetComponent<Rigidbody>();
        }

        if (col == null)
        {
            Debug.Log("NULL COLLIDER!!");
            col = GetComponent<Collider>();
        }

        if (isFlyingStart) {
            if (rb.velocity.y < 0)
            {
                //rb.isKinematic = false;
                if (col != null) col.isTrigger = false;
                //EndThrow();
            }
        }
        if (!isHeld && rb.isKinematic == false) //it is flying around randomly
        {
            //MeshRenderer mr = GetComponent<MeshRenderer>();
            //if (rb != null && rb.velocity.y < 0 && col != null && col.bounds.center.y - col.bounds.size.y < 0)
            if (rb != null && rb.velocity.y < 0 && transform.position.y < defaultY)
            //if (transform.position.y < 0)
            {
                EndThrow();
            }

            ClampToBounds();
        }
    }

    public void ClampToBounds()
    {
        if (rb == null) return;
        //if (rb == null || rb.isKinematic == true) return;

        //should be flying, check for out of bounds
        if (transform.position.x < -PlayAreaSettings.xHalfWidth+1)
        {
            transform.position = new Vector3(-PlayAreaSettings.xHalfWidth+1, transform.position.y, transform.position.z);
            //rb.isKinematic = true;
            rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, rb.velocity.z);
            //rb.isKinematic = false;
        }
        else if (transform.position.x > PlayAreaSettings.xHalfWidth)
        {
            transform.position = new Vector3(PlayAreaSettings.xHalfWidth, transform.position.y, transform.position.z);
            //rb.isKinematic = true;
            rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, rb.velocity.z);
            //rb.isKinematic = false;
        }

        if (transform.position.z < -PlayAreaSettings.zHalfWidth)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -PlayAreaSettings.zHalfWidth);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -rb.velocity.z);
        }
        else if (transform.position.z > PlayAreaSettings.zHalfWidth)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, PlayAreaSettings.zHalfWidth);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -rb.velocity.z);
        }
    }

    public virtual void Interact(PlayerController player) {
        player.AssignToHand(this);
        PickedUp(player);
    }

    public virtual void PickedUp(PlayerController player) {
        heldBy = player;
        lastHeld = player.gameObject;
        if (rb != null) rb.isKinematic = true;
        isFlyingStart = false;
    }

    public virtual void Dropped(PlayerController player) {
        heldBy = null;
    }

    public virtual void Thrown(Vector3 direction, float power)
    {
        Debug.Log("Thrown! "+power);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = direction.normalized * power;
        }
        //if (col != null) col.isTrigger = true;
        lastHeld = heldBy.gameObject;
        heldBy = null;
        isFlyingStart = true;
    }

    //make sure we are upright
    public virtual void EndThrow()
    {
        isFlyingStart = false;
        rb.isKinematic = true;
        col.isTrigger = true;
        //Debug.Log("End throw position1: " + (col.bounds.center.y - col.bounds.size.y));
        //transform.position -= (col.bounds.center.y - col.bounds.size.y) * Vector3.up;
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
        //Debug.Log("End throw position2: " + (col.bounds.center.y - col.bounds.size.y));
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " CollisionEnter " + collision.gameObject.name);
        if (!isHeld)
        {
            Debug.Log("Break fall for " + name);
            isFlyingStart = false;
            if (collision.gameObject == lastHeld) return;
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
            }
            collision.collider.isTrigger = true;
        }
    }
}
