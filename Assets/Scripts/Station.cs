using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Station : MonoBehaviour, IInteractable
{
    public bool isCurrentlyInteractable { get; }

    public Vector3 position { get { return transform.position; } }
    public GameObject GetGameObject() { return gameObject; }

    void OnEnable()
    {
        transform.DOPunchScale(-transform.localScale, 0.5f);
    }
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
