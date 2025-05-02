using UnityEngine;

namespace Core.Input {
	public struct PointerPressData {
		public Vector2 PressPosition { get; private set; }

		public PointerPressData(Vector2 pressPosition) {
			this.PressPosition = pressPosition;
		}
	}
}
