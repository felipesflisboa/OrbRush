using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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
        Debug.Log("Time="+videoPlayer.time); //remove
        if(!VideoPlayed)
            await ShowImageLogo();
    }

    /* //remove
    async Task ShowLogo() {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos", "LudinoLogo.mp4");
        videoPlayer.Prepare();
        await new WaitWhile(() => !videoPlayer.isPrepared);
        try {
            if (disableVideoOnDebug && Debug.isDebugBuild)
                throw new System.Exception("Debug block video play");
            videoPlayer.Play();
        } catch (System.Exception e) {
            await ShowImageLogo();
            return;
        }
        Debug.Log("Time=" + videoPlayer.time);
        await new WaitWhile(() => videoPlayer.isPlaying);
    }
    */

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

    public static void PrintProperties(Object o) { //remove
        System.Type myObjectType = o.GetType();
        foreach (var thisVar in o.GetType().GetProperties()) {
            try {
                Debug.Log("o:  " + o.name + "        Var Name:  " + thisVar.Name + "         Type:  " + thisVar.PropertyType + "       Value:  " + thisVar.GetValue(o, null) + "\n");
            } catch (System.Exception e) {
                Debug.LogError(e);
            }
        }
    }
}
