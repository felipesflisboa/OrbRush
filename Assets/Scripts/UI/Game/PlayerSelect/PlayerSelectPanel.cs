using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class PlayerSelectPanel : MonoBehaviour {
    [Serializable] class ElementColorDictionary : SerializableDictionaryBase<Element, Color> { }

    public int number;
    public Element element;
    internal PlayerType type;
    [SerializeField] ElementColorDictionary colorPerElement;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;
    [SerializeField] private Image backgroundImage;
    Color imageOriginalColor;

    int TypeCount => Enum.GetNames(typeof(PlayerType)).Length;
    Color ImageColor => colorPerElement[element];
    PlayerSelectScreen PlayerSelectScreen => GameManager.I.canvasController.playerSelectScreen;

    string ClickLabel {
        get {
#if UNITY_ANDROID
            return "Human";
#else
            return "Mouse";
#endif
        }
    }

    void Awake() {
        type = Load();
        backgroundImage.color = ImageColor;
        previousButton.onClick.AddListener(() => IncType(-1));
        nextButton.onClick.AddListener(() => IncType(1));
    }

    void Start() {
        Refresh();
    }

    void Update() {
        Refresh();
        RefreshPressedInput();
    }

    void Refresh() {
        text.text = GetTypeName(type);
        if (IsJoystick(type) && !InputIsConnected(type))
            text.text += "\n(off)";
    }

    void RefreshPressedInput() {
        if (!HasAnyButtonPressed(type))
            return;
        DOTween.Kill(backgroundImage);
        DOTween.Sequence().Append(
            backgroundImage.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.1f)
        ).Append(
            backgroundImage.DOColor(ImageColor, 0.2f)
        ).SetUpdate(true);
    }

    string GetTypeName(PlayerType type) {
        switch (type) {
            case PlayerType.Click:      return ClickLabel;
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

    public bool IsJoystick(PlayerType type) {
        return new[] { PlayerType.Joystick1, PlayerType.Joystick2, PlayerType.Joystick3, PlayerType.Joystick4 }.Contains(type);
    }

    public int GetJoystickNumber(PlayerType type) => (int)type - 2;
    public bool IsTypeExclusive(PlayerType type) => new[] { PlayerType.Click, PlayerType.Keyboard }.Contains(type) || IsJoystick(type);
    public bool InputIsConnected(PlayerType type) => GetJoystickNumber(type) <= Input.GetJoystickNames().Length;
    public bool IsCPU(PlayerType type) => new[] { PlayerType.CPUEasy, PlayerType.CPUNormal, PlayerType.CPUHard }.Contains(type);
    public bool IsCPU() => IsCPU(type);
    bool CanSelectNow(PlayerType type) => IsSelectableType(type) && (!IsTypeExclusive(type) || !PlayerSelectScreen.TypeIsAlreadyUsed(type));

    public bool IsSelectableType(PlayerType type) {
        if (new[] { PlayerType.None }.Contains(type))
            return false;
#if UNITY_ANDROID
        if (new[] { PlayerType.Keyboard }.Contains(type) || IsJoystick(type))
            return false;
#endif
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
    
    public void Save() => PlayerSelectScreen.Save(number, type);
    public PlayerType Load() => PlayerSelectScreen.Load(number);
}