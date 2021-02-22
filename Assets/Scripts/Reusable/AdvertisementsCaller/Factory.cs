﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvertisementCaller {
    public static class Factory {
        public static BaseCaller Create(string id) {
#if UNITY_ADS
            return new UnityCaller(id);
#endif
            return null;
        }
    }
}