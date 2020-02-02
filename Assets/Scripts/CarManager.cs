using System.Linq;
using UnityEngine;

public class CarManager : MonoBehaviour {
    public CarBehavior prefab;

    public CarBehavior[] carList = {null, null, null};
    public Vector3[] positionList = new Vector3[3];

    public AnimationCurve chanceCurve;

    public bool spawn;

    private float counter;
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
        var car = Instantiate(prefab, transform);
        var nextAvailableIndex = NextAvailableIndex();
        carList[nextAvailableIndex] = car;
        car.target = positionList[nextAvailableIndex];
        car.dropBox.InitDelivery(plantTypes);
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
}
