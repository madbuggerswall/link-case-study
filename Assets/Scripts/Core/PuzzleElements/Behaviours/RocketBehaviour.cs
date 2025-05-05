using System;
using Core.DataTransfer.Definitions;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public class RocketBehaviour : PuzzleElementBehaviour {
		[SerializeField] private SpriteRenderer leftSpriteRenderer;
		[SerializeField] private SpriteRenderer rightSpriteRenderer;

		private Rocket rocket;

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

		// Getters
		public override PuzzleElement GetPuzzleElement() => rocket;

		// Setters
		public override void SetSortingOrder(int sortingOrder) {
			leftSpriteRenderer.sortingOrder = sortingOrder;
			rightSpriteRenderer.sortingOrder = sortingOrder;
		}

		public void SetSprites(Sprite leftRocket, Sprite rightRocket) {
			leftSpriteRenderer.sprite = leftRocket;
			rightSpriteRenderer.sprite = rightRocket;
		}
	}
}
