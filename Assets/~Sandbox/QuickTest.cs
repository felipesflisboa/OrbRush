using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class QuickTest : MonoBehaviour{
    public Transform main;
    public Transform target;

    async void Start() {
        while (true) {
            Debug.Log($"Loop! frameCount={Time.frameCount}");
            await new WaitForUpdate();
        }
    }

    void Update()
    {
        Debug.Log($"Update! frameCount={Time.frameCount}"); 
        //Debug.Log(MathUtil.UnityAngleToNormal(MathUtil.GetAngle(main.transform.position.To2DXZ() - target.transform.position.To2DXZ())-90));


        //Debug.Log($"{MathUtil.GetAngle(main.transform.position.To2DXZ(), target.transform.position.To2DXZ())}\n{MathUtil.UnityAngleToNormal(MathUtil.GetAngle(main.transform.position.To2DXZ(), target.transform.position.To2DXZ()))}");
    }
}
