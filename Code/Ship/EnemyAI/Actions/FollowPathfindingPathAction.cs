using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class FollowPathfindingPathAction : AIActionBase
	{
		[Export] private float _arrivalDistance = 1.0f;
		private float _arrivalDistanceSqr = 0;

		private FollowPathfindingPathAction() : base()
		{
			_arrivalDistanceSqr = _arrivalDistance * _arrivalDistance;
		}
		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if (ship == null || ship.Brain == null
				|| ship.PathfindingPath == null || ship.PathfindingPath.Count == 0)
			{
				return;
			}

			if (ship.GlobalPosition.DistanceSquaredTo(ship.CurrentPathfindingTarget) <= _arrivalDistanceSqr)
			{
				ship.AdvancePathfindingPathTarget();
			}
		}
	}
}