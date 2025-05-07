using Core.PuzzleLevels.Targets;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[System.Serializable]
	public abstract class TargetDTO {
		public abstract Target CreateTarget();
	}
}
