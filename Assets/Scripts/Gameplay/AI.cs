using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {
    internal Player player;
    internal List<CardType> cardTypeDeck = new List<CardType>(); //TODO rename class

    static bool debugPrint = false;

    public AI(Player pPlayer) {
        player = pPlayer;
        player.ai = this;
        if (!debugPrint) {
            string s = "";
            for (int l = 1; l < 10; l++) { 
                s += $"Level {l} min {GetSecondBasePerRound(l)} max {GetSecondBasePerRound(l)+ GetSecondIncPerRound(l)}\n";
            }
            Debug.Log(s);
            debugPrint = true;
        }
    }

    public void StartRoutine(MonoBehaviour routinePlayer) {
        routinePlayer.StartCoroutine(MainRoutine());
    }

    IEnumerator MainRoutine() {
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        while (GameManager.I.occuring) {
            yield return new WaitWhile(() => cardTypeDeck.Count == 0);
            GameManager.I.ExecuteCardEffect(player, null, cardTypeDeck[0]);
            cardTypeDeck.RemoveAt(0);
            yield return new WaitForSeconds(GetSecondBasePerRound(1) + GetSecondIncPerRound(GameManager.level)*Random.value);
        }
    }

    float GetSecondBasePerRound(int level) => 24f / (level + 1);
    float GetSecondIncPerRound(int level) => 36f / (level + 1);
}
