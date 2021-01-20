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
    Quaternion initialRot;
    float maxDistanceBetweenPlayers; // for debug display
    float distanceLimit;
    bool followingPlayers;

    [Header("Start animation/position")]
    [SerializeField] float initialAnimationDuration;
    [SerializeField] float initialAnimationYBonus;
    [SerializeField] float initialAnimationSpinCount;
    [SerializeField] float characterSelectionAngle;
    [SerializeField] Vector3 characterSelectionBonusPos;
    Vector3 initialAnimationPos;
    Vector3 initialAnimationTargetPos;

    Vector3 PivotPos {
        get {
            if (GetMaxDistanceBetweenPlayers() > distanceLimit) {
                int playerCount = 0;
                Vector3 sumPos = Vector3.zero;
                foreach (var player in GameManager.I.nonNullPlayerArray) {
                    if (player.IsCPU)
                        continue;
                    sumPos += player.transform.position.WithY(0);
                    playerCount++;
                }
                if(playerCount > 0)
                    return sumPos/ playerCount;
            }
            return pivotBonusPos + GetPlayerMidPoint(GameManager.I.nonNullPlayerArray);
        }
    }

    Vector3 TargetPos => PivotPos - transform.forward * zPerDistanceBetweenPlayers.Evaluate(GetMaxDistanceBetweenPlayers());
    Vector3 RelativePosInAnimation => initialAnimationPos - initialAnimationTargetPos;
    bool InitialAnimationPlaying => !followingPlayers;

    void Awake() {
        initialRot = transform.rotation;
        distanceLimit = zPerDistanceBetweenPlayers.keys.Max(key => key.time);
    }

    void Start() {
        PlayInitialAnimation();
    }

    async void PlayInitialAnimation() {
        SetupInitialAnimationPosRot();
        transform.DOBlendableMoveBy(initialAnimationYBonus * Vector3.down, initialAnimationDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        await DOTween.To(Rotate, 0, 1, initialAnimationDuration * 0.98f).SetEase(Ease.InOutSine).SetUpdate(true).WaitForCompletion();
        GameManager.I.OnCameraInitialAnimationEnd();
    }

    void SetupInitialAnimationPosRot() {
        initialAnimationTargetPos = GameManager.I.SpawnPointCenter;
        initialAnimationPos = initialAnimationTargetPos + characterSelectionBonusPos + initialAnimationYBonus * Vector3.up;
        transform.position = initialAnimationPos;
        Rotate(0);
    }

    public void OnGameStart() {

        transform.DORotate(initialRot.eulerAngles, 0.8f).SetEase(Ease.InOutSine);
        followingPlayers = true;
    }

    void Rotate(float ratio) {
        transform.position = (Quaternion.Euler(
            Vector3.up * (initialAnimationSpinCount * 360 * (1 - ratio) + characterSelectionAngle)
        ) * RelativePosInAnimation + initialAnimationTargetPos).WithY(transform.position.y);
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