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

    string ADSID {
        get {
#if UNITY_ADS && UNITY_ANDROID
            return "4022189";
#endif
            Debug.LogError("Ads not supported on this platform!");
            return null;
        }
    }

    void Awake() {
        musicController = FindObjectOfType<MusicController>();
        fader = FindObjectOfType<Fader>(); ;
        videoPlayer = FindObjectOfType<VideoPlayer>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        GameManager.adsCaller = ADSCallerFactory.Create(ADSID);
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
