namespace Gamelogic.Extensions
{
	/// <summary>
	/// When a new state is started in a tracker, a token is created
	/// that wraps custom data, and can be used to stop the state later.
	/// </summary>
	/// <typeparam name="TStateData">The type of the t state data.</typeparam>
	public interface IStateToken<out TStateData>
	{
		TStateData State { get; }
	}
}
