using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject[] cardPrefabArray;

    internal Player[] playerArray; //TODO protect
    internal bool occuring;

    void Start() {
        StartGame();
    }

    void StartGame() {
        occuring = true;

        playerArray = new Player[spawnPointTransformArray.Length];
        for (int i = 1; i < spawnPointTransformArray.Length; i++) {
            playerArray[i] = Instantiate(playerPrefab, spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation).GetComponent<Player>();
            playerArray[i].number = i;
        }

        StartCoroutine(CardRoutine());
    }

    IEnumerator CardRoutine() {
        while (occuring) {
            var card = Instantiate(
                cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)], CanvasController.I.cardZone.transform
            ).GetComponent<Card>();
            yield return new WaitForSeconds(5);
        }
    }

    public void OnReachGoal(Player player) {
        if (!occuring)
            return;
        Debug.Log($"Player {player.number} won in {Time.timeSinceLevelLoad}s!");
        Time.timeScale = 0;
        occuring = false;
    }
}
