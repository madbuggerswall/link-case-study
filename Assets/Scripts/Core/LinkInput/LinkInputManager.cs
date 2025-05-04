using Core.Commands;
using Core.Contexts;
using Core.Links;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using UnityEngine;

namespace Core.LinkInput {
	public class LinkInputManager : MonoBehaviour {
		private readonly HashList<PuzzleElement> puzzleElements = new();

		// Dependencies
		private PuzzleCellDragHelper dragHelper;
		private PuzzleLevelViewController viewController;
		private CommandInvoker commandInvoker;


		public void Initialize() {
			dragHelper = SceneContext.GetInstance().Get<PuzzleCellDragHelper>();
			commandInvoker = SceneContext.GetInstance().Get<CommandInvoker>();

			dragHelper.CellSelectionChangeAction += OnCellsSelectionChanged;
			dragHelper.CellSelectionAcceptedAction += OnCellSelectionAccepted;
		}

		private void OnCellsSelectionChanged() {
			UpdateSelectedElements();
			viewController.ScaleDownUnselectedElements(puzzleElements);
			viewController.ScaleUpSelectedElements(puzzleElements);
		}

		private void OnCellSelectionAccepted() {
			UpdateSelectedElements();

			Link link = new(puzzleElements);
			ExplodeLinkCommand command = new ExplodeLinkCommand(link);
			commandInvoker.Enqueue(command);
		}

		private void UpdateSelectedElements() {
			HashList<PuzzleCell> selectedCells = dragHelper.GetPuzzleCells();
			puzzleElements.Clear();

			for (int index = 0; index < selectedCells.Count; index++) {
				PuzzleCell selectedCell = selectedCells[index];

				if (!selectedCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					break;

				// NOTE This part seems very similar to DragHelper.OnDrag, maybe it can be accessed by LinkManager
				if (puzzleElements.Count == 0)
					puzzleElements.TryAdd(puzzleElement);

				if (puzzleElements[^1].GetDefinition() != puzzleElement.GetDefinition())
					break;

				puzzleElements.TryAdd(puzzleElement);
			}
		}
	}
}
