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

	public class PuzzleCellFactory : SquareCellFactory<PuzzleCell> {
		public override PuzzleCell Create(Vector2 cellPosition, float diameter) {
			return new PuzzleCell(cellPosition, diameter);
		}
	}
}
