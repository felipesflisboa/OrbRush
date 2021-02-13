using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class IntroManager : SingletonMonoBehaviour<IntroManager>{
    MusicController musicController;
    SceneLoader sceneLoader;
    VideoPlayer videoPlayer;
    Fader fader;

    void Awake() {
        musicController = FindObjectOfType<MusicController>();
        fader = FindObjectOfType<Fader>(); ;
        videoPlayer = FindObjectOfType<VideoPlayer>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    async void Start() {
        await fader.FadeIn().WaitForCompletion();
        await PlayVideo();
        await PreloadScene();
        if (musicController != null)
            musicController.Play();
        await fader.FadeOut().WaitForCompletion();
        sceneLoader.allowSceneActivation = true;
    }

    async Task PlayVideo() {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos" , "LudinoLogo.mp4");
        videoPlayer.Prepare();
        await new WaitWhile(() => !videoPlayer.isPrepared);
        videoPlayer.Play();
        await new WaitWhile(() => videoPlayer.isPlaying);
    }

    async Task PreloadScene() {
        SceneLoader.levelName = "MainMenu";
        sceneLoader.allowSceneActivation = false;
        sceneLoader.Activate();
        await new WaitUntil(() => sceneLoader.loadingDone);
    }
}
