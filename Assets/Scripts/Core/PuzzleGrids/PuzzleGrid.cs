using Core.Grids;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleGrid : SquareGrid<PuzzleCell> {
		public PuzzleGrid(Vector2Int gridSizeInCells, float cellDiameter) : base(
			new PuzzleCellFactory(),
			gridSizeInCells,
			cellDiameter
		) { }
	}
}
