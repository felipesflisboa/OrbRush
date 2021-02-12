using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

/// <summary>
/// General game HUD
/// </summary>
public class HUD : MonoBehaviour {
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    [SerializeField] bool alwaysShowModeName;

    static readonly CultureInfo US_INFO = new CultureInfo("en-US");

    void Start() {
        if (GameManager.modeData is MarathonData)
            levelText.text = $"Level {(GameManager.modeData as MarathonData).level}";
        else
            levelText.text = alwaysShowModeName ? "Quick Race" : string.Empty;
    }

    void Update() {
        timeText.text = $"{GameManager.I.CurrentTime.ToString("00.000", US_INFO)} s";
    }
}
