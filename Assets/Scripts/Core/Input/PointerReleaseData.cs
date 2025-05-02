using UnityEngine;

namespace Core.Input {
	public struct PointerReleaseData {
		public Vector2 ReleasePosition { get; private set; }

		public PointerReleaseData(Vector2 releasePosition) {
			this.ReleasePosition = releasePosition;
		}
	}
}
