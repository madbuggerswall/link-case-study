using System;
using UnityEngine;

// TODO Needs a tween pool
namespace Frolics.Tween {
	public abstract class Tween {
		private Action onCompleteCallback;
		private Func<float, float> easingMethod;

		private float time;
		protected float progress;
		private readonly float duration;

		private readonly TweenManager tweenManager;

		private Tween() {
			time = 0;
			progress = 0;
			duration = 1;
			easingMethod = Ease.GetEase(Ease.Type.Linear);

			tweenManager = TweenManager.GetInstance();
		}

		protected Tween(float duration) : this() {
			this.duration = duration;
		}

		public void Play() {
			tweenManager.OnPlay(this);
		}

		public void Stop() {
			tweenManager.OnTweenComplete(this);
		}

		public void SetDelay(float delay) { }

		public void SetEase(Ease.Type easeType) {
			this.easingMethod = Ease.GetEase(easeType);
		}

		public void SetRepeat() { }

		public void SetOnComplete(Action callback) {
			this.onCompleteCallback = callback;
		}

		public void InsertCallback() { }

		// Tween operations
		public void Tick(float deltaTime) {
			time += deltaTime;
			float normalizedTime = Mathf.Clamp01(time / duration);
			progress = Mathf.Lerp(0, 1, easingMethod(normalizedTime));
			UpdateTween();

			if (time >= duration) {
				onCompleteCallback?.Invoke();
				tweenManager.OnTweenComplete(this);
			}
		}

		protected abstract void UpdateTween();
	}
}
