using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class TextAnimationHandler {
    [SerializeField] AnimationCurve showCurve;
    [SerializeField] AnimationCurve hideCurve;

    const string TEXT_COLOR_KEY = "_FaceColor";
    const string TEXT_OUTLINE_COLOR_KEY = "_OutlineColor";
    const string TEXT_DILATE_KEY = "_FaceDilate";
    const float INITIAL_DILATE = -1f;
    const float FADE_DURATION_RATIO = 0.2f;

    public void ShowWithDilateAnimation(TextMeshProUGUI text, float duration) {
        DOTween.Kill(text);
        if (!text.gameObject.activeSelf)
            text.gameObject.SetActive(true);
        SetColorAlpha(text, 0);
        SetColorAlpha(text, 1, FADE_DURATION_RATIO * duration);
        text.fontMaterial.SetFloat(TEXT_DILATE_KEY, INITIAL_DILATE);
        text.fontMaterial.DOFloat(0, TEXT_DILATE_KEY, duration).SetUpdate(true).SetEase(showCurve);
    }

    public void HideWithDilateAnimation(TextMeshProUGUI text, float duration, Action callback) {
        DOTween.Kill(text);
        SetColorAlpha(text, 0, FADE_DURATION_RATIO * duration, (1-FADE_DURATION_RATIO)*duration);
        text.fontMaterial.DOFloat(INITIAL_DILATE, TEXT_DILATE_KEY, duration).SetUpdate(true).SetEase(hideCurve).OnComplete(() => callback());
    }

    void SetColorAlpha(TextMeshProUGUI text, float alpha, float duration = 0, float delay = 0) {
        SetColorAlpha(text, TEXT_COLOR_KEY, alpha, duration, delay);
        SetColorAlpha(text, TEXT_OUTLINE_COLOR_KEY, alpha, duration, delay);
    }

    void SetColorAlpha(TextMeshProUGUI text, string key, float alpha, float duration = 0, float delay=0) {
        SetColor(text, key, ColorWithAlpha(text.fontMaterial.GetColor(key), alpha), duration, delay);
    }

    void SetColor(TextMeshProUGUI text, string key, Color color, float duration, float delay) {
        if (duration == 0)
            text.fontMaterial.SetColor(key, color);
        else
            text.fontMaterial.DOColor(color, key, duration).SetUpdate(true).SetDelay(delay);
    }

    Color ColorWithAlpha(Color color, float alpha) {
        color.a = alpha;
        return color;
    }
}
