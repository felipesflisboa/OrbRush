using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class QuickTest : MonoBehaviour{
    public Transform main;
    public Transform target;
    public Camera cam;
    //bool rotate;

    async void Start() {
        PlayAnim();

        while (true) {
            Debug.Log($"Loop! frameCount={Time.frameCount}");
            await new WaitForUpdate();
        }
    }

    Vector3 originalCamPos;

    async void PlayAnim() {
        float time = 3;
        float y = 5;
        originalCamPos = cam.transform.position;
        cam.transform.position = cam.transform.position.WithIncY(5);
        //rotate = true;
        Rotate(0);
        DOTween.To(Rotate, 0, 1, time*0.98f).SetEase(Ease.OutSine);
        await cam.transform.DOBlendableMoveBy(y * Vector3.down, time).SetEase(Ease.OutSine).WaitForCompletion(); 
        //rotate = false;
        //TODO restore rot and set pos
    }

    void Update(){
        Debug.Log($"Update! frameCount={Time.frameCount}");
        //Debug.Log(MathUtil.UnityAngleToNormal(MathUtil.GetAngle(main.transform.position.To2DXZ() - target.transform.position.To2DXZ())-90));
        /*
        Vector3 posRelative = target.InverseTransformPoint(cam.transform.position);
        posRelative = Quaternion.Euler(Vector3.up * 30 * Time.deltaTime) * posRelative;
        */
        //  cam.position 

        //if (rotate)
        //    Rotate(Time.deltaTime, false);
        //Debug.Log($"{MathUtil.GetAngle(main.transform.position.To2DXZ(), target.transform.position.To2DXZ())}\n{MathUtil.UnityAngleToNormal(MathUtil.GetAngle(main.transform.position.To2DXZ(), target.transform.position.To2DXZ()))}");
    }

    void Rotate (float proportion) {
        Vector3 posRelative = target.InverseTransformPoint(originalCamPos.WithY(cam.transform.position.y));
        posRelative = Quaternion.Euler(Vector3.up * 1200 * (1-proportion))  * posRelative;
        cam.transform.position = target.TransformPoint(posRelative);
        cam.transform.LookAt(target.position);
    }

    void Rotate2(float time, bool reverse) {
        Vector3 posRelative = target.InverseTransformPoint(cam.transform.position);
        posRelative = Quaternion.Euler(Vector3.up * 30 * time * (reverse ? -1 : 1)) * posRelative;
        cam.transform.position = target.TransformPoint(posRelative);
        cam.transform.LookAt(target.position);
        //TODO restore rotation
    }
}