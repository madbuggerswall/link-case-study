using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleCellBehaviour : MonoBehaviour {
		[SerializeField] private SpriteRenderer spriteRenderer;
		
		private PuzzleCell puzzleCell;

		public void Initialize(PuzzleCell puzzleCell) {
			this.puzzleCell = puzzleCell;
			transform.position = puzzleCell.GetWorldPosition();
			
			SetCellSpriteSize(puzzleCell.GetDiameter());
		}

		private void SetCellSpriteSize(float cellDiameter) {
			spriteRenderer.size = new Vector2(cellDiameter, cellDiameter);
		}

		public PuzzleCell GetPuzzleCell() => puzzleCell;
	}
}
