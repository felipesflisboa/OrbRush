using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class Cyclone : SegmentEffect {
    const float MAX_DISTANCE_FROM_ORB = 12;
    const float MIN_TIME_FROM_LAST_TELEPORT = 5;
    const float FORCE_INTENSITY = 550;

    public override void Activate() {
        base.Activate();
        transform.position = GameManager.I.GetOrb(Element.Air).transform.position.WithY(transform.position.y);
    }

    void FixedUpdate() {
        ApplyEffect();
    }

    void ApplyEffect() {
        foreach (var orb in GameManager.I.nonNullOrbArray) {
            if (!CanAffectOrb(orb))
                continue;
            orb.rigidBody.AddForce(
                MathUtil.UnityAngleToNormal(GetAngleToPosition(orb.transform.position)) * FORCE_INTENSITY * Time.deltaTime,
                ForceMode.Acceleration
            );
        }
    }

    bool CanAffectOrb(Orb orb) {
        return (
            orb.element != Element.Air &&
            (orb.lastTeleportTime==0 || MIN_TIME_FROM_LAST_TELEPORT < (Time.timeSinceLevelLoad - orb.lastTeleportTime)) &&
            Vector3.Distance(orb.transform.position.To2DXZ(), transform.position.To2DXZ()) < MAX_DISTANCE_FROM_ORB
        );
    }

    float GetAngleToPosition(Vector3 posV3) => MathUtil.GetAngle(transform.position.To2DXZ() - posV3.To2DXZ()) - 90;
}