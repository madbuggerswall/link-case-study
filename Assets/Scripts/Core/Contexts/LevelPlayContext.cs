using Core.DataTransfer.Definitions;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Utilities.Pooling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Contexts {
	public class LevelPlayContext : SceneContext {
		protected override void ResolveContext() {
			Resolve<ObjectPool>();
			Resolve<CameraController>();
			Resolve<PuzzleGridBehaviourFactory>();
			Resolve<PuzzleCellBehaviourFactory>();
			Resolve<PuzzleElementBehaviourFactory>();
			Resolve<PuzzleLevelInitializer>();
		}

		protected override void InitializeContext() {
			Get<ObjectPool>().Initialize();
			Get<CameraController>().Initialize();
			Get<PuzzleGridBehaviourFactory>().Initialize();
			Get<PuzzleCellBehaviourFactory>().Initialize();
			Get<PuzzleElementBehaviourFactory>().Initialize();
			Get<PuzzleLevelInitializer>().Initialize();
		}
	}
}
