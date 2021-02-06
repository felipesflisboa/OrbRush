using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MenuCameraController : MonoBehaviour {
    [SerializeField] float distanceToOrbs;
    [SerializeField] float lerpDeltatimeMultiplier = 1; //remove
    [SerializeField] Vector3 bonusPos;
    Orb targetOrb;

    Vector3 TargetPos => targetOrb.transform.position - transform.forward * distanceToOrbs + bonusPos;

    void Start() {
        //transform.DORotate(initialRot.eulerAngles, 0.8f).SetEase(Ease.InOutSine);
        targetOrb = GetRandomOrb();
    }

    void Update(){
        transform.position = TargetPos;
    }

    Orb GetRandomOrb() {
        Orb[] orbArray = FindObjectsOfType<Orb>();
        return orbArray[Mathf.FloorToInt(Random.value* orbArray.Length)];
    }

    //remove
    Vector3 GetPos() {
        return Vector3.Lerp(transform.position, TargetPos, lerpDeltatimeMultiplier * Time.deltaTime);
    }
}