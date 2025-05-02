using Core.Grids;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleGrid : SquareGrid<PuzzleCell> {
		public PuzzleGrid(Vector2Int gridSizeInCells, float cellDiameter) : base(
			new PuzzleCellFactory(),
			gridSizeInCells,
			cellDiameter
		) { }
		
		public bool TryGetPuzzleCell(Vector3 worldPosition, out PuzzleCell puzzleCell) {
			for (int i = 0; i < cells.Length; i++) {
				if (!cells[i].IsInsideCell(worldPosition))
					continue;

				puzzleCell = cells[i];
				return true;
			}

			puzzleCell = null;
			return false;
		}
	}
}
