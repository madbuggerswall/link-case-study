using System;

namespace Core.Commands {
	public abstract class Command {
		private Action<Command> onComplete = delegate { };
		public abstract void Execute();

		public void SubscribeToOnComplete(Action<Command> onComplete) {
			this.onComplete += onComplete;
		}
		public void UnSubscribeFromOnComplete(Action<Command> onComplete) {
			this.onComplete -= onComplete;
		}

		public void InvokeOnComplete() {
			onComplete(this);
		}
	}
}