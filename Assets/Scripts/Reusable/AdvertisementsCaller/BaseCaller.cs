using System;

namespace AdvertisementCaller {
    /// <summary>
    /// Handle Advertisement calls
    /// </summary>
    public abstract class BaseCaller {
        protected Action<Result> showCallback;

        public virtual bool IsReady => false;

        public BaseCaller(string id) { }

        public void Show(Action callback) {
            Action<Result> newCallback = (result) => callback();
            Show(newCallback);
        }

        /// <summary>
        /// Show a video Advertisement.
        /// </summary>
        /// <param name="callback">Callback. True if was viewed until the end, false if was skipped, or null if wasn't showed.</param>
        public virtual void Show(Action<Result> callback) {
            showCallback = callback;
        }
    }
}