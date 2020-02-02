using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ItemBase
{
    //"global" settings for interactions
    public static float launchAngleDegs = 30;
    public static float launchSpeed = .05f; //units per sec
    public static float maxThrowSpeed = 30;

    public GameObject handPivot;
    Player player;
    IInteractable nearInteraction;
    public ItemBase heldItem;
    public float speed = 2;
    public float throwSpeed = 0;
    bool justPickedUp = false;

    List<IInteractable> nears;

    static int claimedPlayers = 0;



    void Awake()
    {
        player = ReInput.players.GetPlayer(claimedPlayers);
        claimedPlayers++;

        nears = new List<IInteractable>();
    }


    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Action"))
        {
            if (heldItem != null)
            {
                throwSpeed = 0;
            }
            else if (nearInteraction != null)
            {
                Debug.Log(name + " Picking up");
                nearInteraction.Interact(this); //this calls AssignToHand()
                justPickedUp = true;
            }

            if (heldItem != null && nearInteraction != null)
            {
                Debug.Log("Near something with tool: do something!!");
            }

            Debug.Log("You Actioned!");
        }
        else if (player.GetButtonUp("Action"))
        {
            if (!justPickedUp && heldItem != null)
            {
                Debug.Log(name + " THROW!!");
                heldItem.Dropped(this);
                heldItem.transform.parent = null;
                heldItem.Thrown(Quaternion.AngleAxis(-launchAngleDegs, transform.right) * transform.forward,
                    launchSpeed * throwSpeed);
                heldItem = null;
            }
            justPickedUp = false;
        }
        else if (player.GetButton("Action"))
        {
            if (!justPickedUp && heldItem != null)
            {
                //Debug.Log(name + " charge: " + throwSpeed);
                throwSpeed++;
                if (throwSpeed > maxThrowSpeed) maxThrowSpeed = throwSpeed;
            }
        }

        //handle movement
        Vector2 dir2D = player.GetAxis2D("MoveX", "MoveY");
        Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);

        if (dir.magnitude == 0) return;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * Time.deltaTime * faceScale;
        transform.position += targetDir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact == null) return;
        Debug.Log(name + " trigger enter: " + other.name);
        nearInteraction = interact;

        nears.Add(interact);
        Debug.LogWarning("IMPLEMENT UPDATE NEAR!!");
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact == null) return;
        Debug.Log(name + " trigger leave: " + other.name);
        nearInteraction = null;
        interact.LeaveRange(this);

        nears.Remove(interact);
        Debug.LogWarning("IMPLEMENT UPDATE NEAR!!");
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

