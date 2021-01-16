using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    internal CardZone cardZone;
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
        cardZone = GetComponentInChildren<CardZone>();
    }

    void Start() {
        levelText.text = $"Level {GameManager.level}";
    }

    void Update() { //TODO
        timeText.text = $"{GameManager.I.CurrentTime.ToString("F3")}s";
        cardAlertText.gameObject.SetActive(cardZone.cardList.Count >= 6); //TODO optimize //TODO only when 1P
    }
}
