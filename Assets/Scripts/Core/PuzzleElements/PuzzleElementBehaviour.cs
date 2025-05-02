using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core.PuzzleElements {
	public abstract class PuzzleElementBehaviour : MonoBehaviour {
		public abstract void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell);
		public abstract PuzzleElement GetPuzzleElement();
		public abstract void SetSortingOrder(int sortingOrder);

		private TransformTween transformTween;

		public void PlayScaleTween(float scale) {
			transformTween?.Stop();
			transformTween = new TransformTween(transform, 0.5f);
			transformTween.SetLocalScale(Vector3.one * scale);
			transformTween.Play();
		}
	}
}
