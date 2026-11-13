using GA.Ships.Pathfinding;
using Godot;
using GA.Common;
using System.IO;
using System.Collections.Generic;
using GA.Ships.Navigation;

namespace GA.Ships
{
	public partial class Level : Node3D
	{
		#region Statics
		private static Level _current = null;

		public static Level Current { get {return _current; } }
		#endregion

		[Export] private NavigationGrid _grid = null;
		private PlayerShip _player;
		private PathDebugDrawer _pathVisualizer;
		public NavigationGrid Grid => _grid;
		public PlayerShip Player => _player;

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

			_pathVisualizer ??= this.GetNode<PathDebugDrawer>();

			Pathfinder = new Pathfinder(_grid);
		}

		public void DrawPath(IList<Vector3> path)
		{
			if(_pathVisualizer != null)
			{
				_pathVisualizer.DrawPath(path);
			}
		}

		public void RegisterPlayer(PlayerShip player)
		{
			_player = player;
		}
	}
}