using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;

public class Segment : MonoBehaviour {
    [SerializeField] Transform[] earthquakePlatformArray = new Transform[0];
    [SerializeField] Transform tornadoDiskTransform;
    List<Player> playerInsideList = new List<Player>();
    internal CardType cardType;

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
            case CardType.Tornado:
                ApplyColor(Color.gray);
                break;
            case CardType.Earthquake:
                ApplyColor(new Color32(0, 0xA0, 0, 0));
                foreach (var item in earthquakePlatformArray) {
                    item.gameObject.SetActive(true);
                    DOTween.Sequence().Append(
                        item.transform.DOMoveY(0.2f, 0.4f).SetRelative()
                    ).Append(
                        item.transform.DOMoveY(-0.2f, 0.001f).SetRelative()
                    ).SetLoops(-1);
                }
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
                player.rigidBody.velocity = player.rigidBody.velocity * 1.4f;
                break;
            case CardType.Earthquake:
                //player.rigidBody.velocity = player.rigidBody.velocity * 1.6f;
                break;
        }
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

    void FixedUpdate() {
        if (cardType == CardType.Tornado) {
            foreach (var player in playerInsideList) {
                if (player.element == Element.Air)
                    continue;
                player.rigidBody.AddForce(MathUtil.UnityAngleToNormal(
                    MathUtil.GetAngle(transform.position.To2DXZ() - player.transform.position.To2DXZ()) - 90
                )* 350 * Time.deltaTime, ForceMode.Acceleration);
            }
        }
    }
}
