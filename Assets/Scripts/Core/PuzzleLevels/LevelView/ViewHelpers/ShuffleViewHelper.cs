using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels.LevelView.ViewHelpers {
	public class ShuffleViewHelper {
		private const float MoveDuration = 1f;

		private readonly Dictionary<Transform, TransformTween> moveTweens = new();
		private readonly PuzzleLevelViewController viewController;
		private readonly PuzzleGrid puzzleGrid;

		public ShuffleViewHelper(PuzzleLevelViewController viewController, PuzzleGrid puzzleGrid) {
			this.viewController = viewController;
			this.puzzleGrid = puzzleGrid;
		}

		public void MoveShuffledElements() {
			moveTweens.Clear();

			PuzzleCell[] cells = puzzleGrid.GetCells();
			foreach (PuzzleCell cell in cells) {
				if (!cell.TryGetPuzzleElement(out PuzzleElement element))
					continue;

				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(element);
				PlayMoveTween(elementBehaviour.transform, cell.GetWorldPosition());
			}
		}

		private void PlayMoveTween(Transform elementTransform, Vector3 targetPosition) {
			if (moveTweens.TryGetValue(elementTransform, out TransformTween transformTween)) {
				transformTween.Stop();
				moveTweens.Remove(elementTransform);
			}

			transformTween = new TransformTween(elementTransform, MoveDuration);
			transformTween.SetEase(Ease.Type.InOutQuad);
			transformTween.SetPosition(targetPosition);
			transformTween.SetOnComplete(() => OnMoveTweenComplete(elementTransform));
			transformTween.Play();

			moveTweens.Add(elementTransform, transformTween);
		}

		private void OnMoveTweenComplete(Transform elementTransform) {
			if (!moveTweens.Remove(elementTransform))
				return;

			if (moveTweens.Count == 0)
				viewController.ViewReadyNotifier.OnShuffleTweensComplete();
		}
	}
}
