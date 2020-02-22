using System;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : ItemBase
{
    //"global" settings for interactions
    public static float launchAngleDegs = 40;
    public static float maxLaunchSpeed = 10f;
    public static float minLaunchSpeed = 4f;
    public static float maxPowerTime = .8f;
    public static float smallDropTime = .2f; //before this, drop straight down. else throw with minlaunchspeed and up

    Animator animator;

    public GameObject handPivot;
    Player player;
    public ItemBase heldItem;
    //public float speed = 2;
    public float chargeTime = 0;
    bool justPickedUp = false;
    public bool isCharging; 

    List<IInteractable> nears;
    IInteractable nearInteraction;

    static int claimedPlayers = 0;


    void Awake()
    {
        base.Awake();

        Debug.Log("player awake "+name);
        player = ReInput.players.GetPlayer(claimedPlayers);
        claimedPlayers++;

        nears = new List<IInteractable>();

        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", true);

    }

    bool MaybeDoingSomething()
    {
        return heldItem != null
            && nearInteraction != null 
            && heldItem.gameObject != nearInteraction.GetGameObject()
            && (nearInteraction as Station) != null
            ;
    }

    // Update is called once per frame
    void Update()
    {
        if (heldBy != null)
        {
            if (player.GetButtonDown("Action"))
            {
                //get unheld
                heldBy.ThrowHeld(smallDropTime+.01f);
            }
            return;
        }

        if (player.GetButtonDown("Action"))
        {
            Debug.Log("held: " + (heldItem == null ? "null" : heldItem.name)
                + "  nearInteraction: " + (nearInteraction == null ? "null" : nearInteraction.GetGameObject().name));

            if (heldItem != null)
            {
                chargeTime = 0;
                isCharging = true;
            }
            else if (nearInteraction != null)
            {
                Debug.Log("near name: " + (nearInteraction as MonoBehaviour).name);
                Debug.Log(name + " Picking up");
                nearInteraction.Interact(this); //this calls AssignToHand()
                justPickedUp = true;
            }

            if (MaybeDoingSomething())
            {
                UseToolOnThing();
                isCharging = false;
            }

            Debug.Log("You Actioned!");
        }
        else if (player.GetButtonUp("Action"))
        {
            if (!MaybeDoingSomething())
            {
                if (!justPickedUp && heldItem != null)
                {
                    ThrowHeld(chargeTime);
                }
            }
            justPickedUp = false;
            isCharging = false;
        }
        else if (player.GetButton("Action"))
        {
            if (heldItem != null && nearInteraction != null && heldItem.gameObject != nearInteraction.GetGameObject())
            {
                //UseToolOnThing();
            }
            else if (!justPickedUp && heldItem != null)
            {
                //charge up for a throw
                //Debug.Log(name + " charge: " + throwSpeed);
                chargeTime += Time.deltaTime;
                if (chargeTime > maxPowerTime) chargeTime = maxPowerTime;
            }
        }

        //handle movement
        Vector2 dir2D = player.GetAxis2D("MoveX", "MoveY");
        Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);
        float moveSpeed = dir.magnitude;
        if (player.GetButton("Action") && heldItem != null) moveSpeed = 0;
        //Debug.Log("stick mag: " + moveSpeed);

        //make sure we are upright-ish
        if (heldBy == null && transform.up.y < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                transform.rotation * Quaternion.FromToRotation(transform.up, Vector3.up), .05f);
        }

        if (moveSpeed == 0)
        {
            animator.SetFloat("MoveSpeed", 0);
            //return;
            if (dir.magnitude == 0) return;
        }

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * Time.deltaTime * faceScale;
        Vector3 newPos = transform.position + targetDir * PlayAreaSettings.playerSpeed * moveSpeed;
        if (transform.position.x <= -PlayAreaSettings.middleHalfWidth && newPos.x > -PlayAreaSettings.middleHalfWidth)
            newPos.x = -PlayAreaSettings.middleHalfWidth;
        if (transform.position.x >= PlayAreaSettings.middleHalfWidth && newPos.x < PlayAreaSettings.middleHalfWidth)
            newPos.x = PlayAreaSettings.middleHalfWidth;
        //Debug.Log("x: " + transform.position.x + "  new x: " + newPos.x + "  barrier: " + PlayAreaSettings.middleHalfWidth);

        //transform.position += targetDir * PlayAreaSettings.playerSpeed * moveSpeed;
        transform.position = newPos;

        //Debug.Log("movespeed: "+(targetDir.magnitude * speed));
        animator.SetFloat("MoveSpeed", moveSpeed);

        ClampToBounds();


    }

    /// <summary>
    /// Assuming heldItem is the tool and nearInteraction is the thing...
    /// </summary>
    void UseToolOnThing()
    {
        Debug.Log("Doing something with "+heldItem.name+" on "+nearInteraction.GetGameObject().name);
        nearInteraction.Interact(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact == null) return;
        Debug.Log(name + " near interaction enter: " + other.name);
        nearInteraction = interact;

        nears.Add(interact);
        UpdateNearest();
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact == null) return;
        Debug.Log(name + " near interaction leave: " + other.name);
        nearInteraction = null;
        interact.LeaveRange(this);

        nears.Remove(interact);
        UpdateNearest();
    }

    void UpdateNearest()
    {
        int which_station = -1;
        int which_item = -1;
        float d_station = 1000;
        float dd_station;
        float d_item = 1000;
        float dd_item;
        IInteractable nearest = null;

        for (int i = 0; i<nears.Count; i++)
        {
            if (nears[i] == null) continue;
            if (heldItem == nears[i] as ItemBase && !(nears[i] is Station)) continue; //skip the item we are holding

            if (nears[i] as Station != null)
            {
                dd_station = (nears[i].position - transform.position).magnitude;
                if (dd_station < d_station)
                {
                    which_station = i;
                    d_station = dd_station;
                }
            }
            if (nears[i] as ItemBase != null)
            {
                dd_item = (nears[i].position - transform.position).magnitude;
                if (dd_item < d_item)
                {
                    which_item = i;
                    d_item = dd_item;
                }
            }
        }
        if (heldItem != null) //we are holding a tool, prioritize stations
        {
            if (which_station >= 0) nearest = nears[which_station];
            else if (which_item != -1) nearest = nears[which_item];
        }
        else //we are not holding anything, prioritize items
        {
            if (which_item != -1) nearest = nears[which_item];
            else if (which_station >= 0) nearest = nears[which_station];
        }

        if (nearInteraction != nearest)
        {
            if (nearInteraction != null) StopHighlightingNear(nearInteraction);
            nearInteraction = nearest;
            if (nearInteraction != null) StartHighlightingNear(nearInteraction);
        }
        Debug.Log("new near: " + (nearInteraction == null ? "null" :(nearInteraction as MonoBehaviour).name));
    }

    private void StopHighlightingNear(IInteractable thing)
    {
        // ...
    }

    private void StartHighlightingNear(IInteractable thing)
    {
        // ...
    }

    public void AssignToHand(ItemBase item)
    {
        heldItem = item;
        item.transform.parent = handPivot.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

    public void ItemTaken(ItemBase item) //ItemBase calls this
    {
        if (item == heldItem)
        {
            heldItem = null;
        }
    }

    public void ThrowHeld(float timeCharged)
    {
        heldItem.transform.DOComplete();
        isCharging = false;
        Debug.Log(name + " THROW!!");
        //heldItem.Dropped(this);
        heldItem.transform.parent = null;
        float lspeed = Mathf.Clamp(minLaunchSpeed + (maxLaunchSpeed - minLaunchSpeed) * timeCharged / maxPowerTime,
                                    minLaunchSpeed, maxLaunchSpeed);
        if (timeCharged < smallDropTime) lspeed = minLaunchSpeed / 4;
        heldItem.Thrown(Quaternion.AngleAxis(-launchAngleDegs, transform.right) * transform.forward,
            lspeed);
        heldItem = null;
    }

    public void DropItems(Vector3 throwV3)
    {
        var lastItem = heldItem;

        lastItem.transform.parent = null;

        var rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.velocity = throwV3;
        }

        UnassignHand();
    }

    public void DestroyHeldItem() {
        nears.Remove(heldItem);
        Destroy(heldItem);
        UnassignHand();
    }

    private void OnDestroy() {
        claimedPlayers = 0;
    }

    public void UnassignHand()
    {
        heldItem = null;
    }
}

