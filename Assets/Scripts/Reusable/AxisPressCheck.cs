using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check when an axis is pressed, but wait for time intervals.
/// Good do be used on menus. Work with gamepad stick.
/// 
/// To work, call Update every frame.
/// Version 1.1
/// </summary>
public class AxisPressCheck {
	public string axis { get; private set; }
	public float interval { get; private set; }

	public bool triggerFrame { get; private set; }

	float timeSpend;
	bool currentPressing;

	bool Pressed {
		get {
			return !Mathf.Approximately(Input.GetAxisRaw(axis), 0f);
		}
	}

	public AxisPressCheck(string pAxis, float pInterval) {
		axis = pAxis;
		interval = pInterval;
	}

	public void Update () {
		if (currentPressing) {
			if (Pressed) {
				timeSpend += Time.deltaTime;
				if (timeSpend >= interval) {
					timeSpend -= interval;
					triggerFrame = true;
				} else {
					triggerFrame = false;
				}
			} else {
				currentPressing = false;
				triggerFrame = false;
			}
		} else {
			if (Pressed) {
				timeSpend = 0f;
				currentPressing = true;
				triggerFrame = true;
			}
		}
	}

    public float GetAxisOnPress() {
        return triggerFrame ? Input.GetAxis(axis) : 0;
    }
}