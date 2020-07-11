using System;

namespace Gamelogic.Extensions.Obsolete
{
	public static class GLMathf
	{
		[Obsolete("Use GLMathf.Sign instead.")]
		public static int Sine(float x)
		{
			return Gamelogic.Extensions.GLMathf.Sign(x);
		}

		[Obsolete("Use GLMathf.Sign instead.")]
		public static int Sine(int x)
		{
			return Gamelogic.Extensions.GLMathf.Sign(x);
		}
	}

}