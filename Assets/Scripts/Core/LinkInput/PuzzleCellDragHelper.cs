using Core.Contexts;
using Core.Input;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using UnityEngine;
using UnityEngine.Events;

namespace Core.LinkInput {
	public class PuzzleCellDragHelper : MonoBehaviour {
		private InputController inputController;
		private InputHandler inputHandler;

		private Vector3 pressPosition;
		private Vector3 dragPosition;
		private Vector3 releasePosition;

		private bool isDragging;
		private const float DragThreshold = 0.5f;

		private PuzzleGrid puzzleGrid;
		private readonly HashList<PuzzleCell> puzzleCells = new();
		public HashList<PuzzleCell> GetPuzzleCells() => puzzleCells;

		public UnityEvent OnCellSelectionChanged { get; } = new();
		public UnityEvent OnCellsSelected { get; } = new();


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
			if (puzzleCells.Count == 0) {
				SelectCell(puzzleCell);
				return;
			}

			// Reject non-adjacent cell
			PuzzleCell lastAddedCell = puzzleCells[^1];
			if (!IsCellsAdjacent(lastAddedCell, puzzleCell))
				return;

			// Handle backtracking (player dragging back one step)	
			if (puzzleCells.Count > 1 && puzzleCell == puzzleCells[^2]) {
				DeselectCell(lastAddedCell);
				return;
			}

			// Select cell if it definitions match
			lastAddedCell.TryGetPuzzleElement(out PuzzleElement lastAddedElement);
			if (lastAddedElement.GetDefinition() == puzzleElement.GetDefinition())
				SelectCell(puzzleCell);
		}

		private void OnRelease(PointerReleaseData releaseData) {
			releasePosition = inputController.ScreenPositionToWorldSpace(releaseData.ReleasePosition);
			pressPosition = releasePosition;

			OnCellsSelected.Invoke();
			puzzleCells.Clear();

			isDragging = false;
		}

		// Helper methods
		private void SelectCell(PuzzleCell puzzleCell) {
			if (puzzleCells.TryAdd(puzzleCell))
				OnCellSelectionChanged.Invoke();
		}

		private void DeselectCell(PuzzleCell lastAddedCell) {
			if (puzzleCells.TryRemove(lastAddedCell))
				OnCellSelectionChanged.Invoke();
		}

		private bool IsCellsAdjacent(PuzzleCell centerCell, PuzzleCell cell) {
			PuzzleCell[] cellNeighbors = puzzleGrid.GetNeighbors(centerCell);

			for (int i = 0; i < cellNeighbors.Length; i++)
				if (cell == cellNeighbors[i])
					return true;

			return false;
		}
	}
}
