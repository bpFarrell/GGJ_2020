using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ItemBase
{
    //"global" settings for interactions
    public static float launchAngleDegs = 40;
    public static float launchSpeed = 30f; //units per sec
    public static float minLaunchSpeed = 10f;
    public static float maxPowerTime = 2;
    public static float maxPower = 30;

    Animator animator;

    public GameObject handPivot;
    Player player;
    public ItemBase heldItem;
    public float speed = 2;
    public float chargeTime = 0;
    bool justPickedUp = false;

    List<IInteractable> nears;
    IInteractable nearInteraction;

    static int claimedPlayers = 0;



    void Awake()
    {
        Debug.Log("player awake "+name);
        player = ReInput.players.GetPlayer(claimedPlayers);
        claimedPlayers++;

        nears = new List<IInteractable>();

        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", true);
    }


    // Update is called once per frame
    void Update()
    {
        if (heldBy != null) return;

        if (player.GetButtonDown("Action"))
        {
            if (heldItem != null)
            {
                chargeTime = 0;
            }
            else if (nearInteraction != null)
            {
                Debug.Log(name + " Picking up");
                nearInteraction.Interact(this); //this calls AssignToHand()
                justPickedUp = true;
            }

            if (heldItem != null && nearInteraction != null && heldItem.gameObject != nearInteraction.GetGameObject())
            {
                UseToolOnThing();
            }

            Debug.Log("You Actioned!");
        }
        else if (player.GetButtonUp("Action"))
        {
            if (!justPickedUp && heldItem != null)
            {
                Debug.Log(name + " THROW!!");
                //heldItem.Dropped(this);
                heldItem.transform.parent = null;
                float lspeed = launchSpeed;
                if (lspeed < minLaunchSpeed) lspeed = minLaunchSpeed;
                heldItem.Thrown(Quaternion.AngleAxis(-launchAngleDegs, transform.right) * transform.forward,
                    lspeed * chargeTime / maxPowerTime);
                heldItem = null;
            }
            justPickedUp = false;
        }
        else if (player.GetButton("Action"))
        {
            if (heldItem != null && nearInteraction != null && heldItem.gameObject != nearInteraction.GetGameObject())
            {
                UseToolOnThing();
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

        //Debug.Log("stick mag: " + moveSpeed);
        if (moveSpeed == 0)
        {
            animator.SetFloat("MoveSpeed", 0);
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * Time.deltaTime * faceScale;
        transform.position += targetDir * speed * moveSpeed;

        //Debug.Log("movespeed: "+(targetDir.magnitude * speed));
        animator.SetFloat("MoveSpeed", moveSpeed);

        ClampToBounds();
    }

    /// <summary>
    /// Assuming heldBy is the tool and nearInteraction is the thing...
    /// </summary>
    void UseToolOnThing()
    {
        Debug.Log("Doing something with "+heldItem.name+" on "+nearInteraction.GetGameObject().name);
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
        int which = -1;
        float d = 1000;
        float dd;
        for (int i = 0; i<nears.Count; i++)
        {
            dd = (nears[i].position - transform.position).magnitude;
            if (dd < d)
            {
                which = i;
                d = dd;
                nearInteraction = nears[i];
            }
        }
        if (which == -1) nearInteraction = null;
    }

    public void AssignToHand(ItemBase item)
    {
        heldItem = item;
        item.transform.parent = handPivot.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
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

    public void UnassignHand()
    {
        heldItem = null;
    }
}

