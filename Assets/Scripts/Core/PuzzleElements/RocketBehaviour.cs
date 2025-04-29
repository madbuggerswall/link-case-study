using System;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements {
	public class RocketBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer leftSpriteRenderer;
		[SerializeField] private SpriteRenderer rightSpriteRenderer;

		private Rocket rocket;

		public void SetSprites(Sprite leftRocket, Sprite rightRocket) {
			leftSpriteRenderer.sprite = leftRocket;
			rightSpriteRenderer.sprite = rightRocket;
		}

		// PuzzleElementBehaviour
		public override void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			if (definition is not RocketDefinition rocketDefinition)
				throw new Exception("Invalid PuzzleElementDefinition!");

			Initialize(rocketDefinition, puzzleCell);
		}

		private void Initialize(RocketDefinition definition, PuzzleCell puzzleCell) {
			leftSpriteRenderer.sprite = definition.GetLeftRocketSprite();
			rightSpriteRenderer.sprite = definition.GetRightRocketSprite();
			transform.position = puzzleCell.GetWorldPosition();
		}

		public override PuzzleElement GetPuzzleElement() => rocket;

		public override void SetSortingOrder(int sortingOrder) {
			leftSpriteRenderer.sortingOrder = sortingOrder;
			rightSpriteRenderer.sortingOrder = sortingOrder;
		}
	}
}
