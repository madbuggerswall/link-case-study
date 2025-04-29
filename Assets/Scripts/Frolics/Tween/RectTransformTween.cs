using System;
using UnityEngine;

namespace Frolics.Tween {
	public class RectTransformTween : Tween {
		private readonly RectTransform tweener;
		private Action tweenAction;

		private (Vector2 initial, Vector2 target) anchoredPosition;
		private (Vector3 initial, Vector3 target) localScale;
		private (Quaternion initial, Quaternion target) rotation;

		public RectTransformTween(RectTransform tweener, float duration) : base(duration) {
			this.tweener = tweener;
			this.tweenAction = delegate { };
		}

		public void SetAnchoredPosition(Vector2 targetPosition) {
			this.anchoredPosition.initial = tweener.anchoredPosition;
			this.anchoredPosition.target = targetPosition;
			this.tweenAction = ApplyAnchoredPositionTween;
		}

		public void SetLocalScale(Vector3 targetLocalScale) {
			this.localScale.initial = tweener.localScale;
			this.localScale.target = targetLocalScale;
			this.tweenAction = ApplyScaleTween;
		}

		public void SetRotation(Quaternion targetRotation) {
			this.rotation.initial = tweener.rotation;
			this.rotation.target = targetRotation;
			this.tweenAction = ApplyRotationTween;
		}

		private void ApplyAnchoredPositionTween() {
			tweener.anchoredPosition = Vector2.Lerp(anchoredPosition.initial, anchoredPosition.target, progress);
		}

		private void ApplyScaleTween() {
			tweener.localScale = Vector3.Lerp(localScale.initial, localScale.target, progress);
		}

		private void ApplyRotationTween() {
			tweener.rotation = Quaternion.Lerp(rotation.initial, rotation.target, progress);
		}

		protected override void UpdateTween() {
			tweenAction.Invoke();
		}
	}
}
