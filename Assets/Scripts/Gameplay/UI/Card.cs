using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{
    public CardType type;

    public void OnClick() {
        Debug.Log("OnClick");
        GameManager.I.ExecuteCardEffect(GameManager.I.currentPlayer, this, type);
    }
}
