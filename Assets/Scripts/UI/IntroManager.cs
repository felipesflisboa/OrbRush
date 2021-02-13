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
        videoPlayer.Prepare();
#if !UNITY_WEBGL
        await new WaitWhile(() => !videoPlayer.isPrepared);
#endif
        videoPlayer.Play();
        await new WaitForSeconds(1); // WebGL won't trigger play, so gives a little time to prepare
        await new WaitWhile(() => videoPlayer.isPlaying);
    }

    async Task PreloadScene() {
        SceneLoader.levelName = "MainMenu";
        sceneLoader.allowSceneActivation = false;
        sceneLoader.Activate();
        await new WaitUntil(() => sceneLoader.loadingDone);
    }
}
