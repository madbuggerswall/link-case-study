using Core.Commands;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.Input;
using Core.LinkInput;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using Core.UI;
using Frolics.Pooling;
using Frolics.Signals;

namespace Core.Contexts {
	public class LevelPlayContext : SceneContext {
		protected override void ResolveContext() {
			Resolve<ObjectPool>();
			Resolve<ChipDefinitionManager>();
			Resolve<InputManager>();
			Resolve<CommandInvoker>();
			Resolve<CameraController>();
			Resolve<PuzzleCellBehaviourFactory>();
			Resolve<PuzzleGridBehaviourFactory>();
			Resolve<PuzzleElementBehaviourFactory>();
			Resolve<PuzzleLevelInitializer>();
			Resolve<PuzzleLevelManager>();
			Resolve<LinkInputManager>();
			Resolve<TurnManager>();
			Resolve<TargetManager>();
			Resolve<PuzzleLevelViewController>();
			Resolve<PuzzleLevelUIController>();
		}

		protected override void OnInitialized() {
			SignalBus.GetInstance().Fire(new ContextInitializedSignal());
		}
	}

	public class ContextInitializedSignal : Signal { }
}
