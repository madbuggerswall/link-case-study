using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels.LevelView.ViewHelpers {
	public class ScaledViewHelper {
		private const float Scale = 1.2f;
		private const float ScaleDuration = 0.2f;

		private readonly Dictionary<Transform, TransformTween> scaleTweens = new();
		private readonly HashSet<PuzzleElement> lastSelection = new();

		private readonly PuzzleLevelViewController viewController;

		public ScaledViewHelper(PuzzleLevelViewController viewController) {
			this.viewController = viewController;
		}

		public void ScaleDownUnselectedElements(HashList<PuzzleElement> puzzleElements) {
			foreach (PuzzleElement puzzleElement in lastSelection) {
				if (puzzleElements.Contains(puzzleElement))
					continue;

				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				PlayScaleTween(elementBehaviour.transform, 1f);
			}
		}

		public void ScaleUpSelectedElements(HashList<PuzzleElement> puzzleElements) {
			SaveLastSelection(puzzleElements);

			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				PlayScaleTween(elementBehaviour.transform, Scale);
			}
		}

		public void ResetSelectedElements(HashList<PuzzleElement> puzzleElements) {
			lastSelection.Clear();

			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				PlayScaleTween(elementBehaviour.transform, 1f);
			}
		}


		private void PlayScaleTween(Transform elementTransform, float scale) {
			if (scaleTweens.TryGetValue(elementTransform, out TransformTween transformTween)) {
				transformTween.Stop();
				scaleTweens.Remove(elementTransform);
			}

			transformTween = new TransformTween(elementTransform, ScaleDuration);
			transformTween.SetEase(Ease.Type.OutQuad);
			transformTween.SetLocalScale(Vector3.one * scale);
			transformTween.SetOnComplete(() => scaleTweens.Remove(elementTransform));
			transformTween.Play();

			scaleTweens.Add(elementTransform, transformTween);
		}

		private void SaveLastSelection(HashList<PuzzleElement> puzzleElements) {
			lastSelection.Clear();
			foreach (PuzzleElement puzzleElement in puzzleElements)
				lastSelection.Add(puzzleElement);
		}
	}
}
