using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour{
    public CardType type;
    public Image image;

    public void Highlight() {
        if (GameManager.I.selectedCard != null && GameManager.I.selectedCard != this)
            GameManager.I.selectedCard.Unhighlight();
        GameManager.I.selectedCard = this;
        image.color = new Color32(0xFF, 0xC0, 0xC0, 0xFF);
    }

    public void Unhighlight() {
        image.color = Color.white;
        GameManager.I.selectedCard = null;
    }

    public void OnClick() {
        Debug.Log("OnClick");
        GameManager.I.ExecuteCardEffect(GameManager.I.humanPlayer, this, type);
    }
}
