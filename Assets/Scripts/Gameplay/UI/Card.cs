using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{
    public CardType type;

    public void OnClick() {
        Debug.Log("OnClick");
        switch (type) {
            case CardType.Air:
                GameManager.I.playerArray[1].Boost();
                Destroy(gameObject);
                break;
            case CardType.Fire:
                GameManager.I.selectedCard = this;
                break;
        }
    }
}
