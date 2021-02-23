using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SegmentEffect : MonoBehaviour {
    public CardType cardType;
    protected Segment segment;
    [SerializeField] protected AudioSource activationSfx;

    protected virtual void Awake() {
        segment = GetComponentInParent<Segment>();
    }

    public virtual void Activate() {
        gameObject.SetActive(true);
        if(activationSfx != null)
            activationSfx.Play();
    }

    public virtual void Deactivate() {
        gameObject.SetActive(false);
    }
}
