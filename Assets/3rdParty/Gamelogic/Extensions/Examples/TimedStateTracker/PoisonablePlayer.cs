using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Gamelogic.Extensions.Algorithms;

namespace Gamelogic.Extensions.Examples
{
	// This is an example of using a timed state tracker to track a poisonous state.
	// The player can be poisoned with different poisons.Each poisson is cured
	// automatically after a number of seconds, but can also be cured when the
	// appropriate antidote is drank.
	public class PoisonablePlayer : GLMonoBehaviour
	{
		#region Public Fields
		[Header("Gamelogic")]
		public int secondsBeforeCured = 2;

		[Header("UI")]
		public Image image;
		public Color normalColor;
		public Color poisonColor;
		#endregion

		#region Private Fields
		private TimedStateTracker<Poison> poisonTracker;
		#endregion

		#region Messages
		public void Start()
		{
			//We create an state tracker - in this case, to keep track of 
			//the states of a player being poisoned or not with a variety of poisons.
			//We use a timed state tracker, because after a certain amount of time
			//the player will be automatically cured.
			poisonTracker = new TimedStateTracker<Poison>();

			//We have to register two events. In this case, what to
			//do when the player is poisoned with even one poison,
			//and what to do if the player is completely cured of 
			//all poisons.
			poisonTracker.OnStateActive += MakeColorGreen;
			poisonTracker.OnStateInactive += MakeColorNormal;
		}
		public void Update()
		{
			poisonTracker.Update(Time.deltaTime);
		}
		#endregion

		#region Public Methods
		public void EatPoison(Poison poison)
		{
			//Adds a new active poison that will be cured
			//in the given number of seconds if no antidote is drank.

			//A token is returned, and normally we would use this to stop the state.
			//However, in this case we want to stop all poisson states of the same type,
			//so we filter through the complete list instead when we eat an antidote.
			poisonTracker.StartState(poison, secondsBeforeCured, () => { });
		}

		public void EatAntidote(Poison poison)
		{
			//Find all the active poisons that can be cured by this antidote
			var statesToStop = poisonTracker.ActiveTokens.Where(token => token.State == poison).ToList();

			//And remove them
			foreach (var state in statesToStop)
			{
				poisonTracker.StopState(state);
			}
		}
		#endregion

		#region Private methods
		private void MakeColorNormal()
		{
			image.color = normalColor;
		}

		private void MakeColorGreen()
		{
			image.color = poisonColor;
		}
		#endregion
	}
}