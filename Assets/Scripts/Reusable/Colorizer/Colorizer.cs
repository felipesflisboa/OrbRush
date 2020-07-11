using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Change a transform color over the time. Only works if the material has the property _Color, otherside ignores.
/// Requires AutoColorizer.
///
/// Version 6.2.
/// </summary>
public class Colorizer {
    public Transform mainTransform { get; private set; }
    public Color colorToTint;
    public bool setInChildren = true; // Recursively called on childrens
    public bool onlyFirstMaterial = true; // Only affect the first object material

    // When not null, keep cached and updates and dictionary array of materials per transform in mainTransform hierarchy.
    Dictionary<int, Material[]> materialPerTransformId = new Dictionary<int, Material[]>();

    bool useDefaultProperty = true;
    string _property = "_Color"; // Material color property.
    public string Property{
        get{
            return _property;
        }
        set{
            _property = value;
            useDefaultProperty = value == "_Color";
        }
    }
    public Colorizer(Transform pMainTransform) {
        mainTransform = pMainTransform;
    }

    public void ResetsCachedMaterials() {
        if(materialPerTransformId==null)
            materialPerTransformId = new Dictionary<int, Material[]>();
        else
            materialPerTransformId.Clear();
    }

    /// <summary>
    /// Stop caching the materials.
    /// </summary>
    public void StopCachedMaterialsUse() {
        materialPerTransformId = null;
    }

    /// <summary>
    /// Paint the transform.
    /// </summary>
    /// <param name="colorRatio">When informed, uses as lerp paramether between material original color and definedColor.</param>
    /// <returns>If something is painted.</returns>
    public bool Tint(float colorRatio=1f) {
        return Tint(mainTransform, colorRatio);
    }

    bool Tint(Transform transform, float colorRatio) {
        bool finished = true;

        bool hasOtherSetColor = false;
        bool hasOtherSetColorWithSetInChildren = false;

        if (!hasOtherSetColor) {
            Material[] materialUsedArray = null;

            bool loadCachedMaterialArray = materialPerTransformId != null && materialPerTransformId.ContainsKey(transform.GetInstanceID());
            if (loadCachedMaterialArray) {
                materialUsedArray = materialPerTransformId[transform.GetInstanceID()];
            } else {
                // Ignore objects and chidren with AutoColorizer.
                if (transform != mainTransform) { // Ignores self component
                    AutoColorizer transformSetColor = transform.GetComponent<AutoColorizer>();
                    hasOtherSetColor = transformSetColor != null;
                    if (hasOtherSetColor)
                        hasOtherSetColorWithSetInChildren = transformSetColor.setInChildren;
                }

                if (!hasOtherSetColor) {
                    Renderer transformRenderer = transform.GetComponent<Renderer>();
                    if (transformRenderer != null)
                        materialUsedArray = onlyFirstMaterial ? new[] { transformRenderer.material } : transformRenderer.materials;

                    bool registryOnDictionary = materialPerTransformId != null;
                    if (registryOnDictionary) // Registry material array with transform ID.
                        materialPerTransformId.Add(transform.GetInstanceID(), materialUsedArray);
                }
            }

            if (materialUsedArray != null) {
                foreach (Material transformMaterial in materialUsedArray) {
                    if (transformMaterial != null && transformMaterial.HasProperty(Property)){
                        Color materialCurrentColor = useDefaultProperty ? transformMaterial.color : transformMaterial.GetColor(Property);
                        if(materialCurrentColor != colorToTint) {
                            Color newColorValue = colorRatio == 1f ? colorToTint : Color.Lerp(materialCurrentColor, colorToTint, colorRatio);
                            if(useDefaultProperty)
                                transformMaterial.color = newColorValue;
                            else
                                transformMaterial.SetColor(Property, newColorValue);
                            if (newColorValue != colorToTint)
                                finished = false;
                        }
                    }
                }
            }
        }

        if (setInChildren && !hasOtherSetColorWithSetInChildren) {
            foreach (Transform child in transform) {
                if (!Tint(child, colorRatio))
                    finished = false;
            }
        }

        return finished;
    }
}
