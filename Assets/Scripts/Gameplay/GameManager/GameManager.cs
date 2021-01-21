using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

//TODO Block CPU only game
public class GameManager : SingletonMonoBehaviour<GameManager> {
    public static ModeData modeData;

    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField, PrefabReference] GameObject[] orbPrefabArray;
    [SerializeField, PrefabReference] GameObject[] cardPrefabArray;
    [SerializeField, PrefabReference] GameObject explosionPrefab;
    [SerializeField] Material[] marathonSkyboxMaterialPerLevel; 

    [SerializeField] AudioSource cycloneSFX;
    [SerializeField] AudioSource squidSFX;
    [SerializeField] AudioSource earthquakeSFX;
    [SerializeField] AudioSource explodeSFX;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource endSFX;

    [Header("Debug")]
    [SerializeField] bool standaloneSceneStartsAsMarathon = true;

    internal MusicController musicController;
    internal Orb[] orbArray; //TODO protect
    internal Orb[] nonNullOrbArray;
    internal GameState state;
    internal Card selectedCard;
    internal List<Segment> segmentList;
    CameraController cameraController;
    float endTime;

    int OrbReachGoalCount => nonNullOrbArray.Sum(orb => orb.reachGoal ? 1 : 0);
    int OrbCount => spawnPointTransformArray.Length;
    public Orb ClickInputOrb => nonNullOrbArray.FirstOrDefault(p => p.inputHandler.CanClick);
    public float CurrentTime => GameState.Ocurring==state ? Time.timeSinceLevelLoad : endTime;
    public bool Paused => Time.timeScale == 0 && state == GameState.Ocurring;

    public Vector3 SpawnPointCenter{
        get{
            int validSize = 0;
            Vector3 ret = Vector3.zero;
            foreach (var spawnPoint in spawnPointTransformArray) {
                if (spawnPoint == null)
                    continue;
                ret += spawnPoint.position;
                validSize++;
            }
            return ret / validSize;
        }
    }

    void Awake() {
        cameraController = FindObjectOfType<CameraController>();
        FormatModeData();
    }

    void Start() {
        musicController = FindObjectOfType<MusicController>();
        Time.timeScale = 0;
        segmentList = CreateSegmentList();
        state = GameState.OnInitialAnimation;
        ConfigureSkybox();
        InstantiateOrbs();
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

    void ConfigureSkybox() {
        if (modeData is MarathonData)
            RenderSettings.skybox = marathonSkyboxMaterialPerLevel[(modeData as MarathonData).level % marathonSkyboxMaterialPerLevel.Length];
    }

    List<Segment> CreateSegmentList() {
        List<Segment> ret = FindObjectsOfType<Segment>().ToList();
        ret.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        return ret;
    }

    void InstantiateOrbs() {
        for (int i = 1; i < spawnPointTransformArray.Length; i++)
            Instantiate(orbPrefabArray[i], spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation);
    }

    void InitializeOrbArrayInQuickRace() {
        Orb[] orbTempArray = FindObjectsOfType<Orb>();
        orbArray = new Orb[spawnPointTransformArray.Length];
        for(int i = 0; i < CanvasController.I.playerSelectScreen.panelArray.Length; i++) {
            PlayerSelectPanel panel = CanvasController.I.playerSelectScreen.GetPanelWithCPULast()[i];
            InitializeOrb(
                i + 1,
                orbTempArray.FirstOrDefault(orb => orb.element == panel.element),
                panel.GetInputType(),
                panel.IsCPU() ? 0 : GetCPULevelPerType(panel.type)
            );
        }
        nonNullOrbArray = CreateNonNullOrbArray();
    }

    void InitializeOrbArrayInMarathon(Orb firstPlayerOrb) {
        orbArray = new Orb[spawnPointTransformArray.Length];
        InitializeOrb(1, firstPlayerOrb, InputType.Click);
        int orbIndex = 2;
        foreach (var orb in FindObjectsOfType<Orb>()) {
            if (orb.Initialized)
                continue;
            InitializeOrb(orbIndex++, orb, InputType.CPU, (modeData as MarathonData).level);
        }
        nonNullOrbArray = CreateNonNullOrbArray();
    }

    Orb[] CreateNonNullOrbArray() => orbArray.Where(player => player != null).ToArray(); //remove

    int GetCPULevelPerType(PlayerType playerType) {
        switch (playerType) {
            case PlayerType.CPUEasy:    return 1;
            case PlayerType.CPUNormal:  return 3;
            case PlayerType.CPUHard:    return 6;
        }
        Debug.LogError($"Invalid type {playerType}!");
        return 0;
    }

    void InitializeOrb(int number, Orb orb, InputType inputType, int aiLevel = 0) {
        orbArray[number] = orb;
        orb.Initialize(number, inputType, aiLevel);
    }

    public void OnCameraInitialAnimationEnd() {
        state = GameState.SelectPlayer;
        switch (modeData) {
            case MarathonData m:
                if (m.element == Element.None)
                    CanvasController.I.startText.gameObject.SetActive(true);
                else
                    StartMarathon(FindObjectsOfType<Orb>().First(p => p.element == m.element));
                break;
            case QuickRaceData qr:
                CanvasController.I.playerSelectScreen.gameObject.SetActive(true);
                break;
        }
    }

    public void StartMarathon(Orb selectedOrb = null) {
        InitializeOrbArrayInMarathon(selectedOrb);
        StartGame();
    }

    public void StartQuickRace() {
        InitializeOrbArrayInQuickRace();
        StartGame();
    }

    void StartGame() {
        //TODO put on Canvas
        CanvasController.I.playerSelectScreen.gameObject.SetActive(false);
        CanvasController.I.startText.gameObject.SetActive(false);
        CanvasController.I.hudRectTransform.gameObject.SetActive(true);
        CanvasController.I.cardZone.gameObject.SetActive(true);

        cameraController.OnGameStart();
        Time.timeScale = 1;
        startSFX.Play();
        state = GameState.Ocurring;
        DrawCardLoop();
    }

    async void DrawCardLoop() {
        await new WaitForUpdate();
        while (state == GameState.Ocurring) { //TODO check each player individually
            CanvasController.I.cardZone.Add(cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)]);
            foreach (var orb in orbArray)
                if(orb!=null && orb.ai != null && !orb.reachGoal)
                    orb.ai.cardTypeDeck.Add(EnumUtil.GetRandomValueFromEnum<CardType>(1, -4));
            await new WaitForSeconds(5);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    public void OnReachGoal(Orb orb) {
        orb.reachGoal = true;
        if (state == GameState.Ocurring)
            EndGame(orb);
    }

    async void EndGame(Orb winnerOrb) {
        EndGameState(winnerOrb);
        endSFX.Play();
        await new WaitForSecondsRealtime(1.2f);
        CanvasController.I.victoryText.gameObject.SetActive(true); //TODO
        CanvasController.I.victoryText.text = $"Player {winnerOrb.m_name} won!";
        await new WaitForSecondsRealtime(2.5f);
        await new WaitMultiple(this, 1, new WaitForSecondsRealtime(4f), new WaitUntil(() => OrbReachGoalCount >= OrbCount - 1));
        await new WaitForSecondsRealtime(0.5f);
        FindObjectOfType<Fader>().FadeOut(() => GoToNextScene(winnerOrb));
    }

    void EndGameState(Orb winnerOrb) {
        endTime = CurrentTime;
        state = GameState.End;
        Debug.Log($"Player {winnerOrb.number} won in {CurrentTime}s!");
    }

    void GoToNextScene(Orb winnerOrb) {
        if (!winnerOrb.IsCPU && modeData is MarathonData) 
            GoToNextRace();
        else
            BackToMainMenu(true);
    }

    void GoToNextRace() {
        (modeData as MarathonData).level++;
        SceneManager.LoadScene("Game");
    }

    void BackToMainMenu(bool gameLost) {
        if (gameLost && modeData is MarathonData)
            SaveLastScore();
        SceneManager.LoadScene("MainMenu");
    }

    void SaveLastScore() {
        SimpleScoreListTimedDrawer.lastScore = (modeData as MarathonData).level;
        ScoreListTimed scoreList = new ScoreListTimed();
        scoreList.Load();
        scoreList.AddScore((int)SimpleScoreListTimedDrawer.lastScore);
        scoreList.Save();
    }

    //TODO CardHandlers
    public void ExecuteCardEffect(Orb orb, Card card, CardType cardType) {
        Orb selectedOrb = null;
        switch (cardType) {
            case CardType.Neo:
                Debug.Log("Activated=" + cardType);
                orbArray[1].Boost();
                if (card != null)
                    card.Remove();
                break;
            case CardType.Fire:
                selectedOrb = orbArray.First((p) => p!=null && p.element == Element.Fire);
                const float radius = 7f;
                foreach(var item in Physics.OverlapSphere(selectedOrb.transform.position, radius)) {
                    Orb p = item.GetComponentInParent<Orb>();
                    if(p!= null && p != selectedOrb) {
                        p.rigidBody.AddExplosionForce(700, selectedOrb.transform.position, radius);
                    }
                }
                explodeSFX.Play();
                Destroy(Instantiate(explosionPrefab, selectedOrb.transform.position, selectedOrb.transform.rotation), 8f);
                if (card != null)
                    card.Remove();
                break;
            case CardType.Earth:
                selectedOrb = orbArray.First((p) => p!=null && p.element == Element.Earth);
                if(selectedOrb.currentSegment != null) {
                    selectedOrb.currentSegment.ApplyEffect(CardType.Earthquake);
                    if (card != null)
                        card.Remove();
                    earthquakeSFX.Play();
                }
                break;
            case CardType.Water:
                selectedOrb = orbArray.First((p) => p != null && p.element == Element.Water);
                if (selectedOrb.currentSegment != null) {
                    selectedOrb.currentSegment.ApplyEffect(CardType.Lake);
                    if (card != null)
                        card.Remove();
                    squidSFX.Play();
                }
                break;
            case CardType.Air:
                selectedOrb = orbArray.First((p) => p != null && p.element == Element.Air);
                if (selectedOrb.currentSegment != null) {
                    selectedOrb.currentSegment.ApplyEffect(CardType.Tornado);
                    if (card != null)
                        card.Remove();
                    cycloneSFX.Play();
                }
                break;
                /* //remove
                if (card != null)
                    card.Highlight();
                else
                    AIApplyOnNearSegment(player, cardType);
                break;
                */
            default:
                Debug.Log("Activated=" + cardType);
                break;
        }
    }

    void AIApplyOnNearSegment(Orb orb, CardType cardType) {
        for (int i = 0; i < 10; i++) { //TODO count
            var segmentIndex = segmentList.IndexOf(orb.currentSegment);
            var nearIndexArray = new[] { segmentIndex - 1, segmentIndex, segmentIndex + 1 };
            int randomIndex = nearIndexArray[Mathf.FloorToInt(Random.value * nearIndexArray.Length)];
            //TODO break method
            if (0<=randomIndex && randomIndex<segmentList.Count && segmentList[randomIndex].cardType != cardType) {
                segmentList[randomIndex].ApplyEffect(cardType);
                return;
            }
        }
        // Try any segment
        for (int i = 0; i < 40; i++) {
            int randomIndex = Mathf.FloorToInt(Random.value * segmentList.Count);
            if (segmentList[randomIndex].cardType != cardType) {
                segmentList[randomIndex].ApplyEffect(cardType);
                return;
            }
        }
        Debug.LogWarning("Can't apply effect on valid segment!");
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
}
