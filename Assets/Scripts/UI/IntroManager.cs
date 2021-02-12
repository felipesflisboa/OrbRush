using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : SingletonMonoBehaviour<IntroManager>{
    MusicController musicController;

    async void Start(){
        musicController = FindObjectOfType<MusicController>();
        SceneLoader.levelName = "MainMenu"; 
        var fader = FindObjectOfType<Fader>();
        fader.FadeIn();
        await new WaitWhile(() => fader.InProgress);
        var videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
        videoPlayer.Prepare();
        await new WaitWhile(() => !videoPlayer.isPrepared);
        videoPlayer.Play();
        await new WaitWhile(() => videoPlayer.isPlaying);
        var loader = FindObjectOfType<SceneLoader>();
        loader.allowSceneActivation = false;
        loader.Activate();
        await new WaitUntil(() => loader.loadingDone);
        if(musicController != null)
            musicController.Play();
        fader.FadeOut();
        await new WaitForUpdate();
        await new WaitWhile(() => fader.InProgress);
        await new WaitForUpdate();
        loader.allowSceneActivation = true;
    }
}
