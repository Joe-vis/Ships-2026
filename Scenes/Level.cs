using GA.Ships.Pathfinding;
using Godot;
using GA.Common;
using System.IO;

namespace Ga.Ships
{
	public partial class Level : Node3D
	{
		#region Statics
		private static Level _current = null;

		public static Level Current { get {return _current; } }
		#endregion

		[Export] private NavigationGrid _grid = null;


		public Pathfinder Pathfinder
		{
			get; 
			set;
		}

		public Level()
		{
			_current = this;
		}

		public override void _Ready()
		{
			if(_grid == null)
			{
				_grid = this.GetNode<NavigationGrid>(recursive: false);
			}

			Pathfinder = new Pathfinder(_grid);
		}

	}
}