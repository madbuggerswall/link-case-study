using Core.Contexts;
using Frolics.Utilities;
using UnityEngine;

namespace Core.Input {
	[DefaultExecutionOrder(-32)]
	public class InputController : MonoBehaviour {
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
	}
}
