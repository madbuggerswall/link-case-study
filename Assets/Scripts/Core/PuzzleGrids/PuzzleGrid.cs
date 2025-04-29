using System;
using Core.Grids;
using Core.Grids.NeighborHelpers;
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
