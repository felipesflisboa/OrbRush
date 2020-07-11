using UnityEngine;
using UnityEngine.UI;

namespace Gamelogic.Extensions.Examples
{
	public class ClockExampleMain : GLMonoBehaviour
	{
		public Text clockCountText;
		public Text message;
		public Button pauseButton;

		private Clock clock;

		public void Start()
		{
			clock = new Clock();

			clock.OnClockExpired += () => { message.text = "Clock Expired"; };
			clock.OnSecondsChanged += () => { clockCountText.text = clock.TimeInSeconds.ToString(); };

			TogglePause();
		}

		public void Update()
		{
			clock.Update(Time.deltaTime);
		}

		public void ResetClock()
		{
			clock.Reset(10);
			message.text = "";
		}

		public void TogglePause()
		{
			if(clock.IsPaused)
			{
				clock.Unpause();
				pauseButton.GetComponentInChildren<Text>().text = "Pause";
			}
			else
			{
				clock.Pause();
				pauseButton.GetComponentInChildren<Text>().text = "Unpause";
			}
		}
	}
}