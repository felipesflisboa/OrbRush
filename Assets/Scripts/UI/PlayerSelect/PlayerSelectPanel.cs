using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//TODO save and restore last type combination
public class PlayerSelectPanel : MonoBehaviour {
    public int number;
    public Element element;
    internal PlayerType type;
    [SerializeField] Text text;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;
    [SerializeField] Image backgroundImage;
    Color imageOriginalColor;

    int TypeCount => Enum.GetNames(typeof(PlayerType)).Length;

    void Awake() {
        type = number == 1 ? PlayerType.Click : PlayerType.CPUEasy;
        imageOriginalColor = backgroundImage.color;
        previousButton.onClick.AddListener(() => IncType(-1));
        nextButton.onClick.AddListener(() => IncType(1));
    }

    void Update() {
        Refresh();
        RefreshPressedInput();
    }

    void Refresh() {
        text.text = GetTypeName(type);
        if (IsJoystick(type) && !InputIsConnected(type))
            text.text += "\n(unconnected)";
    }

    void RefreshPressedInput() {
        if (!HasAnyButtonPressed(type))
            return;
        DOTween.Kill(backgroundImage);
        DOTween.Sequence().Append(
            backgroundImage.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.1f)
        ).Append(
            backgroundImage.DOColor(imageOriginalColor, 0.2f)
        );
    }

    string GetTypeName(PlayerType type) {
        switch (type) {
            case PlayerType.Click:      return "Mouse";
            case PlayerType.Keyboard:   return "Keyboard";
            case PlayerType.Joystick1:  return "Joystick1";
            case PlayerType.Joystick2:  return "Joystick2";
            case PlayerType.Joystick3:  return "Joystick3";
            case PlayerType.Joystick4:  return "Joystick4";
            case PlayerType.CPUEasy:    return "CPU\nEasy";
            case PlayerType.CPUNormal:  return "CPU\nNormal";
            case PlayerType.CPUHard:    return "CPU\nHard";
        }
        return "None";
    }

    public InputType GetInputType() => GetInputType(type);

    public InputType GetInputType(PlayerType playerType) {
        return (InputType) Mathf.Min(
            (int)playerType, 
            Enum.GetNames(typeof(InputType)).Length-1
        );
    }

    public bool IsJoystick(PlayerType type) {
        return new[] { PlayerType.Joystick1, PlayerType.Joystick2, PlayerType.Joystick3, PlayerType.Joystick4 }.Contains(type);
    }

    public int GetJoystickNumber(PlayerType type) => (int)type - 2;
    public bool IsTypeExclusive(PlayerType type) => new[] { PlayerType.Click, PlayerType.Keyboard }.Contains(type) || IsJoystick(type);
    public bool InputIsConnected(PlayerType type) => GetJoystickNumber(type) <= Input.GetJoystickNames().Length;
    public bool IsCPU(PlayerType type) => new[] { PlayerType.CPUEasy, PlayerType.CPUEasy, PlayerType.CPUEasy }.Contains(type);
    public bool IsCPU() => IsCPU(type);

    bool CanSelectNow(PlayerType type) {
        return IsSelectableType(type) && (!IsTypeExclusive(type) || !CanvasController.I.playerSelectScreen.TypeIsAlreadyUsed(type));
    }

    public bool IsSelectableType(PlayerType type) {
        if (new[] { PlayerType.None }.Contains(type))
            return false;
        //TODO add here Android types
        return true;
    }

    void IncType(int gain) {
        type = (PlayerType)((TypeCount + (int)type + gain) % TypeCount);
        if (!CanSelectNow(type)) {
            IncType(gain);
            return;
        }
        Refresh();
    }

    bool HasAnyButtonPressed(PlayerType type) {
        if (!IsJoystick(type))
            return false;
        for (int i = 0; i < 20; i++)
            if (Input.GetKey($"joystick {GetJoystickNumber(type)} button {i}"))
                return true;
        return false;
    }
}