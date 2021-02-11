using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardZone : MonoBehaviour {
    public int player;
    [SerializeField] RectTransform rangeRectTransform;
    //[SerializeField] RectTransform cursorRectTransform; //remove
    [SerializeField] float initialCardBonusY;
    Card selectedCard;
    Orb orb;
    float cardHeight;

    public readonly List<Card> cardList = new List<Card>();

    public bool Active => orb != null;
    public InputHandler InputHandler => orb.inputHandler;
    //public bool HasCursor => orb.inputHandler.HasCursor; //remove
    public int ValidCardCount => cardList.Count(c => c.valid);
    public bool ValidSelection => selectedCard!=null && selectedCard.valid;

    public void Initialize(Orb pOrb) {
        orb = pOrb;
    }

    public void Add(GameObject prefab) {
        cardList.Add(CreateCard(prefab));
        if (cardHeight == 0)
            cardHeight = cardList[cardList.Count - 1].RectTransform.rect.height;
        RefreshCardList();
        RefreshPosition();
    }

    void Highlight(Card card) {
        if (ValidSelection)
            selectedCard.Unhighlight();
        selectedCard = card;
        selectedCard.Highlight();
    }

    Card CreateCard(GameObject prefab) {
        Card ret = Instantiate(prefab, transform).GetComponent<Card>();
        ret.zone = this;
        return ret;
    }

    void Update() {
        RefreshCardList();
        UpdateInput();
        RefreshHighlight();
    }

    void UpdateInput() {
        if (!InputHandler.HasCursor)
            return;
        InputHandler.Update();
        if (InputHandler.VerticalInputAxisDown != 0 && ValidCardCount > 1)
            Highlight(GetNextValidCardOnList(selectedCard, InputHandler.VerticalInputAxisDown < 0 ? 1 : -1));
        if (ValidSelection && InputHandler.ConfirmTriggered)
            selectedCard.TryToUse();
    }

    void RefreshHighlight() {
        if (InputHandler.HasCursor && !ValidSelection && ValidCardCount > 0)
            Highlight(GetCardBefore());
    }

    Card GetCardBefore() => GetNextValidCardOnList(selectedCard, -1);

    Card GetNextValidCardOnList(Card currentCard=null, int indexIncrement = 1) {
        if(currentCard != null) {
            for (
                int i = cardList.IndexOf(currentCard) + indexIncrement; 
                0 < i && i < cardList.Count && cardList[i] != currentCard; 
                i = (i + indexIncrement + cardList.Count) % cardList.Count
            ) {
                if (cardList[i] == null || !cardList[i].valid)
                    continue;
                return cardList[i];
            }
        }
        return cardList.FirstOrDefault(c => c!=null || c.valid);
    }

    void RefreshCardList() {
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
