using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;

namespace Core.PuzzleLevels {
	public class LinkFinder {
		private readonly PuzzleGrid puzzleGrid;
		private readonly HashSet<Link> links;

		public LinkFinder(PuzzleGrid puzzleGrid) {
			this.puzzleGrid = puzzleGrid;
			this.links = new HashSet<Link>();
		}

		public bool TryFindLinks(out HashSet<Link> links) {
			TraverseForLinks();
			links = this.links;
			return links.Count != 0;
		}

		private void TraverseForLinks() {
			PuzzleCell[] cells = puzzleGrid.GetCells();
			links.Clear();

			for (int i = 0; i < cells.Length; i++) {
				if (!cells[i].TryGetPuzzleElement(out PuzzleElement puzzleElement))
					continue;

				HashList<PuzzleElement> matchingNeighbors = GetMatchingNeighbors(cells[i], puzzleElement);
				if (matchingNeighbors.Count < 3)
					continue;

				Link link = new(matchingNeighbors);
				links.Add(link);
			}
		}

		private HashList<PuzzleElement> GetMatchingNeighbors(PuzzleCell currentCell, PuzzleElement currentItem) {
			PuzzleCell[] neighborCells = puzzleGrid.GetNeighbors(currentCell);
			HashList<PuzzleElement> matchingNeighbors = new();
			matchingNeighbors.TryAdd(currentItem);

			for (int i = 0; i < neighborCells.Length; i++) {
				if (!neighborCells[i].TryGetPuzzleElement(out PuzzleElement neighborElement))
					continue;

				if (currentItem.GetDefinition() == neighborElement.GetDefinition())
					matchingNeighbors.TryAdd(neighborElement);
			}

			return matchingNeighbors;
		}
	}
}
