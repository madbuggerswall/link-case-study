using System;

namespace Frolics.Tween {
	public static partial class Ease {
		public enum Type {
			Linear,
			InQuad,
			OutQuad,
			InOutQuad,
			InCubic,
			OutCubic,
			InOutCubic,
			InQuart,
			OutQuart,
			InOutQuart,
			InQuint,
			OutQuint,
			InOutQuint,
			InSine,
			OutSine,
			InOutSine,
			InExpo,
			OutExpo,
			InOutExpo,
			InCirc,
			OutCirc,
			InOutCirc
		}

		public static Func<float, float> GetEase(Type easeType) {
			return easeType switch {
				Type.Linear => Linear,
				Type.InQuad => EaseInQuad,
				Type.OutQuad => EaseOutQuad,
				Type.InOutQuad => EaseInOutQuad,
				Type.InCubic => EaseInCubic,
				Type.OutCubic => EaseOutCubic,
				Type.InOutCubic => EaseInOutCubic,
				Type.InQuart => EaseInQuart,
				Type.OutQuart => EaseOutQuart,
				Type.InOutQuart => EaseInOutQuart,
				Type.InQuint => EaseInQuint,
				Type.OutQuint => EaseOutQuint,
				Type.InOutQuint => EaseInOutQuint,
				Type.InSine => EaseInSine,
				Type.OutSine => EaseOutSine,
				Type.InOutSine => EaseInOutSine,
				Type.InExpo => EaseInExpo,
				Type.OutExpo => EaseOutExpo,
				Type.InOutExpo => EaseInOutExpo,
				Type.InCirc => EaseInCirc,
				Type.OutCirc => EaseOutCirc,
				Type.InOutCirc => EaseInOutCirc,
				_ => Linear,
			};
		}
	}
}
