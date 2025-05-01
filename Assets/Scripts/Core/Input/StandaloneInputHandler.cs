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

			if (pressStarted) {
				PressPosition = Mouse.current.position.ReadValue();
				DragPosition = PressPosition;
				ReleasePosition = PressPosition;
				PressEvent.Invoke(new PressData(PressPosition));
			} else if (isPressHeld) {
				DragPosition = Mouse.current.position.ReadValue();
			} else if (pressReleased) {
				ReleasePosition = Mouse.current.position.ReadValue();
				ReleaseEvent.Invoke(new ReleaseData(ReleasePosition));
			}
		}
	}
}
