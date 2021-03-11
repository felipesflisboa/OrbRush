using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO SpaceController and MainController. SpaceController is the pivot
namespace BlueNebula.Intro {
    public class SpaceController : MonoBehaviour {
        Item[] itemArray;

        void Awake() {
            itemArray = GetComponentsInChildren<Item>();
            foreach (var item in itemArray)
                item.Setup(transform.position);
        }

        public void RefreshItems(float zoomMultiplier) {
            foreach (var item in itemArray)
                item.Refresh(zoomMultiplier);
        }
    }
}