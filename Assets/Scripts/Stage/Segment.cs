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

    public void ApplyEffectInPlayer(Orb player) {
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

    void FixedUpdate() {
        if (cardType == CardType.Tornado)
            ApplyTornadoEffect();
    }

    void ApplyTornadoEffect() {
        foreach (var player in playerInsideList) {
            if (player.element == Element.Air)
                continue;
            player.rigidBody.AddForce(MathUtil.UnityAngleToNormal(
                MathUtil.GetAngle(tornadoTransform.position.To2DXZ() - player.transform.position.To2DXZ()) - 90
            ) * 350 * Time.deltaTime, ForceMode.Acceleration);
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
