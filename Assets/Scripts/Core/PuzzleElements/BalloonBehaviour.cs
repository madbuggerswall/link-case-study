using System;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements {
	public class BalloonBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;

		private Balloon balloon;

		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;

		// PuzzleElementBehaviour
		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not BalloonDefinition balloonDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(balloonDefinition, puzzleCell);
		}

		private void Initialize(BalloonDefinition definition, PuzzleCell puzzleCell) {
			spriteRenderer.sprite = definition.GetSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		public override PuzzleElement GetPuzzleElement() => balloon;
		public override void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
	}
}
