using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;

namespace Core.PuzzleElements {
	public abstract class PuzzleElement {
		private readonly PuzzleElementDefinition definition;

		protected PuzzleElement(PuzzleElementDefinition definition) {
			this.definition = definition;
		}

		public abstract void Explode(PuzzleGrid puzzleGrid);
		public abstract void OnAdjacentExplode(PuzzleGrid puzzleGrid);
		public abstract void Fall(PuzzleGrid puzzleGrid);


		// Sandbox methods
		protected bool GetFallTarget(PuzzleGrid puzzleGrid, PuzzleCell currentCell, out PuzzleCell targetCell) {
			bool targetCellFound = false;
			targetCell = null;

			int gridWith = puzzleGrid.GetGridSizeInCells().x;
			int cellBelowIndex = puzzleGrid.GetCellIndex(currentCell) - gridWith;

			while (cellBelowIndex >= 0) {
				PuzzleCell cellBelow = puzzleGrid.GetCell(cellBelowIndex);
				if (cellBelow.TryGetPuzzleElement(out _))
					break;

				cellBelowIndex -= gridWith;
				targetCell = cellBelow;
				targetCellFound = true;
			}

			return targetCellFound;
		}

		// Getters
		public PuzzleElementDefinition GetDefinition() => definition;
	}
}
