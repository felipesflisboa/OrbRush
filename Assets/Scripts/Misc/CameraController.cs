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
    [Tooltip("x/time: biggest distance between orbs\ny/value: local Z Pos"), SerializeField] AnimationCurve zPerDistanceBetweenOrbs;
    Quaternion initialRot;
    float maxDistanceBetweenOrbs; // for debug display
    float distanceLimit;
    bool followingOrbs;

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
            if (GetMaxDistanceBetweenOrbs() > distanceLimit) {
                int playerCount = 0;
                Vector3 sumPos = Vector3.zero;
                foreach (var orb in GameManager.I.nonNullOrbArray) {
                    if (orb.IsCPU)
                        continue;
                    sumPos += orb.transform.position.WithY(0);
                    playerCount++;
                }
                if(playerCount > 0)
                    return sumPos/ playerCount;
            }
            return pivotBonusPos + GetOrbMidPoint(GameManager.I.nonNullOrbArray);
        }
    }

    Vector3 TargetPos => PivotPos - transform.forward * zPerDistanceBetweenOrbs.Evaluate(GetMaxDistanceBetweenOrbs());
    Vector3 RelativePosInAnimation => initialAnimationPos - initialAnimationTargetPos;
    bool InitialAnimationPlaying => !followingOrbs;

    void Awake() {
        initialRot = transform.rotation;
        distanceLimit = zPerDistanceBetweenOrbs.keys.Max(key => key.time);
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
        initialAnimationTargetPos = GameManager.I.stage.SpawnPointCenter;
        initialAnimationPos = initialAnimationTargetPos + characterSelectionBonusPos + initialAnimationYBonus * Vector3.up;
        transform.position = initialAnimationPos;
        Rotate(0);
    }

    public void OnGameStart() {

        transform.DORotate(initialRot.eulerAngles, 0.8f).SetEase(Ease.InOutSine);
        followingOrbs = true;
    }

    void Rotate(float ratio) {
        transform.position = (Quaternion.Euler(
            Vector3.up * (initialAnimationSpinCount * 360 * (1 - ratio) + characterSelectionAngle)
        ) * RelativePosInAnimation + initialAnimationTargetPos).WithY(transform.position.y);
        transform.LookAt(initialAnimationTargetPos);
    }

    void Update(){
        if (GameManager.I.orbArray == null || !followingOrbs)
            return;
        transform.position = GetPos();
#if UNITY_EDITOR
        maxDistanceBetweenOrbs = GetMaxDistanceBetweenOrbs();
#endif
    }

    Vector3 GetOrbMidPoint(Orb[] orbArray) {
        return new Vector3(
            (orbArray.Max(p => p.transform.position.x) + orbArray.Min(p => p.transform.position.x)) / 2,
            0,
            (orbArray.Max(p => p.transform.position.z) + orbArray.Min(p => p.transform.position.z)) / 2
        );
    }

    Vector3 GetPos() {
        return Vector3.Lerp(transform.position, TargetPos, 1.2f * Time.deltaTime);
    }

    float GetMaxDistanceBetweenOrbs() {
        float ret = 0;
        for (int i = 0; i < GameManager.I.nonNullOrbArray.Length - 1; i++) {
            for (int j = i + 1; j < GameManager.I.nonNullOrbArray.Length; j++) {
                ret = Mathf.Max(ret, Mathf.Abs(Vector3.Distance(
                    GameManager.I.nonNullOrbArray[i].transform.position, GameManager.I.nonNullOrbArray[j].transform.position
                )));
            }
        }
        return ret;
    }

    void OnDrawGizmosSelected() {
        if (!Application.isPlaying || GameManager.I.orbArray == null)
            return;
        Gizmos.color = Color.blue.WithAlpha(0.5f);
        Gizmos.DrawSphere(PivotPos, 0.5f);
    }
}