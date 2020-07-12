using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Segment : MonoBehaviour {
    [SerializeField] Transform[] earthquakePlatformArray = new Transform[0];
    [SerializeField] Transform tornadoDiskTransform;
    List<Player> playerInsideList = new List<Player>();
    internal CardType cardType;
    bool onEarthquake;
    bool onTornado; //TODO remove

    void OnMouseDown() {
        Debug.Log($"Mousedown! {name}");
        if (GameManager.I.selectedCard != null) {
            ApplyEffect(GameManager.I.selectedCard.type);
            Destroy(GameManager.I.selectedCard.gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Player>();
        if (player != null) {
            playerInsideList.Add(player);
            if(player.currentSegment != null) //TODO property
                player.lastSegment = player.currentSegment;
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
            case CardType.Water:
                ApplyColor(Color.blue);
                break;
            case CardType.Earth:
                ApplyColor(Color.green);
                break;
            case CardType.Air:
                ApplyColor(Color.yellow);
                break;
            case CardType.Lake:
                ApplyColor(Color.blue);
                break;
        }
    }

    public void ApplyEffectInPlayer(Player player) {
        switch (cardType) {
            case CardType.Fire:
                if(player.element == Element.Fire)
                    player.rigidBody.velocity = player.rigidBody.velocity * 1.2f;
                if (player.element == Element.Air)
                    player.rigidBody.velocity = player.rigidBody.velocity * 0.6f;
                break;
            case CardType.Water:
                if(player.element == Element.Water)
                    player.rigidBody.velocity = player.rigidBody.velocity * 1.2f;
                if (player.element == Element.Fire)
                    player.rigidBody.velocity = player.rigidBody.velocity * 0.6f;
                break;
            case CardType.Air:
                if (player.element == Element.Air)
                    player.rigidBody.velocity = player.rigidBody.velocity * 1.2f;
                if (player.element == Element.Earth)
                    player.rigidBody.velocity = player.rigidBody.velocity * 0.6f;
                break;
            case CardType.Earth:
                if (player.element == Element.Earth)
                    player.rigidBody.velocity = player.rigidBody.velocity * 1.2f;
                if (player.element == Element.Water)
                    player.rigidBody.velocity = player.rigidBody.velocity * 0.6f;
                break;
            case CardType.Lake:
                player.rigidBody.velocity = player.rigidBody.velocity * 0.5f;
                break;
        }
    }

    public void ApplyEarthquake() {
        onEarthquake = true;
        foreach (var item in earthquakePlatformArray) {
            item.gameObject.SetActive(true);
            DOTween.Sequence().Append(
                item.transform.DOMoveY(0.3f, 0.3f).SetRelative()
            ).Append(
                item.transform.DOMoveY(-0.3f, 0.001f).SetRelative()
            ).SetLoops(-1);
        }
    }
    
    //TODO cyclone
    public void ApplyTornado() {
        onTornado = true;
        if (tornadoDiskTransform == null)
            return;
        tornadoDiskTransform.gameObject.SetActive(true);
        tornadoDiskTransform.DOLocalRotate(Vector3.up*90, 0.25f).SetRelative().SetLoops(-1);
    }

    void OnTriggerExit(Collider other) {
        var player = other.GetComponentInParent<Player>();
        if (player != null) {
            if(playerInsideList.Contains(player))
                playerInsideList.Remove(player);
            if (player.currentSegment == this) {
                player.lastSegment = player.currentSegment;
                player.currentSegment = null;
            }
        }
    }
}
