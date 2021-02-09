using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour {
    public Button button;
    [SerializeField] Transform modelTransform;
    [SerializeField] float rotateSpeed;
    [SerializeField] float hoverRotateSpeed;
    bool isHover;

    void Awake() {
        InitializeEvents();
    }

    void InitializeEvents() {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        AddEvent(eventTrigger, EventTriggerType.PointerEnter, () => isHover = true);
        AddEvent(eventTrigger, EventTriggerType.PointerExit, () => isHover = false);
    }

    void OnEnable() {
        modelTransform.localRotation = Quaternion.identity;
    }

    void Update() {
        modelTransform.localEulerAngles += Vector3.up * (isHover ? hoverRotateSpeed : rotateSpeed) * Time.deltaTime;
    }

    void AddEvent(EventTrigger eventTrigger, EventTriggerType type, System.Action callback) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((ed) => callback());
        eventTrigger.triggers.Add(entry);
    }
}
