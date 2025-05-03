using Core.Links;

namespace Core.Commands {
	public class ExplodeLinkCommand : Command {
		private readonly Link link;

		public ExplodeLinkCommand(Link link) {
			this.link = link;
		}

		public override void Execute() {
			link.Explode();
		}
	}
}