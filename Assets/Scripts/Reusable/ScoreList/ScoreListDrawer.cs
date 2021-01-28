using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Draw a Score List. Put on an empty object as pivot.
/// Version 4.7
/// </summary>
public abstract class ScoreListDrawer<TScoreList, TScoreListGeneric> : MonoBehaviour
		where TScoreListGeneric : struct, System.IComparable
		where TScoreList : ScoreList<TScoreListGeneric>, new() {
	public GameObject textScorePrefab;
	public Transform textScoreParent; // Parent to instantiate textScorePrefab. If null uses self
	public string newRecordFormatMessage = "{0} !";
	public bool clearLastScoreAfterShowing;
	bool showedLastScore;

	[Header("Current Score")]
    public Transform currentScoreLabelTransform; // Current Score label to be show
    public Transform currentScoreValueTransform; // Current Score value to be show
	public string currentScoreDefaultMessage; // When empty, simply disable 

	// Override this property with a static variable
	protected abstract TScoreListGeneric? LastScore { get; set; }

    protected virtual void Reset() {
        if (textScoreParent == null)
            textScoreParent = transform;
    }

	protected virtual void Start() {
		Draw();
	}

	public void Restart() {
		Clear();
		Draw();
	}

	/// <summary>
	/// Draw the prefabs.
	/// </summary>
	public virtual void Draw() {
		showedLastScore = false;
		TScoreList scoreList = new TScoreList();
		scoreList.Load();
		for (int i = 0; i < scoreList.Size; i++)
			CreateTextScore(scoreList, i);
		RefreshCurrentScore(scoreList);
		if (clearLastScoreAfterShowing)
			LastScore = null;
	}

	protected virtual void RefreshCurrentScore(TScoreList scoreList) {
		if (currentScoreLabelTransform != null)
            currentScoreLabelTransform.gameObject.SetActive(LastScore != null || !string.IsNullOrEmpty(currentScoreDefaultMessage));
		if (currentScoreValueTransform != null) {
			if (LastScore == null && string.IsNullOrEmpty(currentScoreDefaultMessage)) {
                currentScoreValueTransform.gameObject.SetActive(false);
			} else {
                currentScoreValueTransform.gameObject.SetActive(true);
                if (LastScore == null)
                    SetText(currentScoreValueTransform, currentScoreDefaultMessage);
                else
                    SetText(currentScoreValueTransform, scoreList.GetStringAsValue((TScoreListGeneric)LastScore));
			}
		}
	}

	protected Transform CreateTextScore(TScoreList scoreList, int index) {
        Transform ret = Instantiate(textScorePrefab, textScoreParent).transform;
		SetText(ret, FormatText(index + 1, scoreList.GetString(index)));
		if (!showedLastScore && IsNewRecord(scoreList.values[index])) {
			FormatTextNewRecord(ret);
			showedLastScore = true;
		}
		return ret;
	}

	protected virtual string FormatText(int number, string valueString) {
		return string.Format("{0:00}. {1}", number, valueString);
    }

    protected bool IsNewRecord(TScoreListGeneric score) {
		return LastScore != null && Mathf.Approximately(
			(float)System.Convert.ChangeType(score, typeof(float)),
			(float)System.Convert.ChangeType(LastScore, typeof(float))
		);
	}

	/// <summary>
	/// Receive a text component and format it with New Record text.
	/// </summary>
	protected virtual void FormatTextNewRecord(Transform textTransform) {
		if (newRecordFormatMessage.Trim() == string.Empty)
			return;
		SetText(textTransform, string.Format(newRecordFormatMessage, GetText(textTransform)));
	}

	/// <summary>
	/// Remove all children.
	/// </summary>
	public virtual void Clear() {
		foreach (Transform child in textScoreParent)
			Destroy(child.gameObject);
    }

    /// <summary>
    /// Set text component string. created to easily change between Unity UI, TextMeshPro and others.
    /// </summary>
    protected virtual void SetText(Transform textTransform, string value) {
        textTransform.GetComponent<Text>().text = value;
    }

    /// <summary>
    /// Get text component string.
    /// </summary>
    protected virtual string GetText(Transform textTransform) {
        return textTransform.GetComponent<Text>().text;
    }
}
