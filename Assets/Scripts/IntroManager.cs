using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : SingletonMonoBehaviour<IntroManager>{
    IEnumerator Start(){
        SceneLoader.levelName = "MainMenu"; 
        var fader = FindObjectOfType<Fader>();
        fader.FadeIn();
        yield return new WaitWhile(() => fader.InProgress);
        var loader = FindObjectOfType<SceneLoader>();
        loader.Activate();
        yield return new WaitUntil(() => loader.loadingDone);
        fader.FadeOut();
        yield return null;
        yield return new WaitWhile(() => fader.InProgress);
        loader.AllowSceneActivation();
    }
}
