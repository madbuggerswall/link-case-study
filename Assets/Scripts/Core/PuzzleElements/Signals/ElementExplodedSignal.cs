using Frolics.Signals;

namespace Core.PuzzleElements.Signals {
	public class ElementExplodedSignal : Signal {
		public PuzzleElement PuzzleElement { get; }

		public ElementExplodedSignal(PuzzleElement puzzleElement) {
			this.PuzzleElement = puzzleElement;
		}
	}
}
