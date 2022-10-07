using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvertisementCaller {
    /// <summary>
    /// Factory class for Caller.
    /// 
    /// Version 2.0.
    /// </summary>
    public static class Factory {
        public static BaseCaller Create(string id, bool testMode=false) {
            if (id != null && UnityEngine.Advertisements.Advertisement.isSupported)
                return new UnityCaller(id, testMode);
            return null;
        }
    }
}
