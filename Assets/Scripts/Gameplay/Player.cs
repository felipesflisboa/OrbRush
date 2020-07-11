using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public int number;
    public Rigidbody rigidBody; //TODO protect

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Boost() {
        rigidBody.velocity = rigidBody.velocity + rigidBody.velocity.normalized * 1.2f;
    }
}
