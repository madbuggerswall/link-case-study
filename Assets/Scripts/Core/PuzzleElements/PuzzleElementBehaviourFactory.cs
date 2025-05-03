using System;
using System.Collections.Generic;
using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using Frolics.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PuzzleElements {
	public class PuzzleElementBehaviourFactory : MonoBehaviour {
		[Header("Color Chip Definitions")]
		[SerializeField] private ColorChipDefinition[] colorChipDefinitions;

		[Header("Core")]
		[SerializeField] private Transform elementsParent;

		
		// Dependencies
		private ObjectPool objectPool;

		public void Initialize() {
			this.objectPool = SceneContext.GetInstance().Get<ObjectPool>();
		}
		
		public PuzzleElementBehaviour Create(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			PuzzleElement puzzleElement = definition.CreateElement();
			puzzleCell.SetPuzzleElement(puzzleElement);

			PuzzleElementBehaviour puzzleElementBehaviour = objectPool.Spawn(definition.GetPrefab(), elementsParent);
			puzzleElementBehaviour.Initialize(definition, puzzleCell);

			return puzzleElementBehaviour;
		}

		public PuzzleElementBehaviour Create(PuzzleElement puzzleElement, PuzzleCell puzzleCell) {
			PuzzleElementDefinition elementDefinition = puzzleElement.GetDefinition();
			PuzzleElementBehaviour puzzleElementBehaviour = objectPool.Spawn(elementDefinition.GetPrefab(), elementsParent);
			puzzleElementBehaviour.Initialize(elementDefinition, puzzleCell);

			puzzleCell.SetPuzzleElement(puzzleElement);
			return puzzleElementBehaviour;
		}
	}
}
