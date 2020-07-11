using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour{
    [SerializeField] Vector3 characterSelectionPos; 
    Vector3 initialPos;
    bool followingPlayers;

    void Awake() {
        initialPos = transform.position;
        transform.position = characterSelectionPos;
    }

    public void GoToOriginalPos() {
        transform.DOMove(initialPos, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => followingPlayers = true);
    }

    void Update(){
        if (GameManager.I.playerArray == null || !followingPlayers)
            return;
        transform.position = transform.position.WithZ(GetCurrentPosZ());
    }

    float GetCurrentPosZ() {
        float z = 0;
        foreach (var player in GameManager.I.playerArray) {
            if (player == null)
                continue;
            z += player.transform.position.z;
        }
        return Mathf.Lerp(transform.position.z, z / (GameManager.I.playerArray.Length - 1) + 55, 2 * Time.deltaTime);
    }
}
