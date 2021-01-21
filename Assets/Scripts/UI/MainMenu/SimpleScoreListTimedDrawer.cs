public class SimpleScoreListTimedDrawer : ScoreListDrawer<ScoreListTimed, int>{
	public static int? lastScore;

	protected override int? LastScore{
		get{
			return lastScore;
		}
		set{
			lastScore = value;
		}
	}
}