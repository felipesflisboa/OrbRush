using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CameraController : MonoBehaviour {
    [SerializeField] Vector3 pivotBonusPos;
    [SerializeField] Vector3 bigDistancePivotBonusPos;
    [SerializeField] Vector3 camBonusPos;
    [Tooltip("x/time: biggest distance between players\ny/value: local Z Pos"), SerializeField] AnimationCurve zPerDistanceBetweenPlayers;
    Vector3 initialAnimationTargetPos;
    Quaternion initialRot;

    [Header("Start animation/position")]
    [SerializeField] float initialAnimationDuration;
    [SerializeField] float initialAnimationYBonus;
    [SerializeField] float initialAnimationSpinCount;
    [SerializeField] Vector3 characterSelectionPos;
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
    Vector3 RelativePosInAnimation => characterSelectionPos.WithY(transform.position.y) - initialAnimationTargetPos;
    // bool InitialAnimationPlaying => !followingPlayers; //remove

    void Awake() {
        initialRot = transform.rotation;
        distanceLimit = zPerDistanceBetweenPlayers.keys.Max(key => key.time);
        transform.position = characterSelectionPos;
    }

    void Start() {
        PlayInitialAnimation();
    }

    async void PlayInitialAnimation() {
        transform.position = transform.position.WithIncY(initialAnimationYBonus);
        initialAnimationTargetPos = GameManager.I.SpawnPointCenter;
        Rotate(0);
        transform.DOBlendableMoveBy(
            initialAnimationYBonus * Vector3.down, initialAnimationDuration
        ).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => followingPlayers = true);
        await DOTween.To(Rotate, 0, 1, initialAnimationDuration * 0.98f).SetEase(Ease.InOutSine).SetUpdate(true).WaitForCompletion();
        transform.DORotate(initialRot.eulerAngles, 0.3f).SetUpdate(true);
        if(GameManager.I.state == GameState.BeforeStart)
            CanvasController.I.startText.gameObject.SetActive(true);
    }

    void Rotate(float ratio) {
        transform.position = Quaternion.Euler(
            Vector3.up * initialAnimationSpinCount * 360 *(1 - ratio)
        ) * RelativePosInAnimation + initialAnimationTargetPos;
        transform.LookAt(initialAnimationTargetPos);
    }

    void Update(){
        if (GameManager.I.playerArray == null || !followingPlayers)
            return;
        transform.position = GetPos();
#if UNITY_EDITOR
        maxDistanceBetweenPlayers = GetMaxDistanceBetweenPlayers();
#endif
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