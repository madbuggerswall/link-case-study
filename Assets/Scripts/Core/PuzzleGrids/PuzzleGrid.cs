using Core.Grids;
using Core.PuzzleElements;
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

		public bool TryGetPuzzleCell(PuzzleElement puzzleElement, out PuzzleCell puzzleCell) {
			for (int i = 0; i < cells.Length; i++) {
				if (!cells[i].TryGetPuzzleElement(out PuzzleElement otherElement))
					continue;

				if (otherElement != puzzleElement)
					continue;

				puzzleCell = cells[i];
				return true;
			}

			puzzleCell = null;
			return false;
		}
		
		public int GetCellIndex(PuzzleCell puzzleCell) {
			for (int index = 0; index < cells.Length; index++)
				if (puzzleCell == cells[index])
					return index;

			return -1;
		}
		
		public bool IsTopEdge(int cellIndex) => neighborHelper.IsTopEdge(cellIndex);
		public bool IsBottomEdge(int cellIndex) => neighborHelper.IsBottomEdge(cellIndex);
		public bool IsRightEdge(int cellIndex) => neighborHelper.IsRightEdge(cellIndex);
		public bool IsLeftEdge(int cellIndex) => neighborHelper.IsLeftEdge(cellIndex);
	}
}
