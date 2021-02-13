using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectScreen : MonoBehaviour{
    internal PlayerSelectPanel[] panelArray;
    [SerializeField] Button confirmButton;
    [SerializeField] Button quitButton;

    const string INPUT_KEY = "PlayerInput{0}";

    void Awake() {
        panelArray = GetComponentsInChildren<PlayerSelectPanel>(true).OrderBy(psp => psp.number).ToArray();
        confirmButton.onClick.AddListener(OnConfirm);
        quitButton.onClick.AddListener(() => GameManager.I.BackToMainMenu());
    }

    void OnConfirm() {
        SaveAll();
        GameManager.I.StartQuickRace();
    }

    void SaveAll() {
        foreach (var panel in panelArray)
            panel.Save();
    }

    public PlayerSelectPanel[] GetPanelWithCPULast() => panelArray.OrderBy(psp => psp.IsCPU() ? 1 : 0).ToArray();
    public bool TypeIsAlreadyUsed(PlayerType type) => panelArray.Count((pp) => pp.type == type) >= 2;

    public void Save(int playerNumber, PlayerType type) => PlayerPrefs.SetInt(string.Format(INPUT_KEY, playerNumber), (int)type);

    public PlayerType Load(int playerNumber) {
        PlayerType ret = (PlayerType)PlayerPrefs.GetInt(string.Format(INPUT_KEY, playerNumber));
        if (ret == PlayerType.None)
            ret = playerNumber == 1 ? PlayerType.Click : PlayerType.CPUEasy;
        return ret;
    }
}
