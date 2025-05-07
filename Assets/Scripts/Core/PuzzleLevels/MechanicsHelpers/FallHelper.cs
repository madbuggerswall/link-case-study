using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels.MechanicsHelpers {
	public class FallHelper {
		private readonly PuzzleLevelManager levelManager;
		private readonly HashSet<PuzzleElement> fallenElements = new();

		public FallHelper(PuzzleLevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public void ApplyFall() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			fallenElements.Clear();

			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (!columnCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
						continue;

					puzzleElement.Fall(puzzleGrid);
					fallenElements.Add(puzzleElement);
				}
			}
		}

		// Getters
		public HashSet<PuzzleElement> GetFallenElements() => fallenElements;
	}
}
