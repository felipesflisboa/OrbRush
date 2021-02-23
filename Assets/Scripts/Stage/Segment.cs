using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;
using RotaryHeart.Lib.SerializableDictionary;

public class Segment : MonoBehaviour {
    [System.Serializable] class EffectCardDictionary : SerializableDictionaryBase<CardType, SegmentEffect> { }

    public Transform[] earthquakePlatformArray = new Transform[0];
    [SerializeField] EffectCardDictionary effectPerCard; //TODO create it dynamically
    List<Orb> playerInsideList = new List<Orb>();
    float cardEffectEndTime;
    internal CardType cardType;

    const float CARD_EFFECT_DURATION = 10f;

    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Orb>();
        if (player != null) {
            playerInsideList.Add(player);
            if(player.currentSegment != null)
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
        if (effectPerCard.ContainsKey(newCardType)) {
            DisableCardEffectTransforms();
            effectPerCard[newCardType].Activate();
        }
    }

    public void ApplyEffectInPlayer(Orb orb) {
        switch (cardType) {
            case CardType.Squid:
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
        foreach (var effectCard in effectPerCard) {
            if (effectCard.Key == CardType.Squid)
                continue;
            effectCard.Value.Deactivate();
        }
    }
}
