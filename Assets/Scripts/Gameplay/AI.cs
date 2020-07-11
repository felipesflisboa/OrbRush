using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI{
    internal Player player;
    internal List<CardType> cardTypeDeck = new List<CardType>(); //TODO rename class

    public AI(Player pPlayer) {
        player = pPlayer;
    }

    public void StartRoutine(MonoBehaviour routinePlayer) {
        routinePlayer.StartCoroutine(MainRoutine());
    }

    IEnumerator MainRoutine() {
        yield return new WaitForSeconds(1);
        while (GameManager.I.occuring) {
            yield return new WaitWhile(() => cardTypeDeck.Count == 0);
            GameManager.I.ExecuteCardEffect(player, null, cardTypeDeck[0]);
            cardTypeDeck.RemoveAt(0);
            yield return new WaitForSeconds(Random.Range(8,20));
        }
    }
}
