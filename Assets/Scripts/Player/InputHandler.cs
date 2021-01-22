using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//TODO use inheritance
//TODO make AI use it.
public class InputHandler {
    InputType type;
    AxisPressCheck verticalAxisPressCheck;

    public bool CanClick => type == InputType.Click;

    public bool HasCursor {
        get {
            return new[] {
                InputType.Keyboard, InputType.Joystick1, InputType.Joystick2, InputType.Joystick3, InputType.Joystick4
            }.Contains(type);
        }
    }

    public float VerticalInputAxisDown => verticalAxisPressCheck == null ? 0f : verticalAxisPressCheck.GetAxisOnPress();

    public bool ConfirmTriggered {
        get {
            switch (type) {
                case InputType.Keyboard: return Input.GetButtonDown("Fire1");
                case InputType.Joystick1: return Input.GetButtonDown("Fire1Joystick1");
                case InputType.Joystick2: return Input.GetButtonDown("Fire1Joystick2");
                case InputType.Joystick3: return Input.GetButtonDown("Fire1Joystick3");
                case InputType.Joystick4: return Input.GetButtonDown("Fire1Joystick4");
            }
            return false;
        }
    }

    string VerticalAxisButtonName {
        get {
            switch (type) {
                case InputType.Keyboard: return "Vertical";
                case InputType.Joystick1: return "VerticalJoystick1";
                case InputType.Joystick2: return "VerticalJoystick2";
                case InputType.Joystick3: return "VerticalJoystick3";
                case InputType.Joystick4: return "VerticalJoystick4";
            }
            return null;
        }

    }

    public InputHandler(InputType pType) {
        type = pType;
        InitializePressCheck();
    }

    void InitializePressCheck() {
        if (VerticalAxisButtonName != null)
            verticalAxisPressCheck = new AxisPressCheck(VerticalAxisButtonName, 0.4f);
    }

    public void Update() {
        if (VerticalAxisButtonName != null)
            verticalAxisPressCheck.Update();
    }
}