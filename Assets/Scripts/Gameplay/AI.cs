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
            for (int r = 1; r < 10; r++) { 
                s += $"Round {r} min {GetSecondBasePerRound(r)} max {GetSecondBasePerRound(r)+ GetSecondIncPerRound(r)}\n";
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
            yield return new WaitForSeconds(GetSecondBasePerRound(1) + GetSecondIncPerRound(1)*Random.value);
        }
    }

    float GetSecondBasePerRound(int round) => 24 / (round + 1);
    float GetSecondIncPerRound(int round) => 36 / (round + 1);
}
