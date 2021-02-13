using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Orb>();
        if (player != null) 
            GameManager.I.stage.OnReachGoal(player);
    }
}