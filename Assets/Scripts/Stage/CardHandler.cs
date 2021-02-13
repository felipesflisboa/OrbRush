using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle Card Effects
/// </summary>
[System.Serializable]
public class CardHandler {
    [SerializeField, PrefabReference] GameObject explosionPrefab;

    public void ExecuteCardEffect(Card card, CardType cardType, bool isCPU) {
        switch (cardType) {
            case CardType.Fire:
                if (ApplyExplosionEffect(GameManager.I.GetOrb(Element.Fire)) && card != null)
                    card.Remove();
                break;
            case CardType.Tornado:
            case CardType.Earthquake:
            case CardType.Lake:
                if (ApplySegmentEffect(cardType, isCPU) && card != null)
                    card.Remove();
                break;
            default:
                Debug.Log("Card without effect=" + cardType);
                break;
        }
    }

    bool ApplyExplosionEffect(Orb selectedOrb) {
        const float radius = 7f;
        foreach (var item in Physics.OverlapSphere(selectedOrb.transform.position, radius)) {
            Orb orb = item.GetComponentInParent<Orb>();
            if (orb != null && orb != selectedOrb)
                orb.rigidBody.AddExplosionForce(700, selectedOrb.transform.position, radius);
        }
        Object.Destroy(Object.Instantiate(explosionPrefab, selectedOrb.transform.position, selectedOrb.transform.rotation), 5f);
        return true;
    }

    bool ApplySegmentEffect(CardType cardType, bool isCPU) {
        return ApplySegmentEffect(cardType, isCPU, GameManager.I.GetOrb(GetElement(cardType)));
    }

    bool ApplySegmentEffect(CardType cardType, bool isCPU, Orb orb) {
        if (orb.currentSegment == null) {
            if (!isCPU)
                GameManager.I.canvasController.DisplayOfftrackAlert();
            return false;
        }
        orb.currentSegment.ApplyEffect(cardType);
        return true;
    }

    Element GetElement(CardType cardType) {
        switch (cardType) {
            case CardType.Fire:
                return Element.Fire;
            case CardType.Lake:
                return Element.Water;
            case CardType.Tornado:
                return Element.Air;
            case CardType.Earthquake:
                return Element.Earth;
        }
        return Element.None;
    }
}