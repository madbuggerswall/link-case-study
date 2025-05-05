using UnityEngine;

namespace Frolics.Tween {
	public static partial class Ease {
		// Linear
		private static float Linear(float time) {
			return time;
		}

		// Quadratic easing in - accelerating from zero velocity
		private static float EaseInQuad(float time) {
			return time * time;
		}

		// Quadratic easing out - decelerating to zero velocity
		private static float EaseOutQuad(float time) {
			return 1 - (1 - time) * (1 - time);
		}

		//  Quadratic easing in/out - acceleration until halfway, then deceleration
		private static float EaseInOutQuad(float time) {
			return time < 0.5f ? 2f * time * time : 1 - (-2 * time + 2) * (-2 * time + 2) / 2;
		}

		// Cubic easing in - accelerating from zero velocity
		private static float EaseInCubic(float time) {
			return time * time * time;
		}

		// Cubic easing out - decelerating to zero velocity
		private static float EaseOutCubic(float time) {
			return 1 - (1 - time) * (1 - time) * (1 - time);
		}


		// Cubic easing in/out - acceleration until halfway, then deceleration
		private static float EaseInOutCubic(float time) {
			return time < 0.5 ? 4 * time * time * time : 1 - (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) / 2;
		}

		// Quartic easing in - accelerating from zero velocity
		private static float EaseInQuart(float time) {
			return time * time * time * time;
		}

		// Quartic easing out - decelerating to zero velocity
		private static float EaseOutQuart(float time) {
			return 1 - (1 - time) * (1 - time) * (1 - time) * (1 - time);
		}

		// Quartic easing in/out - acceleration until halfway, then deceleration
		private static float EaseInOutQuart(float time) {
			return time < 0.5
				? 8 * time * time * time * time
				: 1 - (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) / 2;
		}

		// Quintic easing in - accelerating from zero velocity
		private static float EaseInQuint(float time) {
			return time * time * time * time * time;
		}

		// Quintic easing out - decelerating to zero velocity
		private static float EaseOutQuint(float time) {
			return 1 - (1 - time) * (1 - time) * (1 - time) * (1 - time) * (1 - time);
		}

		// Quintic easing in/out - acceleration until halfway, then deceleration
		private static float EaseInOutQuint(float time) {
			return time < 0.5
				? 16 * time * time * time * time * time
				: 1 - (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) * (-2 * time + 2) / 2;
		}

		// Sinusoidal easing in - accelerating from zero velocity
		private static float EaseInSine(float time) {
			return 1 - Mathf.Cos(time * Mathf.PI / 2);
		}

		// Sinusoidal easing out - decelerating to zero velocity
		private static float EaseOutSine(float time) {
			return Mathf.Sin(time * Mathf.PI / 2);
		}

		// Sinusoidal easing in/out - accelerating until halfway, then decelerating
		private static float EaseInOutSine(float time) {
			return -(Mathf.Cos(Mathf.PI * time) - 1) / 2;
		}

		// Exponential easing in - accelerating from zero velocity
		private static float EaseInExpo(float time) {
			return Mathf.Approximately(time, 0) ? 0 : Mathf.Pow(2, 10 * time - 10);
		}

		// Exponential easing out - decelerating to zero velocity
		private static float EaseOutExpo(float time) {
			return Mathf.Approximately(time, 1) ? 1 : 1 - Mathf.Pow(2, -10 * time);
		}

		// Exponential easing in/out - accelerating until halfway, then decelerating
		private static float EaseInOutExpo(float time) {
			if (Mathf.Approximately(time, 0))
				return 0;
			else if (Mathf.Approximately(time, 1))
				return 1;
			else
				return time < 0.5 ? Mathf.Pow(2, 20 * time - 10) / 2 : (2 - Mathf.Pow(2, -20 * time + 10)) / 2;
		}

		// Circular easing in - accelerating from zero velocity
		private static float EaseInCirc(float time) {
			return 1f - Mathf.Sqrt(1 - time * time);
		}

		// Circular easing out - decelerating to zero velocity
		private static float EaseOutCirc(float time) {
			return Mathf.Sqrt(1 - (time - 1) * (time - 1));
		}

		// Circular easing in/out - acceleration until halfway, then deceleration
		private static float EaseInOutCirc(float time) {
			return time < 0.5
				? (1 - Mathf.Sqrt(1 - (2 * time) * (2 * time))) / 2
				: (Mathf.Sqrt(1 - (-2 * time + 2) * (-2 * time + 2)) + 1) / 2;
		}
	}
}
