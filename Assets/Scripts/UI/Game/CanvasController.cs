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
    [SerializeField] Transform pauseButtonParent;
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

    void Awake() {
        Initialize();
    }

    void Initialize() {
        DestroyWrongPlatformComponents();
        InitializeCardZone();
        InitializePauseButton();
        playerSelectScreen = GetComponentInChildren<PlayerSelectScreen>(true);
        playerSelectScreen.gameObject.SetActive(false);
        hud = GetComponentInChildren<HUD>(true);
        hud.gameObject.SetActive(false);
        alert = GetComponentInChildren<Alert>(true);
        alert.gameObject.SetActive(true);
    }

    void DestroyWrongPlatformComponents() {
        foreach (var platformDependent in GetComponentsInChildren<PlatformDependentComponent>(true))
            platformDependent.Execute();
    }

    void InitializeCardZone() {
        foreach (var cardZone in GetComponentsInChildren<CardZone>(true)) {
            if (cardZone == null)
                continue;
            cardZoneArray[cardZone.player] = cardZone;
            cardZone.gameObject.SetActive(false);
        }
    }

    /* //remove
    void InitializePauseButtons() {
        foreach (var pauseButtonRectTransform in pauseButtonRectTransformArray) {
            if (pauseButtonRectTransform == null)
                continue;
            pauseButtonRectTransform.GetComponentInChildren<Button>().onClick.AddListener(GameManager.I.TogglePause);
            pauseButtonRectTransform.gameObject.SetActive(false);
        }
    }
    */

    void InitializePauseButton() {
        pauseButtonParent.GetComponentInChildren<Button>().onClick.AddListener(GameManager.I.TogglePause);
        pauseButtonParent.GetComponentInChildren<Button>().onClick.AddListener(() => Debug.Log("Test"));
        pauseButtonParent.gameObject.SetActive(false);
    }

    public void OnGameStart() {
        playerSelectScreen.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        pauseButtonParent.gameObject.SetActive(true);
        EnableActiveCardZones();
        alert.enabled = !GameManager.I.IsMultiplayer;
        if(GameManager.I.IsMultiplayer)
            Destroy(pauseButtonParent);
    }

    void EnableActiveCardZones() {
        foreach (var cardZone in cardZoneArray) {
            if (cardZone == null || !cardZone.Active)
                continue;
            cardZone.gameObject.SetActive(true);
        }
    }

    /* //remove
    void DestroyPauseButtons() {
        foreach (var pauseButtonRectTransform in pauseButtonRectTransformArray) {
            if (pauseButtonRectTransform == null)
                continue;
            Destroy(pauseButtonParent);
        }
    }
    */

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