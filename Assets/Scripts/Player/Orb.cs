using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Orb : MonoBehaviour {
    public Element element;
    public string m_name;
    internal int number;
    internal CardZone cardZone;
    internal InputHandler inputHandler;
    internal Rigidbody rigidBody; //TODO protect
    internal Segment currentSegment;
    internal Segment lastSegment;
    internal bool reachGoal;
    internal bool menuMode;
    public AI ai { get; private set; }
    int fixedUpdateCount;

    const float MAX_MENU_VELOCITY = 12f;

    public int CardCount => IsCPU ? ai.cardTypeDeck.Count : cardZone.ValidCardCount;
    public float Velocity => VelocityV3.magnitude;
    public float HalfSecondAccelerationRatio => 1 - 0.01f * CardCount;
    public bool IsCPU => ai != null;
    public bool Initialized => inputHandler != null || menuMode;

    public Vector3 VelocityV3 {  
        get => rigidBody.velocity;
        set => rigidBody.velocity = value;
    }

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
        menuMode = FindObjectOfType<GameManager>() == null;
    }

    public void InitializeAsCPU(int pNumber, InputHandler pInputHandler, int aiLevel) {
        Initialize(pNumber, pInputHandler);
        ai = new AI(aiLevel, this);
    }

    public void InitializeAsHuman(int pNumber, InputHandler pInputHandler, CardZone pCardZone) {
        Initialize(pNumber, pInputHandler);
        cardZone = pCardZone;
        cardZone.Initialize(this);
    }

    void Initialize(int pNumber, InputHandler pInputHandler) {
        number = pNumber;
        inputHandler = pInputHandler;
    }

    public void Boost() {
        VelocityV3 += VelocityV3.normalized * 1.2f;
    }

    public void TeleportBackToLastSegment() {
        if(!menuMode)
            InstantiateTeleportEffect();
        transform.position = lastSegment.transform.position + Vector3.up * 0.6f;
        if (!menuMode)
            InstantiateTeleportEffect();
    }

    void InstantiateTeleportEffect() {
        Destroy(Instantiate(GameManager.I.teleportEffectPrefab, transform.position, Quaternion.identity), 4f);
    }

    void FixedUpdate() {
        if (!Initialized)
            return;
        if (menuMode)
            MenuFixedUpdate();
        else
            GameFixedUpdate();
    }

    void GameFixedUpdate() {
        if (fixedUpdateCount % 25 == 0) // half-second //TODO count other way
            VelocityV3 *= HalfSecondAccelerationRatio;
        fixedUpdateCount++;
    }

    void MenuFixedUpdate() {
        if(Velocity > MAX_MENU_VELOCITY)
            VelocityV3 = VelocityV3.normalized* MAX_MENU_VELOCITY;
    }

    void OnMouseDown() {
        if (GameManager.I.state == GameState.SelectPlayer && GameManager.modeData is MarathonData)  //TODO check mode
            GameManager.I.StartMarathon(this);
    }
}
