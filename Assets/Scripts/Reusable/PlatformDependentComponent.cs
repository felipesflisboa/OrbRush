using UnityEngine;

/// <summary>
/// Destroy/disable GameObject if the platform is unavailable.
/// 
/// Version 1.2;
/// </summary>
public class PlatformDependentComponent : MonoBehaviour {
	public enum Call{
		None = 0, Awake, Start, Update, LateUpdate
    }
    public enum Action {
        None = 0, Disable, Destroy, DestroyImmediate
    }

	public Call call = Call.LateUpdate;
    public Action unavailableAction = Action.Disable; 
    public bool Windows;
    public bool MacOS;
    public bool Linux;
	public bool WebPlayer;
	public bool Android;
    public bool iOS;
    public bool WindowsPhone8;
    public bool WindowsStore;
	public bool WebGL;

	void Awake () {
		if(call == Call.Awake)
            Execute();
    }

	void Start () {
		if(call == Call.Start)
            Execute();
    }

	void Update () {
		if(call == Call.Update)
            Execute();
    }

	void LateUpdate () {
        if (call == Call.LateUpdate)
            Execute();
    }

    public void Execute() {
        if (IsAvailable())
            return;
        switch (unavailableAction) {
            case Action.Disable:
                gameObject.SetActive(false);
                break;
            case Action.Destroy:
                Destroy(gameObject);
                break;
            case Action.DestroyImmediate:
                DestroyImmediate(gameObject);
                break;
        }
    }

    bool IsAvailable() {
#if UNITY_STANDALONE_WIN
        return Windows;
#elif UNITY_STANDALONE_OSX
        return MacOS;
#elif UNITY_STANDALONE_LINUX
        return Linux;
#elif UNITY_WEBPLAYER
		return WebPlayer;
#elif UNITY_ANDROID
		return Android;
#elif UNITY_IOS
        return iOS;
#elif UNITY_WP8
        return WindowsPhone8;
#elif UNITY_WSA
        return WindowsStore;
#elif UNITY_WEBGL
        return WebGL;
#else
		return false;
#endif
    }
}