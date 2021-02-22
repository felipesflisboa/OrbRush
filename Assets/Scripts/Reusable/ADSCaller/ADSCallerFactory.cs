using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ADSCallerFactory{
    public static ADSCaller Create(string id){
#if UNITY_ADS
        return new UnityADSCaller(id);
#endif
        return null;
    }
}
