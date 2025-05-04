using Core.Grids;
using Core.PuzzleElements;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleCell : SquareCell {
		private PuzzleElement puzzleElement;
		private bool isEmpty;

		public PuzzleCell(Vector3 worldPosition, float diameter) : base(worldPosition, diameter) {
			this.isEmpty = true;
		}

		public void SetPuzzleElement(PuzzleElement puzzleElement) {
			this.puzzleElement = puzzleElement;
			this.isEmpty = false;
		}

		public void SetCellEmpty() {
			this.puzzleElement = null;
			this.isEmpty = true;
		}

		public bool TryGetPuzzleElement(out PuzzleElement puzzleElement) {
			puzzleElement = this.puzzleElement;
			return !isEmpty;
		}
	}
}
