using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crop : Station
{
    private const int PRODUCE_SPAWN_RADIUS = 1;
    
    private Dictionary<PlantType, GameObject> produceLookup;
    private Dictionary<PlantType, GameObject[]> modelLookup;
    
    public CropType cropType;
    public PlantType plantType;

    private Material cropMaterial;
    private Material wetMaterial;
    private float lastWatered = float.MaxValue;
    private CropType nextCropType; // used when water is done

    public bool isWatered = false;
    public float wateringWaitTime = 5;

    public MeshFilter meshFilter = null;

    public Mesh tilledMesh = null;
    public Mesh plainMesh = null;

    public int rockHealth = 5;
    public GameObject rockPrefab = null;

    private GameObject plantModel = null;
    private GameObject rockInstance;

    private void Start()
    {
        meshFilter.mesh = plainMesh;

        if (cropType == CropType.Rocky && rockPrefab)
        {
            rockInstance = Instantiate(rockPrefab, position, Quaternion.identity);
        }
        else
        {
            cropType = CropType.Plain;
        }
        
        cropMaterial = new Material(GetComponentInChildren<MeshRenderer>().material);

        produceLookup = new Dictionary<PlantType, GameObject>
        {
            {PlantType.Red, Resources.Load<GameObject>("crops/red")},
            {PlantType.Green, Resources.Load<GameObject>("crops/green")},
            {PlantType.Yellow, Resources.Load<GameObject>("crops/yellow")}
        };
        
        modelLookup = new Dictionary<PlantType, GameObject[]> {
            {PlantType.Red, new [] {Resources.Load<GameObject>("crops/RedPlant_1"), Resources.Load<GameObject>("crops/RedPlant_2"), Resources.Load<GameObject>("crops/RedPlant_3")}},
            {PlantType.Green, new [] {Resources.Load<GameObject>("crops/GreenPlant_1"), Resources.Load<GameObject>("crops/GreenPlant_2"), Resources.Load<GameObject>("crops/GreenPlant_3")}},
            {PlantType.Yellow, new [] {Resources.Load<GameObject>("crops/YellowPlant_1"), Resources.Load<GameObject>("crops/YellowPlant_2"), Resources.Load<GameObject>("crops/YellowPlant_3")}}
        };

        wetMaterial = Resources.Load<Material>("Unlit_WetDirt");
    }

    private void Update()
    {
        if (isWatered && lastWatered + wateringWaitTime < Time.time)
        {
            isWatered = false;
            
            CropTransition(cropType, nextCropType);
            ChangeMaterial(cropMaterial);
        }
    }

    public override void Interact(PlayerController player)
    {
        base.Interact(player);

        var item = player.heldItem;

        var canTill = cropType == CropType.Plain;
        var canSeed = cropType == CropType.Tilled;
        var canHammer = cropType == CropType.Rocky;
        var canWater = CanWater(cropType);
        var canHarvest = CanHarvest(cropType);

        var animateItem = true;
        
        if (canHammer)
        {
            rockHealth--;

            if (rockHealth <= 0)
            {
                Destroy(rockInstance);
                CropTransition(cropType, CropType.Plain);
            }
            else
            {
                rockInstance.transform.localScale *= .9f;
                rockInstance.transform.DOShakePosition(0.5f, new Vector3(0.1f, 0.25f, 0.1f));
            }
        }
        else if (canTill && item is ItemHoe)
        {
            CropTransition(cropType, CropType.Tilled);
        } 
        else if (canSeed && item is ItemSeedBag bag)
        {
            plantType = bag.plantType;
            Destroy(bag.gameObject);
            CropTransition(cropType, CropType.Sowed);
            player.DestroyHeldItem();
        }
        else if (canWater && item is ItemWatteringCan can && !can.isEmpty)
        {
            isWatered = true;
            lastWatered = Time.time;
            ChangeMaterial(wetMaterial);

            nextCropType = cropType + 1;
            can.Use();
        } 
        else if (canHarvest && item is ItemScyth)
        {
            var produceCount = GetProduceCount(cropType);

            isWatered = false;
            
            ProduceProduce(plantType, produceCount);
            CropTransition(cropType, CropType.Plain);
        }
        else
        {
            animateItem = false;
        }

        item?.StartAnimation(animateItem ? ItemAnimationType.Success : ItemAnimationType.Failure);
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
        !isWatered && (
            cropType == CropType.Sowed || 
            cropType == CropType.Sprout ||
            cropType == CropType.Juvenile ||
            cropType == CropType.Mature
        )
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
    
    private GameObject GetCropPrefab(CropType cropType)
    {
        GameObject asset = null;

        switch (cropType)
        {
            case CropType.Plain:
                meshFilter.mesh = plainMesh;
                break;
            case CropType.Tilled:
                meshFilter.mesh = tilledMesh;
                break;
            case CropType.Sowed:
                asset = Resources.Load<GameObject>("crops/sowed");
                break;
            case CropType.Sprout:
                asset = Resources.Load<GameObject>("crops/sprout");
                break;
            case CropType.Juvenile:
                asset = modelLookup[plantType][0];
                break;
            case CropType.Mature:
                asset = modelLookup[plantType][1];
                break;
            case CropType.Overgrown:
                asset = modelLookup[plantType][2];
                break;
        }

        return asset;
    }

    void ChangeMaterial(Material m)
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = m;
    }

    void CropTransition(CropType from, CropType to) {
        cropType = to;
        
        if(plantModel != null) Destroy(plantModel);
        
        GameObject prefab = GetCropPrefab(to);
        
        if(prefab!=null) {
            plantModel = Instantiate(prefab, transform);
        }
    } 
}