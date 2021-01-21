using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//TODO remove singleton
public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    [Tooltip("Per player")] public CardZone[] cardZoneArray;
    internal PlayerSelectScreen playerSelectScreen;
    public RectTransform hudRectTransform;
    public Text startText;
    public Text levelText;
    public Text victoryText;
    public Text cardAlertText;
    public Text timeText;
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
        hudRectTransform.gameObject.SetActive(false);
        playerSelectScreen.gameObject.SetActive(false);
        cardAlertText.gameObject.SetActive(false);
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
        hudRectTransform.gameObject.SetActive(true);
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

    void Start() {
        levelText.text = GameManager.modeData is MarathonData ? $"Level {(GameManager.modeData as MarathonData).level}" : string.Empty;
    }

    void Update() {
        timeText.text = $"{GameManager.I.CurrentTime.ToString("00.000")} s";
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
}
