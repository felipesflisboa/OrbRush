using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Gamelogic.Extensions;

public class Card : MonoBehaviour {
    public CardType type;
    [SerializeField] RectTransform[] highlightRectArray;
    [SerializeField] Button button;
    internal CardZone zone;
    CanvasGroup canvasGroup;
    Tweener movementTween;
    float targetLocalY = 1;
    public bool valid { get; private set; } = true;

    public RectTransform RectTransform => transform as RectTransform;

    const float ANIMATION_DURATION = 0.8f;

    void Awake() {
        button.onClick.AddListener(OnClick);
        canvasGroup = GetComponent<CanvasGroup>();
        Unhighlight();
    }

    void Start() {
        PlayInitialAnimation();
    }

    void PlayInitialAnimation() {
        DOTween.Kill(movementTween);
        transform.localPosition = transform.localPosition.WithY(-1500);
        movementTween = RectTransform.DOLocalMoveY(targetLocalY, ANIMATION_DURATION).SetEase(Ease.OutCubic);
    }

    public void Highlight() {
        foreach (var rectTransform in highlightRectArray)
            rectTransform.gameObject.SetActive(true);
    }

    public void Unhighlight() {
        foreach (var rectTransform in highlightRectArray)
            rectTransform.gameObject.SetActive(false);
    }

    public void Remove() {
        valid = false;
        PlayRemoveAnimation(0.2f, () => Destroy(gameObject));
    }

    public void PlayRemoveAnimation(float time, System.Action callback) {
        canvasGroup.DOFade(0f, time).SetEase(Ease.InSine).OnComplete(() => callback());
        transform.DOScale(transform.localScale.x*1.2f, time).SetEase(Ease.OutSine);
    }

    public void RefreshPosition(float localY) {
        if (Mathf.Approximately(targetLocalY, localY))
            return;
        targetLocalY = localY;
        if (movementTween != null)
            DOTween.Kill(movementTween);
        movementTween = RectTransform.DOLocalMoveY(targetLocalY, ANIMATION_DURATION).SetEase(Ease.InOutSine);
    }

    void OnClick() {
        if(zone.InputHandler.CanClick)
            TryToUse();
    }

    public void TryToUse() {
        if (GameManager.I.Paused || !valid)
            return;
        GameManager.I.cardHandler.ExecuteCardEffect(this, type, false);
    }
}
