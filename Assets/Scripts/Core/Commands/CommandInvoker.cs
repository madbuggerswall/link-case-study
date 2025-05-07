using System.Collections.Generic;
using Core.Contexts;

namespace Core.Commands {
	public class CommandInvoker : IInitializable {
		private readonly Queue<Command> commands = new();
		private bool isRunning = false;

		public void Initialize() { }

		public void Enqueue(Command command) {
			if (isRunning)
				return;

			commands.Enqueue(command);
			TryExecuteNextCommand();
		}

		private void TryExecuteNextCommand() {
			if (isRunning || commands.Count == 0)
				return;

			isRunning = true;

			Command currentCommand = commands.Dequeue();
			currentCommand.RegisterCompletionHandler(OnCommandCompleted);
			currentCommand.Execute();
		}

		private void OnCommandCompleted(Command command) {
			isRunning = false;
			command.DeregisterCompletionHandler(OnCommandCompleted);
			commands.Clear();
		}
	}
}
