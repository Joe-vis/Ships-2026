using GA.Ships.Pathfinding;
using Godot;

namespace Ga.Ships
{
	public partial class Level : Node3D
	{
		[Export] private NavigationGrid _grid = null;


		public Pathfinder Pathfinder
		{
			get; 
			set;
		}


	}
}