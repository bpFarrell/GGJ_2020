using UnityEngine;
public enum PlantType {
    Red,
    Yellow,
    Green
}
public static class PlantMeta {
    private static Material redMat;
    private static Material yellowMat;
    private static Material greenMat;
    public static Material GetMaterial(PlantType type) {
        switch (type) {
            case PlantType.Red:
                return redMat ?? (redMat = Resources.Load("Unlit_PlantRed") as Material);
                break;
            case PlantType.Yellow:
                return yellowMat ?? (yellowMat = Resources.Load("Unlit_PlantYellow") as Material);
                break;
            case PlantType.Green:
                return greenMat ?? (greenMat = Resources.Load("Unlit_PlantGreen") as Material);
                break;
            default:
                return null;
                break;
        }
    }
}