using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class Cyclone : MonoBehaviour{
    const float MAX_DISTANCE_FROM_ORB = 12;
    const float FORCE_INTENSITY = 350;

    void FixedUpdate() {
        ApplyEffect();
    }

    void ApplyEffect() {
        foreach (var orb in GameManager.I.nonNullOrbArray) {
            if (orb.element == Element.Air || Vector3.Distance(orb.transform.position.To2DXZ(), transform.position.To2DXZ()) > MAX_DISTANCE_FROM_ORB)
                continue;
            orb.rigidBody.AddForce(
                MathUtil.UnityAngleToNormal(GetAngleToPosition(orb.transform.position)) * FORCE_INTENSITY * Time.deltaTime, ForceMode.Acceleration
            );
        }
    }

    float GetAngleToPosition(Vector3 posV3) => MathUtil.GetAngle(transform.position.To2DXZ() - posV3.To2DXZ()) - 90;
}
