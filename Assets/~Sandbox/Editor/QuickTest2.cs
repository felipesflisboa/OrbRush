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

    [Test] public void TestFormula1() => TestFormula(1, 1, 1);
    [Test] public void TestFormula2() => TestFormula(1, 4, 4);
    [Test] public void TestFormula3() => TestFormula(2, 1, 1);
    [Test] public void TestFormula4() => TestFormula(2, 4, 2);
    [Test] public void TestFormula5() => TestFormula(3, 8, 2);

    void TestFormula(int d, int zoom, int expected) {
        Assert.AreEqual(expected, Mathf.Pow(zoom, 1f/d));
    }
}
