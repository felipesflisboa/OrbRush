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
        transform.position = GetPos();
    }

    Vector3 GetPos() {
        /*
        Vector3 playerSum = Vector3.zero;
        foreach (var player in GameManager.I.playerArray) {
            if (player == null)
                continue;
            playerSum += player.transform.position;
        }
        return Vector3.Lerp(transform.position, (playerSum/ (GameManager.I.playerArray.Length - 1)) + new  Vector3(0, 40, 65), 1.2f * Time.deltaTime);
        */
        return Vector3.Lerp(transform.position, GameManager.I.humanPlayer.transform.position + new Vector3(0, 35, 55), 1.2f * Time.deltaTime);
    }

    /* //remove
    float GetCurrentPosZ() {
        if (playerListWithoutNull == null) {
            playerListWithoutNull = GameManager.I.playerArray.ToList();
            playerListWithoutNull.Remove(null);
        }
        float z = (playerListWithoutNull.Max((p) => p.transform.position.z) + playerListWithoutNull.Min((p) => p.transform.position.z)) / 2 + 30;
        return Mathf.Lerp(transform.position.z, z, 2 * Time.deltaTime);
    }
    */
}