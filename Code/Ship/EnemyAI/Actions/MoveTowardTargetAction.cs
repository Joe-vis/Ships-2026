using Godot;

namespace GA.Ships
{
	[GlobalClass]
	public partial class MoveTowardTargetAction : AIActionBase
	{
		private const float _minDistanceToTarget = 0.5f;
		[Export] private Curve _angleSpeedCurve = null;
		[Export(PropertyHint.Range, "0, 90, 1, radians_as_degrees")]
		private float _maxAngle;
		[Export] private float _breakingDistance = 5.0f;
		private RigidBody3D a;
		private float MinDistanceToTarget
		{
			get => _minDistanceToTarget * _minDistanceToTarget;
		}
		private float BreakingDistanceSqr
		{
			get => _breakingDistance * _breakingDistance;
		}

		public override void Act(EnemyShip ship, float deltaTime = 0)
		{
			if(ship == null || ship.ShipData == null)
			{
				return;
			}
			float angleToTargetPoint = 0;
			Vector3 currentVelocity = ship.Velocity;
			Vector3 xzVelocity = new Vector3(currentVelocity.X, 0, currentVelocity.Z);

			Vector3 forward = ship.GlobalBasis.Z;
			forward.Y = 0;
			forward = forward.Normalized();

			Vector3 toTargetPoint = ship.CurrentPathfindingTarget - ship.GlobalPosition;
			toTargetPoint.Y = 0;
			float sqrDistanceToTarget = toTargetPoint.LengthSquared();

			if(MinDistanceToTarget > sqrDistanceToTarget)
			{
				angleToTargetPoint = forward.AngleTo(toTargetPoint);
			}

			float speedScale = CalculateSpeedScale(angleToTargetPoint, sqrDistanceToTarget);
			Vector3 targetVelocity = forward * (ship.ShipData.MovementSpeed * speedScale);
			float blendSpeed = targetVelocity.Length() < 0.01f ? ship.ShipData.Acceleration : ship.ShipData.Deceleration;

			xzVelocity = xzVelocity.Lerp(targetVelocity, blendSpeed);

			currentVelocity.X = xzVelocity.X;
			currentVelocity.Z = xzVelocity.Z;

			ship.Velocity = currentVelocity;
			ship.MoveAndSlide();
		}

		private float CalculateSpeedScale(float angleToTarget, float sqrDistanceToTarget)
		{
			float absAngle = Mathf.Abs(angleToTarget);
			if(absAngle >= _maxAngle)
			{
				return 0;
			}

			float t = Mathf.Clamp(absAngle / Mathf.Max(_maxAngle, 0.001f), 0.0f, 1.0f);
			float speedMultiplier = _angleSpeedCurve != null
				? _angleSpeedCurve.Sample(t)
				: 1 - t;

			float breakeScale = 1.0f;
			if(BreakingDistanceSqr > 0)
			{
				breakeScale = Mathf.Clamp(sqrDistanceToTarget / BreakingDistanceSqr, 0, 1);
			}

			return Mathf.Clamp(speedMultiplier * breakeScale, 0, 1);
		}
	}
}