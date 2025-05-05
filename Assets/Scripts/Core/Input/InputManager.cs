using Core.Contexts;
using Core.Input.Mobile;
using Core.Input.Standalone;
using UnityEngine;

namespace Core.Input {
	[DefaultExecutionOrder(-32)]

	// NOTE Can be renamed to InputManager
	public class InputManager : MonoBehaviour, IInitializable {
		public MobileInputHandler MobileInputHandler { get; private set; }
		public StandaloneInputHandler StandaloneInputHandler { get; private set; }
		public InputHandler CommonInputHandler { get; private set; }

		// Dependencies
		private CameraController cameraController;

		public void Initialize() {
			cameraController = SceneContext.GetInstance().Get<CameraController>();
			
			MobileInputHandler = new MobileInputHandler();
			StandaloneInputHandler = new StandaloneInputHandler();
			CommonInputHandler = GetPlatformDependentHandler();
		}

		private void Update() {
			MobileInputHandler.HandleInput();
			StandaloneInputHandler.HandleInput();
		}

		public Vector3 ScreenPositionToWorldSpace(Vector2 screenPosition) {
			return cameraController.ScreenPositionToWorldSpace(screenPosition);
		}

		private InputHandler GetPlatformDependentHandler() {
			bool isMobile = Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;
			return isMobile ? MobileInputHandler : StandaloneInputHandler;
		}
	}
}
