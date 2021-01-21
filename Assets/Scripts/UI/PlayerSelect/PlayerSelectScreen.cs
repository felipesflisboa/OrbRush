using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectScreen : MonoBehaviour{
    internal PlayerSelectPanel[] panelArray;
    [SerializeField] Button confirmButton;

    void Awake() {
        panelArray = GetComponentsInChildren<PlayerSelectPanel>(true).OrderBy(psp => psp.number).ToArray();
        confirmButton.onClick.AddListener(() => GameManager.I.StartQuickRace());
    }

    public PlayerSelectPanel[] GetPanelWithCPULast() => panelArray.OrderBy(psp => psp.IsCPU() ? 1 : 0).ToArray();
    public bool TypeIsAlreadyUsed(PlayerType type) => CanvasController.I.playerSelectScreen.panelArray.Count((pp) => pp.type == type) >= 2;
}
