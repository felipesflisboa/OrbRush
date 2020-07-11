using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Gamelogic.Extensions.Examples
{
	//A class that adds a bunch of buttons
	public class ButtonListUI : GLMonoBehaviour
	{
		#region Public Fields
		public Button buttonPrefab;
		#endregion

		#region Initialization
		public void Init(IEnumerable<Poison> poisons, Action<Poison> onClick, Color color)
		{
			transform.DestroyChildren();

			foreach(var poison in poisons)
			{
				var button = Instantiate(buttonPrefab, gameObject);

				button.GetComponentInChildren<Text>().text = poison.ToString();
				button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => onClick(poison)));
				button.image.color = color;
			}
		}
		#endregion
	}
}