using Core.DataTransfer.Definitions;

namespace Core.PuzzleElements {
	public abstract class PuzzleElement {
		private readonly PuzzleElementDefinition definition;

		protected PuzzleElement(PuzzleElementDefinition definition) {
			this.definition = definition;
		}

		public abstract void Explode();
		public abstract void OnAdjacentExplode();

		public PuzzleElementDefinition GetDefinition() => definition;
	}

	public class ColorChip : PuzzleElement {
		public ColorChip(ColorChipDefinition definition) : base(definition) { }

		public override void Explode() {
			throw new System.NotImplementedException();
		}

		public override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Balloon : PuzzleElement {
		public Balloon(BalloonDefinition definition) : base(definition) { }

		public override void Explode() {
			throw new System.NotImplementedException();
		}

		public override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Duck : PuzzleElement {
		public Duck(DuckDefinition definition) : base(definition) { }

		public override void Explode() {
			throw new System.NotImplementedException();
		}

		public override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Rocket : PuzzleElement {
		public Rocket(RocketDefinition definition) : base(definition) { }

		public override void Explode() {
			throw new System.NotImplementedException();
		}

		public override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}
}
