using System.Collections.Generic;
using UnityEngine;

// TODO Needs a tween pool
namespace Frolics.Tween {
	public class TweenManager {
		private static TweenManager instance;

		private readonly HashSet<Tween> tweens;
		private readonly HashSet<Tween> tweensToRemove;

		private bool isInitialized;

		private TweenManager() {
			InitializeTweenBehavior();
			tweens = new HashSet<Tween>();
			tweensToRemove = new HashSet<Tween>();
		}

		public void OnPlay(Tween tween) {
			tweens.Add(tween);
		}

		public void OnTweenComplete(Tween tween) {
			tweensToRemove.Add(tween);
		}

		private void InitializeTweenBehavior() {
			const string gameObjectName = "Tween Behaviour";

			if (isInitialized)
				return;

			TweenBehaviour behaviourInScene = Object.FindAnyObjectByType<TweenBehaviour>();
			isInitialized = true;

			if (behaviourInScene is not null)
				return;

			TweenBehaviour tweenBehaviour = new GameObject(gameObjectName).AddComponent<TweenBehaviour>();
			tweenBehaviour.Initialize(this);
			Object.DontDestroyOnLoad(tweenBehaviour.gameObject);
			isInitialized = true;
		}

		public void TickAllTweens() {
			foreach (Tween tween in tweens) {
				tween.Tick(Time.deltaTime);
			}

			foreach (Tween tween in tweensToRemove) {
				tweens.Remove(tween);
			}

			tweensToRemove.Clear();
		}

		public static TweenManager GetInstance() {
			instance ??= new TweenManager();
			return instance;
		}
	}
}
