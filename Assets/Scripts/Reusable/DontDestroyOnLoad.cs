using UnityEngine;

/// <summary>
/// Apply DontDestroyOnLoad on Awake
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour {
	void Awake () {
        DontDestroyOnLoad(gameObject);
	}
}