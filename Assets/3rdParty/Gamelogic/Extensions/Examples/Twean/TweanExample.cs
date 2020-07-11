using UnityEngine;

namespace Gamelogic.Extensions.Examples
{
	public class TweanExample : GLMonoBehaviour
	{
		public Transform cube;
		public Transform leftPosition;
		public Transform rightPosition;

		public void GoLeft()
		{
			Tween(
				cube.position, 
				leftPosition.transform.position, 
				1, 
				Vector3.Lerp, 
				newPosition => { cube.position = newPosition; });
		}

		public void GoRight()
		{
			Tween(
				cube.position,
				rightPosition.transform.position,
				1,
				Vector3.Lerp,
				newPosition => { cube.position = newPosition; });
		}
	}
}