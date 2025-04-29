using Core.Grids;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleCellFactory : CellFactory<PuzzleCell> {
		public override PuzzleCell Create(Vector2 cellPosition, float diameter) {
			return new PuzzleCell(cellPosition, diameter);
		}
	}
}
