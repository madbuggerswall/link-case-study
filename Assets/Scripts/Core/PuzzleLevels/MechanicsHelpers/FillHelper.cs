using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels.MechanicsHelpers {
	public class FillHelper {
		private readonly PuzzleLevelManager levelManager;
		private readonly HashSet<PuzzleElement> filledElements = new();

		public FillHelper(PuzzleLevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public void ApplyFill() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			filledElements.Clear();

			// Assumes that a fall operation has already resolved empty spaces
			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (columnCell.TryGetPuzzleElement(out _))
						continue;

					ColorChip colorChip = levelManager.CreateRandomColorChip();
					columnCell.SetPuzzleElement(colorChip);
					filledElements.Add(colorChip);
				}
			}
		}

		public HashSet<PuzzleElement> GetFilledElements() => filledElements;
	}
}
