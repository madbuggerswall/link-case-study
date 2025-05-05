using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Signals;

namespace Core.PuzzleElements {
	public class Vase : PuzzleElement {
		private int durability;

		public Vase(VaseDefinition definition) : base(definition) {
			this.durability = definition.GetDurability();
		}

		public override void Explode(PuzzleGrid puzzleGrid) {
			if (!puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell puzzleCell))
				return;

			puzzleCell.SetCellEmpty();
			SignalBus.GetInstance().Fire(new ElementExplodedSignal(this));
		}

		public override void OnAdjacentExplode(PuzzleGrid puzzleGrid) {
			durability--;
			if (durability <= 0)
				Explode(puzzleGrid);
		}

		public override void Fall(PuzzleGrid puzzleGrid) {
			if (!puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell currentCell))
				return;

			if (!GetFallTarget(puzzleGrid, currentCell, out PuzzleCell targetCell))
				return;

			currentCell.SetCellEmpty();
			targetCell.SetPuzzleElement(this);
		}
	}
}
