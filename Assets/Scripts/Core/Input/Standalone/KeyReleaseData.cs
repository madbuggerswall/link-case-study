using UnityEngine.InputSystem.Controls;

namespace Core.Input.Standalone {
	public struct KeyReleaseData {
		public KeyControl KeyControl { get; }

		public KeyReleaseData(KeyControl keyControl) {
			this.KeyControl = keyControl;
		}
	}
}
