using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    public static ModeData modeData;

    public CardHandler cardHandler;
    internal CanvasController canvasController;
    internal Stage stage;
    internal MusicController musicController;
    CameraController cameraController;

    [SerializeField, PrefabReference] GameObject[] orbPrefabArray;
    [SerializeField, PrefabReference] GameObject[] cardPrefabArray;
    [PrefabReference] public GameObject teleportEffectPrefab;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource endSFX;

    internal Orb[] orbArray;
    internal Orb[] nonNullOrbArray;
    internal GameState state;
    float endTime;

    [Header("Debug")]
    [SerializeField] bool standaloneSceneStartsAsMarathon = true;

    int OrbReachGoalCount => nonNullOrbArray.Sum(orb => orb.reachGoal ? 1 : 0);
    public bool IsMultiplayer => nonNullOrbArray.Count(o => !o.IsCPU) > 1;
    public Orb ClickInputOrb => nonNullOrbArray.FirstOrDefault(p => p.inputHandler.CanClick);
    public float CurrentTime => GameState.Ocurring==state ? Time.timeSinceLevelLoad : endTime;
    public bool Paused => Time.timeScale == 0 && state == GameState.Ocurring;

    void Awake() {
        canvasController = FindObjectOfType<CanvasController>();
        stage = FindObjectOfType<Stage>();
        cameraController = FindObjectOfType<CameraController>();
        musicController = FindObjectOfType<MusicController>();
        FormatModeData();
    }

    void Start() {
        Time.timeScale = 0;
        state = GameState.OnInitialAnimation;
        stage.InstantiateOrbs(orbPrefabArray);
    }

    void FormatModeData() {
        if (modeData != null)
            return;
#if UNITY_EDITOR
        modeData = standaloneSceneStartsAsMarathon ? (ModeData)new MarathonData() : new QuickRaceData();
#else
        Debug.LogError("No mode defined!");
#endif
    }

    public void OnCameraInitialAnimationEnd() {
        state = GameState.SelectPlayer;
        switch (modeData) {
            case MarathonData m:
                if (m.element == Element.None)
                    canvasController.startText.gameObject.SetActive(true);
                else
                    StartMarathon(FindObjectsOfType<Orb>().First(p => p.element == m.element));
                break;
            case QuickRaceData qr:
                canvasController.playerSelectScreen.gameObject.SetActive(true);
                break;
        }
    }

    public void StartMarathon(Orb selectedOrb) {
        InitializeOrbArrayInMarathon(selectedOrb);
        StartGame();
    }

    public void StartQuickRace() {
        InitializeOrbArrayInQuickRace();
        StartGame();
    }


    void InitializeOrbArrayInQuickRace() {
        orbArray = new Orb[FindObjectsOfType<Orb>().Length + 1];
        for (int i = 0; i < canvasController.playerSelectScreen.panelArray.Length; i++) {
            PlayerSelectPanel panel = canvasController.playerSelectScreen.GetPanelWithCPULast()[i];
            InitializeOrb(
                i + 1,
                FindObjectsOfType<Orb>().FirstOrDefault(orb => orb.element == panel.element),
                InputHandler.Factory(panel.type),
                panel.IsCPU() ? GetCPULevelPerType(panel.type) : 0
            );
        }
        nonNullOrbArray = orbArray.Where(player => player != null).ToArray();
    }

    void InitializeOrbArrayInMarathon(Orb firstPlayerOrb) {
        (modeData as MarathonData).element = firstPlayerOrb.element;
        orbArray = new Orb[FindObjectsOfType<Orb>().Length + 1];
        InitializeOrb(1, firstPlayerOrb, new ClickInputHandler());
        int orbIndex = 2;
        foreach (var orb in FindObjectsOfType<Orb>()) {
            if (orb.Initialized)
                continue;
            InitializeOrb(orbIndex++, orb, new CPUInputHandler(), (modeData as MarathonData).level);
        }
        nonNullOrbArray = orbArray.Where(player => player != null).ToArray();
    }

    Orb[] CreateNonNullOrbArray() => orbArray.Where(player => player != null).ToArray();

    int GetCPULevelPerType(PlayerType playerType) {
        switch (playerType) {
            case PlayerType.CPUEasy: return 1;
            case PlayerType.CPUNormal: return 3;
            case PlayerType.CPUHard: return 6;
        }
        Debug.LogError($"Invalid type {playerType}!");
        return 0;
    }

    void InitializeOrb(int number, Orb orb, InputHandler inputHandler, int aiLevel = 0) {
        orbArray[number] = orb;
        if (aiLevel == 0)
            orb.InitializeAsHuman(number, inputHandler, canvasController.NextAvailableCardZone);
        else
            orb.InitializeAsCPU(number, inputHandler, aiLevel);
    }

    void StartGame() {
        canvasController.OnGameStart();
        cameraController.OnGameStart();
        Time.timeScale = 1;
        startSFX.Play();
        state = GameState.Ocurring;
        DrawCardLoop();
    }

    async void DrawCardLoop() {
        while (state == GameState.Ocurring) {
            foreach (var orb in nonNullOrbArray) {
                if (orb.reachGoal)
                    continue;
                DrawCard(orb);
            }
            await new WaitForSeconds(5);
        }
    }

    void DrawCard(Orb orb) {
        if (orb.IsCPU) {
            orb.ai.cardTypeDeck.Add(EnumUtil.GetRandomValueFromEnum<CardType>(1, -4));
        } else {
            orb.cardZone.Add(cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)]);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    public async void EndGame(Orb winnerOrb) {
        EndGameState(winnerOrb);
        endSFX.Play();
        await new WaitForSecondsRealtime(1.2f);
        canvasController.DisplayVictoryText(winnerOrb);
        await new WaitForSecondsRealtime(2.5f);
        await new WaitMultiple(this, 1, new WaitForSecondsRealtime(4f), new WaitUntil(() => OrbReachGoalCount >= nonNullOrbArray.Length));
        await new WaitForSecondsRealtime(0.5f);
        await canvasController.fader.FadeOut().WaitForCompletion();
        GoToNextScene(winnerOrb);
    }

    void EndGameState(Orb winnerOrb) {
        endTime = CurrentTime;
        state = GameState.End;
        Debug.Log($"Player {winnerOrb.number} won in {CurrentTime}s!");
    }

    void GoToNextScene(Orb winnerOrb) {
        if (modeData is MarathonData) {
            if(winnerOrb.IsCPU)
                BackToMainMenu(true);
            else
                GoToNextRace();
        } else {
            GoToNextRace();
        }
    }

    void GoToNextRace() {
        if(modeData is MarathonData)
            (modeData as MarathonData).level++;
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu(bool gameLost = true) {
        if (gameLost && modeData is MarathonData)
            (modeData as MarathonData).SaveLastScore();
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause() {
        if (state != GameState.Ocurring)
            return;
        Time.timeScale = Paused ? 1 : 0;
        if(musicController != null) {
            if (Paused)
                musicController.Pause();
            else
                musicController.Play();
        }
    }

    public Orb GetOrb(Element element) => nonNullOrbArray.First(o => o.element == element);
}
