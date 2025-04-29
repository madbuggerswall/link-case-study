using Core.Grids;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// NOTE This class might not be needed, as CellBehaviour can do OK
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
