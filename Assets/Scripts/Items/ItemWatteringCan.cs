using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWatteringCan : ItemBase
{
    public int maxWaterStacks = 3;
    public int currentWater;
    public bool isEmpty => currentWater == null;
    public void Fill() {
        currentWater = maxWaterStacks;
    }

}
