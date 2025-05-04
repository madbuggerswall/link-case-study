using Core.Contexts;
using Frolics.Pooling;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleGridBehaviourFactory : MonoBehaviour {
		[SerializeField] private PuzzleGridBehaviour puzzleGridBehaviourPrefab;
		[SerializeField] private Transform puzzleGridRoot;

		// Dependencies
		private ObjectPool objectPool;

		public void Initialize() {
			this.objectPool = SceneContext.GetInstance().Get<ObjectPool>();
		}

		public PuzzleGridBehaviour Create(PuzzleGrid puzzleGrid) {
			PuzzleGridBehaviour puzzleGridBehaviour = objectPool.Spawn(puzzleGridBehaviourPrefab, puzzleGridRoot);
			puzzleGridBehaviour.Initialize(puzzleGrid);
			return puzzleGridBehaviour;
		}
	}
}
