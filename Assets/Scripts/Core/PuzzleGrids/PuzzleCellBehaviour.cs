using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleCellBehaviour : MonoBehaviour {
		private PuzzleCell puzzleCell;

		public void Initialize(PuzzleCell puzzleCell) {
			this.puzzleCell = puzzleCell;
			transform.position = puzzleCell.GetWorldPosition();
		}

		public PuzzleCell GetPuzzleCell() => puzzleCell;
	}
}
