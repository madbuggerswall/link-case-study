using Core.Contexts;
using Core.Links;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels {
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

	// TODO Implement and utilize LinkManager
	public class LinkManager {
		public LinkManager() { }

		public void EvaluateLink() { }

		public void Explode(Link link) {
			
		}
	}
	public class TargetManager {
		
	}

	public class TurnManager {
		
	}
}
