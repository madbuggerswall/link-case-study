using Core.Links;

namespace Core.Commands {
	public class ExplodeLinkCommand : Command {
		private readonly Link link;

		public ExplodeLinkCommand(Link link) {
			this.link = link;
		}

		// NOTE This should call LinkManager.Explode(Link)
		public override void Execute() {
			link.Explode();
		}
	}
}