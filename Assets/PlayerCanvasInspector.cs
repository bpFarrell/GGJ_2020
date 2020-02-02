using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasInspector : MonoBehaviour {
    public PlayerController playerController;
    public GameObject chargeMeterGameObject;
    public RectTransform chargeMeterElement;
    [Space]
    public GameObject waterMeterGameObject;
    public RectTransform[] waterMeterElements;
    

    private void LateUpdate() {
        if (playerController.isCharging) {
            chargeMeterGameObject.SetActive(true);
            waterMeterGameObject.SetActive(false);
            chargeMeterElement.anchorMax = new Vector2(playerController.chargeTime / PlayerController.maxPowerTime, 1.0f);
        }
        else if (playerController.heldItem != null && playerController.heldItem is ItemWatteringCan can) {
            waterMeterGameObject.SetActive(true);
            chargeMeterGameObject.SetActive(false);
            for (var index = 0; index < waterMeterElements.Length; index++) {
                if(index < can.currentWater)
                    waterMeterElements[index].gameObject.SetActive(true);
                else
                    waterMeterElements[index].gameObject.SetActive(false);
            }
        }
        else {
            waterMeterGameObject.SetActive(false);
            chargeMeterGameObject.SetActive(false);
        }
    }
}
