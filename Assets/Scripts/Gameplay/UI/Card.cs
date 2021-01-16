using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Gamelogic.Extensions;

public class Card : MonoBehaviour {
    public CardType type;
    public Image image;
    public bool valid { get; private set; } = true;

    void Awake() {
        image.transform.localPosition = image.transform.localPosition.WithY(-1500);
        image.transform.DOLocalMoveY(0, 0.8f).SetEase(Ease.OutCubic);
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

    public void OnClick() {
        if (GameManager.I.Paused || !valid)
            return;
        GameManager.I.ExecuteCardEffect(GameManager.I.humanPlayer, this, type);
    }
}
