/// <summary>
/// Timed racing game example for ScoreList. Values are stored as millis.
/// </summary>
public class ScoreListTimed : ScoreListInt{
	protected override string KeyLabel{
		get{
			return "Impl9";
		}
	}
	public override string GetStringAsValue(int value){
		return $"Level {value}"; 
	}
}