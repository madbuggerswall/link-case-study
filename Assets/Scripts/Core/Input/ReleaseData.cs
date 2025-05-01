using UnityEngine;

namespace Core.Input {
	public struct ReleaseData {
		public Vector2 ReleasePosition { get; private set; }

		public ReleaseData(Vector2 releasePosition) {
			this.ReleasePosition = releasePosition;
		}
	}
}
