using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wait multiple conditions. Internally uses coroutines.
/// </summary>
/// <example>
/// new WaitMultiple(monoBehaviour, 1, new WaitForSeconds(1), new WaitWhile(() => !var)) // Wait until one of these two conditions finish.
/// </example>
public class WaitMultiple : CustomYieldInstruction {
    int currentMathCount = 0;
    int requiredMathCount = 0;
    MonoBehaviour activator;
    readonly List<Coroutine> coroutineList = new List<Coroutine>();

    public override bool keepWaiting {
        get {
            if (currentMathCount < requiredMathCount) {
                return true;
            } else {
                StopAllCoroutines();
                return false;
            }
        }
    }

    /// <param name="activator">MonoBehaviour to run the coroutine.</param>
    /// <param name="matchCount">How many conditions should return true to stop waiting.</param>
    /// <param name="instructionArray">YieldStruction array</param>
    public WaitMultiple(MonoBehaviour activator, int matchCount, params object[] instructionArray) {
        Initialize(activator, matchCount, instructionArray);
    }

    public WaitMultiple(MonoBehaviour activator, params object[] instructionArray) {
        Initialize(activator, 0, instructionArray);
    }

    void Initialize(MonoBehaviour activator, int matchCount, object[] instructionArray) {
        this.activator = activator;
        requiredMathCount = matchCount < 1 ? instructionArray.Length : matchCount;
        foreach (var instruction in instructionArray)
            coroutineList.Add(activator.StartCoroutine(InstructionRoutine(instruction)));
    }

    IEnumerator InstructionRoutine(object instruction) {
        yield return instruction;
        currentMathCount++;
    }

    void StopAllCoroutines() {
        if (activator == null)
            return;
        if (coroutineList.Count == 0)
            return;
        foreach (var coroutine in coroutineList)
            if (coroutine != null)
                activator.StopCoroutine(coroutine);
        coroutineList.Clear();
    }
}