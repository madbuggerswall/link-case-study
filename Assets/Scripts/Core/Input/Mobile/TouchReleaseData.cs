using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Core.Input.Mobile {
	public struct TouchReleaseData {
		public Touch Touch { get; private set; }
		public Vector2 ReleasePosition { get; private set; }

		public TouchReleaseData(Touch touch, Vector2 releasePosition) {
			this.Touch = touch;
			this.ReleasePosition = releasePosition;
		}
	}
}
