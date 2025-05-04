using System.Collections.Generic;
using Core.PuzzleElements;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleLevels {
	public class ScaledViewHelper {
		private const float Scale = 1.5f;
		private const float ScaleDuration = 0.5f;

		private readonly Dictionary<Transform, TransformTween> scaleTweens = new();
		private readonly PuzzleLevelViewController viewController;
		private HashList<PuzzleElement> lastSelection = new();

		public ScaledViewHelper(PuzzleLevelViewController viewController) {
			this.viewController = viewController;
		}

		public void ScaleDownUnselectedElements(HashList<PuzzleElement> puzzleElements) {
			for (int index = 0; index < lastSelection.Count; index++) {
				PuzzleElement puzzleElement = lastSelection[index];
				if (puzzleElements.Contains(puzzleElement))
					continue;

				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				PlayScaleTween(elementBehaviour.transform, 1f);
			}
		}

		public void ScaleUpSelectedElements(HashList<PuzzleElement> puzzleElements) {
			this.lastSelection = puzzleElements;

			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				PlayScaleTween(elementBehaviour.transform, Scale);
			}
		}

		private void PlayScaleTween(Transform elementTransform, float scale) {
			if (scaleTweens.TryGetValue(elementTransform, out TransformTween transformTween))
				transformTween.Stop();

			transformTween = new TransformTween(elementTransform, ScaleDuration);
			transformTween.SetLocalScale(Vector3.one * scale);
			transformTween.Play();
			transformTween.SetOnComplete(() => OnScaleTweenComplete(elementTransform));

			scaleTweens.Add(elementTransform, transformTween);
		}

		private void OnScaleTweenComplete(Transform elementTransform) {
			scaleTweens.Remove(elementTransform);
		}
	}
}
