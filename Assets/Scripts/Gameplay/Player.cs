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
    internal Segment lastSegment;
    public AI ai;
    int fixedUpdateCount;

    public int CardCount => ai == null ? CanvasController.I.cardZone.cardList.Count : ai.cardTypeDeck.Count;

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Boost() {
        rigidBody.velocity = rigidBody.velocity + rigidBody.velocity.normalized * 1.2f;
    }

    void FixedUpdate() {
        velTest = rigidBody.velocity; //remove
        if (fixedUpdateCount % 25 == 0) // half-second
            rigidBody.velocity = rigidBody.velocity * (1 - 0.01f * CardCount);
        fixedUpdateCount++;
    }

    public Vector3 velTest;

    void OnMouseDown() {
        if (GameManager.I.humanPlayer == null) {
            GameManager.I.humanPlayer = this;
            GameManager.I.StartGame();
        }
    }
}
