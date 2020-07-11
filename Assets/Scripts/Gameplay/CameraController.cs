using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    void Update(){
        if (GameManager.I.playerArray == null)
            return;
        float z = 0;
        foreach (var player in GameManager.I.playerArray) {
            if (player == null)
                continue;
            z += player.transform.position.z;
        }
        transform.position = transform.position.WithZ(z/(+GameManager.I.playerArray.Length-1) + 55);
    }
}
