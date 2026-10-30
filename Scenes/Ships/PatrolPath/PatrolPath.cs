using System.Collections.Generic;
using Godot;
using GA.Common;
using System;

namespace GA.Ships
{
    public partial class PatrolPath : Node3D
    {
        private IList<Waypoint> _path = null;

        public int Length => _path?.Count ?? 0;
        private void Initialize()
        {
            _path = this.GetChildren<Waypoint>(recursive: false);
        }

        public override void _Ready()
        {
            Initialize();
        }

        public Waypoint GetWaypoint(int index)
        {
            if(index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException($"Invalid waypoint index {index} for patrol route with length of {Length}");
            }

            return _path[index];
        }

        public int GetClosestWaypointIndex(Vector3 position)
        {
            if(_path == null || Length == 0)
            {
                return -1;
            }

            Waypoint closest = null;
            float nearestSqrDistance = float.PositiveInfinity;
            foreach(Waypoint waypoint in _path)
            {
                Vector3 toTarget = waypoint.TargetPosition - position;
                float sqrDistance = toTarget.LengthSquared();
                if(sqrDistance < nearestSqrDistance)
                {
                    closest = waypoint;
                    nearestSqrDistance = sqrDistance;
                }
            }

            return _path.IndexOf(closest);
        }

        public int GetNextIndex(int currentIndex)
        {
            if(_path == null || Length == 0)
            {
                throw new InvalidOperationException("Patrol route has no points");
            }
            return (currentIndex + 1) % Length;
        }

    }
}