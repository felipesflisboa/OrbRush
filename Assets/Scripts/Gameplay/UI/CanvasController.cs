using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO remove singleton
public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    internal PlayerSelectScreen playerSelectScreen;
    internal CardZone cardZone;
    public RectTransform hudRectTransform;
    public Text startText;
    public Text levelText;
    public Text victoryText;
    public Text cardAlertText;
    public Text timeText;

    Canvas _canvas;
    public Canvas Canvas {
        get {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }

    void Awake() {
        Initialize();
    }

    void Initialize() {
        cardZone = GetComponentInChildren<CardZone>(true);
        playerSelectScreen = GetComponentInChildren<PlayerSelectScreen>(true);
        hudRectTransform.gameObject.SetActive(false);
        cardZone.gameObject.SetActive(false);
        playerSelectScreen.gameObject.SetActive(false);
    }

    void Start() {
        levelText.text = GameManager.modeData is MarathonData ? $"Level {(GameManager.modeData as MarathonData).level}" : string.Empty;
    }

    void Update() { //TODO
        timeText.text = $"{GameManager.I.CurrentTime.ToString("00.000")} s";
        cardAlertText.gameObject.SetActive(cardZone.cardList.Count >= 6); //TODO optimize //TODO only when 1P
    }
}
