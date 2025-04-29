using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using Frolics.Utilities.Pooling;
using UnityEngine;

namespace Core.PuzzleElements {
	public class PuzzleElementBehaviourFactory : MonoBehaviour {
		[Header("Color Cube Definitions")]
		[SerializeField] private ColorChipDefinition[] colorCubeDefinitions;

		[Header("Core")]
		[SerializeField] private Transform elementsParent;

		// Dependencies
		private ObjectPool objectPool;

		public void Initialize() {
			this.objectPool = SceneContext.GetInstance().Get<ObjectPool>();
		}

		public PuzzleElementBehaviour CreateRandomColorChip(PuzzleCell puzzleCell) {
			int randomIndex = Random.Range(0, colorCubeDefinitions.Length);
			ColorChipDefinition definition = colorCubeDefinitions[randomIndex];

			return Create(definition, puzzleCell);
		}

		public PuzzleElementBehaviour Create(PuzzleElementDefinition definition, PuzzleCell puzzleCell) {
			PuzzleElement puzzleElement = definition.CreateElement();
			puzzleCell.SetPuzzleElement(puzzleElement);

			PuzzleElementBehaviour puzzleElementBehaviour = objectPool.Spawn(definition.GetPrefab(), elementsParent);
			puzzleElementBehaviour.Initialize(definition, puzzleCell);

			return puzzleElementBehaviour;
		}

		public void Despawn(PuzzleElementBehaviour puzzleElementBehaviour) {
			objectPool.Despawn(puzzleElementBehaviour.GetPuzzleElement().GetDefinition().GetPrefab());
		}
	}
}
