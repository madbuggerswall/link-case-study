using Core.DataTransfer.Definitions;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;

namespace Core.PuzzleElements {
	public class Crate : PuzzleElement {
		private int durability;

		public Crate(CrateDefinition definition) : base(definition) {
			this.durability = definition.GetDurability();
		}

		public override void Explode(PuzzleGrid puzzleGrid) {
			if (puzzleGrid.TryGetPuzzleCell(this, out PuzzleCell puzzleCell))
				puzzleCell.SetCellEmpty();
		}

		public override void OnAdjacentExplode(PuzzleGrid puzzleGrid) {
			durability--;
			if (durability <= 0)
				Explode(puzzleGrid);
		}

		public override void Fall(PuzzleGrid puzzleGrid) { }
	}
}
