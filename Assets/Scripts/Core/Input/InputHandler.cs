using UnityEngine;
using UnityEngine.Events;

namespace Core.Input {
	public abstract class InputHandler {
		public Vector2 PointerPosition { get; protected set; }

		public UnityEvent<PointerPressData> PressEvent { get; } = new();
		public UnityEvent<PointerReleaseData> ReleaseEvent { get; } = new();

		public abstract void HandleInput();

		public static InputHandler CreateForPlatform(RuntimePlatform platform) {
			bool isMobile = platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;
			return isMobile ? new MobileInputHandler() : new StandaloneInputHandler();
		}
	}
}
