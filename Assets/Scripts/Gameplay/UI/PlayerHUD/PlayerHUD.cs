using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//TODO color
//TODO pulse start on 4 and raise until 6 or more cards
public class PlayerHUD : MonoBehaviour{
    [SerializeField] PlayerHUDColorGroup colorGroup;
    [SerializeField] Image backgroundImage;
    [SerializeField] Text speedText;
    [SerializeField] Text accelerationText;
    [SerializeField] Text cardText;
    [SerializeField] int playerNumber;
    Player player;

    const float BALL_SIZE_MULTIPLIER = 0.1f;

    bool Initialized => player != null; //remove
    public float DisplayVelocity => player.Velocity * 3.6f* BALL_SIZE_MULTIPLIER;

    void Start() {
        UpdateLoop();
    }

    void Initialize() {
        player = GameManager.I.playerArray[playerNumber];
        backgroundImage.DOColor(colorGroup.getColor(player.element), 1.2f);
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
        cardText.text = player.CardCount.ToString();
        if (player.CardCount >= 4) {
            accelerationText.text = $"! {accelerationText.text}";
            cardText.text = $"! {cardText.text}";
        }
    }
}
