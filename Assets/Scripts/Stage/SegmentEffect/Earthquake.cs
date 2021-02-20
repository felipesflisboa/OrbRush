using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class Earthquake : SegmentEffect{
    public override void Activate() {
        base.Activate();
        foreach (var platform in segment.earthquakePlatformArray)
            ActivatePlatformMovement(platform);
    }

    void ActivatePlatformMovement(Transform platform) {
        platform.gameObject.SetActive(true);
        DOTween.Sequence().Append(
            platform.transform.DOMoveY(0.2f, 0.3f).SetRelative()
        ).Append(
            platform.transform.DOMoveY(-0.2f, 0.001f).SetRelative()
        ).SetLoops(-1);
    }
}