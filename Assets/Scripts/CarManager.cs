using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarManager : MonoBehaviour {
    public CarBehavior prefab;

    public CarBehavior[] carList = { null, null, null };
    public Vector3[] positionList = new Vector3[3];

    public AnimationCurve chanceCurve;

    public bool spawn;
    [HideInInspector]
    public static bool onFirstCar = true;
    public static Action onFirstLoad;
    private void Start() {
        Crop.OnFirstCrop += type => { SpawnCar(new[] { type }); };
    }
    private void OnDisable() {

        Crop.OnFirstCrop = null;
    }
    private void Update() {
        if (spawn) {
            spawn = false;
            SpawnCar(RandomDropBox());
        }
    }

    private PlantType[] RandomDropBox() {
        int rand = Random.Range(1, 5);
        rand = (int)(chanceCurve.Evaluate(rand / 4f)*4);
        PlantType[] typeArr = new PlantType[rand];
        
        for (var i = 0; i < typeArr.Length; i++) {
            typeArr[i] = RandomType();
        }

        return typeArr;
    }
    
    private PlantType RandomType() {
        int rand = Random.Range(0, 3);
        switch (rand) {
            case 0:
                return PlantType.Red;
            case 1:
                return PlantType.Green;
            case 2:
                return PlantType.Yellow;
            default:
                return PlantType.Yellow;
        }
    }

    private void SpawnCar(PlantType[] plantTypes) {
        if (CountNullCars() == 0) return;
        CarBehavior go = Instantiate(prefab, transform);
        int nextAvailableIndex = NextAvailableIndex();
        carList[nextAvailableIndex] = go;
        go.target = positionList[nextAvailableIndex];
        go.dropBox.InitDelivery(plantTypes);
        go.dropBox.onDelivered= () => {
            if (onFirstCar) {
                onFirstCar = false;
                RecursiveSpawnCar(RandomDropBox());
            }
            DeliveryCompleted(go);
        };
    }

    private void RecursiveSpawnCar(PlantType[] plantTypes) {
        SpawnCar(plantTypes);
        PigTimer.Instance.SetTimer(15f, () => {
            RecursiveSpawnCar(RandomDropBox());
        });
    }

    private void DeliveryCompleted(CarBehavior car) {
        int carIndex = IndexAtObject(car);
        if (carIndex == -1 || carIndex != 0) return;

        for (int i = 0; i < carList.Length; i++) {
            if (carList[i].dropBox.isDelivered) {
                carList[i].Depart(i);
                carList[i] = null;
            } else {
                int nextIndex = NextAvailableIndex();
                carList[i].target = positionList[nextIndex];
                carList[nextIndex] = carList[i];
                carList[i] = null;
                carList[nextIndex].MoveToTarget(i);
            }
        }
    }

    private int CountNullCars() {
        return carList.Where((c) => { return c == null; }).Count();
    }
    
    private int NextAvailableIndex() {
        for (var i = 0; i < carList.Length; i++) {
            if (carList[i] == null) return i;
        }
        return -1;
    }

    private int IndexAtObject(CarBehavior car) {
        for (int i = 0; i < carList.Length; i++) {
            if (carList[i] == car) return i;
        }
        return -1;
    }
}
