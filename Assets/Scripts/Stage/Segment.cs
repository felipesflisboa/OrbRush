using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;

public class Segment : MonoBehaviour {
    [SerializeField] Transform[] earthquakePlatformArray = new Transform[0];
    [SerializeField] Transform tornadoTransform;
    [SerializeField] Transform earthquakeTransform; //TODO rename
    [SerializeField] Transform squirtTransform; //TODO rename lake
    List<Orb> playerInsideList = new List<Orb>();
    float cardEffectEndTime;
    internal CardType cardType;

    const float CARD_EFFECT_DURATION = 10f;

    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Orb>();
        if (player != null) {
            playerInsideList.Add(player);
            if(player.currentSegment != null) //TODO property
                player.lastSegment = player.currentSegment;
            player.currentSegment = this;
            ApplyEffectInPlayer(player);
        }
    }

    void ApplyColor(Color color) {
        var oldColorizer = GetComponentInChildren<AutoColorizer>();
        if(oldColorizer == null) {
            if (color == Color.white)
                return;
        } else {
            if (oldColorizer.definedColor == color)
                return;
            Destroy(oldColorizer);
        }
        CreateColorizer(color, 1.5f);
    }

    void CreateColorizer(Color color, float time) {
        var newColorizer = gameObject.AddComponent<AutoColorizer>();
        newColorizer.definedColor = color;
        newColorizer.totalTime = time;
    }

    public void ApplyEffect(CardType newCardType) {
        foreach (var player in playerInsideList)
            ApplyEffectInPlayer(player);
        cardType = newCardType;
        cardEffectEndTime = CARD_EFFECT_DURATION + Time.timeSinceLevelLoad;
        switch (newCardType) {
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
                DisableCardEffectTransforms();
                ApplyColor(new Color32(0xD0, 0xD0, 0xFF, 0xFF));
                squirtTransform.gameObject.SetActive(true);
                break;
            case CardType.Tornado:
                DisableCardEffectTransforms();
                tornadoTransform.gameObject.SetActive(true);
                tornadoTransform.position = GameManager.I.GetOrb(Element.Air).transform.position.WithY(tornadoTransform.position.y);
                break;
            case CardType.Earthquake:
                DisableCardEffectTransforms();
                earthquakeTransform.gameObject.SetActive(true);
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

    public void ApplyEffectInPlayer(Orb orb) {
        switch (cardType) {
            case CardType.Fire:
                if(orb.element == Element.Fire)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 1.2f;
                if (orb.element == Element.Air)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 0.6f;
                break;
            case CardType.Water:
                if(orb.element == Element.Water)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 1.2f;
                if (orb.element == Element.Fire)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 0.6f;
                break;
            case CardType.Air:
                if (orb.element == Element.Air)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 1.2f;
                if (orb.element == Element.Earth)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 0.6f;
                break;
            case CardType.Earth:
                if (orb.element == Element.Earth)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 1.2f;
                if (orb.element == Element.Water)
                    orb.rigidBody.velocity = orb.rigidBody.velocity * 0.6f;
                break;
            case CardType.Lake:
                orb.rigidBody.velocity = orb.rigidBody.velocity * 1.4f * (orb.element == Element.Water ? 0.75f : 1);
                break;
            case CardType.Earthquake:
                //player.rigidBody.velocity = player.rigidBody.velocity * 1.6f;
                break;
        }
    }

    void OnTriggerExit(Collider other) {
        var player = other.GetComponentInParent<Orb>();
        if (player != null) {
            if(playerInsideList.Contains(player))
                playerInsideList.Remove(player);
            if (player.currentSegment == this) {
                player.lastSegment = player.currentSegment;
                player.currentSegment = null;
            }
        }
    }

    void Update() {
        if(cardEffectEndTime!= 0 && cardEffectEndTime < Time.timeSinceLevelLoad)
            RemoveCardEffect();
    }

    void RemoveCardEffect() {
        DisableCardEffectTransforms();
        ApplyColor(Color.white);
        cardType = CardType.None;
        cardEffectEndTime = 0;
    }

    void DisableCardEffectTransforms() {
        earthquakeTransform.gameObject.SetActive(false);
        tornadoTransform.gameObject.SetActive(false);
    }
}
