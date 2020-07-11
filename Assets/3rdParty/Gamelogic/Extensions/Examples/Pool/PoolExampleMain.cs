using UnityEngine;
using UnityEngine.UI;

using Gamelogic.Extensions.Algorithms;

namespace Gamelogic.Extensions.Examples
{
	public class PoolExampleMain : GLMonoBehaviour
	{
		public int poolCapacity;
		public Button characterPrefab;
		public GameObject poolRoot;
		private MonoBehaviourPool<Button> characterPool;

		public Color color1;
		public Color color2;

		private IGenerator<Color> skinColor;

		public void Start()
		{
			characterPool = new MonoBehaviourPool<Button>(
				characterPrefab,
				poolRoot,
				poolCapacity,	

				(character) => 
				{
					character.gameObject.SetActive(true);
					character.gameObject.transform.SetAsLastSibling();
					character.image.color = skinColor.Next();
					character.onClick.AddListener(
						new UnityEngine.Events.UnityAction(() => characterPool.ReleaseObject(character)));	
				},
				
				(character) => 
				{
					character.onClick.RemoveAllListeners();
					character.gameObject.SetActive(false);
				});

			skinColor = Generator
				.UniformRandomFloat()
				.Select(t => Color.Lerp(color1, color2, t));
		}

		public void AddCharacter()
		{
			if(characterPool.IsObjectAvailable)
			{
				characterPool.GetNewObject();
			}
		}
	}
}