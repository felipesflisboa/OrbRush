using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{
    public void OnClick() {
        Debug.Log("OnClick");
        GameManager.I.playerArray[1].Boost();
        Destroy(gameObject);
    }
}
