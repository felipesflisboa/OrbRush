using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Create Fadein and Fadeout transitions.
/// During fade is occurring, enable an image who may have an collider. Soit may block raycasts if necessary
/// Version 1.1
/// </summary>
[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour {
    public bool fadeInOnStart = true;
    public float inDuration = 0.5f;
    public Ease inEase = Ease.Linear;
    public float outDuration = 0.5f;
    public Ease outEase = Ease.Linear;
    Image image;

    public bool Faded {
        get {
            return image.color == Color.black;
        }
    }

    public bool InProgress {
        get {
            return ImageActive && !Faded;
        }
    }

    public bool ImageActive {
        get {
            return image.enabled;
        }
    }

    void Awake() {
        image = GetComponent<Image>();
        SetMask(fadeInOnStart ? Color.black : Color.clear);
    }
    
    void Start () {
        if (fadeInOnStart)
            FadeIn();
    }

    public void SetMaskAsBlack() {
        SetMask(Color.black);
    }

    public void SetMaskAsClear() {
        SetMask(Color.clear);
    }

    void SetMask(Color color) {
        DOTween.Kill(image);
        image.enabled = color!=Color.clear;
        image.color = color;
    }
	
    public Tweener FadeIn(Action callback = null) {
        SetMaskAsBlack();
        return image.DOColor(Color.clear, inDuration).SetUpdate(true).SetEase(inEase).OnComplete(() => {
            image.enabled = false;
            if (callback != null)
                callback();
        });
    }

    public Tweener FadeOut(Action callback = null) {
        SetMaskAsClear();
        image.enabled = true;
        return image.DOColor(Color.black, outDuration).SetUpdate(true).SetEase(outEase).OnComplete(() => {
            if (callback != null)
                callback();
        });
    }
}