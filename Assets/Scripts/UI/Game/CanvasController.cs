using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;
using System.Globalization;
using DG.Tweening;

//TODO remove singleton
public class CanvasController : MonoBehaviour {
    [System.Serializable] class ElementColorDictionary : SerializableDictionaryBase<Element, Color> { }

    [SerializeField] TextAnimationHandler textAnimationHandler;
    internal CardZone[] cardZoneArray = new CardZone[5];
    internal PlayerSelectScreen playerSelectScreen;
    HUD hud;
    [SerializeField] Transform pauseButtonParent;
    public TextMeshProUGUI startText;
    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] ElementColorDictionary textColorPerElement; 
    internal Alert alert;
    internal Fader fader;
    float lastCardZoneCount;

    public static readonly CultureInfo US_INFO = new CultureInfo("en-US");

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
        fader = GetComponentInChildren<Fader>(true);
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

    void InitializePauseButton() {
        pauseButtonParent.GetComponentInChildren<Button>().onClick.AddListener(GameManager.I.TogglePause);
        pauseButtonParent.gameObject.SetActive(false);
    }

    public void OnGameStart() {
        playerSelectScreen.gameObject.SetActive(false);
        if (startText.gameObject.activeSelf)
            HideTextWithDilateAnimation(startText);
        hud.gameObject.SetActive(true);
        pauseButtonParent.gameObject.SetActive(true);
        EnableActiveCardZones();
        alert.enabled = !GameManager.I.IsMultiplayer;
        if(GameManager.I.IsMultiplayer)
            Destroy(pauseButtonParent.gameObject);
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
            if (lastCardZoneCount >= 6 && !alert.displayingText)
                alert.Display("Your hand is full.\nCards make your orb slower!", 4f);
        }
    }

    public void DisplayOfftrackAlert() {
        alert.Display("Orb is off track!", 3);
    }

    public void DisplayVictoryText(Orb winnerOrb) {
        victoryText.text = string.Format(
            "Player <color=#{1}>{0}</color> won!",
            winnerOrb.m_name,
            ColorUtility.ToHtmlStringRGB(textColorPerElement[winnerOrb.element])
        );
        ShowTextWithDilateAnimation(victoryText);
    }

    public void ShowTextWithDilateAnimation(TextMeshProUGUI text){
        textAnimationHandler.ShowTextWithDilateAnimation(text, 0.8f);
    }

    public void HideTextWithDilateAnimation(TextMeshProUGUI text, System.Action callback=null) {
        textAnimationHandler.HideTextWithDilateAnimation(text, 0.4f, callback ?? (() => text.gameObject.SetActive(false)));
    }
}