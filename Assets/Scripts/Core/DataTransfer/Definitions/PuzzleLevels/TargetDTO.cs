using Core.PuzzleLevels.Targets;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[System.Serializable]
	public abstract class TargetDTO {
		public abstract Target CreateTarget();
	}
}
