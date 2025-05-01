using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Core.Input {
	public class MobileInputHandler : InputHandler {
		private const int MaxTouches = 1;

		public MobileInputHandler() : base() {
			UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
			Debug.Log(nameof(MobileInputHandler));
		}

		public override void HandleInput() {
			ReadPrimaryTouchInput();
		}

		private void ReadAllTouchInputs() {
			int touchCount = Mathf.Min(Touch.activeTouches.Count, MaxTouches);

			for (int touchIndex = 0; touchIndex < touchCount; touchIndex++) {
				Touch touch = Touch.activeTouches[touchIndex];
				ReadTouchInput(touch);
			}
		}

		private void ReadPrimaryTouchInput() {
			int touchCount = Mathf.Min(Touch.activeTouches.Count, 1);
			if (touchCount <= 0)
				return;

			Touch touch = Touch.activeTouches[0];
			ReadTouchInput(touch);
		}

		private void ReadTouchInput(in Touch touch) {
			if (touch.began) {
				PressPosition = touch.screenPosition;
				DragPosition = PressPosition;
				ReleasePosition = PressPosition;

				PressEvent.Invoke(new PressData(PressPosition));
			} else if (touch.inProgress) {
				DragPosition = touch.screenPosition;
			} else if (touch.ended) {
				ReleasePosition = touch.screenPosition;
				ReleaseEvent.Invoke(new ReleaseData(ReleasePosition));
			}
		}
	}
}
