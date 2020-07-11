using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField] GameObject playerPrefab;

    internal bool occuring;

    void Start() {
        StartGame();
    }

    void StartGame() {
        occuring = true;
        for (int i = 0; i < spawnPointTransformArray.Length; i++) {
            Instantiate(playerPrefab, spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation).GetComponent<Player>().number = i+1;
        }
    }

    public void OnReachGoal(Player player) {
        if (!occuring)
            return;
        Debug.Log($"Player {player.number} won!");
        Time.timeScale = 0;
        occuring = false;
    }
}
