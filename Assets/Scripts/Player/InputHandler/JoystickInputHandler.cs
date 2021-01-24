using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JoystickInputHandler : InputHandler {
    int number;
    AxisPressCheck verticalAxisPressCheck;
    string fire1Name;

    public override float VerticalInputAxisDown => verticalAxisPressCheck == null ? 0f : verticalAxisPressCheck.GetAxisOnPress();
    public override bool ConfirmTriggered => Input.GetButtonDown(fire1Name);

    public JoystickInputHandler(int pNumber) : base() {
        number = pNumber;
        fire1Name = $"Fire1Joystick{number}";
        verticalAxisPressCheck = new AxisPressCheck($"VerticalJoystick{number}", 0.2f);
    }

    public override void Update() {
        verticalAxisPressCheck.Update();
    }
}