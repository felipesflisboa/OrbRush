using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInputHandler : InputHandler {
    public override bool CanClick => true;
    public override bool HasCursor => false;

    public ClickInputHandler() : base() {}
}