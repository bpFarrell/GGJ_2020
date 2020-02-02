using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSeedBag : ItemBase
{
    public PlantType plantType;
    private void Start()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        
        //they've all got the same material so just use the first one to make the new ref
        var plantColor = Color.red;

        if (plantType == PlantType.Green)
        {
            plantColor = Color.green;
        }
        else if (plantType == PlantType.Yellow)
        {
            plantColor = Color.yellow;
        }

        var newMaterial = new Material(meshRenderers[0].material)
        {
            color = plantColor
        };

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = newMaterial;
        }
    }
    
    
}
