using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.Links {
	// NOTE For hints (HintManager)
	public class LinkFinder {
		private readonly PuzzleGrid puzzleGrid;
		private readonly Dictionary<PuzzleElement, Link> linksByItem;

		public LinkFinder(PuzzleGrid puzzleGrid) {
			this.puzzleGrid = puzzleGrid;

			int maxMatchCount = GetMaxMatchCount(puzzleGrid);
			this.linksByItem = new Dictionary<PuzzleElement, Link>(maxMatchCount);
		}

		private void MapLinksByPuzzleElements() {
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();
			linksByItem.Clear();
			
			for (int i = 0; i < puzzleCells.Length; i++) {
				PuzzleCell cell = puzzleCells[i];
				if (cell.IsEmpty())
					continue;

				ProcessNeighborCells(cell);
			}
		}

		private void ProcessNeighborCells(PuzzleCell cell) {
			PuzzleElement currentItem = cell.GetPuzzleElement();
			PuzzleCell[] neighborCells = puzzleGrid.GetNeighbors(cell);

			for (int i = 0; i < neighborCells.Length; i++) {
				PuzzleCell neighborCell = neighborCells[i];
				if (neighborCell.IsEmpty())
					continue;

				PuzzleElement neighborElement = neighborCell.GetPuzzleElement();
				if (currentItem.GetDefinition() == neighborElement.GetDefinition())
					ExtendLink(currentItem, neighborElement);
			}
		}

		private void ExtendLink(PuzzleElement currentItem, PuzzleElement neighborItem) {
			if (linksByItem.TryGetValue(currentItem, out Link formerLink)) {
				if (formerLink.TryAdd(neighborItem))
					linksByItem.Add(neighborItem, formerLink);
			} else {
				Link link = new(currentItem, neighborItem);
				linksByItem.Add(currentItem, link);
				linksByItem.Add(neighborItem, link);
			}
		}

		private static int GetMaxMatchCount(PuzzleGrid puzzleGrid) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			int cellCount = gridSize.x * gridSize.y;
			int maxMatchCount = cellCount / 2;

			return maxMatchCount;
		}
	}
}
