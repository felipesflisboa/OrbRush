using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class PlayerHUD : MonoBehaviour {
    [System.Serializable] class ElementColorDictionary : SerializableDictionaryBase<Element, Color> { }

    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI accelerationText;
    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] int playerNumber;
    Orb player;

    [SerializeField] ElementColorDictionary colorPerElement;
    [SerializeField] Color tooManyCardColor;
    [SerializeField] Color cardSmallWarnColor;
    [SerializeField] Color cardBigWarnColor;
    Color initialTextColor;

    const float BALL_SIZE_MULTIPLIER = 0.02f;

    public float DisplayVelocity => player.Velocity * 3.6f* BALL_SIZE_MULTIPLIER;

    Color CardTextColor {
        get {
            if (player.CardCount >= 5) {
                return cardSmallWarnColor;
            } else if(player.CardCount >= 3) {
                return cardBigWarnColor;
            }
            return initialTextColor;
        }
    }

    void Awake() {
        initialTextColor = cardText.color;
    }

    void Start() {
        UpdateLoop();
    }

    void Initialize() {
        player = GameManager.I.orbArray[playerNumber];
        backgroundImage.DOColor(colorPerElement[player.element], 1.2f);
    }

    async void UpdateLoop() {
        await new WaitWhile(() => GameManager.I.state != GameState.Ocurring);
        Initialize();
        while (GameManager.I.state == GameState.Ocurring) {
            Refresh();
            await new WaitForSeconds(0.15f);
        }
    }

    void Refresh() {
        speedText.text = DisplayVelocity.ToString("F2");
        accelerationText.text = $"{(100 * Mathf.Pow(player.HalfSecondAccelerationRatio, 2)):F0}%";
        accelerationText.color = CardTextColor;
        cardText.text = player.CardCount.ToString();
        cardText.color = CardTextColor;
    }
}
