using System;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements {
	public class DuckBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;

		private Duck duck;

		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;

		// PuzzleElementBehaviour
		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not DuckDefinition duckDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(duckDefinition, puzzleCell);
		}

		private void Initialize(DuckDefinition definition, PuzzleCell puzzleCell) {
			spriteRenderer.sprite = definition.GetSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		public override PuzzleElement GetPuzzleElement() => duck;
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
	}
}
