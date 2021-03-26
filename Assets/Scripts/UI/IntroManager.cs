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
    [SerializeField] Transform logoTransform;
    [SerializeField] bool disableVideoOnDebug;

    bool VideoPlayed => videoPlayer.time >= 0.1f;

    void Awake() {
        musicController = FindObjectOfType<MusicController>();
        fader = FindObjectOfType<Fader>(); ;
        videoPlayer = FindObjectOfType<VideoPlayer>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        logoTransform.gameObject.SetActive(false);
        AdvertisementHandler.I.Initialize();
    }

    async void Start() {
        fader.SetMaskAsClear();
        await ShowLogo();
        await PreloadScene();
        if (musicController != null)
            musicController.Play();
        await fader.FadeOut().WaitForCompletion();
        sceneLoader.allowSceneActivation = true;
    }

    async Task ShowLogo() {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos" , "LudinoLogo.mp4");
        videoPlayer.Prepare();
        await new WaitWhile(() => !videoPlayer.isPrepared);
        if (!disableVideoOnDebug || !Debug.isDebugBuild)
            videoPlayer.Play();
        await new WaitWhile(() => videoPlayer.isPlaying);
        if(!VideoPlayed)
            await ShowImageLogo();
    }

    async Task ShowImageLogo() {
        fader.SetMaskAsBlack();
        logoTransform.gameObject.SetActive(true);
        await fader.FadeIn().WaitForCompletion();
        await new WaitForSeconds(2f);
    }

    async Task PreloadScene() {
        SceneLoader.levelName = "MainMenu";
        sceneLoader.allowSceneActivation = false;
        sceneLoader.Activate();
        await new WaitUntil(() => sceneLoader.loadingDone);
    }
}
