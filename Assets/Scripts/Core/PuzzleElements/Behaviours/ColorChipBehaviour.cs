using System;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public class ColorChipBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;

		private ColorChip colorChip;

		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not ColorChipDefinition colorCubeDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(colorCubeDefinition, puzzleCell);
		}

		private void Initialize(ColorChipDefinition definition, PuzzleCell puzzleCell) {
			spriteRenderer.sprite = definition.GetSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		// Setters
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
	}
}
