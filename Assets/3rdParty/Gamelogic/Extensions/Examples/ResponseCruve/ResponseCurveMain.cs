using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using Gamelogic.Extensions.Algorithms;

namespace Gamelogic.Extensions.Examples
{
	public class ResponseCurveMain : GLMonoBehaviour
	{
		public float[] inputs;
		public float[] floatOutputs;
		public Vector3[] vector3Outputs;
		public Color[] colorOutputs;

		public Text floatText;
		public Image colorSwatch;
		public GameObject objectToMove;

		private IResponseCurve<string> floatTextCurve;
		private IResponseCurve<Vector3> vector3Curve;
		private IResponseCurve<Color> colorCurve;

		public void Start()
		{
			Assert.AreEqual(inputs.Length, floatOutputs.Length);
			Assert.AreEqual(inputs.Length, vector3Outputs.Length);
			Assert.AreEqual(inputs.Length, colorOutputs.Length);

			floatTextCurve = new ResponseCurveFloat(inputs, floatOutputs).Select(x => x.ToString());
			colorCurve = new ResponseCurveColor(inputs, colorOutputs);
			vector3Curve = new ResponseCurveVector3(inputs, vector3Outputs);

			OnSliderChange(0);
		}

		public void OnSliderChange(float value)
		{
			floatText.text = floatTextCurve[value];
			colorSwatch.color = colorCurve[value];
			objectToMove.transform.position = vector3Curve[value];
		}
	}
}