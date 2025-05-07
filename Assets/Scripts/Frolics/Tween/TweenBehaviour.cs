using UnityEngine;
using UnityEngine.SceneManagement;

namespace Frolics.Tween {

	[DefaultExecutionOrder(-16)]
	public class TweenBehaviour : MonoBehaviour {
		private TweenManager tweenManager;

		private void Update() {
			tweenManager.TickAllTweens();
		}

		public void Initialize(TweenManager tweenManager) {
			this.tweenManager = tweenManager;
			
			// Very questionable 
			SceneManager.sceneUnloaded += scene => this.tweenManager.ClearAllTweens();
		}
	}
}
