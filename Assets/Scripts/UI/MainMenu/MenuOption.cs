using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour {
    public Button button;
    [SerializeField] Transform modelTransform;

    void Update() {
        modelTransform.localEulerAngles += Vector3.up * 8 * Time.deltaTime;
    }
}
