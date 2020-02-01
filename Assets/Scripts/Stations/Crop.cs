using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crop : Station
{
    private const int PRODUCE_SPAWN_RADIUS = 1;
    private Dictionary<PlantType, GameObject> produceLookup;
    public CropType cropType { get; private set; }
    public PlantType plantType { get; private set; }

    private void Start()
    {
        produceLookup = new Dictionary<PlantType, GameObject>
        {
            {PlantType.Red, Resources.Load<GameObject>("crops/red")},
            {PlantType.Green, Resources.Load<GameObject>("crops/green")},
            {PlantType.Yellow, Resources.Load<GameObject>("crops/yellow")}
        };
    }

    public override void Interact(PlayerController player)
    {
        base.Interact(player);

        var item = player.heldItem;

        var canTill = cropType == CropType.Plain;
        var canSeed = cropType == CropType.Tilled;
        
        var canWater = CanWater(cropType);
        var canHarvest = CanHarvest(cropType);

        if (canTill && item is ItemHoe)
        {
            CropTransition(cropType, CropType.Tilled);
        } 
        else if (canSeed && item is ItemSeedBag bag)
        {
            plantType = bag.plantType;
            player.UnassignHand();
        }
        else if (canWater && item is ItemWatteringCan can && !can.isEmpty)
        {
            can.Use();
            player.UnassignHand();
        } 
        else if (canHarvest && item is ItemScyth scyth)
        {
            var produceCount = GetProduceCount(cropType);
            
            ProduceProduce(cropType, plantType, produceCount);
            CropTransition(cropType, CropType.Plain);
        }
    }

    private void ProduceProduce(CropType cropType, PlantType plantType, int numProduce = 1)
    {
        var i = 0;

        for (; i < numProduce; i++)
        {
            var unit = Random.onUnitSphere * PRODUCE_SPAWN_RADIUS;
            var produceLocation = new Vector3(unit.x, 0, unit.z);
            
            Instantiate(produceLookup[plantType], produceLocation, Quaternion.identity);
        }
    }

    bool CanWater(CropType cropType) =>
        cropType == CropType.Sprout ||
        cropType == CropType.Juvenile ||
        cropType == CropType.Mature
    ;
    
    bool CanHarvest(CropType cropType) =>
        cropType == CropType.Sprout ||
        cropType == CropType.Juvenile ||
        cropType == CropType.Mature ||
        cropType == CropType.Overgrown
    ;

    private int GetProduceCount(CropType cropType)
    {
        var numProduce = 0;

        switch (cropType)
        {
            case CropType.Sprout:
                numProduce = 1;
                break;
            case CropType.Juvenile:
                numProduce = 2;
                break;
            case CropType.Mature:
                numProduce = 3;
                break;
            case CropType.Overgrown:
                numProduce = 4;
                break;
        }

        return numProduce;
    }
    
    void CropTransition(CropType from, CropType to)
    {
        cropType = to;
    } 
}