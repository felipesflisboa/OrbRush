/// <summary>
/// Timed racing game example for ScoreList. Values are stored as millis.
/// </summary>
public class ScoreListTimed : ScoreListInt{
	protected override string KeyLabel{
		get{
			return "Impl8";
		}
	}
	public override string GetStringAsValue(int value){
		return new System.DateTime(System.TimeSpan.FromMilliseconds(value).Ticks).ToString("mm:ss:fff"); 
	}
}