using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarathonData : ModeData {
    public int level = 1;
    public Element element;

    public void SaveLastScore() {
        ScoreListMarathonDrawer.lastScore = level;
        ScoreListMarathonDrawer.lastElement = element;
        ScoreListMarathon scoreList = new ScoreListMarathon();
        scoreList.Load();
        scoreList.AddScore(level, element);
        scoreList.Save();
    }
}
