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

    public Material cropMaterial;

    private void Start()
    {
        cropType = CropType.Plain;
        cropMaterial = GetComponentInChildren<MeshRenderer>().material;
        
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
            CropTransition(cropType, CropType.Sowed);
            player.UnassignHand();
        }
        else if (canWater && item is ItemWatteringCan can && !can.isEmpty)
        {
            CropTransition(cropType, cropType + 1);
            can.Use();
        } 
        else if (canHarvest && item is ItemScyth)
        {
            var produceCount = GetProduceCount(cropType);
            
            ProduceProduce(plantType, produceCount);
            CropTransition(cropType, CropType.Plain);
        }
    }

    private void ProduceProduce(PlantType plantType, int numProduce = 1)
    {
        var i = 0;

        for (; i < numProduce; i++)
        {
            var unit = transform.position + (Random.onUnitSphere * PRODUCE_SPAWN_RADIUS);
            var produceLocation = new Vector3(unit.x, 0, unit.z);
            
            Instantiate(produceLookup[plantType], produceLocation, Quaternion.identity);
        }
    }

    bool CanWater(CropType cropType) =>
        cropType == CropType.Sowed || 
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
                numProduce = 0;
                break;
            case CropType.Juvenile:
                numProduce = 1;
                break;
            case CropType.Mature:
                numProduce = 2;
                break;
            case CropType.Overgrown:
                numProduce = 1;
                break;
        }

        return numProduce;
    }

    private Color GetCropColor(CropType cropType)
    {
        var color = Color.red;

        switch (cropType)
        {
            case CropType.Plain: 
                color = Color.green;
                break;
            case CropType.Tilled:
                color = Color.yellow;
                break;
            case CropType.Sowed:
                color = Color.gray;
                break;
            case CropType.Sprout:
                color = Color.blue;
                break;
        }

        return color;
    }
    
    void CropTransition(CropType from, CropType to)
    {
        cropMaterial.color = GetCropColor(to);
        cropType = to;
    } 
}