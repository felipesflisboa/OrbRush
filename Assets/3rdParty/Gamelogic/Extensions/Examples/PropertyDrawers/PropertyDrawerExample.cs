using UnityEngine;

using Gamelogic.Extensions.Internal;
using System;

namespace Gamelogic.Extensions.Examples
{
	[Flags]
	public enum MonsterState
	{
		IsHungry = 1,
		IsThirsty = 2,
		IsAngry = 4,
		IsTired = 8
	}

	[Serializable]
	public class MonsterData
	{
		public string name;
		public string nickName;
		public Color color;
	}
	
	//To use a re-orderable list with your own types,
	//you need to define your own list type like this.
	[Serializable]
	public class MonsterList : InspectorList<MonsterData>
	{

	}

	//This class would work exactly the same in the inspector
	//if it was extended from MonoBehaviour instead, except
	//for the InspectorButton
	public class PropertyDrawerExample : GLMonoBehaviour
	{
		[ReadOnly]
		public string readonlyString = "Cannot change in inspector";

		[Space]
		[Comment("Cannot be negative")]
		[NonNegative]
		public int nonNegativeInt = 0;

		[NonNegative]
		public float nonNegativeFloat = 0f;

		[Highlight]
		public int highligtedInt;

		[Space]
		[Comment("Can only be positive")]
		[Positive]
		public int positiveInt = 1;

		[Positive]
		public float positiveFloat = 0.1f;
		
		[Space]
		[Header("Reorderable Lists")]
		public ColorList colors = new ColorList { Utils.Blue, Utils.Green, Utils.Yellow, Utils.Red };

		[Space]
		public MonsterList monsters = new MonsterList
		{
			new MonsterData{ name = "Vampire", nickName = "Vamp", color = Utils.Blue },
			new MonsterData{ name = "Wherewolve", nickName = "Wolf", color = Utils.Red},
		};

		[Header("Other Fields")]
		//Note the nickName is used for the labels
		[LabelField("nickName")]
		public MonsterData[] moreMonsters = new[]
		{
			new MonsterData{ name = "Vampire", nickName = "Vamp", color = Utils.Blue },
			new MonsterData{ name = "Wherewolve", nickName = "Wolf", color = Utils.Red},
		};

		[InspectorFlags]
		public MonsterState monsterState = MonsterState.IsAngry | MonsterState.IsHungry;

		public MinMaxFloat range = new MinMaxFloat(0.25f, 0.75f);

		public OptionalInt anOptionalInt = new OptionalInt
		{
			UseValue = true,
			Value = 3
		};

		[Space]
		[Header("Inspector Buttons")]
		[Dummy, SerializeField]
		private bool dummy; //This hack may be questionable.

		//This will only show as a button if you extend from GLMonoBehaviour.
		[InspectorButton]
		public static void LogHello()
		{
			Debug.Log("Hello");
		}
	}
}
