using Core.Contexts;
using UnityEngine;

namespace Core.Input {
	[DefaultExecutionOrder(-32)]
	
	// NOTE Can be renamed to InputManager
	public class InputController : MonoBehaviour,IInitializable {
		public InputHandler InputHandler { get; private set; }

		// Dependencies
		private CameraController cameraController;

		public void Initialize() {
			this.cameraController = SceneContext.GetInstance().Get<CameraController>();
			InputHandler = InputHandler.CreateForPlatform(Application.platform);
		}

		private void Update() {
			InputHandler.HandleInput();
		}

		public Vector3 ScreenPositionToWorldSpace(Vector2 screenPosition) {
			return cameraController.ScreenPositionToWorldSpace(screenPosition);
		}
	}
}
