using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TODO remove singleton
public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    [Tooltip("Per player")] public CardZone[] cardZoneArray;
    internal PlayerSelectScreen playerSelectScreen;
    HUD hud;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI cardAlertText;
    public TextMeshProUGUI offtackAlertText;
    bool canShowCardAlert;

    public CardZone NextAvailableCardZone => cardZoneArray.First(cz => cz!=null && !cz.Active);

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
        InitializeCardZone();
        playerSelectScreen = GetComponentInChildren<PlayerSelectScreen>(true);
        hud = GetComponentInChildren<HUD>();
        hud.gameObject.SetActive(false);
        playerSelectScreen.gameObject.SetActive(false);
        cardAlertText.gameObject.SetActive(false);
        offtackAlertText.gameObject.SetActive(false);
    }

    void InitializeCardZone() {
        foreach(var cardZone in cardZoneArray) {
            if (cardZone == null)
                continue;
            cardZone.gameObject.SetActive(false);
        }
    }

    public void OnGameStart() {
        playerSelectScreen.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        EnableActiveCardZones();
        canShowCardAlert = cardZoneArray.Count(cz => cz != null && cz.Active) == 1;
    }

    void EnableActiveCardZones() {
        foreach (var cardZone in cardZoneArray) {
            if (cardZone == null || !cardZone.Active)
                continue;
            cardZone.gameObject.SetActive(true);
        }
    }

    void Update() {
        RefreshCardAlert();
    }

    void RefreshCardAlert() {
        if (!canShowCardAlert)
            return;
        foreach (var cardZone in cardZoneArray) {
            if (cardZone == null || !cardZone.Active)
                continue;
            cardAlertText.gameObject.SetActive(cardZone.cardList.Count >= 6);
        }
    }

    public void DisplayOfftrackAlert() {
        if (!canShowCardAlert)
            return;
        offtackAlertText.gameObject.SetActive(true);
        this.Schedule(new WaitForSeconds(3f), () => offtackAlertText.gameObject.SetActive(false));
    }
}