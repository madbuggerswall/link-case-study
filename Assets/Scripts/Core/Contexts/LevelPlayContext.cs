using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Pooling;

namespace Core.Contexts {
	public class LevelPlayContext : SceneContext {
		protected override void ResolveContext() {
			Resolve<ObjectPool>();
			Resolve<CameraController>();
			Resolve<PuzzleCellBehaviourFactory>();
			Resolve<PuzzleGridBehaviourFactory>();
			Resolve<PuzzleElementBehaviourFactory>();
			Resolve<PuzzleLevelInitializer>();
		}

		protected override void InitializeContext() {
			Get<ObjectPool>().Initialize();
			Get<CameraController>().Initialize();
			Get<PuzzleCellBehaviourFactory>().Initialize();
			Get<PuzzleGridBehaviourFactory>().Initialize();
			Get<PuzzleElementBehaviourFactory>().Initialize();
			Get<PuzzleLevelInitializer>().Initialize();
		}
	}
}
