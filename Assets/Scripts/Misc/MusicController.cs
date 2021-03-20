using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)) ]
public class MusicController : MonoBehaviour{
    AudioSource bgm;

    public bool IsPlaying => bgm.isPlaying;

    void Awake(){
        bgm = GetComponent<AudioSource>();
    }

    public void Play() => bgm.Play();
    public void Pause() => bgm.Pause();
}