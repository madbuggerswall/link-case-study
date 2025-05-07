using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels.LevelView.ViewHelpers {
	public class FallViewHelper {
		private const float FallDuration = 0.6f;

		private readonly Dictionary<Transform, TransformTween> fallTweens = new();
		private readonly PuzzleLevelViewController viewController;
		private readonly PuzzleGrid puzzleGrid;

		public FallViewHelper(PuzzleLevelViewController viewController, PuzzleGrid puzzleGrid) {
			this.viewController = viewController;
			this.puzzleGrid = puzzleGrid;
		}

		public void MoveFallenElements(HashSet<PuzzleElement> fallenElements) {
			fallTweens.Clear();

			foreach (PuzzleElement fallenElement in fallenElements) {
				if (!puzzleGrid.TryGetPuzzleCell(fallenElement, out PuzzleCell cell))
					return;

				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(fallenElement);
				PlayFallTween(elementBehaviour.transform, cell.GetWorldPosition());
			}
		}

		private void PlayFallTween(Transform elementTransform, Vector3 targetPosition) {
			if (fallTweens.TryGetValue(elementTransform, out TransformTween transformTween)) {
				transformTween.Stop();
				fallTweens.Remove(elementTransform);
			}

			transformTween = new TransformTween(elementTransform, FallDuration);
			transformTween.SetEase(Ease.Type.InCubic);
			transformTween.SetPosition(targetPosition);
			transformTween.SetOnComplete(() => OnFallTweenComplete(elementTransform));
			transformTween.Play();

			fallTweens.Add(elementTransform, transformTween);
		}

		private void OnFallTweenComplete(Transform elementTransform) {
			if (!fallTweens.Remove(elementTransform))
				return;

			if (fallTweens.Count == 0)
				viewController.ViewReadyNotifier.OnFallTweensComplete();
		}
	}
}
