using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Core.Input.Mobile {
	public struct TouchPressData {
		public Touch Touch { get; private set; }
		public Vector2 PressPosition { get; private set; }

		public TouchPressData(Touch touch, Vector2 pressPosition) {
			this.Touch = touch;
			this.PressPosition = pressPosition;
		}
	}
}