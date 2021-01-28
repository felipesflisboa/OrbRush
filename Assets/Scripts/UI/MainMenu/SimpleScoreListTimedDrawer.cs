using UnityEngine;
using TMPro;

public class SimpleScoreListTimedDrawer : ScoreListDrawer<ScoreListTimed, int>{
	public static int? lastScore;

	protected override int? LastScore{
		get => lastScore;
		set => lastScore = value;
    }
    
    protected override void SetText(Transform textTransform, string value) => textTransform.GetComponent<TextMeshProUGUI>().text = value;
    protected override string GetText(Transform textTransform) => textTransform.GetComponent<TextMeshProUGUI>().text;
}