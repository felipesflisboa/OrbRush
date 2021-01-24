using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputHandler : InputHandler {
    AxisPressCheck verticalAxisPressCheck;

    public override float VerticalInputAxisDown => verticalAxisPressCheck == null ? 0f : verticalAxisPressCheck.GetAxisOnPress();
    public override bool ConfirmTriggered => Input.GetButtonDown("Fire1");
    
    public KeyboardInputHandler() : base (){
        verticalAxisPressCheck = new AxisPressCheck("Vertical", 0.2f);
    }

    public override void Update() {
        verticalAxisPressCheck.Update();
    }
}