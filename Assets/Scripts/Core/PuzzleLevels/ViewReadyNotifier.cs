using UnityEngine.Events;

namespace Core.PuzzleLevels {
	public class ViewReadyNotifier {
		private bool isFallTweensComplete = false;
		private bool isFillTweensComplete = false;
		public UnityEvent OnViewReady { get; private set; } = new UnityEvent();

		public void OnFallTweensComplete() {
			isFallTweensComplete = true;
			TryNotifyViewReady();
		}

		public void OnFillTweensComplete() {
			isFillTweensComplete = true;
			TryNotifyViewReady();
		}

		private void TryNotifyViewReady() {
			if (!isFillTweensComplete || !isFallTweensComplete)
				return;

			OnViewReady.Invoke();
			OnViewReady.RemoveAllListeners();
		}
	}
}
