using Core.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class CrateDefinition : PuzzleElementDefinition {
		private const string Filename = nameof(CrateDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Sprite sprite;
		[SerializeField] private int durability = 1;

		public override PuzzleElement CreateElement() => new Crate(this);

		public Sprite GetSprite() => sprite;
		public int GetDurability() => durability;
	}
}
