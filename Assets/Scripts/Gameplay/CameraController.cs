using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CameraController : MonoBehaviour{
    [SerializeField] Vector3 characterSelectionPos;
    [SerializeField] Vector3 pivotBonusPos;
    [SerializeField] Vector3 bigDistancePivotBonusPos;
    [SerializeField] Vector3 camBonusPos;
    [Tooltip("x/time: biggest distance between players\ny/value: local Z Pos"), SerializeField] AnimationCurve zPerDistanceBetweenPlayers;
    Vector3 initialPos;
    float maxDistanceBetweenPlayers; // for debug display
    float distanceLimit;
    bool followingPlayers;
    
    Vector3 PivotPos {
        get {
            if (GetMaxDistanceBetweenPlayers() > distanceLimit)
                return GameManager.I.humanPlayer.transform.position.WithY(0)+ bigDistancePivotBonusPos;
            return pivotBonusPos + GetPlayerMidPoint(GameManager.I.nonNullPlayerArray);
        }
    }

    Vector3 TargetPos => PivotPos - transform.forward * zPerDistanceBetweenPlayers.Evaluate(GetMaxDistanceBetweenPlayers());

    void Awake() {
        initialPos = transform.position;
        distanceLimit = zPerDistanceBetweenPlayers.keys.Max(key => key.time);
        transform.position = characterSelectionPos;
    }

    void Update(){
        if (GameManager.I.playerArray == null || !followingPlayers)
            return;
        transform.position = GetPos();
#if UNITY_EDITOR
        maxDistanceBetweenPlayers = GetMaxDistanceBetweenPlayers();
#endif
    }

    public void GoToOriginalPos() {
        transform.DOMove(initialPos, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => followingPlayers = true);
    }

    Vector3 GetPlayerMidPoint(Player[] playerArray) {
        return new Vector3(
            (playerArray.Max(p => p.transform.position.x) + playerArray.Min(p => p.transform.position.x)) / 2,
            0,
            (playerArray.Max(p => p.transform.position.z) + playerArray.Min(p => p.transform.position.z)) / 2
        );
    }

    Vector3 GetPos() {
        return Vector3.Lerp(transform.position, TargetPos, 1.2f * Time.deltaTime);
    }

    float GetMaxDistanceBetweenPlayers() {
        float ret = 0;
        for (int i = 0; i < GameManager.I.nonNullPlayerArray.Length - 1; i++) {
            for (int j = i + 1; j < GameManager.I.nonNullPlayerArray.Length; j++) {
                ret = Mathf.Max(ret, Mathf.Abs(Vector3.Distance(
                    GameManager.I.nonNullPlayerArray[i].transform.position, GameManager.I.nonNullPlayerArray[j].transform.position
                )));
            }
        }
        return ret;
    }

    void OnDrawGizmosSelected() {
        if (!Application.isPlaying || GameManager.I.playerArray == null)
            return;
        Gizmos.color = Color.blue.WithAlpha(0.5f);
        Gizmos.DrawSphere(PivotPos, 0.5f);
    }
}