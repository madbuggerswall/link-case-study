using System;
using Core.Contexts;
using Core.Input;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using UnityEngine;

namespace Core.LinkInput {
	// NOTE Rename to LinkDragHelper
	// NOTE This should be a vanilla class initialized by LinkInputManager
	public class PuzzleCellDragHelper : MonoBehaviour {
		private InputController inputController;
		private InputHandler inputHandler;

		private Vector3 pressPosition;
		private Vector3 dragPosition;
		private Vector3 releasePosition;

		private bool isDragging;
		private const float DragThreshold = 0.5f;

		private PuzzleGrid puzzleGrid;
		private readonly HashList<PuzzleCell> selectedCells = new();

		private LinkInputManager linkInputManager;

		public void Initialize() {
			SceneContext sceneContext = SceneContext.GetInstance();
			inputController = sceneContext.Get<InputController>();
			puzzleGrid = sceneContext.Get<PuzzleLevelInitializer>().GetPuzzleGrid();

			inputHandler = inputController.InputHandler;
			inputHandler.PressEvent.AddListener(OnPress);
			inputHandler.ReleaseEvent.AddListener(OnRelease);
		}

		private void Update() {
			dragPosition = inputController.ScreenPositionToWorldSpace(inputHandler.PointerPosition);
			if (isDragging)
				OnDrag();
		}

		private void OnPress(PointerPressData pressData) {
			pressPosition = inputController.ScreenPositionToWorldSpace(pressData.PressPosition);
			isDragging = true;
		}

		private void OnDrag() {
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

		private void OnRelease(PointerReleaseData releaseData) {
			releasePosition = inputController.ScreenPositionToWorldSpace(releaseData.ReleasePosition);
			pressPosition = releasePosition;

			linkInputManager.OnCellSelectionAccepted();
			selectedCells.Clear();

			isDragging = false;
		}

		// Helper methods
		private void SelectCell(PuzzleCell puzzleCell) {
			if (selectedCells.TryAdd(puzzleCell))
				linkInputManager.OnCellsSelectionChanged();
		}

		private void DeselectCell(PuzzleCell lastAddedCell) {
			if (selectedCells.TryRemove(lastAddedCell))
				linkInputManager.OnCellsSelectionChanged();
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
