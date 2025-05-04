using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using Frolics.Pooling;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public class PuzzleElementBehaviourFactory : MonoBehaviour, IInitializable {
		[SerializeField] private Transform elementsParent;

		// Dependencies
		private ObjectPool objectPool;

		public void Initialize() {
			this.objectPool = SceneContext.GetInstance().Get<ObjectPool>();
		}

		public PuzzleElementBehaviour Create(PuzzleElement puzzleElement, PuzzleCell puzzleCell) {
			PuzzleElementDefinition elementDefinition = puzzleElement.GetDefinition();
			PuzzleElementBehaviour elementBehaviour = objectPool.Spawn(elementDefinition.GetPrefab(), elementsParent);
			elementBehaviour.Initialize(elementDefinition, puzzleCell);

			puzzleCell.SetPuzzleElement(puzzleElement);
			return elementBehaviour;
		}
	}
}
