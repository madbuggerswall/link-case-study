using System;

namespace Core.Commands {
	public abstract class Command {
		private Action<Command> completionHandlers = delegate { };

		public abstract void Execute();

		public void RegisterCompletionHandler(Action<Command> handler) {
			this.completionHandlers += handler;
		}

		public void DeregisterCompletionHandler(Action<Command> handler) {
			this.completionHandlers -= handler;
		}

		public void InvokeCompletionHandlers() {
			completionHandlers(this);
		}
	}
}
