using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// The same as StateTracker, but states can also time out. 
	/// </summary>
	/// <typeparam name="TStateData">The type of the t state data.</typeparam>
	/// <remarks>Time-outs are managed by this class: when the state times out, 
	/// it is stopped, and an event is raised. This tracker must be updated (typically
	/// in a mono-behaviour Update method).</remarks>
	public class TimedStateTracker<TStateData>
	{
		#region Private Fields
		private readonly StateTracker<TStateData> tracker;
		private readonly Dictionary<IStateToken<TStateData>, Clock> clocks;
		private List<IStateToken<TStateData>> tokensCopy;
		#endregion

		/// <summary>
		/// Gets a value indicating whether this tracker is active, that is, whether
		/// any state has been started that has not been stopped.
		/// </summary>
		/// <value><c>true</c> if this tracker is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return tracker.IsActive; }
		}

		/// <summary>
		/// Occurs when this tracker is inactive and a state is started (so that this tracker becomes active).
		/// </summary>
		public event Action OnStateActive
		{
			add { tracker.OnStateActive += value; }
			remove { tracker.OnStateActive -= value; }
		}

		/// <summary>
		/// Occurs when all active states are stopped, that is, when this tracker is active and becomes inactive.
		/// </summary>
		public event Action OnStateInactive
		{
			add { tracker.OnStateInactive += value; }
			remove { tracker.OnStateInactive -= value; }
		}

		/// <summary>
		/// Returns all the active tokens: tokens returned when states has been started that has not yet
		/// been stopped.
		/// </summary>
		/// <value>The active tokens.</value>
		public IEnumerable<IStateToken<TStateData>> ActiveTokens
		{
			get { return tracker.ActiveTokens; }
		}

		/// <summary>
		/// Gets a value indicating whether this tracker is paused.
		/// </summary>
		/// <value><c>true</c> if this tracker is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused
		{
			get; private set;
		}

		/// <summary>
		/// Updates this tracker with the specified current time.
		/// </summary>
		/// <param name="deltaTime">The current delta time.</param>
		public void Update(float deltaTime)
		{
			//We can remove a clock during this iteration
			//from the dictionary, therefore we do not
			//iterate directly of the dictionary values.
			foreach (var token in tokensCopy)
			{
				clocks[token].Update(deltaTime);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimedStateTracker{TStateData}"/> class.
		/// </summary>
		public TimedStateTracker()
		{
			tracker = new StateTracker<TStateData>();
			clocks = new Dictionary<IStateToken<TStateData>, Clock>();
			tokensCopy = ActiveTokens.ToList();
		}

		/// <summary>
		/// Starts a state, and returns a token that can be used to stop it again.
		/// </summary>
		/// <param name="stateData">Custom state data. This is useful in cases
		/// where all the active states needs to be examined. For example, this data
		/// can be used to identify states externally.</param>
		/// <param name="maxTime">The maximum amount of time this state should survive past the current time.</param>
		/// <param name="onTimeOut">The action to perform when timing out.</param>
		/// <returns>A token that wraps the custom state data and can be used to stop 
		/// the state started with this method.</returns>
		/// <remarks>For a state to time out, it is necessary for the Update method to
		/// be called regularly (for example, in a MonoBehaviours Update method).</remarks>
		public IStateToken<TStateData> StartState(
			TStateData stateData,
			float maxTime,
			Action onTimeOut)
		{
			var token = tracker.StartState(stateData);
			var clock = new Clock();

			clock.OnClockExpired += () =>
			{
				StopState(token);

				if (onTimeOut != null)
				{
					onTimeOut();
				}
			};

			clock.Reset(maxTime);

			if (!IsPaused)
			{
				clock.Unpause();
			}

			clocks[token] = clock;
			tokensCopy = ActiveTokens.ToList();
			return token;
		}

		/// <summary>
		/// Starts a state, and returns a token that can be used to stop it again.
		/// </summary>
		/// <param name="stateData">Custom state data. This is useful in cases
		/// where all the active states needs to be examined. For example, this data
		/// can be used to identify states externally.</param>
		/// <param name="maxTime">The maximum amount of time this state should survive past the current time.</param>
		/// <returns>A token that wraps the custom state data and can be used to stop 
		/// the state started with this method.</returns>
		/// <remarks>For a state to time out, it is necessary for the Update method to
		/// be called regularly (for example, in a MonoBehaviours Update method).</remarks>
		public IStateToken<TStateData> StartState(
			TStateData stateData,
			float maxTime)
		{
			return StartState(stateData, maxTime, DoNothing);
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
			if(token == null)
				throw new ArgumentNullException("token");

			if (!clocks.ContainsKey(token))
				throw new InvalidOperationException("The token was not obtain by this tracker or already has been stopped.");

			clocks.Remove(token);
			tracker.StopState(token);
			tokensCopy = ActiveTokens.ToList();
		}

		/// <summary>
		/// Pauses this tracker. 
		/// </summary>
		/// <remarks>When paused, the time for timeouts is not advanced.</remarks>
		public void Pause()
		{
			IsPaused = true;

			foreach (var clock in clocks.Values)
			{
				clock.Pause();
			}
		}

		/// <summary>
		/// Unpauses this tracker. 
		/// </summary>
		public void Unpause()
		{
			IsPaused = false;

			foreach (var clock in clocks.Values)
			{
				clock.Unpause();
			}
		}

		private void DoNothing()
		{ }
	}
}

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// The same as StateTracker, but states can also time out.
	/// </summary>
	/// <typeparam name="TStateData">The type of the t state data.</typeparam>
	/// <remarks>
	/// <para>Time-outs are managed by this class: when the state times out, 
	/// it is stopped, and an event is raised. This tracker must be updated (typically
	/// in a mono-behaviour Update method).</para>
	/// <para>This is a benchmark implementation.</para>
	/// </remarks>
	public class TimedStateTracker<TStateData>
	{
		private class TimeData
		{
			public float startTime;
			public float maxTime;
			public Action onTimeOut;
		}

		private class TimedState
		{
			public TStateData stateData;
			public TimeData timeData;
		}

		private class TimeStateWrapper : IStateToken<TStateData>
		{
			public IStateToken<TimedState> token;

			public TStateData State
			{
				get { return token.State.stateData; }
			}
		}

		private readonly StateTracker<TimedState> tracker;

		/// <summary>
		/// Gets a value indicating whether this tracker is active, that is, whether
		/// any state has been started that has not been stopped.
		/// </summary>
		/// <value><c>true</c> if this tracker is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return tracker.IsActive; }
		}

		/// <summary>
		/// Occurs when this tracker is inactive and a state is started (so that this tracker becomes active).
		/// </summary>
		public event Action OnStateActive
		{
			add { tracker.OnStateActive += value; }
			remove { tracker.OnStateActive -= value; }
		}

		/// <summary>
		/// Occurs when all active states are stopped, that is, when this tracker is active and becomes inactive.
		/// </summary>
		public event Action OnStateInactive
		{
			add { tracker.OnStateInactive += value; }
			remove { tracker.OnStateInactive -= value; }
		}

		/// <summary>
		/// Returns all the active tokens: tokens returned when states has been started that has not yet
		/// been stopped.
		/// </summary>
		/// <value>The active tokens.</value>
		public IEnumerable<IStateToken<TStateData>> ActiveTokens
		{
			get
			{
				return
					tracker.ActiveTokens.Select(token => (IStateToken<TStateData>)new TimeStateWrapper { token = token });
			}
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
		/// <exception cref="InvalidOperationException">Invalid token</exception>
		public void StopState(IStateToken<TStateData> token)
		{
			var timeState = token as TimeStateWrapper;

			if (timeState == null)
			{
				// This can happen when a user somehow 
				// obtains a IStateToken<TStateData>
				// that was not returned by this tracker.
				throw new InvalidOperationException("Invalid token");
			}

			if (!tracker.ActiveTokens.Contains(timeState.token))
			{
				throw new InvalidOperationException("The token is not active.");
			}

			tracker.StopState(timeState.token);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimedStateTracker{TStateData}"/> class.
		/// </summary>
		public TimedStateTracker()
		{
			tracker = new StateTracker<TimedState>();
		}

		/// <summary>
		/// Starts a state, and returns a token that can be used to stop it again.
		/// </summary>
		/// <param name="stateData">Custom state data. This is useful in cases
		/// where all the active states needs to be examined. For example, this data
		/// can be used to identify states externally.</param>
		/// <param name="currentTime">The current time.</param>
		/// <param name="maxTime">The maximum amount of time this state should survive past the current time.</param>
		/// <param name="onTimeOut">An action to call when this state times out.</param>
		/// <returns>A token that wraps the custom state data and can be used to stop 
		/// the state started with this method.</returns>
		/// <remarks>For a state to time out, it is necessary for the Update method to
		/// be called regularly (for example, in a MonoBehaviours Update method).</remarks>
		public IStateToken<TStateData> StartState(
			TStateData stateData,
			float currentTime,
			float maxTime,
			Action onTimeOut)
		{
			var state = new TimedState
			{
				stateData = stateData,
				timeData = new TimeData()
				{
					startTime = currentTime,
					maxTime = maxTime,
					onTimeOut = onTimeOut
				}
			};

			var token = tracker.StartState(state);

			return new TimeStateWrapper
			{
				token = token
			};
		}

		/// <summary>
		/// Starts a state, and returns a token that can be used to stop it again.
		/// </summary>
		/// <param name="stateData">Custom state data. This is useful in cases
		/// where all the active states needs to be examined. For example, this data
		/// can be used to identify states externally.</param>
		/// <param name="currentTime">The current time.</param>
		/// <param name="maxTime">The maximum amount of time this state should survive past the current time.</param>
		/// <returns>A token that wraps the custom state data and can be used to stop 
		/// the state started with this method.</returns>
		/// <remarks>For a state to time out, it is necessary for the Update method to
		/// be called regularly (for example, in a MonoBehaviours Update method).</remarks>
		public IStateToken<TStateData> StartState(
			TStateData stateData,
			float currentTime,
			float maxTime)
		{
			return StartState(stateData, currentTime, maxTime, DoNothing);
		}

		/// <summary>
		/// Updates this tracker with the specified current time.
		/// </summary>
		/// <param name="currentTime">The current time.</param>
		public void Update(float currentTime)
		{
			var statesToStop = tracker.ActiveTokens
				.Where(token => currentTime - token.State.timeData.startTime >= token.State.timeData.maxTime)
				.ToList();

			foreach (var stateToken in statesToStop)
			{
				tracker.StopState(stateToken);
				stateToken.State.timeData.onTimeOut();
			}
		}

		private void DoNothing()
		{
		}
	}
}