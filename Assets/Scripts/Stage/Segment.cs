using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;

public class Segment : MonoBehaviour {
    public Transform[] earthquakePlatformArray = new Transform[0];
    Earthquake earthquake;
    Squid squid;
    Cyclone cyclone;
    List<Orb> playerInsideList = new List<Orb>();
    float cardEffectEndTime;
    internal CardType cardType;

    const float CARD_EFFECT_DURATION = 10f;

    void Awake() {
        cyclone = GetComponentInChildren<Cyclone>(true);
        earthquake = GetComponentInChildren<Earthquake>(true);
        squid = GetComponentInChildren<Squid>(true);
    }

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

    public void ApplyColor(Color color) {
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
                squid.Activate();
                break;
            case CardType.Tornado:
                DisableCardEffectTransforms();
                cyclone.Activate();
                break;
            case CardType.Earthquake:
                DisableCardEffectTransforms();
                earthquake.Activate();
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
                orb.rigidBody.velocity *=  1f + 0.4f * (orb.element == Element.Water ? 0.75f : 1);
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
        earthquake.Deactivate();
        cyclone.Deactivate();
    }
}
