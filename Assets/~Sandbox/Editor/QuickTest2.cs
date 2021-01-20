using NUnit.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Profiling;

public class QuickTest2 {
    [Test]
    public void CheckEuler() {
        Quaternion q = Quaternion.Euler(Vector3.up * 90) * Quaternion.Euler(Vector3.up * 90);
        Assert.IsTrue(q == Quaternion.Euler(Vector3.up * 180));
        Debug.Log("Euler: "+q);
    }
}
