using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Orb : MonoBehaviour {
    public Element element;
    public string m_name;
    internal int number;
    internal InputHandler inputHandler;
    internal Rigidbody rigidBody; //TODO protect
    internal Segment currentSegment;
    internal Segment lastSegment;
    internal bool reachGoal;
    public AI ai { get; private set; }
    int fixedUpdateCount;

    public int CardCount => IsCPU ? ai.cardTypeDeck.Count : CanvasController.I.cardZone.ValidCardCount;
    public float Velocity => VelocityV3.magnitude;
    public float HalfSecondAccelerationRatio => 1 - 0.01f * CardCount;
    public bool IsCPU => ai != null;
    public bool Initialized => inputHandler != null;

    public Vector3 VelocityV3 {
        get => rigidBody.velocity;
        set => rigidBody.velocity = value;
    }

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Initialize(int pNumber, InputType inputType, int aiLevel) {
        number = pNumber;
        inputHandler = new InputHandler(inputType);
        if (aiLevel > 0)
            ai = new AI(aiLevel, this);
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
        if (GameManager.I.state == GameState.SelectPlayer && GameManager.modeData is MarathonData)  //TODO check mode
            GameManager.I.StartMarathon(this);
    }
}
