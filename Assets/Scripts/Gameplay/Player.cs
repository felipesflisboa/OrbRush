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
    internal bool reachGoal;
    public AI ai;
    int fixedUpdateCount;

    public int CardCount => ai == null ? CanvasController.I.cardZone.cardList.Count : ai.cardTypeDeck.Count;
    public float Velocity => VelocityV3.magnitude;
    public float HalfSecondAccelerationRatio => 1 - 0.01f * CardCount;

    public Vector3 VelocityV3 {
        get => rigidBody.velocity;
        set => rigidBody.velocity = value;
    }

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Boost() {
        rigidBody.velocity = rigidBody.velocity + rigidBody.velocity.normalized * 1.2f;
    }

    void FixedUpdate() {
        if (fixedUpdateCount % 25 == 0) // half-second //TODO count other way
            VelocityV3 *= HalfSecondAccelerationRatio;
        fixedUpdateCount++;
    }

    void OnMouseDown() {
        if (GameManager.I.humanPlayer == null)  //TODO check mode
            GameManager.I.StartGame(this);
    }
}
