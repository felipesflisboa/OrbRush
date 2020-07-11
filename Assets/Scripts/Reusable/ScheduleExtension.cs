using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Allow Invoke (Schedule) and InvokeRepeating (ScheduleRepeating) with Yield instruction.
/// Version 1.2
/// </summary>
public static class ScheduleExtension{
	/// <summary>
	/// Invoke with a yield instruction and an Action.
	/// </summary>
	/// <example>
	/// <code>this.Invoke(new WaitForSeconds(1f), () => Debug.Log("Do action here"))</code>
	/// </example>
	/// <returns>
	/// Coroutine played.
	/// </returns>
    public static Coroutine Schedule(this MonoBehaviour monoBehaviour, YieldInstruction yieldInstruction, Action action){      
		return monoBehaviour.StartCoroutine(Coroutine(yieldInstruction, action));
	}

	public static Coroutine Schedule(this MonoBehaviour monoBehaviour, CustomYieldInstruction yieldInstruction, Action action){      
		return monoBehaviour.StartCoroutine(Coroutine(yieldInstruction, action));
	}

	public static Coroutine Schedule(this MonoBehaviour monoBehaviour, float time, Action action) {
		return monoBehaviour.Schedule(new WaitForSeconds(time), action);
	}

    public static Coroutine ScheduleAfterFrame(this MonoBehaviour monoBehaviour, Action action) {
        return monoBehaviour.StartCoroutine(CoroutineAfterFrame(action));
    }

    static IEnumerator Coroutine(YieldInstruction yieldInstruction, Action action){
		yield return yieldInstruction;
		action.Invoke();
	}

	static IEnumerator Coroutine(CustomYieldInstruction yieldInstruction, Action action){
		yield return yieldInstruction;
		action.Invoke();
    }

    static IEnumerator CoroutineAfterFrame(Action action) {
        yield return null;
        action.Invoke();
    }

    /// <summary>
    /// Invoke with a yield instruction, an Action and repeating after every new yield instruction.
    /// </summary>
    /// <example>
    /// <code>this.Invoke(new WaitForSeconds(1f), () => Debug.Log("Do action here"), new WaitForSeconds(.2f))</code>
    /// </example>
    /// <returns>
    /// Coroutine played.
    /// </returns>
    public static Coroutine ScheduleRepeating(
		this MonoBehaviour monoBehaviour, YieldInstruction yieldInstruction, Action action, YieldInstruction repeatYieldInstruction
	){      
		return monoBehaviour.StartCoroutine(Coroutine(yieldInstruction, action, repeatYieldInstruction));
	}

	public static Coroutine ScheduleRepeating(
		this MonoBehaviour monoBehaviour, CustomYieldInstruction yieldInstruction, Action action, CustomYieldInstruction repeatYieldInstruction
	){      
		return monoBehaviour.StartCoroutine(Coroutine(yieldInstruction, action, repeatYieldInstruction));
	}

	static IEnumerator Coroutine(YieldInstruction yieldInstruction, Action action, YieldInstruction repeatYieldInstruction){
        yield return yieldInstruction;
		while(true){
			action.Invoke();
			yield return repeatYieldInstruction;
		}
	}

	static IEnumerator Coroutine(CustomYieldInstruction yieldInstruction, Action action, CustomYieldInstruction repeatYieldInstruction){
		yield return yieldInstruction;
		while(true){
			action.Invoke();
			yield return repeatYieldInstruction;
		}
    }
}