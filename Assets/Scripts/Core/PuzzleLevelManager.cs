using Core.Contexts;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core {
	public class PuzzleLevelManager : MonoBehaviour {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;

		// Fields
		private PuzzleGrid puzzleGrid;

		public void Initialize() {
			this.levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();

			this.puzzleGrid = levelInitializer.GetPuzzleGrid();
		}

		public PuzzleGrid GetPuzzleGrid() => this.puzzleGrid;
	}

	public class LinkManager {
		public LinkManager() { }

		public void EvaluateLink() { }
	}
}
