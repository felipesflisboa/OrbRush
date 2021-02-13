using UnityEngine;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class ScoreListMarathonDrawer : ScoreListDrawer<ScoreListMarathon, int> {
    [System.Serializable]
    public class ElementMaterialDictionary : SerializableDictionaryBase<Element, Material> { }

    public ElementMaterialDictionary orbMaterialPerElement;

    public static int? lastScore;
    public static Element lastElement;

    protected override int? LastScore {
        get => lastScore;
        set => lastScore = value;
    }

    public static void ClearLast() {
        lastScore = null;
        lastElement = Element.None;
    }

    protected override void SetText(Transform textTransform, string value) => textTransform.GetComponentInChildren<TextMeshProUGUI>().text = value;
    protected override string GetText(Transform textTransform) => textTransform.GetComponentInChildren<TextMeshProUGUI>().text;

    void SetScoreModel(Transform transform, Element element) => SetScoreModel(transform.GetComponentInChildren<MeshRenderer>(), element);

    void SetScoreModel(MeshRenderer renderer, Element element) {
        if (element == Element.None)
            renderer.gameObject.SetActive(false);
        else
            renderer.sharedMaterial = orbMaterialPerElement[element];
    }

    protected override Transform CreateTextScore(ScoreListMarathon scoreList, int index) {
        Transform ret = Instantiate(textScorePrefab, textScoreParent).transform;
        SetText(ret, FormatText(index + 1, scoreList.GetString(index)));
        SetScoreModel(ret, scoreList.orbElements[index]);
        if (!showedLastScore && IsNewRecord(scoreList.values[index], scoreList.orbElements[index])) {
            FormatTextNewRecord(ret);
            showedLastScore = true;
        }
        return ret;
    }

    bool IsNewRecord(int score, Element element) {
        return LastScore != null && lastElement==element && Mathf.Approximately(
            (float)System.Convert.ChangeType(score, typeof(float)),
            (float)System.Convert.ChangeType(LastScore, typeof(float))
        );
    }

    protected override void RefreshCurrentScore(ScoreListMarathon scoreList) {
        base.RefreshCurrentScore(scoreList);
        SetScoreModel(currentScoreValueTransform, lastElement);
    }
}