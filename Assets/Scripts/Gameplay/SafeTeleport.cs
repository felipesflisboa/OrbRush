using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTeleport : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<Player>();
        if (player != null) {
            Segment segment = player.lastSegment;
            player.transform.position = segment.transform.position + Vector3.up * 0.6f;
            // player.rigidBody.velocity = Vector3.zero;
        }
    }
}
