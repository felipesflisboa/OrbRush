using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZone : MonoBehaviour{
    public readonly List<Card> cardList = new List<Card>();

    public void Add(GameObject prefab) {
        var card = Instantiate(prefab, transform).GetComponent<Card>();
        cardList.Add(card);
    }

    void Update() {
        for (int i = 0; i < cardList.Count; i++) {
            if (cardList[i] == null)
                cardList.RemoveAt(i--);
        }
    }
}
