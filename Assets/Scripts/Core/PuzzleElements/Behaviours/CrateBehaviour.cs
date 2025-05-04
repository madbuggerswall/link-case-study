using System;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public class CrateBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;

		private Crate crate;

		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not CrateDefinition duckDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(duckDefinition, puzzleCell);
		}

		private void Initialize(CrateDefinition definition, PuzzleCell puzzleCell) {
			spriteRenderer.sprite = definition.GetSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		// Getters
		public override PuzzleElement GetPuzzleElement() => crate;

		// Setters
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
	}
}
