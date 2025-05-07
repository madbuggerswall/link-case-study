using System.Collections.Generic;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels.Links;
using UnityEngine;

namespace Core.PuzzleLevels.MechanicsHelpers {
	public class ShuffleHelper {
		private readonly PuzzleLevelManager levelManager;
		private readonly LinkFinder linkFinder;

		private readonly Dictionary<ColorChipDefinition, HashSet<PuzzleElement>> elementsByDefinition = new();

		public ShuffleHelper(PuzzleLevelManager levelManager) {
			this.levelManager = levelManager;
			linkFinder = new LinkFinder(levelManager.GetPuzzleGrid());
		}

		public bool IsShuffleNeeded() {
			return !linkFinder.TryFindLinks(out _);
		}

		public void Shuffle() {
			MapColorChipCountByDefinition();
			CreateLink(Link.MinLength);
			ShuffleElements();
		}

		private void ShuffleElements() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();
			for (int i = Link.MinLength; i < puzzleCells.Length; i++) {
				PuzzleCell currentCell = puzzleCells[i];
				PuzzleCell randomCell = puzzleCells[Random.Range(Link.MinLength, puzzleCells.Length)];

				if (!currentCell.TryGetPuzzleElement(out PuzzleElement currentElement))
					return;

				if (!randomCell.TryGetPuzzleElement(out PuzzleElement randomElement))
					return;

				SwapElements(currentElement, randomElement);
			}
		}

		private void SwapElements(PuzzleElement first, PuzzleElement second) {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			if (!puzzleGrid.TryGetPuzzleCell(first, out PuzzleCell firstCell))
				return;

			if (!puzzleGrid.TryGetPuzzleCell(second, out PuzzleCell secondCell))
				return;

			firstCell.SetPuzzleElement(second);
			secondCell.SetPuzzleElement(first);
		}


		private void MapColorChipCountByDefinition() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int i = 0; i < puzzleCells.Length; i++) {
				if (!puzzleCells[i].TryGetPuzzleElement(out PuzzleElement puzzleElement))
					return;

				if (puzzleElement.GetDefinition() is not ColorChipDefinition definition)
					return;

				if (!elementsByDefinition.TryGetValue(definition, out HashSet<PuzzleElement> elements))
					elementsByDefinition.Add(definition, new HashSet<PuzzleElement>());
				else
					elements.Add(puzzleElement);
			}
		}

		private void CreateLink(int linkLength) {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			if (!TryGetLinkElements(linkLength, out HashSet<PuzzleElement> linkElements))
				return;

			int cellIndex = 0;
			foreach (PuzzleElement linkElement in linkElements) {
				PuzzleCell puzzleCell = puzzleGrid.GetCell(cellIndex);
				if (puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					SwapElements(linkElement, puzzleElement);
				else
					puzzleCell.SetPuzzleElement(linkElement);

				if (cellIndex++ >= linkLength - 1)
					break;
			}
		}

		private bool TryGetLinkElements(int linkLength, out HashSet<PuzzleElement> linkElements) {
			foreach ((ColorChipDefinition definition, HashSet<PuzzleElement> elements) in elementsByDefinition) {
				if (elements.Count < linkLength)
					continue;

				linkElements = elements;
				return true;
			}

			linkElements = null;
			return false;
		}
	}
}
