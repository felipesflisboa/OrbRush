using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvertisementCaller;

/// <summary>
/// Handle game Advertisement logic
/// </summary>
public class AdvertisementHandler{
    static AdvertisementHandler instance;

    BaseCaller caller;

    const string ADS_RUN_COUNT_KEY = "ADS Count";
    const int MAX_RUN_COUNT_WITHOUT_ADS = 10;

    public static AdvertisementHandler I {
        get {
            if (instance == null)
                instance = new AdvertisementHandler();
            return instance;
        }
    }

    string GameId {
        get {
#if UNITY_ADS && UNITY_ANDROID
            return "4022189";
#else
            return null;
#endif
        }
    }

    public bool CanPlayADS => caller != null && GetRunCountWithoutADS() >= MAX_RUN_COUNT_WITHOUT_ADS && caller.IsReady;

    public void Initialize() {
        if (caller != null)
            return;
        caller = Factory.Create(GameId);
    }

    public void Show(Action callback) {
        SetRunCountWithoutADS(0);
        caller.Show(callback);
    }

    public void TryToPlay(Action callback) {
        if (CanPlayADS)
            Show(callback);
        else
            callback?.Invoke();
    }

    public int GetRunCountWithoutADS() => PlayerPrefs.GetInt(ADS_RUN_COUNT_KEY, 0);
    public void SetRunCountWithoutADS(int value) => PlayerPrefs.SetInt(ADS_RUN_COUNT_KEY, value);
    public void AddRunCountWithoutADS(int value = 1) => SetRunCountWithoutADS(GetRunCountWithoutADS() + value);
}
