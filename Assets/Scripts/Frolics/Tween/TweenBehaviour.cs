using System.Collections.Generic;
using UnityEngine;

namespace Frolics.Tween {

	[DefaultExecutionOrder(-16)]
	public class TweenBehaviour : MonoBehaviour {
		private TweenManager tweenManager;

		private void Update() {
			tweenManager.TickAllTweens();
		}

		internal void Initialize(TweenManager tweenManager) {
			this.tweenManager = tweenManager;
		}
	}
}
