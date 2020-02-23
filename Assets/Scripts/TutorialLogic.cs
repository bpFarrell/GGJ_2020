using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState
{
    Start           = 0,
    Tilled          = 1,
    SpawnedSeeds    = 2,
    PlantedSeeds    = 3,
    SpawnedCan      = 4,
    Watered         = 5,
    SpawnedWell     = 6,
    FullyGrown      = 7,
    SpawnedScyth    = 8,
    FirstDelivery   = 9,
    SecondDelivery = 10,
    SpawnedHammer = 11
};
public class TutorialLogic : MonoBehaviour
{
    public TutorialState state = TutorialState.Start;
    public GameObject[] seedBoxes;
    public GameObject waterCan;
    public GameObject waterWell;
    public GameObject scyth;
    public GameObject hammer;
    private static TutorialLogic instance;
    void Awake()
    {
        instance = this;
        for (int i = 0; i < seedBoxes.Length; i++)
        {
            seedBoxes[i].SetActive(false);
        }
        waterCan.SetActive(false);
        waterWell.SetActive(false);
        scyth.SetActive(false);
        hammer.SetActive(false);
    }
    void Update()
    {
        switch (state)
        {
            case TutorialState.Tilled:
                for (int i = 0; i < seedBoxes.Length; i++)
                {
                    seedBoxes[i].SetActive(true);
                }
                state++;
                break;
            case TutorialState.PlantedSeeds:
                waterCan.SetActive(true);
                state++;
                break;
            case TutorialState.Watered:
                waterWell.SetActive(true);
                state++;
                break;
            case TutorialState.FullyGrown:
                scyth.SetActive(true);
                state++;
                break;
            case TutorialState.SecondDelivery:
                scyth.SetActive(true);
                state++;
                break;
            default:
                break;
        }
    }
    public static void TillTransition(CropType type)
    {
        switch (instance.state)
        {
            case TutorialState.Start:
                if(type==CropType.Tilled) instance.state++;
                break;
            case TutorialState.SpawnedSeeds:
                if (type == CropType.Sowed) instance.state++;
                break;
            case TutorialState.SpawnedWell:
                if (type == CropType.Mature) instance.state++;
                break;
            default:
                break;
        }
    }
    public static void WaterTransition()
    {
        if(instance.state == TutorialState.SpawnedCan)
        {
            instance.state++;
        }
    }
    public static void DeliverTransition()
    {
        if(instance.state == TutorialState.SpawnedScyth)
        {
            instance.state++;
        }else if(instance.state == TutorialState.FirstDelivery) {
            instance.hammer.SetActive(true);
            instance.state++;
        }
    }
}
