using Core.Contexts;
using Core.PuzzleGrids;
using Frolics.Tween;
using Frolics.Utilities;
using UnityEngine;

namespace Core.CameraUtilities {
	public class CameraController : MonoBehaviour, IInitializable {
		[SerializeField] private new Camera camera;
		[SerializeField] private Ease.Type easeType = Ease.Type.InOutQuad;

		public void Initialize() { }

		public void MoveCameraToGridCenter(PuzzleGrid puzzleGrid, float cameraDistance = 12f) {
			Vector3 targetPosition = GetGridCenteredCameraPosition(puzzleGrid, cameraDistance);

			TransformTween movementTween = new TransformTween(camera.transform, 1f);
			movementTween.SetPosition(targetPosition);
			movementTween.SetEase(easeType);
			movementTween.Play();
		}

		public void AdjustOrthographicSizeToFit(PuzzleGrid puzzleGrid, float margin = 1f) {
			float targetOrthographicSize = GetFittingOrthographicSize(puzzleGrid, margin);

			CameraTween cameraTween = new CameraTween(camera, 1f);
			cameraTween.SetOrthographicSize(targetOrthographicSize);
			cameraTween.SetEase(easeType);
			cameraTween.Play();
		}

		public void CenterCameraToGrid(PuzzleGrid puzzleGrid, float cameraDistance = 12f) {
			Vector3 cameraPos = GetGridCenteredCameraPosition(puzzleGrid, cameraDistance);
			camera.transform.position = cameraPos;
		}

		private Vector3 GetGridCenteredCameraPosition(PuzzleGrid puzzleGrid, float cameraDistance) {
			Vector3 cameraPos = puzzleGrid.GetCenterPoint() - Vector3.forward * cameraDistance;
			return cameraPos;
		}

		public void FitProjectionSizeToGrid(PuzzleGrid puzzleGrid, float margin = 1f) {
			camera.orthographicSize = GetFittingOrthographicSize(puzzleGrid, margin);
		}

		public Vector3 ScreenPositionToWorldSpace(Vector2 screenPosition) {
			return camera.ScreenToWorldPoint(screenPosition.WithZ(camera.nearClipPlane));
		}

		private float GetFittingOrthographicSize(PuzzleGrid puzzleGrid, float margin) {
			Vector2 gridSize = puzzleGrid.GetGridSize();
			float fittingWidth = gridSize.x + 2 * margin;
			float fittingHeight = gridSize.y + 2 * margin;

			// Redundant viewport calculations are left for future usages if needed
			float aspectRatio = camera.aspect;
			Vector2 viewportFittingWidth = new(fittingWidth, fittingWidth / aspectRatio);
			Vector2 viewportFittingHeight = new(fittingHeight * aspectRatio, fittingHeight);

			// Greater ortho size ensures grid is inbound horizontally and vertically
			float fittingOrthoSize = Mathf.Max(viewportFittingWidth.y / 2, viewportFittingHeight.y / 2);
			return fittingOrthoSize;
		}

		public Camera GetCamera() => camera;
	}
}
