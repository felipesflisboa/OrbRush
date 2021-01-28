using UnityEngine;

[System.Serializable]
public class ElementColorGroup{
    [SerializeField] Color fireColor;
    [SerializeField] Color waterColor;
    [SerializeField] Color earthColor;
    [SerializeField] Color airColor;

    public Color GetColor(Element element) {
        switch (element) {
            case Element.Fire:  return fireColor;
            case Element.Water: return waterColor;
            case Element.Earth: return earthColor;
            case Element.Air:   return airColor;
        }
        return Color.magenta;
    }
}