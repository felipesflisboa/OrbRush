// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// A component that makes it easy to take screenshots, usually for development purposes.
	/// </summary>
	[HelpURL("http://gamelogic.co.za/documentation/extensions/?topic=html/T-Gamelogic.Extensions.ScreenshotTaker.htm")]
	[AddComponentMenu("Gamelogic/Extensions/ScreenshotTaker")]
	[ExecuteInEditMode]
	public sealed class ScreenshotTaker : Singleton<ScreenshotTaker>
	{
		#region Configuration

		[Tooltip("The key to use for taking a screenshot.")]
		[SerializeField]
		private KeyCode screenshotKey = KeyCode.Q;

		[Tooltip("The scale at which to take the screen shot.")]
		[Positive]
		[SerializeField]
		private int scale = 1;

		[Tooltip("The fist part of the file name")]
		[SerializeField]
		private string fileNamePrefix = "screen_";

		//[Tooltip("Set this to true to have screenshots taken periodically and specify the interval in seconds.")]
		[SerializeField]
		private OptionalFloat automaticScreenshotInterval = new OptionalFloat { UseValue = false, Value = 60f};
		
		[Tooltip("Objects to disable when taking a screenshot.")]
		[SerializeField]
		private GameObject[] dirtyObjects = new GameObject[0];

		private Dictionary<GameObject, bool> stateOfDirtyObjects;

		#endregion

		#region Unity Messages

		public void Start()
		{
			if (Application.isPlaying && automaticScreenshotInterval.UseValue)
			{
				if (dirtyObjects.Length > 0)
				{
					InvokeRepeating(TakeCleanImpl, automaticScreenshotInterval.Value, automaticScreenshotInterval.Value);
				}
				else
				{
					InvokeRepeating(TakeImpl, automaticScreenshotInterval.Value, automaticScreenshotInterval.Value);
				}
			}
		}

		public void Update()
		{
			if (Input.GetKeyDown(screenshotKey))
			{
				if (dirtyObjects.Length > 0)
				{
					TakeClean();
				}
				else
				{
					Take();
				}
			}
		}

		#endregion

		#region Public Methods

		[InspectorButton]
		public static void Take()
		{
			Instance.TakeImpl();
		}

		[InspectorButton]
		public static void TakeClean()
		{
			Instance.TakeCleanImpl();
		}

		#endregion

		#region Implementation

		private void TakeCleanImpl()
		{
			StartCoroutine(TakeCleanEnumerator());
		}

		private IEnumerator TakeCleanEnumerator()
		{
			stateOfDirtyObjects = new Dictionary<GameObject, bool>();

			foreach (var dirtyObject in dirtyObjects)
			{
				stateOfDirtyObjects.Add(dirtyObject, dirtyObject.activeSelf);
				dirtyObject.SetActive(false);
			}

			yield return new WaitForEndOfFrame();

			TakeImpl();

			yield return new WaitForEndOfFrame();

			foreach (var stateOfDirtyObject in stateOfDirtyObjects)
			{
				stateOfDirtyObject.Key.SetActive(stateOfDirtyObject.Value);
			}
		}

		private void TakeImpl()
		{
			string path = fileNamePrefix + DateTime.Now.Ticks + ".png";
#if UNITY_2017_1_OR_NEWER //mod
            ScreenCapture.CaptureScreenshot(path, scale);
#endif
        }

        #endregion
    }
}