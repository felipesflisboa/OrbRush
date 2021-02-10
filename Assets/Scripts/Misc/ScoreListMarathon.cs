using UnityEngine;

/// <summary>
/// Saves the level and orb element of a Marathon record.
/// </summary>
public class ScoreListMarathon : ScoreListInt {
    public Element[] orbElements { protected set; get; }

    protected override string KeyLabel => "Impl10";
    string ElementKeyLabel => KeyLabel+"-"+"Element";

    public override string GetStringAsValue(int value)  => $"Level {value}";

    public override void Clear() {
        base.Clear();
        orbElements = new Element[Size];
        for (int i = 0; i < Size; i++) 
            values[i] = DefaultValue;
    }

    /// <summary>
    /// Add a new score and returns if the score is enough to be added.
    /// </summary>
    public virtual bool AddScore(int newScore, Element element) {
        bool somethingChanged = false;
        int nextScore = Asc ^ DefaultValue.CompareTo(newScore) < 0 ? newScore : DefaultValue;
        Element nextType = nextScore == DefaultValue ? Element.None : element;
        for (int i = 0; i < Size; i++) {
            if (Asc ^ nextScore.CompareTo(values[i]) > 0) {
                somethingChanged = true;

                int aux = values[i];
                values[i] = nextScore;
                nextScore = aux;

                Element eAux = orbElements[i];
                orbElements[i] = nextType;
                nextType = eAux;
            }
        }
        return somethingChanged;
    }
    
    public override void Save() {
        for (int i = 0; i < Size; i++)
            SaveSingleElement(ElementKeyLabel + "-" + i, orbElements[i]);
        base.Save();
    }

    void SaveSingleElement(string key, Element element) => PlayerPrefs.SetInt(key, (int)element);

    public override bool Load() {
        bool registryFound = base.Load();
        if (!registryFound)
            return false;
        for (int i = 0; i < Size; i++)
            orbElements[i] = LoadSingleElement(ElementKeyLabel + "-" + i);
        return true;
    }

    Element LoadSingleElement(string key) =>  (Element)PlayerPrefs.GetInt(key, DefaultValue);

    public override bool AddScore(int newScore) => throw new System.NotImplementedException("Use AddScore");
}