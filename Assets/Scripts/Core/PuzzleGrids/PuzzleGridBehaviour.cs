using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleGridBehaviour : MonoBehaviour {
		[SerializeField] private Transform cellsParent;
		[SerializeField] private SpriteRenderer backgroundImage;
		[SerializeField] private SpriteMask backgroundMask;

		public void Initialize(PuzzleGrid puzzleGrid) {
			InitializeBackground(backgroundImage, puzzleGrid);
			InitializeBackgroundMask(backgroundMask, puzzleGrid);
		}

		private void InitializeBackground(SpriteRenderer background, PuzzleGrid puzzleGrid) {
			background.enabled = false;

			Vector2 padding = new Vector2(1, 1);
			Vector2 puzzleGridSize = puzzleGrid.GetGridSize();
			background.transform.position = puzzleGrid.GetCenterPoint();
			background.size = new Vector2(puzzleGridSize.x + padding.x, puzzleGridSize.y + padding.y);
		}

		private void InitializeBackgroundMask(SpriteMask backgroundMask, PuzzleGrid puzzleGrid) {
			backgroundMask.transform.position = puzzleGrid.GetCenterPoint();
			backgroundMask.transform.localScale = puzzleGrid.GetGridSize();
		}

		// Getters
		public Transform GetCellsParent() => cellsParent;
	}
}
