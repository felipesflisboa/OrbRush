using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class Squirt : SegmentEffect {
    public override void Activate() {
        base.Activate();
        segment.ApplyColor(new Color32(0xD0, 0xD0, 0xFF, 0xFF));
    }

    public void ApplyEffectInPlayer(Orb orb) {
        orb.rigidBody.velocity *= 1f + 0.4f * (orb.element == Element.Water ? 0.75f : 1);
    }
}