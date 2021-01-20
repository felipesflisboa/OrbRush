using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO use inheritance
//TODO make AI use it.
public class InputHandler {
    InputType type;

    const string INPUT_KEY = "PlayerInput{0}";

    public bool CanClick => type == InputType.Click;

    public float VerticalInputAxis {
        get {
            switch (type) {
                case InputType.Keyboard: return Input.GetAxis("Vertical");
                case InputType.Joystick1: return Input.GetAxis("VerticalJoystick1");
                case InputType.Joystick2: return Input.GetAxis("VerticalJoystick2");
                case InputType.Joystick3: return Input.GetAxis("VerticalJoystick3");
                case InputType.Joystick4: return Input.GetAxis("VerticalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxis {
        get {
            switch (type) {
                case InputType.Keyboard: return Input.GetAxis("Horizontal");
                case InputType.Joystick1: return Input.GetAxis("HorizontalJoystick1");
                case InputType.Joystick2: return Input.GetAxis("HorizontalJoystick2");
                case InputType.Joystick3: return Input.GetAxis("HorizontalJoystick3");
                case InputType.Joystick4: return Input.GetAxis("HorizontalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public float HorizontalInputAxisRaw {
        get {
            switch (type) {
                case InputType.Keyboard: return Input.GetAxisRaw("Horizontal");
                case InputType.Joystick1: return Input.GetAxisRaw("HorizontalJoystick1");
                case InputType.Joystick2: return Input.GetAxisRaw("HorizontalJoystick2");
                case InputType.Joystick3: return Input.GetAxisRaw("HorizontalJoystick3");
                case InputType.Joystick4: return Input.GetAxisRaw("HorizontalJoystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public bool ConfirmTriggered {
        get {
            switch (type) {
                case InputType.Keyboard: return Input.GetButtonDown("Fire1");
                case InputType.Joystick1: return Input.GetButtonDown("Fire1Joystick1");
                case InputType.Joystick2: return Input.GetButtonDown("Fire1Joystick2");
                case InputType.Joystick3: return Input.GetButtonDown("Fire1Joystick3");
                case InputType.Joystick4: return Input.GetButtonDown("Fire1Joystick4");
            }
            throw new System.NotImplementedException();
        }
    }

    public InputHandler(InputType pType) {
        type = pType;
    }
}