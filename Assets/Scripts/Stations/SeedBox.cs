using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SeedBox : Station {
    public PlantType plantType;

    private ItemSeedBag resource;


    private void Start() {
        resource = Resources.Load<ItemSeedBag>("Seeds");
    }

    public override void Interact(PlayerController player) {
        if(resource == null) Debug.LogError("Player Interaction with SeedBox Station and 'Seeds' asset was not found in Resources");
        Debug.Log("Temporary Log, please delete: SeedBox Interaction Called");

        if (player.heldItem == null) {
            ItemSeedBag bag = SpawnSeed();
            player.AssignToHand(bag);
            bag.PickedUp(player);
        }
    }

    public ItemSeedBag SpawnSeed() {
        var result = Instantiate(resource, transform.position + (Vector3.back), Quaternion.identity);
        result.plantType = this.plantType;
        return result;
    }
}
