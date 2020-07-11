using System.Collections;
using UnityEngine;
using Gamelogic.Extensions.Algorithms;

namespace Gamelogic.Extensions.Examples
{
	// This example shows how to use a state tracker to elegantly 
	// implement a loading message for async operations (such as, 
	// for example, WWW calls).
	public class LoadingExample : GLMonoBehaviour
	{
		#region Public Fields
		public GameObject loadingMessage;
		#endregion

		#region Private Fields
		private IGenerator<float> randomTimeGenerator;
		private StateTracker<int> stateTracker;
		private int eventID;
		#endregion

		#region Public Messages
		public void Start()
		{
			eventID = 0;
			randomTimeGenerator = Generator.UniformRandomFloat().Select(x => x * 5);
			loadingMessage.SetActive(false);
			stateTracker = new StateTracker<int>();

			stateTracker.OnStateActive += () => { loadingMessage.SetActive(true); };
			stateTracker.OnStateInactive += () => { loadingMessage.SetActive(false); };
		}
		#endregion

		#region Public Methods
		public void SimulateAsyncOperation()
		{
			StartCoroutine(SimulateAsyncOperationImpl());
		}
		#endregion

		#region Private Methods
		private IEnumerator SimulateAsyncOperationImpl()
		{
			float time = randomTimeGenerator.Next();
			int currentEventID = eventID;
			var token = stateTracker.StartState(eventID++);

			GLDebug.Log("Event Info", "Event started (" + currentEventID + ") and will finish in " + time + " seconds.");

			yield return new WaitForSeconds(time);

			GLDebug.Log("Event Info", "Event stopped (" + currentEventID + ").");
			stateTracker.StopState(token);
		}
		#endregion
	}
}
