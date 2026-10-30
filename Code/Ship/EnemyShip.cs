using System.Collections.Generic;
using System.IO;
using GA.Common;
using Godot;

namespace GA.Ships
{
	public partial class EnemyShip : ShipBase
	{
		// EnemyAI system should be stateless. This means, all enemy data
		// should be stored here. States and actions modify that data.

		public EnemyBrain Brain { get; private set; } = null;
		public PatrolPath PatrolPath { get; private set; } = null;
		public int CurrentWaypointIndex {get; set; } = 0;
		public bool IsPathValid => PatrolPath != null;
		public Waypoint Target {get; private set;}
        public IList<Waypoint> PathfindingPath => null;

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
				PatrolPath = patrolPath;
			}
		}

		public void AdvancePatrolIndex()
		{
			if(!IsPathValid)
			{
				return;
			}

			CurrentWaypointIndex = PatrolPath.GetNextIndex(CurrentWaypointIndex);
		}
	}
}