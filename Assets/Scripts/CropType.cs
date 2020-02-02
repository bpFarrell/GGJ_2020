using UnityEngine;
using System.Collections.Generic;

public enum CropType
{
    Plain,
    Rocky,
    Tilled,
    Sowed,
    Sprout,
    Juvenile,
    Mature,
    Overgrown,
}

public static class CropMeta
{
    private static Dictionary<CropType, Material> cropMaterials;

    static CropMeta()
    {
        cropMaterials = new Dictionary<CropType, Material>()
        {
            {CropType.Plain, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Rocky, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Tilled, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Sowed, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Sprout, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Juvenile, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Mature, Resources.Load<Material>("Unlit_PlantRed")},
            {CropType.Overgrown, Resources.Load<Material>("Unlit_PlantRed")},
        };
    }

    public static Material GetMaterial(CropType type)
    {
        return cropMaterials[type];
    }
}