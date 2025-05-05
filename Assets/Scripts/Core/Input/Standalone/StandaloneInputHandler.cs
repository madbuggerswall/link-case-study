using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Core.Input.Standalone {
	public class StandaloneInputHandler : InputHandler {
		public Vector2 MousePressPosition { get; private set; }
		public Vector2 MouseDragPosition { get; private set; }
		public Vector2 MouseReleasePosition { get; private set; }

		public UnityEvent<MousePressData> MousePressEvent { get; }
		public UnityEvent<MouseReleaseData> MouseReleaseEvent { get; }

		public UnityEvent<KeyPressData> KeyPressEvent { get; }
		public UnityEvent<KeyReleaseData> KeyReleaseEvent { get; }

		public StandaloneInputHandler() : base() {
			MousePressEvent = new UnityEvent<MousePressData>();
			MouseReleaseEvent = new UnityEvent<MouseReleaseData>();

			KeyPressEvent = new UnityEvent<KeyPressData>();
			KeyReleaseEvent = new UnityEvent<KeyReleaseData>();
		}

		public override void HandleInput() {
			ReadMouseButtonInput(Mouse.current.leftButton);
			ReadMouseButtonInput(Mouse.current.rightButton);
			ReadKeyboardButtonInput(Keyboard.current.spaceKey);
		}

		private void ReadMouseButtonInput(ButtonControl buttonControl) {
			bool pressStarted = buttonControl.wasPressedThisFrame;
			bool isPressHeld = buttonControl.isPressed;
			bool pressReleased = buttonControl.wasReleasedThisFrame;

			PointerPosition = Mouse.current.position.ReadValue();

			if (pressStarted) {
				MousePressPosition = Mouse.current.position.ReadValue();
				MouseDragPosition = Mouse.current.position.ReadValue();
				MouseReleasePosition = Mouse.current.position.ReadValue();
				MousePressEvent.Invoke(new MousePressData(buttonControl));

				if (buttonControl == Mouse.current.leftButton)
					PressEvent.Invoke(new PointerPressData(PointerPosition));
			} else if (isPressHeld) {
				MouseDragPosition = Mouse.current.position.ReadValue();
			} else if (pressReleased) {
				MouseReleasePosition = Mouse.current.position.ReadValue();
				MouseReleaseEvent.Invoke(new MouseReleaseData(buttonControl));

				if (buttonControl == Mouse.current.leftButton)
					ReleaseEvent.Invoke(new PointerReleaseData(PointerPosition));
			}
		}

		private void ReadKeyboardButtonInput(KeyControl keyControl) {
			bool pressStarted = keyControl.wasPressedThisFrame;
			bool isPressHeld = keyControl.isPressed;
			bool pressReleased = keyControl.wasReleasedThisFrame;

			if (pressStarted) {
				KeyPressEvent.Invoke(new KeyPressData(keyControl));
			} else if (isPressHeld) {
				// Nope
			} else if (pressReleased) {
				KeyReleaseEvent.Invoke(new KeyReleaseData(keyControl));
			}
		}
	}
}
