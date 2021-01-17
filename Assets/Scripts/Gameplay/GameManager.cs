﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    public static int level = 1;

    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField, PrefabReference] GameObject[] playerPrefabArray;
    [SerializeField, PrefabReference] GameObject[] cardPrefabArray;
    [SerializeField, PrefabReference] GameObject explosionPrefab;

    [SerializeField] AudioSource cycloneSFX;
    [SerializeField] AudioSource squidSFX;
    [SerializeField] AudioSource earthquakeSFX;
    [SerializeField] AudioSource explodeSFX;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource endSFX;

    internal MusicController musicController;
    internal Player[] playerArray; //TODO protect
    internal Player[] nonNullPlayerArray;
    internal Player humanPlayer;
    internal GameState state;
    internal Card selectedCard;
    internal List<Segment> segmentList;
    float endTime;

    int PlayerReachGoalCount => nonNullPlayerArray.Sum(player => player.reachGoal ? 1 : 0);
    int PlayerCount => spawnPointTransformArray.Length;
    public float CurrentTime => GameState.Ocurring==state ? Time.timeSinceLevelLoad : endTime;
    public bool Paused => Time.timeScale == 0 && state != GameState.BeforeStart;

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

    void Start() {
        musicController = FindObjectOfType<MusicController>();

        Time.timeScale = 0;
        segmentList = FindObjectsOfType<Segment>().ToList();
        segmentList.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        // zoneList.Sort((a, b) => Mathf.Approximately(a.transform.position.z, b.transform.position.z) ? a.transform.position.x.CompareTo(b.transform.position.x) : a.transform.position.z.CompareTo(b.transform.position.z));

        CreatePlayers();
    }

    void CreatePlayers() {
        for (int i = 1; i < spawnPointTransformArray.Length; i++)
            Instantiate(playerPrefabArray[i], spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation);
    }

    void InitializePlayerArray(Player firstPlayer) {
        playerArray = new Player[spawnPointTransformArray.Length];
        playerArray[1] = firstPlayer;
        humanPlayer = firstPlayer;
        int playerIndex = 2;
        foreach (var player in FindObjectsOfType<Player>()) {
            if (player == humanPlayer)
                continue;
            InitializePlayer(player, playerIndex++);
        }
        nonNullPlayerArray = playerArray.Where(player => player != null).ToArray();
    }

    void InitializePlayer(Player player, int number) {
        playerArray[number] = player;
        player.number = number;
        new AI(player);
        player.ai.MainLoop();
    }

    
    /* //remove
    void InitializePlayers(Player firstPlayer) {
        Player[] newPlayerArray = new Player[playerArray.Length];
        newPlayerArray[1] = humanPlayer;
        int newI = 2;
        for (int oldI = 1; oldI < playerArray.Length; oldI++) {
            if (newPlayerArray[1] == playerArray[oldI])
                continue;
            newPlayerArray[newI++] = 

        }
        aiArray = new AI[3];
        int playerI = 1;
        for (int aiI = 0; aiI < aiArray.Length; aiI++) {
            if (humanPlayer == playerArray[playerI])
                playerI++;
            aiArray[aiI] = new AI(playerArray[playerI]);
            aiArray[aiI].MainLoop();
            playerI++;
        }
        nonNullPlayerArray = playerArray.Where(player => player != null).ToArray();
    }
    */

    public void StartGame(Player firstPlayer) {
        CanvasController.I.startText.gameObject.SetActive(false);
        Time.timeScale = 1;
        InitializePlayerArray(firstPlayer);
        startSFX.Play();
        state = GameState.Ocurring;
        DrawCardLoop();
    }

    async void DrawCardLoop() {
        await new WaitForUpdate();
        while (state == GameState.Ocurring) { //TODO check each player individually
            CanvasController.I.cardZone.Add(cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)]);
            foreach (var player in playerArray)
                if(player!=null && player.ai != null && !player.reachGoal)
                    player.ai.cardTypeDeck.Add(EnumUtil.GetRandomValueFromEnum<CardType>(1, -4));
            await new WaitForSeconds(5);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    public void OnReachGoal(Player player) {
        player.reachGoal = true;
        if (state == GameState.Ocurring)
            EndGame(player);
    }

    async void EndGame(Player winnerPlayer) {
        EndGameState(winnerPlayer);
        endSFX.Play();
        await new WaitForSecondsRealtime(1.2f);
        CanvasController.I.victoryText.gameObject.SetActive(true); //TODO
        CanvasController.I.victoryText.text = $"Player {winnerPlayer.m_name} won!";
        await new WaitForSecondsRealtime(2.5f);
        await new WaitMultiple(this, 1, new WaitForSecondsRealtime(4f), new WaitUntil(() => PlayerReachGoalCount >= PlayerCount - 1));
        await new WaitForSecondsRealtime(0.5f);
        FindObjectOfType<Fader>().FadeOut(() => GoToNextScene(winnerPlayer));
    }

    void EndGameState(Player winnerPlayer) {
        endTime = CurrentTime;
        state = GameState.End;
        Debug.Log($"Player {winnerPlayer.number} won in {CurrentTime}s!");
    }

    void GoToNextScene(Player winnerPlayer) {
        if (winnerPlayer == humanPlayer) {
            level++;
            SceneManager.LoadScene("Game");
        } else {
            SaveLastScore();
            SceneManager.LoadScene("MainMenu");
        }
    }

    void SaveLastScore() {
        SimpleScoreListTimedDrawer.lastScore = level;
        ScoreListTimed scoreList = new ScoreListTimed();
        scoreList.Load();
        scoreList.AddScore((int)SimpleScoreListTimedDrawer.lastScore);
        scoreList.Save();
    }

    //TODO CardHandlers
    public void ExecuteCardEffect(Player player, Card card, CardType cardType) {
        Player selectedPlayer = null;
        switch (cardType) {
            case CardType.Neo:
                Debug.Log("Activated=" + cardType);
                playerArray[1].Boost();
                if (card != null)
                    card.Remove();
                break;
            case CardType.Fire:
                selectedPlayer = playerArray.First((p) => p!=null && p.element == Element.Fire);
                const float radius = 7f;
                foreach(var item in Physics.OverlapSphere(selectedPlayer.transform.position, radius)) {
                    Player p = item.GetComponentInParent<Player>();
                    if(p!= null && p != selectedPlayer) {
                        p.rigidBody.AddExplosionForce(700, selectedPlayer.transform.position, radius);
                    }
                }
                explodeSFX.Play();
                Destroy(Instantiate(explosionPrefab, selectedPlayer.transform.position, selectedPlayer.transform.rotation), 8f);
                if (card != null)
                    card.Remove();
                break;
            case CardType.Earth:
                selectedPlayer = playerArray.First((p) => p!=null && p.element == Element.Earth);
                if(selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Earthquake);
                    if (card != null)
                        card.Remove();
                    earthquakeSFX.Play();
                }
                break;
            case CardType.Water:
                selectedPlayer = playerArray.First((p) => p != null && p.element == Element.Water);
                if (selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Lake);
                    if (card != null)
                        card.Remove();
                    squidSFX.Play();
                }
                break;
            case CardType.Air:
                selectedPlayer = playerArray.First((p) => p != null && p.element == Element.Air);
                if (selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Tornado);
                    if (card != null)
                        card.Remove();
                    cycloneSFX.Play();
                }
                break;
                /*
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

    void AIApplyOnNearSegment(Player player, CardType cardType) {
        for (int i = 0; i < 10; i++) { //TODO count
            var segmentIndex = segmentList.IndexOf(player.currentSegment);
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
