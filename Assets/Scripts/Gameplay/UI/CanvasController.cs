using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    internal CardZone cardZone;
    public Text startText;
    public Text playerText;
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

    void Update() { //TODO
        timeText.text = $"{Time.timeSinceLevelLoad.ToString("F3")}s";
        cardAlertText.gameObject.SetActive(cardZone.cardList.Count >= 5); //TODO optimize
    }
}
