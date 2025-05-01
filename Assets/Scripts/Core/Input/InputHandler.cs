using UnityEngine;
using UnityEngine.Events;

namespace Core.Input {
	public abstract class InputHandler {
		public Vector2 PressPosition { get; protected set; }
		public Vector2 DragPosition { get; protected set; }
		public Vector2 ReleasePosition { get; protected set; }

		public UnityEvent<PressData> PressEvent { get; }
		public UnityEvent<ReleaseData> ReleaseEvent { get; }

		protected InputHandler() {
			PressEvent = new UnityEvent<PressData>();
			ReleaseEvent = new UnityEvent<ReleaseData>();
		}

		public abstract void HandleInput();

		public static InputHandler CreateForPlatform(RuntimePlatform platform) {
			bool isMobile = platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;
			return isMobile ? new MobileInputHandler() : new StandaloneInputHandler();
		}
	}
}
