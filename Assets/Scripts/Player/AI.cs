using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {
    internal Orb player;
    internal List<CardType> cardTypeDeck = new List<CardType>(); //TODO rename class
    int level;

    float SecondBasePerRound => GetSecondBasePerRound(level);
    float SecondIncPerRound => GetSecondIncPerRound(level);

    public AI(int pLevel, Orb pPlayer) {
        level = pLevel;
        player = pPlayer;
        MainLoop();
    }

    public async void MainLoop() {
        await new WaitForSeconds(Random.Range(0.5f, 4f));
        while (!player.reachGoal) {
            await new WaitWhile(() => cardTypeDeck.Count == 0);
            if(GameManager.Active)
                GameManager.I.cardHandler.ExecuteCardEffect(null, cardTypeDeck[0], true);
            cardTypeDeck.RemoveAt(0);
            await new WaitForSeconds(SecondBasePerRound + SecondIncPerRound * Random.value);
        }
    }

    static float GetSecondBasePerRound(int level) => 18f / (level);
    static float GetSecondIncPerRound(int level) => 24f / (level);

    public static string GetDebugString() {
        string ret = "AI interval between card uses:";
        for (int l = 1; l < 10; l++)
            ret += $"\nLv{l}:\t{GetSecondBasePerRound(l)}-{GetSecondBasePerRound(l) + GetSecondIncPerRound(l)}";
        return ret;
    }
}
