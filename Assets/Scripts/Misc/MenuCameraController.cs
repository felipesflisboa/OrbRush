using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuCameraController : MonoBehaviour {
    [Positive, SerializeField] float minDistanceToOrbs;
    [Positive, SerializeField] float maxDistanceToOrbs;
    float distanceToOrbs;
    [SerializeField] Vector3 bonusPos;
    [SerializeField] Vector3 eulerVariation;
    [NonNegative, SerializeField] float rotateRoundDuration = 10f;
    [NonNegative, SerializeField] float zoomRoundDuration = 10f;
    Orb targetOrb;

    Vector3 TargetPos => targetOrb.transform.position - transform.forward * distanceToOrbs + bonusPos;

    void Start() {
        targetOrb = GetRandomOrb();
        InitializeRotation();
        InitializeZoom();
    }

    void InitializeRotation() {
        transform.eulerAngles -= eulerVariation / 2f;
        transform.DORotate(transform.eulerAngles + eulerVariation, rotateRoundDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void InitializeZoom() {
        distanceToOrbs = minDistanceToOrbs;
        DOTween.To(
            () => distanceToOrbs, value => distanceToOrbs = value, maxDistanceToOrbs, zoomRoundDuration
        ).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void Update(){
        transform.position = TargetPos;
    }

    Orb GetRandomOrb() {
        Orb[] orbArray = FindObjectsOfType<Orb>();
        return orbArray[Mathf.FloorToInt(Random.value* orbArray.Length)];
    }
}