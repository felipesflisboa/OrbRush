using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TODO remove singleton
public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    internal CardZone[] cardZoneArray = new CardZone[5];
    internal PlayerSelectScreen playerSelectScreen;
    HUD hud;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI victoryText;
    internal Alert alert;
    float lastCardZoneCount;

    public CardZone NextAvailableCardZone => cardZoneArray.First(cz => cz != null && !cz.Active);
    Canvas _canvas;
    public Canvas Canvas {
        get {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }

    void Start() {
        Initialize();
    }

    void Initialize() {
        InitializeCardZone();
        playerSelectScreen = GetComponentInChildren<PlayerSelectScreen>(true);
        playerSelectScreen.gameObject.SetActive(false);
        hud = GetComponentInChildren<HUD>();
        hud.gameObject.SetActive(false);
        alert = GetComponentInChildren<Alert>(true);
        alert.gameObject.SetActive(true);
    }

    void InitializeCardZone() {
        foreach (var cardZone in GetComponentsInChildren<CardZone>(true)) {
            cardZoneArray[cardZone.player] = cardZone;
            cardZone.gameObject.SetActive(false);
        }
    }

    public void OnGameStart() {
        playerSelectScreen.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        EnableActiveCardZones();
        alert.enabled = cardZoneArray.Count(cz => cz != null && cz.Active) == 1;
    }

    void EnableActiveCardZones() {
        foreach (var cardZone in cardZoneArray) {
            if (cardZone == null || !cardZone.Active)
                continue;
            cardZone.gameObject.SetActive(true);
        }
    }

    void Update() {
        if(alert.enabled)
            RefreshCardAlert();
    }

    void RefreshCardAlert() {
        if (cardZoneArray[1].ValidCardCount != lastCardZoneCount) {
            lastCardZoneCount = cardZoneArray[1].ValidCardCount;
            if (lastCardZoneCount >= 6 && !alert.DisplayingText)
                alert.Display("Your hand is full.\nCards make your ball slower!", 4f);
        }
    }

    public void DisplayOfftrackAlert() {
        alert.Display("Orb is off track!", 3);
    }
}