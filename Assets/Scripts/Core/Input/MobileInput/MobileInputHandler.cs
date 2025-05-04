using UnityEngine;
using UnityEngine.Events;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Core.Input {
	public class MobileInputHandler : InputHandler {
		private const int MaxTouches = 1;

		public Vector2 TouchPressPosition { get; private set; }
		public Vector2 TouchDragPosition { get; private set; }
		public Vector2 TouchReleasePosition { get; private set; }

		public UnityEvent<TouchPressData> TouchPressEvent { get; }
		public UnityEvent<TouchReleaseData> TouchReleaseEvent { get; }

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
				TouchPressPosition = touch.screenPosition;
				TouchDragPosition = touch.screenPosition;
				TouchReleasePosition = touch.screenPosition;
				TouchPressEvent.Invoke(new TouchPressData(touch, TouchPressPosition));

				PointerPosition = touch.screenPosition;
				PressEvent.Invoke(new PointerPressData(PointerPosition));
			} else if (touch.inProgress) {
				TouchDragPosition = touch.screenPosition;
				PointerPosition = touch.screenPosition;
			} else if (touch.ended) {
				TouchReleasePosition = touch.screenPosition;
				TouchReleaseEvent.Invoke(new TouchReleaseData(touch, TouchReleasePosition));

				PointerPosition = touch.screenPosition;
				ReleaseEvent.Invoke(new PointerReleaseData(PointerPosition));
			}
		}
	}
}
