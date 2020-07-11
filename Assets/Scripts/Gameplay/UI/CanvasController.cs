using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : SingletonMonoBehaviour<CanvasController> {
    public CardZone cardZone;

    Canvas _canvas;
    public Canvas Canvas {
        get {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }

    void Awake() {
        cardZone = GetComponentInChildren<CardZone>();
    }
}
