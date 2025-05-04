using Core.Contexts;
using Frolics.Pooling;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleCellBehaviourFactory : MonoBehaviour,IInitializable {
		[SerializeField] private PuzzleCellBehaviour puzzleCellBehaviourPrefab;

		// Dependencies
		private ObjectPool objectPool;

		public void Initialize() {
			this.objectPool = SceneContext.GetInstance().Get<ObjectPool>();
		}

		public PuzzleCellBehaviour Create(PuzzleCell puzzleCell, Transform parent) {
			PuzzleCellBehaviour puzzleCellBehaviour = objectPool.Spawn(puzzleCellBehaviourPrefab, parent);
			puzzleCellBehaviour.Initialize(puzzleCell);
			return puzzleCellBehaviour;
		}
	}
}
