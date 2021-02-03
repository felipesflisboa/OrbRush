using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {
    internal Orb player;
    internal List<CardType> cardTypeDeck = new List<CardType>(); //TODO rename class
    int level;

    static bool showedDebug = false;

    public AI(int pLevel, Orb pPlayer) {
        level = pLevel;
        player = pPlayer;
        if (!showedDebug)
            DebugPrint();
        MainLoop();
    }

    public void DebugPrint() {
        string s = "";
        for (int l = 1; l < 10; l++)
            s += $"AI level {l}: {GetSecondBasePerRound(l)}-{GetSecondBasePerRound(l) + GetSecondIncPerRound(l)} seconds per card use \n";
        Debug.Log(s);
        showedDebug = true;
    }

    public async void MainLoop() {
        await new WaitForSeconds(Random.Range(0.5f, 4f));
        while (!player.reachGoal) {
            await new WaitWhile(() => cardTypeDeck.Count == 0);
            if(GameManager.Active)
                GameManager.I.ExecuteCardEffect(null, cardTypeDeck[0]);
            cardTypeDeck.RemoveAt(0);
            await new WaitForSeconds(GetSecondBasePerRound(level) + GetSecondIncPerRound(level) *Random.value);
        }
    }

    float GetSecondBasePerRound(int level) => 18f / (level);
    float GetSecondIncPerRound(int level) => 24f / (level);
}
