using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : SingletonMonoBehaviour<IntroManager>{
    [SerializeField] AudioSource bgm;

    IEnumerator Start(){
        SceneLoader.levelName = "MainMenu"; 
        var fader = FindObjectOfType<Fader>();
        fader.FadeIn();
        yield return new WaitWhile(() => fader.InProgress);
        var videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
        videoPlayer.Play();
        yield return new WaitForSeconds(1); // For some reason. 1 frame isn't enough
        yield return new WaitWhile(() => videoPlayer.isPlaying);
        var loader = FindObjectOfType<SceneLoader>();
        loader.allowSceneActivation = false;
        loader.Activate();
        yield return new WaitUntil(() => loader.loadingDone);
        bgm.Play();
        fader.FadeOut();
        yield return null;
        yield return new WaitWhile(() => fader.InProgress);
        yield return null;
        loader.allowSceneActivation = true;
    }
}
