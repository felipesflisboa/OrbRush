using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNebula.Intro {
    public class Item : MonoBehaviour {
        [Tooltip("Bigger = More distant, less effect. Exponential."), SerializeField] float distanceLevel = 1;
        Vector2 initialPosWithoutPivot;
        Vector3 initialLocalScale;
        Vector2 pivotPos;
        float distanceMultiplier;

        public void Setup(Vector2 pPivotPos) {
            pivotPos = pPivotPos;
            initialPosWithoutPivot = (Vector2)transform.position - pivotPos;
            initialLocalScale = transform.transform.localScale;
        }

        public void Refresh(float zoomMultiplier) {
            distanceMultiplier = Mathf.Pow(1f/zoomMultiplier, 1f / distanceLevel);
            transform.position = (Vector3)pivotPos + new Vector3(
                initialPosWithoutPivot.x/distanceMultiplier, initialPosWithoutPivot.y/distanceMultiplier, transform.position.z
            );
            transform.localScale = initialLocalScale/distanceMultiplier;
        }
    }
}
