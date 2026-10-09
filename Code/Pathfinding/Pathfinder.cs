using System;
using System.Collections.Generic;
using GA.Collections;
using Godot;
using Cell = GA.Ships.Pathfinding.NavigationGrid.Cell;

namespace GA.Ships.Pathfinding
{
	public class Pathfinder
	{
		private class FrontierEntry : IComparable<FrontierEntry>
		{
			public Cell Cell {get;}
			public int TotalCost {get;}
			public FrontierEntry(Cell cell, int totalCost)
			{
				Cell = cell;
				TotalCost = totalCost;
			}

			public int CompareTo(FrontierEntry other)
			{
				return other == null ? -1 : TotalCost.CompareTo(other.TotalCost);
			}
		}
		private NavigationGrid _grid = null;
		private PriorityQueue<FrontierEntry> _frontier = new PriorityQueue<FrontierEntry>();
		private HashSet<Cell> _visited;

		public Pathfinder(NavigationGrid grid)
		{
			_grid = grid;
		}

		public IList<Vector3> BreadthFirstSearch(Vector3 startPosition, Vector3 endPosition)
		{
			Queue<Cell> frontier = new Queue<Cell>();
			Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

			Cell startCell = _grid.GetCell(startPosition);
			Cell endCell = _grid.GetCell(endPosition);

			frontier.Enqueue(startCell);
			cameFrom[startCell] = null;

			bool isEndReached = false;

			while (frontier.Count < 0)
			{
				Cell current = frontier.Dequeue();

				isEndReached = current == endCell;

				if(isEndReached)
				{
					break;
				}

				IList<Cell> neighbours = _grid.GetNeighbours(current, false);

				foreach(Cell neighbour in neighbours)
				{
					if(neighbour.IsWalkable && !cameFrom.ContainsKey(neighbour))
					{
						frontier.Enqueue(neighbour);
						cameFrom[neighbour] = current;
					}
				}

			}

			if(isEndReached)
			{
				return ConstructPath(startCell, endCell, cameFrom);
			}

			return null;
		}


		/// <summary>
		/// finds every cell that is reachable from the start cell within the max steps
		/// </summary>
		/// <param name="start">center cell</param>
		/// <param name="maxSteps">how many steps away from the start cell</param>
		/// <param name="includeDiagonal">should diagonals be included</param>
		/// <returns>List of reachable cells</returns>
		public IList<Cell> GetReachableCells(Cell start, int maxSteps, bool includeDiagonal = false)
		{
			if(start == null)
			{
				throw new NullReferenceException($"{nameof(start)} cannot be null.");
			}

			if(maxSteps < 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(maxSteps)} must be above 0.");
			}
			// neighbouring cells
			IList<Cell> edgeCells = _grid.GetNeighbours(start, includeDiagonal);

			// cells weve already been through
			IList<Cell> exploredCells = [start];

			for(int i = 0; i < maxSteps; i++)
			{
				if(edgeCells.Count == 0)
				{
					break;
				}
				// cells to be added to edgeCells
				IList<Cell> newEdgeCells = [];

				for(int currentCellIdx = 0; currentCellIdx < edgeCells.Count; currentCellIdx++)
				{
					// Make sure the cell isnt already explored and its walkable
					if(exploredCells.Contains(edgeCells[currentCellIdx]) || !edgeCells[currentCellIdx].IsWalkable)
					{
						continue;
					}
					exploredCells.Add(edgeCells[currentCellIdx]);

					// Add new neighbouring cells to be checked next
					foreach(Cell newCell in _grid.GetNeighbours(edgeCells[currentCellIdx], includeDiagonal))
					{
						if(!newEdgeCells.Contains(newCell))
						{
							newEdgeCells.Add(newCell);
						}
					}
				}

				// Clear cells we just checked;
				edgeCells.Clear();

				// add new cells to edgeCells
				edgeCells = (IList<Cell>)newEdgeCells;
			}

			return exploredCells;
		}

