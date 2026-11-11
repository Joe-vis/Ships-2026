using System.Collections.Generic;
using System.IO;
using GA.Common;
using GA.Ships.Pathfinding;
using Godot;

namespace GA.Ships
{
	public partial class EnemyShip : ShipBase
	{
		// EnemyAI system should be stateless. This means, all enemy data
		// should be stored here. States and actions modify that data.
		[Export] private PatrolPath _patrolPath = null;
		#region Pathfinding
		private float _pathfindingTimer = 0;
		private List<Vector3> _pathfindingPath = new List<Vector3>();
		[Export] private PathPostProcessorResource[] _pathPostProcessors = null;
		#endregion

		private ITarget _target;
		public EnemyBrain Brain { get; private set; } = null;
		public PatrolPath PatrolPath => _patrolPath;
		public int CurrentWaypointIndex {get; set; } = 0;
		public bool IsPatrolPathValid => PatrolPath != null && _patrolPath.Length > 0;
		public bool IsPathfindingPathValid => PathfindingPath != null && PathfindingPath.Count > 0;

		public IReadOnlyList<Vector3> PathfindingPath => _pathfindingPath;
		public float PathfindingTimer
		{
			get => _pathfindingTimer;
			set => _pathfindingTimer = value;
		}
		public ITarget Target
		{
			get => _target;
			set
			{
				_target = value;
			}
		}

		public Vector3 CurrentPathfindingTarget
		{
			get
			{
				return IsPathfindingPathValid
					? _pathfindingPath[0]
					: Target != null
						? Target.TargetPosition
						: GlobalPosition;
			}
		}

		public override void _Ready()
		{
			base._Ready();

			Brain = this.GetNode<EnemyBrain>();
			if (Brain != null)
			{
				Brain.Initialize(this);
			}
			else
			{
				GD.PushError("Brain for the enemy ship can't be found!");
			}
		}

		public void SetPatrolPath(PatrolPath patrolPath)
		{
			if(patrolPath != null)
			{
				_patrolPath = patrolPath;
			}
		}

		public void SetPathfindingPath(IList<Vector3> pathfindingPath)
		{
			_pathfindingPath.Clear();
			if(pathfindingPath == null || pathfindingPath.Count <= 0)
			{
				GD.PushWarning("No path provided for the enemy ship");
				return;
			}

			if(_pathPostProcessors != null)
			{
				foreach (var postProcessor in _pathPostProcessors)
				{
					if(postProcessor != null)
					{
						pathfindingPath = postProcessor.PostProcess(pathfindingPath);
					}
				}
			}

			foreach(Vector3 cellPosition in pathfindingPath)
			{
				_pathfindingPath.Add(cellPosition);
			}
		}

		public void AdvancePatrolIndex()
		{
			if(!IsPatrolPathValid)
			{
				return;
			}

			CurrentWaypointIndex = PatrolPath.GetNextIndex(CurrentWaypointIndex);
		}

		public void AdvancePathfindingPathTarget()
		{
			if(_pathfindingPath != null && _pathfindingPath.Count > 0)
			{
				_pathfindingPath.RemoveAt(0);
			}
		}
	}
}