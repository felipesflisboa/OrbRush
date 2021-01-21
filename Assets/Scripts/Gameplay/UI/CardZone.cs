using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardZone : MonoBehaviour {
    [SerializeField] RectTransform rangeRectTransform;
    [SerializeField] float initialCardBonusY;
    internal InputHandler inputHandler;
    float cardHeight;

    public readonly List<Card> cardList = new List<Card>();

    public bool Active => inputHandler != null;
    public int ValidCardCount => cardList.Count(c => c.valid);

    public void Add(GameObject prefab) {
        cardList.Add(CreateCard(prefab));
        if (cardHeight == 0)
            cardHeight = cardList[cardList.Count - 1].RectTransform.rect.height;
        RefreshPosition();
    }

    Card CreateCard(GameObject prefab) {
        Card ret = Instantiate(prefab, transform).GetComponent<Card>();
        ret.zone = this;
        return ret;
    }

    void Update() {
        for (int i = 0; i < cardList.Count; i++) {
            if (cardList[i] == null) {
                cardList.RemoveAt(i--);
                RefreshPosition();
            }
        }
    }

    void RefreshPosition() {
        for (int i = 0; i < cardList.Count; i++) {
            if (cardList[i] == null)
                continue;
            float gain = Mathf.Min(rangeRectTransform.rect.height / cardList.Count, cardHeight + initialCardBonusY);
            cardList[i].RefreshPosition(rangeRectTransform.rect.yMax - cardHeight*0.5f - i* gain);
        }
    }
}
