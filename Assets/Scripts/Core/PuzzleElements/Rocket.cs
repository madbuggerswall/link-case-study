using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;

namespace Core.PuzzleElements {
	public class Rocket : PuzzleElement {
		public Rocket(RocketDefinition definition) : base(definition) { }

		public override void Explode(PuzzleGrid puzzleGrid) {
			ExplodeHorizontally(puzzleGrid);
		}

		public override void OnAdjacentExplode(PuzzleGrid puzzleGrid) {
			throw new System.NotImplementedException();
		}

		public override void Fall(PuzzleGrid puzzleGrid) {
			if (!puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell currentCell))
				return;

			if (!GetFallTarget(puzzleGrid, currentCell, out PuzzleCell targetCell))
				return;

			currentCell.SetCellEmpty();
			targetCell.SetPuzzleElement(this);
		}

		// Helper methods
		private void ExplodeHorizontally(PuzzleGrid puzzleGrid) {
			if (!puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell puzzleCell))
				return;

			int cellIndex = puzzleGrid.GetCellIndex(puzzleCell);
			for (int i = cellIndex; !puzzleGrid.IsLeftEdge(i); i--)
				if (puzzleGrid.GetCell(i).TryGetPuzzleElement(out PuzzleElement puzzleElement))
					puzzleElement.Explode(puzzleGrid); // Should not trigger adjacent cells

			for (int i = cellIndex; !puzzleGrid.IsRightEdge(i); i++)
				if (puzzleGrid.GetCell(i).TryGetPuzzleElement(out PuzzleElement puzzleElement))
					puzzleElement.Explode(puzzleGrid); // Should not trigger adjacent cells
		}

		private void ExplodeVertically(PuzzleGrid puzzleGrid) {
			if (!puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell puzzleCell))
				return;

			int cellIndex = puzzleGrid.GetCellIndex(puzzleCell);
			for (int i = cellIndex; !puzzleGrid.IsLeftEdge(i); i--)
				if (puzzleGrid.GetCell(i).TryGetPuzzleElement(out PuzzleElement puzzleElement))
					puzzleElement.Explode(puzzleGrid); // Should not trigger adjacent cells

			for (int i = cellIndex; !puzzleGrid.IsRightEdge(i); i++)
				if (puzzleGrid.GetCell(i).TryGetPuzzleElement(out PuzzleElement puzzleElement))
					puzzleElement.Explode(puzzleGrid); // Should not trigger adjacent cells
		}
	}
}
