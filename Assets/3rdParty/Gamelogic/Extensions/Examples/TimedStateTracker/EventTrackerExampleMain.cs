using System;
using System.Linq;
using UnityEngine;

namespace Gamelogic.Extensions.Examples
{
	// This example shows how to use a event tracker.
	// The interesting code is in poisonable player.
	public class EventTrackerExampleMain : GLMonoBehaviour
	{
		#region Public Fields
		[Header("Player")]
		public PoisonablePlayer player;

		[Header("Button Lists")]
		public Color poisonColor;
		public ButtonListUI poisonList;

		[Space]
		public Color antidoteColor;
		public ButtonListUI antidotesList;
		#endregion

		#region Messages
		public void Start()
		{
			var poisons = Enum.GetValues(typeof(Poison)).Cast<Poison>();

			poisonList.Init(poisons, (poison) => { player.EatPoison(poison); }, poisonColor);
			antidotesList.Init(poisons, (poison) => { player.EatAntidote(poison); }, antidoteColor);
		}
		#endregion
	}
}