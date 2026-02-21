using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.Util
{
	internal static class GameMathUtil
	{
		/// <summary>
		/// Performs linear interpolation to find a value between a min and max,
		/// based on a current value's position within a start and end range.
		/// </summary>
		/// <param name="currentValue">The current input value (e.g., skill level).</param>
		/// <param name="minValue">The minimum output value (at startRange).</param>
		/// <param name="maxValue">The maximum output value (at endRange).</param>
		/// <param name="startRange">The start of the input range (defaults to 1).</param>
		/// <param name="endRange">The end of the input range (defaults to 100).</param>
		/// <returns>The interpolated value.</returns>
		public static float Lerp(float currentValue, float minValue, float maxValue, float startRange = 1f, float endRange = 100f)
		{
			// Clamp the current value to be within the specified range.
			var x = Math.Clamp(currentValue, startRange, endRange);

			// Handle the case where the range is zero to avoid division by zero.
			if (Math.Abs(endRange - startRange) < float.Epsilon)
			{
				return minValue;
			}

			// Perform the linear interpolation.
			return minValue + ((x - startRange) / (endRange - startRange) * (maxValue - minValue));
		}

		public static byte ToByte(this float value)
		{
			return (byte)Math.Clamp((int)MathF.Round(value * 255f), 0, 255);
		}
	}
}
