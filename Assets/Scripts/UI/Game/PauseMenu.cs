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
        set {
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
            canvasGroup.alpha = value ? 1 : 0;
        }
    }

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        Active = false;
        closeButton.onClick.AddListener(GameManager.I.TogglePause);
        exitButton.onClick.AddListener(() => GameManager.I.BackToMainMenu(false));
    }

    void LateUpdate() {
        if (GameManager.I.Paused != Active)
            Active = !Active;
    }
}
