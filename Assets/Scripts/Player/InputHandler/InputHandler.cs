using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class InputHandler {
    public virtual bool CanClick => false;
    public virtual bool HasCursor => true;
    public virtual float VerticalInputAxisDown => 0f;
    public virtual bool ConfirmTriggered => false;

    public InputHandler() { }

    public virtual void Update() { }

    public static InputHandler Factory(PlayerType playerType) {
        switch (playerType) {
            case PlayerType.Click:
                return new ClickInputHandler();
            case PlayerType.Keyboard:
                return new KeyboardInputHandler();
            case PlayerType.Joystick1:
            case PlayerType.Joystick2:
            case PlayerType.Joystick3:
            case PlayerType.Joystick4:
                return new JoystickInputHandler(playerType - PlayerType.Joystick1 + 1);
            case PlayerType.CPUEasy:
            case PlayerType.CPUNormal:
            case PlayerType.CPUHard:
                return new CPUInputHandler();
        }
        return null;
    }
}