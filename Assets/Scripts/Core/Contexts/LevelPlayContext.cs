using Core.Input;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Pooling;

namespace Core.Contexts {
	public class LevelPlayContext : SceneContext {
		protected override void ResolveContext() {
			Resolve<ObjectPool>();
			Resolve<InputController>();
			Resolve<CameraController>();
			Resolve<PuzzleCellBehaviourFactory>();
			Resolve<PuzzleGridBehaviourFactory>();
			Resolve<PuzzleElementBehaviourFactory>();
			Resolve<PuzzleLevelInitializer>();
			Resolve<PuzzleCellDragHelper>();
			Resolve<LinkInputManager>();
		}

		protected override void InitializeContext() {
			Get<ObjectPool>().Initialize();
			Get<InputController>().Initialize();
			Get<CameraController>().Initialize();
			Get<PuzzleCellBehaviourFactory>().Initialize();
			Get<PuzzleGridBehaviourFactory>().Initialize();
			Get<PuzzleElementBehaviourFactory>().Initialize();
			Get<PuzzleLevelInitializer>().Initialize();
			Get<PuzzleCellDragHelper>().Initialize();
			Get<LinkInputManager>().Initialize();
		}
	}
}
