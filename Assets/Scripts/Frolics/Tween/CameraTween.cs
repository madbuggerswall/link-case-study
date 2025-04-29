using System;
using UnityEngine;

namespace Frolics.Tween {
	public class CameraTween : Tween {
		private readonly Camera tweener;
		private Action tweenAction;

		private (float initial, float target) orthographicSize;

		public CameraTween(Camera tweener, float duration) : base(duration) {
			this.tweener = tweener;
			this.tweenAction = delegate { };
		}

		public void SetOrthographicSize(float targetOrthographicSize) {
			this.orthographicSize.initial = tweener.orthographicSize;
			this.orthographicSize.target = targetOrthographicSize;
			this.tweenAction = ApplyOrthographicSizeTween;
		}

		private void ApplyOrthographicSizeTween() {
			tweener.orthographicSize = Mathf.Lerp(orthographicSize.initial, orthographicSize.target, progress);
		}

		protected override void UpdateTween() {
			tweenAction.Invoke();
		}
	}
}
