using System.Collections.Generic;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

public class MatchManager {
	private readonly PuzzleGrid puzzleGrid;
	private readonly Dictionary<PuzzleElement, PuzzleMatch> matchesByItem;

	public MatchManager(PuzzleGrid puzzleGrid) {
		this.puzzleGrid = puzzleGrid;

		int maxMatchCount = GetMaxMatchCount(puzzleGrid);
		this.matchesByItem = new Dictionary<PuzzleElement, PuzzleMatch>(maxMatchCount);
	}

	private void FindMatches() {
		PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

		for (int i = 0; i < puzzleCells.Length; i++) {
			PuzzleElement currentItem = puzzleCells[i].GetPuzzleElement();
			PuzzleCell[] neighborCells = puzzleGrid.GetNeighbors(puzzleCells[i]);

			for (int j = 0; j < neighborCells.Length; j++) {
				PuzzleElement neighborItem = neighborCells[j].GetPuzzleElement();
				if (currentItem.GetDefinition() != neighborItem.GetDefinition())
					continue;

				if (matchesByItem.TryGetValue(currentItem, out PuzzleMatch formerMatch)) {
					if (formerMatch.Add(neighborItem))
						matchesByItem.Add(neighborItem, formerMatch);
				} else {
					PuzzleMatch puzzleMatch = new(currentItem, neighborItem);
					matchesByItem.Add(currentItem, puzzleMatch);
					matchesByItem.Add(neighborItem, puzzleMatch);
				}
			}
		}
	}

	private static int GetMaxMatchCount(PuzzleGrid puzzleGrid) {
		Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
		int cellCount = gridSize.x * gridSize.y;
		int maxMatchCount = cellCount / 2;

		return maxMatchCount;
	}
}
