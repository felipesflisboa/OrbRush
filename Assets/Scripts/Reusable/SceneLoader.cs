using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads a scene async. Sets a minimum time for loading.
/// 
/// Good to create loading screen with logos or informations. Can be combined with loading bars using onProgressChangeCallback.
/// Version 2.2
/// </summary>
public class SceneLoader : MonoBehaviour {
	public static string levelName; // Here goes the next level to load name

	public float minimumTimeForWait = 1f;
	[Tooltip("Call on scene start. Else, Activate should be called manually")] public bool callOnStart = true;
	[Tooltip("Do a resources unload")] public bool clearResources = false;
    [Tooltip("When clearResources==true. Clear resources weight in load callback"), SerializeField] float clearResourcesWeight = 0.2f;

	[Serializable] public class LoadFinishedCallback : UnityEngine.Events.UnityEvent{}
	[Tooltip("When empty, just load the scene")] public LoadFinishedCallback loadFinishedCallback;

	[Serializable] public class OnProgressChangeCallback : UnityEngine.Events.UnityEvent<float>{}
	[Tooltip("Receives the progress from 0-1f")] public OnProgressChangeCallback onProgressChangeCallback;

    public bool loadingDone { private set; get; }
    internal bool allowSceneActivation; // Only works if there is no callbacks
    AsyncOperation loadSceneOperation;
	int originalSleepTimeout;
	float beforeLoadTime;

	void Start(){
		if(callOnStart)
			Activate();
	}

	public void Activate(){
		if(string.IsNullOrEmpty(levelName)){
			Debug.LogError("levelName is empty!");
			return;
		}
		StartCoroutine(MainRoutine());
	}

	IEnumerator MainRoutine() {
		if(loadFinishedCallback.GetPersistentEventCount()==0 && allowSceneActivation)
			loadFinishedCallback.AddListener(() => allowSceneActivation = true);
		if(!clearResources)
			clearResourcesWeight = 0f;
		originalSleepTimeout = Screen.sleepTimeout;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		beforeLoadTime = Time.realtimeSinceStartup;
		if(clearResources){
			Action<float> onProgressCallback = null;
			if(clearResourcesWeight>0f && onProgressChangeCallback.GetPersistentEventCount()>0)
				onProgressCallback = progress => onProgressChangeCallback.Invoke(progress*clearResourcesWeight);
			yield return ClearResources(onProgressCallback);
			yield return new WaitForEndOfFrame();
        } else {
            yield return null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            yield return null;
        }
		yield return Load();
	}
		
	/// <summary>
	/// Unload unused resources. Can be called without the use of this class.
	/// </summary>
	/// <param name="onProgressCallback">Callback to be called on every interval with the progress value between 0-1.</param>
	/// <param name="finishedCallback">Callback to be called when clear ends.</param>
	public static IEnumerator ClearResources(Action<float> onProgressCallback = null, Action finishedCallback = null) {
		yield return null;
		AsyncOperation unloadUnusedAssets = Resources.UnloadUnusedAssets ();
		while(!unloadUnusedAssets.isDone){
			yield return null;
			if(onProgressCallback != null)
				onProgressCallback.Invoke(unloadUnusedAssets.progress);
        }
        yield return null;
		if(finishedCallback != null)
			finishedCallback();
	}

	IEnumerator Load() {
		loadSceneOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName);
		loadSceneOperation.allowSceneActivation = false;
		while(loadSceneOperation.progress < 0.9f || Time.realtimeSinceStartup-beforeLoadTime<minimumTimeForWait){
			if(clearResourcesWeight != 1f){
				float progress = clearResourcesWeight+loadSceneOperation.progress*(1f-clearResourcesWeight);
				onProgressChangeCallback.Invoke(progress);
			}
			yield return null;
		}
		if(clearResourcesWeight != 1f)
			onProgressChangeCallback.Invoke(1f);
		yield return new WaitForEndOfFrame();
		loadingDone = true;
		Screen.sleepTimeout = originalSleepTimeout;
		loadFinishedCallback.Invoke();
	}

	void Update(){
		if(loadingDone && !loadSceneOperation.allowSceneActivation && allowSceneActivation)
            loadSceneOperation.allowSceneActivation = true;
	}
}