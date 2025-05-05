using System;
using UnityEngine;

// TODO Needs a tween pool
namespace Frolics.Tween {
	
	// TODO Separate this class to PositionTween, RotationTween and ScaleTween
	public class TransformTween : Tween {
		private readonly Transform tweener;
		private Action tweenAction;

		private (Vector3 initial, Vector3 target) position;
		private (Vector3 initial, Vector3 target) localScale;
		private (Quaternion initial, Quaternion target) rotation;

		public TransformTween(Transform tweener, float duration) : base(duration) {
			this.tweener = tweener;
			this.tweenAction = delegate { };
		}

		public void SetPosition(Vector3 targetPosition) {
			this.position.initial = tweener.position;
			this.position.target = targetPosition;
			this.tweenAction = ApplyPositionTween;
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

		private void ApplyPositionTween() {
			tweener.position = Vector3.Lerp(position.initial, position.target, progress);
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
