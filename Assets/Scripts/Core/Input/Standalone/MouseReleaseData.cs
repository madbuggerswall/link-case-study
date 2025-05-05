using UnityEngine.InputSystem.Controls;

namespace Core.Input.Standalone {
	public struct MouseReleaseData {
		public ButtonControl ButtonControl { get; private set; }

		public MouseReleaseData(ButtonControl buttonControl) {
			this.ButtonControl = buttonControl;
		}
	}
}