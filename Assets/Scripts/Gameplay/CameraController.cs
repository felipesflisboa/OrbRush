using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CameraController : MonoBehaviour{
    [SerializeField] Vector3 characterSelectionPos;
    [SerializeField] Vector3 camBonusPos;
    [Tooltip("x/time: biggest distance between players\ny/value: local Z Pos"), SerializeField] AnimationCurve zPerDistanceBetweenPlayers;
    Vector3 initialPos;
    float maxDistanceBetweenPlayers; // for debug display
    bool followingPlayers;

    //TODO adjust when too distant. Focus human players only
    Vector3 PivotPos {
        get {
            return new Vector3(
                (
                    GameManager.I.nonNullPlayerArray.Max(p => p.transform.position.x) + 
                    GameManager.I.nonNullPlayerArray.Min(p => p.transform.position.x)
                ) / 2,
                0,
                (
                    GameManager.I.nonNullPlayerArray.Max(p => p.transform.position.z) + 
                    GameManager.I.nonNullPlayerArray.Min(p => p.transform.position.z)
                ) / 2
            );
        }
    }

    Vector3 TargetPos => PivotPos + camBonusPos - transform.forward * zPerDistanceBetweenPlayers.Evaluate(GetMaxZDistanceBetweenPlayers());

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
#if UNITY_EDITOR
        maxDistanceBetweenPlayers = GetMaxZDistanceBetweenPlayers();
#endif
    }

    Vector3 GetPos() {
        return Vector3.Lerp(transform.position, TargetPos, 2f * Time.deltaTime);
    }

    float GetMaxZDistanceBetweenPlayers() {
        float ret = 0;
        for (int i = 0; i < GameManager.I.nonNullPlayerArray.Length - 1; i++) {
            for (int j = i + 1; j < GameManager.I.nonNullPlayerArray.Length; j++) {
                ret = Mathf.Max(ret, Mathf.Abs(
                    GameManager.I.nonNullPlayerArray[i].transform.position.z - GameManager.I.nonNullPlayerArray[j].transform.position.z
                ));
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