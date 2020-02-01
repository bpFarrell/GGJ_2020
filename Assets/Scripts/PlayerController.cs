using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject handPivot;
    Player player;
    IInteractable nearInteraction;
    ItemBase heldItem;
    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Action")) {
            if (heldItem != null) {
                heldItem.Dropped(this);
                heldItem.transform.parent = null;
                heldItem = null;
            }else if (nearInteraction != null) {
                nearInteraction.Interact(this);
            }

            Debug.Log("You Actioned!");
        }
        Vector2 dir2D = player.GetAxis2D("MoveX", "MoveY");
        Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);

        if (dir.magnitude == 0)return;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * Time.deltaTime * faceScale;
        transform.position += targetDir;
    }
    private void OnTriggerEnter(Collider other) {
        IInteractable interact = other.GetComponent<IInteractable>();
        nearInteraction = interact;
        interact.EnterRange(this);

    }
    private void OnTriggerExit(Collider other) {
        IInteractable interact = other.GetComponent<IInteractable>();
        nearInteraction = null;
        interact.LeaveRange(this);
    }
    public void AssignToHand(ItemBase item) {
        heldItem = item;
        item.transform.parent = handPivot.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }
}
