using System;
using Core.DataTransfer.Definitions;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public class VaseBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;

		private Vase vase;

		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not VaseDefinition balloonDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(balloonDefinition, puzzleCell);
		}

		private void Initialize(VaseDefinition definition, PuzzleCell puzzleCell) {
			spriteRenderer.sprite = definition.GetSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		// Getters
		public override PuzzleElement GetPuzzleElement() => vase;

		// Setters
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
	}
}
