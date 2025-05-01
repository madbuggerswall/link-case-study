using UnityEngine;

namespace Core.Input {
	public struct PressData {
		public Vector2 PressPosition { get; private set; }

		public PressData(Vector2 pressPosition) {
			this.PressPosition = pressPosition;
		}
	}
}
