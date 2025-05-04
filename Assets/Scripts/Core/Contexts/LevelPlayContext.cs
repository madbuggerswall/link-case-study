using Core.Commands;
using Core.DataTransfer.Definitions;
using Core.Input;
using Core.LinkInput;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using Frolics.Pooling;

namespace Core.Contexts {
	public class LevelPlayContext : SceneContext {
		protected override void ResolveContext() {
			Resolve<ObjectPool>();
			Resolve<ChipDefinitionManager>();
			Resolve<InputController>();
			Resolve<CommandInvoker>();
			Resolve<CameraController>();
			Resolve<PuzzleCellBehaviourFactory>();
			Resolve<PuzzleGridBehaviourFactory>();
			Resolve<PuzzleElementBehaviourFactory>();
			Resolve<PuzzleLevelInitializer>();
			Resolve<PuzzleCellDragHelper>();
			Resolve<LinkInputManager>();
			Resolve<PuzzleLevelViewController>();
		}

		protected override void InitializeContext() {
			Get<ObjectPool>().Initialize();
			Get<ChipDefinitionManager>().Initialize();
			Get<InputController>().Initialize();
			Get<CommandInvoker>().Initialize();
			Get<CameraController>().Initialize();
			Get<PuzzleCellBehaviourFactory>().Initialize();
			Get<PuzzleGridBehaviourFactory>().Initialize();
			Get<PuzzleElementBehaviourFactory>().Initialize();
			Get<PuzzleLevelInitializer>().Initialize();
			Get<PuzzleCellDragHelper>().Initialize();
			Get<LinkInputManager>().Initialize();
			Get<PuzzleLevelViewController>().Initialize();
		}
	}
}
