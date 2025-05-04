using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using Frolics.Pooling;
using UnityEngine;

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

		public PuzzleElementBehaviour Create(PuzzleElement puzzleElement, PuzzleCell puzzleCell) {
			PuzzleElementDefinition elementDefinition = puzzleElement.GetDefinition();
			PuzzleElementBehaviour puzzleElementBehaviour = objectPool.Spawn(elementDefinition.GetPrefab(), elementsParent);
			puzzleElementBehaviour.Initialize(elementDefinition, puzzleCell);

			puzzleCell.SetPuzzleElement(puzzleElement);
			return puzzleElementBehaviour;
		}
	}
}
