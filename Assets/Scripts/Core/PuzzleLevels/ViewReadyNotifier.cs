using UnityEngine.Events;

namespace Core.PuzzleLevels {
	public class ViewReadyNotifier {
		private bool isFallTweensComplete = false;
		private bool isFillTweensComplete = false;
		private bool isShuffleTweensComplete = false;

		public UnityEvent OnViewReady { get; private set; } = new UnityEvent();
		public UnityEvent OnReadyForShuffle { get; private set; } = new UnityEvent();

		public void OnFallTweensComplete() {
			isFallTweensComplete = true;
			TryNotifyReadyForShuffle();
			TryNotifyViewReady();
		}

		public void OnFillTweensComplete() {
			isFillTweensComplete = true;
			TryNotifyReadyForShuffle();
			TryNotifyViewReady();
		}

		public void OnShuffleTweensComplete() {
			isShuffleTweensComplete = true;
			TryNotifyViewReady();
		}

		private void TryNotifyReadyForShuffle() {
			if (!isFillTweensComplete || !isFallTweensComplete)
				return;

			OnReadyForShuffle.Invoke();
			OnReadyForShuffle.RemoveAllListeners();
		}

		private void TryNotifyViewReady() {
			if (!isFillTweensComplete || !isFallTweensComplete || isShuffleTweensComplete)
				return;

			OnViewReady.Invoke();
			OnViewReady.RemoveAllListeners();
		}
	}
}
