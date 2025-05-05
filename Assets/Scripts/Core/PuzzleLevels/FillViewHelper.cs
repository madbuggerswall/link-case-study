using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels {
	public class FillViewHelper {
		private const float FillDuration = 0.5f;

		private readonly Dictionary<Transform, TransformTween> fillTweens = new();
		private readonly PuzzleLevelViewController viewController;
		private readonly PuzzleGrid puzzleGrid;

		public FillViewHelper(PuzzleLevelViewController viewController, PuzzleGrid puzzleGrid) {
			this.viewController = viewController;
			this.puzzleGrid = puzzleGrid;
		}

		public void MoveFilledElements(HashSet<PuzzleElement> filledElements) {
			Vector2 gridSize = puzzleGrid.GetGridSize();
			Vector3 centerPoint = puzzleGrid.GetCenterPoint();
			float upperEdge = centerPoint.y + gridSize.y / 2f;

			foreach (PuzzleElement filledElement in filledElements) {
				if (!puzzleGrid.TryGetPuzzleCell(filledElement, out PuzzleCell cell))
					return;

				PuzzleElementBehaviour elementBehaviour = viewController.SpawnElementBehaviour(filledElement, cell);
				Vector3 startPosition = elementBehaviour.transform.position;
				startPosition.y += upperEdge - startPosition.y;
				elementBehaviour.transform.position = startPosition;
				
				PlayFallTween(elementBehaviour.transform, cell.GetWorldPosition());
			}
		}

		private void PlayFallTween(Transform elementTransform, Vector3 targetPosition) {
			if (fillTweens.TryGetValue(elementTransform, out TransformTween transformTween)) {
				transformTween.Stop();
				fillTweens.Remove(elementTransform);
			}

			transformTween = new TransformTween(elementTransform, FillDuration);
			transformTween.SetEase(Ease.Type.InQuad);
			transformTween.SetPosition(targetPosition);
			transformTween.Play();
			transformTween.SetOnComplete(() => OnFillTweenComplete(elementTransform));

			fillTweens.Add(elementTransform, transformTween);
		}

		private void OnFillTweenComplete(Transform elementTransform) {
			fillTweens.Remove(elementTransform);
			if (fillTweens.Count == 0)
				viewController.OnFallTweensComplete();
		}
	}
}
