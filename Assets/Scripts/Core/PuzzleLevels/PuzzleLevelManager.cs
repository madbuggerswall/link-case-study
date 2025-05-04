using Core.Contexts;
using Core.Links;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels {
	public class PuzzleLevelManager : MonoBehaviour {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;

		// Fields
		private PuzzleGrid puzzleGrid;
		
		private LinkManager linkManager;
		private TurnManager turnManager;
		private ScoreManager scoreManager;

		public void Initialize() {
			this.levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();

			this.puzzleGrid = levelInitializer.GetPuzzleGrid();
		}

		public PuzzleGrid GetPuzzleGrid() => this.puzzleGrid;
	}

	// TODO Implement and utilize LinkManager
	public class LinkManager {
		private PuzzleGrid puzzleGrid;
		public LinkManager() { }

		public void Explode(Link link) {
			if (!link.IsValid(puzzleGrid))
				return;

			link.Explode(puzzleGrid);
			// Fire signal
		}
	}

	public class TargetManager { }

	public class TurnManager { }

	public class ScoreManager { }

	public class FillManager {
		private PuzzleGrid puzzleGrid;

		public void ApplyFall(PuzzleGrid puzzleGrid) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();

			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (columnCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
						puzzleElement.Fall(puzzleGrid);
				}
			}
		}

		private PuzzleCell[] GetEveryCellOfColumn(int columnIndex) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			PuzzleCell[] columnCells = new PuzzleCell[gridSize.y];

			for (int index = 0; index < columnCells.Length; index++) {
				int gridWidth = gridSize.x;
				columnCells[index] = puzzleGrid.GetCell(index * gridWidth + columnIndex);
			}

			return columnCells;
		}

		private bool IsColumnIndexInRange(int columnIndex) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			return columnIndex >= 0 && columnIndex < gridSize.x;
		}
	}
}
