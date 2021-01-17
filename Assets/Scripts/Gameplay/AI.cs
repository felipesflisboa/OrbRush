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

    public async void MainLoop() {
        await new WaitForSeconds(Random.Range(0.5f, 4f));
        while (!player.reachGoal) {
            await new WaitWhile(() => cardTypeDeck.Count == 0);
            if(GameManager.Active)
                GameManager.I.ExecuteCardEffect(player, null, cardTypeDeck[0]);
            cardTypeDeck.RemoveAt(0);
            await new WaitForSeconds(GetSecondBasePerRound(GameManager.level) + GetSecondIncPerRound(GameManager.level)*Random.value);
        }
    }

    float GetSecondBasePerRound(int level) => 18f / (level);
    float GetSecondIncPerRound(int level) => 24f / (level);
}