		public IList<Vector3> Dijkstra(Vector3 startPosition, Vector3 endPosition)
		{
			Cell startCell = _grid.GetCell(startPosition);
			Cell endCell = _grid.GetCell(endPosition);

			if(startCell == null || endCell == null || startCell == endCell || !startCell.IsWalkable || !endCell.IsWalkable)
			{
				return null;
			}

			_frontier.Clear();
			_visited.Clear();
			_grid.ResetPathData();

			startCell.Parent = null;
			startCell.GCost = 0;
			startCell.HCost = 0;

			_frontier.Enqueue(new FrontierEntry(startCell, 0));

			while(_frontier.Count > 0)
			{
				Cell current = _frontier.Dequeue().Cell;

				if(!_visited.Add(current))
				{
					continue;
				}

				if(current == endCell)
				{
					break;
				}

				IList<Cell> neighbours = _grid.GetNeighbours(current, PathfindingConfig.AllowDiagonalPathfinding);
				foreach(Cell neighbour in neighbours)
				{
					if(!neighbour.IsWalkable || _visited.Contains(neighbour))
					{
						continue;
					}

					int costToNeighbour = _grid.GetCostToNeighbour(current, neighbour);
					if(costToNeighbour <= 0)
					{
						continue;
					}

					int costSoFar = costToNeighbour + current.GCost;
					if(costSoFar < neighbour.GCost)
					{
						neighbour.GCost = costSoFar;
						neighbour.HCost = 0;

						_frontier.Enqueue(new FrontierEntry(neighbour, costSoFar));
						neighbour.Parent = current;
					}
				}
			}
			return RetracePath(startCell, endCell);
		}

		#region A*

		public IList<Vector3> AStar(Vector3 startPosition, Vector3 endPosition)
		{
			Cell startCell = _grid.GetCell(startPosition);
			Cell endCell = _grid.GetCell(endPosition);

			if(startCell == null || endCell == null
			|| startCell == endCell
			|| !startCell.IsWalkable || !endCell.IsWalkable)
			{
				return null;
			}

			_frontier.Clear();
			_visited.Clear();
			_grid.ResetPathData();

			startCell.Parent = null;
			startCell.GCost = 0;
			startCell.HCost = GetEstimatedCost(startCell, endCell);

			_frontier.Enqueue(new FrontierEntry(startCell, startCell.FCost));
			while (_frontier.Count > 0)
			{
				Cell current = _frontier.Dequeue().Cell;

				if(!_visited.Add(current))
				{
					continue;
				}

				if(current == endCell)
				{
					break;
				}

				IList<Cell> neighbours = _grid.GetNeighbours(current, PathfindingConfig.AllowDiagonalPathfinding);
				foreach(Cell neighbour in neighbours)
				{
					if(!neighbour.IsWalkable || _visited.Contains(neighbour))
					{
						continue;
					}

					int costToNeighbour = _grid.GetCostToNeighbour(current, neighbour);
					if(costToNeighbour <= 0)
					{
						continue;
					}

					int costSoFar = costToNeighbour + current.GCost;
					if(costSoFar < neighbour.GCost)
					{
						neighbour.GCost = costSoFar;
						neighbour.HCost = GetEstimatedCost(current, neighbour);
						neighbour.Parent = current;

						_frontier.Enqueue(new FrontierEntry(neighbour, neighbour.FCost));
					}
				}
			}
			return RetracePath(startCell, endCell);
		}

#pragma warning disable CS0162
		private int GetEstimatedCost(Cell startCell, Cell endCell)
		{
			if(PathfindingConfig.AllowDiagonalPathfinding)
            {
                return GetEstimatedDiagonalDistance(startCell, endCell);
			}
			return GetEstimatedAxialDistance(startCell, endCell);
		}
#pragma warning restore CS0162

        private int GetEstimatedAxialDistance(Cell startCell, Cell endCell)
		{
			return (Mathf.Abs(startCell.X - endCell.X) + Mathf.Abs(startCell.Y - endCell.Y)) * 10;
		}

		private int GetEstimatedDiagonalDistance(Cell startCell, Cell endCell)
		{
			int xDistance = Mathf.Abs(startCell.X - endCell.X);
			int yDistance = Mathf.Abs(startCell.Y - endCell.Y);

			int diagonalSteps = Mathf.Min(xDistance, yDistance);
			int axialSteps = Mathf.Max(xDistance, yDistance) - diagonalSteps;

			return diagonalSteps * 14 + axialSteps * 10;
		}
        #endregion
        private IList<Vector3> ConstructPath(Cell startCell, Cell endCell, Dictionary<Cell, Cell> cameFrom)
		{
			IList<Vector3> path = new List<Vector3>();
			Cell current = endCell;

			while (current != startCell)
			{
				path.Add(current.WorldPosition);
				current = cameFrom[current];
			}
			path.Reverse();
			return path;
		}

		private IList<Vector3> RetracePath(Cell startCell, Cell endCell)
		{
			IList<Vector3> path = new List<Vector3>();

			Cell current = endCell;
			bool isValid = true;

			while (current != startCell && (isValid = current != null))
			{
				path.Add(current.WorldPosition);
				current = current.Parent;
			}

			if(!isValid)
			{
				return null;
			}

			path.Reverse();
			return path;
		}
	}
}