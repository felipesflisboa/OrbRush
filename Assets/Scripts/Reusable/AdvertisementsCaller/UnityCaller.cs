using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace AdvertisementCaller {
#if UNITY_ADS
    /// <summary>
    /// Handle ADS calls
    /// </summary>
    public class UnityCaller : BaseCaller, IUnityAdsListener {
        public override bool IsReady => Advertisement.isSupported && Advertisement.IsReady();

        public UnityCaller(string id) : base(id) {
            Advertisement.Initialize(id);
            Advertisement.AddListener(this);
        }

        /// <summary>
        /// Show a video ADS.
        /// </summary>
        /// <param name="callback">Callback. True if was viewed until the end, false if was skipped, or null if wasn't showed.</param>
        public override void Show(Action<Result> callback) {
            base.Show(callback);
            Advertisement.Show();
        }

        void OnAdsDidFinish(Action<Result> callback, ShowResult result) {
            switch (result) {
                case ShowResult.Finished: callback(Result.Finished); break;
                case ShowResult.Skipped: callback(Result.Skipped); break;
                case ShowResult.Failed: callback(Result.Failed); break;
            }
        }

        public void OnUnityAdsReady(string placementId) { }
        public void OnUnityAdsDidStart(string placementId) { }

        public void OnUnityAdsDidError(string message) {
            Debug.LogWarning(message);
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            switch (showResult) {
                case ShowResult.Finished: showCallback?.Invoke(Result.Finished); break;
                case ShowResult.Skipped: showCallback?.Invoke(Result.Skipped); break;
                case ShowResult.Failed: showCallback?.Invoke(Result.Failed); break;
            }
        }
    }
#else
    public class UnityADSCaller : ADSCaller {
        public UnityADSCaller(string id) : base(id) { }
    }
#endif
}