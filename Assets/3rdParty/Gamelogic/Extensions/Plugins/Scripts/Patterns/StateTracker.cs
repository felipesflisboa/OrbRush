using System;
using System.Linq;
using System.Collections.Generic;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Tracks a number of states. Events are raised when any state is started, and when all has stopped.
	/// </summary>
	/// <typeparam name="TStateData">The type of the t state data.</typeparam>
	/// <remarks>A loading bar is a typical use case. The loading bar should be displayed
	/// when any of a number of load processes has started; it should be removed when all
	/// has stopped. Another use-case is keeping track of poisoning, when the poisoning
	/// can be caused by a number of poisons, and each poison is cured independently.
	/// </remarks>
	public class StateTracker<TStateData>
	{
		private class StateToken : IStateToken<TStateData>
		{
			private readonly TStateData state;

			public StateToken(TStateData state)
			{
				this.state = state;
			}

			public TStateData State
			{
				get { return state; }
			}
		}

		/// <summary>
		/// Gets a value indicating whether this tracker is active, that is, whether
		/// any state has been started that has not been stopped.
		/// </summary>
		/// <value><c>true</c> if this tracker is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return activeTokens.Any(); }
		}

		/// <summary>
		/// Occurs when this tracker is inactive and a state is started (so that this tracker becomes active).
		/// </summary>
		public event Action OnStateActive;

		/// <summary>
		/// Occurs when all active states are stopped, that is, when this tracker is active and becomes inactive.
		/// </summary>
		public event Action OnStateInactive;

		private readonly List<StateToken> activeTokens;

		/// <summary>
		/// Returns all the active tokens: tokens returned when states has been started that has not yet
		/// been stopped.
		/// </summary>
		/// <value>The active tokens.</value>
		public IEnumerable<IStateToken<TStateData>> ActiveTokens
		{
			get { return activeTokens.Cast<IStateToken<TStateData>>(); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StateTracker{TStateData}"/> class.
		/// </summary>
		public StateTracker()
		{
			activeTokens = new List<StateToken>();
		}

		/// <summary>
		/// Starts a state, and returns a token that can be used to stop it again.
		/// </summary>
		/// <param name="stateData">Custom state data. This is useful in cases
		/// where all the active states needs to be examined. For example, this data
		/// can be used to identify states externally.</param>
		/// <returns>A token that wraps the custom state data and can be used to stop
		/// the state started with this method.</returns>
		public IStateToken<TStateData> StartState(TStateData stateData)
		{
			var token = new StateToken(stateData);

			if (!IsActive)
			{
				if (OnStateActive != null)
				{
					OnStateActive();
				}
			}

			activeTokens.Add(token);

			return token;
		}

		/// <summary>
		/// Stops the state associated with the token. The token must be one that was
		/// returned when the state was started.
		/// </summary>
		/// <param name="token">The token of the state to stop.</param>
		/// <exception cref="ArgumentNullException">token</exception>
		/// <exception cref="InvalidOperationException">
		/// The given token is not from this state tracker
		/// or
		/// the given token is not active
		/// </exception>
		public void StopState(IStateToken<TStateData> token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}

			if (!(token is StateToken))
			{
				throw new InvalidOperationException("The given token is not from this state tracker");
			}

			if (!activeTokens.Contains((StateToken)token))
			{
				throw new InvalidOperationException("The given token is not active");
			}

			activeTokens.Remove((StateToken)token);

			if (!IsActive)
			{
				if (OnStateInactive != null)
				{
					OnStateInactive();
				}
			}
		}
	}
}
