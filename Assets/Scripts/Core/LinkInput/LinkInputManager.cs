using System;
using System.Collections.Generic;
using System.Windows.Input;
using Core.Commands;
using Core.Contexts;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core {
	public class LinkInputManager : MonoBehaviour {
		private readonly HashList<PuzzleElement> puzzleElements = new();

		private PuzzleCellDragHelper dragHelper;
		private PuzzleElementBehaviourFactory elementBehaviourFactory;
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelViewController viewController;
		private CommandInvoker commandInvoker;
		

		public void Initialize() {
			SceneContext sceneContext = SceneContext.GetInstance();
			dragHelper = sceneContext.Get<PuzzleCellDragHelper>();
			elementBehaviourFactory = sceneContext.Get<PuzzleElementBehaviourFactory>();
			levelInitializer = sceneContext.Get<PuzzleLevelInitializer>();
			commandInvoker = sceneContext.Get<CommandInvoker>();

			dragHelper.OnCellsChanged.AddListener(OnCellsChanged);
			dragHelper.OnCellsSelected.AddListener(OnCellsSelected);
		}

		private void OnCellsChanged() {
			GetSelectedElements();

			PuzzleCell[] puzzleCells = levelInitializer.GetPuzzleGrid().GetCells();
			for (int index = 0; index < puzzleCells.Length; index++) {
				PuzzleCell puzzleCell = puzzleCells[index];
				if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					return;

				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				elementBehaviour.PlayScaleTween(1f);
			}

			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
				elementBehaviour.PlayScaleTween(1.5f);
			}
		}

		private void OnCellsSelected() {
			GetSelectedElements();

			Link link = new(puzzleElements);
			ExplodeLinkCommand command = new ExplodeLinkCommand(link);
			commandInvoker.Enqueue(command);
		}

		private void GetSelectedElements() {
			HashList<PuzzleCell> selectedCells = dragHelper.PuzzleCells;
			puzzleElements.Clear();

			for (int index = 0; index < selectedCells.Count; index++) {
				PuzzleCell selectedCell = selectedCells[index];

				if (!selectedCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					break;

				if (puzzleElements.Count == 0)
					puzzleElements.TryAdd(puzzleElement);

				if (puzzleElements[^1].GetDefinition() != puzzleElement.GetDefinition())
					break;

				puzzleElements.TryAdd(puzzleElement);
			}
		}
	}

	// TODO Rename PuzzleElement to Chip/PuzzleChip
}
