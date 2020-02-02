using Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class DropBox : Station
{
    public List<GameObject> itemDisplay;
    private delegate void DeliveryComplete(DropBox box);
    private static DeliveryComplete onDeliveryComplete;

    private void OnEnable() {
        onDeliveryComplete += SomoneFinished;
    }
    private void OnDisable() {
        onDeliveryComplete -= SomoneFinished;
    }
    private class DeliveryData {
        public PlantType type;
        public GameObject go;
        public MeshRenderer mr;
        public DeliveryData(PlantType type,GameObject go) {
            this.type = type;
            this.go = go;
            this.mr = this.go.GetComponent<MeshRenderer>();
            this.mr.material = PlantMeta.GetMaterial(type);
        }
    }
    List<DeliveryData> items = new List<DeliveryData>();
    public void InitDelivery(PlantType[] deliveryTypes) {
        for (int i = itemDisplay.Count - 1;i >= 0 ; i--) {
            if (deliveryTypes.Length <= i) {
                Destroy(itemDisplay[i]);
                itemDisplay.RemoveAt(i);
                continue;
            }
            items.Add(new DeliveryData(deliveryTypes[i], itemDisplay[i]));
        }
    }
    public override void Interact(PlayerController player) {
        base.Interact(player);
        if(player.heldItem is ItemProduce produce) {
            DeliveryData target = items.FirstOrDefault((data) => { return data.type == produce.plantType; });
            if (target == null) return;
            Destroy(target.go);
            items.Remove(target);
            produce.heldBy.heldItem = null;
            Destroy(produce.gameObject);
            CheckIfDone();
        }
    }
    private void CheckIfDone() {
        if (items.Count == 0)
            onDeliveryComplete(this);
    }
    private void SomoneFinished(DropBox box) {
        Debug.Log("Moving Forward");
    }

}
