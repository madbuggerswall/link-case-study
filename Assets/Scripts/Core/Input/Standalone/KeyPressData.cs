using UnityEngine.InputSystem.Controls;

namespace Core.Input.Standalone {
	public struct KeyPressData {
		public KeyControl KeyControl { get; }

		public KeyPressData(KeyControl keyControl) {
			this.KeyControl = keyControl;
		}
	}
}
