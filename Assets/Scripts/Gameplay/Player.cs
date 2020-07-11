using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO rename
public class Player : MonoBehaviour{
    public int number;
    internal Rigidbody rigidBody; //TODO protect
    internal Segment currentSegment;

    void Awake() {
        rigidBody = GetComponentInChildren<Rigidbody>();
    }

    public void Boost() {
        rigidBody.velocity = rigidBody.velocity + rigidBody.velocity.normalized * 1.2f;
    }
}
