using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWatteringCan : ItemBase
{
    public int maxWaterStacks = 3;
    public int currentWater;
    public bool isEmpty => currentWater == 0;
    public void Fill() {
        currentWater = maxWaterStacks;
    }

    public void Use()
    {
        currentWater = currentWater > 0 ? currentWater - 1 : 0;
    }
}
