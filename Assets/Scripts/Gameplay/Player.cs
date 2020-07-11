using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO rename
public class Player : MonoBehaviour {
    public Element element;
    public string m_name;
    internal int number;
    internal Rigidbody rigidBody; //TODO protect
    internal Segment currentSegment;

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Boost() {
        rigidBody.velocity = rigidBody.velocity + rigidBody.velocity.normalized * 1.2f;
    }

    void OnMouseDown() {
        if (GameManager.I.humanPlayer == null) {
            GameManager.I.humanPlayer = this;
            GameManager.I.StartGame();
        }
    }
}
