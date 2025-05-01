using System.Collections;
using System.Collections.Generic;
using Core.PuzzleElements;

public class PuzzleMatch : IEnumerable<PuzzleElement> {
	private readonly HashSet<PuzzleElement> puzzleElements;

	public PuzzleMatch() {
		puzzleElements = new HashSet<PuzzleElement>();
	}

	public PuzzleMatch(PuzzleElement current, PuzzleElement neighbor) {
		puzzleElements = new HashSet<PuzzleElement> { current, neighbor };
	}

	public bool Add(PuzzleElement puzzleElement) {
		return puzzleElements.Add(puzzleElement);
	}

	// Make it iterable
	public IEnumerator<PuzzleElement> GetEnumerator() => puzzleElements.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
