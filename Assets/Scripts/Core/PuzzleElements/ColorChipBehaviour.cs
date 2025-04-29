using System;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements {
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

		public override PuzzleElement GetPuzzleElement() => colorChip;

		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
	}
}
