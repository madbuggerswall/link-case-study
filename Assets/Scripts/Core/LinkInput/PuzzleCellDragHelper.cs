using Core.Contexts;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using UnityEngine;

namespace Core.LinkInput {
	// NOTE Rename to LinkDragHelper
	// NOTE This should be a vanilla class initialized by LinkInputManager
	public class PuzzleCellDragHelper : IInitializable {
		private readonly HashList<PuzzleCell> selectedCells = new();

		// Dependencies
		private PuzzleGrid puzzleGrid;
		private LinkInputManager linkInputManager;

		public void Initialize() {
			puzzleGrid = SceneContext.GetInstance().Get<PuzzleLevelInitializer>().GetPuzzleGrid();
			linkInputManager = SceneContext.GetInstance().Get<LinkInputManager>();
		}


		public void OnDrag(Vector3 dragPosition) {
			if (!puzzleGrid.TryGetPuzzleCell(dragPosition, out PuzzleCell puzzleCell))
				return;

			if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
				return;

			// Add first cell
			if (selectedCells.Count == 0) {
				SelectCell(puzzleCell);
				return;
			}

			// Reject non-adjacent cell
			PuzzleCell lastAddedCell = selectedCells[^1];
			if (!IsCellsAdjacent(lastAddedCell, puzzleCell))
				return;

			// Handle backtracking (player dragging back one step)	
			if (selectedCells.Count > 1 && puzzleCell == selectedCells[^2]) {
				DeselectCell(lastAddedCell);
				return;
			}

			// Select cell if its definitions match
			lastAddedCell.TryGetPuzzleElement(out PuzzleElement lastAddedElement);
			if (lastAddedElement.GetDefinition() == puzzleElement.GetDefinition())
				SelectCell(puzzleCell);
		}

		public void OnRelease() {
			linkInputManager.OnCellSelectionAccepted(selectedCells);
			selectedCells.Clear();
		}

		// Helper methods
		private void SelectCell(PuzzleCell puzzleCell) {
			if (selectedCells.TryAdd(puzzleCell))
				linkInputManager.OnCellsSelectionChanged(selectedCells);
		}

		private void DeselectCell(PuzzleCell lastAddedCell) {
			if (selectedCells.TryRemove(lastAddedCell))
				linkInputManager.OnCellsSelectionChanged(selectedCells);
		}

		private bool IsCellsAdjacent(PuzzleCell centerCell, PuzzleCell cell) {
			PuzzleCell[] cellNeighbors = puzzleGrid.GetNeighbors(centerCell);

			for (int i = 0; i < cellNeighbors.Length; i++)
				if (cell == cellNeighbors[i])
					return true;

			return false;
		}

		public HashList<PuzzleCell> GetSelectedCells() => selectedCells;
	}
}
