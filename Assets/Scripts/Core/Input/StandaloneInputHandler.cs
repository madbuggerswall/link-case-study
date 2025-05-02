using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core.Input {
	public class StandaloneInputHandler : InputHandler {
		public StandaloneInputHandler() : base() {
			Debug.Log(nameof(StandaloneInputHandler));

		}

		public override void HandleInput() {
			ReadMouseButtonInput(Mouse.current.leftButton);
		}

		private void ReadMouseButtonInput(ButtonControl buttonControl) {
			bool pressStarted = buttonControl.wasPressedThisFrame;
			bool isPressHeld = buttonControl.isPressed;
			bool pressReleased = buttonControl.wasReleasedThisFrame;

			PointerPosition = Mouse.current.position.ReadValue();
			
			if (pressStarted) {
				PressEvent.Invoke(new PointerPressData(PointerPosition));
			} else if (isPressHeld) {
				
			} else if (pressReleased) {
				ReleaseEvent.Invoke(new PointerReleaseData(PointerPosition));
			}
		}
	}
}
