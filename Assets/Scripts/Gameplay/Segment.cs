using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {
    List<Player> playerInsideList = new List<Player>();
    internal CardType cardType;

    void OnMouseDown() {
        Debug.Log($"Mousedown! {name}");
        if (GameManager.I.selectedCard != null && cardType == CardType.None) {
            ApplyEffect(GameManager.I.selectedCard.type);
            Destroy(GameManager.I.selectedCard.gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Player>();
        if (player != null) {
            playerInsideList.Add(player);
            player.currentSegment = this;
            ApplyEffectInPlayer(player);
        }
    }

    void ApplyColor(Color color) {
        Colorizer colorizer = new Colorizer(transform);
        colorizer.colorToTint = color;
        colorizer.setInChildren = true;
        colorizer.Tint();
    }

    public void ApplyEffect(CardType newCardType) {
        Debug.Log("Activated=" + newCardType);
        cardType = newCardType;
        foreach (var player in playerInsideList) {
            ApplyEffectInPlayer(player);
        }
        switch (cardType) {
            case CardType.Fire:
                ApplyColor(Color.red);
                break;
        }
    }

    public void ApplyEffectInPlayer(Player player) {
        switch (cardType) {
            case CardType.Fire:
                if(player.element == Element.Fire)
                    player.rigidBody.velocity = player.rigidBody.velocity * 1.1f;
                if (player.element == Element.Air)
                    player.rigidBody.velocity = player.rigidBody.velocity * 0.8f;
                break;
        }
    }

    void OnTriggerExit(Collider other) {
        var player = other.GetComponentInParent<Player>();
        if (player != null) {
            if(playerInsideList.Contains(player))
                playerInsideList.Remove(player);
            if (player.currentSegment == this)
                player.currentSegment = null;
        }
    }
}
