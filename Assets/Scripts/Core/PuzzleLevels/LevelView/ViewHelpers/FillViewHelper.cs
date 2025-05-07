using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels.LevelView.ViewHelpers {
	public class FillViewHelper {
		private const float FillDuration = 0.6f;

		private readonly Dictionary<Transform, TransformTween> fillTweens = new();
		private readonly PuzzleLevelViewController viewController;
		private readonly PuzzleGrid puzzleGrid;

		public FillViewHelper(PuzzleLevelViewController viewController, PuzzleGrid puzzleGrid) {
			this.viewController = viewController;
			this.puzzleGrid = puzzleGrid;
		}

		public void MoveFilledElements(HashSet<PuzzleElement> filledElements) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			Dictionary<int, int> filledElementByColumn = new();
			fillTweens.Clear();

			foreach (PuzzleElement filledElement in filledElements) {
				if (!puzzleGrid.TryGetPuzzleCell(filledElement, out PuzzleCell cell))
					return;

				int cellIndex = puzzleGrid.GetCellIndex(cell);
				int cellColumn = cellIndex % gridSize.x;
				int cellRow = Mathf.FloorToInt((float) cellIndex / gridSize.x);

				if (!filledElementByColumn.TryAdd(cellColumn, 1))
					filledElementByColumn[cellColumn]++;

				PuzzleCell topColumnCell = puzzleGrid.GetCell((gridSize.y - 1) * gridSize.x + cellColumn);
				Vector3 startPosition = topColumnCell.GetWorldPosition();
				startPosition.y += puzzleGrid.GetCellDiameter() * filledElementByColumn[cellColumn];

				PuzzleElementBehaviour elementBehaviour = viewController.SpawnElementBehaviour(filledElement, cell);
				elementBehaviour.transform.position = startPosition;
				PlayFillTween(elementBehaviour.transform, cell.GetWorldPosition());
			}
		}

		private void PlayFillTween(Transform elementTransform, Vector3 targetPosition) {
			if (fillTweens.TryGetValue(elementTransform, out TransformTween transformTween)) {
				transformTween.Stop();
				fillTweens.Remove(elementTransform);
			}

			transformTween = new TransformTween(elementTransform, FillDuration);
			transformTween.SetEase(Ease.Type.InCubic);
			transformTween.SetPosition(targetPosition);
			transformTween.Play();
			transformTween.SetOnComplete(() => OnFillTweenComplete(elementTransform));

			fillTweens.Add(elementTransform, transformTween);
		}

		private void OnFillTweenComplete(Transform elementTransform) {
			if (!fillTweens.Remove(elementTransform))
				return;

			if (fillTweens.Count == 0)
				viewController.ViewReadyNotifier.OnFillTweensComplete();
		}
	}
}
