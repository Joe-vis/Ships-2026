using Godot;

namespace GA.Ships.Pathfinding
{
	
	public partial class NavigationObject : Node3D
	{
		[Export] public int MovementCost { get; set; } = 1;
		[Export] public int Priority { get; set; } = 0;

		public bool IsWalkable => MovementCost >= 0;
	}
}