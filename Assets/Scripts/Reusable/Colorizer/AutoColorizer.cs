using UnityEngine;

/// <summary>
/// Auto apply Colorizer in MonoBehaviour.
/// Requires Colorizer.
///
/// Version 6.2.
/// </summary>
public class AutoColorizer : MonoBehaviour {
	public Color definedColor;
	public bool setInChildren = true; // Recursively called on childrens
	public bool onlyFirstMaterial = true; // Only affect the first object material
	public float totalTime; // Approximately time for changing color. When 0, change at start.
	public string overrideColorProperty; // When isn't empty, use it on material instead of default value..

	Colorizer colorizer;

	void Start () {
        colorizer = new Colorizer(transform);
		colorizer.colorToTint = definedColor;
		colorizer.setInChildren = setInChildren;
        colorizer.onlyFirstMaterial = onlyFirstMaterial;
		if (!string.IsNullOrEmpty(overrideColorProperty))
			colorizer.Property = overrideColorProperty;

        if (totalTime==0f){
            colorizer.Tint();
		    Destroy(this);
	    }
	}

	void Update () {
		float percentage01 = Time.deltaTime/totalTime;
		if(colorizer.Tint(percentage01))
			Destroy(this);
	}
}
