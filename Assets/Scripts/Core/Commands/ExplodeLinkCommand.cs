using Core.Links;
using Core.PuzzleLevels;

namespace Core.Commands {
	public class ExplodeLinkCommand : Command {
		private readonly LinkManager linkManager;
		private readonly Link link;

		public ExplodeLinkCommand(LinkManager linkManager, Link link) {
			this.linkManager = linkManager;
			this.link = link;
		}

		// NOTE This should call LinkManager.Explode(Link)
		public override void Execute() {
			// Maybe evaluate here
		}
	}
}