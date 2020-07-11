using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO optimize
    void Update(){
        float z = 0;
        var pArray = FindObjectsOfType<Player>();
        foreach (var player in pArray) {
            z += player.transform.position.z;
        }
        transform.position = transform.position.WithZ(z/+pArray.Length + 20);
    }
}
