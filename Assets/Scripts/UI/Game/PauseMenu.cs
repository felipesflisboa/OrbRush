using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour{
    CanvasGroup canvasGroup;
    [SerializeField] Button closeButton;
    [SerializeField] Button exitButton;

    bool Active {
        get => canvasGroup.interactable;
        set => SetActive(value);
    }

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        SetActive(false, false);
        closeButton.onClick.AddListener(GameManager.I.TogglePause);
        exitButton.onClick.AddListener(GameManager.I.CancelGame);
    }

    void LateUpdate() {
        if (GameManager.I.Paused != Active)
            Active = !Active;
    }

    void SetActive(bool active, bool playAnimation = true) {
        canvasGroup.interactable = active;
        canvasGroup.blocksRaycasts = active;
        if (playAnimation) {
            if (active)
                GameManager.I.canvasController.ShowWithFadeAnimation(canvasGroup);
            else
                GameManager.I.canvasController.HideWithFadeAnimation(canvasGroup, () => { });
        }
    }
}
