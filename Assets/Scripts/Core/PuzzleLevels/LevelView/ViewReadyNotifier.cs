using UnityEngine.Events;

namespace Core.PuzzleLevels.LevelView {
	public class ViewReadyNotifier {
		private bool isFallTweensComplete = true;
		private bool isFillTweensComplete = true;
		private bool isShuffleTweensComplete = true;

		public UnityEvent OnViewReady { get; } = new UnityEvent();
		public UnityEvent OnReadyForShuffle { get; } = new UnityEvent();

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

		public void WaitForFallTweens() => isFallTweensComplete = false;

		public void WaitForFillTweens() => isFillTweensComplete = false;

		public void WaitShuffleForTweens() => isShuffleTweensComplete = false;

		private void TryNotifyReadyForShuffle() {
			if (!isFillTweensComplete || !isFallTweensComplete || isShuffleTweensComplete)
				return;

			OnReadyForShuffle.Invoke();
			OnReadyForShuffle.RemoveAllListeners();
		}

		private void TryNotifyViewReady() {
			if (!isFillTweensComplete || !isFallTweensComplete || !isShuffleTweensComplete)
				return;

			OnViewReady.Invoke();
			OnViewReady.RemoveAllListeners();
		}
	}
}
