using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Gamelogic.Extensions;

public class Card : MonoBehaviour {
    public CardType type;
    public Image image;
    Tweener movementTween;
    float targetLocalY = 1;
    public bool valid { get; private set; } = true;

    public RectTransform RectTransform => transform as RectTransform;

    const float ANIMATION_DURATION = 0.8f;

    void Start() {
        InitialAnimation();
    }

    void InitialAnimation() {
        DOTween.Kill(movementTween);
        transform.localPosition = transform.localPosition.WithY(-1500);
        movementTween = RectTransform.DOLocalMoveY(targetLocalY, ANIMATION_DURATION).SetEase(Ease.OutCubic);
    }

    public void Highlight() {
        if (GameManager.I.selectedCard != null && GameManager.I.selectedCard != this)
            GameManager.I.selectedCard.Unhighlight();
        GameManager.I.selectedCard = this;
        image.color = new Color32(0xFF, 0xF0, 0xA0, 0xFF);
    }

    public void Unhighlight() {
        image.color = Color.white;
        GameManager.I.selectedCard = null;
    }

    public void Remove() {
        valid = false;
        image.DOFade(0, 0.25f).SetEase(Ease.InSine).OnComplete(() => Destroy(gameObject));
    }

    public void RefreshPosition(float localY) {
        if (Mathf.Approximately(targetLocalY, localY))
            return;
        targetLocalY = localY;
        if (movementTween != null)
            DOTween.Kill(movementTween);
        movementTween = RectTransform.DOLocalMoveY(targetLocalY, ANIMATION_DURATION).SetEase(Ease.InOutSine);
    }

    public void OnClick() {
        if (GameManager.I.Paused || !valid)
            return;
        GameManager.I.ExecuteCardEffect(GameManager.I.humanPlayer, this, type);
    }
}
