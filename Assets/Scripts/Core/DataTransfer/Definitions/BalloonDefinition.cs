using Core.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class BalloonDefinition : PuzzleElementDefinition {
		private const string Filename = nameof(BalloonDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Sprite sprite;

		public Sprite GetSprite() => sprite;
		public override PuzzleElement CreateElement() => new Balloon(this);
	}
}
