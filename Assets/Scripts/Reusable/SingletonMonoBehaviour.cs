using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// Version 1.4
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour{
	private static T _instance;

	public static bool Active {
		get {
			return _instance != null;
		}
	}

    public static T Instance{
		get{
            if (_instance == null){
                _instance = (T) FindObjectOfType(typeof(T));
                if ( FindObjectsOfType(typeof(T)).Length > 1 ) {
                    Debug.LogError("[SingletonMonoBehaviour] Something went really wrong " +
                        " - there should never be more than 1 singletonMonoBehaviour!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }
			}

			return _instance;
		}
	}

	public static T I{
		get{
			return Instance;
		}
	}

	public void Close () {
		if (_instance != null)
			_instance = null;
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	protected virtual void OnDestroy () 
    {
          if (_instance != null && _instance.gameObject.scene.buildIndex != -1)
            _instance = null;
    }
}
