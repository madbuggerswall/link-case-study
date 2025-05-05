using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;

namespace Core.PuzzleLevels {
	public class LinkFinder {
		private readonly PuzzleGrid puzzleGrid;
		private readonly Dictionary<PuzzleElement, Link> linksByElement;

		public LinkFinder(PuzzleGrid puzzleGrid) {
			this.puzzleGrid = puzzleGrid;
			this.linksByElement = new Dictionary<PuzzleElement, Link>();
		}

		public bool TryFindLinks(out Dictionary<PuzzleElement, Link> linksByElement) {
			MapLinksByPuzzleElements();
			RemoveInvalidLinks();

			linksByElement = this.linksByElement;
			return linksByElement.Count != 0;
		}

		private void RemoveInvalidLinks() {
			foreach ((PuzzleElement puzzleElement, Link link) in linksByElement)
				if (!link.IsValid(puzzleGrid))
					linksByElement.Remove(puzzleElement);
		}

		private void MapLinksByPuzzleElements() {
			PuzzleCell[] cells = puzzleGrid.GetCells();
			linksByElement.Clear();

			for (int i = 0; i < cells.Length; i++)
				if (cells[i].TryGetPuzzleElement(out PuzzleElement puzzleElement))
					ProcessNeighborCells(cells[i], puzzleElement);
		}

		private void ProcessNeighborCells(PuzzleCell currentCell, PuzzleElement currentItem) {
			PuzzleCell[] neighborCells = puzzleGrid.GetNeighbors(currentCell);

			for (int i = 0; i < neighborCells.Length; i++) {
				PuzzleCell neighborCell = neighborCells[i];
				if (!neighborCell.TryGetPuzzleElement(out PuzzleElement neighborElement))
					return;

				if (currentItem.GetDefinition() == neighborElement.GetDefinition())
					ExtendLink(currentItem, neighborElement);
			}
		}

		private void ExtendLink(PuzzleElement currentItem, PuzzleElement neighborItem) {
			if (linksByElement.TryGetValue(currentItem, out Link formerLink)) {
				if (formerLink.TryAdd(neighborItem))
					linksByElement.Add(neighborItem, formerLink);
			} else {
				Link link = new(currentItem, neighborItem);
				linksByElement.Add(currentItem, link);
				linksByElement.Add(neighborItem, link);
			}
		}
	}
}
