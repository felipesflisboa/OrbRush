using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace AdvertisementCaller {
    /// <summary>
    /// Handle ADS calls
    /// 
    /// Version 2.0.
    /// </summary>
    public class UnityCaller : BaseCaller, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {
        public override bool IsReady => Advertisement.isSupported && IsLoaded;
        public override bool IsLoaded => loaded;
        string gameId;
        bool testMode;
        bool loaded;

        string AdId {
            get {
#if UNITY_ANDROID
                return "video";
#else
                return null;
#endif
            }
        }

        public UnityCaller(string pId, bool pTestMode) : base(pId, pTestMode) {
            gameId = pId;
            testMode = pTestMode;
            Initialize();
        }

        void Initialize() {
            Advertisement.Initialize(gameId, testMode, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
            Debug.LogWarning(message);
        }

        public void OnInitializationComplete() {
            Load();
        }

        public override void Load() {
            if (!Advertisement.isInitialized) {
                Initialize();
                return;
            }
            base.Load();
            Advertisement.Load(AdId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId) {
            loaded = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
            loaded = false;
            Debug.LogWarning(message);
        }

        /// <summary>
        /// Show a video ADS.
        /// </summary>
        /// <param name="callback">Callback. True if was viewed until the end, false if was skipped, or null if wasn't showed.</param>
        public override void Show(Action<Result> callback) {
            base.Show(callback);
            Advertisement.Show(AdId, this);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
            loaded = false;
            Debug.LogWarning(message);
            showCallback?.Invoke(Result.Failed);
        }

        public void OnUnityAdsShowStart(string placementId) {
            loaded = false;
        }

        public void OnUnityAdsShowClick(string placementId) {}

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
            switch (showCompletionState) {
                case UnityAdsShowCompletionState.COMPLETED: showCallback?.Invoke(Result.Finished); break;
                case UnityAdsShowCompletionState.SKIPPED:   showCallback?.Invoke(Result.Skipped); break;
                case UnityAdsShowCompletionState.UNKNOWN:   showCallback?.Invoke(Result.Failed); break;
            }
            Load();
        }
    }
}