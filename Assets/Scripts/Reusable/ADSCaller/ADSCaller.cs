using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle ADS calls
/// </summary>
public abstract class ADSCaller {
    /// <summary>
    /// ADS Result.
    /// Override other frameworks enums to have total package independency.
    /// </summary>
    public enum Result {
        None = 0, Failed, Skipped, Finished
    }

    protected Action<Result> showCallback;

    public virtual bool IsReady => false;

    public ADSCaller(string id) { }

    public void Show(Action callback) {
        Action<Result> newCallback = (result) => callback();
        Show(newCallback);
    }

    /// <summary>
    /// Show a video ADS.
    /// </summary>
    /// <param name="callback">Callback. True if was viewed until the end, false if was skipped, or null if wasn't showed.</param>
    public virtual void Show(Action<Result> callback) {
        showCallback = callback;
    }
}