using System;

namespace AdvertisementCaller {
    /// <summary>
    /// Handle Advertisement calls
    /// 
    /// Version 2.0.
    /// </summary>
    public abstract class BaseCaller {
        protected Action<Result> loadCallback;
        protected Action<Result> showCallback;

        public virtual bool IsReady => false;
        public virtual bool IsLoaded => false;

        public BaseCaller(string id, bool testMode) { }

        public void Show(Action callback) {
            Action<Result> newCallback = (result) => callback();
            Show(newCallback);
        }

        public virtual void Load() {}

        /// <summary>
        /// Show a video Advertisement.
        /// </summary>
        /// <param name="callback">Callback. True if was viewed until the end, false if was skipped, or null if wasn't showed.</param>
        public virtual void Show(Action<Result> callback) {
            showCallback = callback;
        }
    }
}