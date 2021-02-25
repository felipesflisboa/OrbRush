using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//TODO maybe stack
[RequireComponent(typeof(TextMeshProUGUI))]
public class Alert : MonoBehaviour {
    TextMeshProUGUI label;
    float clearTime;
    public bool displayingText { get; private set; } 


    void Awake() {
        label = GetComponent<TextMeshProUGUI> ();
        label.text = string.Empty;
    }

    void Update() {
        if (!enabled)
            return;
        if (clearTime != 0f && clearTime < Time.timeSinceLevelLoad) {
            GameManager.I.canvasController.HideTextWithDilateAnimation(label, () => displayingText = false);
            clearTime = 0f;
        }
    }

    public void Display(string text, float duration) {
        if (!enabled)
            return;
        label.text = text;
        GameManager.I.canvasController.ShowTextWithDilateAnimation(label);
        clearTime = Time.timeSinceLevelLoad + duration;
        displayingText = true;
    }
}
