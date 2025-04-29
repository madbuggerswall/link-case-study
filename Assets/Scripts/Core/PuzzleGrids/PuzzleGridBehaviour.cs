using System.Collections.Generic;
using Core.Contexts;
using Frolics.Utilities.Pooling;
using UnityEngine;

namespace Core.PuzzleGrids {
	public class PuzzleGridBehaviour : MonoBehaviour {
		[SerializeField] private Transform cellsParent;
		[SerializeField] private SpriteRenderer backgroundImage;

		private PuzzleGrid puzzleGrid;

		public void Initialize(PuzzleGrid puzzleGrid) {
			this.puzzleGrid = puzzleGrid;
			InitializeBackground(backgroundImage, puzzleGrid);
		}

		private void InitializeBackground(SpriteRenderer background, PuzzleGrid puzzleGrid) {
			const float verticalOffset = 0.088f;
			const float horizontalPadding = 0.24f;
			const float verticalPadding = 0.44f;
			Vector2 puzzleGridSize = puzzleGrid.GetGridSize();

			background.transform.position = puzzleGrid.GetCenterPoint() + Vector3.up * verticalOffset;
			background.size = new Vector2(puzzleGridSize.x + horizontalPadding, puzzleGridSize.y + verticalPadding);
		}

		private void InitializeBackgroundMask(SpriteMask backgroundMask, PuzzleGrid puzzleGrid) {
			backgroundMask.transform.position = puzzleGrid.GetCenterPoint();
			backgroundMask.transform.localScale = puzzleGrid.GetGridSize();
		}

		// Getters
		public PuzzleGrid GetPuzzleGrid() => puzzleGrid;
		public Transform GetCellsParent() => cellsParent;
		
	}
}
