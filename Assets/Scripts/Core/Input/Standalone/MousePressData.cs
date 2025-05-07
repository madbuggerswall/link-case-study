using UnityEngine.InputSystem.Controls;

namespace Core.Input.Standalone {
	public struct MousePressData {
		public ButtonControl ButtonControl { get; private set; }

		public MousePressData(ButtonControl buttonControl) {
			this.ButtonControl = buttonControl;
		}
	}
}
