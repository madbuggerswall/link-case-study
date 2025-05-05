using Core.PuzzleLevels;

namespace Core.Commands {
	public class ExplodeLinkCommand : Command {
		private readonly PuzzleLevelManager levelManager;
		private readonly Link link;

		public ExplodeLinkCommand(PuzzleLevelManager levelManager, Link link) {
			this.levelManager = levelManager;
			this.link = link;
		}

		public override void Execute() {
			levelManager.Explode(this, link);
		}
	}
}